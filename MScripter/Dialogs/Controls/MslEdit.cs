using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MScripter {
    public partial class MslControl {
        public class MslEdit : MslMovableControl {
            [Category("State")]
            [Description("The text associated with the control.")]
            public string Text { get { return ((TextBox) this.Control).Text; } set { ((TextBox) this.Control).Text = value; this.OnTextChanged(EventArgs.Empty); } }

            [Category("Style")]
            [Description("Right-aligns the text in the control.")]
            public bool right {
                get { return this.Styles.Contains("right"); }
                set {
                    if (value) ((TextBox) this.Control).TextAlign = HorizontalAlignment.Right;
                    else ((TextBox) this.Control).TextAlign = this.center ? HorizontalAlignment.Center : HorizontalAlignment.Left;
                    this.SetStyle("right", value); if (value) this.center = false;
                }
            }
            [Category("Style")]
            [Description("Centres the text in the control.")]
            public bool center {
                get { return this.Styles.Contains("center"); }
                set {
                    if (value) ((TextBox) this.Control).TextAlign = HorizontalAlignment.Right;
                    else ((TextBox) this.Control).TextAlign = this.right ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                    this.SetStyle("center", value); if (value) this.right = false;
                }
            }
            [Category("Style")]
            [Description("Allows the control to contain multiple lines of text.")]
            public bool multi {
                get { return this.Styles.Contains("multi"); }
                set { ((TextBox) this.Control).Multiline = value; this.SetStyle("multi", value); }
            }
            [Category("Style")]
            [Description("Masks the characters in the control as a password field.")]
            public bool pass {
                get { return this.Styles.Contains("pass"); }
                set { ((TextBox) this.Control).UseSystemPasswordChar = value; this.SetStyle("pass", value); }
            }
            [Category("Style")]
            [Description("Makes the text box read-only.")]
            public bool read {
                get { return this.Styles.Contains("read"); }
                set { ((TextBox) this.Control).ReadOnly = value; this.SetStyle("read", value); }
            }
            [Category("Style")]
            [Description("If set, the Return key will enter a newline into the control rather than pressing the OK button.")]
            public bool @return {
                get { return this.Styles.Contains("return"); }
                set { ((TextBox) this.Control).AcceptsReturn = value; this.SetStyle("return", value); }
            }
            [Category("Style")]
            [Description("If set, the horizontal scroll bar will always be shown.")]
            public bool hsbar {
                get { return this.Styles.Contains("hsbar"); }
                set { this.SetStyle("hsbar", value); this.SetScrollBars(); }
            }
            [Category("Style")]
            [Description("If set, the vertical scroll bar will always be shown.")]
            public bool vsbar {
                get { return this.Styles.Contains("vsbar"); }
                set { this.SetStyle("vsbar", value); this.SetScrollBars(); }
            }
            [Category("Style")]
            [Description("Shows the horizontal scrollbar automatically when needed.")]
            public bool autohs {
                get { return this.Styles.Contains("autohs"); }
                set { this.SetStyle("autohs", value); this.SetScrollBars(); }
            }
            [Category("Style")]
            [Description("Shows the vertical scrollbar automatically when needed.")]
            public bool autovs {
                get { return this.Styles.Contains("autovs"); }
                set { this.SetStyle("autovs", value); this.SetScrollBars(); }
            }
            [Category("Style")]
            [DisplayName("Limit")]
            [Description("Limits the text to the specified number of characters.")]
            [DefaultValue(int.MaxValue)]
            public int limit {
                get {
                    var value = this.GetStyle1("limit");
                    if (value == -1) return int.MaxValue;
                    return value;
                }
                set {
                    if (value < 0) throw new ArgumentException("limit cannot be negative.");
                    if (value == int.MaxValue) this.RemoveStyle("limit");
                    else this.SetStyle1("limit", value);
                }
            }
            [Category("Style")]
            [Description("Creates a rich text box.")]
            public bool rich {
                get { return this.Styles.Contains("rich"); }
                set { this.SetStyle("rich", value); }
            }

            public MslEdit(MslDialog dialog, int x, int y, int width, int height, string text, string style = null) : base(ControlType.Edit, dialog, x, y, width, height, style) {
                this.Control = new TextBox() { Multiline = true, Left = x, Top = y, Width = width, Height = height, Text = text };
            }

            private void SetScrollBars() {
                ScrollBars value = 0;
                if (this.autohs || this.hsbar) value |= ScrollBars.Horizontal;
                if (this.autovs || this.vsbar) value |= ScrollBars.Vertical;
                ((TextBox) this.Control).ScrollBars = value;
                this.OnStyleChanged(EventArgs.Empty);
            }

            public static new MslEdit Parse(MslDialog dialog, string s) {
                var match = MslControl.ParseRegex.Match(s);
                // TODO: More descriptive exception message.
                if (!match.Success) throw new FormatException();

                return new MslEdit(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
            }
            public static bool TryParse(MslDialog dialog, string s, out MslEdit control) {
                var match = MslControl.ParseRegex.Match(s);
                if (!match.Success) { control = null; return false; }

                control = new MslEdit(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
                return true;
            }

            public override string ToString() {
                return this.ToString("{0} \"{1}\", {2}, {3} {4} {5} {6}, {7}");
            }
            public string ToString(string format) {
                return string.Format(format, "edit", this.Text, this.ID, this.X, this.Y, this.Width, this.Height, this.Style).TrimEnd(new char[] { ' ', ',' });
            }
        }
    }
}
