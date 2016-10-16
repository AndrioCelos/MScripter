using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MIDE {
    public partial class MslControl {
        public class MslText : MslMovableControl {
            [Category("State")]
            [Description("The text associated with the control.")]
            public string Text { get { return ((Label) this.Control).Text; } set { ((Label) this.Control).Text = value; } }

            [Category("Style")]
            [Description("Right-aligns the text in the control.")]
            public bool right {
                get { return this.Styles.Contains("right"); }
                set { 
                    if (value) ((Label) this.Control).TextAlign = ContentAlignment.TopRight;
                    else ((Label) this.Control).TextAlign = this.center ? ContentAlignment.TopCenter : ContentAlignment.TopLeft;
                    this.SetStyle("right", value); if (value) this.center = false;
                }
            }
            [Category("Style")]
            [Description("Centers the text in the control.")]
            public bool center {
                get { return this.Styles.Contains("center"); }
                set {
                    if (value) ((Label) this.Control).TextAlign = ContentAlignment.TopCenter;
                    else ((Label) this.Control).TextAlign = this.right ? ContentAlignment.TopRight : ContentAlignment.TopLeft;
                    this.SetStyle("center", value); if (value) this.right = false;
                }
            }
            [Category("Style")]
            [Description("If set, the text in the control will not wrap to multiple lines.")]
            public bool nowrap {
                get { return this.Styles.Contains("nowrap"); }
                set { this.SetStyle("nowrap", value); }
            }

            public MslText(MslDialog dialog, int x, int y, int width, int height, string text, string style = null) : base(ControlType.Text, dialog, x, y, width, height, style) {
                this.Control = new Label() { AutoSize = false, Left = x, Top = y, Width = width, Height = height, Text = text };
            }

            public static new MslText Parse(MslDialog dialog, string s) {
                var match = MslControl.ParseRegex.Match(s);
                // TODO: More descriptive exception message.
                if (!match.Success) throw new FormatException();

                return new MslText(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
            }
            public static bool TryParse(MslDialog dialog, string s, out MslText control) {
                var match = MslControl.ParseRegex.Match(s);
                if (!match.Success) { control = null; return false; }

                control = new MslText(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
                return true;
            }

            public override string ToString() {
                return this.ToString("{0} \"{1}\", {2}, {3} {4} {5} {6}, {7}");
            }
            public string ToString(string format) {
                return string.Format(format, "text", this.Text, this.ID, this.X, this.Y, this.Width, this.Height, this.Style).TrimEnd(new char[] { ' ', ',' });
            }
        }
    }
}
