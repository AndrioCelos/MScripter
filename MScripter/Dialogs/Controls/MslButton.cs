using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MScripter {
    public partial class MslControl {
        public class MslButton : MslMovableControl {
            [Category("State")]
            [Description("The text associated with the control.")]
            public string Text { get { return ((Button) this.Control).Text; } set { ((Button) this.Control).Text = value; this.OnTextChanged(EventArgs.Empty); } }

            [Category("Style")]
            [Description("Sets the button to be the default button.")]
            public bool @default {
                get { return this.Styles.Contains("default"); }
                set { this.SetStyle("default", value); }
            }
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
            [Category("Style")]
            [Description("Gives the button a flat appearance.")]
            public bool flat {
                get { return this.Styles.Contains("flat"); }
                set { ((Button) this.Control).FlatStyle = value ? FlatStyle.Flat : FlatStyle.Standard; this.SetStyle("flat", value); }
            }
            [Category("Style")]
            [Description("Allows the text in the button to wrap to multiple lines.")]
            public bool multi {
                get { return this.Styles.Contains("multi"); }
                set { this.SetStyle("multi", value); }
            }

            public MslButton(MslDialog dialog, int x, int y, int width, int height, string text, string style = null) : base(ControlType.Button, dialog, x, y, width, height, style) {
                this.Control = new Button() { Left = x, Top = y, Width = width, Height = height, Text = text, UseVisualStyleBackColor = true, ForeColor = SystemColors.ControlText };
            }

            public static new MslButton Parse(MslDialog dialog, string s) {
                var match = MslControl.ParseRegex.Match(s);
                // TODO: More descriptive exception message.
                if (!match.Success) throw new FormatException();

                return new MslButton(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
            }
            public static bool TryParse(MslDialog dialog, string s, out MslButton control) {
                var match = MslControl.ParseRegex.Match(s);
                if (!match.Success) { control = null; return false; }

                control = new MslButton(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
                return true;
            }

            public override string ToString() {
                return this.ToString("{0} \"{1}\", {2}, {3} {4} {5} {6}, {7}");
            }
            public string ToString(string format) {
                return string.Format(format, "button", this.Text, this.ID, this.X, this.Y, this.Width, this.Height, this.Style).TrimEnd(new char[] { ' ', ',' });
            }
        }
    }
}
