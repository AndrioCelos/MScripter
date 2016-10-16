/*
    Thanks to Bill for this trick.
    Original post: https://www.lyquidity.com/devblog/?p=136
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FolderSelect {
	/// <summary>
	/// Wraps a FileDialog to make it present a Vista-style dialog.
	/// </summary>
	public abstract class VistaFileDialog : Component {
        /// <summary>The dialog wrapped by this object.</summary>
        protected FileDialog dialog;
        /// <summary>The Reflector object used to invoke methods on the wrapped dialog.</summary>
        protected Reflector reflector = new Reflector("System.Windows.Forms");

        /// <summary>Gets or sets a value indicating whether the dialog box automatically adds an extension to a file name if the user omits the extension.</summary>
        [Category("Behavior")] [DefaultValue(true)]
        public virtual bool AddExtension { get { return dialog.AddExtension; } set { dialog.AddExtension = value; } }
        /// <summary>Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a file name that does not exist.</summary>
        [Category("Behavior")] [DefaultValue(false)]
        public virtual bool CheckFileExists { get { return dialog.CheckFileExists; } set { dialog.CheckFileExists = value; } }
        /// <summary>Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a path that does not exist.</summary>
        [Category("Behavior")] [DefaultValue(true)]
        public bool CheckPathExists { get { return dialog.CheckPathExists; } set { dialog.CheckPathExists = value; } }
        /// <summary>Gets the custom places collection for the wrapped dialog.</summary>
        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileDialogCustomPlacesCollection CustomPlaces => dialog.CustomPlaces;
        /// <summary>Gets or sets the default file name extension, not including the period.</summary>
        [Category("Behavior")] [DefaultValue("")]
        public string DefaultExt { get { return dialog.DefaultExt; } set { dialog.DefaultExt = value; } }
        /// <summary>
        /// Gets or sets a value indicating whether the dialog box returns the location of
        /// the file referenced by a shortcut (.lnk) or the location of the shortcut.
        /// </summary>
        /// <returns>
        /// true if the dialog box returns the location of the file referenced by a shortcut;
        /// otherwise, false.
        /// </returns>
        [Category("Behavior")] [DefaultValue(true)]
        public bool DereferenceLinks { get { return dialog.DereferenceLinks; } set { dialog.DereferenceLinks = value; } }
        /// <summary>Gets or sets the file name selected in the file dialog box.</summary>
        [Category("Data")] [DefaultValue("")]
        public string FileName { get { return dialog.FileName; } set { dialog.FileName = value; } }
        /// <summary>Gets the file names of all selected files in the dialog box.</summary>
        [Browsable(false)] [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] FileNames => dialog.FileNames;
        /// <summary>
        /// Gets or sets the current file name filter string, which determines the choices
        /// that appear in the "Save as file type" or "Files of type" box in the dialog box.
        /// </summary>
        [Category("Behavior")] [DefaultValue("")]
        public virtual string Filter { get { return dialog.Filter; } set { dialog.Filter = value; } }
        /// <summary>Gets or sets the index of the filter currently selected in the file dialog box.</summary>
        [Category("Behavior")] [DefaultValue(1)]
        public int FilterIndex { get { return dialog.FilterIndex; } set { dialog.FilterIndex = value; } }
        /// <summary>Gets or sets the initial directory displayed by the file dialog box.</summary>
        [Category("Behavior")] [DefaultValue("")]
        public string InitialDirectory { get { return dialog.InitialDirectory; } set { dialog.InitialDirectory = value; } }
        /// <summary>Gets or sets a value indicating whether the dialog box restores the current directory before closing.</summary>
        [Category("Behavior")] [DefaultValue(false)]
        public bool RestoreDirectory { get { return dialog.RestoreDirectory; } set { dialog.RestoreDirectory = value; } }
        /// <summary>Gets or sets a value indicating whether the Help button is displayed in the file dialog box.</summary>
        [Category("Behavior")] [DefaultValue(false)]
        public bool ShowHelp { get { return dialog.ShowHelp; } set { dialog.ShowHelp = value; } }
        /// <summary>Gets or sets whether the dialog box supports displaying and saving files that have multiple file name extensions.</summary>
        [Category("Behavior")] [DefaultValue(false)]
        public bool SupportMultiDottedExtensions { get { return dialog.SupportMultiDottedExtensions; } set { dialog.SupportMultiDottedExtensions = value; } }
        /// <summary>Gets or sets the file dialog box title.</summary>
        [Category("Appearance")] [DefaultValue("")]
        public string Title { get { return dialog.Title; } set { dialog.Title = value; } }
        /// <summary>Gets or sets a value indicating whether the dialog box accepts only valid Win32 file names.</summary>
        [Category("Behavior")] [DefaultValue(true)]
        public bool ValidateNames { get { return dialog.ValidateNames; } set { dialog.ValidateNames = value; } }
        /// <summary>Allows data to be associated with this control.</summary>
        [Category("Data")] [TypeConverter(typeof(StringConverter))] [DefaultValue(null)] 
        public object Tag { get; set; }

        protected virtual bool shouldSetOptions => false;

        public event CancelEventHandler FileOk;
        public event EventHandler HelpRequest;

        /// <summary>Raises the FileOk event.</summary>
        /// <param name="e">An EventArgs object containing the event data.</param>
        protected void OnFileOk(CancelEventArgs e) {
            this.FileOk?.Invoke(this, e);
        }
        /// <summary>Raises the HelpRequest event.</summary>
        /// <param name="e">An EventArgs object containing the event data.</param>
        protected void OnHelpRequest(EventArgs e) {
            this.HelpRequest?.Invoke(this, e);
        }

        protected VistaFileDialog(FileDialog dialog) {
            this.dialog = dialog;
            this.dialog.FileOk += Dialog_FileOk;
            this.dialog.HelpRequest += Dialog_HelpRequest;
        }

        private void Dialog_FileOk(object sender, CancelEventArgs e) {
            this.OnFileOk(e);
        }
        private void Dialog_HelpRequest(object sender, EventArgs e) {
            this.OnHelpRequest(e);
        }

        /// <summary>Runs a common dialog box modally with a default owner.</summary>
        /// <returns>DialogResult.OK if the user clicks OK in the dialog box; DialogResult.Cancel otherwise.</returns>
        public DialogResult ShowDialog() {
			return this.ShowDialog(null);
		}
        /// <summary>Runs a common dialog box modally with the specified owner.</summary>
        /// <param name="owner">Any object that implements IWin32Window that represents
        ///     the top-level window that will own the modal dialog box.</param>
        /// <returns>DialogResult.OK if the user clicks OK in the dialog box; DialogResult.Cancel otherwise.</returns>
		public virtual DialogResult ShowDialog(IWin32Window owner) {
            /* Quote from the original post:
                Being a Vista-style dialog box the dialog can only be shown on Vista or later (not WinNT, XP or 2003).
                The code gracefully reverts to show the old-style dialog if OS is not major version 6 or later.
             */
            if (Environment.OSVersion.Version.Major >= 6) {
				uint num = 0;
				Type typeIFileDialog = reflector.GetType("FileDialogNative.IFileDialog");
				object dialog = reflector.Call(this.dialog, "CreateVistaDialog");
				reflector.Call(this.dialog, "OnBeforeVistaDialog", dialog);

                if (this.shouldSetOptions) {
                    uint options = (uint) reflector.CallAs(typeof(FileDialog), this.dialog, "GetOptions");
                    this.SetOptions(ref options);
                    reflector.CallAs(typeIFileDialog, dialog, "SetOptions", options);
                }

                object pfde = reflector.New("FileDialog.VistaDialogEvents", this.dialog);
				object[] parameters = new object[] { pfde, num };
				reflector.CallAs2(typeIFileDialog, dialog, "Advise", parameters);
				num = (uint) parameters[1];

				try	{
					var result = (int) reflector.CallAs(typeIFileDialog, dialog, "Show", owner?.Handle ?? IntPtr.Zero);
					return (result == 0 ? DialogResult.OK : DialogResult.Cancel);
				} finally {
					reflector.CallAs(typeIFileDialog, dialog, "Unadvise", num);
					GC.KeepAlive(pfde);
				}
			} else {
                return this.ShowDialogFallback(owner);
			}
		}

        protected virtual void SetOptions(ref uint options) { }

        /// <summary>Provides a fallback to run, modally, the old-style dialog on systems other than Windows Vista or later.</summary>
        /// <param name="owner">The IWin32Window that should own the dialog box.</param>
        /// <returns>DialogResult.OK if the user clicks OK in the dialog box; DialogResult.Cancel otherwise.</returns>
        protected virtual DialogResult ShowDialogFallback(IWin32Window owner) {
            return this.dialog.ShowDialog(owner);
        }

        /// <summary>Sets an option on the wrapped dialog. Refer to the FILEOPENDIALOGOPTIONS documentation for more information.</summary>
        /// <param name="option">An integer representing the flag to set.</param>
        /// <param name="value">True if the flag should be set; false if it should be unset.</param>
        protected void SetOption(int option, bool value) {
            reflector.CallAs(typeof(FileDialog), this.dialog, "SetOption", option, value);
        }
    }
}
