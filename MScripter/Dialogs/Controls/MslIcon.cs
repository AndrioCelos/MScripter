using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MScripter {
    public partial class MslControl {
        public class MslIcon : MslMovableControl {
            [Category("State")]
            [Description("Loads an image from the specified filename.")]
            public string Filename { get; set; }
            [Category("State")]
            [Description("Specifies which image to load from files that contain multiple icons.")]
            public int Index { get; set; }

            [Category("Style")]
            [Description("Prevents the control from showing a border.")]
            public bool noborder {
                get { return this.Styles.Contains("noborder"); }
                set { ((PictureBox) this.Control).BorderStyle = value ? BorderStyle.None : BorderStyle.Fixed3D; this.SetStyle("noborder", value); }
            }
            [Category("Style")]
            [Description("Aligns the image to the top edge of the control.")]
            public bool top {
                get { return this.Styles.Contains("top"); }
                set {
                    this.SetStyle("top", value);
                    if (value) { this.bottom = false; this.left = false; this.right = false; }
                    //this.SetAlignment();
                }
            }
            [Category("Style")]
            [Description("Aligns the image to the left edge of the control.")]
            public bool left {
                get { return this.Styles.Contains("left"); }
                set {
                    this.SetStyle("left", value);
                    if (value) { this.top = false; this.bottom = false; this.right = false; }
                    //this.SetAlignment();
                }
            }
            [Category("Style")]
            [Description("Aligns the image to the bottom edge of the control.")]
            public bool bottom {
                get { return this.Styles.Contains("bottom"); }
                set {
                    this.SetStyle("bottom", value);
                    if (value) { this.top = false; this.left = false; this.right = false; }
                    //this.SetAlignment();
                }
            }
            [Category("Style")]
            [Description("Aligns the image to the right edge of the control.")]
            public bool right {
                get { return this.Styles.Contains("right"); }
                set {
                    this.SetStyle("right", value);
                    if (value) { this.top = false; this.bottom = false; this.left = false; }
                    //this.SetAlignment();
                }
            }
            [Category("Style")]
            public bool small {
                get { return this.Styles.Contains("small"); }
                set {
                    this.SetStyle("smalll", value);
                    if (value) { this.large = false; this.actual = false; }
                }
            }
            [Category("Style")]
            public bool large {
                get { return this.Styles.Contains("large"); }
                set {
                    this.SetStyle("large", value);
                    if (value) { this.small = false; this.actual = false; }
                }
            }
            [Category("Style")]
            public bool actual {
                get { return this.Styles.Contains("actual"); }
                set {
                    this.SetStyle("actual", value);
                    if (value) { this.small = false; this.large = false; }
                }
            }

            public MslIcon(MslDialog dialog, int x, int y, int width, int height, string style = null) : base(ControlType.Icon, dialog, x, y, width, height, style) {
                this.Control = new PictureBox() { Left = x, Top = y, Width = width, Height = height, BorderStyle = BorderStyle.Fixed3D };
            }

            public static new MslIcon Parse(MslDialog dialog, string s) {
                var match = MslControl.ParseRegexIcon.Match(s);
                // TODO: More descriptive exception message.
                if (!match.Success) throw new FormatException();

                return new MslIcon(dialog, int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), match.Groups[7].Value) {
                    ID = int.Parse(match.Groups[2].Value)
                };
            }
            public static bool TryParse(MslDialog dialog, string s, out MslIcon control) {
                var match = MslControl.ParseRegexIcon.Match(s);
                if (!match.Success) { control = null; return false; }

                control = new MslIcon(dialog, int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), match.Groups[7].Value) {
                    ID = int.Parse(match.Groups[2].Value)
                };
                return true;
            }

            public override string ToString() {
                return this.ToString("{0} {2}, {3} {4} {5} {6}, {8}, {9}, {7}");
            }
            public string ToString(string format) {
                return string.Format(format, "icon", null, this.ID, this.X, this.Y, this.Width, this.Height, this.Style, this.Filename, this.Index).TrimEnd(new char[] { ' ', ',' });
            }
        }
    }
}
