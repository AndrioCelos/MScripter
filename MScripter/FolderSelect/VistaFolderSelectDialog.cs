/*
    Thanks to Bill for this trick.
    Original post: https://www.lyquidity.com/devblog/?p=136
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FolderSelect {
    /// <summary>
    /// Wraps an OpenFileDialog to make it present a Vista-style dialog to allow the user to select a folder.
    /// If Vista-style dialogs are not supported, a FolderBrowserDialog will be shown instead.
    /// </summary>
    public sealed class VistaFolderSelectDialog : VistaFileDialog {
        /// <summary>Gets or sets a value indicating whether the dialog box automatically adds an extension to a file name if the user omits the extension.</summary>
        [Category("Behavior")] [DefaultValue(false)]
        public override bool AddExtension { get { return base.AddExtension; } set { base.AddExtension = value; } }
        /// <summary>
        /// Gets or sets the current file name filter string, which determines the choices
        /// that appear in the "Save as file type" or "Files of type" box in the dialog box.
        /// </summary>
        [Category("Behavior")] [DefaultValue("Folders|\n")]
        public override string Filter { get { return base.Filter; } set { base.Filter = value; } }
        /// <summary>
        /// Returns or sets a value indicating whether a New Folder button appears in the old-style dialog.
        /// In a Vista-style dialog, a New Folder button will always be shown.
        /// </summary>
        [Category("Behavior")] [DefaultValue(true)]
        public bool ShowNewFolderButton { get; set; }

        /// <summary>Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a file name that does not exist.</summary>
        [DefaultValue(true)]
        public override bool CheckFileExists { get { return base.CheckFileExists; } set { base.CheckFileExists = value; } }
        /// <summary>Gets or sets a value indicating whether the dialog box allows multiple files to be selected.</summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool Multiselect { get { return ((OpenFileDialog) dialog).Multiselect; } set { ((OpenFileDialog) dialog).Multiselect = value; } }
        /// <summary>Gets the file name and extension for the file selected in the dialog box, excluding the path.</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SafeFileName => ((OpenFileDialog) dialog).SafeFileName;
        /// <summary>Gets the file names and extensions for all the selected files in the dialog box, excluding the paths.</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] SafeFileNames => ((OpenFileDialog) dialog).SafeFileNames;
        /// <summary>Gets or sets a value indicating whether the dialog box contains a read-only check box.</summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool ShowReadOnly { get { return ((OpenFileDialog) dialog).ShowReadOnly; } set { ((OpenFileDialog) dialog).ShowReadOnly = value; } }
        /// <summary>Gets or sets a value indicating whether the read-only check box is checked.</summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool ReadOnlyChecked { get { return ((OpenFileDialog) dialog).ReadOnlyChecked; } set { ((OpenFileDialog) dialog).ReadOnlyChecked = value; } }

        protected override bool shouldSetOptions => true;

        public VistaFolderSelectDialog() : base(new OpenFileDialog() {
            AddExtension = false,
            CheckFileExists = false,
            Filter = "Folders|\n"
        }) { }

        protected override void SetOptions(ref uint options) {
            options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_PICKFOLDERS");
            if (this.CheckFileExists) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_FILEMUSTEXIST");
            if (this.CheckPathExists) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_PATHMUSTEXIST");
            if (!this.ValidateNames) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_NOVALIDATE");
            if (this.AddExtension) options |= 2147483648;
            if (!this.DereferenceLinks) options |= 1048576;
            if (this.RestoreDirectory) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_NOCHANGEDIR");
            if (this.ShowHelp) options |= 16;
            if (this.Multiselect) options |= (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_ALLOWMULTISELECT");
            if (this.ReadOnlyChecked) options |= 1;
            if (!this.ShowReadOnly) options |= 4;
        }

        protected override DialogResult ShowDialogFallback(IWin32Window owner) {
            var dialog = new FolderBrowserDialog() {
                Description = this.Title,
                SelectedPath = this.InitialDirectory,
                ShowNewFolderButton = this.ShowNewFolderButton
            };
            dialog.HelpRequest += Dialog_HelpRequest;

            var result = dialog.ShowDialog(owner);
            if (result == DialogResult.OK)
                this.dialog.FileName = dialog.SelectedPath;
            return result;
        }

        private void Dialog_HelpRequest(object sender, EventArgs e) {
            this.OnHelpRequest(e);
        }
    }
}
