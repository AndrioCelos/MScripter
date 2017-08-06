using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;

namespace MScripter {
    public partial class OptionsForm : Form {
        Config config;
        FontStyle style, style2;
        bool busy;

        private List<object> selectedStyles = new List<object>();
        private List<Style> selectedStyles2 = new List<Style>();

        public OptionsForm() {
            InitializeComponent();

            // We clone the configuration here so that the clone can be modified and then cancelled.
            config = Program.Config.Clone();

            treeView.Nodes[0].Tag = generalPanel;
            treeView.Nodes[1].Tag = editorPanel;
            treeView.Nodes[1].Nodes[0].Tag = editorColoursPanel;
            treeView.Nodes[2].Tag = dialogDesignerPanel;

            // Populate the font list.
            foreach (var family in new InstalledFontCollection().Families) {
                // Check that the font is monospace.
                // If 'l' and 'm' are the same width, the font is probably monospace.
                var font = new Font(family, 16, FontStyle.Regular);
                var lWidth = TextRenderer.MeasureText("l", font).Width;
                var mWidth = TextRenderer.MeasureText("m", font).Width;
                if (lWidth == mWidth) {
                    editorFontFamilyComboBox.Items.Add(family);
                    if (config.EditorFont.FontFamily.Name == family.Name)
                        editorFontFamilyComboBox.SelectedIndex = editorFontFamilyComboBox.Items.Count - 1;
                }
            }
            editorFontFamilyComboBox.DisplayMember = "Name";


            this.Size = new Size(540, 405);
        }

        private Panel _currentPage;
        private Panel currentPage {
            get { return _currentPage; }
            set {
                if (currentPage != null) {
                    currentPage.Visible = false;
                }
                if (value != null) {
                    value.Parent = splitContainer1.Panel2;
                    value.Visible = true;
                }
                _currentPage = value;
            }
        }

        private void OptionsForm_Load(object sender, EventArgs e) {
            // Populate the controls.
            themeBox.SelectedIndex = (int) config.Theme;
            preferredAliasExtensionBox.Text = config.PreferredAliasExtension;
            assumeMrcFilesCheckBox.Checked = config.AssumeMrcFilesAreRemote;
            previousAssume = config.AssumeMrcFilesAreRemote;
            mIRCInstanceLabelBox.Text = config.mIRCInstanceLabel;

            showLineNumbersCheckBox.Checked = config.ShowLineNumbers;
            indentationBox.Value = config.PreferredIndentationCount;

            defaultDialogSizeUnitBox.SelectedIndex = (int) config.DefaultDialogSizeUnit;

            // Set up the category list.
            treeView.ExpandAll();
            treeView.SelectedNode = treeView.Nodes[0];
            currentPage = generalPanel;

            // Set up the sample text box.
            applyTheme();
            var document = new MslDocument(DocumentType.RemoteScript, "sample.mrc", false, false);
            editorColoursSampleTextBox.Tag = document;
            document.TextBox = editorColoursSampleTextBox;
            Rehighlight(document);
        }

        internal void applyTheme() {
            switch (config.Theme) {
                case Theme.Default:
                    footerPanel.BackColor = SystemColors.AppWorkspace; footerPanel.ForeColor = SystemColors.ControlText;

                    configurationFileLink.LinkColor = Color.Blue;
                    configurationFileLink.VisitedLinkColor = Color.Purple;
                    break;
                case Theme.Dark:
                    footerPanel.BackColor = Color.FromArgb(64, 64, 64); footerPanel.ForeColor = Color.White;
                    configurationFileLink.LinkColor = Color.RoyalBlue;
                    configurationFileLink.VisitedLinkColor = Color.MediumPurple;
                    break;
            }

            applyTheme(this);
            applyTheme(generalPanel);
            applyTheme(editorPanel);
            applyTheme(editorColoursPanel);
            applyTheme(dialogDesignerPanel);
            applyTheme(groupBox1);

            applyThemeToTextBox(treeView);

            applyThemeToTextBox(preferredAliasExtensionBox);
            applyThemeToTextBox(textBox1);

            applyThemeToTextBox(editorFontSizeBox);
            applyThemeToTextBox(editorFontStyleList);
            applyThemeToTextBox(indentationBox);
            applyThemeToTextBox(editorStyleList);
            applyThemeToTextBox(editorForegroundColourBox);
            applyThemeToTextBox(editorBackgroundColourBox);
            applyThemeToTextBox(editorUnderlineColourBox);
            applyThemeToTextBox(editorFontStyleList2);
            applyThemeToTextBox(defaultDialogWidthBox);
            applyThemeToTextBox(defaultDialogHeightBox);

            config.ApplyTheme(mIRCInstanceLabelBox);
            config.ApplyTheme(editorSampleTextBox);
            config.ApplyTheme(editorColoursSampleTextBox);

            setNormalForegroundColour();
            setNormalBackgroundColour();

            this.Refresh();
        }

