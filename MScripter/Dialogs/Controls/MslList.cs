using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MScripter {
    public partial class MslControl {
        public class MslList : MslMovableControl {
            [Category("Style")]
            [Description("Sorts the items in the list.")]
            public bool sort {
                get { return this.Styles.Contains("sort"); }
                set { ((ListBox) this.Control).Sorted = value; this.SetStyle("sort", value); }
            }
            [Category("Style")]
            [Description("Enables extended selection mode.")]
            public bool extsel {
                get { return this.Styles.Contains("extsel"); }
                set { ((ListBox) this.Control).SelectionMode = value ? SelectionMode.MultiExtended :
                                                                       (this.multsel ? SelectionMode.MultiSimple : SelectionMode.One);
                    this.SetStyle("extsel", value);
                }
            }
            [Category("Style")]
            [Description("Allows multiple items to be selected.")]
            public bool multsel {
                get { return this.Styles.Contains("multsel"); }
                set {
                    ((ListBox) this.Control).SelectionMode = this.extsel ? SelectionMode.MultiExtended :
                                                                           (value ? SelectionMode.MultiSimple : SelectionMode.One);
                    this.SetStyle("multsel", value);
                }
            }
            public bool size {
                get { return this.Styles.Contains("size"); }
                set { this.SetStyle("size", value); }
            }
            [Category("Style")]
            [Description("If set, the vertical scroll bar will always be shown.")]
            public bool vsbar {
                get { return this.Styles.Contains("vsbar"); }
                set { ((ListBox) this.Control).ScrollAlwaysVisible = value; this.SetStyle("vsbar", value); }
            }
            [Category("Style")]
            [Description("If set, the horizontal scroll bar will always be shown.")]
            public bool hsbar {
                get { return this.Styles.Contains("hsbar"); }
                set { ((ListBox) this.Control).HorizontalScrollbar = value; this.SetStyle("hsbar", value); }
            }
            [Category("Style")]
            [Description("Displays check boxes on the list items.")]
            public bool check {
                get { return this.Styles.Contains("check"); }
                set {
                    if (value || this.radio) this.Control = new CheckedListBox() { IntegralHeight = false, Left = this.X, Top = this.Y, Width = this.Width, Height = this.Height };
                    else this.Control = new ListBox() { IntegralHeight = false, Left = this.X, Top = this.Y, Width = this.Width, Height = this.Height };
                    this.SetStyle("check", value);
                }
            }
            [Category("Style")]
            [Description("Displays radio buttons on the list items.")]
            public bool radio {
                get { return this.Styles.Contains("radio"); }
                set {
                    if (value || this.check) this.Control = new CheckedListBox() { IntegralHeight = false, Left = this.X, Top = this.Y, Width = this.Width, Height = this.Height };
                    else this.Control = new ListBox() { IntegralHeight = false, Left = this.X, Top = this.Y, Width = this.Width, Height = this.Height };
                    this.SetStyle("radio", value);
                }
            }

            public MslList(MslDialog dialog, int x, int y, int width, int height, string style = null) : base(ControlType.List, dialog, x, y, width, height, style) {
                this.Control = new ListBox() { IntegralHeight = false, Left = x, Top = y, Width = width, Height = height };
            }

            public static new MslList Parse(MslDialog dialog, string s) {
                var match = MslControl.ParseRegexList.Match(s);
                // TODO: More descriptive exception message.
                if (!match.Success) throw new FormatException();

                return new MslList(dialog, int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), match.Groups[7].Value) {
                    ID = int.Parse(match.Groups[2].Value)
                };
            }
            public static bool TryParse(MslDialog dialog, string s, out MslList control) {
                var match = MslControl.ParseRegexList.Match(s);
                if (!match.Success) { control = null; return false; }

                control = new MslList(dialog, int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), match.Groups[7].Value) {
                    ID = int.Parse(match.Groups[2].Value)
                };
                return true;
            }

            public override string ToString() {
                return this.ToString("{0} {2}, {3} {4} {5} {6}, {7}");
            }
            public string ToString(string format) {
                return string.Format(format, "list", null, this.ID, this.X, this.Y, this.Width, this.Height, this.Style).TrimEnd(new char[] { ' ', ',' });
            }
        }
    }
}
