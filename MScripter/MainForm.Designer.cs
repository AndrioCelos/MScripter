using FastColoredTextBoxNS;

namespace MScripter {
    partial class MainForm {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.ToolStripLabel toolStripLabel1;
            System.Windows.Forms.ToolStripLabel toolStripLabel2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Add new...");
            this.treeView = new System.Windows.Forms.TreeView();
            this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.MslTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            this.documentContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goTodefinitionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MslTextBox2 = new FastColoredTextBoxNS.FastColoredTextBox();
            this.parsingProgressBarPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.parsingProgressBar = new System.Windows.Forms.ProgressBar();
            this.functionList = new MScripter.RefreshingComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.hashtableAddPanel = new System.Windows.Forms.TableLayoutPanel();
            this.hashtableAddTextBox = new System.Windows.Forms.TextBox();
            this.hashtableAddForwardButton = new System.Windows.Forms.Button();
            this.hashtableAddBackButton = new System.Windows.Forms.Button();
            this.hashtableAddExpiryPanel = new System.Windows.Forms.TableLayoutPanel();
            this.hashtableAddExpiryUnitBox = new System.Windows.Forms.ComboBox();
            this.hashtableAddExpiryTimeBox = new System.Windows.Forms.NumericUpDown();
            this.hashtableItemExpiryCheckBox = new System.Windows.Forms.CheckBox();
            this.hashtableListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hashtableHeaderPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.hashtableBinaryViewButton = new System.Windows.Forms.CheckBox();
            this.hashtableBaseComboBox = new System.Windows.Forms.ComboBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dialogDesigner = new MScripter.DialogDesigner();
            this.scriptTypeImageList = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.errorList = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.errorImageList = new System.Windows.Forms.ImageList(this.components);
            this.errorListToolbar = new System.Windows.Forms.ToolStrip();
            this.errorsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.warningsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.noticesToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.errorListScopeComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.consoleTextBox = new MScripter.ConsoleTextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.debugTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newRemoteScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newAliasScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newPopupScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.newTextFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newINIFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.newHashtableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noRunningMIRCInstancesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.revertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.restyleLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restyleDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator21 = new System.Windows.Forms.ToolStripSeparator();
            this.foldingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.foldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unfoldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator22 = new System.Windows.Forms.ToolStripSeparator();
            this.unfoldAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.foldallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator23 = new System.Windows.Forms.ToolStripSeparator();
            this.dialogDesignerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.interfaceThemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.standardThemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkThemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.openDirectoryToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.refreshToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.redoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.fileContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllButThisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToTheRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.formatAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aliasScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.popupScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.usersListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.variablesListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
            this.iNIFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formatWorker = new System.ComponentModel.BackgroundWorker();
            this.errorBalloon = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mIRCContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.identifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDataDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openScriptDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.openDialog = new FolderSelect.VistaOpenFileDialog();
            this.saveDialog = new FolderSelect.VistaSaveFileDialog();
            this.openDirectoryDialog = new FolderSelect.VistaFolderSelectDialog();
            label2 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MslTextBox)).BeginInit();
            this.documentContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MslTextBox2)).BeginInit();
            this.parsingProgressBarPanel.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.hashtableAddPanel.SuspendLayout();
            this.hashtableAddExpiryPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hashtableAddExpiryTimeBox)).BeginInit();
            this.hashtableHeaderPanel.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            this.errorListToolbar.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.consoleTextBox)).BeginInit();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.debugTextBox)).BeginInit();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.menuBar.SuspendLayout();
            this.newMenu.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.fileContextMenu.SuspendLayout();
            this.mIRCContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 8);
            label2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(30, 13);
            label2.TabIndex = 0;
            label2.Text = "Size:";
            // 
            // label6
            // 
            label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(213, 8);
            label6.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(31, 13);
            label6.TabIndex = 0;
            label6.Text = "Base";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(36, 22);
            toolStripLabel1.Text = "Show";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new System.Drawing.Size(17, 22);
            toolStripLabel2.Text = "In";
            toolStripLabel2.Visible = false;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.treeViewImageList;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(217, 497);
            this.treeView.TabIndex = 1;
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.treeView.DoubleClick += new System.EventHandler(this.treeView_DoubleClick);
            // 
            // treeViewImageList
            // 
            this.treeViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeViewImageList.ImageStream")));
            this.treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.treeViewImageList.Images.SetKeyName(0, "mIRC");
            this.treeViewImageList.Images.SetKeyName(1, "categoryRemote");
            this.treeViewImageList.Images.SetKeyName(2, "categoryAliases");
            this.treeViewImageList.Images.SetKeyName(3, "categoryPopups");
            this.treeViewImageList.Images.SetKeyName(4, "categoryUsers");
            this.treeViewImageList.Images.SetKeyName(5, "categoryVariables");
            this.treeViewImageList.Images.SetKeyName(6, "categoryHashtables");
            this.treeViewImageList.Images.SetKeyName(7, "directory");
            this.treeViewImageList.Images.SetKeyName(8, "file");
            this.treeViewImageList.Images.SetKeyName(9, "remote");
            this.treeViewImageList.Images.SetKeyName(10, "alias");
            this.treeViewImageList.Images.SetKeyName(11, "popup");
            this.treeViewImageList.Images.SetKeyName(12, "variables");
            this.treeViewImageList.Images.SetKeyName(13, "users");
            this.treeViewImageList.Images.SetKeyName(14, "hashtable");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(838, 497);
            this.splitContainer1.SplitterDistance = 217;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.splitContainer2.Panel1.Controls.Add(this.tabControl);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer2.Size = new System.Drawing.Size(617, 497);
            this.splitContainer2.SplitterDistance = 324;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.HotTrack = true;
            this.tabControl.ImageList = this.scriptTypeImageList;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.ShowToolTips = true;
            this.tabControl.Size = new System.Drawing.Size(617, 324);
            this.tabControl.TabIndex = 0;
            this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Selecting);
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            this.tabControl.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Deselecting);
            this.tabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControl_MouseClick);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer);
            this.tabPage1.Controls.Add(this.parsingProgressBarPanel);
            this.tabPage1.Controls.Add(this.functionList);
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(609, 297);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Script";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.Color.Silver;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 42);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.MslTextBox);
            this.splitContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer.Panel1MinSize = 0;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.MslTextBox2);
            this.splitContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer.Panel2MinSize = 0;
            this.splitContainer.Size = new System.Drawing.Size(609, 255);
            this.splitContainer.SplitterDistance = 127;
            this.splitContainer.TabIndex = 3;
            this.splitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            // 
            // MslTextBox
            // 
            this.MslTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.MslTextBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.MslTextBox.AutoScrollMinSize = new System.Drawing.Size(562, 750);
            this.MslTextBox.BackBrush = null;
            this.MslTextBox.ChangedLineColor = System.Drawing.Color.LightBlue;
            this.MslTextBox.CharHeight = 15;
            this.MslTextBox.CharWidth = 7;
            this.MslTextBox.CommentPrefix = ";";
            this.MslTextBox.ContextMenuStrip = this.documentContextMenu;
            this.MslTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MslTextBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.MslTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MslTextBox.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.MslTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.MslTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.MslTextBox.IsReplaceMode = false;
            this.MslTextBox.Location = new System.Drawing.Point(0, 0);
            this.MslTextBox.Name = "MslTextBox";
            this.MslTextBox.Paddings = new System.Windows.Forms.Padding(0);
            this.MslTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.MslTextBox.ServiceColors = null;
            this.MslTextBox.ShowFoldingLines = true;
            this.MslTextBox.Size = new System.Drawing.Size(609, 127);
            this.MslTextBox.TabIndex = 0;
            this.MslTextBox.TabLength = 2;
            this.MslTextBox.Text = resources.GetString("MslTextBox.Text");
            this.MslTextBox.Zoom = 100;
            this.MslTextBox.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.MslTextBox_TextChanged);
            this.MslTextBox.SelectionChangedDelayed += new System.EventHandler(this.MslTextBox_SelectionChangedDelayed);
            this.MslTextBox.LineInserted += new System.EventHandler<FastColoredTextBoxNS.LineInsertedEventArgs>(this.MslTextBox_LineInserted);
            this.MslTextBox.LineRemoved += new System.EventHandler<FastColoredTextBoxNS.LineRemovedEventArgs>(this.MslTextBox_LineRemoved);
            this.MslTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.MslTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.MslTextBox.Enter += new System.EventHandler(this.mSLTextBox_Enter);
            // 
            // documentContextMenu
            // 
            this.documentContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goTodefinitionToolStripMenuItem});
            this.documentContextMenu.Name = "documentContextMenu";
            this.documentContextMenu.Size = new System.Drawing.Size(158, 26);
            this.documentContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.documentContextMenu_Opening);
            // 
            // goTodefinitionToolStripMenuItem
            // 
            this.goTodefinitionToolStripMenuItem.Name = "goTodefinitionToolStripMenuItem";
            this.goTodefinitionToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.goTodefinitionToolStripMenuItem.Text = "Go to &definition";
            this.goTodefinitionToolStripMenuItem.Click += new System.EventHandler(this.goTodefinitionToolStripMenuItem_Click);
            // 
            // MslTextBox2
            // 
            this.MslTextBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.MslTextBox2.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.MslTextBox2.AutoScrollMinSize = new System.Drawing.Size(562, 750);
            this.MslTextBox2.BackBrush = null;
            this.MslTextBox2.ChangedLineColor = System.Drawing.Color.LightBlue;
            this.MslTextBox2.CharHeight = 15;
            this.MslTextBox2.CharWidth = 7;
            this.MslTextBox2.CommentPrefix = ";";
            this.MslTextBox2.ContextMenuStrip = this.documentContextMenu;
            this.MslTextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MslTextBox2.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.MslTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MslTextBox2.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.MslTextBox2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.MslTextBox2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.MslTextBox2.IsReplaceMode = false;
            this.MslTextBox2.Location = new System.Drawing.Point(0, 0);
            this.MslTextBox2.Name = "MslTextBox2";
            this.MslTextBox2.Paddings = new System.Windows.Forms.Padding(0);
            this.MslTextBox2.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.MslTextBox2.ServiceColors = null;
            this.MslTextBox2.ShowFoldingLines = true;
            this.MslTextBox2.Size = new System.Drawing.Size(609, 124);
            this.MslTextBox2.TabIndex = 1;
            this.MslTextBox2.TabLength = 2;
            this.MslTextBox2.Text = resources.GetString("MslTextBox2.Text");
            this.MslTextBox2.Zoom = 100;
            // 
            // parsingProgressBarPanel
            // 
            this.parsingProgressBarPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.parsingProgressBarPanel.ColumnCount = 2;
            this.parsingProgressBarPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.parsingProgressBarPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.parsingProgressBarPanel.Controls.Add(this.label1, 0, 0);
            this.parsingProgressBarPanel.Controls.Add(this.parsingProgressBar, 1, 0);
            this.parsingProgressBarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.parsingProgressBarPanel.Location = new System.Drawing.Point(0, 21);
            this.parsingProgressBarPanel.Name = "parsingProgressBarPanel";
            this.parsingProgressBarPanel.RowCount = 1;
            this.parsingProgressBarPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.parsingProgressBarPanel.Size = new System.Drawing.Size(609, 21);
            this.parsingProgressBarPanel.TabIndex = 2;
            this.parsingProgressBarPanel.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Parsing...";
            // 
            // parsingProgressBar
            // 
            this.parsingProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parsingProgressBar.Location = new System.Drawing.Point(57, 0);
            this.parsingProgressBar.Margin = new System.Windows.Forms.Padding(0);
            this.parsingProgressBar.Name = "parsingProgressBar";
            this.parsingProgressBar.Size = new System.Drawing.Size(552, 21);
            this.parsingProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.parsingProgressBar.TabIndex = 3;
            // 
            // functionList
            // 
            this.functionList.Dock = System.Windows.Forms.DockStyle.Top;
            this.functionList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.functionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.functionList.FormattingEnabled = true;
            this.functionList.Location = new System.Drawing.Point(0, 0);
            this.functionList.Name = "functionList";
            this.functionList.Size = new System.Drawing.Size(609, 21);
            this.functionList.Sorted = true;
            this.functionList.TabIndex = 1;
            this.functionList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.functionList_DrawItem);
            this.functionList.SelectionChangeCommitted += new System.EventHandler(this.functionList_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.hashtableAddPanel);
            this.tabPage2.Controls.Add(this.hashtableListView);
            this.tabPage2.Controls.Add(this.hashtableHeaderPanel);
            this.tabPage2.ImageIndex = 7;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(609, 297);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Hashtable";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // hashtableAddPanel
            // 
            this.hashtableAddPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hashtableAddPanel.ColumnCount = 4;
            this.hashtableAddPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.hashtableAddPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.hashtableAddPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.hashtableAddPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.hashtableAddPanel.Controls.Add(this.hashtableAddTextBox, 1, 0);
            this.hashtableAddPanel.Controls.Add(this.hashtableAddForwardButton, 3, 0);
            this.hashtableAddPanel.Controls.Add(this.hashtableAddBackButton, 0, 0);
            this.hashtableAddPanel.Controls.Add(this.hashtableAddExpiryPanel, 2, 0);
            this.hashtableAddPanel.Location = new System.Drawing.Point(9, 79);
            this.hashtableAddPanel.Name = "hashtableAddPanel";
            this.hashtableAddPanel.RowCount = 1;
            this.hashtableAddPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.hashtableAddPanel.Size = new System.Drawing.Size(394, 21);
            this.hashtableAddPanel.TabIndex = 4;
            this.hashtableAddPanel.Visible = false;
            this.hashtableAddPanel.Leave += new System.EventHandler(this.hashtableAddPanel_Leave);
            // 
            // hashtableAddTextBox
            // 
            this.hashtableAddTextBox.AcceptsTab = true;
            this.hashtableAddTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hashtableAddTextBox.Location = new System.Drawing.Point(22, 0);
            this.hashtableAddTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.hashtableAddTextBox.Name = "hashtableAddTextBox";
            this.hashtableAddTextBox.Size = new System.Drawing.Size(175, 20);
            this.hashtableAddTextBox.TabIndex = 1;
            this.hashtableAddTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.hashtableItemNameTextBox_KeyDown);
            this.hashtableAddTextBox.Leave += new System.EventHandler(this.hashtableItemNameTextBox_Leave);
            this.hashtableAddTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.hashtableItemNameTextBox_Validating);
            // 
            // hashtableAddForwardButton
            // 
            this.hashtableAddForwardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hashtableAddForwardButton.AutoSize = true;
            this.hashtableAddForwardButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hashtableAddForwardButton.Image = ((System.Drawing.Image)(resources.GetObject("hashtableAddForwardButton.Image")));
            this.hashtableAddForwardButton.Location = new System.Drawing.Point(372, 0);
            this.hashtableAddForwardButton.Margin = new System.Windows.Forms.Padding(0);
            this.hashtableAddForwardButton.Name = "hashtableAddForwardButton";
            this.hashtableAddForwardButton.Size = new System.Drawing.Size(22, 21);
            this.hashtableAddForwardButton.TabIndex = 3;
            this.hashtableAddForwardButton.UseVisualStyleBackColor = true;
            this.hashtableAddForwardButton.Enter += new System.EventHandler(this.hashtableAddForwardButton_Enter);
            // 
            // hashtableAddBackButton
            // 
            this.hashtableAddBackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hashtableAddBackButton.AutoSize = true;
            this.hashtableAddBackButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hashtableAddBackButton.Image = ((System.Drawing.Image)(resources.GetObject("hashtableAddBackButton.Image")));
            this.hashtableAddBackButton.Location = new System.Drawing.Point(0, 0);
            this.hashtableAddBackButton.Margin = new System.Windows.Forms.Padding(0);
            this.hashtableAddBackButton.Name = "hashtableAddBackButton";
            this.hashtableAddBackButton.Size = new System.Drawing.Size(22, 21);
            this.hashtableAddBackButton.TabIndex = 0;
            this.hashtableAddBackButton.UseVisualStyleBackColor = true;
            this.hashtableAddBackButton.Enter += new System.EventHandler(this.hashtableAddBackButton_Enter);
            // 
            // hashtableAddExpiryPanel
            // 
            this.hashtableAddExpiryPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hashtableAddExpiryPanel.ColumnCount = 3;
            this.hashtableAddExpiryPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.hashtableAddExpiryPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.hashtableAddExpiryPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.hashtableAddExpiryPanel.Controls.Add(this.hashtableAddExpiryUnitBox, 2, 0);
            this.hashtableAddExpiryPanel.Controls.Add(this.hashtableAddExpiryTimeBox, 1, 0);
            this.hashtableAddExpiryPanel.Controls.Add(this.hashtableItemExpiryCheckBox, 0, 0);
            this.hashtableAddExpiryPanel.Location = new System.Drawing.Point(197, 0);
            this.hashtableAddExpiryPanel.Margin = new System.Windows.Forms.Padding(0);
            this.hashtableAddExpiryPanel.MinimumSize = new System.Drawing.Size(99, 0);
            this.hashtableAddExpiryPanel.Name = "hashtableAddExpiryPanel";
            this.hashtableAddExpiryPanel.RowCount = 1;
            this.hashtableAddExpiryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.hashtableAddExpiryPanel.Size = new System.Drawing.Size(175, 21);
            this.hashtableAddExpiryPanel.TabIndex = 2;
            this.hashtableAddExpiryPanel.Layout += new System.Windows.Forms.LayoutEventHandler(this.hashtableAddExpiryPanel_Layout);
            this.hashtableAddExpiryPanel.Leave += new System.EventHandler(this.flowLayoutPanel1_Leave);
            // 
            // hashtableAddExpiryUnitBox
            // 
            this.hashtableAddExpiryUnitBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.hashtableAddExpiryUnitBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hashtableAddExpiryUnitBox.ForeColor = System.Drawing.Color.White;
            this.hashtableAddExpiryUnitBox.Items.AddRange(new object[] {
            "seconds",
            "minutes",
            "hours",
            "days"});
            this.hashtableAddExpiryUnitBox.Location = new System.Drawing.Point(115, 0);
            this.hashtableAddExpiryUnitBox.Margin = new System.Windows.Forms.Padding(0);
            this.hashtableAddExpiryUnitBox.Name = "hashtableAddExpiryUnitBox";
            this.hashtableAddExpiryUnitBox.Size = new System.Drawing.Size(60, 21);
            this.hashtableAddExpiryUnitBox.TabIndex = 7;
            this.hashtableAddExpiryUnitBox.TabStop = false;
            // 
            // hashtableAddExpiryTimeBox
            // 
            this.hashtableAddExpiryTimeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hashtableAddExpiryTimeBox.Location = new System.Drawing.Point(35, 0);
            this.hashtableAddExpiryTimeBox.Margin = new System.Windows.Forms.Padding(0);
            this.hashtableAddExpiryTimeBox.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.hashtableAddExpiryTimeBox.Name = "hashtableAddExpiryTimeBox";
            this.hashtableAddExpiryTimeBox.Size = new System.Drawing.Size(80, 20);
            this.hashtableAddExpiryTimeBox.TabIndex = 6;
            this.hashtableAddExpiryTimeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.hashtableAddExpiryTimeBox.TextChanged += new System.EventHandler(this.hashtableAddExpiryTimeBox_TextChanged);
            this.hashtableAddExpiryTimeBox.ValueChanged += new System.EventHandler(this.hashtableAddExpiryTimeBox_ValueChanged);
            this.hashtableAddExpiryTimeBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.hashtableAddExpiryTimeBox_KeyDown);
            // 
            // hashtableItemExpiryCheckBox
            // 
            this.hashtableItemExpiryCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.hashtableItemExpiryCheckBox.AutoSize = true;
            this.hashtableItemExpiryCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.hashtableItemExpiryCheckBox.Location = new System.Drawing.Point(0, 2);
            this.hashtableItemExpiryCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.hashtableItemExpiryCheckBox.Name = "hashtableItemExpiryCheckBox";
            this.hashtableItemExpiryCheckBox.Size = new System.Drawing.Size(35, 17);
            this.hashtableItemExpiryCheckBox.TabIndex = 5;
            this.hashtableItemExpiryCheckBox.TabStop = false;
            this.hashtableItemExpiryCheckBox.Text = "In";
            this.hashtableItemExpiryCheckBox.UseVisualStyleBackColor = false;
            this.hashtableItemExpiryCheckBox.CheckedChanged += new System.EventHandler(this.hashtableItemExpiryCheckBox_CheckedChanged);
            // 
            // hashtableListView
            // 
            this.hashtableListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.hashtableListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hashtableListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.hashtableListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.hashtableListView.Location = new System.Drawing.Point(3, 32);
            this.hashtableListView.Name = "hashtableListView";
            this.hashtableListView.Size = new System.Drawing.Size(603, 262);
            this.hashtableListView.TabIndex = 1;
            this.hashtableListView.UseCompatibleStateImageBehavior = false;
            this.hashtableListView.View = System.Windows.Forms.View.Details;
            this.hashtableListView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.hashtableListView_AfterLabelEdit);
            this.hashtableListView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView_DrawColumnHeader);
            this.hashtableListView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listView_DrawItem);
            this.hashtableListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listView_DrawSubItem);
            this.hashtableListView.ItemActivate += new System.EventHandler(this.hashtableListView_ItemActivate);
            this.hashtableListView.DoubleClick += new System.EventHandler(this.hashtableListView_DoubleClick);
            this.hashtableListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.hashtableListView_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Key";
            this.columnHeader1.Width = 159;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 182;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Expires";
            this.columnHeader3.Width = 120;
            // 
            // hashtableHeaderPanel
            // 
            this.hashtableHeaderPanel.AutoSize = true;
            this.hashtableHeaderPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hashtableHeaderPanel.Controls.Add(label2);
            this.hashtableHeaderPanel.Controls.Add(this.label3);
            this.hashtableHeaderPanel.Controls.Add(this.label4);
            this.hashtableHeaderPanel.Controls.Add(this.label5);
            this.hashtableHeaderPanel.Controls.Add(this.hashtableBinaryViewButton);
            this.hashtableHeaderPanel.Controls.Add(label6);
            this.hashtableHeaderPanel.Controls.Add(this.hashtableBaseComboBox);
            this.hashtableHeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.hashtableHeaderPanel.Location = new System.Drawing.Point(3, 3);
            this.hashtableHeaderPanel.Name = "hashtableHeaderPanel";
            this.hashtableHeaderPanel.Size = new System.Drawing.Size(603, 29);
            this.hashtableHeaderPanel.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(39, 8);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "0";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 8);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Capacity:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(116, 8);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "0";
            // 
            // hashtableBinaryViewButton
            // 
            this.hashtableBinaryViewButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.hashtableBinaryViewButton.AutoSize = true;
            this.hashtableBinaryViewButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.hashtableBinaryViewButton.Location = new System.Drawing.Point(136, 3);
            this.hashtableBinaryViewButton.Name = "hashtableBinaryViewButton";
            this.hashtableBinaryViewButton.Size = new System.Drawing.Size(71, 23);
            this.hashtableBinaryViewButton.TabIndex = 2;
            this.hashtableBinaryViewButton.Text = "Binary view";
            this.hashtableBinaryViewButton.UseVisualStyleBackColor = true;
            this.hashtableBinaryViewButton.CheckedChanged += new System.EventHandler(this.hashtableBinaryViewButton_CheckedChanged);
            // 
            // hashtableBaseComboBox
            // 
            this.hashtableBaseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hashtableBaseComboBox.FormattingEnabled = true;
            this.hashtableBaseComboBox.Items.AddRange(new object[] {
            "Decimal",
            "Hexadecimal"});
            this.hashtableBaseComboBox.Location = new System.Drawing.Point(250, 4);
            this.hashtableBaseComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.hashtableBaseComboBox.Name = "hashtableBaseComboBox";
            this.hashtableBaseComboBox.Size = new System.Drawing.Size(100, 21);
            this.hashtableBaseComboBox.TabIndex = 3;
            this.hashtableBaseComboBox.SelectedIndexChanged += new System.EventHandler(this.hashtableBaseComboBox_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dialogDesigner);
            this.tabPage3.ImageIndex = 9;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(609, 297);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Dialog";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dialogDesigner
            // 
            this.dialogDesigner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.dialogDesigner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dialogDesigner.ForeColor = System.Drawing.Color.White;
            this.dialogDesigner.Location = new System.Drawing.Point(3, 3);
            this.dialogDesigner.Name = "dialogDesigner";
            this.dialogDesigner.Size = new System.Drawing.Size(603, 291);
            this.dialogDesigner.TabIndex = 0;
            this.dialogDesigner.Theme = MScripter.Theme.Dark;
            this.dialogDesigner.ViewCode += new System.EventHandler(this.dialogDesigner_ViewCode);
            // 
            // scriptTypeImageList
            // 
            this.scriptTypeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("scriptTypeImageList.ImageStream")));
            this.scriptTypeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.scriptTypeImageList.Images.SetKeyName(0, "remote");
            this.scriptTypeImageList.Images.SetKeyName(1, "aliases");
            this.scriptTypeImageList.Images.SetKeyName(2, "popups");
            this.scriptTypeImageList.Images.SetKeyName(3, "variables");
            this.scriptTypeImageList.Images.SetKeyName(4, "users");
            this.scriptTypeImageList.Images.SetKeyName(5, "text");
            this.scriptTypeImageList.Images.SetKeyName(6, "INI");
            this.scriptTypeImageList.Images.SetKeyName(7, "hashtable");
            this.scriptTypeImageList.Images.SetKeyName(8, "misc");
            this.scriptTypeImageList.Images.SetKeyName(9, "dialog");
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(617, 169);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.toolStripContainer2);
            this.tabPage5.Location = new System.Drawing.Point(4, 4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(609, 143);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Error List";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // toolStripContainer2
            // 
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.Controls.Add(this.errorList);
            this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(609, 118);
            this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer2.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.Size = new System.Drawing.Size(609, 143);
            this.toolStripContainer2.TabIndex = 2;
            this.toolStripContainer2.Text = "toolStripContainer2";
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.errorListToolbar);
            // 
            // errorList
            // 
            this.errorList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader7,
            this.columnHeader8});
            this.errorList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorList.FullRowSelect = true;
            this.errorList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.errorList.Location = new System.Drawing.Point(0, 0);
            this.errorList.Margin = new System.Windows.Forms.Padding(0);
            this.errorList.Name = "errorList";
            this.errorList.Size = new System.Drawing.Size(609, 118);
            this.errorList.StateImageList = this.errorImageList;
            this.errorList.TabIndex = 0;
            this.errorList.UseCompatibleStateImageBehavior = false;
            this.errorList.View = System.Windows.Forms.View.Details;
            this.errorList.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView_DrawColumnHeader);
            this.errorList.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listView_DrawItem);
            this.errorList.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listView_DrawSubItem);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Category";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "#";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Description";
            this.columnHeader7.Width = 198;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Location";
            this.columnHeader8.Width = 155;
            // 
            // errorImageList
            // 
            this.errorImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("errorImageList.ImageStream")));
            this.errorImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.errorImageList.Images.SetKeyName(0, "error");
            this.errorImageList.Images.SetKeyName(1, "warning");
            this.errorImageList.Images.SetKeyName(2, "information.png");
            // 
            // errorListToolbar
            // 
            this.errorListToolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.errorListToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripLabel1,
            this.errorsToolStripButton,
            this.warningsToolStripButton,
            this.noticesToolStripButton,
            this.toolStripSeparator19,
            toolStripLabel2,
            this.errorListScopeComboBox});
            this.errorListToolbar.Location = new System.Drawing.Point(0, 0);
            this.errorListToolbar.Name = "errorListToolbar";
            this.errorListToolbar.Size = new System.Drawing.Size(609, 25);
            this.errorListToolbar.Stretch = true;
            this.errorListToolbar.TabIndex = 1;
            this.errorListToolbar.EndDrag += new System.EventHandler(this.errorListToolbar_EndDrag);
            // 
            // errorsToolStripButton
            // 
            this.errorsToolStripButton.AutoToolTip = false;
            this.errorsToolStripButton.Checked = true;
            this.errorsToolStripButton.CheckOnClick = true;
            this.errorsToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.errorsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("errorsToolStripButton.Image")));
            this.errorsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.errorsToolStripButton.Name = "errorsToolStripButton";
            this.errorsToolStripButton.Size = new System.Drawing.Size(57, 22);
            this.errorsToolStripButton.Text = "Errors";
            this.errorsToolStripButton.CheckStateChanged += new System.EventHandler(this.errorsToolStripButton_CheckStateChanged);
            // 
            // warningsToolStripButton
            // 
            this.warningsToolStripButton.AutoToolTip = false;
            this.warningsToolStripButton.Checked = true;
            this.warningsToolStripButton.CheckOnClick = true;
            this.warningsToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.warningsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("warningsToolStripButton.Image")));
            this.warningsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.warningsToolStripButton.Name = "warningsToolStripButton";
            this.warningsToolStripButton.Size = new System.Drawing.Size(77, 22);
            this.warningsToolStripButton.Text = "Warnings";
            this.warningsToolStripButton.CheckStateChanged += new System.EventHandler(this.errorsToolStripButton_CheckStateChanged);
            // 
            // noticesToolStripButton
            // 
            this.noticesToolStripButton.AutoToolTip = false;
            this.noticesToolStripButton.Checked = true;
            this.noticesToolStripButton.CheckOnClick = true;
            this.noticesToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.noticesToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("noticesToolStripButton.Image")));
            this.noticesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.noticesToolStripButton.Name = "noticesToolStripButton";
            this.noticesToolStripButton.Size = new System.Drawing.Size(67, 22);
            this.noticesToolStripButton.Text = "Notices";
            this.noticesToolStripButton.CheckStateChanged += new System.EventHandler(this.errorsToolStripButton_CheckStateChanged);
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            this.toolStripSeparator19.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator19.Visible = false;
            // 
            // errorListScopeComboBox
            // 
            this.errorListScopeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorListScopeComboBox.Items.AddRange(new object[] {
            "Current file"});
            this.errorListScopeComboBox.Name = "errorListScopeComboBox";
            this.errorListScopeComboBox.Size = new System.Drawing.Size(95, 25);
            this.errorListScopeComboBox.Visible = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.consoleTextBox);
            this.tabPage4.Location = new System.Drawing.Point(4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(609, 143);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Console";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // consoleTextBox
            // 
            this.consoleTextBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.consoleTextBox.AutoScrollMinSize = new System.Drawing.Size(2, 15);
            this.consoleTextBox.BackBrush = null;
            this.consoleTextBox.BackColor = System.Drawing.Color.Black;
            this.consoleTextBox.CaretColor = System.Drawing.Color.White;
            this.consoleTextBox.CharHeight = 15;
            this.consoleTextBox.CharWidth = 7;
            this.consoleTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.consoleTextBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.consoleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleTextBox.FoldingIndicatorColor = System.Drawing.Color.Gold;
            this.consoleTextBox.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.consoleTextBox.ForeColor = System.Drawing.Color.White;
            this.consoleTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.consoleTextBox.IndentBackColor = System.Drawing.Color.Black;
            this.consoleTextBox.IsReadLineMode = false;
            this.consoleTextBox.IsReplaceMode = false;
            this.consoleTextBox.LineNumberColor = System.Drawing.Color.Gold;
            this.consoleTextBox.Location = new System.Drawing.Point(0, 0);
            this.consoleTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.consoleTextBox.Name = "consoleTextBox";
            this.consoleTextBox.PaddingBackColor = System.Drawing.Color.Black;
            this.consoleTextBox.Paddings = new System.Windows.Forms.Padding(0);
            this.consoleTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.consoleTextBox.ServiceColors = null;
            this.consoleTextBox.ServiceLinesColor = System.Drawing.Color.DimGray;
            this.consoleTextBox.Size = new System.Drawing.Size(609, 143);
            this.consoleTextBox.TabIndex = 0;
            this.consoleTextBox.TabLength = 2;
            this.consoleTextBox.Zoom = 100;
            this.consoleTextBox.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.consoleTextBox_TextChanged);
            this.consoleTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.consoleTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.consoleTextBox.Paint += new System.Windows.Forms.PaintEventHandler(this.consoleTextBox_Paint);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.debugTextBox);
            this.tabPage6.Location = new System.Drawing.Point(4, 4);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(609, 143);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Debug";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // debugTextBox
            // 
            this.debugTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.debugTextBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.debugTextBox.AutoScrollMinSize = new System.Drawing.Size(128, 75);
            this.debugTextBox.BackBrush = null;
            this.debugTextBox.BackColor = System.Drawing.Color.Black;
            this.debugTextBox.CaretColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.debugTextBox.ChangedLineColor = System.Drawing.Color.LightBlue;
            this.debugTextBox.CharHeight = 15;
            this.debugTextBox.CharWidth = 7;
            this.debugTextBox.CommentPrefix = ";";
            this.debugTextBox.ContextMenuStrip = this.documentContextMenu;
            this.debugTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.debugTextBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.debugTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugTextBox.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.debugTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.debugTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.debugTextBox.IsReplaceMode = false;
            this.debugTextBox.Location = new System.Drawing.Point(3, 3);
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.Paddings = new System.Windows.Forms.Padding(0);
            this.debugTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.debugTextBox.ServiceColors = null;
            this.debugTextBox.ShowFoldingLines = true;
            this.debugTextBox.ShowLineNumbers = false;
            this.debugTextBox.Size = new System.Drawing.Size(603, 137);
            this.debugTextBox.TabIndex = 1;
            this.debugTextBox.TabLength = 2;
            this.debugTextBox.Text = "Current line (1):\r\nop mode # +ooo $1-\r\n\r\n1 tag:\r\n  Command @ 0 - 2";
            this.debugTextBox.Zoom = 100;
            this.debugTextBox.SelectionChangedDelayed += new System.EventHandler(this.debugTextBox_SelectionChangedDelayed);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(838, 497);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(838, 546);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuBar);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolbar);
            // 
            // menuBar
            // 
            this.menuBar.Dock = System.Windows.Forms.DockStyle.None;
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(838, 24);
            this.menuBar.TabIndex = 0;
            this.menuBar.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.openDirectoryToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.loadInToolStripMenuItem,
            this.toolStripSeparator3,
            this.revertToolStripMenuItem,
            this.closeToolStripMenuItem1,
            this.toolStripSeparator13,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDown = this.newMenu;
            this.newToolStripMenuItem.Image = global::MScripter.Properties.Resources.newDocument;
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+N";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // newMenu
            // 
            this.newMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newRemoteScriptToolStripMenuItem,
            this.newAliasScriptToolStripMenuItem,
            this.newPopupScriptToolStripMenuItem,
            this.toolStripSeparator14,
            this.newTextFileToolStripMenuItem,
            this.newINIFileToolStripMenuItem,
            this.toolStripSeparator15,
            this.newHashtableToolStripMenuItem,
            this.newDialogToolStripMenuItem});
            this.newMenu.Name = "fileContextMenu";
            this.newMenu.OwnerItem = this.newToolStripButton;
            this.newMenu.Size = new System.Drawing.Size(148, 170);
            // 
            // newRemoteScriptToolStripMenuItem
            // 
            this.newRemoteScriptToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newRemoteScriptToolStripMenuItem.Image")));
            this.newRemoteScriptToolStripMenuItem.Name = "newRemoteScriptToolStripMenuItem";
            this.newRemoteScriptToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.newRemoteScriptToolStripMenuItem.Text = "&Remote script";
            this.newRemoteScriptToolStripMenuItem.Click += new System.EventHandler(this.newRemoteScriptToolStripMenuItem_Click);
            // 
            // newAliasScriptToolStripMenuItem
            // 
            this.newAliasScriptToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newAliasScriptToolStripMenuItem.Image")));
            this.newAliasScriptToolStripMenuItem.Name = "newAliasScriptToolStripMenuItem";
            this.newAliasScriptToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.newAliasScriptToolStripMenuItem.Text = "&Alias script";
            this.newAliasScriptToolStripMenuItem.Click += new System.EventHandler(this.newAliasScriptToolStripMenuItem_Click);
            // 
            // newPopupScriptToolStripMenuItem
            // 
            this.newPopupScriptToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newPopupScriptToolStripMenuItem.Image")));
            this.newPopupScriptToolStripMenuItem.Name = "newPopupScriptToolStripMenuItem";
            this.newPopupScriptToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.newPopupScriptToolStripMenuItem.Text = "&Popup script";
            this.newPopupScriptToolStripMenuItem.Click += new System.EventHandler(this.newPopupScriptToolStripMenuItem_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(144, 6);
            // 
            // newTextFileToolStripMenuItem
            // 
            this.newTextFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newTextFileToolStripMenuItem.Image")));
            this.newTextFileToolStripMenuItem.Name = "newTextFileToolStripMenuItem";
            this.newTextFileToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.newTextFileToolStripMenuItem.Text = "&Text file";
            // 
            // newINIFileToolStripMenuItem
            // 
            this.newINIFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newINIFileToolStripMenuItem.Image")));
            this.newINIFileToolStripMenuItem.Name = "newINIFileToolStripMenuItem";
            this.newINIFileToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.newINIFileToolStripMenuItem.Text = "&INI file";
            this.newINIFileToolStripMenuItem.Click += new System.EventHandler(this.newINIFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(144, 6);
            // 
            // newHashtableToolStripMenuItem
            // 
            this.newHashtableToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newHashtableToolStripMenuItem.Image")));
            this.newHashtableToolStripMenuItem.Name = "newHashtableToolStripMenuItem";
            this.newHashtableToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.newHashtableToolStripMenuItem.Text = "&Hashtable";
            this.newHashtableToolStripMenuItem.Click += new System.EventHandler(this.newHashtableToolStripMenuItem_Click);
            // 
            // newDialogToolStripMenuItem
            // 
            this.newDialogToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newDialogToolStripMenuItem.Image")));
            this.newDialogToolStripMenuItem.Name = "newDialogToolStripMenuItem";
            this.newDialogToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.newDialogToolStripMenuItem.Text = "&Dialog";
            this.newDialogToolStripMenuItem.Click += new System.EventHandler(this.newDialogToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openDirectoryToolStripMenuItem
            // 
            this.openDirectoryToolStripMenuItem.Image = global::MScripter.Properties.Resources.openDirectory;
            this.openDirectoryToolStripMenuItem.Name = "openDirectoryToolStripMenuItem";
            this.openDirectoryToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openDirectoryToolStripMenuItem.Text = "Open &directory";
            this.openDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openDirectoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(184, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAsToolStripMenuItem.Image")));
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAllToolStripMenuItem.Image")));
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveAllToolStripMenuItem.Text = "Save A&ll";
            // 
            // loadInToolStripMenuItem
            // 
            this.loadInToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noRunningMIRCInstancesToolStripMenuItem});
            this.loadInToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("loadInToolStripMenuItem.Image")));
            this.loadInToolStripMenuItem.Name = "loadInToolStripMenuItem";
            this.loadInToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.loadInToolStripMenuItem.Text = "&Load in mIRC";
            this.loadInToolStripMenuItem.DropDownOpening += new System.EventHandler(this.loadInToolStripMenuItem_DropDownOpening);
            // 
            // noRunningMIRCInstancesToolStripMenuItem
            // 
            this.noRunningMIRCInstancesToolStripMenuItem.Enabled = false;
            this.noRunningMIRCInstancesToolStripMenuItem.Name = "noRunningMIRCInstancesToolStripMenuItem";
            this.noRunningMIRCInstancesToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.noRunningMIRCInstancesToolStripMenuItem.Text = "No running mIRC instances";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(184, 6);
            // 
            // revertToolStripMenuItem
            // 
            this.revertToolStripMenuItem.Name = "revertToolStripMenuItem";
            this.revertToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.revertToolStripMenuItem.Text = "&Revert";
            this.revertToolStripMenuItem.Click += new System.EventHandler(this.revertToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem1
            // 
            this.closeToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("closeToolStripMenuItem1.Image")));
            this.closeToolStripMenuItem1.Name = "closeToolStripMenuItem1";
            this.closeToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.closeToolStripMenuItem1.Size = new System.Drawing.Size(187, 22);
            this.closeToolStripMenuItem1.Text = "&Close";
            this.closeToolStripMenuItem1.Click += new System.EventHandler(this.closeToolStripMenuItem1_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(184, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator5,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator6,
            this.selectAllToolStripMenuItem,
            this.toolStripSeparator8,
            this.restyleLineToolStripMenuItem,
            this.restyleDocumentToolStripMenuItem,
            this.toolStripSeparator21,
            this.foldingToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("undoToolStripMenuItem.Image")));
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Z";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("redoToolStripMenuItem.Image")));
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Y";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(239, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+X";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+C";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+V";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(239, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+A";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.selectAllToolStripMenuItem.Text = "Select &All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(239, 6);
            // 
            // restyleLineToolStripMenuItem
            // 
            this.restyleLineToolStripMenuItem.Name = "restyleLineToolStripMenuItem";
            this.restyleLineToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.restyleLineToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.restyleLineToolStripMenuItem.Text = "Re&style line";
            this.restyleLineToolStripMenuItem.Click += new System.EventHandler(this.restyleLineToolStripMenuItem_Click);
            // 
            // restyleDocumentToolStripMenuItem
            // 
            this.restyleDocumentToolStripMenuItem.Name = "restyleDocumentToolStripMenuItem";
            this.restyleDocumentToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.restyleDocumentToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.restyleDocumentToolStripMenuItem.Text = "Res&tyle document";
            this.restyleDocumentToolStripMenuItem.Click += new System.EventHandler(this.restyleDocumentToolStripMenuItem_Click);
            // 
            // toolStripSeparator21
            // 
            this.toolStripSeparator21.Name = "toolStripSeparator21";
            this.toolStripSeparator21.Size = new System.Drawing.Size(239, 6);
            // 
            // foldingToolStripMenuItem
            // 
            this.foldingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.foldToolStripMenuItem,
            this.unfoldToolStripMenuItem,
            this.toolStripSeparator22,
            this.unfoldAllToolStripMenuItem,
            this.foldallToolStripMenuItem});
            this.foldingToolStripMenuItem.Name = "foldingToolStripMenuItem";
            this.foldingToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.foldingToolStripMenuItem.Text = "Fol&ding";
            // 
            // foldToolStripMenuItem
            // 
            this.foldToolStripMenuItem.Name = "foldToolStripMenuItem";
            this.foldToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.foldToolStripMenuItem.Text = "&Fold";
            this.foldToolStripMenuItem.Click += new System.EventHandler(this.foldToolStripMenuItem_Click);
            // 
            // unfoldToolStripMenuItem
            // 
            this.unfoldToolStripMenuItem.Name = "unfoldToolStripMenuItem";
            this.unfoldToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.unfoldToolStripMenuItem.Text = "&Unfold";
            this.unfoldToolStripMenuItem.Click += new System.EventHandler(this.unfoldToolStripMenuItem_Click);
            // 
            // toolStripSeparator22
            // 
            this.toolStripSeparator22.Name = "toolStripSeparator22";
            this.toolStripSeparator22.Size = new System.Drawing.Size(122, 6);
            // 
            // unfoldAllToolStripMenuItem
            // 
            this.unfoldAllToolStripMenuItem.Name = "unfoldAllToolStripMenuItem";
            this.unfoldAllToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.unfoldAllToolStripMenuItem.Text = "U&nfold all";
            this.unfoldAllToolStripMenuItem.Click += new System.EventHandler(this.unfoldAllToolStripMenuItem_Click);
            // 
            // foldallToolStripMenuItem
            // 
            this.foldallToolStripMenuItem.Name = "foldallToolStripMenuItem";
            this.foldallToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.foldallToolStripMenuItem.Text = "Fold &all";
            this.foldallToolStripMenuItem.Click += new System.EventHandler(this.foldallToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.splitToolStripMenuItem,
            this.toolStripSeparator23,
            this.dialogDesignerToolStripMenuItem,
            this.toolStripSeparator17,
            this.interfaceThemeToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            this.viewToolStripMenuItem.DropDownOpening += new System.EventHandler(this.viewToolStripMenuItem_DropDownOpening);
            this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // splitToolStripMenuItem
            // 
            this.splitToolStripMenuItem.Name = "splitToolStripMenuItem";
            this.splitToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.splitToolStripMenuItem.Text = "&Split";
            this.splitToolStripMenuItem.Click += new System.EventHandler(this.splitToolStripMenuItem_Click);
            // 
            // toolStripSeparator23
            // 
            this.toolStripSeparator23.Name = "toolStripSeparator23";
            this.toolStripSeparator23.Size = new System.Drawing.Size(154, 6);
            // 
            // dialogDesignerToolStripMenuItem
            // 
            this.dialogDesignerToolStripMenuItem.Name = "dialogDesignerToolStripMenuItem";
            this.dialogDesignerToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.dialogDesignerToolStripMenuItem.Text = "&Dialog designer";
            this.dialogDesignerToolStripMenuItem.Click += new System.EventHandler(this.dialogDesignerToolStripMenuItem_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(154, 6);
            // 
            // interfaceThemeToolStripMenuItem
            // 
            this.interfaceThemeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.standardThemeToolStripMenuItem,
            this.darkThemeToolStripMenuItem});
            this.interfaceThemeToolStripMenuItem.Name = "interfaceThemeToolStripMenuItem";
            this.interfaceThemeToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.interfaceThemeToolStripMenuItem.Text = "Interface &theme";
            // 
            // standardThemeToolStripMenuItem
            // 
            this.standardThemeToolStripMenuItem.Name = "standardThemeToolStripMenuItem";
            this.standardThemeToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.standardThemeToolStripMenuItem.Text = "Standard theme";
            this.standardThemeToolStripMenuItem.Click += new System.EventHandler(this.standardThemeToolStripMenuItem_Click);
            // 
            // darkThemeToolStripMenuItem
            // 
            this.darkThemeToolStripMenuItem.Name = "darkThemeToolStripMenuItem";
            this.darkThemeToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.darkThemeToolStripMenuItem.Text = "Dark theme";
            this.darkThemeToolStripMenuItem.Click += new System.EventHandler(this.darkThemeToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator7,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Visible = false;
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.contentsToolStripMenuItem.Text = "&Contents";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(119, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // toolbar
            // 
            this.toolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.toolStripSeparator,
            this.openDirectoryToolStripButton,
            this.refreshToolStripButton,
            this.toolStripSeparator18,
            this.undoToolStripButton,
            this.redoToolStripButton,
            this.toolStripSeparator4,
            this.cutToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.toolStripSeparator1,
            this.helpToolStripButton});
            this.toolbar.Location = new System.Drawing.Point(0, 24);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(838, 25);
            this.toolbar.Stretch = true;
            this.toolbar.TabIndex = 1;
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.DropDown = this.newMenu;
            this.newToolStripButton.ForeColor = System.Drawing.Color.White;
            this.newToolStripButton.Image = global::MScripter.Properties.Resources.newDocument;
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(29, 22);
            this.newToolStripButton.Text = "&New";
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "&Open";
            this.openToolStripButton.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // openDirectoryToolStripButton
            // 
            this.openDirectoryToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openDirectoryToolStripButton.Image = global::MScripter.Properties.Resources.openDirectory;
            this.openDirectoryToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openDirectoryToolStripButton.Name = "openDirectoryToolStripButton";
            this.openDirectoryToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openDirectoryToolStripButton.Text = "Open &directory";
            this.openDirectoryToolStripButton.Click += new System.EventHandler(this.openDirectoryToolStripMenuItem_Click);
            // 
            // refreshToolStripButton
            // 
            this.refreshToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshToolStripButton.Image = global::MScripter.Properties.Resources.refresh;
            this.refreshToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshToolStripButton.Name = "refreshToolStripButton";
            this.refreshToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.refreshToolStripButton.Text = "Refresh file &list";
            this.refreshToolStripButton.Click += new System.EventHandler(this.refreshToolStripButton_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(6, 25);
            // 
            // undoToolStripButton
            // 
            this.undoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.undoToolStripButton.Image = global::MScripter.Properties.Resources.undo;
            this.undoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.undoToolStripButton.Name = "undoToolStripButton";
            this.undoToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.undoToolStripButton.Text = "&Undo";
            this.undoToolStripButton.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripButton
            // 
            this.redoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.redoToolStripButton.Image = global::MScripter.Properties.Resources.redo;
            this.redoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.redoToolStripButton.Name = "redoToolStripButton";
            this.redoToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.redoToolStripButton.Text = "&Redo";
            this.redoToolStripButton.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // cutToolStripButton
            // 
            this.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cutToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripButton.Image")));
            this.cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripButton.Name = "cutToolStripButton";
            this.cutToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.cutToolStripButton.Text = "C&ut";
            this.cutToolStripButton.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripButton
            // 
            this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
            this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripButton.Name = "copyToolStripButton";
            this.copyToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.copyToolStripButton.Text = "&Copy";
            this.copyToolStripButton.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripButton
            // 
            this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pasteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripButton.Image")));
            this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripButton.Name = "pasteToolStripButton";
            this.pasteToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.pasteToolStripButton.Text = "&Paste";
            this.pasteToolStripButton.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator1.Visible = false;
            // 
            // helpToolStripButton
            // 
            this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
            this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpToolStripButton.Name = "helpToolStripButton";
            this.helpToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.helpToolStripButton.Text = "He&lp";
            this.helpToolStripButton.Visible = false;
            // 
            // fileContextMenu
            // 
            this.fileContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem,
            this.closeAllButThisToolStripMenuItem,
            this.closeAllToTheRightToolStripMenuItem,
            this.toolStripSeparator10,
            this.saveToolStripMenuItem1,
            this.saveAsToolStripMenuItem1,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator9,
            this.formatAsToolStripMenuItem});
            this.fileContextMenu.Name = "fileContextMenu";
            this.fileContextMenu.Size = new System.Drawing.Size(181, 192);
            this.fileContextMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.fileContextMenu_Closed);
            this.fileContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.fileContextMenu_Opening);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("closeToolStripMenuItem.Image")));
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // closeAllButThisToolStripMenuItem
            // 
            this.closeAllButThisToolStripMenuItem.Name = "closeAllButThisToolStripMenuItem";
            this.closeAllButThisToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeAllButThisToolStripMenuItem.Text = "Close all but this";
            // 
            // closeAllToTheRightToolStripMenuItem
            // 
            this.closeAllToTheRightToolStripMenuItem.Name = "closeAllToTheRightToolStripMenuItem";
            this.closeAllToTheRightToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeAllToTheRightToolStripMenuItem.Text = "Close all to the right";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(177, 6);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem1.Image")));
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            // 
            // saveAsToolStripMenuItem1
            // 
            this.saveAsToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("saveAsToolStripMenuItem1.Image")));
            this.saveAsToolStripMenuItem1.Name = "saveAsToolStripMenuItem1";
            this.saveAsToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem1.Text = "Save as";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(177, 6);
            // 
            // formatAsToolStripMenuItem
            // 
            this.formatAsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remoteScriptToolStripMenuItem,
            this.aliasScriptToolStripMenuItem,
            this.popupScriptToolStripMenuItem,
            this.toolStripSeparator11,
            this.usersListToolStripMenuItem,
            this.variablesListToolStripMenuItem,
            this.toolStripSeparator20,
            this.iNIFileToolStripMenuItem,
            this.textFileToolStripMenuItem});
            this.formatAsToolStripMenuItem.Name = "formatAsToolStripMenuItem";
            this.formatAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.formatAsToolStripMenuItem.Text = "Format as";
            // 
            // remoteScriptToolStripMenuItem
            // 
            this.remoteScriptToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("remoteScriptToolStripMenuItem.Image")));
            this.remoteScriptToolStripMenuItem.Name = "remoteScriptToolStripMenuItem";
            this.remoteScriptToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.remoteScriptToolStripMenuItem.Text = "Remote script";
            this.remoteScriptToolStripMenuItem.Click += new System.EventHandler(this.remoteScriptToolStripMenuItem_Click);
            // 
            // aliasScriptToolStripMenuItem
            // 
            this.aliasScriptToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aliasScriptToolStripMenuItem.Image")));
            this.aliasScriptToolStripMenuItem.Name = "aliasScriptToolStripMenuItem";
            this.aliasScriptToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.aliasScriptToolStripMenuItem.Text = "Alias script";
            this.aliasScriptToolStripMenuItem.Click += new System.EventHandler(this.aliasScriptToolStripMenuItem_Click);
            // 
            // popupScriptToolStripMenuItem
            // 
            this.popupScriptToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("popupScriptToolStripMenuItem.Image")));
            this.popupScriptToolStripMenuItem.Name = "popupScriptToolStripMenuItem";
            this.popupScriptToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.popupScriptToolStripMenuItem.Text = "Popup script";
            this.popupScriptToolStripMenuItem.Click += new System.EventHandler(this.popupScriptToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(144, 6);
            // 
            // usersListToolStripMenuItem
            // 
            this.usersListToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("usersListToolStripMenuItem.Image")));
            this.usersListToolStripMenuItem.Name = "usersListToolStripMenuItem";
            this.usersListToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.usersListToolStripMenuItem.Text = "Users list";
            this.usersListToolStripMenuItem.Click += new System.EventHandler(this.usersListToolStripMenuItem_Click);
            // 
            // variablesListToolStripMenuItem
            // 
            this.variablesListToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("variablesListToolStripMenuItem.Image")));
            this.variablesListToolStripMenuItem.Name = "variablesListToolStripMenuItem";
            this.variablesListToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.variablesListToolStripMenuItem.Text = "Variables list";
            this.variablesListToolStripMenuItem.Click += new System.EventHandler(this.variablesListToolStripMenuItem_Click);
            // 
            // toolStripSeparator20
            // 
            this.toolStripSeparator20.Name = "toolStripSeparator20";
            this.toolStripSeparator20.Size = new System.Drawing.Size(144, 6);
            // 
            // iNIFileToolStripMenuItem
            // 
            this.iNIFileToolStripMenuItem.Image = global::MScripter.Properties.Resources.documentINI;
            this.iNIFileToolStripMenuItem.Name = "iNIFileToolStripMenuItem";
            this.iNIFileToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.iNIFileToolStripMenuItem.Text = "INI file";
            this.iNIFileToolStripMenuItem.Click += new System.EventHandler(this.iNIFileToolStripMenuItem_Click);
            // 
            // textFileToolStripMenuItem
            // 
            this.textFileToolStripMenuItem.Image = global::MScripter.Properties.Resources.documentText;
            this.textFileToolStripMenuItem.Name = "textFileToolStripMenuItem";
            this.textFileToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.textFileToolStripMenuItem.Text = "Text file";
            this.textFileToolStripMenuItem.Click += new System.EventHandler(this.textFileToolStripMenuItem_Click);
            // 
            // formatWorker
            // 
            this.formatWorker.WorkerReportsProgress = true;
            this.formatWorker.WorkerSupportsCancellation = true;
            this.formatWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.formatWorker_DoWork);
            this.formatWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.formatWorker_ProgressChanged);
            this.formatWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.formatWorker_RunWorkerCompleted);
            // 
            // errorBalloon
            // 
            this.errorBalloon.IsBalloon = true;
            this.errorBalloon.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
            this.errorBalloon.ToolTipTitle = "Something is wrong.";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(144, 6);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(144, 6);
            // 
            // mIRCContextMenu
            // 
            this.mIRCContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem,
            this.identifyToolStripMenuItem,
            this.openDataDirectoryToolStripMenuItem,
            this.openScriptDirectoryToolStripMenuItem});
            this.mIRCContextMenu.Name = "mIRCContextMenu";
            this.mIRCContextMenu.Size = new System.Drawing.Size(186, 92);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.findToolStripMenuItem.Text = "Find";
            // 
            // identifyToolStripMenuItem
            // 
            this.identifyToolStripMenuItem.Name = "identifyToolStripMenuItem";
            this.identifyToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.identifyToolStripMenuItem.Text = "Identify";
            this.identifyToolStripMenuItem.Click += new System.EventHandler(this.identifyToolStripMenuItem_Click);
            // 
            // openDataDirectoryToolStripMenuItem
            // 
            this.openDataDirectoryToolStripMenuItem.Name = "openDataDirectoryToolStripMenuItem";
            this.openDataDirectoryToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.openDataDirectoryToolStripMenuItem.Text = "Open data directory";
            this.openDataDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openDataDirectoryToolStripMenuItem_Click);
            // 
            // openScriptDirectoryToolStripMenuItem
            // 
            this.openScriptDirectoryToolStripMenuItem.Name = "openScriptDirectoryToolStripMenuItem";
            this.openScriptDirectoryToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.openScriptDirectoryToolStripMenuItem.Text = "Open script directory";
            this.openScriptDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openScriptDirectoryToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem1.Image")));
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(147, 22);
            this.toolStripMenuItem1.Text = "&Remote script";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.newRemoteScriptToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem2.Image")));
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(147, 22);
            this.toolStripMenuItem2.Text = "&Alias script";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.newAliasScriptToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem3.Image")));
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(147, 22);
            this.toolStripMenuItem3.Text = "&Popup script";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.newPopupScriptToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem4.Image")));
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(147, 22);
            this.toolStripMenuItem4.Text = "&Text file";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem5.Image")));
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(147, 22);
            this.toolStripMenuItem5.Text = "&INI file";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.newINIFileToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem6.Image")));
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(147, 22);
            this.toolStripMenuItem6.Text = "&Hashtable";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.newHashtableToolStripMenuItem_Click);
            // 
            // openDialog
            // 
            this.openDialog.Filter = "mIRC script files|*.mrc;*.als;*.ini|Hashtables (text)|*|Hashtables (binary)|*|Has" +
    "htables (INI)|*.ini|All files|*";
            this.openDialog.Multiselect = true;
            this.openDialog.SupportMultiDottedExtensions = true;
            // 
            // saveDialog
            // 
            this.saveDialog.Filter = "mIRC remote script files (*.mrc)|*.mrc|mIRC alias script files (*.als)|*.als|INI " +
    "files (*.ini)|*.ini|All files|*";
            this.saveDialog.SupportMultiDottedExtensions = true;
            // 
            // openDirectoryDialog
            // 
            this.openDirectoryDialog.CheckFileExists = false;
            this.openDirectoryDialog.Multiselect = true;
            this.openDirectoryDialog.ShowNewFolderButton = false;
            this.openDirectoryDialog.ShowReadOnly = true;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 546);
            this.Controls.Add(this.toolStripContainer1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuBar;
            this.Name = "MainForm";
            this.Text = "MScripter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MslTextBox)).EndInit();
            this.documentContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MslTextBox2)).EndInit();
            this.parsingProgressBarPanel.ResumeLayout(false);
            this.parsingProgressBarPanel.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.hashtableAddPanel.ResumeLayout(false);
            this.hashtableAddPanel.PerformLayout();
            this.hashtableAddExpiryPanel.ResumeLayout(false);
            this.hashtableAddExpiryPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hashtableAddExpiryTimeBox)).EndInit();
            this.hashtableHeaderPanel.ResumeLayout(false);
            this.hashtableHeaderPanel.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.errorListToolbar.ResumeLayout(false);
            this.errorListToolbar.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.consoleTextBox)).EndInit();
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.debugTextBox)).EndInit();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.newMenu.ResumeLayout(false);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.fileContextMenu.ResumeLayout(false);
            this.mIRCContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolbar;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton cutToolStripButton;
        private System.Windows.Forms.ToolStripButton copyToolStripButton;
        private System.Windows.Forms.ToolStripButton pasteToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton helpToolStripButton;
        private FolderSelect.VistaOpenFileDialog openDialog;
        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private FolderSelect.VistaSaveFileDialog saveDialog;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private FastColoredTextBox MslTextBox;
        private System.Windows.Forms.ImageList scriptTypeImageList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem restyleDocumentToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip fileContextMenu;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllButThisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToTheRightToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem formatAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remoteScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aliasScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem popupScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usersListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem variablesListToolStripMenuItem;
        private RefreshingComboBox functionList;
        private System.ComponentModel.BackgroundWorker formatWorker;
        private System.Windows.Forms.TableLayoutPanel parsingProgressBarPanel;
        private System.Windows.Forms.ProgressBar parsingProgressBar;
        private System.Windows.Forms.Label label1;
        private ConsoleTextBox consoleTextBox;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView hashtableListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.FlowLayoutPanel hashtableHeaderPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox hashtableBinaryViewButton;
        private System.Windows.Forms.ComboBox hashtableBaseComboBox;
        private System.Windows.Forms.TextBox hashtableAddTextBox;
        private System.Windows.Forms.TableLayoutPanel hashtableAddPanel;
        private System.Windows.Forms.ToolTip errorBalloon;
        private System.Windows.Forms.Button hashtableAddForwardButton;
        private System.Windows.Forms.Button hashtableAddBackButton;
        private System.Windows.Forms.TableLayoutPanel hashtableAddExpiryPanel;
        private System.Windows.Forms.ComboBox hashtableAddExpiryUnitBox;
        private System.Windows.Forms.NumericUpDown hashtableAddExpiryTimeBox;
        private System.Windows.Forms.CheckBox hashtableItemExpiryCheckBox;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noRunningMIRCInstancesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ContextMenuStrip newMenu;
        private System.Windows.Forms.ToolStripMenuItem newRemoteScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newAliasScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newPopupScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem newTextFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newINIFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem newHashtableToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripDropDownButton newToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TabPage tabPage3;
        private DialogDesigner dialogDesigner;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.ListView errorList;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ImageList errorImageList;
        private System.Windows.Forms.ToolStripMenuItem dialogDesignerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripMenuItem newDialogToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip mIRCContextMenu;
        private System.Windows.Forms.ToolStripMenuItem identifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDataDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openScriptDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton openDirectoryToolStripButton;
        private System.Windows.Forms.ToolStripButton refreshToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripButton undoToolStripButton;
        private System.Windows.Forms.ToolStripButton redoToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem restyleLineToolStripMenuItem;
        private System.Windows.Forms.ImageList treeViewImageList;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStrip errorListToolbar;
        private System.Windows.Forms.ToolStripButton errorsToolStripButton;
        private System.Windows.Forms.ToolStripButton warningsToolStripButton;
        private System.Windows.Forms.ToolStripButton noticesToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
        private System.Windows.Forms.ToolStripComboBox errorListScopeComboBox;
        private System.Windows.Forms.ToolStripMenuItem interfaceThemeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem standardThemeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem darkThemeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator20;
        private System.Windows.Forms.ToolStripMenuItem textFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iNIFileToolStripMenuItem;
        private FolderSelect.VistaFolderSelectDialog openDirectoryDialog;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator21;
        private System.Windows.Forms.ToolStripMenuItem foldingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem foldallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unfoldAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem foldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unfoldToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator22;
        private System.Windows.Forms.ContextMenuStrip documentContextMenu;
        private System.Windows.Forms.ToolStripMenuItem goTodefinitionToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer;
        private FastColoredTextBox MslTextBox2;
        private System.Windows.Forms.ToolStripMenuItem splitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator23;
        private System.Windows.Forms.TabPage tabPage6;
        private FastColoredTextBox debugTextBox;
    }
}