        internal void applyTheme(Control control) {
            switch (config.Theme) {
                case Theme.Default:
                    control.ForeColor = SystemColors.ControlText;
                    control.BackColor = SystemColors.Control;
                    break;
                case Theme.Dark:
                    control.ForeColor = Color.White;
                    control.BackColor = Color.FromArgb(32, 32, 32);
                    break;
            }
        }
        internal void applyThemeToTextBox(Control control) {
            switch (config.Theme) {
                case Theme.Default:
                    control.ForeColor = SystemColors.WindowText;
                    control.BackColor = SystemColors.Window;
                    break;
                case Theme.Dark:
                    control.ForeColor = Color.White;
                    control.BackColor = Color.Black;
                    break;
            }
        }

        internal void Rehighlight(MslDocument document) => Rehighlight(document, 0, true);
        internal void Rehighlight(MslDocument document, int startLine) => Rehighlight(document, startLine, false);
        internal void Rehighlight(MslDocument document, int startLine, bool force) {
            for (int i = startLine; i < document.TextBox.LinesCount; ++i) {
                var oldResult = document.GetLineInfo(i);
                var result = MslSyntaxHighlighter.Highlight(document.TextBox, i, document.Type);

                if (!force && result.Matches(oldResult)) break;

                // Skip past continued lines.
                while (i < document.TextBox.LinesCount && document.lineInfoTable.TryGetValue(document.TextBox[i].UniqueId, out result) &&
                       result.State == (byte) MslSyntaxHighlighter.ParseStateIndex.Continuation)
                    ++i;
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e) {
            currentPage = (Panel) e.Node.Tag;
        }

        private void editorFontFamilyComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            SetSampleFont();
        }

        private void button1_Click(object sender, EventArgs e) {
            fontDialog.Font = editorSampleTextBox.Font;
            if (fontDialog.ShowDialog(this) == DialogResult.OK) {
                editorFontFamilyComboBox.SelectedItem = fontDialog.Font.FontFamily;
                editorFontSizeBox.Value = (decimal) fontDialog.Font.Size;

                editorFontStyleList.SetItemChecked(0, fontDialog.Font.Style.HasFlag(FontStyle.Bold));
                editorFontStyleList.SetItemChecked(1, fontDialog.Font.Style.HasFlag(FontStyle.Italic));
                editorFontStyleList.SetItemChecked(2, fontDialog.Font.Style.HasFlag(FontStyle.Underline));
                editorFontStyleList.SetItemChecked(3, fontDialog.Font.Style.HasFlag(FontStyle.Strikeout));
            }
        }

        private void SetSampleFont() {
            var dpi = this.CreateGraphics().DpiY;

            // The font size is rounded to an integral number of pixels.
            float size = (float) editorFontSizeBox.Value * dpi / 72;  // Convert from points (1/72 inches) to pixels.
            size = (float) Math.Round(size);
            size = size * 72 / dpi;  // Convert back to points.

            editorSampleTextBox.Font = new Font((FontFamily) editorFontFamilyComboBox.SelectedItem, size, style);
        }

        private void editorFontSizeBox_ValueChanged(object sender, EventArgs e) {
            SetSampleFont();
        }

        private void editorFontStyleListBox_ItemCheck(object sender, ItemCheckEventArgs e) {
            var styleFlag = (FontStyle) (1 << e.Index);

            if (e.NewValue == CheckState.Checked)
                style |= styleFlag;
            else
                style &= ~styleFlag;

            SetSampleFont();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e) {
            indentationLabel.Text = ((int) indentationBox.Value == 1 ? "space" : "spaces");

            adjustIndentation(editorSampleTextBox);
            adjustIndentation(editorColoursSampleTextBox);
            config.PreferredIndentationCount = (int) indentationBox.Value;
        }

