using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using FastColoredTextBoxNS;

using static MScripter.Program;

namespace MScripter {
    public partial class MainForm : Form {
        private StringBuilder buffer;

        private List<MslDocument> documents;
        private List<Hashtable> hashtables;
        private List<MslDialog> dialogs;

        private object currentItem;

        private IntPtr selectedWindow;

        private bool acceptingConsoleInput;
        private bool surpressSelectionEvents;

        private int hashtableEditState = -1;

        private Thread consoleThread;

        private int cursorCharType;

        public Style wordHighlightStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(96, Color.Gray)));
        public Style searchResultStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(96, Color.Yellow)));

        public MainForm(IEnumerable<string> filePaths, Exception configFailure) {
            InitializeComponent();

            documents = new List<MslDocument>();
            hashtables = new List<Hashtable>();
            dialogs = new List<MslDialog>();
            tabControl.TabPages.Clear();

            StringBuilder errorBuilder = null;
            MslDocument configDocument = null;

            if (configFailure != null) {
                // Open the config file if it could not be loaded.
                configDocument = MslDocument.FromFile(Program.ConfigFileDefault, DocumentType.Text);

                if (errorBuilder == null) errorBuilder = new StringBuilder();
                errorBuilder.Append("; Failed to load the configuration file:\n");
                errorBuilder.Append(";   ");
                errorBuilder.Append(configFailure.GetType().FullName);
                errorBuilder.Append(": ");
                errorBuilder.Append(configFailure.Message);
                errorBuilder.Append("\n;   The file has been opened for editing. After editing it, restart MScripter.\n");
            }

            foreach (string path in filePaths) {
                if (!File.Exists(path)) {
                    // Any error messages that occur opening the documents go to a new document.
                    if (errorBuilder == null) errorBuilder = new StringBuilder();
                    errorBuilder.Append("; Failed to open ");
                    errorBuilder.Append(path);
                    errorBuilder.Append(": \n;   File not found.\n");
                    continue;
                }

                try {
                    OpenFile(path, false, IntPtr.Zero);
                } catch (IOException ex) {
                    if (errorBuilder == null) errorBuilder = new StringBuilder();
                    errorBuilder.Append("; Failed to open ");
                    errorBuilder.Append(path);
                    errorBuilder.Append(": \n;   ");
                    errorBuilder.Append(ex.Message);
                    errorBuilder.Append("\n");
                }
            }

            if (errorBuilder != null) {
                MslDocument document = new MslDocument(DocumentType.Text, "errors.txt", false, false, IntPtr.Zero, false, errorBuilder.ToString());
                TabPage tabPage = newTabDocument(document);
                documents.Add(document);
                FormatFile(document);
                addTabPage(tabPage);
                tabControl.SelectTab(tabPage);

                if (configDocument != null) {
                    tabPage = newTabDocument(configDocument);
                    documents.Add(configDocument);
                    addTabPage(tabPage);
                }
            } else if (documents.Count == 0) {
                // No paths given; open a blank document.
                MslDocument document = new MslDocument(DocumentType.RemoteScript, "script.mrc", false, false);
                TabPage tabPage = newTabDocument(document);
                documents.Add(document);
                FormatFile(document);
                addTabPage(tabPage);
            }

            consoleTextBox.Tag = new MslDocument(DocumentType.ConsoleInput);

            applyTheme();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            Program.ApplyConfig(Program.Config);

            this.currentItem = documents[0];

            buffer = new StringBuilder(6);

            if (!Program.Config.NoSync)
                refreshToolStripButton_Click(sender, EventArgs.Empty);

            consoleTextBox.Write("Select an mIRC instance and type a command here to execute it.\n");
            consoleTextBox.Write("Type ");
            consoleTextBox.Write("?", MslSyntaxHighlighter.KeywordStyle);
            consoleTextBox.Write(" ");
            consoleTextBox.Write("expression", MslSyntaxHighlighter.FunctionStyle);
            consoleTextBox.Write(" to evaluate it.\n");
            consoleTextBox.Write("> ");
            acceptingConsoleInput = true;
            consoleTextBox.AcceptLine();
        }

        private async void refreshToolStripButton_Click(object sender, EventArgs e) {
#if (!MONO)
            refreshToken?.Cancel();
            refreshToken = new CancellationTokenSource();
            await refreshSyncList(refreshToken.Token);
#endif
        }

#if (!MONO)
        private CancellationTokenSource refreshToken;
        private async Task refreshSyncList(CancellationToken token) {
            // Remove all existing mIRC nodes.
            while (treeView.Nodes.Count != 0 && treeView.Nodes[0].Tag is Tuple<IntPtr, uint>) {
                treeView.Nodes.RemoveAt(0);
            }

            // First, search for mIRC windows.
            Program.EnumWindows(this.EnumWindowsCallback, IntPtr.Zero);

            // Then, get scripts.
            for (int i = 0; i < treeView.Nodes.Count; ++i) {
                var node = treeView.Nodes[i];
                if (treeView.Nodes[i].ImageIndex != 0) break;
                token.ThrowIfCancellationRequested();


                var tag = (Tuple<IntPtr, uint>) node.Tag;
                await GetScripts(node, tag.Item1, tag.Item2, token);
            }
        }

        private bool EnumWindowsCallback(IntPtr hwnd, IntPtr lParam) {
            int n = Program.GetClassName(hwnd, buffer, 6);
            if (n == 4 && buffer.ToString() == "mIRC") {
                // This is an mIRC window; add it.
                uint pid;
                Program.GetWindowThreadProcessId(hwnd, out pid);

                TreeNode node = new TreeNode("mIRC (PID " + pid + ")", 0, 0);
                node.Tag = new Tuple<IntPtr, uint>(hwnd, pid);

                int i;
                for (i = 0; i < treeView.Nodes.Count; ++i)
                    if (treeView.Nodes[i].ImageIndex != 0) break;
                treeView.Nodes.Insert(i, node);
            }
            return true;
        }

        private async Task GetScripts(TreeNode node, IntPtr window, uint pid, CancellationToken token) {
            string title = node.Text; string[] remotes; string[] aliases; string[] hashtables;

            try {
                using (var context = new MircMessenger(window)) {
                    title = await context.EvaluateAsync(Program.Config.mIRCInstanceLabel.Replace("$pid", pid.ToString()), token);  // $pid isn't a real mSL function.

                    // Read script paths.
                    int remoteCount = int.Parse(await context.EvaluateAsync("$script(0)", token));
                    remotes = new string[remoteCount];
                    for (int i = 1; i <= remoteCount; ++i) {
                        remotes[i - 1] = await context.EvaluateAsync("$script(" + i + ")", token);
                    }

                    int aliasCount = int.Parse(await context.EvaluateAsync("$alias(0)", token));
                    aliases = new string[aliasCount];
                    for (int i = 1; i <= aliasCount; ++i) {
                        aliases[i - 1] = await context.EvaluateAsync("$alias(" + i + ")", token);
                    }

                    // Read hashtables.
                    int hashtableCount = int.Parse(await context.EvaluateAsync("$hget(0)", token));
                    hashtables = new string[hashtableCount];
                    for (int i = 1; i <= hashtableCount; ++i) {
                        hashtables[i - 1] = await context.EvaluateAsync("$hget(" + i + ")", token);
                    }

                    // Update the tree node.
                    node.Text = title;
                    node.Nodes.Clear();

                    TreeNode remotesNode = new TreeNode("Remote scripts", 1, 1) { Name = "Remotes" };
                    node.Nodes.Add(remotesNode);
                    foreach (string file in remotes)
                        remotesNode.Nodes.Add(new TreeNode(Path.GetFileName(file), 9, 9) { Tag = file, ToolTipText = file });

                    TreeNode aliasesNode = new TreeNode("Alias scripts", 2, 2) { Name = "Aliases" };
                    node.Nodes.Add(aliasesNode);
                    foreach (string file in aliases)
                        aliasesNode.Nodes.Add(new TreeNode(Path.GetFileName(file), 10, 10) { Tag = file, ToolTipText = file });

                    TreeNode hashtablesNode = new TreeNode("Hashtables", 6, 6) { Name = "Hashtables" };
                    node.Nodes.Add(hashtablesNode);
                    foreach (string hashtable in hashtables)
                        hashtablesNode.Nodes.Add(new TreeNode(hashtable, 14, 14) { Tag = hashtable });
                }
            } catch (OperationCanceledException) {
                node.Text = title + " [cancelled]";
                node.Nodes.Clear();
            } catch (MircException ex) {
                if (ex.ErrorCode.HasFlag(MircErrorCode.Disabled))
                    node.Text = title + " [disabled]";
                else
                    node.Text = title + " [error]";
                node.Nodes.Clear();
            }
        }
