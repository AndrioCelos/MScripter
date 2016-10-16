using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MIDE {
    public partial class MslControl {
        public class MslCheck : MslMovableControl {
            [Category("State")]
            [Description("The text associated with the control.")]
            public string Text { get { return ((CheckBox) this.Control).Text; } set { ((CheckBox) this.Control).Text = value; this.OnTextChanged(EventArgs.Empty); } }

            [Category("Style")]
            [Description("Places the text to the left of the check box.")]
            public bool left {
                get { return this.Styles.Contains("left"); }
                set { ((CheckBox) this.Control).CheckAlign = value ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft; this.SetStyle("left", value); }
            }
            [Category("Style")]
            [Description("Creates a toggle button instead of a check box.")]
            public bool push {
                get { return this.Styles.Contains("push"); }
                set { ((CheckBox) this.Control).Appearance = value ? Appearance.Button : Appearance.Normal; this.SetStyle("push", value); }
            }
            [Category("Style")]
            [Description("If set, the check box can be set to an 'indeterminate' state.")]
            public bool threestate {
                get { return this.Styles.Contains("3state"); }
                set { this.SetStyle("3state", value); }
            }

            public MslCheck(MslDialog dialog, int x, int y, int width, int height, string text, string style = null) : base(ControlType.Check, dialog, x, y, width, height, style) {
                this.Control = new CheckBox() { Left = x, Top = y, Width = width, Height = height, Text = text };
            }

            public static new MslCheck Parse(MslDialog dialog, string s) {
                var match = MslControl.ParseRegex.Match(s);
                // TODO: More descriptive exception message.
                if (!match.Success) throw new FormatException();

                return new MslCheck(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
            }
            public static bool TryParse(MslDialog dialog, string s, out MslCheck control) {
                var match = MslControl.ParseRegex.Match(s);
                if (!match.Success) { control = null; return false; }

                control = new MslCheck(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
                return true;
            }

            public override string ToString() {
                return this.ToString("{0} \"{1}\", {2}, {3} {4} {5} {6}, {7}");
            }
            public string ToString(string format) {
                return string.Format(format, "check", this.Text, this.ID, this.X, this.Y, this.Width, this.Height, this.Style).TrimEnd(new char[] { ' ', ',' });
            }
        }
    }
}
