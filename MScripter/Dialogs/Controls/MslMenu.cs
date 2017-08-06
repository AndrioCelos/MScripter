using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MScripter {
    public class MslMenu : MslControl {
        [Category("State")]
        [Description("The text associated with the control.")]
        public string Text { get { return this.control.Text; } set { this.control.Text = value; } }

        [Category("Position")]
        [Description("The ID of the menu item containing the control. Set this to -1 to make it a top-level menu.")]
        public int Parent { get; set; }

        private ToolStripMenuItem _control;
        [Browsable(false)]
        public ToolStripMenuItem control => this._control;
        [Browsable(false)]
        public ToolStripDropDown dropDown { get; }
        [Browsable(false)]
        public ToolStripItem newMenuItem { get; }
        [Browsable(false)]
        public ToolStripItem newItemItem { get; }

        public event EventHandler ParentChanged;

        public MslMenu(string text) : base(ControlType.Menu) {
            this._control = new ToolStripMenuItem(text) { ForeColor = Color.White, Tag = this };
            this.newMenuItem = new ToolStripMenuItem("Add menu") { ForeColor = Color.SkyBlue };
            this.newItemItem = new ToolStripMenuItem("Add item") { ForeColor = Color.SkyBlue };
            this.dropDown = new ContextMenuStrip() { ForeColor = Color.White, Renderer = new ToolStripProfessionalRenderer(new DarkThemeColorTable()) };
            this.dropDown.Items.Add(this.newMenuItem);
            this.dropDown.Items.Add(this.newItemItem);
            this.control.DropDown = this.dropDown;
        }
        protected MslMenu(ControlType type, string style) : base(type, style) { }

        protected void OnParentChanged(object sender, EventArgs e) { this.ParentChanged?.Invoke(sender, e); }

        public static MslMenu Parse(string s) {
            var match = MslControl.ParseRegexItem.Match(s);
            // TODO: More descriptive exception message.
            if (!match.Success) throw new FormatException();

            return new MslMenu(match.Groups[2].Value) {
                ID = int.Parse(match.Groups[3].Value),
                Parent = match.Groups[4].Success ? int.Parse(match.Groups[4].Value) : 0
            };
        }
        public static bool TryParse(string s, out MslMenu control) {
            var match = MslControl.ParseRegexItem.Match(s);
            if (!match.Success) { control = null; return false; }

            control = new MslMenu(match.Groups[2].Value) {
                ID = int.Parse(match.Groups[3].Value),
                Parent = match.Groups[4].Success ? int.Parse(match.Groups[4].Value) : 0
            };
            return true;
        }

        public class MslItem : MslMenu {
            [Category("Style")]
            [Description("The OK button closes the dialog and in modal mode returns the return value set.")]
            public bool ok {
                get { return this.Styles.Contains("ok"); }
                set { this.SetStyle("ok", value); }
            }
            [Category("Style")]
            [Description("The Cancel button closes the dialog.")]
            public bool cancel {
                get { return this.Styles.Contains("cancel"); }
                set { this.SetStyle("cancel", value); }
            }

            public MslItem(string text) : this(text, null) { }
            public MslItem(string text, string style) : base(ControlType.Item, style) {
                this._control = new ToolStripMenuItem(text) { ForeColor = Color.White, Tag = this };
            }

            public static new MslItem Parse(string s) {
                var match = MslControl.ParseRegexItem.Match(s);
                // TODO: More descriptive exception message.
                if (!match.Success) throw new FormatException();

                return new MslItem(match.Groups[2].Value, match.Groups[4].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
            }
            public static bool TryParse(string s, out MslItem control) {
                var match = MslControl.ParseRegexItem.Match(s);
                if (!match.Success) { control = null; return false; }

                control = new MslItem(match.Groups[2].Value, match.Groups[4].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
                return true;
            }
        }
    }
}
