/*
    Thanks to Bill for this trick.
    Original post: https://www.lyquidity.com/devblog/?p=136
 */

using System.ComponentModel;
using System.Windows.Forms;

namespace FolderSelect {
    /// <summary>
    /// Wraps a SaveFileDialog to make it present a Vista-style dialog.
    /// </summary>
    public class VistaSaveFileDialog : VistaFileDialog {
        /// <summary>
        /// Gets or sets a value indicating whether the dialog box prompts the user for permission
        /// to create a file if the user specifies a file that does not exist.
        /// </summary>
        [Category("Behavior")] [DefaultValue(false)]
        public bool CreatePrompt { get { return ((SaveFileDialog) dialog).CreatePrompt; } set { ((SaveFileDialog) dialog).CreatePrompt = value; } }
        /// <summary>
        /// Gets or sets a value indicating whether the dialog box displays a warning
        /// if the user specifies a file name that already exists.
        /// </summary>
        [Category("Behavior")] [DefaultValue(true)]
        public bool OverwritePrompt { get { return ((SaveFileDialog) dialog).OverwritePrompt; } set { ((SaveFileDialog) dialog).OverwritePrompt = value; } }

        //protected override bool shouldSetOptions => true;

        public VistaSaveFileDialog() : base(new SaveFileDialog()) { }

        protected override void SetOptions(ref uint options) {
            if (this.CheckFileExists) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_FILEMUSTEXIST");
            if (this.CheckPathExists) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_PATHMUSTEXIST");
            if (!this.ValidateNames) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_NOVALIDATE");
            if (this.RestoreDirectory) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_NOCHANGEDIR");
            if (this.CreatePrompt) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_CREATEPROMPT");
            if (this.OverwritePrompt) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_OVERWRITEPROMPT");
        }
    }
}
