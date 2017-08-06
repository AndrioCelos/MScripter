using System.Windows.Forms;

namespace MScripter {
    public class LayoutPictureBox : PictureBox {
        public LayoutPictureBox() : base() {
            this.SetStyle(ControlStyles.Selectable, true);
            this.DoubleBuffered = true;
        }

        protected override bool IsInputKey(Keys keyData) {
            var key = keyData & Keys.KeyCode;
            if (key >= Keys.Left && key <= Keys.Down) return true;
            return base.IsInputKey(keyData);
        }
    }
}
