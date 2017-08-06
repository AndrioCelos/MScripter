using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MScripter {
    public partial class MslControl {
        public class MslTab : MslMovableControl {
            private List<Rectangle> tabRectangles = new List<Rectangle>();
            private List<int> tabID = new List<int>();

            [Category("State")]
            [Description("The text associated with the control.")]
            public string Text { get { return ((TabControl) this.Control).Text; } set { ((TabControl) this.Control).Text = value; this.OnTextChanged(EventArgs.Empty); } }

            [Browsable(false)]
            public int TabCount => ((TabControl) this.Control).TabCount - 1;

            [Browsable(false)]
            public int SelectedIndex => ((TabControl) this.Control).SelectedIndex;

            public MslTab(MslDialog dialog, int x, int y, int width, int height, string text, string style = null) : base(ControlType.Tab, dialog, x, y, width, height, style) {
                var control = new TabControl() { Left = x, Top = y, Width = width, Height = height, Text = text,
                    ImageList = new ImageList()
                };
                control.ImageList.Images.Add(MScripter.Properties.Resources.newTab);
                control.TabPages.Add(new TabPage(text));
                control.TabPages.Add(new TabPage("New") { ImageIndex = 0 });

                this.tabRectangles.Add(control.GetTabRect(0));
                this.tabRectangles.Add(control.GetTabRect(1));

                this.tabID.Add(-1);

                this.Control = control;
            }

            public int GetTabID(int index) {
                if (index == 0 && this.tabID[0] == -1) return (this.tabID[0] = this.ID);
                return this.tabID[index];
            }

            public int GetIndexOfTabID(int id) {
                for (int i = 0; i < this.tabID.Count; ++i) {
                    if (this.GetTabID(i) == id) return i;
                }
                return -1;
            }

            public string GetTabText(int index) {
                return ((TabControl) this.Control).TabPages[index].Text;
            }

            public void AddTab(int ID, string text) {
                int index = ((TabControl) this.Control).TabCount - 1;
                ((TabControl) this.Control).TabPages.Insert(index, text);
                this.tabID.Add(ID);
                this.tabRectangles.Insert(index, ((TabControl) this.Control).GetTabRect(index));
                this.tabRectangles[index + 1] = ((TabControl) this.Control).GetTabRect(index + 1);

                ((TabControl) this.Control).SelectedIndex = index;
            }

            public void SelectTab(int index) {
                ((TabControl) this.Control).SelectedIndex = index;
            }

            public void RemoveTab(int index) {
                ((TabControl) this.Control).TabPages.RemoveAt(index);
                this.tabID.RemoveAt(index);
                this.tabRectangles.RemoveAt(index);
                this.tabRectangles[this.TabCount - 1] = ((TabControl) this.Control).GetTabRect(this.TabCount - 1);
            }

            public int GetTabIndexAt(Point point) {
                for (int i = 0; i < this.tabRectangles.Count; ++i) {
                    if (this.tabRectangles[i].Contains(point)) return i;
                }
                return -1;
            }

            public static new MslTab Parse(MslDialog dialog, string s) {
                var match = MslControl.ParseRegex.Match(s);
                // TODO: More descriptive exception message.
                if (!match.Success) throw new FormatException();

                return new MslTab(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
            }
            public static bool TryParse(MslDialog dialog, string s, out MslTab control) {
                var match = MslControl.ParseRegex.Match(s);
                if (!match.Success) { control = null; return false; }

                control = new MslTab(dialog, int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), match.Groups[2].Value, match.Groups[8].Value) {
                    ID = int.Parse(match.Groups[3].Value)
                };
                return true;
            }

            public override string ToString() {
                return this.ToString("{0} \"{1}\", {2}, {3} {4} {5} {6}, {7}");
            }
            public string ToString(string format) {
                return string.Format(format, "tab", this.Text, this.ID, this.X, this.Y, this.Width, this.Height, this.Style).TrimEnd(new char[] { ' ', ',' });
            }
        }
    }
}
