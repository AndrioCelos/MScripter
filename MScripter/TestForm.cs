using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MScripter {
    public partial class TestForm : Form {
        public TestForm() {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            colourComboBox.ShowDefault = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            colourComboBox.ShowInitial = checkBox2.Checked;
        }

        private void button1_Click(object sender, EventArgs e) {
            colourComboBox.Color = Color.Red;
        }

        private void button2_Click(object sender, EventArgs e) {
            colourComboBox.Color = Color.Empty;
        }

        private void colourComboBox_ColorChanged(object sender, EventArgs e) {
            pictureBox1.BackColor = colourComboBox.Color;
        }
    }
}
