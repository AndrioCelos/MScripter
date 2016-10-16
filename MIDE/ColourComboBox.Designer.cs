namespace MIDE {
    partial class ColourComboBox {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.panel = new System.Windows.Forms.TableLayoutPanel();
            this.sampleBox = new System.Windows.Forms.PictureBox();
            this.textBox = new System.Windows.Forms.TextBox();
            this.focusTimer = new System.Windows.Forms.Timer(this.components);
            this.dialog = new System.Windows.Forms.ColorDialog();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampleBox)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox
            // 
            this.comboBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(0, 0);
            this.comboBox.Margin = new System.Windows.Forms.Padding(0);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(211, 21);
            this.comboBox.TabIndex = 0;
            this.comboBox.TabStop = false;
            this.comboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox_DrawItem);
            this.comboBox.DropDown += new System.EventHandler(this.comboBox_DropDown);
            this.comboBox.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectionChangeCommitted);
            this.comboBox.DropDownClosed += new System.EventHandler(this.comboBox_DropDownClosed);
            this.comboBox.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            this.comboBox.Enter += new System.EventHandler(this.comboBox_Enter);
            this.comboBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.comboBox_MouseDown);
            this.comboBox.Resize += new System.EventHandler(this.comboBox_Resize);
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.BackColor = System.Drawing.Color.Transparent;
            this.panel.ColumnCount = 2;
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel.Controls.Add(this.sampleBox, 0, 0);
            this.panel.Controls.Add(this.textBox, 1, 0);
            this.panel.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.panel.Location = new System.Drawing.Point(2, 2);
            this.panel.Margin = new System.Windows.Forms.Padding(0);
            this.panel.Name = "panel";
            this.panel.RowCount = 1;
            this.panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.panel.Size = new System.Drawing.Size(186, 17);
            this.panel.TabIndex = 1;
            this.panel.EnabledChanged += new System.EventHandler(this.panel_EnabledChanged);
            this.panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.sampleBox_MouseDown);
            // 
            // sampleBox
            // 
            this.sampleBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.sampleBox.Location = new System.Drawing.Point(2, 2);
            this.sampleBox.Margin = new System.Windows.Forms.Padding(2, 0, 4, 0);
            this.sampleBox.Name = "sampleBox";
            this.sampleBox.Size = new System.Drawing.Size(13, 13);
            this.sampleBox.TabIndex = 4;
            this.sampleBox.TabStop = false;
            this.sampleBox.Paint += new System.Windows.Forms.PaintEventHandler(this.sampleBox_Paint);
            this.sampleBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.sampleBox_MouseDown);
            // 
            // textBox
            // 
            this.textBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox.Location = new System.Drawing.Point(19, 2);
            this.textBox.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(167, 13);
            this.textBox.TabIndex = 0;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.textBox.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            this.textBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBox_PreviewKeyDown);
            // 
            // focusTimer
            // 
            this.focusTimer.Interval = 1;
            // 
            // ColourComboBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.comboBox);
            this.Name = "ColourComboBox";
            this.Size = new System.Drawing.Size(211, 21);
            this.Load += new System.EventHandler(this.ColourComboBox_Load);
            this.BackColorChanged += new System.EventHandler(this.ColourComboBox_BackColorChanged);
            this.EnabledChanged += new System.EventHandler(this.ColourComboBox_EnabledChanged);
            this.ForeColorChanged += new System.EventHandler(this.ColourComboBox_ForeColorChanged);
            this.Enter += new System.EventHandler(this.ColourComboBox_Enter);
            this.Resize += new System.EventHandler(this.ColourComboBox_Resize);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampleBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.TableLayoutPanel panel;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.PictureBox sampleBox;
        private System.Windows.Forms.Timer focusTimer;
        private System.Windows.Forms.ColorDialog dialog;
    }
}
