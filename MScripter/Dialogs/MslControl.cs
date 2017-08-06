using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MScripter {
    public abstract class MslStylable {
        [Browsable(false)]
        public List<string> Styles { get; private set; } = new List<string>();

        public event EventHandler StyleChanged;

        protected void OnStyleChanged(EventArgs e) { this.StyleChanged?.Invoke(this, e); }

        protected MslStylable(string style) {
            if (style != null) {
                string[] fields = style.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int i;
                StringBuilder builder = new StringBuilder();
                for (i = 0; i < fields.Length; ++i) {
                    builder.Append(fields[i]);
                    // Some styles can have one or more integral parameters.
                    for (++i; i < fields.Length; ++i) {
                        int value;
                        if (int.TryParse(fields[i], System.Globalization.NumberStyles.None, null, out value)) {
                            builder.Append(" ");
                            builder.Append(fields[i]);
                        } else
                            break;
                    }

                    this.Styles.Add(builder.ToString());
                    builder.Clear();
                }
            }
        }

        public virtual void SetStyle(string style, bool active) {
            if (active) {
                if (!this.Styles.Contains(style)) this.Styles.Add(style);
            } else
                this.Styles.Remove(style);

            this.OnStyleChanged(EventArgs.Empty);
        }
        public virtual void SetStyle1(string style, int param) {
            for (int i = 0; i < this.Styles.Count; ++i) {
                if (this.Styles[i].StartsWith(style + " ", StringComparison.OrdinalIgnoreCase)) {
                    this.Styles[i] = style + " " + param.ToString();
                    this.OnStyleChanged(EventArgs.Empty);
                    return;
                }
            }
            this.Styles.Add(style + " " + param.ToString());
            this.OnStyleChanged(EventArgs.Empty);
        }
        public virtual void SetStyle2(string style, int param1, int param2) {
            for (int i = 0; i < this.Styles.Count; ++i) {
                if (this.Styles[i].StartsWith(style + " ", StringComparison.OrdinalIgnoreCase)) {
                    this.Styles[i] = style + " " + param1.ToString() + " " + param2.ToString();
                    this.OnStyleChanged(EventArgs.Empty);
                    return;
                }
            }
            this.Styles.Add(style + " " + param1.ToString() + " " + param2.ToString());
            this.OnStyleChanged(EventArgs.Empty);
        }

        public void RemoveStyle(string style) {
            for (int i = 0; i < this.Styles.Count; ++i) {
                if (this.Styles[i].StartsWith(style + " ", StringComparison.OrdinalIgnoreCase)) {
                    this.Styles.RemoveAt(i);
                    this.OnStyleChanged(EventArgs.Empty);
                    return;
                }
            }
        }

        public int GetStyle1(string style) {
            for (int i = 0; i < this.Styles.Count; ++i) {
                if (this.Styles[i].StartsWith(style + " ", StringComparison.OrdinalIgnoreCase)) {
                    return int.Parse(this.Styles[i].Substring(style.Length + 1));
                }
            }
            return -1;
        }
        public int[] GetStyle2(string style) {
            for (int i = 0; i < this.Styles.Count; ++i) {
                if (this.Styles[i].StartsWith(style + " ", StringComparison.OrdinalIgnoreCase)) {
                    int delimiter = this.Styles[i].IndexOf(' ', style.Length + 2);
                    return new[] {
                        int.Parse(this.Styles[i].Substring(style.Length + 1, delimiter - (style.Length + 1))),
                        int.Parse(this.Styles[i].Substring(delimiter)),
                    };
                }
            }
            return null;
        }
    }

    public abstract partial class MslControl : MslStylable {
        public static readonly Regex ParseRegex = new Regex(@"^ *([^ ]+) *""?((?<="")(?>[^""]*)|(?<!"")(?>[^,]*))""?,? *(\d+)[^,]*, *(?>(\d+)) *(?>(\d+)) *(?>(\d+)) *(?>(\d+))[^,]*(?:, *(.*))?", RegexOptions.Compiled);
        public static readonly Regex ParseRegexList = new Regex(@"^ *([^ ]+) *(\d+)[^,]*, *(?>(\d+)) *(?>(\d+)) *(?>(\d+)) *(?>(\d+))[^,]*(?:, *(.*))?", RegexOptions.Compiled);
        public static readonly Regex ParseRegexIcon = new Regex(@"^ *([^ ]+) *(\d+)[^,]*, *(?>(\d+)) *(?>(\d+)) *(?>(\d+)) *(?>(\d+))[^,]*, *([^,]*), (\d+) *(?:, *(.*))?", RegexOptions.Compiled);
        public static readonly Regex ParseRegexMenu = new Regex(@"^ *([^ ]+) *""?((?<="")(?>[^""]*)|(?<!"")(?>[^,]*))""?,? *(\d+)[^,]*(?:, *(\d+))?", RegexOptions.Compiled);
        public static readonly Regex ParseRegexItem = new Regex(@"^ *([^ ]+) *""?((?<="")(?>[^""]*)|(?<!"")(?>[^,]*))""?,? *(\d+)[^,]*(?:, *(.*))?", RegexOptions.Compiled);

        [Browsable(false)]
        public ControlType Type { get; }

        [Category("Identity")]
        [ParenthesizePropertyName(true)]
        [Description("A unique numeric ID used to refer to the control.")]
        public int ID { get; set; }

        [Category("Style (general)")]
        [ParenthesizePropertyName(true)]
        [Description("The style code for the control.")]
        public string Style => string.Join(" ", this.Styles);

        public event EventHandler TextChanged;

        internal int lineNumber;

        protected MslControl(ControlType type) : this(type, null) { }
        protected MslControl(ControlType type, string style) : base(style) {
            this.Type = type;
        }

        protected void OnTextChanged(EventArgs e) { this.TextChanged?.Invoke(this, e); }

        public static MslControl Parse(MslDialog dialog, string s) {
            s = s.Trim();
            int space = s.IndexOf(' ');
            if (space == -1) throw new FormatException("Invalid code: missing ID.");
            string type = s.Substring(0, space);

            if (type.Equals("text", StringComparison.OrdinalIgnoreCase))
                return MslText.Parse(dialog, s);
            if (type.Equals("edit", StringComparison.OrdinalIgnoreCase))
                return MslEdit.Parse(dialog, s);
            if (type.Equals("button", StringComparison.OrdinalIgnoreCase))
                return MslButton.Parse(dialog, s);
            if (type.Equals("check", StringComparison.OrdinalIgnoreCase))
                return MslCheck.Parse(dialog, s);
            if (type.Equals("radio", StringComparison.OrdinalIgnoreCase))
                return MslRadio.Parse(dialog, s);
            if (type.Equals("box", StringComparison.OrdinalIgnoreCase))
                return MslBox.Parse(dialog, s);
            if (type.Equals("scroll", StringComparison.OrdinalIgnoreCase))
                return MslScroll.Parse(dialog, s);
            if (type.Equals("list", StringComparison.OrdinalIgnoreCase))
                return MslList.Parse(dialog, s);
            if (type.Equals("combo", StringComparison.OrdinalIgnoreCase))
                return MslCombo.Parse(dialog, s);
            if (type.Equals("icon", StringComparison.OrdinalIgnoreCase))
                return MslIcon.Parse(dialog, s);
            if (type.Equals("link", StringComparison.OrdinalIgnoreCase))
                return MslLink.Parse(dialog, s);
            if (type.Equals("tab", StringComparison.OrdinalIgnoreCase))
                return MslTab.Parse(dialog, s);
            if (type.Equals("menu", StringComparison.OrdinalIgnoreCase))
                return MslMenu.Parse(s);
            if (type.Equals("item", StringComparison.OrdinalIgnoreCase))
                return MslMenu.MslItem.Parse(s);

            throw new FormatException("Invalid code: no such control type as '" + type + ".");
        }
        public static bool TryParse(MslDialog dialog, string s, out MslControl control) {
            bool result;

            s = s.Trim();
            int space = s.IndexOf(' ');
            if (space == -1) { control = null; return false; }
            string type = s.Substring(0, space);

            if (type.Equals("text", StringComparison.OrdinalIgnoreCase)) {
                MslText control2;
                result = MslText.TryParse(dialog, s, out control2);
                control = control2;
            } else if (type.Equals("edit", StringComparison.OrdinalIgnoreCase)) {
                MslEdit control2;
                result = MslEdit.TryParse(dialog, s, out control2);
                control = control2;
            } else if (type.Equals("button", StringComparison.OrdinalIgnoreCase)) {
                MslButton control2;
                result = MslButton.TryParse(dialog, s, out control2);
                control = control2;
            } else if (type.Equals("check", StringComparison.OrdinalIgnoreCase)) {
                MslCheck control2;
                result = MslCheck.TryParse(dialog, s, out control2);
                control = control2;
            } else if (type.Equals("radio", StringComparison.OrdinalIgnoreCase)) {
                MslRadio control2;
                result = MslRadio.TryParse(dialog, s, out control2);
                control = control2;
            } else if (type.Equals("box", StringComparison.OrdinalIgnoreCase)) {
                MslBox control2;
                result = MslBox.TryParse(dialog, s, out control2);
                control = control2;
            } else if (type.Equals("scroll", StringComparison.OrdinalIgnoreCase)) {
                MslScroll control2;
                result = MslScroll.TryParse(dialog, s, out control2);
                control = control2;
            } else if (type.Equals("list", StringComparison.OrdinalIgnoreCase)) {
                MslList control2;
                result = MslList.TryParse(dialog, s, out control2);
                control = control2;
            } else if (type.Equals("combo", StringComparison.OrdinalIgnoreCase)) {
                MslCombo control2;
                result = MslCombo.TryParse(dialog, s, out control2);
                control = control2;
            } else if (type.Equals("icon", StringComparison.OrdinalIgnoreCase)) {
                MslIcon control2;
                result = MslIcon.TryParse(dialog, s, out control2);
                control = control2;
            } else if (type.Equals("link", StringComparison.OrdinalIgnoreCase)) {
                MslLink control2;
                result = MslLink.TryParse(dialog, s, out control2);
                control = control2;
            } else if (type.Equals("tab", StringComparison.OrdinalIgnoreCase)) {
                MslTab control2;
                result = MslTab.TryParse(dialog, s, out control2);
                control = control2;
            } else if (type.Equals("menu", StringComparison.OrdinalIgnoreCase)) {
                MslMenu control2;
                result = MslMenu.TryParse(s, out control2);
                control = control2;
            } else if (type.Equals("item", StringComparison.OrdinalIgnoreCase)) {
                MslMenu.MslItem control2;
                result = MslMenu.MslItem.TryParse(s, out control2);
                control = control2;
            } else {
                control = null;
                return false;
            }

            return result;
        }
    }

    public abstract partial class MslMovableControl : MslControl {
        [Browsable(false)]
        public MslDialog Dialog { get; }
        private Control control;
        [Browsable(false)]
        public Control Control {
            get { return this.control; }
            protected set {
                this.control = value;
                ResizeControl();
            }
        }

        internal Bitmap image;

        private int x, y, width, height;
        internal int startX, startY, startWidth, startHeight;

        [Category("Position")]
        [Description("The distance between the left edge of the control and the left edge of its container.")]
        public int X { get { return this.x; } set { this.x = value; this.UpdateRectangle(); } }
        [Category("Position")]
        [Description("The distance between the top edge of the control and the top edge of its container.")]
        public int Y { get { return this.y; } set { this.y = value; this.UpdateRectangle(); } }
        [Category("Position")]
        [Description("The width of the control.")]
        public int Width { get { return this.width; } set { this.width = value; this.UpdateRectangle(); } }
        [Category("Position")]
        [Description("The height of the control.")]
        public int Height { get { return this.height; } set { this.height = value; this.UpdateRectangle(); } }

        [Category("Position")]
        [Description("The ID of the tab containing the control. Set this to 0 to remove the control from all tabs.")]
        [DefaultValue(0)]
        public int TabIndex {
            get {
                var value = this.GetStyle1("tab");
                if (value == -1) return 0;
                return value;
            }
            set {
                int oldValue = this.TabIndex;
                this.SetStyle1("tab", value);
                CancelEventArgs e = new CancelEventArgs();
                this.TabIndexChanged?.Invoke(this, e);
                if (e.Cancel) {
                    this.SetStyle1("tab", oldValue);
                    throw new ArgumentException("The specified tab index is not valid.");
                }
            }
        }

        [Browsable(false)]
        public bool tab {
            get { return this.TabIndex != 0; }
        }

        [Category("Style (general)")]
        [Description("If set, disables the control.")]
        public bool disable {
            get { return this.Styles.Contains("disable"); }
            set { this.Control.Enabled = !value; this.SetStyle("disable", value); }
        }
        [Category("Style (general)")]
        [Description("If set, hides the control.")]
        public bool hide {
            get { return this.Styles.Contains("hide"); }
            set { this.SetStyle("hide", value); }
        }
        [Category("Style (general)")]
        [Description("If set, the text in the control is returned when a modal dialog exits.")]
        public bool result {
            get { return this.Styles.Contains("result"); }
            set { this.SetStyle("result", value); }
        }

        public event EventHandler RectangleChanged;
        public event EventHandler<CancelEventArgs> TabIndexChanged;

        protected MslMovableControl(ControlType type, MslDialog dialog, int x, int y, int width, int height) : this(type, dialog, x, y, width, height, null) { }
        protected MslMovableControl(ControlType type, MslDialog dialog, int x, int y, int width, int height, string style) : base(type, style) {
            if (dialog == null) throw new ArgumentNullException("dialog");

            this.Dialog = dialog;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            this.UpdateRectangle();
        }

        public override void SetStyle(string style, bool active) {
            this.RedrawControl();
            base.SetStyle(style, active);
        }

        protected internal void ResizeControl() {
            Point location;
            Size size;

            if (this.Dialog.SizeUnit == DialogSizeUnit.DBU) {
                var dbuSize = Program.GetDialogBaseUnitSize();
                location = new Point(this.x * dbuSize.Width, this.y * dbuSize.Height);
                size = new Size(this.width * dbuSize.Width, this.height * dbuSize.Height);
            } else {
                location = new Point(this.x, this.y);
                size = new Size(this.width, this.height);
            }

            if (this.Control != null) {
                this.Control.Location = location;
                this.Control.Size = size;
                RedrawControl();
            }
        }
        protected internal void RedrawControl() {
            if (this.Control != null) {
                this.image = new Bitmap(this.control.Width, this.control.Height);
                this.Control.DrawToBitmap(this.image, new Rectangle(Point.Empty, this.Control.Size));
            }
        }
        protected internal virtual void UpdateRectangle() {
            this.ResizeControl();
            this.OnRectangleChanged(EventArgs.Empty);
        }
        protected void OnRectangleChanged(EventArgs e) { this.RectangleChanged?.Invoke(this, e); }
    }

}
