using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MScripter {
    public partial class MslControl {
        public class MslBox : MslMovableControl {
            [Category("State")]
            [Description("The text associated with the control.")]
            public string Text { get { return ((GroupBox) this.Control).Text; } set { ((GroupBox) this.Control).Text = value; } }

            public MslBox(MslDialog dialog, int x, int y, int width, int height, string text, string style = null) : base(ControlType.Box, dialog, x, y, width, height, style) {
                this.Control = new GroupBox() { Left = x, Top = y, Width = width, Height = height, Text = text };
            }

            public static new MslBox Parse(MslDialog dialog, string s) {
                var match = MslControl.ParseRegex.Match(s);
                // TODO: More descriptive exception message.
                if (!match.Success) throw new FormatException();

                return new MslBox(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
            }
            public static bool TryParse(MslDialog dialog, string s, out MslBox control) {
                var match = MslControl.ParseRegex.Match(s);
                if (!match.Success) { control = null; return false; }

                control = new MslBox(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
                return true;
            }

            public override string ToString() {
                return this.ToString("{0} \"{1}\", {2}, {3} {4} {5} {6}, {7}");
            }
            public string ToString(string format) {
                return string.Format(format, "box", this.Text, this.ID, this.X, this.Y, this.Width, this.Height, this.Style).TrimEnd(new char[] { ' ', ',' });
            }
        }
    }
}
