using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MIDE {
    public partial class MslControl {
        public class MslScroll : MslMovableControl {
            [Category("State")]
            [Description("The text associated with the control.")]
            public string Text { get { return ((ScrollBar) this.Control).Text; } set { ((ScrollBar) this.Control).Text = value; this.OnTextChanged(EventArgs.Empty); } }

            [Category("Style")]
            [Description("Aligns the scroll bar to the top or left edge of its defined rectangle.")]
            public bool top {
                get { return this.Styles.Contains("top"); }
                set { this.SetStyle("top", value);
                    if (value) { this.bottom = false; this.left = false; this.right = false; }
                    this.SetPosition();
                }
            }
            [Category("Style")]
            [Description("Aligns the scroll bar to the left or top edge of its defined rectangle.")]
            public bool left {
                get { return this.Styles.Contains("left"); }
                set {
                    this.SetStyle("left", value);
                    if (value) { this.top = false; this.bottom = false; this.right = false; }
                    this.SetPosition();
                }
            }
            [Category("Style")]
            [Description("Aligns the scroll bar to the bottom or right edge of its defined rectangle.")]
            public bool bottom {
                get { return this.Styles.Contains("bottom"); }
                set {
                    this.SetStyle("bottom", value);
                    if (value) { this.top = false; this.left = false; this.right = false; }
                    this.SetPosition();
                }
            }
            [Category("Style")]
            [Description("Aligns the scroll bar to the right or bottom edge of its defined rectangle.")]
            public bool right {
                get { return this.Styles.Contains("right"); }
                set {
                    this.SetStyle("right", value);
                    if (value) { this.top = false; this.bottom = false; this.left = false; }
                    this.SetPosition();
                }
            }
            [Category("Style")]
            [Description("Creates a horizontal scroll bar instead of a vertical one.")]
            public bool horizontal {
                get { return this.Styles.Contains("horizontal"); }
                set { this.SetStyle("horizontal", value);
                    if (value) this.Control = new HScrollBar() { Location = ((ScrollBar) this.Control).Location, Size = ((ScrollBar) this.Control).Size };
                    else this.Control = new VScrollBar() { Location = ((ScrollBar) this.Control).Location, Size = ((ScrollBar) this.Control).Size };
                    this.SetPosition();
                }
            }
            [Category("Style")]
            [Description("The range of values that can be selected.")]
            [DefaultValue(null)]
            public ScrollRange range {
                get {
                    var value = this.GetStyle2("range");
                    if (value == null) return ScrollRange.Empty;
                    return new ScrollRange(value[0], value[1]);
                }
                set {
                    if (value.IsEmpty) this.RemoveStyle("range");
                    else this.SetStyle2("range", value.Minimum, value.Maximum);
                }
            }

            public MslScroll(MslDialog dialog, int x, int y, int width, int height, string text, string style = null) : base(ControlType.Scroll, dialog, x, y, width, height, style) {
                this.Control = new VScrollBar() { Text = text };
                this.UpdateRectangle();
            }

            protected internal override void UpdateRectangle() {
                if (this.Control == null) return;
                this.SetPosition();
                this.OnRectangleChanged(EventArgs.Empty);
            }

            private void SetPosition() {
                var control = (ScrollBar) this.Control;
                if (this.horizontal) {
                    if (this.left || this.top) control.Top = this.Y;
                    else control.Top = this.Y + this.Height - 17;
                    control.Left = this.X;
                    control.Width = this.Width;
                    control.Height = 17;
                } else {
                    if (this.top || this.left) control.Left = this.X;
                    else control.Left = this.X + this.Width - 17;
                    control.Top = this.Y;
                    control.Width = 17;
                    control.Height = this.Height;
                }
                this.RedrawControl();
            }

            private void SetPositionAndUpdate() {
                this.SetPosition();
                this.OnRectangleChanged(EventArgs.Empty);
            }

            public static new MslScroll Parse(MslDialog dialog, string s) {
                var match = MslControl.ParseRegex.Match(s);
                // TODO: More descriptive exception message.
                if (!match.Success) throw new FormatException();

                return new MslScroll(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
            }
            public static bool TryParse(MslDialog dialog, string s, out MslScroll control) {
                var match = MslControl.ParseRegex.Match(s);
                if (!match.Success) { control = null; return false; }

                control = new MslScroll(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
                return true;
            }

            public override string ToString() {
                return this.ToString("{0} \"{1}\", {2}, {3} {4} {5} {6}, {7}");
            }
            public string ToString(string format) {
                return string.Format(format, "scroll", this.Text, this.ID, this.X, this.Y, this.Width, this.Height, this.Style).TrimEnd(new char[] { ' ', ',' });
            }
        }
    }
}