#endif
			
        private async void consoleTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (consoleTextBox.Tag == null) return;  // Form has not yet finished initialising.

            if (acceptingConsoleInput)
                mSLSyntaxHighlight((FastColoredTextBox) sender, e.ChangedRange, DocumentType.ConsoleInput);

            if (acceptingConsoleInput && !consoleTextBox.IsReadLineMode) {
                try {
                    acceptingConsoleInput = false;  // This flag prevents infinite recursion.

                    // Process the line.
                    if (selectedWindow == IntPtr.Zero) {
                        consoleTextBox.Write("But there was no target...\n");
                        consoleTextBox.Write("> ");
                        acceptingConsoleInput = true;
                        consoleTextBox.AcceptLine();
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(consoleTextBox.ReadText)) {
                        consoleTextBox.Write("> ");
                        acceptingConsoleInput = true;
                        consoleTextBox.AcceptLine();
                        return;
                    }
                    string text = consoleTextBox.ReadText.Trim();

                    using (var context = new MircMessenger(selectedWindow)) {
                        var tokenSource = new CancellationTokenSource(10000);

                        try {
                            if (text[0] == '?') {
                                text = text.Substring(1).TrimStart();
                                var result = await context.EvaluateAsync(text, tokenSource.Token);

                                consoleTextBox.Write(result);
                                consoleTextBox.Write(Environment.NewLine);
                            } else {
                                await context.RunAsync(text, tokenSource.Token);
                            }
                        } catch (MircException ex) {
                            consoleTextBox.Write("mIRC returned an error: " + ex.ErrorCode);
                            consoleTextBox.Write(Environment.NewLine);
                        } catch (TaskCanceledException) {
                            consoleTextBox.Write("The request timed out. Perhaps mIRC crashed, or you encountered an infinite loop. Pressing Ctrl+Break in mIRC may stop the script.");
                            consoleTextBox.Write(Environment.NewLine);
                        }
                    }

                    consoleTextBox.Write("> ");
                    acceptingConsoleInput = true;
                    consoleTextBox.AcceptLine();
                } finally {
                    consoleTextBox.ClearUndo();
                }
            }
        }

        private TabPage newTabDocument(MslDocument document) {
            int imageIndex;
            switch (document.Type) {
                case DocumentType.RemoteScript: imageIndex = 0; break;
                case DocumentType.AliasScript: imageIndex = 1; break;
                case DocumentType.Popup: imageIndex = 2; break;
                default: imageIndex = 3; break;
            }

            TabPage tab = new TabPage(Path.GetFileName(document.Path)) { Tag = document, ImageIndex = imageIndex, ToolTipText = document.Path };
            FastColoredTextBox textBox;
            textBox = document.textBoxes[0];

            if (textBox.Parent?.Parent?.Parent != tab) {
                setUpTextBox(textBox);

                var splitContainer = new SplitContainer() {
                    BackColor = Color.Silver,
                    Dock = DockStyle.Fill,
                    Orientation = Orientation.Horizontal,
                    Panel2Collapsed = true,
                    Tag = document
                };
                splitContainer.Panel1MinSize = 0;
                splitContainer.Panel2MinSize = 0;
                splitContainer.SplitterMoved += splitContainer_SplitterMoved;

                textBox.ClearUndo();

                splitContainer.Parent = tab;
                textBox.Parent = splitContainer.Panel1;
            }

            return tab;
        }

        private FastColoredTextBox createTextBox(MslDocument document) {
            var textBox = new FastColoredTextBox() { Tag = document };
            setUpTextBox(textBox);
            return textBox;
        }

        private void setUpTextBox(FastColoredTextBox textBox) {
            var document = (MslDocument) textBox.Tag;

            textBox.AccessibleRole = AccessibleRole.Text;
            textBox.AutoCompleteBracketsList = new char[] { '(', ')', '{', '}', '[', ']', '\"', '\"', '\'', '\'' };
            textBox.AutoScrollMinSize = new Size(179, 90);
            textBox.BackBrush = null;
            textBox.ChangedLineColor = Color.DarkBlue;
            textBox.CharHeight = 15;
            textBox.CharWidth = 7;
            textBox.ContextMenuStrip = documentContextMenu;
            textBox.Cursor = Cursors.IBeam;
            textBox.DisabledColor = Color.FromArgb(100, 180, 180, 180);
            textBox.Dock = DockStyle.Fill;
            textBox.Font = new Font("Consolas", 9.75F);
            textBox.ForeColor = SystemColors.WindowText;
            textBox.IsReplaceMode = false;
            textBox.Location = new Point(0, 44);
            textBox.Name = "MslTextBox";
            textBox.Paddings = new Padding(0);
            textBox.SelectionColor = Color.FromArgb(60, 0, 0, 255);
            textBox.ShowFoldingLines = true;
            textBox.SelectionChangedDelayed += MslTextBox_SelectionChangedDelayed;
            textBox.Size = new Size(590, 301);
            textBox.TabIndex = 1;
            textBox.TabLength = 2;
            textBox.TextChanged += MslTextBox_TextChanged;
            textBox.ToolTipNeeded += MslTextBox_ToolTipNeeded;
            textBox.LineInserted += MslTextBox_LineInserted;
            textBox.LineRemoved += MslTextBox_LineRemoved;
            textBox.KeyDown += MslTextBox_KeyDown;

            textBox.AddStyle(ControlCharStyle.Instance);
            textBox.AddStyle(wordHighlightStyle);
            textBox.AddStyle(searchResultStyle);
            textBox.AddStyle(MslSyntaxHighlighter.CommentStyle);
            textBox.AddStyle(MslSyntaxHighlighter.KeywordStyle);
            textBox.AddStyle(MslSyntaxHighlighter.CommandStyle);
            textBox.AddStyle(MslSyntaxHighlighter.CustomCommandStyle);
            textBox.AddStyle(MslSyntaxHighlighter.FunctionStyle);
            textBox.AddStyle(MslSyntaxHighlighter.CustomFunctionStyle);
            textBox.AddStyle(MslSyntaxHighlighter.FunctionPropertyStyle);
            textBox.AddStyle(MslSyntaxHighlighter.VariableStyle);
            textBox.AddStyle(MslSyntaxHighlighter.AliasStyle);
            textBox.AddStyle(MslSyntaxHighlighter.ErrorStyle);
            textBox.AddStyle(MslSyntaxHighlighter.WarningStyle);
            textBox.AddStyle(MslSyntaxHighlighter.NoticeStyle);

            applyTheme(textBox);

            var popupMenu = new AutocompleteMenu(textBox);
            document.autoCompleteMenu = popupMenu;
            document.autoCompleteMenu.BackColor = Color.Black;
            document.autoCompleteMenu.ForeColor = Color.White;
            popupMenu.AppearInterval = int.MaxValue;  // So the menu will not appear automatically.
            popupMenu.MinFragmentLength = 0;
            popupMenu.SearchPattern = @"[^:\n]";
            popupMenu.AllowTabKey = true;
            popupMenu.Items.MaximumSize = new Size(200, 300);
            popupMenu.Items.Width = 200;
            popupMenu.Items.SetAutocompleteItems(MslSyntaxHighlighter.events.Select(
                ev => new AutocompleteItem(ev.Key, -1, ev.Key, ev.Key, ev.Value.Description)
            ));
        }

        private TabPage newTabDialog(MslDialog dialog) {
            TabPage tab = new TabPage(dialog.Name) {
                Tag = dialog, ImageIndex = 9,
                ToolTipText = dialog.Document?.Path
            };
            DialogDesigner designer = new DialogDesigner(dialog);

            designer.Dock = DockStyle.Fill;
            designer.TabIndex = 0;
            designer.ViewCode += dialogDesigner_ViewCode;
            designer.Theme = Program.Config.Theme;

            tab.Controls.Add(designer);

            dialog.designer = designer;
            return tab;
        }

        public void showHashtable(TabPage tabPage) {
            refreshHashtable(tabPage);
        }

        private void MslTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (currentItem is MslDocument && e.KeyData == (Keys.Space | Keys.Control)) {
                ((MslDocument) currentItem).autoCompleteMenu.Show(true);
                e.Handled = true;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            saveAs(tabControl.SelectedTab);
        }
        private void saveAs(TabPage tabPage) {
            if (tabPage.ImageIndex == 7)
                saveHashtableAs(tabPage);
            else
                saveDocumentAs(tabPage);
        }
        private void saveDocumentAs(TabPage tabPage) {
            MslDocument document = (MslDocument) tabPage.Tag;

            // Show the dialog box.
            saveDialog.Filter = "mIRC remote script files (*.mrc)|*.mrc|mIRC alias script files (*.als)|*.als|INI files (*.ini)|*.ini|All files|*";
            if (document.IsINI) saveDialog.FilterIndex = 3;
            else {
                switch (document.Type) {
                    case DocumentType.RemoteScript: saveDialog.FilterIndex = 1; break;
                    case DocumentType.AliasScript: saveDialog.FilterIndex = 2; break;
                    default: saveDialog.FilterIndex = 4; break;
                }
            }
            DialogResult result = saveDialog.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK) {
                // Set the format.
                DocumentType type;
                switch (saveDialog.FilterIndex) {
                    case 1: type = DocumentType.RemoteScript; break;
                    case 2: type = DocumentType.AliasScript; break;
                    default: type = document.Type; break;
                }

                if (type != document.Type && MessageBox.Show(this, "This file was " + (document.Type == DocumentType.AliasScript ? "an " : "a ") + Program.GetTypeString(document.Type) +
                    ". Are you sure you want to save it as " + (type == DocumentType.AliasScript ? "an " : "a ") + Program.GetTypeString(type) + "?",
                    "MScripter", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No) return;

                // Save the file.
                document.Save(saveDialog.FileName, saveDialog.FilterIndex == 3);
                document.ReadOnly = false;
            }
        }
        private void saveHashtableAs(TabPage tabPage) {
            Hashtable hashtable = (Hashtable) tabPage.Tag;

            // Show the dialog box.
            saveDialog.Filter = "Text file|*|INI file|*.ini|Binary file|*";
            switch (hashtable.Format) {
                case HashtableFormat.Text: saveDialog.FilterIndex = 1; break;
                case HashtableFormat.INI: saveDialog.FilterIndex = 2; break;
                case HashtableFormat.Binary: saveDialog.FilterIndex = 3; break;
                default: saveDialog.FilterIndex = 1; break;
            }

            DialogResult result = saveDialog.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK) {
                // Set the format.
                HashtableFormat type;
                switch (saveDialog.FilterIndex) {
                    case 1: type = HashtableFormat.Text; break;
                    case 2: type = HashtableFormat.INI; break;
                    case 3: type = HashtableFormat.Binary; break;
                    default: throw new IndexOutOfRangeException("Invalid filter index.");
                }

                // Save the file.
                hashtable.Save(saveDialog.FileName, type);
                hashtable.ReadOnly = false;
            }
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e) {
            if (e.TabPage == null) {
                currentItem = null;
            } else if (e.TabPage.ImageIndex == 7) {
                currentItem = e.TabPage.Tag;

                // This tab is a hashtable.
                showHashtable(e.TabPage);

                hashtableAddPanel.Parent = e.TabPage;
                hashtableListView.Parent = e.TabPage;
                hashtableHeaderPanel.Parent = e.TabPage;
            } else if (e.TabPage.ImageIndex == 9) {
                // This tab is a dialog.
                currentItem = e.TabPage.Tag;
            } else {
                // This tab is a file.
                MslDocument document = (MslDocument) e.TabPage.Tag;
                currentItem = document;

                parsingProgressBarPanel.Parent = e.TabPage;
                parsingProgressBarPanel.SendToBack();
                functionList.Parent = e.TabPage;
                functionList.SendToBack();
                RebuildFunctionList();
                e.TabPage.PerformLayout();

                document.textBoxes[document.SelectedIndex].Select();
                document.textBoxes[document.SelectedIndex].Refresh();
            }
        }

        public void refreshHashtable() { refreshHashtable(tabControl.SelectedTab); }
        public void refreshHashtable(TabPage tabPage) {
            Hashtable hashtable = (Hashtable) tabPage.Tag;
            MircMessenger context = null;

            if (hashtable.SyncTarget != IntPtr.Zero) hashtable.content.Clear();
            hashtableListView.Items.Clear();

            if (hashtable.SyncTarget != IntPtr.Zero) {
                context = new MircMessenger(hashtable.SyncTarget);
            }

            int tableCount; string key; byte[] value; string displayValue; int timeRemaining;
            IEnumerator<KeyValuePair<string, byte[]>> enumerator = null;

            if (hashtable.SyncTarget == IntPtr.Zero) {
                tableCount = hashtable.content.Count;
                label3.Text = tableCount.ToString();
                label5.Text = "—";
                toolTip.SetToolTip(label4, "This is only relevant when the hashtable is synced with mIRC.");
                toolTip.SetToolTip(label5, "This is only relevant when the hashtable is synced with mIRC.");

                enumerator = hashtable.content.GetEnumerator();
            } else {
                label3.Text = context.Evaluate("$hget(" + hashtable.Name + ", 0).item");
                label5.Text = context.Evaluate("$hget(" + hashtable.Name + ").size");
                toolTip.SetToolTip(label4, null);
                toolTip.SetToolTip(label5, null);
                tableCount = int.Parse(label3.Text);
            }

            for (int i = 1; i <= tableCount; ++i) {
                // Get the value.
                if (hashtable.SyncTarget == IntPtr.Zero) {
                    enumerator.MoveNext();
                    key = enumerator.Current.Key;
                    timeRemaining = 0;

                    if (hashtableBinaryViewButton.Checked)
                        value = enumerator.Current.Value;
                    else
                        value = enumerator.Current.Value;

                } else {
                    key = context.Evaluate("$hget(" + hashtable.Name + ", " + i + ").item");
                    timeRemaining = int.Parse(context.Evaluate("$hget(" + hashtable.Name + ", " + key + ").unset"));

                    if (hashtableBinaryViewButton.Checked) {
                        // $null does actually evaluate parameters, even though they don't do anything. This avoids the need for a helper script.
                        string reply = context.Evaluate("$null($hget(" + hashtable.Name + ", " + key + ", &b)) $bvar(&b, 1, 2147483647)");
                        if (reply.Length == 0) value = new byte[0];
                        else value = reply.Split(' ').Select(s => byte.Parse(s)).ToArray();
                    } else
                        value = Encoding.UTF8.GetBytes(context.Evaluate("$hget(" + hashtable.Name + ", " + key + ")"));
                }

                // Format the value.
                if (hashtableBinaryViewButton.Checked) {
                    displayValue = FormatBinaryValue(value);
                } else {
                    try {
                        displayValue = Program.encoding.GetString(value);
                    } catch (DecoderFallbackException) {
                        displayValue = Encoding.Default.GetString(value);
                    }
                }

                // Add the value.
                if (hashtable.SyncTarget != IntPtr.Zero) hashtable.content.Add(key, value);

                ListViewItem item = new ListViewItem(key);
                item.UseItemStyleForSubItems = false;
                if (value.Length == 0)
                    item.SubItems.Add("Empty", SystemColors.GrayText, Color.Empty, item.Font);
                else
                    item.SubItems.Add(displayValue);
                item.SubItems.Add(timeRemaining == 0 ? "Never" : "In " + timeRemaining + " seconds");
                hashtableListView.Items.Add(item);
            }

            if (hashtable.SyncTarget != IntPtr.Zero) {
                context.Dispose();
            }

            hashtableListView.Items.Add(new ListViewItem("Add new...") { ForeColor = SystemColors.GrayText });
        }

        public string FormatBinaryValue(byte[] value) {
            string displayValue = "";
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();

            if (value.Length == 0) return "";

            builder.EnsureCapacity(value.Length * (hashtableBaseComboBox.SelectedIndex == 1 ? 3 : 4));
            builder2.EnsureCapacity(value.Length);
            foreach (byte b in value) {
                builder2.Append((char) b);
                if (hashtableBaseComboBox.SelectedIndex == 1) builder.Append(b.ToString("X2"));
                else builder.Append(b.ToString().PadLeft(3, '\u2002'));  // That's an en space.
                builder.Append(" ");
            }
            displayValue = builder.ToString();

            return displayValue;
        }

        private void tabControl_Deselecting(object sender, TabControlCancelEventArgs e) {
            if (e.TabPage == null) return;
            e.TabPage.SuspendLayout();
            if (e.TabPage.ImageIndex == 7) return;
            if (e.TabPage.ImageIndex == 9) {
                UpdateDialogCode();
                return;
            }
        }

        private void FormatFile(MslDocument file) {
            parsingProgressBar.Maximum = file.TextBox.Lines.Count;
            parsingProgressBar.Value = 0;
            parsingProgressBarPanel.Show();
            functionList.Hide();
            formatWorker.RunWorkerAsync(file);
        }

        private void ActivateNode(TreeNode node) {
            // Open a file if the user double-clicks it in the list.
            if (node.ImageIndex == 14) {
                OpenHashtable(((Tuple<IntPtr, uint>) node.Parent.Parent.Tag).Item2, node.Text, ((Tuple<IntPtr, uint>) node.Parent.Parent.Tag).Item1);
            } else if (node.ImageIndex >= 8) {
                TreeNode root = node;
                while (root.Parent != null) root = root.Parent;

                if (root.Tag is Tuple<IntPtr, uint>)  // A file linked to mIRC
                    OpenFile(node.ToolTipText, openDialog.ReadOnlyChecked, ((Tuple<IntPtr, uint>) root.Tag).Item1);
                else
                    OpenFile((string) node.Tag, false, IntPtr.Zero);
            }
        }

        private void restyleDocumentToolStripMenuItem_Click(object sender, EventArgs e) {
            FormatFile((MslDocument) currentItem);
        }

        private void tabControl_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                fileContextMenu.Tag = null;
                // Show the context menu.
                for (int i = 0; i < tabControl.TabCount; ++i) {
                    if (tabControl.GetTabRect(i).Contains(e.Location)) {
                        fileContextMenu.Tag = tabControl.TabPages[i];
                        fileContextMenu.Show(tabControl, e.Location);
                        return;
                    }
                }
            }
        }

        private void fileContextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e) {
            // Clean up the menu.
            remoteScriptToolStripMenuItem.Checked = false;
            aliasScriptToolStripMenuItem.Checked = false;
            popupScriptToolStripMenuItem.Checked = false;
            usersListToolStripMenuItem.Checked = false;
            variablesListToolStripMenuItem.Checked = false;
        }

        private void remoteScriptToolStripMenuItem_Click(object sender, EventArgs e) {
            // The menu's tag points to the tab page. The tab page's tag points to the file. Scavenger hunt complete.
            MslDocument file = (MslDocument) ((TabPage) fileContextMenu.Tag).Tag;
            file.Type = DocumentType.RemoteScript;
            FormatFile(file);
        }

        private void aliasScriptToolStripMenuItem_Click(object sender, EventArgs e) {
            MslDocument file = (MslDocument) ((TabPage) fileContextMenu.Tag).Tag;
            file.Type = DocumentType.AliasScript;
            FormatFile(file);
        }

        private void popupScriptToolStripMenuItem_Click(object sender, EventArgs e) {
            MslDocument file = (MslDocument) ((TabPage) fileContextMenu.Tag).Tag;
            file.Type = DocumentType.Popup;
            FormatFile(file);
        }

        private void usersListToolStripMenuItem_Click(object sender, EventArgs e) {
            MslDocument file = (MslDocument) ((TabPage) fileContextMenu.Tag).Tag;
            file.Type = DocumentType.Users;
            FormatFile(file);
        }

        private void variablesListToolStripMenuItem_Click(object sender, EventArgs e) {
            MslDocument file = (MslDocument) ((TabPage) fileContextMenu.Tag).Tag;
            file.Type = DocumentType.Variables;
            FormatFile(file);
        }

        private void fileContextMenu_Opening(object sender, CancelEventArgs e) {
            if (currentItem is MslDocument) {
                formatAsToolStripMenuItem.Enabled = true;
                formatAsToolStripMenuItem.ToolTipText = null;

                // Check one of the syntax type options.
                switch (((MslDocument) currentItem).Type) {
                    case DocumentType.RemoteScript: remoteScriptToolStripMenuItem.Checked = true; break;
                    case DocumentType.AliasScript: aliasScriptToolStripMenuItem.Checked = true; break;
                    case DocumentType.Popup: popupScriptToolStripMenuItem.Checked = true; break;
                    case DocumentType.Users: usersListToolStripMenuItem.Checked = true; break;
                    case DocumentType.Variables: variablesListToolStripMenuItem.Checked = true; break;
                }
            } else {
                formatAsToolStripMenuItem.Enabled = false;
                formatAsToolStripMenuItem.ToolTipText = "This is only valid for text documents.";
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e) {
            // Allow a mIRC window to be selected.
            if (e.Node.ImageIndex == 0) {
                selectedWindow = ((Tuple<IntPtr, uint>) e.Node.Tag).Item1;

                foreach (TreeNode node in treeView.Nodes)
                    node.NodeFont = null;

                e.Node.NodeFont = new Font(treeView.Font, FontStyle.Bold);
            }
        }

        private void treeView_DoubleClick(object sender, EventArgs e) {
            if (treeView.SelectedNode == null || !treeView.SelectedNode.Bounds.Contains(treeView.PointToClient(Cursor.Position))) return;
            ActivateNode(treeView.SelectedNode);
        }

        private void OpenFile(string filePath, bool readOnly, IntPtr syncTarget) {
            MslDocument document;
            foreach (TabPage tab in tabControl.TabPages) {
                if (!(tab.Tag is MslDocument)) continue;
                document = (MslDocument) tab.Tag;
                if (document.Path == filePath) {
                    // The file is already open; select it instead.
                    tabControl.SelectTab(tab);
                    return;
                }
            }

            // Open the file.
            DocumentType type; bool detectType;
            string ext = Path.GetExtension(filePath);

            if (Program.Config.PreferredAliasExtension != string.Empty &&
                Program.Config.PreferredAliasExtension != "mrc" &&
                Program.Config.PreferredAliasExtension != "ini" &&
                "." + ext == Program.Config.PreferredAliasExtension) {
                detectType = false;
                type = DocumentType.AliasScript;
            } else {
                switch (ext.ToLowerInvariant()) {
                    case ".mrc":
                        detectType = !Program.Config.AssumeMrcFilesAreRemote;
                        type = DocumentType.RemoteScript;
                        break;
                    case ".als": detectType = false; type = DocumentType.AliasScript; break;
                    case ".ini": detectType = false; type = DocumentType.RemoteScript; break;
                    default: detectType = true; type = DocumentType.RemoteScript; break;
                }
            }
            
            document = MslDocument.FromFile(filePath, type, readOnly, syncTarget);
            if (detectType) document.DetectType();

            TabPage newTab = newTabDocument(document);
            FormatFile(document);
            addTabPage(newTab);

            documents.Add(document);
            tabControl.SelectTab(newTab);
        }
        private void OpenHashtable(uint processID, string name, IntPtr syncTarget) {
            string title = (processID == 0 ? name : processID + "/" + name);

            foreach (TabPage tab in tabControl.TabPages) {
                if (tab.ImageIndex == 7 && tab.Text == title) {
                    // The hashtable is already open; select it instead.
                    tabControl.SelectTab(tab);
                    return;
                }
            }

            Hashtable hashtable = new Hashtable(treeView.SelectedNode.Text, new Dictionary<string, byte[]>(), null, HashtableFormat.Text, false, syncTarget);

            // Open the hashtable.
            TabPage newTab = new TabPage(title) { Tag = hashtable, ImageIndex = 7 };
            hashtables.Add(hashtable);
            addTabPage(newTab);

            tabControl.SelectTab(newTab);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            if (currentItem is MslDocument) {
                var currentDocument = (MslDocument) currentItem;

                if (currentDocument.ReadOnly || !currentDocument.Saved) {
                    saveAsToolStripMenuItem_Click(sender, e);
                } else if (tabControl.SelectedTab.ImageIndex == 7) {
                    Hashtable hashtable = (Hashtable) tabControl.SelectedTab.Tag;
                    hashtable.Save();
                } else {
                    saveAndSync(currentDocument);
                }
            }
        }

        private void saveAndSync(MslDocument document) { saveAndSync(document, null, null); }
        private void saveAndSync(MslDocument document, string path, bool? INI) {
            // Save the file.
            if (path == null)
                document.Save();
            else
                document.Save(path, (bool) INI);

            // Sync the file.
            if (document.SyncTarget != IntPtr.Zero) {
                using (var context = new MircMessenger(document.SyncTarget)) {
                    switch (document.Type) {
                        case DocumentType.RemoteScript: context.Run("/reload -rs \"" + document.Path + "\""); break;
                        case DocumentType.AliasScript : context.Run("/reload -a \""  + document.Path + "\""); break;
                        case DocumentType.Popup       : context.Run("/reload -ps \"" + document.Path + "\""); break;
                        case DocumentType.Users       : context.Run("/reload -ru \"" + document.Path + "\""); break;
                        case DocumentType.Variables   : context.Run("/reload -rv \"" + document.Path + "\""); break;
                        default: return;
                    }
                }
            }
        }

        private void MainForm_Shown(object sender, EventArgs e) {
            ((MslDocument) currentItem).SelectedTextBox.Focus();
        }

        private void MslTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            var textBox = (FastColoredTextBox) sender;
            var document = (MslDocument) ((FastColoredTextBox) sender).Tag;

            Console.WriteLine($"Changed range: {e.ChangedRange.Start.iLine}:{e.ChangedRange.Start.iChar} - {e.ChangedRange.End.iLine}:{e.ChangedRange.End.iChar}");

            if (document != null) {
                if (textBox.LinesCount != document.lineCount) {
                    if (e.ChangedRange.End.iLine != e.ChangedRange.Start.iLine) {
                        // TODO: Remove this.
                        if (textBox.LinesCount < document.lineCount) throw new Exception("Adding lines decreased the line count?!");
                        int start = e.ChangedRange.FromLine + 1; int count = textBox.LinesCount - document.lineCount;
                        //Console.WriteLine($"{count} line(s) added at {start}.");
                        /*
                        for (int i = document.bookmarks.Count - 1; i >= 0; --i) {
                            var bookmark = document.bookmarks[i];

                            if (bookmark.position >= start) {
                                bookmark.position += count;
                            }
                        }
                        */
                    } else {
                        // TODO: Remove this.
                        if (textBox.LinesCount > document.lineCount) throw new Exception("Removing lines increased the line count?!");
                        int start = e.ChangedRange.FromLine + 1; int count = document.lineCount - textBox.LinesCount; int end = start + count;
                        //Console.WriteLine($"{count} line(s) removed at {start}.");

                        /*
                        for (int i = document.bookmarks.Count - 1; i >= 0; --i) {
                            var bookmark = document.bookmarks[i];

                            if (bookmark.position >= start) {
                                if (bookmark.position < end) {
                                    document.bookmarks.RemoveAt(i);
                                    bookmark.position = -1;
                                    bookmark.endLine = -1;
                                } else {
                                    bookmark.position -= count;
                                    bookmark.endLine -= count;
                                }
                            } else if (bookmark.endLine >= start) {
                                if (bookmark.endLine < end)
                                    bookmark.endLine = start - 1;
                                else
                                    bookmark.endLine -= count;
                            }
                        }
                        */
                    }
                }

                mSLSyntaxHighlight(textBox, e.ChangedRange, document.Type);
                
                if (cursorCharType == 1 && textBox[textBox.Selection.ToLine][textBox.Selection.End.iChar - 1].c == ':') {
                    document.autoCompleteMenu.Show(true);
                    cursorCharType = 0;
                }
            }
        }

        private void MslTextBox_SelectionChangedDelayed(object sender, EventArgs e) {
            if (!surpressSelectionEvents && currentItem != null && currentItem == ((FastColoredTextBox) sender).Tag) {
                var currentDocument = (MslDocument) currentItem;

                Line currentLine = currentDocument.textBoxes[0][currentDocument.textBoxes[0].Selection.End.iLine];
                LineInfo lineInfo_ = currentDocument.GetLineInfo(currentDocument.textBoxes[0].Selection.End.iLine);
				if (lineInfo_ != null) {
					debugTextBox.Text = $"Current line ({currentDocument.textBoxes[0].Selection.End.iLine}):\n{currentLine.Text}\n\n{lineInfo_.tags.Count} tags\n";
					foreach (var tag in lineInfo_.tags) {
						debugTextBox.AppendText($"  {tag.type} @ {tag.range.Start.iChar} - {tag.range.End.iChar}\n");
					}
				}

                surpressSelectionEvents = true;
                MslBookmark nearest = null;
                var cursorLine = currentDocument.SelectedTextBox.Selection.End.iLine;
                LineInfo lineInfo;
                if (currentDocument.lineInfoTable.TryGetValue(currentDocument.TextBox[cursorLine].UniqueId, out lineInfo) && lineInfo.BraceLevel <= 0) {
                    // Allow the cursor to be on a closing brace too.
                    if (cursorLine == 0 || (currentDocument.lineInfoTable.TryGetValue(currentDocument.TextBox[cursorLine - 1].UniqueId, out lineInfo) && lineInfo.BraceLevel <= 0)) {
                        functionList.SelectedItem = null;
                        surpressSelectionEvents = false;
                        return;
                    }
                }

                foreach (var bookmark in currentDocument.bookmarks) {
                    if (nearest == null || (bookmark.position > nearest.position && bookmark.position <= cursorLine))
                        nearest = bookmark;
                }

                functionList.SelectedItem = nearest;
                surpressSelectionEvents = false;
            }
            checkWordHighlight();
        }

        private Range highlightedRange;
        private void MslTextBox_ToolTipNeeded(object sender, ToolTipNeededEventArgs e) {
            var textBox = (FastColoredTextBox) sender;

            var errorIndex = textBox.GetStyleIndex(MslSyntaxHighlighter.ErrorStyle);
            var warningIndex = textBox.GetStyleIndex(MslSyntaxHighlighter.WarningStyle);

            if (e.Place.iChar >= textBox[e.Place.iLine].Count) return;

            // Look for a tag.
            var tag = ((MslDocument) currentItem).GetTag(e.Place, TagType.EventName);
            if (tag != null) {
                if (highlightedRange != null)
                    highlightedRange.ClearStyle(wordHighlightStyle);

                highlightedRange = tag.range;
                highlightedRange.SetStyle(wordHighlightStyle);

                var eventName = tag.range.Text;
                MslSyntaxHighlighter.Event eventData;
                if (MslSyntaxHighlighter.events.TryGetValue(eventName, out eventData)) {
                    e.ToolTipIcon = ToolTipIcon.None;
                    e.ToolTipTitle = eventName + " event";
                    e.ToolTipText = eventData.Description;
                    return;
                }
            }


            if (errorIndex != -1 && textBox[e.Place].style.HasFlag((StyleIndex) (1 << errorIndex))) e.ToolTipTitle = "Syntax error";
            else if (warningIndex != -1 && textBox[e.Place].style.HasFlag((StyleIndex) (1 << warningIndex))) e.ToolTipTitle = "Warning";
            else return;

            var error = ((MslDocument) currentItem).errors.FirstOrDefault(error2 => error2.Location.Contains(e.Place));
            if (error != null)
                e.ToolTipText = error.Text;
#if (DEBUG)
            else
                Program.impossible(7);
#endif
        }

        private void MslTextBox_LineRemoved(object sender, LineRemovedEventArgs e) {
            Console.WriteLine($"{e.Count} line(s) removed at {e.Index}.");
            var document = (MslDocument) ((FastColoredTextBox) sender).Tag;
            foreach (var id in e.RemovedLineUniqueIds)
                document.lineInfoTable.Remove(id);
        }

        private void MslTextBox_LineInserted(object sender, LineInsertedEventArgs e) {
            var document = (MslDocument) ((FastColoredTextBox) sender).Tag;
            Console.WriteLine($"{e.Count} line(s) added at {e.Index}.");
            for (int i = document.bookmarks.Count - 1; i >= 0; --i) {
                var bookmark = document.bookmarks[i];

                if (bookmark.position >= e.Index) {
                    bookmark.position += e.Count;
                }
            }
        }

        private void formatWorker_DoWork(object sender, DoWorkEventArgs e) {
            var worker = (BackgroundWorker) sender;
            MslDocument document = (MslDocument) e.Argument;

            document.TextBox.Range.ClearStyle(MslSyntaxHighlighter.CommandStyle,
                                              MslSyntaxHighlighter.CommentStyle,
                                              MslSyntaxHighlighter.CustomCommandStyle,
                                              MslSyntaxHighlighter.ErrorStyle,
                                              MslSyntaxHighlighter.FunctionStyle,
                                              MslSyntaxHighlighter.FunctionPropertyStyle,
                                              MslSyntaxHighlighter.KeywordStyle,
                                              MslSyntaxHighlighter.AliasStyle,
                                              MslSyntaxHighlighter.VariableStyle,
                                              ControlCharStyle.Instance);
        
            document.bookmarks.Clear();
            document.errors.Clear();
            for (int i = 0; i < document.TextBox.LinesCount; ++i) {
                var result = MslSyntaxHighlighter.Highlight(document.TextBox, i, document.Type);

                // Skip past continued lines.
                while (document.GetLineInfo(i)?.State == (byte) MslSyntaxHighlighter.ParseStateIndex.Continuation)
                    ++i;

                if (result != null) {
                    if (result.bookmark != null)
                        document.bookmarks.Add(result.bookmark);
                    if (result.errors != null)
                        document.errors.AddRange(result.errors);
                }

                if (i % 16 == 0) worker.ReportProgress(i);
            }

            document.TextBox.Range.SetStyle(ControlCharStyle.Instance, @"[\x01-\x03\x0F\x13\x16\x1C\x1F]");
            document.TextBox.Range.ClearFoldingMarkers();
            document.TextBox.Range.SetFoldingMarkers("(?<![^ :\r\n]){(?![^ \r\n])", "(?<![^ \r\n])}(?![^ \r\n])");
        }

        private void formatWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            parsingProgressBar.Value = e.ProgressPercentage;  // It doesn't actually have to be a percentage.
        }

        private void formatWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            parsingProgressBarPanel.Hide();
            functionList.Show();
            ((MslDocument) currentItem).TextBox.Refresh();

            errorList.Items.Clear();
            RebuildFunctionList();
            RebuildErrorList();
        }

        private void RebuildFunctionList() {
            surpressSelectionEvents = true;
            if (currentItem is MslDocument) {
                MslBookmark nearest = null;
                var cursorLine = ((MslDocument) currentItem).SelectedTextBox.Selection.End.iLine;

                functionList.BeginUpdate();
                functionList.Items.Clear();
                foreach (var bookmark in ((MslDocument) currentItem).bookmarks.ToArray()) {
                    if (nearest == null || (bookmark.position > nearest.position && bookmark.position <= cursorLine))
                        nearest = bookmark;

                    functionList.Items.Add(bookmark);
                }

                functionList.SelectedItem = nearest;
                functionList.EndUpdate();
            }
            surpressSelectionEvents = false;
        }

        private void RebuildErrorList() {
            int i = 0; var count = new int[3];
            var toggleButtons = new ToolStripButton[] { errorsToolStripButton, warningsToolStripButton, noticesToolStripButton };

            errorList.Items.Clear();
            if (!(currentItem is MslDocument)) {
                errorsToolStripButton.Text = "Errors";
                warningsToolStripButton.Text = "Warnings";
                noticesToolStripButton.Text = "Notices";
                return;
            }
            foreach (var error in ((MslDocument) currentItem).errors) {
                ++count[(int) error.Type];

                if (toggleButtons[(int) error.Type].Checked) {
                    ListViewItem item = new ListViewItem("");
                    item.Tag = error;
                    item.StateImageIndex = (int) error.Type;
                    item.SubItems.Add((++i).ToString());
                    item.SubItems.Add(error.Text);
                    item.SubItems.Add(Path.GetFileName(((MslDocument) currentItem).Path) + ":" + (error.Location.FromLine + 1));
                    errorList.Items.Add(item);
                }
            }

            if (count[0] == 1)
                errorsToolStripButton.Text = "1 error";
            else
                errorsToolStripButton.Text = count[0] + " errors";
            if (count[1] == 1)
                warningsToolStripButton.Text = "1 warning";
            else
                warningsToolStripButton.Text = count[1] + " warnings";
            if (count[2] == 1)
                noticesToolStripButton.Text = "1 notice";
            else
                noticesToolStripButton.Text = count[2] + " notices";
        }

        private void mSLSyntaxHighlight(FastColoredTextBox sender, Range range, DocumentType syntaxType) {
            MslDocument document = (MslDocument) range.tb.Tag;

            /*
            //clear style of changed range
            if (sender == consoleTextBox) {
                if (acceptingConsoleInput) 
                    new Range(consoleTextBox, range.ToLine).ClearStyle(MslSyntaxHighlighter.CommandStyle,
                                                                                MslSyntaxHighlighter.CommentStyle,
                                                                                MslSyntaxHighlighter.CustomCommandStyle,
                                                                                MslSyntaxHighlighter.ErrorStyle,
                                                                                MslSyntaxHighlighter.FunctionStyle,
                                                                                MslSyntaxHighlighter.FunctionPropertyStyle,
                                                                                MslSyntaxHighlighter.KeywordStyle,
                                                                                MslSyntaxHighlighter.AliasStyle,
                                                                                MslSyntaxHighlighter.VariableStyle,
                                                                                ControlCharStyle.Instance);
            } else {
                range.ClearStyle(MslSyntaxHighlighter.CommandStyle,
                                          MslSyntaxHighlighter.CommentStyle,
                                          MslSyntaxHighlighter.CustomCommandStyle,
                                          MslSyntaxHighlighter.ErrorStyle,
                                          MslSyntaxHighlighter.FunctionStyle,
                                          MslSyntaxHighlighter.FunctionPropertyStyle,
                                          MslSyntaxHighlighter.KeywordStyle,
                                          MslSyntaxHighlighter.AliasStyle,
                                          MslSyntaxHighlighter.VariableStyle,
                                          ControlCharStyle.Instance);
            }
            */

            // Highlight the text.
            if (sender == consoleTextBox) {
                if (acceptingConsoleInput)
                    MslSyntaxHighlighter.Highlight(range.tb, range.ToLine, syntaxType);
            } else {
                bool bookmarksChanged = false; bool errorsChanged = false;
                // If the changed line was a continuation, find the start.
                int i = range.FromLine;
                LineInfo lineInfo;
                while (i != 0 && document.lineInfoTable.TryGetValue(document.TextBox[i - 1].UniqueId, out lineInfo) && lineInfo.State == (byte) MslSyntaxHighlighter.ParseStateIndex.Continuation)
                    --i;

                for (; i < range.tb.LinesCount; ++i) {
                    LineInfo oldLineInfo; MslBookmark oldBookmark = null; string oldTitle = null;
                    if (document.lineInfoTable.TryGetValue(range.tb[i].UniqueId, out oldLineInfo)) {
                        oldBookmark = oldLineInfo.bookmark;
                        oldTitle = oldBookmark?.title;
                    }

                    var newLineInfo = MslSyntaxHighlighter.Highlight(range.tb, i, syntaxType);

                    if (document.Type != DocumentType.Text) {
                        // Deal with bookmarks.
                        if (oldBookmark != null) {
                            if (newLineInfo.bookmark != null) {
                                if (oldBookmark != newLineInfo.bookmark) {
                                    // A bookmark was replaced.
                                    document.bookmarks[document.bookmarks.IndexOf(oldBookmark)] = newLineInfo.bookmark;
                                    bookmarksChanged = true;
                                } else if (oldTitle != newLineInfo.bookmark.title) {
                                    // A bookmark was renamed.
                                    surpressSelectionEvents = true;
                                    functionList.RefreshItem(functionList.Items.IndexOf(newLineInfo.bookmark));
                                    surpressSelectionEvents = false;
                                }
                            } else {
                                // A bookmark was removed.
                                document.bookmarks.Remove(oldLineInfo.bookmark);
                                bookmarksChanged = true;
                            }
                        } else {
                            if (newLineInfo?.bookmark != null) {
                                // A bookmark was added.
                                document.bookmarks.Add(newLineInfo.bookmark);
                                bookmarksChanged = true;
                            }
                        }
                    }

                    if (document.Type != DocumentType.Text && document.Type != DocumentType.INI) {

                        if (oldLineInfo != null && oldLineInfo.errors.Length != 0) {
                            errorsChanged = true;
                            foreach (var error in oldLineInfo.errors)
                                document.errors.Remove(error);
                        }

                        // Skip past continued lines.
                        while (i < range.tb.LinesCount && document.lineInfoTable.TryGetValue(document.TextBox[i].UniqueId, out lineInfo) && lineInfo.State == (byte) MslSyntaxHighlighter.ParseStateIndex.Continuation) {
                            // Always reparse the next line if there is a continuation.
                            // This avoids glitches when continuations are added or removed.
                            oldLineInfo = null;
                            ++i;

                            if (lineInfo.errors.Length != 0) errorsChanged = true;
                            foreach (Error error in lineInfo.errors)
                                document.errors.Remove(error);
                        }

                        // Deal with errors.
                        if (newLineInfo != null && newLineInfo.errors.Length != 0) {
                            errorsChanged = true;
                            foreach (var error in newLineInfo.errors)
                                document.errors.Add(error);
                        }

                        // (Normally we only reparse the next line if it was changed, or the current line was changed in such
                        //  a way as to affect its meaning, such as adding braces.)
                        if (i >= range.ToLine && oldLineInfo != null && 
                            newLineInfo.BraceLevel == oldLineInfo.BraceLevel && newLineInfo.State == oldLineInfo.State) break;
                    } else {
                        // Such cascading isn't an issue for INI files.
                        if (i >= range.ToLine) break;
                    }
                }

                if (bookmarksChanged) {
                    functionList.Items.Clear();
                    if (currentItem == document) RebuildFunctionList();
                }

                if (errorsChanged) {
                    errorList.Items.Clear();
                    RebuildErrorList();
                }
            }

            range.SetStyle(ControlCharStyle.Instance, @"[\x01-\x03\x0F\x13\x16\x1C\x1F]");

            range.ClearFoldingMarkers();
            range.SetFoldingMarkers("(?<![^ :\r\n]){(?![^ \r\n])", "(?<![^ \r\n])}(?![^ \r\n])");
        }

        private void consoleTextBox_Paint(object sender, PaintEventArgs e) {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, consoleTextBox.ForeColor)), consoleTextBox.Width - 75, 5, 70, 25);
            TextRenderer.DrawText(e.Graphics, "Console", consoleTextBox.Font, new Rectangle(consoleTextBox.Width - 75, 5, 70, 25), consoleTextBox.BackColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
            if (e.Node.ImageIndex == 6) {
                // Enumerate hashtables.
                IntPtr window = ((Tuple<IntPtr, uint>) e.Node.Parent.Tag).Item1;

                using (var context = new MircMessenger(window)) {
                    e.Node.Nodes.Clear();

                    int hashtableCount = int.Parse(context.Evaluate("$hget(0)"));
                    string[] hashtables = new string[hashtableCount];
                    for (int i = 1; i <= hashtableCount; ++i) {
                        hashtables[i - 1] = context.Evaluate("$hget(" + i + ")");

                        TreeNode node = new TreeNode(hashtables[i - 1], 14, 14);
                        e.Node.Nodes.Add(node);
                    }
                }
            } else if (e.Node.ImageIndex == 7) {
                // Directory node; enumerate the contents.
                e.Node.Nodes.Clear();

                var directory = (string) e.Node.Tag;
                if (Directory.Exists(directory)) {
                    foreach (var directory2 in Directory.GetDirectories(directory)) {
                        var node = new TreeNode(Path.GetFileName(directory2), 7, 7) { Tag = directory2 };
                        node.Nodes.Add("dummy");  // This allows the node to be expanded.
                        e.Node.Nodes.Add(node);
                    }
                    foreach (var file in Directory.GetFiles(directory))
                        e.Node.Nodes.Add(new TreeNode(Path.GetFileName(file), 8, 8) { Tag = file });
                }
            }
        }

        private void hashtableBinaryViewButton_CheckedChanged(object sender, EventArgs e) {
            showHashtable(tabControl.SelectedTab);
        }

        private void hashtableBaseComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            showHashtable(tabControl.SelectedTab);
        }

        private void hashtableListView_DoubleClick(object sender, EventArgs e) {
        }

        private void hashtableListView_MouseDoubleClick(object sender, MouseEventArgs e) {
            Point point = hashtableListView.PointToClient(Cursor.Position);
            if (hashtableListView.GetItemAt(e.X, e.Y) == null) {
                // Add a new item.
                ListViewItem newItem = new ListViewItem(new string[] { "", "", "" });
                newItem.Tag = true;

                hashtableListView.Items.Add(newItem);
                newItem.BeginEdit();
            }
        }

        private void hashtableListView_ItemActivate(object sender, EventArgs e) {
            if (hashtableListView.SelectedItems[0].SubItems.Count < 3) {
                hashtableListView.SelectedItems[0].SubItems.Add("Press Tab");
                hashtableAddPanel.Location = hashtableListView.SelectedItems[0].SubItems[0].Bounds.Location;
                hashtableAddPanel.Location += new Size(hashtableListView.Location);
                hashtableAddPanel.Width = hashtableListView.Columns[0].Width;
                hashtableAddTextBox.Text = "Item" + hashtableListView.Items.Count;

                hashtableAddExpiryPanel.Hide();
                hashtableAddTextBox.Show();
                hashtableAddPanel.ColumnStyles[1].Width = 100;
                hashtableAddPanel.ColumnStyles[2].Width =   0;

                hashtableEditState = 0;
                hashtableAddPanel.Show();
                hashtableAddTextBox.Focus();
                hashtableAddTextBox.SelectAll();
            }
        }

        private void hashtableListView_AfterLabelEdit(object sender, LabelEditEventArgs e) {
            hashtableListView.LabelEdit = false;
            
        }

        private void hashtableItemNameTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter) {
                ;
            }
        }

        private void hashtableAddForwardButton_Enter(object sender, EventArgs e) {
            if (hashtableEditState == 0) {
                if (hashtableListView.SelectedItems[0].SubItems.Count == 2) {
                    hashtableListView.SelectedItems[0].SubItems.Add("Press Tab");
                    hashtableListView.SelectedItems[0].SubItems[2].Tag = 0;
                }
                hashtableAddPanel.Location = hashtableListView.SelectedItems[0].SubItems[1].Bounds.Location;
                hashtableAddPanel.Location += new Size(hashtableListView.Location);
                hashtableAddPanel.Width = hashtableListView.Columns[1].Width;
                hashtableAddTextBox.Text = "";

                hashtableEditState = 1;
                hashtableAddTextBox.Focus();
                hashtableAddTextBox.SelectAll();
            } else if (hashtableEditState == 1) {
                // Set the value.
                hashtableListView.SelectedItems[0].SubItems[1].Text = hashtableAddTextBox.Text;
                hashtableListView.SelectedItems[0].SubItems[1].Tag = hashtableAddTextBox.Text;

                hashtableAddTextBox.Hide();
                hashtableAddExpiryPanel.Show();
                hashtableAddPanel.ColumnStyles[1].Width =   0;
                hashtableAddPanel.ColumnStyles[2].Width = 100;

                hashtableAddPanel.Location = hashtableListView.SelectedItems[0].SubItems[2].Bounds.Location;
                hashtableAddPanel.Location += new Size(hashtableListView.Location);
                hashtableAddPanel.Width = hashtableListView.Columns[2].Width;
                if ((int) hashtableListView.SelectedItems[0].SubItems[2].Tag == 0) {
                    hashtableAddExpiryUnitBox.SelectedIndex = 0;
                    hashtableAddExpiryTimeBox.Value = 0;
                } else {
                    hashtableAddExpiryUnitBox.SelectedIndex = 0;
                    hashtableAddExpiryTimeBox.Value = (int) hashtableListView.SelectedItems[0].SubItems[2].Tag;
                }

                hashtableEditState = 2;
                hashtableAddExpiryTimeBox.Focus();
                hashtableAddExpiryTimeBox.Select(0, hashtableAddExpiryTimeBox.Text.Length);
            } else if (hashtableEditState == 2) {
                hashtableListView.Focus();
            }
        }

        private void hashtableAddBackButton_Enter(object sender, EventArgs e) {
            if (hashtableEditState == 0) {
                hashtableAddTextBox.Focus();
            } else if (hashtableEditState == 1) {
                // Set the value.
                hashtableListView.SelectedItems[0].SubItems[1].Text = hashtableAddTextBox.Text;
                hashtableListView.SelectedItems[0].SubItems[1].Tag = hashtableAddTextBox.Text;

                hashtableAddPanel.Location = hashtableListView.SelectedItems[0].SubItems[0].Bounds.Location;
                hashtableAddPanel.Location += new Size(hashtableListView.Location);
                hashtableAddPanel.Width = hashtableListView.Columns[0].Width;
                hashtableAddTextBox.Text = hashtableListView.SelectedItems[0].SubItems[0].Text;

                hashtableEditState = 0;
                hashtableAddTextBox.Focus();
                hashtableAddTextBox.SelectAll();
            } else if (hashtableEditState == 2) {
                hashtableAddExpiryPanel.Hide();
                hashtableAddTextBox.Show();
                hashtableAddPanel.ColumnStyles[1].Width = 100;
                hashtableAddPanel.ColumnStyles[2].Width =   0;

                hashtableAddPanel.Location = hashtableListView.SelectedItems[0].SubItems[1].Bounds.Location;
                hashtableAddPanel.Location += new Size(hashtableListView.Location);
                hashtableAddPanel.Width = hashtableListView.Columns[1].Width;
                hashtableAddTextBox.Text = hashtableListView.SelectedItems[0].SubItems[1].Text;

                hashtableEditState = 1;
                hashtableAddTextBox.Focus();
                hashtableAddTextBox.SelectAll();
            }
        }

        private void hashtableAddExpiryTimeBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.S) {
                hashtableAddExpiryUnitBox.SelectedIndex = 0;
                e.Handled = true;
            } else if (e.KeyCode == Keys.M) {
                hashtableAddExpiryUnitBox.SelectedIndex = 1;
                e.Handled = true;
            } else if (e.KeyCode == Keys.H) {
                hashtableAddExpiryUnitBox.SelectedIndex = 2;
                e.Handled = true;
            } else if (e.KeyCode == Keys.D) {
                hashtableAddExpiryUnitBox.SelectedIndex = 3;
                e.Handled = true;
            }
        }

        private void hashtableAddExpiryTimeBox_ValueChanged(object sender, EventArgs e) {
            // Check the check box if there is a number other than zero in the text box.
            hashtableItemExpiryCheckBox.Checked = (hashtableAddExpiryTimeBox.Value != 0);
        }

        private void hashtableItemExpiryCheckBox_CheckedChanged(object sender, EventArgs e) {
            if (hashtableItemExpiryCheckBox.Checked && hashtableAddExpiryTimeBox.Text == "0") {
                hashtableItemExpiryCheckBox.Checked = false;
                hashtableAddExpiryTimeBox.Focus();
                hashtableAddExpiryTimeBox.Select(0, hashtableAddExpiryTimeBox.Text.Length);
            }
        }

        private void AcceptNewHashtableEntry() {
            if (hashtableListView.SelectedItems[0].SubItems[1].Tag == null) {
                // No value was entered; cancel.
                hashtableListView.SelectedItems[0].SubItems.Clear();
                hashtableListView.SelectedItems[0].Text = "Add new...";
                hashtableListView.SelectedItems[0].ForeColor = SystemColors.GrayText;
                return;
            }

            Hashtable hashtable = (Hashtable) tabControl.SelectedTab.Tag;
            string key = hashtableListView.SelectedItems[0].SubItems[0].Text;
            byte[] value;
            int expiry = (int) hashtableListView.SelectedItems[0].SubItems[2].Tag;

            if (hashtable.SyncTarget == IntPtr.Zero) {
                if (hashtableBinaryViewButton.Checked) {
                    if (hashtableBaseComboBox.SelectedIndex == 1)
                        value = hashtableListView.SelectedItems[0].SubItems[1].Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => byte.Parse(s, System.Globalization.NumberStyles.HexNumber)).ToArray();
                    else
                        value = hashtableListView.SelectedItems[0].SubItems[1].Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => byte.Parse(s)).ToArray();
                } else {
                    value = Encoding.UTF8.GetBytes(hashtableListView.SelectedItems[0].SubItems[1].Text);
                }
                hashtable.content[key] = value;
            } else {
                using (var context = new MircMessenger(hashtable.SyncTarget)) {
                    if (hashtableBinaryViewButton.Checked) {
                        if (hashtableBaseComboBox.SelectedIndex == 1)
                            value = hashtableListView.SelectedItems[0].SubItems[1].Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => byte.Parse(s, System.Globalization.NumberStyles.HexNumber)).ToArray();
                        else
                            value = hashtableListView.SelectedItems[0].SubItems[1].Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => byte.Parse(s)).ToArray();
                        context.Run("//bset &value 1 " + string.Join(" ", value) + " | hadd -b" + (expiry != 0 ? " -u" + expiry : "") + " " + hashtable.Name + " " + key + " &value");
                    } else {
                        context.Run("/hadd" + (expiry != 0 ? " -u" + expiry : "") + " " + hashtable.Name + " " + key + " " + hashtableListView.SelectedItems[0].SubItems[1].Text);
                    }
                }
            }

            refreshHashtable();
        }

        private void hashtableAddPanel_Leave(object sender, EventArgs e) {
            if (hashtableEditState == -1) return;
            hashtableEditState = -1;
            AcceptNewHashtableEntry();
            hashtableAddPanel.Hide();
        }

        private void flowLayoutPanel1_Leave(object sender, EventArgs e) {
            if (hashtableEditState == -1) return;

            if (!hashtableItemExpiryCheckBox.Checked || hashtableAddExpiryTimeBox.Value == 0) {
                hashtableListView.SelectedItems[0].SubItems[2].Tag = 0;
                hashtableListView.SelectedItems[0].SubItems[2].Text = "Never";
            } else {
                int unit = 1;

                switch (hashtableAddExpiryUnitBox.SelectedIndex) {
                    case 0: unit =     1; break;
                    case 1: unit =    60; break;
                    case 2: unit =  3600; break;
                    case 3: unit = 86400; break;
                }

                hashtableListView.SelectedItems[0].SubItems[2].Tag = (int) hashtableAddExpiryTimeBox.Value * unit;
                hashtableListView.SelectedItems[0].SubItems[2].Text = "In " + hashtableAddExpiryTimeBox.Value + " " + hashtableAddExpiryUnitBox.Text;
            }
        }

        private void hashtableAddExpiryTimeBox_TextChanged(object sender, EventArgs e) {
            // Check the check box if there is a number other than zero in the text box.
            foreach (char c in hashtableAddExpiryTimeBox.Text) {
                if (c >= '1' && c <= '9') {
                    hashtableItemExpiryCheckBox.Checked = true;
                    return;
                }
            }
            hashtableItemExpiryCheckBox.Checked = false;
        }

        private void hashtableItemNameTextBox_Leave(object sender, EventArgs e) {
            if (hashtableEditState == 0) {
                // Validation has already been done before this event.
                hashtableListView.SelectedItems[0].SubItems[0].Text = hashtableAddTextBox.Text;
            } else if (hashtableEditState == 1) {
                hashtableListView.SelectedItems[0].SubItems[1].Text = hashtableAddTextBox.Text;
            }
        }

        private void hashtableItemNameTextBox_Validating(object sender, CancelEventArgs e) {
            if (hashtableEditState == 0 && hashtableAddTextBox.TextLength == 0) {
                hashtableAddTextBox.Focus();
                hashtableAddTextBox.SelectAll();

                errorBalloon.Show("The key cannot be empty.", hashtableAddTextBox, 5000);
                e.Cancel = true;
                return;
            }
        }

        private void hashtableAddExpiryPanel_Layout(object sender, LayoutEventArgs e) {
            if (hashtableAddExpiryPanel.Width >= 139) {
                if (hashtableAddExpiryUnitBox.Width < 60) {
                    hashtableAddExpiryUnitBox.Width = 60;
                    hashtableAddExpiryUnitBox.Items[0] = "seconds";
                    hashtableAddExpiryUnitBox.Items[1] = "minutes";
                    hashtableAddExpiryUnitBox.Items[2] = "hours";
                    hashtableAddExpiryUnitBox.Items[3] = "days";
                }
            } else {
                if (hashtableAddExpiryUnitBox.Width > 46) {
                    hashtableAddExpiryUnitBox.Width = 46;
                    hashtableAddExpiryUnitBox.Items[0] = "sec";
                    hashtableAddExpiryUnitBox.Items[1] = "min";
                    hashtableAddExpiryUnitBox.Items[2] = "hr";
                    hashtableAddExpiryUnitBox.Items[3] = "days";
                }
            }
            if (hashtableAddExpiryPanel.Width >= 126) {
                if (hashtableItemExpiryCheckBox.Width < 35) {
                    hashtableItemExpiryCheckBox.Text = "In";
                    //hashtableItemExpiryCheckBox.Width = 35;
                }
            } else {
                if (hashtableAddExpiryUnitBox.Width > 15) {
                    hashtableItemExpiryCheckBox.Text = "";
                    //hashtableItemExpiryCheckBox.Width = 15;
                }
            }
        }

        private void newHashtableToolStripMenuItem_Click(object sender, EventArgs e) {
            Hashtable hashtable = new Hashtable("table" + (hashtables.Count + 1), new Dictionary<string, byte[]>(), null, HashtableFormat.Text, false, IntPtr.Zero);

            // Open the hashtable.
            TabPage newTab = new TabPage(hashtable.Name) { Tag = hashtable, ImageIndex = 7 };
            hashtables.Add(hashtable);
            addTabPage(newTab);

            tabControl.SelectTab(newTab);
        }

        private void loadInToolStripMenuItem_DropDownOpening(object sender, EventArgs e) {
            loadInToolStripMenuItem.DropDownItems.Clear();

            if (treeView.Nodes.Count == 0) {
                loadInToolStripMenuItem.DropDownItems.Add(noRunningMIRCInstancesToolStripMenuItem);
            } else {
                foreach (TreeNode node in treeView.Nodes) {
                    if (node.Tag is Tuple<IntPtr, uint>) {
                        var item = new ToolStripMenuItem(node.Text, null, new EventHandler(this.loadInInstanceToolStripMenuItem_Click));
                        item.ForeColor = loadInToolStripMenuItem.ForeColor;
                        item.Tag = ((Tuple<IntPtr, uint>) node.Tag).Item1;
                        loadInToolStripMenuItem.DropDownItems.Add(item);
                    }
                }
            }
        }

        void loadInInstanceToolStripMenuItem_Click(object sender, EventArgs e) {
            if (tabControl.SelectedTab.ImageIndex == 7) {
                Hashtable hashtable = (Hashtable) tabControl.SelectedTab.Tag;
                hashtable.SyncTarget = (IntPtr) ((ToolStripMenuItem) sender).Tag;
                hashtable.Save();
            } else {
                MslDocument document = (MslDocument) tabControl.SelectedTab.Tag;
                document.SyncTarget = (IntPtr) ((ToolStripMenuItem) sender).Tag;    
                saveAndSync(document);
            }
        }

        private void newRemoteScriptToolStripMenuItem_Click(object sender, EventArgs e) {
            newDocument(DocumentType.RemoteScript, "script" + this.documents.Count + ".mrc", 0);
        }

        private void newAliasScriptToolStripMenuItem_Click(object sender, EventArgs e) {
            newDocument(DocumentType.AliasScript, "aliases" + this.documents.Count + ".als", 1);
        }

        private void newPopupScriptToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void newINIFileToolStripMenuItem_Click(object sender, EventArgs e) {
            newDocument(DocumentType.INI, "file" + this.documents.Count + ".ini", 6);
        }

        private void newDocument(DocumentType type, string defaultName, int imageIndex) {
            MslDocument document = new MslDocument(type, defaultName, false, false);
            TabPage tabPage = newTabDocument(document);
            this.documents.Add(document);
            addTabPage(tabPage);
            tabPage.Select();
            tabControl.SelectTab(tabPage);
        }

        private void addTabPage(TabPage tabPage) {
            tabControl.TabPages.Add(tabPage);
            if (tabControl.TabCount == 1) {
                // We must manually fire the events, because they don't work if the control was empty.
                tabControl_Selecting(this, new TabControlCancelEventArgs(tabPage, 0, false, TabControlAction.Selecting));
                tabControl_Selected(this, new TabControlEventArgs(tabPage, 0, TabControlAction.Selected));
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            // TODO: Fix the read only check box.
            if (openDialog.ShowDialog(this) == DialogResult.OK)
                OpenFile(openDialog.FileName, false, IntPtr.Zero);
        }

        private void listView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e) {
            if (e.Bounds.Width == 0 || e.Bounds.Height == 0) return;

            // Draw the background.
            Brush brush = new LinearGradientBrush(e.Bounds, Color.FromArgb(unchecked((int) 0xFF303030)), Color.FromArgb(unchecked((int) 0xFF505050)),
                LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(brush, e.Bounds);

            // Draw the border.
            e.Graphics.DrawLine(Pens.Black, new Point(e.Bounds.Right - 2, e.Bounds.Top), new Point(e.Bounds.Right - 2, e.Bounds.Bottom));
            e.Graphics.DrawLine(Pens.Gray, new Point(e.Bounds.Right - 1, e.Bounds.Top), new Point(e.Bounds.Right - 1, e.Bounds.Bottom));

            // Draw the text.
            TextRenderer.DrawText(e.Graphics, e.Header.Text, e.Font, e.Bounds, Color.White, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);

            return;
        }

        private void listView_DrawItem(object sender, DrawListViewItemEventArgs e) {
            if ((e.State & ListViewItemStates.Selected) != 0) {
                // Draw the background and focus rectangle for a selected item.
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 64, 64)), e.Bounds);
            }

            var stateImage = e.Item.ListView.StateImageList?.Images[e.Item.StateImageIndex];
            if (stateImage != null)
                e.Graphics.DrawImage(stateImage, e.Bounds.Location);
        }

        private void listView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e) {
            if (e.SubItem.ForeColor == SystemColors.GrayText)
                TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.Item.Font, e.Bounds, Color.Gray, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
            else
                TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.Item.Font, e.Bounds, Color.White, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
        }

        public void CloseDocument() {
            int i = tabControl.SelectedIndex;

            // Close the tab.
            tabControl.TabPages.Remove(tabControl.SelectedTab);
            if (currentItem is MslDocument)
                documents.Remove((MslDocument) currentItem);
            else if (currentItem is Hashtable)
                hashtables.Remove((Hashtable) currentItem);
            else if (currentItem is MslDialog)
                dialogs.Remove((MslDialog) currentItem);

            // Select the tab to the right.
            tabControl.SelectedIndex = i;
        }

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (currentItem != null) {
                // Ask if the user wants to save the file.
                if (currentItem is MslDocument && ((MslDocument) currentItem).Modified) {
                    switch (MessageBox.Show("Will you save your changes to the file?", "MScripter", MessageBoxButtons.YesNoCancel)) {
                        case DialogResult.Yes:
                            saveToolStripMenuItem_Click(sender, e);
                            break;
                        case DialogResult.Cancel:
                            return;
                    }
                }

                CloseDocument();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e) {
            if (currentItem is MslDocument)
                ((MslDocument) currentItem).SelectedTextBox.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
            if (currentItem is MslDocument)
                ((MslDocument) currentItem).SelectedTextBox.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) {
            if (currentItem is MslDocument)
                ((MslDocument) currentItem).SelectedTextBox.Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            if (currentItem is MslDocument)
                ((MslDocument) currentItem).SelectedTextBox.SelectAll();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e) {
            if (currentItem is MslDocument)
                ((MslDocument) currentItem).SelectedTextBox.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e) {
            if (currentItem is MslDocument)
                ((MslDocument) currentItem).SelectedTextBox.Redo();
        }

        private void functionList_SelectedIndexChanged(object sender, EventArgs e) {
            if (surpressSelectionEvents) return;
            surpressSelectionEvents = true;
            var currentDocument = (MslDocument) currentItem;
            MslBookmark bookmark = (MslBookmark) functionList.SelectedItem;
            currentDocument.SelectedTextBox.Selection = new Range(currentDocument.SelectedTextBox, 0, bookmark.position, 0, bookmark.position);
            currentDocument.SelectedTextBox.DoSelectionVisible();
            currentDocument.SelectedTextBox.Select();
            surpressSelectionEvents = false;
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e) {
            if (e.TabPage == null) return;

            e.TabPage.ResumeLayout(true);
            if (currentItem is MslDocument) ((MslDocument) currentItem).SelectedTextBox.Select();
            else if (currentItem is Hashtable) hashtableListView.Select();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.N && e.Control) {
                e.Handled = true;
                newToolStripButton.ShowDropDown();
                newRemoteScriptToolStripMenuItem.Select();
            }
        }

        private void dialogDesignerToolStripMenuItem_Click(object sender, EventArgs e) {
            var currentDocument = (MslDocument) currentItem;
            var lineInfoTable = currentDocument.lineInfoTable;
            var builder = new StringBuilder();

            int lineNumber = (int) dialogDesignerToolStripMenuItem.Tag;
            Match m = Regex.Match(currentDocument.TextBox[lineNumber].Text, "^ *dialog +([^ ]*)", RegexOptions.IgnoreCase);
            string name = m.Groups[1].Value;

            MslBookmark bookmark = new MslBookmark(BookmarkType.Dialog, name, lineNumber) { hidden = true };
            currentDocument.bookmarks.Add(bookmark);

            while (lineNumber < currentDocument.TextBox.LinesCount) {
                builder.AppendLine(currentDocument.TextBox[lineNumber].Text);

                var lineInfo = lineInfoTable[currentDocument.TextBox[lineNumber].UniqueId];
                if (lineInfo.BraceLevel == 0) break;
                ++lineNumber;
            }

            TabPage tabPage = null; DialogDesigner designer = null;
            foreach (TabPage tabPage2 in tabControl.TabPages) {
                if (tabPage2.ImageIndex == 9) {
                    MslDialog dialog = (MslDialog) tabPage2.Tag;
                    if (dialog.Document == currentDocument && dialog.Name == name) {
                        tabPage = tabPage2;
                        designer = dialog.designer;
                    }
                }
            }
            if (tabPage == null) {
                MslDialog dialog = new MslDialog(name, currentDocument, bookmark);
                tabPage = newTabDialog(dialog);
                designer = dialog.designer;
                tabControl.TabPages.Add(tabPage);
            }
            
            designer.Text = builder.ToString();
            tabControl.SelectTab(tabPage);
        }

        private void viewToolStripMenuItem_DropDownOpening(object sender, EventArgs e) {
            if (currentItem is MslDocument) {
                var currentDocument = (MslDocument) currentItem;

                // Search upward for the start of the dialog block.
                var lineInfoTable = currentDocument.lineInfoTable;
                int lineNumber;
                for (lineNumber = currentDocument.TextBox.Selection.Start.iLine; lineNumber >= 0; --lineNumber) {
                    LineInfo lineInfo;
                    if (lineInfoTable.TryGetValue(currentDocument.TextBox[lineNumber].UniqueId, out lineInfo) && lineInfo?.BraceLevel == 0) break;
                }
                ++lineNumber;

                if (lineNumber < currentDocument.TextBox.LinesCount && Regex.IsMatch(currentDocument.TextBox[lineNumber].Text, "^ *dialog ", RegexOptions.IgnoreCase)) {
                    dialogDesignerToolStripMenuItem.Tag = lineNumber;
                    dialogDesignerToolStripMenuItem.Enabled = true;
                    dialogDesignerToolStripMenuItem.ToolTipText = null;
                } else {
                    dialogDesignerToolStripMenuItem.Enabled = false;
                    dialogDesignerToolStripMenuItem.ToolTipText = "The cursor must be on a dialog block to do that.";
                }
            } else {
                dialogDesignerToolStripMenuItem.Enabled = false;
                dialogDesignerToolStripMenuItem.ToolTipText = "The cursor must be on a dialog block to do that.";
            }
        }

        private void newDialogToolStripMenuItem_Click(object sender, EventArgs e) {
            MslDialog dialog = new MslDialog("dialog" + this.dialogs.Count);
            this.dialogs.Add(dialog);
            TabPage tabPage = newTabDialog(dialog);
            addTabPage(tabPage);
            tabControl.SelectTab(tabPage);
        }

        private void dialogDesigner_ViewCode(object sender, EventArgs e) {
            var currentDialog = (MslDialog) currentItem;

            if (currentDialog.Document != null) {
                foreach (TabPage tabPage in tabControl.TabPages) {
                    if (tabPage.Tag == currentDialog.Document) {
                        tabControl.SelectTab(tabPage);
                        return;
                    }
                }
            } else {
                currentDialog.Bookmark = new MslBookmark(BookmarkType.Dialog, currentDialog.Name, 0) { hidden = true };
                currentDialog.Document = new MslDocument(DocumentType.RemoteScript);
                currentDialog.Document.bookmarks.Add(currentDialog.Bookmark);
                this.documents.Add(currentDialog.Document);
            }

            TabPage tabPage2 = newTabDocument(currentDialog.Document);
            tabControl.TabPages.Add(tabPage2);
            tabControl.SelectTab(tabPage2);
        }

        private void UpdateDialogCode() {
            if (!(currentItem is MslDialog)) throw new InvalidOperationException("The selected item is not a dialog.");

            var currentDialog = (MslDialog) currentItem;
            if (currentDialog.Document == null) return;

            if (string.IsNullOrEmpty(currentDialog.Document.Text)) {
                currentDialog.Document.TextBox.Text = currentDialog.designer.Text;
                return;
            }

            // Sanity check.
            if (!currentDialog.Document.TextBox[currentDialog.Bookmark.position].Text.Trim().Equals("dialog " + currentDialog.Name + " {", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Dialog block was changed in an unexpected way.");

            var lineInfoTable = currentDialog.Document.lineInfoTable;
            int lineNumber = currentDialog.Bookmark.position;
            do {
                ++lineNumber;
                if (lineNumber >= currentDialog.Document.TextBox.LinesCount) {
                    --lineNumber;
                    break;
                }
            } while (lineInfoTable[currentDialog.Document.TextBox[lineNumber].UniqueId].BraceLevel != 0);

            // Replace the block.
            currentDialog.Document.TextBox.Selection = new Range(currentDialog.Document.TextBox, 0, currentDialog.Bookmark.position, currentDialog.Document.TextBox[lineNumber].Count, lineNumber);
            currentDialog.Document.TextBox.ClearSelected();
            currentDialog.Document.TextBox.InsertText(currentDialog.designer.Text);
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                if (e.Node.ImageIndex == 0) {
                    var tag = (Tuple<IntPtr, uint>) e.Node.Tag;
                    mIRCContextMenu.Tag = tag;
                    mIRCContextMenu.Show();
                }
            }
        }

        private void identifyToolStripMenuItem_Click(object sender, EventArgs e) {
            var data = (Tuple<IntPtr, uint>) mIRCContextMenu.Tag;
            using (var context = new MircMessenger(data.Item1))
                context.Run("//noop $tip(identify, Identify, Here I am!, $null, $null, $null, showmirc -s, $null) | flash");
        }

        private void openDataDirectoryToolStripMenuItem_Click(object sender, EventArgs e) {
            string directory;
            var data = (Tuple<IntPtr, uint>) mIRCContextMenu.Tag;
            using (var context = new MircMessenger(data.Item1))
                directory = context.Evaluate("$mircdir");

            var process = new Process() { StartInfo = new ProcessStartInfo(directory) };
            process.Start();
        }

        private void openScriptDirectoryToolStripMenuItem_Click(object sender, EventArgs e) {
            string directory;
            var data = (Tuple<IntPtr, uint>) mIRCContextMenu.Tag;
            using (var context = new MircMessenger(data.Item1))
                directory = context.Evaluate("$scriptdir");

            var process = new Process() { StartInfo = new ProcessStartInfo(directory) };
            process.Start();
        }

        private void functionList_DrawItem(object sender, DrawItemEventArgs e) {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index != -1) {
                MslBookmark bookmark = (MslBookmark) functionList.Items[e.Index];
                switch (bookmark.type) {
                    case BookmarkType.Event     : e.Graphics.DrawString("on"     , functionList.Font, Brushes.Red      , e.Bounds); break;
                    case BookmarkType.Alias     : e.Graphics.DrawString("alias"  , functionList.Font, Brushes.RoyalBlue, e.Bounds); break;
                    case BookmarkType.CtcpEvent : e.Graphics.DrawString("ctcp"   , functionList.Font, Brushes.Purple   , e.Bounds); break;
                    case BookmarkType.RawEvent  : e.Graphics.DrawString("raw"    , functionList.Font, Brushes.Olive    , e.Bounds); break;
                    case BookmarkType.Dialog    : e.Graphics.DrawString("dialog" , functionList.Font, Brushes.Teal     , e.Bounds); break;
                    case BookmarkType.Menu      : e.Graphics.DrawString("menu"   , functionList.Font, Brushes.Green    , e.Bounds); break;
                    case BookmarkType.IniSection: e.Graphics.DrawString("section", functionList.Font, Brushes.RoyalBlue, e.Bounds); break;
                }
                e.Graphics.DrawString(bookmark.title, functionList.Font, new SolidBrush(functionList.ForeColor), 40, e.Bounds.Top);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            Program.SaveConfig();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.All;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                foreach (string file in (string[]) e.Data.GetData(DataFormats.FileDrop)) {
                    if (!File.Exists(file)) {
                        MessageBox.Show($"Failed to open {file}:\nFile not found.", "mIDE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    try {
                        OpenFile(file, false, IntPtr.Zero);
                    } catch (IOException ex) {
                        MessageBox.Show($"Failed to open {file}:\n{ex.Message}", "mIDE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openDirectoryDialog.ShowDialog(this) != DialogResult.OK) return;

            var node = new TreeNode(Path.GetFileName(openDirectoryDialog.FileName), 7, 7) { Tag = openDirectoryDialog.FileName };
            node.Nodes.Add("dummy");  // This allows the node to be expanded.
            treeView.Nodes.Add(node);
        }

        private void revertToolStripMenuItem_Click(object sender, EventArgs e) {
            if (currentItem != null) {
                // Ask if the user wants to save the file.
                if (currentItem is MslDocument && ((MslDocument) currentItem).Modified) {
                    if (MessageBox.Show("Are you sure you want to discard your changes?", "MScripter", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    }
                }
            }
        }

        private void restyleLineToolStripMenuItem_Click(object sender, EventArgs e) {
            if (currentItem is MslDocument)
                mSLSyntaxHighlight(((MslDocument) currentItem).SelectedTextBox, ((MslDocument) currentItem).SelectedTextBox.Selection, ((MslDocument) currentItem).Type);
        }

        private void errorsToolStripButton_CheckStateChanged(object sender, EventArgs e) {
            RebuildErrorList();
        }

        private void SetColours(Color background, Color background2, Color background3, Color foreground) {
            this.BackColor = background3;
            toolStripContainer1.TopToolStripPanel.BackColor = background3;
            toolStripContainer1.BottomToolStripPanel.BackColor = background3;
            toolStripContainer1.LeftToolStripPanel.BackColor = background3;
            toolStripContainer1.RightToolStripPanel.BackColor = background3;
            toolStripContainer1.ContentPanel.BackColor = background3;
            splitContainer2.Panel1.BackColor = background3;
            hashtableHeaderPanel.BackColor = background2;
            hashtableAddPanel.BackColor = background2;
            functionList.BackColor = background2;
            parsingProgressBarPanel.BackColor = background2;

            hashtableListView.BackColor = background2;
            treeView.BackColor = background2;
            hashtableAddTextBox.BackColor = background2;
            hashtableAddExpiryTimeBox.BackColor = background2;
            errorList.BackColor = background2;

            this.ForeColor = foreground;
            toolStripContainer1.TopToolStripPanel.ForeColor = foreground;
            toolStripContainer1.BottomToolStripPanel.ForeColor = foreground;
            toolStripContainer1.LeftToolStripPanel.ForeColor = foreground;
            toolStripContainer1.RightToolStripPanel.ForeColor = foreground;
            toolStripContainer1.ContentPanel.ForeColor = foreground;
            splitContainer2.Panel1.ForeColor = foreground;
            treeView.ForeColor = foreground;
            consoleTextBox.ForeColor = foreground;
            hashtableListView.ForeColor = foreground;
            hashtableAddExpiryTimeBox.ForeColor = foreground;
            hashtableAddTextBox.ForeColor = foreground;
            hashtableItemExpiryCheckBox.ForeColor = foreground;
            errorList.ForeColor = foreground;
            toolbar.ForeColor = foreground;
            //errorListToolbar.ForeColor = foreground;
            menuBar.ForeColor = foreground;
            functionList.ForeColor = foreground;
            parsingProgressBarPanel.ForeColor = foreground;

            foreach (MslDocument document in documents) {
                foreach (var textBox in document.textBoxes.Where(box => box != null)) {
                    textBox.BackColor = background2;
                    textBox.ForeColor = foreground;
                }
            }

            SetMenuColours(menuBar.Items, foreground);
            SetMenuColours(fileContextMenu.Items, foreground);
        }

        private void SetMenuColours(ToolStripItemCollection items, Color foreground) {
            foreach (ToolStripItem item in items) {
                item.ForeColor = foreground;
                if (item is ToolStripMenuItem) SetMenuColours(((ToolStripMenuItem) item).DropDownItems, foreground);
            }
        }

        internal void applyConfig(Config config) {
            applyTheme();

            foreach (var document in this.documents) {
                foreach (var textBox in document.textBoxes.Where(box => box != null)) {
                    textBox.ShowLineNumbers = config.ShowLineNumbers;
                    textBox.TabLength = config.PreferredIndentationCount;
                }
            }

            if (config.PreferredAliasExtension == "mrc" || config.PreferredAliasExtension == "") {
                openDialog.Filter = "mIRC script files|*.mrc;*.ini|Hashtables (text)|*|Hashtables (binary)|*|Hashtables (INI)|*.ini|All files|*";
            } else {
                openDialog.Filter = "mIRC script files|*.mrc;*." + config.PreferredAliasExtension + ";*.ini|Hashtables (text)|*|Hashtables (binary)|*|Hashtables (INI)|*.ini|All files|*";
            }

            if (config.PreferredAliasExtension == "") {
                saveDialog.Filter = "mIRC remote script files (*.mrc)|*.mrc|mIRC alias script files (*)|*|INI files (*.ini)|*.ini|All files|*";
            } else {
                saveDialog.Filter = "mIRC remote script files (*.mrc)|*.mrc|mIRC alias script files (*." + config.PreferredAliasExtension + ")|*." + config.PreferredAliasExtension + "|INI files (*.ini)|*.ini|All files|*";
            }
        }

        internal void applyTheme() {
            Color foregroundColour; Color backgroundColour; Color backgroundColour2; Color backgroundColour3;

            standardThemeToolStripMenuItem.Checked = false;
            darkThemeToolStripMenuItem.Checked = false;

            foreach (MslDialog dialog in dialogs)
                dialog.designer.Theme = Program.Config.Theme;

            switch (Program.Config.Theme) {
                case Theme.Default:
                    standardThemeToolStripMenuItem.Checked = true;

                    foregroundColour = SystemColors.WindowText;
                    backgroundColour = SystemColors.Window;
                    backgroundColour2 = SystemColors.Window;
                    backgroundColour3 = SystemColors.Control;
                    
                    foreach (MslDocument document in documents) {
                        foreach (var textBox in document.textBoxes.Where(box => box != null)) {
                            textBox.BackColor = Color.White;
                            textBox.CaretColor = Color.Black;
                            textBox.FoldingIndicatorColor = Color.Green;
                            textBox.ForeColor = Color.Black;
                            textBox.IndentBackColor = Color.WhiteSmoke;
                            textBox.LineNumberColor = Color.Teal;
                            textBox.PaddingBackColor = Color.Transparent;
                            textBox.SelectionColor = Color.FromArgb(60, 0, 0);
                            textBox.ServiceLinesColor = Color.Silver;
                        }
                    }

                    hashtableListView.OwnerDraw = false;
                    errorList.OwnerDraw = false;

                    menuBar.Renderer = null;
                    toolbar.Renderer = null;
                    errorListToolbar.Renderer = null;
                    newMenu.Renderer = null;
                    fileContextMenu.Renderer = null;

                    break;
                case Theme.Dark:
                    darkThemeToolStripMenuItem.Checked = true;

                    foregroundColour = Color.White;
                    backgroundColour = Color.FromArgb(32, 32, 32);
                    backgroundColour2 = Color.FromArgb(32, 32, 32);
                    backgroundColour3 = Color.FromArgb(32, 32, 32);

                    hashtableListView.OwnerDraw = true;
                    errorList.OwnerDraw = true;

                    menuBar.Renderer = new ToolStripProfessionalRenderer(new DarkThemeColorTable());
                    toolbar.Renderer = new ToolStripProfessionalRenderer(new DarkThemeColorTable());
                    errorListToolbar.Renderer = new ToolStripProfessionalRenderer(new DarkThemeColorTable());
                    newMenu.Renderer = new ToolStripProfessionalRenderer(new DarkThemeColorTable());
                    fileContextMenu.Renderer = new ToolStripProfessionalRenderer(new DarkThemeColorTable());

                    break;
                default:
                    throw new ArgumentOutOfRangeException("theme");
            }

            this.SetColours(backgroundColour, backgroundColour2, backgroundColour3, foregroundColour);

            foreach (MslDocument document in documents) {
                foreach (var textBox in document.textBoxes.Where(box => box != null)) {
                    applyTheme(textBox);
                }
            }
            applyTheme(consoleTextBox);
        }
        internal static void applyTheme(FastColoredTextBox textBox) {
            switch (Program.Config.Theme) {
                case Theme.Default:
                    textBox.BackColor = Color.White;
                    textBox.CaretColor = Color.Black;
                    textBox.FoldingIndicatorColor = Color.Green;
                    textBox.ForeColor = Color.Black;
                    textBox.IndentBackColor = Color.WhiteSmoke;
                    textBox.LineNumberColor = Color.Teal;
                    textBox.PaddingBackColor = Color.Transparent;
                    textBox.SelectionColor = Color.FromArgb(60, 0, 0);
                    textBox.ServiceLinesColor = Color.Silver;
                    break;
                case Theme.Dark:
                    textBox.BackColor = Color.FromArgb(32, 32, 32);
                    textBox.CaretColor = Color.White;
                    textBox.FoldingIndicatorColor = Color.Gold;
                    textBox.ForeColor = Color.White;
                    textBox.IndentBackColor = Color.FromArgb(32, 32, 32);
                    textBox.LineNumberColor = Color.Teal;
                    textBox.PaddingBackColor = Color.FromArgb(32, 32, 32);
                    textBox.SelectionColor = Color.FromArgb(150, Color.White);
                    textBox.ServiceLinesColor = Color.DimGray;
                    break;
            }
        }

        private void standardThemeToolStripMenuItem_Click(object sender, EventArgs e) {
            Program.Config.Theme = Theme.Default;
            applyTheme();
        }

        private void darkThemeToolStripMenuItem_Click(object sender, EventArgs e) {
            Program.Config.Theme = Theme.Dark;
            applyTheme();
        }

        private void textFileToolStripMenuItem_Click(object sender, EventArgs e) {
            MslDocument file = (MslDocument) ((TabPage) fileContextMenu.Tag).Tag;
            file.Type = DocumentType.Text;
            FormatFile(file);
        }

        private void iNIFileToolStripMenuItem_Click(object sender, EventArgs e) {
            MslDocument file = (MslDocument) ((TabPage) fileContextMenu.Tag).Tag;
            file.Type = DocumentType.INI;
            FormatFile(file);
        }

        private void errorListToolbar_EndDrag(object sender, EventArgs e) {
            if (errorListToolbar.Parent.Parent != toolStripContainer2) {
                if (errorListToolbar.Parent == toolStripContainer1.TopToolStripPanel)
                    errorListToolbar.Parent = toolStripContainer2.TopToolStripPanel;
                else if (errorListToolbar.Parent == toolStripContainer1.BottomToolStripPanel)
                    errorListToolbar.Parent = toolStripContainer2.BottomToolStripPanel;
                else if (errorListToolbar.Parent == toolStripContainer1.LeftToolStripPanel)
                    errorListToolbar.Parent = toolStripContainer2.LeftToolStripPanel;
                else if (errorListToolbar.Parent == toolStripContainer1.RightToolStripPanel)
                    errorListToolbar.Parent = toolStripContainer2.RightToolStripPanel;
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) {
            var optionsDialog = new OptionsForm();
            optionsDialog.ShowDialog();
        }

        private void foldallToolStripMenuItem_Click(object sender, EventArgs e) {
            var currentDocument = currentItem as MslDocument;
            currentDocument?.SelectedTextBox.CollapseAllFoldingBlocks();
        }

        private void unfoldAllToolStripMenuItem_Click(object sender, EventArgs e) {
            var currentDocument = currentItem as MslDocument;
            currentDocument?.SelectedTextBox.ExpandAllFoldingBlocks();
        }

        private void foldToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!(currentItem is MslDocument)) return;
            var currentDocument = (MslDocument) currentItem;
            // Find the start of the folding block.
            for (int i = currentDocument.TextBox.Selection.FromLine; i >= 0; --i) {
                if (!string.IsNullOrEmpty(currentDocument.TextBox[i].FoldingStartMarker)) {
                    currentDocument.SelectedTextBox.CollapseFoldingBlock(i);
                    break;
                }
            }
        }

        private void unfoldToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!(currentItem is MslDocument)) return;
            var currentDocument = (MslDocument) currentItem;
            // Find the start of the folding block.
            for (int i = currentDocument.TextBox.Selection.FromLine; i >= 0; --i) {
                if (!string.IsNullOrEmpty(currentDocument.TextBox[i].FoldingStartMarker)) {
                    currentDocument.SelectedTextBox.ExpandFoldedBlock(i);
                    break;
                }
            }
        }

        private void documentContextMenu_Opening(object sender, CancelEventArgs e) {
            if (!(currentItem is MslDocument)) {
                goTodefinitionToolStripMenuItem.Enabled = false;
                return;
            }
            var currentDocument = (MslDocument) currentItem;
            var lineInfoTable = currentDocument.lineInfoTable;
            int lineNumber = currentDocument.SelectedTextBox.Selection.Start.iLine;
            if (currentDocument.SelectedTextBox.Selection.End.iLine == lineNumber) {
                LineInfo lineInfo;
                if (lineInfoTable.TryGetValue(currentDocument.TextBox[lineNumber].UniqueId, out lineInfo)) {
                    foreach (var tag in lineInfo.tags) {
                        if ((tag.type == TagType.CommandCall || tag.type == TagType.FunctionCall) &&
                            tag.range.Contains(currentDocument.SelectedTextBox.Selection.Start) && tag.range.Contains(currentDocument.SelectedTextBox.Selection.End)) {
                            goTodefinitionToolStripMenuItem.Tag = tag;
                            goTodefinitionToolStripMenuItem.Text = "Go to &definition [" + tag.range.Text + "]";
                            goTodefinitionToolStripMenuItem.Enabled = true;
                            return;
                        }
                    }
                }
            }

            goTodefinitionToolStripMenuItem.Text = "Go to &definition";
            goTodefinitionToolStripMenuItem.Enabled = false;
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void goTodefinitionToolStripMenuItem_Click(object sender, EventArgs e) {
            string name = ((Tag) goTodefinitionToolStripMenuItem.Tag).range.Text.Substring(1);
            foreach (var document in this.documents) {
                foreach (var bookmark in document.bookmarks) {
                    if (bookmark.type == BookmarkType.Alias && bookmark.title == name) {
                        foreach (TabPage tabPage in tabControl.TabPages) {
                            if (tabPage.Tag == document) {
                                tabControl.SelectTab(tabPage);
                                return;
                            }
                        }

                        TabPage tabPage2 = newTabDocument(document);
                        tabControl.TabPages.Add(tabPage2);
                        tabControl.SelectTab(tabPage2);

                        document.SelectedTextBox.Selection = new Range(document.SelectedTextBox, 0, bookmark.position, 0, bookmark.position);
                        return;
                    }
                }
            }
            MessageBox.Show(this, "The alias was not found.", "MScripter", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private List<Tag> wordHighlights = new List<Tag>();
        private void checkWordHighlight() {
            foreach (var tag in wordHighlights) tag.range.ClearStyle(wordHighlightStyle);
            wordHighlights.Clear();

            var currentDocument = currentItem as MslDocument;
            if (currentDocument == null) return;
            var tag2 = currentDocument.GetSelectedTag(TagType.Variable);
            if (tag2 == null) return;
            string symbol = tag2.range.Text;

            var bookmark = currentDocument.GetBookmark(currentDocument.SelectedTextBox.Selection.Start.iLine);
            if (bookmark == null) return;

            for (int i = bookmark.position; i < currentDocument.TextBox.LinesCount; ++i) {
                var line = currentDocument.GetLineInfo(i);
                if (line == null) continue;
                foreach (var tag in line.tags) {
                    if (tag.type == tag2.type && tag.range.Text == symbol) {
                        wordHighlights.Add(tag);
                        tag.range.SetStyle(wordHighlightStyle);
                    }
                }
                if (line.BraceLevel == 0) return;
            }
        }

        private void splitToolStripMenuItem_Click(object sender, EventArgs e) {
            var document = (MslDocument) currentItem;
            if (document.textBoxes[1] == null) {
                document.textBoxes[1] = createTextBox(document);
                document.textBoxes[1].SourceTextBox = document.textBoxes[0];
            }

            var splitContainer = (SplitContainer) document.TextBox.Parent.Parent;
            splitContainer.Panel2.Controls.Add(document.textBoxes[1]);
            document.textBoxes[1].Dock = DockStyle.Fill;
            splitContainer.Panel2Collapsed = false;
        }

        private void mSLTextBox_Enter(object sender, EventArgs e) {
            var document = (MslDocument) ((FastColoredTextBox) sender).Tag;
            if (document != null) document.SelectedTextBox = (FastColoredTextBox) sender;
        }

        private void debugTextBox_SelectionChangedDelayed(object sender, EventArgs e) {
            debugTextBox.Range.ClearStyle(wordHighlightStyle);

            var lineIndex = debugTextBox.Selection.End.iLine;
            var match = Regex.Match(debugTextBox[lineIndex].Text, @"@ (\d+) - (\d+)$");
            if (match.Success)
                new Range(debugTextBox, int.Parse(match.Groups[1].Value), 1, int.Parse(match.Groups[2].Value), 1).SetStyle(wordHighlightStyle);
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e) {
            var splitter = (SplitContainer) sender;
            var document = (MslDocument) splitter.Tag;

            if (e.SplitY >= splitter.Height - 12) {
                // Close the bottom text box.
                document.textBoxes[1] = null;
                splitter.Panel2.Controls.Clear();
                splitter.Panel2Collapsed = true;
                document.textBoxes[0].Focus();
            } else if (e.SplitY <= 0) {
                // TODO: Close the top text box.
                document.textBoxes[1] = null;
                splitter.Panel2.Controls.Clear();
                splitter.Panel2Collapsed = true;
                document.textBoxes[0].Focus();
            }
        }
    }
}
