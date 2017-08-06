using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MScripter {
    public partial class MslControl {
        public class MslCombo : MslMovableControl {
            [Category("Style")]
            [Description("Sorts the items in the list.")]
            public bool sort {
                get { return this.Styles.Contains("sort"); }
                set { ((ComboBox) this.Control).Sorted = value; this.SetStyle("sort", value); }
            }
            [Category("Style")]
            [Description("Allows the user to edit the text of a drop-down list.")]
            public bool edit {
                get { return this.Styles.Contains("edit"); }
                set {
                    if (this.drop) ((ComboBox) this.Control).DropDownStyle = value ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
                    this.SetStyle("edit", value);
                }
            }
            [Category("Style")]
            [Description("Creates a drop-down list.")]
            public bool drop {
                get { return this.Styles.Contains("drop"); }
                set {
                    ((ComboBox) this.Control).DropDownStyle = value ? (this.edit ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList) : ComboBoxStyle.Simple;
                    this.SetStyle("drop", value);
                }
            }
            [Category("Style")]
            public bool size {
                get { return this.Styles.Contains("size"); }
                set { this.SetStyle("size", value); }
            }
            [Category("Style")]
            [Description("If set, the vertical scroll bar will always be shown.")]
            public bool vsbar {
                get { return this.Styles.Contains("vsbar"); }
                set { this.SetStyle("vsbar", value); }
            }
            [Category("Style")]
            [Description("If set, the horizontal scroll bar will always be shown.")]
            public bool hsbar {
                get { return this.Styles.Contains("hsbar"); }
                set { this.SetStyle("hsbar", value); }
            }

            public MslCombo(MslDialog dialog, int x, int y, int width, int height, string style = null) : base(ControlType.Combo, dialog, x, y, width, height, style) {
                this.Control = new ComboBox() { IntegralHeight = false, DropDownStyle = ComboBoxStyle.Simple, Left = x, Top = y, Width = width, Height = height };
            }

            public static new MslCombo Parse(MslDialog dialog, string s) {
                var match = MslControl.ParseRegexList.Match(s);
                // TODO: More descriptive exception message.
                if (!match.Success) throw new FormatException();

                return new MslCombo(dialog, int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), match.Groups[7].Value) {
                    ID = int.Parse(match.Groups[2].Value)
                };
            }
            public static bool TryParse(MslDialog dialog, string s, out MslCombo control) {
                var match = MslControl.ParseRegexList.Match(s);
                if (!match.Success) { control = null; return false; }

                control = new MslCombo(dialog, int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), match.Groups[7].Value) {
                    ID = int.Parse(match.Groups[2].Value)
                };
                return true;
            }

            public override string ToString() {
                return this.ToString("{0} {2}, {3} {4} {5} {6}, {7}");
            }
            public string ToString(string format) {
                return string.Format(format, "combo", null, this.ID, this.X, this.Y, this.Width, this.Height, this.Style).TrimEnd(new char[] { ' ', ',' });
            }
        }
    }
}
