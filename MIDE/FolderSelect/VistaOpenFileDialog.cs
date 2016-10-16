/*
    Thanks to Bill for this trick.
    Original post: https://www.lyquidity.com/devblog/?p=136
 */

using System.ComponentModel;
using System.Windows.Forms;

namespace FolderSelect {
    /// <summary>
    /// Wraps an OpenFileDialog to make it present a Vista-style dialog.
    /// </summary>
    public class VistaOpenFileDialog : VistaFileDialog {
        /// <summary>Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a file name that does not exist.</summary>
        [DefaultValue(true)]
        public override bool CheckFileExists { get { return base.CheckFileExists; } set { base.CheckFileExists = value; } }
        /// <summary>Gets or sets a value indicating whether the dialog box allows multiple files to be selected.</summary>
        [Category("Behavior")] [DefaultValue(false)]
        public bool Multiselect { get { return ((OpenFileDialog) dialog).Multiselect; } set { ((OpenFileDialog) dialog).Multiselect = value; } }
        /// <summary>Gets the file name and extension for the file selected in the dialog box, excluding the path.</summary>
        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SafeFileName => ((OpenFileDialog) dialog).SafeFileName;
        /// <summary>Gets the file names and extensions for all the selected files in the dialog box, excluding the paths.</summary>
        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] SafeFileNames => ((OpenFileDialog) dialog).SafeFileNames;
        /// <summary>Gets or sets a value indicating whether the dialog box contains a read-only check box.</summary>
        [Category("Behavior")] [DefaultValue(false)]
        public bool ShowReadOnly { get { return ((OpenFileDialog) dialog).ShowReadOnly; } set { ((OpenFileDialog) dialog).ShowReadOnly = value; } }
        /// <summary>Gets or sets a value indicating whether the read-only check box is checked.</summary>
        [Category("Behavior")] [DefaultValue(false)]
        public bool ReadOnlyChecked { get { return ((OpenFileDialog) dialog).ReadOnlyChecked; } set { ((OpenFileDialog) dialog).ReadOnlyChecked = value; } }

        protected override bool shouldSetOptions => true;

        public VistaOpenFileDialog() : base(new OpenFileDialog()) { }

        protected override void SetOptions(ref uint options) {
            if (this.CheckFileExists) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_FILEMUSTEXIST");
            if (this.CheckPathExists) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_PATHMUSTEXIST");
            if (!this.ValidateNames) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_NOVALIDATE");
            if (this.RestoreDirectory) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_NOCHANGEDIR");
            if (this.Multiselect) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_ALLOWMULTISELECT");
            if (this.ReadOnlyChecked) options |= 1;
            if (!this.ShowReadOnly) options |= 4;
        }
    }
}