        private void adjustIndentation(FastColoredTextBox textBox) {
            for (int y = 0; y < textBox.LinesCount; ++y) {
                for (int x = 0; x < textBox[y].Count; ++x) {
                    if (textBox[y][x].c != ' ') {
                        var indentLevel = (int) Math.Ceiling((double) x / config.PreferredIndentationCount);
                        if (indentLevel != 0) {
                            textBox.Selection = new Range(textBox, 0, y, x, y);
                            textBox.SelectedText = new string(' ', (int) indentationBox.Value * indentLevel);
                        }
                        break;
                    }
                }
            }
            textBox.Selection = new Range(textBox, 0, 0, 0, 0);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            editorSampleTextBox.ShowLineNumbers = showLineNumbersCheckBox.Checked;
        }

        private void applyButton_Click(object sender, EventArgs e) {
            Program.ApplyConfig(this.config);
        }

        private void okButton_Click(object sender, EventArgs e) {
            applyButton_Click(this, e);
        }

        private void editorStyleList_SelectedIndexChanged(object sender, EventArgs e) {
            busy = true;
            selectedStyles.Clear();
            selectedStyles2.Clear();

            bool anyTextStyles = false, anyUnderlineStyles = false;
            // Settings for populating the form.
            Color? foregroundColour = null, backgroundColour = null, underlineColour = null;
            bool indeterminateForeground = false, indeterminateBackground = false, indeterminateUnderline = false;
            Color defaultForeground = Color.Empty, defaultBackground = Color.Empty, defaultUnderline = Color.Empty;
            var styleFlags = new int[4];

            foreach (var index in editorStyleList.SelectedIndices) {
                object style = null; Style style2 = null;

                switch ((int) index) {
                    case 0:
                        style = config.SyntaxColours.TextStyle;
                        break;
                    case 1:
                        style = config.SyntaxColours.CommentStyle;
                        style2 = MslSyntaxHighlighter.CommentStyle;
                        break;
                    case 2:
                        style = config.SyntaxColours.KeywordStyle;
                        style2 = MslSyntaxHighlighter.KeywordStyle;
                        break;
                    case 3:
                        style = config.SyntaxColours.CommandStyle;
                        style2 = MslSyntaxHighlighter.CommandStyle;
                        break;
                    case 4:
                        style = config.SyntaxColours.CustomCommandStyle;
                        style2 = MslSyntaxHighlighter.CustomCommandStyle;
                        break;
                    case 5:
                        style = config.SyntaxColours.FunctionStyle;
                        style2 = MslSyntaxHighlighter.FunctionStyle;
                        break;
                    case 6:
                        style = config.SyntaxColours.CustomFunctionStyle;
                        style2 = MslSyntaxHighlighter.CustomFunctionStyle;
                        break;
                    case 7:
                        style = config.SyntaxColours.FunctionPropertyStyle;
                        style2 = MslSyntaxHighlighter.FunctionPropertyStyle;
                        break;
                    case 8:
                        style = config.SyntaxColours.VariableStyle;
                        style2 = MslSyntaxHighlighter.VariableStyle;
                        break;
                    case 9:
                        style = config.SyntaxColours.AliasStyle;
                        style2 = MslSyntaxHighlighter.AliasStyle;
                        break;
                    case 10:
                        style = config.SyntaxColours.ErrorStyle;
                        style2 = MslSyntaxHighlighter.ErrorStyle;
                        break;
                    case 11:
                        style = config.SyntaxColours.WarningStyle;
                        style2 = MslSyntaxHighlighter.WarningStyle;
                        break;
                    case 12:
                        style = config.SyntaxColours.NoticeStyle;
                        style2 = MslSyntaxHighlighter.NoticeStyle;
                        break;
                }

                if (style != null) {
                    selectedStyles.Add(style);
                    selectedStyles2.Add(style2);

                    if (style is Config.TextStyle) {
                        if (anyTextStyles) {
                            defaultForeground = Color.Empty;
                            defaultBackground = Color.Empty;

                            if (foregroundColour != ((Config.TextStyle) style).TextColour)
                                indeterminateForeground = true;
                            if (backgroundColour != ((Config.TextStyle) style).BackgroundColour)
                                indeterminateBackground = true;
                        } else {
                            anyTextStyles = true;
                            defaultForeground = ((Config.TextStyle) style).DefaultTextColour;
                            defaultBackground = ((Config.TextStyle) style).DefaultBackgroundColour;
                            foregroundColour = ((Config.TextStyle) style).TextColour;
                            backgroundColour = ((Config.TextStyle) style).BackgroundColour;
                        }

                        for (int i = 0; i < 4; ++i) {
                            if (((Config.TextStyle) style).Style.HasFlag((FontStyle) (1 << i)))
                                styleFlags[i] |= 2;
                            else
                                styleFlags[i] |= 1;
                        }
                    } else if (style is Config.UnderlineStyle) {
                        if (anyUnderlineStyles) {
                            defaultUnderline = Color.Empty;
                            if (underlineColour != ((Config.UnderlineStyle) style).UnderlineColour)
                                indeterminateUnderline = true;
                        } else {
                            anyUnderlineStyles = true;
                            defaultUnderline = ((Config.UnderlineStyle) style).DefaultUnderlineColour;
                            underlineColour = ((Config.UnderlineStyle) style).UnderlineColour;
                        }
                    }
                }
            }

            // Populate the text boxes.
            editorForegroundColourLabel.Enabled = anyTextStyles;
            editorForegroundColourBox.Enabled = anyTextStyles;
            button2.Enabled = anyTextStyles;
            editorBackgroundColourLabel.Enabled = anyTextStyles;
            editorBackgroundColourBox.Enabled = anyTextStyles;
            button3.Enabled = anyTextStyles;
            editorStyleLabel.Enabled = anyTextStyles;
            editorFontStyleList2.Enabled = anyTextStyles;
            editorUnderlineColourLabel.Enabled = anyUnderlineStyles;
            editorUnderlineColourBox.Enabled = anyUnderlineStyles;
            button4.Enabled = anyUnderlineStyles;

            if (anyTextStyles) {
                if (indeterminateForeground) {
                    editorForegroundColourBox.Color = Color.Empty;
                } else if (foregroundColour == null) {
                    editorForegroundColourBox.DefaultSelected = true;
                } else if (foregroundColour.Value.IsEmpty) {
                    editorForegroundColourBox.InitialSelected = true;
                } else {
                    editorForegroundColourBox.Color = foregroundColour.Value;
                }
                if (indeterminateBackground) {
                    editorBackgroundColourBox.Color = Color.Empty;
                } else if (backgroundColour == null) {
                    editorBackgroundColourBox.DefaultSelected = true;
                } else if (backgroundColour.Value.IsEmpty) {
                    editorBackgroundColourBox.InitialSelected = true;
                } else {
                    editorBackgroundColourBox.Color = backgroundColour.Value;
                }

                for (int i = 0; i < 4; ++i) {
                    switch (styleFlags[i]) {
                        case 0: case 1:
                            editorFontStyleList2.SetItemCheckState(i, CheckState.Unchecked); break;
                        case 2:
                            editorFontStyleList2.SetItemCheckState(i, CheckState.Checked); break;
                        case 3:
                            editorFontStyleList2.SetItemCheckState(i, CheckState.Indeterminate); break;
                        default:
                            Program.impossible(12); break;
                    }
                }
            } else {
                editorForegroundColourBox.Color = Color.Empty;
                editorBackgroundColourBox.Color = Color.Empty;
            }

            if (anyUnderlineStyles) {
                if (indeterminateUnderline) {
                    editorUnderlineColourBox.Color = Color.Empty;
                } else if (underlineColour == null) {
                    editorUnderlineColourBox.DefaultSelected = true;
                } else if (underlineColour.Value.IsEmpty) {
                    editorUnderlineColourBox.InitialSelected = true;
                } else {
                    editorUnderlineColourBox.Color = underlineColour.Value;
                }
            } else
                editorUnderlineColourBox.Color = Color.Empty;

            editorForegroundColourBox.DefaultColor = defaultForeground;
            editorBackgroundColourBox.DefaultColor = defaultBackground;
            editorUnderlineColourBox.DefaultColor = defaultUnderline;
            busy = false;
        }

