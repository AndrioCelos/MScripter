using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MIDE {
    [DefaultProperty("Color")] [DefaultEvent("ColorChanged")]
    public partial class ColourComboBox : UserControl {
        private Color color = Color.Black;
        private Color defaultColor = Color.Black;
        private bool busy;

        private static readonly object DefaultLabel = "Default";
        private static readonly object InitialLabel = "Initial";
        private static readonly object Separator = "-";
        private static readonly object HexEntry = "#________";
        private static readonly object RGBAEntry = "(___, ___, ___)";
        private static readonly object DialogLabel = "Use dialog";

        public event EventHandler ColorChanged;

        protected virtual void OnColorChanged(EventArgs e) => this.ColorChanged?.Invoke(this, e);

        [DefaultValue(typeof(Color), "Window")]
        public override Color BackColor {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [DefaultValue(typeof(Color), "WindowText")]
        public override Color ForeColor {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [EditorBrowsable]
        public override string Text {
            get { return textBox.Text; }
            set { comboBox.Text = value; textBox.Text = value; }
        }

        /// <summary>
        /// Returns or sets the <see cref="System.Drawing.Color"/> selected in the control.
        /// </summary>
        public Color Color {
            get { return color; }
            set {
                busy = true;

                color = value;
                sampleBox.Visible = true;
                if (value.IsEmpty) {
                    comboBox.SelectedIndex = -1;
                    textBox.Text = "";
                } else {
                    bool found = false;
                    for (int i = 4; i < comboBox.Items.Count; ++i) {
                        if (value.Equals(comboBox.Items[i])) {
                            comboBox.SelectedIndex = i;
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        string text = DisplayColour(color);
                        if (text.StartsWith("#"))
                            comboBox.SelectedItem = HexEntry;
                        else
                            comboBox.SelectedItem = null;
                        textBox.Text = text;
                    }
                }

                busy = false;
                sampleBox.Refresh();
                this.OnColorChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Returns or sets a value indicating whether the Default option is selected.
        /// </summary>
        public bool DefaultSelected {
            get { return comboBox.SelectedItem == DefaultLabel; }
            set {
                if (value) {
                    if (comboBox.Items[0] == DefaultLabel) comboBox.SelectedIndex = 0;
                    else throw new InvalidOperationException("The Default option has been removed.");
                    comboBox_SelectionChangeCommitted(this, EventArgs.Empty);
                } else if (DefaultSelected)
                    this.Color = this.defaultColor;
            }
        }
        /// <summary>
        /// Returns or sets a value indicating whether the Initial option is selected.
        /// </summary>
        public bool InitialSelected {
            get { return comboBox.SelectedItem == InitialLabel; }
            set {
                if (value) {
                    if (comboBox.Items[0] == InitialLabel) comboBox.SelectedIndex = 0;
                    if (comboBox.Items[1] == InitialLabel) comboBox.SelectedIndex = 1;
                    else throw new InvalidOperationException("The Initial option has been removed.");
                    comboBox_SelectionChangeCommitted(this, EventArgs.Empty);
                } else if (InitialSelected)
                    this.Color = Color.Empty;
            }
        }

        /// <summary>
        /// Returns or sets the colour that the Default option selects.
        /// </summary>
        public Color DefaultColor {
            get { return defaultColor; }
            set {
                defaultColor = value;
                if (DefaultSelected) {
                    color = value;
                    sampleBox.Refresh();
                    OnColorChanged(EventArgs.Empty);
                }
            }
        }

        public bool ShowDefault {
            get { return comboBox.Items[0] == DefaultLabel; }
            set {
                if (value) {
                    if (!ShowDefault) {
                        comboBox.Items.Insert(0, DefaultLabel);
                        if (!ShowInitial) comboBox.Items.Insert(1, Separator);
                    }
                } else {
                    if (ShowDefault) {
                        if (!ShowInitial) comboBox.Items.RemoveAt(1);
                        comboBox.Items.RemoveAt(0);
                    }
                }
            }
        }

        public bool ShowInitial {
            get { return comboBox.Items[0] == InitialLabel || comboBox.Items[1] == InitialLabel; }
            set {
                if (value) {
                    if (!ShowInitial) {
                        if (ShowDefault) comboBox.Items.Insert(1, InitialLabel);
                        else {
                            comboBox.Items.Insert(0, InitialLabel);
                            comboBox.Items.Insert(1, Separator);
                        }
                    }
                } else {
                    if (ShowInitial) {
                        if (comboBox.Items[0] == InitialLabel) {
                            comboBox.Items.RemoveAt(1);
                            comboBox.Items.RemoveAt(0);
                        } else if (comboBox.Items[1] == InitialLabel) {
                            comboBox.Items.RemoveAt(1);
                        }
                    }
                }
            }
        }

        public ColourComboBox() {
            InitializeComponent();

            comboBox.Items.AddRange(new object[] {
                DefaultLabel,
                InitialLabel,
                Separator,
                HexEntry,
                RGBAEntry,
                DialogLabel,
                Separator,
                Color.Black,
                Color.Maroon,
                Color.Olive,
                Color.Green,
                Color.Teal,
                Color.Navy,
                Color.Purple,
                Color.Gray,
                Color.Red,
                Color.Yellow,
                Color.Lime,
                Color.Cyan,
                Color.Blue,
                Color.Magenta,
                Color.Silver,
                Color.White
            });
        }

        private void textBox_TextChanged(object sender, EventArgs e) {
            this.OnTextChanged(e);
            if (!busy) {
                Match match;
                match = Regex.Match(this.Text, @"^#([0-9A-Fa-f]{2})?([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})$");
                if (match.Success) {
                    comboBox.SelectedIndex = 3;
                    color = Color.FromArgb(match.Groups[1].Success ? int.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber) : 255,
                        int.Parse(match.Groups[2].Value, System.Globalization.NumberStyles.HexNumber),
                        int.Parse(match.Groups[3].Value, System.Globalization.NumberStyles.HexNumber),
                        int.Parse(match.Groups[4].Value, System.Globalization.NumberStyles.HexNumber));
                    sampleBox.Refresh();
                    this.OnColorChanged(EventArgs.Empty);
                    return;
                }

                match = Regex.Match(this.Text, @"^\(\s*(\d{1,3}|_{1,3}),\s*(\d{1,3}|_{1,3}),\s*(\d{1,3}|_{1,3})(?:,\s*(\d{1,3}|_{1,3}))?\s*\)$");
                if (match.Success) {
                    comboBox.SelectedIndex = 4;
                    color = Color.FromArgb(match.Groups[4].Success ? int.Parse(match.Groups[4].Value) : 255,
                        int.Parse(match.Groups[1].Value),
                        int.Parse(match.Groups[2].Value),
                        int.Parse(match.Groups[3].Value));
                    sampleBox.Refresh();
                    this.OnColorChanged(EventArgs.Empty);
                    return;
                }

                if (this.Text.Equals("Default", StringComparison.CurrentCultureIgnoreCase)) {
                    color = Color.Empty;
                    sampleBox.Refresh();
                    this.OnColorChanged(EventArgs.Empty);
                    return;
                } else if (this.Text.Equals("None", StringComparison.CurrentCultureIgnoreCase)) {
                    color = Color.Transparent;
                    sampleBox.Refresh();
                    this.OnColorChanged(EventArgs.Empty);
                    return;
                }

                KnownColor newColor;
                if (Enum.TryParse(this.Text, true, out newColor)) {
                    color = Color.FromKnownColor(newColor);
                    sampleBox.Refresh();
                    this.OnColorChanged(EventArgs.Empty);
                    return;
                }
            }
        }

        private void comboBox_TextChanged(object sender, EventArgs e) {
            textBox.Text = comboBox.Text;
        }

        private void comboBox_DrawItem(object sender, DrawItemEventArgs e) {

            var item = comboBox.Items[e.Index];
            if (item == Separator) {
                // Draw a separator.
                e.Graphics.FillRectangle(new SolidBrush(this.BackColor), e.Bounds);
                e.Graphics.DrawLine(SystemPens.GrayText, new Point(e.Bounds.Left, e.Bounds.Top + e.Bounds.Height / 2), new Point(e.Bounds.Right, e.Bounds.Top + e.Bounds.Height / 2));
            } else {
                if (e.State.HasFlag(DrawItemState.Selected))
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                else
                    e.Graphics.FillRectangle(new SolidBrush(this.BackColor), e.Bounds);
                //e.DrawBackground();
                e.DrawFocusRectangle();

                Color colour; string text;
                if (item == DefaultLabel) {
                    colour = defaultColor;
                    text = (string) DefaultLabel;
                } else if (item == InitialLabel) {
                    colour = Color.Empty;
                    text = (string) InitialLabel;
                } else if (item is string) {
                    colour = Color.Empty;
                    text = (string) item;
                } else {
                    colour = (Color) item;
                    text = DisplayColour(colour);
                }

                if (colour.IsEmpty) {
                    TextRenderer.DrawText(e.Graphics, text, this.Font, e.Bounds.Location, e.State.HasFlag(DrawItemState.Selected) ? SystemColors.HighlightText : this.ForeColor);
                } else {
                    e.Graphics.FillRectangle(new SolidBrush(colour), new Rectangle(e.Bounds.Left + 2, e.Bounds.Top + 2, e.Bounds.Height - 4, e.Bounds.Height - 4));
                    e.Graphics.DrawRectangle(new Pen(this.ForeColor), new Rectangle(e.Bounds.Left + 2, e.Bounds.Top + 2, e.Bounds.Height - 4, e.Bounds.Height - 4));
                    TextRenderer.DrawText(e.Graphics, text, this.Font, new Point(e.Bounds.Left + e.Bounds.Height, e.Bounds.Top), e.State.HasFlag(DrawItemState.Selected) ? SystemColors.HighlightText : this.ForeColor);
                }
            }
        }

        private void sampleBox_Paint(object sender, PaintEventArgs e) {
            e.Graphics.Clear(panel.BackColor);
            if (!InitialSelected && (!DefaultSelected || !defaultColor.IsEmpty)) {
                e.Graphics.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, sampleBox.Width - 1, sampleBox.Height - 1));
                e.Graphics.DrawRectangle(new Pen(this.ForeColor), new Rectangle(0, 0, sampleBox.Width - 1, sampleBox.Height - 1));
            }
        }

        private void textBox_Enter(object sender, EventArgs e) {
            textBox.SelectAll();
        }

        private void comboBox_SelectionChangeCommitted(object sender, EventArgs e) {
            if (comboBox.SelectedItem == Separator)
                ++comboBox.SelectedIndex;

            if (busy) return;
            busy = true;
            if (comboBox.SelectedItem == DefaultLabel) {
                color = defaultColor;
                textBox.Text = (string) DefaultLabel;
                OnColorChanged(EventArgs.Empty);
            } else if (comboBox.SelectedItem == InitialLabel) {
                color = Color.Empty;
                textBox.Text = (string) InitialLabel;
                OnColorChanged(EventArgs.Empty);
            } else if (comboBox.SelectedItem == DialogLabel) {
                if (DefaultSelected) dialog.Color = defaultColor;
                else if (InitialSelected) dialog.Color = Color.Black;
                else dialog.Color = color;

                Color newColor;
                if (dialog.ShowDialog(this.ParentForm) == DialogResult.OK)
                    newColor = dialog.Color;
                else
                    newColor = color;

                busy = false;
                Color = newColor;
            } else if (comboBox.SelectedItem is Color) {
                color = (Color) comboBox.SelectedItem;
                textBox.Text = DisplayColour(color);
                OnColorChanged(EventArgs.Empty);
            }

            busy = false;
            sampleBox.Refresh();
            if (this.ContainsFocus) textBox.Focus();
            textBox.SelectAll();
        }

        private void ColourComboBox_Enter(object sender, EventArgs e) {
            textBox.Focus();
        }

        private string DisplayColour(Color colour) {
            if (colour == Color.Empty) return "";
            if (colour.IsNamedColor) return colour.Name;
            if (colour.A == 255) return "#" + colour.R.ToString("X2") + colour.G.ToString("X2") + colour.B.ToString("X2");
            return "#" + colour.A.ToString("X2") + colour.R.ToString("X2") + colour.G.ToString("X2") + colour.B.ToString("X2");
        }

        private void comboBox_Enter(object sender, EventArgs e) {

        }

        private void comboBox_DropDown(object sender, EventArgs e) {

        }

        private void sampleBox_MouseDown(object sender, MouseEventArgs e) {
            textBox.Focus();
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) {
                e.Handled = true;
            }
        }

        private void textBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            if (comboBox.SelectedItem == HexEntry) {
                HexEntryKeys(e);
                return;
            }
            if (comboBox.SelectedItem == RGBAEntry) {
                RGBAEntryKeys(e);
                return;
            }

            // What happens here depends on whether the Default and Initial items are there.
            if (e.KeyCode == Keys.Up) {
                if (comboBox.SelectedIndex <= 0) {
                    if (comboBox.Items[0] == HexEntry) comboBox.SelectedIndex = 4;
                    else comboBox.SelectedIndex = 0;
                } else if (comboBox.SelectedItem != HexEntry && comboBox.Items[comboBox.SelectedIndex - 1] == Separator) {
                    if (comboBox.Items[1] == InitialLabel) comboBox.SelectedIndex = 1;
                    else if (comboBox.Items[0] != HexEntry) comboBox.SelectedIndex = 0;
                    else return;
                } else --comboBox.SelectedIndex;
                comboBox_SelectionChangeCommitted(this, EventArgs.Empty);
            } else if (e.KeyCode == Keys.Down) {
                if (comboBox.SelectedIndex == -1) {
                    if (comboBox.Items[0] == HexEntry) comboBox.SelectedIndex = 4;
                    else comboBox.SelectedIndex = 0;
                } else if (comboBox.SelectedIndex == comboBox.Items.Count - 1) return;
                else if (comboBox.SelectedItem != DialogLabel && comboBox.Items[comboBox.SelectedIndex + 1] == Separator)
                    comboBox.SelectedIndex += 6;
                else ++comboBox.SelectedIndex;
                comboBox_SelectionChangeCommitted(this, EventArgs.Empty);
            }
            e.IsInputKey = true;
        }

        private void HexEntryKeys(PreviewKeyDownEventArgs e) {
            if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down) return;
            e.IsInputKey = true;

            Match match = Regex.Match(this.Text, @"^#([0-9A-Fa-f]{2}|__)?([0-9A-Fa-f]{2}|__)([0-9A-Fa-f]{2}|__)([0-9A-Fa-f]{2}|__)$");
            if (!match.Success) return;

            int startIndex;
            if (textBox.SelectionStart < (match.Groups[1].Success ? match.Groups[1].Index : match.Groups[2].Index))
                return;

            if (textBox.SelectionStart < match.Groups[1].Index + match.Groups[1].Length) startIndex = match.Groups[1].Index;
            else if (textBox.SelectionStart < match.Groups[2].Index + match.Groups[2].Length) startIndex = match.Groups[2].Index;
            else if (textBox.SelectionStart < match.Groups[3].Index + match.Groups[3].Length) startIndex = match.Groups[3].Index;
            else if (textBox.SelectionStart <= match.Groups[4].Index + match.Groups[4].Length) startIndex = match.Groups[4].Index;
            else return;

            // Calculate the new number.
            string substring = textBox.Text.Substring(startIndex, 2);
            int value;
            if (substring == "__") value = 0;
            else {
                value = int.Parse(textBox.Text.Substring(startIndex, 2), System.Globalization.NumberStyles.HexNumber);
                if (e.KeyCode == Keys.Up) {
                    if (value >= 255) value = 255;
                    else ++value;
                } else if (e.KeyCode == Keys.Down) {
                    if (value <= 0) value = 0;
                    else --value;
                }
            }

            // Replace the characters.
            char[] chars = textBox.Text.ToCharArray();
            substring = value.ToString("X2");
            chars[startIndex] = substring[0];
            chars[startIndex + 1] = substring[1];
            textBox.Text = new string(chars);
            textBox.Select(startIndex, 2);
        }

        private void RGBAEntryKeys(PreviewKeyDownEventArgs e) {
            if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down) return;
            e.IsInputKey = true;

            Match match = Regex.Match(this.Text, @"^\(\s*(\d{1,3}|_{1,3}),\s*(\d{1,3}|_{1,3}),\s*(\d{1,3}|_{1,3})(?:,\s*(\d{1,3}|_{1,3}))?\s*\)$");
            if (!match.Success) return;

            Group group;
            if (textBox.SelectionStart < match.Groups[1].Index) return;

            if (textBox.SelectionStart < match.Groups[1].Index + match.Groups[1].Length) group = match.Groups[1]; 
            else if (textBox.SelectionStart < match.Groups[2].Index + match.Groups[2].Length) group = match.Groups[2];
            else if (textBox.SelectionStart < match.Groups[3].Index + match.Groups[3].Length) group = match.Groups[3];
            else if (textBox.SelectionStart <= match.Groups[4].Index + match.Groups[4].Length) group = match.Groups[4];
            else return;

            // Calculate the new number.
            string substring = group.Value;
            int value;
            if (substring[0] == '_') value = 0;
            else {
                value = int.Parse(group.Value);
                if (e.KeyCode == Keys.Up) {
                    if (value >= 255) value = 255;
                    else ++value;
                } else if (e.KeyCode == Keys.Down) {
                    if (value <= 0) value = 0;
                    else --value;
                }
            }

            // Replace the characters.
            substring = value.ToString();
            textBox.Text = textBox.Text.Remove(group.Index, group.Length).Insert(group.Index, substring);
            textBox.Select(group.Index, substring.Length);
        }

        private void comboBox_MouseDown(object sender, MouseEventArgs e) {
            
        }

        private void ColourComboBox_Load(object sender, EventArgs e) {
            panel.Width = this.Width - 2 - SystemInformation.VerticalScrollBarWidth;
        }

        private void panel_EnabledChanged(object sender, EventArgs e) {
            panel.BackColor = (panel.Enabled ? this.BackColor : SystemColors.Control);
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e) {
            textBox.Focus();
        }

        private void ColourComboBox_BackColorChanged(object sender, EventArgs e) {
            panel.BackColor = this.BackColor;
            textBox.BackColor = this.BackColor;
        }

        private void comboBox_Resize(object sender, EventArgs e) {
            this.Height = comboBox.Height;
        }

        private void ColourComboBox_Resize(object sender, EventArgs e) {
            comboBox.Width = this.Width;
            panel.Width = this.Width - 2 - SystemInformation.VerticalScrollBarWidth;
            panel.Height = this.Height - 4;
            sampleBox.Size = new Size(this.Height - 10, this.Height - 10);
            panel.Location = new Point(2, 2);
        }

        private void ColourComboBox_ForeColorChanged(object sender, EventArgs e) {
            panel.ForeColor = this.ForeColor;
            textBox.ForeColor = this.ForeColor;
        }

        private void ColourComboBox_EnabledChanged(object sender, EventArgs e) {
            textBox.Enabled = this.Enabled;
        }
    }
}
