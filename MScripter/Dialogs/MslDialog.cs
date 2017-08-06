using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace MScripter {
    public class MslDialog : MslStylable {
        private string name;
        [Description("The name used in scripts to refer to the dialog table.")]
        [ParenthesizePropertyName(true)]
        public string Name {
            get { return this.name; }
            set {
                this.name = value;
                this.NameChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        public string OriginalName;

        private string title;
        [Description("The title to display in the dialog.")]
        public string Title {
            get { return this.title; }
            set {
                this.title = value;
                this.TitleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        public MslDocument Document { get; set; }
        [Browsable(false)]
        public MslBookmark Bookmark { get; set; }
        [Browsable(false)]
        public DialogDesigner designer;

        private DialogSizeUnit sizeUnit;
        [Description("The unit of measurement to use to place and size controls. Using dialog base units ensures that the dialog will look the same on any display size.")]
        [DefaultValue(DialogSizeUnit.Pixel)]
        public DialogSizeUnit SizeUnit {
            get { return this.sizeUnit; }
            set {
                this.sizeUnit = value;

                this.Styles.RemoveAll(s => s.Equals("dbu", StringComparison.OrdinalIgnoreCase) || s.Equals("pixels", StringComparison.OrdinalIgnoreCase));
                this.Styles.Insert(0, (value == DialogSizeUnit.DBU ? "dbu" : "pixels"));
                this.OnStyleChanged(EventArgs.Empty);

                this.UpdateSize();
            }
        }

        private Size size = new Size(254, 192);
        internal int startWidth, startHeight;
        [Browsable(false)]
        public Size SizePixels { get; private set; } = new Size(254, 192);
        [Description("The size of the dialog, excluding the border and menu bar.")]
        public Size Size {
            get { return this.size; }
            set {
                this.size = value;
                this.UpdateSize();
            }
        }

        private Point position = new Point(-1, -1);
        [Description("The position of the dialog. Set this to (-1, -1) to centre it.")]
        public Point Position {
            get { return this.position; }
            set {
                this.position = value;
                this.PositionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private string iconFilename;
        [Description("The file name to load an icon from. Blank this to use a default icon.")]
        [DefaultValue(null)]
        public string IconFilename {
            get { return this.iconFilename; }
            set {
                this.iconFilename = value;
                this.IconChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private int iconIndex;
        [Description("The index within the file of the icon to display on the dialog.")]
        [DefaultValue(0)]
        public int IconIndex {
            get { return this.iconIndex; }
            set {
                this.iconIndex = value;
                this.IconChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Description("Disables the dialog for one second to avoid input errors.")]
        [DefaultValue(false)]
        public bool disable {
            get { return this.Styles.Contains("disable"); }
            set { this.SetStyle("disable", value); }
        }

        [Description("Prevents the dialog from being styled.")]
        [DefaultValue(false)]
        public bool notheme {
            get { return this.Styles.Contains("notheme"); }
            set { this.SetStyle("notheme", value); }
        }

        [Browsable(false)]
        public List<Error> errors { get; } = new List<Error>();

        public MslDialog(string name) : base("pixels") {
            this.Name = name;
        }
        public MslDialog(string name, MslDocument document, MslBookmark bookmark) : base("pixels") {
            this.Name = name;
            this.Document = document;
            this.Bookmark = bookmark;
        }

        private void UpdateSize() {
            if (this.SizeUnit == DialogSizeUnit.DBU) {
                var dbuSize = Program.GetDialogBaseUnitSize();
                this.SizePixels = new Size(this.Size.Width * dbuSize.Width, this.Size.Height * dbuSize.Height);
            } else {
                this.SizePixels = this.Size;
            }
            this.SizeChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler NameChanged;
        public event EventHandler PositionChanged;
        public event EventHandler SizeChanged;
        public event EventHandler TitleChanged;
        public event EventHandler IconChanged;
    }

    public enum DialogSizeUnit {
        Pixel,
        DBU
    }
}