        private void button2_Click(object sender, EventArgs e) {
            if (colorDialog.ShowDialog(this) == DialogResult.OK) {
                editorForegroundColourBox.Color = colorDialog.Color;
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            if (colorDialog.ShowDialog(this) == DialogResult.OK) {
                editorBackgroundColourBox.Color = colorDialog.Color;
            }
        }

        private void button4_Click(object sender, EventArgs e) {
            if (colorDialog.ShowDialog(this) == DialogResult.OK) {
                editorUnderlineColourBox.Color = colorDialog.Color;
            }
        }

        private void setForegroundColour(ColourComboBox box) {
            for (int i = 0; i < selectedStyles.Count; ++i) {
                if (selectedStyles[i] is Config.TextStyle) {
                    var configStyle = (Config.TextStyle) selectedStyles[i];
                    configStyle.TextColour = box.DefaultSelected ? (Color?) null : box.Color;

                    if (selectedStyles2[i] != null) {
                        var textStyle = (TextStyle) selectedStyles2[i];
                        textStyle.ForeBrush = (box.InitialSelected || box.Color.IsEmpty ? null : new SolidBrush(box.Color));
                    }
                }
            }
            if (editorStyleList.GetSelected(0))
                setNormalForegroundColour();

            editorColoursSampleTextBox.Refresh();
        }

        private void setNormalForegroundColour() {
            editorColoursSampleTextBox.ForeColor = config.SyntaxColours.TextStyle.TrueTextColour;
            if (config.SyntaxColours.CommentStyle.TrueTextColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.CommentStyle).ForeBrush = null;
            if (config.SyntaxColours.KeywordStyle.TrueTextColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.KeywordStyle).ForeBrush = null;
            if (config.SyntaxColours.CommandStyle.TrueTextColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.CommandStyle).ForeBrush = null;
            if (config.SyntaxColours.CustomCommandStyle.TrueTextColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.CustomCommandStyle).ForeBrush = null;
            if (config.SyntaxColours.FunctionStyle.TrueTextColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.FunctionStyle).ForeBrush = null;
            if (config.SyntaxColours.CustomFunctionStyle.TrueTextColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.CustomFunctionStyle).ForeBrush = null;
            if (config.SyntaxColours.FunctionPropertyStyle.TrueTextColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.FunctionPropertyStyle).ForeBrush = null;
            if (config.SyntaxColours.VariableStyle.TrueTextColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.VariableStyle).ForeBrush = null;
            if (config.SyntaxColours.AliasStyle.TrueTextColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.AliasStyle).ForeBrush = null;
        }

        private void setBackgroundColour(ColourComboBox box) {
            for (int i = 0; i < selectedStyles.Count; ++i) {
                if (selectedStyles[i] is Config.TextStyle) {
                    var configStyle = (Config.TextStyle) selectedStyles[i];
                    configStyle.BackgroundColour = box.DefaultSelected ? (Color?) null : box.Color;

                    if (selectedStyles2[i] != null) {
                        var textStyle = (TextStyle) selectedStyles2[i];
                        textStyle.BackgroundBrush = (box.InitialSelected || box.Color.IsEmpty ? null : new SolidBrush(box.Color));
                    }
                }
            }
            if (editorStyleList.GetSelected(0))
                setNormalBackgroundColour();


            editorColoursSampleTextBox.Refresh();
        }

        private void setNormalBackgroundColour() {
            editorColoursSampleTextBox.BackColor = config.SyntaxColours.TextStyle.TrueBackgroundColour;
            if (config.SyntaxColours.CommentStyle.TrueBackgroundColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.CommentStyle).BackgroundBrush = null;
            if (config.SyntaxColours.KeywordStyle.TrueBackgroundColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.KeywordStyle).BackgroundBrush = null;
            if (config.SyntaxColours.CommandStyle.TrueBackgroundColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.CommandStyle).BackgroundBrush = null;
            if (config.SyntaxColours.CustomCommandStyle.TrueBackgroundColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.CustomCommandStyle).BackgroundBrush = null;
            if (config.SyntaxColours.FunctionStyle.TrueBackgroundColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.FunctionStyle).BackgroundBrush = null;
            if (config.SyntaxColours.CustomFunctionStyle.TrueBackgroundColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.CustomFunctionStyle).BackgroundBrush = null;
            if (config.SyntaxColours.FunctionPropertyStyle.TrueBackgroundColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.FunctionPropertyStyle).BackgroundBrush = null;
            if (config.SyntaxColours.VariableStyle.TrueBackgroundColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.VariableStyle).BackgroundBrush = null;
            if (config.SyntaxColours.AliasStyle.TrueBackgroundColour == Color.Empty) ((TextStyle) MslSyntaxHighlighter.AliasStyle).BackgroundBrush = null;
        }

        private void setUnderlineColour(ColourComboBox box) {
            Color colour2;
            foreach (var style in selectedStyles) {
                if (style is Config.UnderlineStyle)
                    ((Config.UnderlineStyle) style).UnderlineColour = box.DefaultSelected ? (Color?) null : box.Color;
            }

            var errorIndex = editorColoursSampleTextBox.GetStyleIndex(MslSyntaxHighlighter.ErrorStyle);
            var warningIndex = editorColoursSampleTextBox.GetStyleIndex(MslSyntaxHighlighter.WarningStyle);
            var noticeIndex = editorColoursSampleTextBox.GetStyleIndex(MslSyntaxHighlighter.NoticeStyle);

            // WavyLineStyles are immutable, so we must create new ones.
            if (editorStyleList.GetSelected(10))
                MslSyntaxHighlighter.ErrorStyle = new WavyLineStyle(box.Color.A, box.Color);
            if (editorStyleList.GetSelected(11))
                MslSyntaxHighlighter.WarningStyle = new WavyLineStyle(box.Color.A, box.Color);
            if (editorStyleList.GetSelected(12))
                MslSyntaxHighlighter.NoticeStyle = new WavyLineStyle(box.Color.A, box.Color);

            if (errorIndex >= 0) editorColoursSampleTextBox.Styles[errorIndex] = MslSyntaxHighlighter.ErrorStyle;
            if (warningIndex >= 0) editorColoursSampleTextBox.Styles[warningIndex] = MslSyntaxHighlighter.WarningStyle;
            if (noticeIndex >= 0) editorColoursSampleTextBox.Styles[noticeIndex] = MslSyntaxHighlighter.NoticeStyle;

            //Rehighlight((MslDocument) editorColoursSampleTextBox.Tag);
            editorColoursSampleTextBox.Refresh();
        }

        private void setStyle(FontStyle flag, bool active) {
            foreach (var style in selectedStyles) {
                if (style is Config.TextStyle) {
                    if (active) ((Config.TextStyle) style).Style |= flag;
                    else ((Config.TextStyle) style).Style &= ~flag;
                }
            }
            foreach (var style in selectedStyles2) {
                if (style is TextStyle) {
                    if (active) ((TextStyle) style).FontStyle |= flag;
                    else ((TextStyle) style).FontStyle &= ~flag;
                }
            }

            editorColoursSampleTextBox.Refresh();
        }

        bool previousAssume;
        private void preferredAliasExtensionBox_TextChanged(object sender, EventArgs e) {
            if (preferredAliasExtensionBox.Text.Equals("mrc", StringComparison.OrdinalIgnoreCase)) {
                assumeMrcFilesCheckBox.Enabled = false;
                assumeMrcFilesCheckBox.Checked = false;
            } else if (!assumeMrcFilesCheckBox.Enabled) {
                assumeMrcFilesCheckBox.Enabled = true;
                assumeMrcFilesCheckBox.Checked = previousAssume;
            }
        }

        private void configurationFileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(Program.ConfigFileDefault);
            configurationFileLink.LinkVisited = true;
        }

