using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace MIDE {
    public partial class DialogDesigner : UserControl {
        [DefaultValue(2)]
        public int IndentCount { get; set; } = 2;

        public ControlType selectedType = 0;
        private int startTabIndex;
        private Point startPoint;
        private Point endPoint;
        private bool moving;
        private AnchorStyles resizing;

        private MslControl.MslTab tabControl;

        public List<MslMovableControl> controls = new List<MslMovableControl>();
        public List<MslMenu> menus = new List<MslMenu>();
        private List<MslMovableControl> selectedControls = new List<MslMovableControl>();
        private List<MslMenu> selectedMenus = new List<MslMenu>();
        private int nextID = 1;
        public bool menusSelected;
        private int selectedTab;

        private List<Rectangle> resizeHandles = new List<Rectangle>(8);

        private Bitmap layoutImage;
        private Graphics layoutGraphics;
        private Bitmap clientAreaImage;
        private Graphics clientAreaGraphics;

        [TypeConverter(typeof(ExpandableObjectConverter))]  // Allows the dialog's properties to be edited in the Windows Forms designer.
        public MslDialog Dialog { get; }

        [Browsable(false)]
        public ReadOnlyCollection<MslMovableControl> SelectedControls { get; }
        [Browsable(false)]
        public ReadOnlyCollection<MslMenu> SelectedMenus { get; }

        private Color selectedMenuColor = Color.RoyalBlue;

        private Theme theme;
        [DefaultValue(Theme.Default)]
        public Theme Theme {
            get { return this.theme; }
            set {
                this.theme = value;
                applyTheme();
            }
        }

        private List<string> lines;
        private int titleLine = -1, iconLine = -1, sizeLine = -1, optionLine = -1, controlLine;

        public static readonly Regex ParseRegexTitle = new Regex(@"""?((?<="")(?>[^""]*)|(?<!"")(?>.*))""?", RegexOptions.Compiled);

        private Size layoutOffset = new Size(8, 28);

#if (!MONO)
        [DllImport("user32.dll")]
        private static extern bool MapDialogRect(IntPtr dialogHandle, ref RECT rectangle);
#endif

        private Rectangle MapDbuRectangle(Rectangle rectangle) {
#if (!MONO)
            RECT rect = new RECT() { Left = rectangle.Left, Top = rectangle.Top, Right = rectangle.Right, Bottom = rectangle.Bottom };
            bool result = MapDialogRect(this.Handle, ref rect);
            return new Rectangle(rect.Left, rect.Top, rect.Right = rect.Left, rect.Bottom - rect.Top);
#else
            return new Rectangle(rectangle.Left * 2, rectangle.Top * 2, rectangle.Width * 2, rectangle.Height * 2);
#endif
        }

        [Browsable(true)] [EditorBrowsable(EditorBrowsableState.Always)] [Editor("System.ComponentModel.Design.MultilineStringEditor", typeof(UITypeEditor))]
        public override string Text {
            get {
                return string.Join(Environment.NewLine, this.lines);
            }
            set {
                this.lines.Clear();
                this.controls.Clear();
                this.menus.Clear();
                this.tabControl = null;

                this.optionLine = -1;
                this.sizeLine = -1;
                this.iconLine = -1;
                this.titleLine = -1;
                this.controlLine = 0;

                this.selectedControls.Clear();
                this.selectedMenus.Clear();
                
                this.layoutPanel.Controls.Clear();
                this.menuBar.Items.Clear();
                this.menuBar.Items.Add(this.newMenuItem);
                this.menuBar.Items.Add(this.addItemToolStripMenuItem);

                int lineNumber = 0;
                using (var reader = new StringReader(value)) {
                    while (true) {
                        var line = reader.ReadLine();
                        if (line == null) break;  // End of text.
                        this.lines.Add(line);

                        var fields = line.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                        if (fields.Length != 0) {
                            if (fields[0].Equals("title", StringComparison.OrdinalIgnoreCase)) {
                                titleLine = lineNumber;
                                var m = ParseRegexTitle.Match(fields[1].TrimStart());
                                this.Dialog.Title = m.Groups[1].Value;
                            } else if (fields[0].Equals("icon", StringComparison.OrdinalIgnoreCase)) {
                                iconLine = lineNumber;
                                fields = fields[1].Split(new[] { ',' }, 2, StringSplitOptions.RemoveEmptyEntries);
                                this.Dialog.IconFilename = fields[0];
                                this.Dialog.IconIndex = int.Parse(fields[1]);
                            } else if (fields[0].Equals("size", StringComparison.OrdinalIgnoreCase)) {
                                sizeLine = lineNumber;
                                fields = fields[1].Split(new[] { ' ' }, 5, StringSplitOptions.RemoveEmptyEntries);
                                this.Dialog.Size = new Size(int.Parse(fields[2]), int.Parse(fields[3]));
                            } else if (fields[0].Equals("option", StringComparison.OrdinalIgnoreCase)) {
                                optionLine = lineNumber;
                                fields = fields[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                var pixels = false;
                                foreach (var option in fields) {
                                    if (option.Equals("dbu", StringComparison.OrdinalIgnoreCase)) {
                                        if (!pixels) this.Dialog.SizeUnit = DialogSizeUnit.DBU;
                                    } else if (option.Equals("pixels", StringComparison.OrdinalIgnoreCase)) {
                                        this.Dialog.SizeUnit = DialogSizeUnit.Pixel;
                                        pixels = true;
                                    } else if (option.Equals("notheme", StringComparison.OrdinalIgnoreCase)) {
                                        this.Dialog.notheme = true;
                                    } else if (option.Equals("disable", StringComparison.OrdinalIgnoreCase)) {
                                        this.Dialog.disable = true;
                                    }
                                }
                            } else {
                                MslControl control;
                                if (MslControl.TryParse(this.Dialog, line, out control)) {
                                    if (this.controlLine == 0) this.controlLine = lineNumber;
                                    control.lineNumber = lineNumber;
                                    if (control is MslMovableControl) this.AddControl((MslMovableControl) control, false);
                                    else if (control is MslMenu) this.AddMenu((MslMenu) control, false);
                                    else throw new InvalidOperationException("An attempt was made to import an unknown control type. This is a bug.");
                                }
                            }
                        } else {
                            if (this.controlLine == 0 && (this.optionLine != 0 || this.sizeLine != 0 || this.iconLine != 0 || this.titleLine != 0))
                                this.controlLine = lineNumber;
                        }

                        ++lineNumber;
                    }
                }

                this.RedrawLayout();
                this.RebuildListBox(false);
            }
        }

        [Description("Occurs when the 'View code' button in the control is clicked.")]
        public event EventHandler ViewCode;

        public DialogDesigner() : this(new MslDialog("dialog")) { }
        public DialogDesigner(MslDialog dialog) {
            dialog.designer = this;
            this.Dialog = dialog;
            this.Dialog.TitleChanged += Dialog_TitleChanged;
            this.Dialog.SizeChanged += Dialog_SizeChanged;
            this.Dialog.StyleChanged += Dialog_StyleChanged;

            if (dialog.Document == null || dialog.Bookmark == null) {
                this.lines = new List<string>() {
                    "dialog " + dialog.Name + " {",
                    "  title \"Dialog\"",
                    "  option " + (dialog.SizeUnit == DialogSizeUnit.DBU ? "dbu" : "pixels"),
                    "}"
                };
                this.titleLine = 1;
                this.optionLine = 2;
                this.sizeLine = -1;
                this.iconLine = -1;
                this.controlLine = 3;
            } else {
                var line = dialog.Document.TextBox[dialog.Bookmark.position];
                var endLineIndex = dialog.Document.EndOfBlock(dialog.Bookmark.position);

                this.lines = new List<string>(endLineIndex - dialog.Bookmark.position + 1);
                for (int i = dialog.Bookmark.position; i <= endLineIndex; ++i)
                    this.lines.Add(dialog.Document.TextBox[i].Text);
            }

            this.SelectedControls = new ReadOnlyCollection<MslMovableControl>(this.selectedControls);
            this.SelectedMenus = new ReadOnlyCollection<MslMenu>(this.selectedMenus);

            InitializeComponent();
            layoutPictureBox.KeyDown += LayoutPictureBox_KeyDown;
            this.SetStyle(ControlStyles.Selectable, true);

            this.RebuildListBox(false);
        }

        private void Dialog_StyleChanged(object sender, EventArgs e) {
            setOptionLine(ref optionLine, new string(' ', this.IndentCount) + "option " + string.Join(" ", this.Dialog.Styles));
        }

        private void setOptionLine(ref int lineIndex, string line) {
            if (lineIndex == -1) {
                this.AddLine(this.controlLine, line);
                lineIndex = this.controlLine - 1;  // AddLine increments it.
            } else {
                this.lines[lineIndex] = line;
            }
        }

        private void LayoutPictureBox_KeyDown(object sender, KeyEventArgs e) {
            // Arrow keys
            if (e.KeyCode >= Keys.Left && e.KeyCode <= Keys.Down) {
                if (selectedControls.Count != 0) {
                    var d = (e.Control ? 8 : 1);
                    if (e.Shift) {
                        // Resize controls.
                        foreach (var control in this.selectedControls) {
                            var width = control.Width; var height = control.Height;

                            switch (e.KeyCode) {
                                case Keys.Left : width  -= d; break;
                                case Keys.Up   : height -= d; break;
                                case Keys.Right: width  += d; break;
                                case Keys.Down : height += d; break;
                            }

                            if (width < 1) width = 1;
                            if (height < 1) height = 1;

                            control.Width = width;
                            control.Height = height;
                        }
                        RedrawLayout();
                        this.propertyGrid.Refresh();
                    } else {
                        // Move controls.
                        foreach (var control in this.selectedControls) {
                            switch (e.KeyCode) {
                                case Keys.Left : control.X -= d; break;
                                case Keys.Up   : control.Y -= d; break;
                                case Keys.Right: control.X += d; break;
                                case Keys.Down : control.Y += d; break;
                            }
                        }
                        RedrawLayout();
                        this.propertyGrid.Refresh();
                    }
                }

            // Clipboard shortcuts
            } else if (e.Control) {
                if (e.KeyCode == Keys.X) {
                    this.Cut();
                } else if (e.KeyCode == Keys.C || e.KeyCode == Keys.Insert) {
                    this.Copy();
                } else if (e.KeyCode == Keys.V) {
                    this.Paste();
                }
            } else if (e.Shift) {
                if (e.KeyCode == Keys.Delete) {
                    this.Cut();
                } else if (e.KeyCode == Keys.Insert) {
                    this.Paste();
                }
            } else {
                // Delete key
                if (e.KeyCode == Keys.Delete) {
                    this.DeleteSelection();
                }
            }
        }

        private void Dialog_TitleChanged(object sender, EventArgs e) {
            setOptionLine(ref titleLine, new string(' ', this.IndentCount) + "title \"" + this.Dialog.Title + "\"");
            RedrawLayout();
        }

        private void Dialog_SizeChanged(object sender, EventArgs e) {
            ResizeLayout();
            foreach (var control in this.controls)
                control.UpdateRectangle();
            RedrawLayout();
        }

        private void controlToolStripButton_Click(object sender, System.EventArgs e) {
            var button = (ToolStripButton) sender;

            foreach (ToolStripItem item in toolbox.Items)
                if (item != button) ((ToolStripButton) item).Checked = false;

            button.Checked = true;
            selectedType = (ControlType) (int) button.Tag;

            layoutPictureBox.Cursor = Cursors.Cross;
        }

        private void layoutPictureBox_MouseDown(object sender, MouseEventArgs e) {
            layoutPictureBox.Focus();
            if (e.Button == MouseButtons.Left) {
                startPoint = e.Location;
                if (this.selectedType == 0) {
                    // Possibly begin a resize operation.
                    for (int i = 0; i < this.resizeHandles.Count; ++i) {
                        if (this.resizeHandles[i].Contains(e.Location)) {
                            switch (i % 8) {
                                case 0: resizing = AnchorStyles.Bottom; break;
                                case 1: resizing = AnchorStyles.Bottom | AnchorStyles.Right; break;
                                case 2: resizing = AnchorStyles.Right; break;
                                case 3: resizing = AnchorStyles.Top | AnchorStyles.Right; break;
                                case 4: resizing = AnchorStyles.Top; break;
                                case 5: resizing = AnchorStyles.Top | AnchorStyles.Left; break;
                                case 6: resizing = AnchorStyles.Left; break;
                                case 7: resizing = AnchorStyles.Bottom | AnchorStyles.Left; break;
                            }

                            if (this.selectedControls.Count == 0) {
                                this.Dialog.startWidth = this.Dialog.Size.Width;
                                this.Dialog.startHeight = this.Dialog.Size.Height;
                            } else {
                                foreach (var control in this.selectedControls) {
                                    control.startX = control.X;
                                    control.startY = control.Y;
                                    control.startWidth = control.Width;
                                    control.startHeight = control.Height;
                                }
                            }
                            return;
                        }
                    }

                    // Otherwise, select a control.
                    if (!ModifierKeys.HasFlag(Keys.Control)) this.selectedControls.Clear();

                    var point = e.Location - layoutOffset;
                    if (point.X >= 0 || point.X < this.Dialog.SizePixels.Width || point.Y >= 0 || point.Y < this.Dialog.SizePixels.Height) {
                        var control = this.GetControlAtPoint(point);
                        if (control is MslMovableControl) {
                            this.selectedControls.Add((MslMovableControl) control);

                            if (control.Type == ControlType.Tab && !ModifierKeys.HasFlag(Keys.Control)) {
                                // Allow the user to select tabs in a tab control.
                                var tabControl = (MslControl.MslTab) control;
                                var index = tabControl.GetTabIndexAt(e.Location - new Size(tabControl.X, tabControl.Y));
                                if (index != -1) {
                                    if (index == tabControl.TabCount) {
                                        // They clicked the New Tab button.
                                        tabControl.AddTab(tabControl.TabCount + 1, "Tab " + (tabControl.TabCount + 1));
                                    } else {
                                        tabControl.SelectTab(index);
                                    }
                                    this.RedrawLayout();
                                }
                            }
                        }
                    }

                    if (this.selectedControls.Count == 0)
                        propertyGrid.SelectedObject = this.Dialog;
                    else
                        propertyGrid.SelectedObjects = this.selectedControls.ToArray();

                    placeResizeHandles();
                    RebuildListBox(true);
                    layoutPictureBox.Refresh();
                }
            }
        }

        private void layoutPictureBox_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button.HasFlag(MouseButtons.Left)) {
                if (resizing != 0) {
                    var unit = (this.Dialog.SizeUnit == DialogSizeUnit.DBU ? Program.GetDialogBaseUnitSize() : new Size(1, 1));
                    endPoint = e.Location;

                    if (this.selectedControls.Count == 0) {
                        // Resize the dialog.
                        var width = this.Dialog.Size.Width; var height = this.Dialog.Size.Height;

                        if (resizing.HasFlag(AnchorStyles.Right)) {
                            width = this.Dialog.startWidth + (endPoint.X - startPoint.X) / unit.Width;
                            if (width < 1) width = 1;
                        }
                        if (resizing.HasFlag(AnchorStyles.Bottom)) {
                            height = this.Dialog.startHeight + (endPoint.Y - startPoint.Y) / unit.Height;
                            if (height < 1) height = 1;
                        }

                        if (width != this.Dialog.Size.Width || height != this.Dialog.Size.Height) {
                            this.Dialog.Size = new Size(width, height);
                            this.propertyGrid.Refresh();
                        }
                    } else {
                        // Resize controls.
                        bool anyControlsChanged = false;
                        foreach (var control in this.selectedControls) {
                            var width = control.Width; var height = control.Height;

                            if (resizing.HasFlag(AnchorStyles.Right)) {
                                width = control.startWidth + (endPoint.X - startPoint.X) / unit.Width;
                            } else if (resizing.HasFlag(AnchorStyles.Left)) {
                                width = control.startWidth - (endPoint.X - startPoint.X) / unit.Width;
                            }
                            if (resizing.HasFlag(AnchorStyles.Bottom)) {
                                height = control.startHeight + (endPoint.Y - startPoint.Y) / unit.Height;
                            } else if (resizing.HasFlag(AnchorStyles.Top)) {
                                height = control.startHeight - (endPoint.Y - startPoint.Y) / unit.Height;
                            }

                            if (width < 1) width = 1;
                            if (height < 1) height = 1;

                            if (width != control.Width || height != control.Height) {
                                control.Width = width;
                                control.Height = height;
                                if (resizing.HasFlag(AnchorStyles.Left))
                                    control.X = control.startX - (control.Width - control.startWidth);
                                if (resizing.HasFlag(AnchorStyles.Top))
                                    control.Y = control.startY - (control.Height - control.startHeight);
                                anyControlsChanged = true;
                            }
                        }
                        if (anyControlsChanged)
                            this.propertyGrid.Refresh();
                    }
                    RedrawLayout();
                } else if (!startPoint.IsEmpty) {
                    endPoint = e.Location;

                    if (!moving && this.selectedType == 0 && (Math.Abs(endPoint.X - startPoint.X) >= 4 || Math.Abs(endPoint.Y - startPoint.Y) >= 4)) {
                        // Start a move operation.
                        foreach (var control in this.selectedControls) {
                            control.startX = control.X;
                            control.startY = control.Y;
                        }
                        moving = true;
                    }

                    if (moving) {
                        // Move controls.
                        var unit = (this.Dialog.SizeUnit == DialogSizeUnit.DBU ? Program.GetDialogBaseUnitSize() : new Size(1, 1));
                        bool anyControlsChanged = false;
                        foreach (var control in this.selectedControls) {
                            var x = control.startX + (endPoint.X - startPoint.X) / unit.Width;
                            var y = control.startY + (endPoint.Y - startPoint.Y) / unit.Height;

                            if (x != control.X || y != control.Y) {
                                control.X = x;
                                control.Y = y;
                                anyControlsChanged = true;
                            }
                        }
                        if (anyControlsChanged)
                            this.propertyGrid.Refresh();
                    }

                    layoutPictureBox.Refresh();
                }
            } else {
                if (selectedType == 0) {
                    // Change the cursor if it is on a resize handle.
                    for (int i = 0; i < this.resizeHandles.Count; ++i) {
                        if (this.resizeHandles[i].Contains(e.Location)) {
                            switch (i % 8) {
                                case 0: case 4: layoutPictureBox.Cursor = Cursors.SizeNS; break;
                                case 1: case 5: layoutPictureBox.Cursor = Cursors.SizeNWSE; break;
                                case 2: case 6: layoutPictureBox.Cursor = Cursors.SizeWE; break;
                                case 3: case 7: layoutPictureBox.Cursor = Cursors.SizeNESW; break;
                            }
                            return;
                        }
                    }

                    // Change the cursor if it is on a control.
                    var point = e.Location - layoutOffset;
                    if (this.GetControlAtPoint(point) != null)
                        layoutPictureBox.Cursor = Cursors.SizeAll;
                    else
                        layoutPictureBox.Cursor = Cursors.Default;
                }
            }
        }

        private void layoutPictureBox_Paint(object sender, PaintEventArgs e) {
            e.Graphics.DrawImage(layoutImage, Point.Empty);

            if (selectedType != 0) {
                e.Graphics.DrawRectangle(new Pen(this.ForeColor), new Rectangle(
                    Math.Min(startPoint.X, endPoint.X), Math.Min(startPoint.Y, endPoint.Y),
                    Math.Abs(endPoint.X - startPoint.X), Math.Abs(endPoint.Y - startPoint.Y))
                );
            }

            if (this.resizing == 0) {
                Pen selectionBorderPen = new Pen(SystemColors.Highlight, 1);

                foreach (var control in this.selectedControls) {
                    Rectangle bounds = control.Control.Bounds;
                    bounds.Offset(layoutOffset.Width, layoutOffset.Height);
                    if (control.TabIndex != 0) {
                        bounds.Offset(tabControl.Control.Bounds.Location);
                        bounds.Offset(((TabControl) tabControl.Control).SelectedTab.Bounds.Location);
                    }
                    e.Graphics.DrawRectangle(selectionBorderPen, bounds);
                }

                // Draw the resize handles.
                if (!ModifierKeys.HasFlag(Keys.Alt)) {
                    foreach (Rectangle rectangle in resizeHandles) {
                        e.Graphics.FillRectangle(Brushes.White, rectangle);
                        e.Graphics.DrawRectangle(new Pen(Color.Black), rectangle);
                    }
                }
            }
        }

        private void layoutPictureBox_MouseUp(object sender, MouseEventArgs e) {
            if (resizing != 0) {
                // End a resize operation.
                resizing = 0;
                startPoint = Point.Empty;
                endPoint = Point.Empty;
                layoutPictureBox.Refresh();
                this.propertyGrid.Refresh();
            } else if (moving) {
                // End a move operation.
                moving = false;
                startPoint = Point.Empty;
                endPoint = Point.Empty;
                layoutPictureBox.Refresh();
                this.propertyGrid.Refresh();
            } else if (this.selectedType != 0) {
                // Add a control.
                int x = Math.Min(startPoint.X, endPoint.X) - layoutOffset.Width;
                int y = Math.Min(startPoint.Y, endPoint.Y) - layoutOffset.Height;
                int width = Math.Abs(endPoint.X - startPoint.X);
                int height = Math.Abs(endPoint.Y - startPoint.Y);

                MslMovableControl control = null;
                switch (selectedType) {
                    case ControlType.Text:
                        control = new MslControl.MslText(this.Dialog, x, y, width, height, "Text");
                        break;
                    case ControlType.Edit:
                        control = new MslControl.MslEdit(this.Dialog, x, y, width, height, "Edit");
                        break;
                    case ControlType.Button:
                        control = new MslControl.MslButton(this.Dialog, x, y, width, height, "Button");
                        break;
                    case ControlType.Check:
                        control = new MslControl.MslCheck(this.Dialog, x, y, width, height, "Check");
                        break;
                    case ControlType.Radio:
                        control = new MslControl.MslRadio(this.Dialog, x, y, width, height, "Radio");
                        break;
                    case ControlType.Box:
                        control = new MslControl.MslBox(this.Dialog, x, y, width, height, "Box");
                        break;
                    case ControlType.Scroll:
                        control = new MslControl.MslScroll(this.Dialog, x, y, width, height, "Scroll");
                        break;
                    case ControlType.List:
                        control = new MslControl.MslList(this.Dialog, x, y, width, height);
                        break;
                    case ControlType.Combo:
                        control = new MslControl.MslCombo(this.Dialog, x, y, width, height);
                        break;
                    case ControlType.Icon:
                        control = new MslControl.MslIcon(this.Dialog, x, y, width, height);
                        break;
                    case ControlType.Link:
                        control = new MslControl.MslLink(this.Dialog, x, y, width, height, "Link");
                        break;
                    case ControlType.Tab:
                        control = new MslControl.MslTab(this.Dialog, x, y, width, height, "Tab");
                        break;
                    default:
                        throw new InvalidOperationException("Tried to add a control of an unknown type.");
                }

                this.AddControl(control);
                ((ToolStripButton) toolbox.Items[(int) selectedType - 1]).Checked = false;

                this.selectedControls.Clear();
                this.selectedControls.Add(control);
                propertyGrid.SelectedObject = control;

                selectedType = 0;
                startPoint = Point.Empty;
                endPoint = Point.Empty;
                layoutPictureBox.Cursor = Cursors.Default;

                this.RedrawLayout();
                this.RebuildListBox(false);
            }
        }

        private void Control_TextChanged(object sender, EventArgs e) {
            this.lines[((MslControl) sender).lineNumber] = new string(' ', this.IndentCount) + sender.ToString();
            this.RedrawLayout();
        }

        private void Control_StyleChanged(object sender, EventArgs e) {
            this.lines[((MslControl) sender).lineNumber] = new string(' ', this.IndentCount) + sender.ToString();
            if (sender is MslMovableControl) applyTheme((MslMovableControl) sender);
            this.RedrawLayout();
        }

        private void Control_RectangleChanged(object sender, EventArgs e) {
            this.lines[((MslControl) sender).lineNumber] = new string(' ', this.IndentCount) + sender.ToString();
            this.RedrawLayout();
        }

        private void ResizeLayout() {
            this.clientAreaImage = new Bitmap(this.Dialog.SizePixels.Width, this.Dialog.SizePixels.Height);
            this.clientAreaGraphics = Graphics.FromImage(this.clientAreaImage);

            layoutPanel.Size = this.Dialog.SizePixels;
            menuBar.Width = this.Dialog.SizePixels.Width;
        }

        private void RedrawLayout() {
            if (this.clientAreaImage == null) return;

            // Draw the window border.
            var size = this.Dialog.SizePixels;
            layoutGraphics.Clear(layoutPictureBox.BackColor);
            layoutGraphics.FillRectangle(new SolidBrush(SystemColors.Control), 4, 4, size.Width + layoutOffset.Width, size.Height + layoutOffset.Height);
            layoutGraphics.DrawLine(new Pen(SystemColors.ControlLight), 4, 4, size.Width + 11, 4);
            layoutGraphics.DrawLine(new Pen(SystemColors.ControlLight), 4, 4, 4, size.Height + layoutOffset.Height + 3);
            layoutGraphics.DrawLine(new Pen(SystemColors.ControlLightLight), 5, 5, size.Width + 10, 5);
            layoutGraphics.DrawLine(new Pen(SystemColors.ControlLightLight), 5, 5, 5, size.Height + layoutOffset.Height + 2);
            layoutGraphics.FillRectangle(new SolidBrush(SystemColors.Control), 6, 6, size.Width + 4, size.Height + 4);
            layoutGraphics.DrawLine(new Pen(SystemColors.ControlDarkDark), size.Width + 11, 4, size.Width + 11, size.Height + layoutOffset.Height + 3);
            layoutGraphics.DrawLine(new Pen(SystemColors.ControlDarkDark), 4, size.Height + layoutOffset.Height + 3, size.Width + 11, size.Height + layoutOffset.Height + 3);
            layoutGraphics.DrawLine(new Pen(SystemColors.ControlDark), size.Width + 10, 5, size.Width + 10, size.Height + layoutOffset.Height + 2);
            layoutGraphics.DrawLine(new Pen(SystemColors.ControlDark), 5, size.Height + layoutOffset.Height + 2, size.Width + 10, size.Height + layoutOffset.Height + 2);
            layoutGraphics.FillRectangle(new SolidBrush(this.BackColor), layoutOffset.Width, layoutOffset.Height, size.Width, size.Height);

            // Draw the caption bar.
            layoutGraphics.FillRectangle(new SolidBrush(SystemColors.ActiveCaption), 8, 8, size.Width, 20);
            layoutGraphics.DrawIcon(MIDE.Properties.Resources.dialogIcon, new Rectangle(11, 10, 16, 16));
            TextRenderer.DrawText(layoutGraphics, this.Dialog.Title, new Font(this.Font, FontStyle.Bold), new Rectangle(28, 8, size.Width - 16, 20), SystemColors.ActiveCaptionText, SystemColors.ActiveCaption, TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter);

            // Draw the controls.
            clientAreaGraphics.Clear(this.BackColor);
            foreach (var control in this.controls)
                clientAreaGraphics.DrawImage(control.image, control.Control.Left, control.Control.Top);

            layoutGraphics.DrawImage(clientAreaImage, layoutOffset.Width, layoutOffset.Height);
            
            placeResizeHandles();
            layoutPictureBox.Refresh();
        }

        public MslControl GetControlAtPoint(Point point) {
            for (int i = this.controls.Count - 1; i >= 0; --i) {
                var control = this.controls[i];
                //if (control.Type == ControlType.Tab) continue;  // Otherwise we would be unable to select controls inside tabs.
                Rectangle bounds = control.Control.Bounds;
                if (control.TabIndex != 0) {
                    bounds.Offset(tabControl.Control.Bounds.Location);
                    bounds.Offset(((TabControl) tabControl.Control).SelectedTab.Bounds.Location);
                }
                if (bounds.Contains(point)) return control;
            }
            //if (this.tabControl?.Control.Bounds.Contains(point) == true) return this.tabControl;
            return null;
        }

        private void DialogDesigner_KeyDown(object sender, KeyEventArgs e) {
        }

        [Browsable(false)]
        public bool CanCopy { get { return this.selectedControls.Count != 0; } }
        [Browsable(false)]
        public bool CanPaste { get { return Clipboard.ContainsText(TextDataFormat.UnicodeText); } }

        public void DeleteSelection() {
            if (this.selectedControls.Count != 0) {
                do {
                    this.controls.Remove(this.selectedControls[0]);
                    this.RemoveControl(this.selectedControls[0]);
                    layoutPanel.Controls.Remove((this.selectedControls[0]).Control);
                    this.selectedControls.RemoveAt(0);
                } while (this.selectedControls.Count != 0);
                propertyGrid.SelectedObject = this.Dialog;
                this.RedrawLayout();
                this.RebuildListBox(false);
            }
        }

        public void Cut() {
            if (this.CanCopy) {
                // We don't combine the two operations, as that would be unsafe in case the clipboard assignment fails.
                this.Copy();
                this.DeleteSelection(); 
            }
        }

        public void Copy() {
            if (this.CanCopy) {
                StringBuilder builder = new StringBuilder();
                foreach (var control in this.selectedControls) {
                    builder.AppendLine(control.ToString());
                }
                Clipboard.SetText(builder.ToString(), TextDataFormat.UnicodeText);
            }
        }

        public void Paste() {
            if (!this.CanPaste) return;

            this.selectedControls.Clear();
            foreach (string s in Clipboard.GetText().Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries)) {
                var control = MslControl.Parse(this.Dialog, s);

                if (control is MslMenu) {
                    MslMenu parent;
                    if (selectedMenus.Count == 1) {
                        parent = selectedMenus[0];
                        while (parent is MslMenu.MslItem) parent = this.menus.First(c => c.ID == parent.Parent);
                    } else parent = null;

                    if (control is MslMenu.MslItem && parent == null) {
                        MessageBox.Show("Can't add an item to the top level. Select a menu before pasting to add it as a child to that menu.", "MIDE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    ((MslMenu) control).Parent = parent == null ? 0 : parent.ID;
                    this.AddMenu((MslMenu) control);

                    var collection = parent?.control.DropDown.Items ?? menuBar.Items;
                    collection.Insert(collection.Count - 2, ((MslMenu) control).control);
                } else {
                    this.AddControl(control);
                    this.selectedControls.Add((MslMovableControl) control);
                }
            }

            propertyGrid.SelectedObjects = this.selectedControls.ToArray();
            this.RedrawLayout();
            this.RebuildListBox(false);
        }

        public void AddControl(MslControl control) {
            this.AddControl(control, true);
        }
        private void AddControl(MslControl control, bool addText) {
            if (control is MslControl.MslTab) {
                if (this.tabControl != null)
                    throw new InvalidOperationException("Only one tab control is allowed per dialog.");
                this.tabControl = (MslControl.MslTab) control;
                toolboxTabButton.Enabled = false;
                toolboxTabButton.ToolTipText = "Only one tab control is allowed per dialog.";
            }

            control.ID = nextID;
            ++nextID;

            control.StyleChanged += Control_StyleChanged;
            control.TextChanged += Control_TextChanged;

            if (control is MslMovableControl) {
                var mcontrol = (MslMovableControl) control;
                applyTheme(mcontrol);
                mcontrol.RedrawControl();

                mcontrol.RectangleChanged += Control_RectangleChanged;
                mcontrol.TabIndexChanged += Control_TabIndexChanged;
                this.controls.Add(mcontrol);
                layoutPanel.Controls.Add(mcontrol.Control);
            }

            if (addText) {
                this.AddLine(control.ToString());
                control.lineNumber = this.lines.Count - 2;
            }
        }

        public void AddMenu(MslMenu menu) {
            this.AddMenu(menu, true);
        }
        private void AddMenu(MslMenu menu, bool addText) {
            menu.control.Click += MenuItem_Click;
            menu.dropDown.Closing += DropDown_Closing;
            menu.newMenuItem.Click += this.newMenuItem_Click;
            menu.newItemItem.Click += this.newItemItem_Click;
            this.menus.Add(menu);

            if (addText) {
                this.AddLine(menu.ToString());
                menu.lineNumber = this.lines.Count - 2;
            }
        }

        private void AddLine(string text) {
            this.lines.Insert(this.lines.Count - 1, new string(' ', this.IndentCount) + text);
        }
        private void AddLine(int index, string text) {
            this.lines.Insert(index, new string(' ', this.IndentCount) + text);

            foreach (var control in this.controls) {
                if (control.lineNumber >= index) ++control.lineNumber;
            }
            foreach (var menu in this.menus) {
                if (menu.lineNumber >= index) ++menu.lineNumber;
            }

            if (this.titleLine >= index) ++this.titleLine;
            if (this.sizeLine >= index) ++this.sizeLine;
            if (this.iconLine >= index) ++this.iconLine;
            if (this.optionLine >= index) ++this.optionLine;
            if (this.controlLine >= index) ++this.controlLine;
        }

        private void Control_TabIndexChanged(object sender, CancelEventArgs e) {
            var control = (MslMovableControl) sender;
            if (control.TabIndex == 0)
                control.Control.Parent = layoutPanel;
            else {
                var index = this.tabControl.GetIndexOfTabID(control.TabIndex);
                if (index == -1)
                    e.Cancel = true;
                else
                    control.Control.Parent = ((TabControl) this.tabControl.Control).TabPages[index];
            }
            this.RedrawLayout();
        }

        public void RemoveControl(MslControl control) {
            if (this.tabControl == control) {
                this.tabControl = null;
                toolboxTabButton.Enabled = true;
                toolboxTabButton.ToolTipText = null;
            }

            int lineNumber = control.lineNumber;
            this.lines.RemoveAt(lineNumber);
            foreach (MslControl control2 in this.controls)
                if (control2.lineNumber > lineNumber) --control2.lineNumber;
            foreach (MslControl control2 in this.menus)
                if (control2.lineNumber > lineNumber) --control2.lineNumber;

            this.RebuildListBox(false);
        }

        private void RebuildListBox(bool selectionOnly) {
            if (!selectionOnly) {
                controlList.Items.Clear();
                controlList.Items.Add("Dialog " + this.Dialog.Name);

                foreach (var control in this.controls)
                    controlList.Items.Add(control.Type + " " + control.ID);
            }

            if (this.selectedControls.Count == 0)
                controlList.SelectedIndex = 0;
            else if (this.selectedControls.Count == 1)
                controlList.SelectedIndex = this.controls.IndexOf(this.selectedControls[0]) + 1;
            else
                controlList.SelectedIndex = -1;
        }

        protected override void WndProc(ref Message m) {
            // Cancel WM_FOCUS messages to avoid setting focus to a child control.
            if (m.Msg == 0x0007) return;
            base.WndProc(ref m);
        }

        private void MenuItem_Click(object sender, EventArgs e) {
            var item = (ToolStripMenuItem) sender;
            propertyGrid.SelectedObject = item.Tag;
            item.ForeColor = selectedMenuColor;

            if (this.selectedType == 0) {
                if (!ModifierKeys.HasFlag(Keys.Control)) {
                    this.selectedControls.Clear();
                    foreach (var menu in this.selectedMenus) {
                        menu.control.ForeColor = this.ForeColor;
                    }
                    this.selectedMenus.Clear();
                }

                var control = (MslMenu) item.Tag;
                this.selectedMenus.Add(control);
                control.control.ForeColor = selectedMenuColor;
            }

            propertyGrid.SelectedObjects = this.selectedMenus.ToArray();
            RebuildListBox(true);
        }

        private void newMenuItem_Click(object sender, EventArgs e) {
            var item = (ToolStripMenuItem) sender;

            var menu = new MslMenu("Menu " + nextID) { ID = nextID };
            ++nextID;
            if (item.OwnerItem != null) {
                var parent = (MslMenu) item.OwnerItem.Tag;
                menu.Parent = parent.ID;
            }

            menu.control.Click += MenuItem_Click;
            menu.dropDown.Closing += DropDown_Closing;
            menu.newMenuItem.Click += this.newMenuItem_Click;
            menu.newItemItem.Click += this.newItemItem_Click;
            this.menus.Add(menu);

            item.Owner.Items.Insert(item.Owner.Items.Count - 2, menu.control);
        }

        private void newItemItem_Click(object sender, EventArgs e) {
            var item = (ToolStripMenuItem) sender;

            var menu = new MslMenu.MslItem("Menu " + nextID) { ID = nextID };
            ++nextID;
            if (item.OwnerItem != null) {
                var parent = (MslMenu) item.OwnerItem.Tag;
                menu.Parent = parent.ID;
            }

            menu.control.Click += MenuItem_Click;
            this.menus.Add(menu);

            item.Owner.Items.Insert(item.Owner.Items.Count - 2, menu.control);
        }

        private void DropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e) {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked) e.Cancel = true;
        }

        private void viewCodeButton_Click(object sender, EventArgs e) {
            this.ViewCode?.Invoke(this, EventArgs.Empty);
        }

        private void applyTheme() {
            switch (this.theme) {
                case Theme.Default:
                    this.BackColor = SystemColors.Control;
                    this.ForeColor = SystemColors.ControlText;

                    layoutPictureBox.BackColor = SystemColors.AppWorkspace;

                    menuBar.Renderer = null;
                    toolbox.Renderer = null;

                    propertyGrid.BackColor = SystemColors.Control;
                    propertyGrid.CategoryForeColor = SystemColors.ControlText;
                    //propertyGrid.CategorySplitterColor = SystemColors.Control;
                    //propertyGrid.CommandsBorderColor = SystemColors.ControlDark;
                    propertyGrid.CommandsDisabledLinkColor = Color.FromArgb(133, 133, 133);
                    propertyGrid.CommandsForeColor = SystemColors.ControlText;
                    propertyGrid.CommandsLinkColor = Color.Blue;
                    //propertyGrid.DisabledItemForeColor = SystemColors.GrayText;
                    propertyGrid.HelpBackColor = SystemColors.Control;
                    //propertyGrid.HelpBorderColor = SystemColors.ControlDark;
                    propertyGrid.HelpForeColor = SystemColors.ControlText;
                    propertyGrid.LineColor = SystemColors.InactiveBorder;
                    propertyGrid.ViewBackColor = SystemColors.Window;
                    //propertyGrid.ViewBorderColor = SystemColors.ControlDark;
                    propertyGrid.ViewForeColor = SystemColors.WindowText;

                    SetMenuColours(menuBar.Items, SystemColors.Control, SystemColors.Highlight);

                    break;
                case Theme.Dark:
                    this.BackColor = Color.FromArgb(32, 32, 32);
                    this.ForeColor = Color.White;

                    layoutPictureBox.BackColor = Color.Black;

                    menuBar.Renderer = new ToolStripProfessionalRenderer(new DarkThemeColorTable());
                    toolbox.Renderer = new ToolStripProfessionalRenderer(new DarkThemeColorTable());

                    propertyGrid.BackColor = Color.FromArgb(32, 32, 32);
                    propertyGrid.CategoryForeColor = Color.RoyalBlue;
                    //propertyGrid.CategorySplitterColor = Color.FromArgb(32, 32, 32);
                    //propertyGrid.CommandsBorderColor = Color.FromArgb(16, 16, 16);
                    propertyGrid.CommandsDisabledLinkColor = Color.Gray;
                    propertyGrid.CommandsForeColor = Color.White;
                    propertyGrid.CommandsLinkColor = Color.RoyalBlue;
                    //propertyGrid.DisabledItemForeColor = Color.FromArgb(160, 160, 160);
                    propertyGrid.HelpBackColor = Color.FromArgb(48, 48, 48);
                    //propertyGrid.HelpBorderColor = Color.FromArgb(80, 80, 80);
                    propertyGrid.HelpForeColor = Color.White;
                    propertyGrid.LineColor = Color.FromArgb(64, 64, 64);
                    propertyGrid.ViewBackColor = Color.FromArgb(32, 32, 32);
                    //propertyGrid.ViewBorderColor = Color.FromArgb(16, 16, 16);
                    propertyGrid.ViewForeColor = Color.White;

                    SetMenuColours(menuBar.Items, Color.White, Color.SkyBlue);
                    break;
            }

            foreach (var control in this.controls)
                applyTheme(control);

            RedrawLayout();
        }
        private void applyTheme(MslMovableControl control) {
            if (control.Type == ControlType.Button ||
                (control.Type == ControlType.Check && ((MslControl.MslCheck) control).push) ||
                (control.Type == ControlType.Radio && ((MslControl.MslRadio) control).push)) {
                control.Control.BackColor = SystemColors.Control;
                control.Control.ForeColor = SystemColors.ControlText;
                ((ButtonBase) control.Control).UseVisualStyleBackColor = true;
            } else {
                control.Control.BackColor = this.BackColor;
                control.Control.ForeColor = this.ForeColor;
            }
            control.RedrawControl();
        }

        private void SetMenuColours(ToolStripItemCollection items, Color foreground, Color foreground2) {
            foreach (ToolStripItem item in items) {
                item.ForeColor = foreground;
                if (item is ToolStripMenuItem) SetMenuColours(((ToolStripMenuItem) item).DropDownItems, foreground, foreground2);
            }
            if (items.Count >= 2) {
                items[items.Count - 2].ForeColor = foreground2;
                items[items.Count - 1].ForeColor = foreground2;
            }
        }

        private void layoutPictureBox_Resize(object sender, EventArgs e) {
            layoutImage = new Bitmap(layoutPictureBox.Width, layoutPictureBox.Height);
            layoutGraphics = Graphics.FromImage(layoutImage);

            RedrawLayout();
            layoutPictureBox.Refresh();
        }

        private void DialogDesigner_Load(object sender, EventArgs e) {
            if (DesignMode) {
                toolbox.Enabled = false;
                menuBar.Enabled = false;
            } else
                this.propertyGrid.SelectedObject = this.Dialog;

            ResizeLayout();
            RedrawLayout();

            layoutPictureBox.Focus();
        }

        private void placeResizeHandles() {
            resizeHandles.Clear();
            if (this.DesignMode) return;

            if (this.selectedControls.Count == 0) {
                // No selected controls; place resize handles on the dialog.
                resizeHandles.Add(new Rectangle(layoutOffset.Width + this.Dialog.SizePixels.Width / 2 - 4, layoutOffset.Height + this.Dialog.SizePixels.Height + 4 - 4, 7, 7));
                resizeHandles.Add(new Rectangle(layoutOffset.Width + this.Dialog.SizePixels.Width + 4 - 4, layoutOffset.Height + this.Dialog.SizePixels.Height + 4 - 4, 7, 7));
                resizeHandles.Add(new Rectangle(layoutOffset.Width + this.Dialog.SizePixels.Width + 4 - 4, layoutOffset.Height + this.Dialog.SizePixels.Height / 2 - 4, 7, 7));
            } else {
                foreach (MslMovableControl control in this.selectedControls) {
                    Point location;
                    if (control.tab)
                        location = control.Control.Location + new Size(this.tabControl.Control.Location);
                    else
                        location = control.Control.Location;

                    resizeHandles.Add(new Rectangle(layoutOffset.Width + location.X + control.Control.Width / 2 - 4, layoutOffset.Height + location.Y + control.Control.Height     - 4, 7, 7));
                    resizeHandles.Add(new Rectangle(layoutOffset.Width + location.X + control.Control.Width     - 4, layoutOffset.Height + location.Y + control.Control.Height     - 4, 7, 7));
                    resizeHandles.Add(new Rectangle(layoutOffset.Width + location.X + control.Control.Width     - 4, layoutOffset.Height + location.Y + control.Control.Height / 2 - 4, 7, 7));
                    resizeHandles.Add(new Rectangle(layoutOffset.Width + location.X + control.Control.Width     - 4, layoutOffset.Height + location.Y                              - 4, 7, 7));
                    resizeHandles.Add(new Rectangle(layoutOffset.Width + location.X + control.Control.Width / 2 - 4, layoutOffset.Height + location.Y                              - 4, 7, 7));
                    resizeHandles.Add(new Rectangle(layoutOffset.Width + location.X                             - 4, layoutOffset.Height + location.Y                              - 4, 7, 7));
                    resizeHandles.Add(new Rectangle(layoutOffset.Width + location.X                             - 4, layoutOffset.Height + location.Y + control.Control.Height / 2 - 4, 7, 7));
                    resizeHandles.Add(new Rectangle(layoutOffset.Width + location.X                             - 4, layoutOffset.Height + location.Y + control.Control.Height     - 4, 7, 7));
                }
            }
        }
    }
}
