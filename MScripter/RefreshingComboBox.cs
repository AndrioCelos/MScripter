/*
 * Thanks to Ian Ringrose for this trick.
 * Original post: http://stackoverflow.com/a/9796348
 */

using System.Windows.Forms;

namespace MScripter {
    public class RefreshingComboBox : ComboBox {
        // Why is ComboBox.RefreshItem protected and not public?
        public new void RefreshItem(int index) {
            base.RefreshItem(index);
        }

        public new void RefreshItems() {
            base.RefreshItems();
        }
    }
}