        private void editorColoursSampleTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            var document = (MslDocument) editorColoursSampleTextBox.Tag;
            if (document != null) Rehighlight(document, e.ChangedRange.Start.iLine);
        }

        private void editorForegroundColourBox_ColorChanged(object sender, EventArgs e) {
            if (!busy)
                setForegroundColour(editorForegroundColourBox);
        }

        private void editorBackgroundColourBox_ColorChanged(object sender, EventArgs e) {
            if (!busy)
                setBackgroundColour(editorBackgroundColourBox);
        }

        private void editorUnderlineColourBox_ColorChanged(object sender, EventArgs e) {
            if (!busy)
                setUnderlineColour(editorUnderlineColourBox);
        }

        private void editorForegroundColourBox_Load(object sender, EventArgs e) {

        }

        private void themeBox_SelectedIndexChanged(object sender, EventArgs e) {
            config.Theme = (Theme) themeBox.SelectedIndex;
            applyTheme();
            editorStyleList_SelectedIndexChanged(sender, EventArgs.Empty);
        }

        private void editorFontStyleList2_ItemCheck(object sender, ItemCheckEventArgs e) {
            if (busy) return;
            var styleFlag = (FontStyle) (1 << e.Index);
            setStyle(styleFlag, e.NewValue == CheckState.Checked);
        }
    }
}
