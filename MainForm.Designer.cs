/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/7/2016
 * Time: 1:53 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace FAD3
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeMain = new System.Windows.Forms.TreeView();
            this.menuDropDown = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.labelErrorDetail = new System.Windows.Forms.Label();
            this.lblErrorFormOpen = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelSamplingButtons = new System.Windows.Forms.Panel();
            this.buttonMap = new System.Windows.Forms.Button();
            this.buttonCatch = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.lvMain = new System.Windows.Forms.ListView();
            this.menuMenuBar = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripFileNewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripRecentlyOpened = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripRecentOpenedList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.resetReferenceNumbersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.referenceNumberRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coordinateFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.symbolFontsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateGridMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uTMZone50ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uTMZone51ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showErrorMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onlineManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusPanelDBPath = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusPanelTargetArea = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusPanelLandingSite = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusPanelGearUsed = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tsToolbar = new System.Windows.Forms.ToolStrip();
            this.tbButtonAbout = new System.Windows.Forms.ToolStripButton();
            this.tbButtonGears = new System.Windows.Forms.ToolStripButton();
            this.tbButtonSpecies = new System.Windows.Forms.ToolStripButton();
            this.tbButtonReport = new System.Windows.Forms.ToolStripButton();
            this.tbButtonMap = new System.Windows.Forms.ToolStripButton();
            this.tbButtonExit = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelSamplingButtons.SuspendLayout();
            this.menuMenuBar.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tsToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 58);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.labelErrorDetail);
            this.splitContainer1.Panel2.Controls.Add(this.lblErrorFormOpen);
            this.splitContainer1.Panel2.Controls.Add(this.lblTitle);
            this.splitContainer1.Panel2.Controls.Add(this.panelSamplingButtons);
            this.splitContainer1.Panel2.Controls.Add(this.lvMain);
            this.splitContainer1.Size = new System.Drawing.Size(929, 451);
            this.splitContainer1.SplitterDistance = 240;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeMain
            // 
            this.treeMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeMain.ContextMenuStrip = this.menuDropDown;
            this.treeMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeMain.ImageIndex = 0;
            this.treeMain.ImageList = this.imageList1;
            this.treeMain.Location = new System.Drawing.Point(3, 3);
            this.treeMain.Name = "treeMain";
            this.treeMain.RightToLeftLayout = true;
            this.treeMain.SelectedImageIndex = 0;
            this.treeMain.Size = new System.Drawing.Size(234, 445);
            this.treeMain.TabIndex = 0;
            this.treeMain.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.OntreeMainAfterExpand);
            this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeMainAfterSelect);
            this.treeMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OntreeMain_MouseDown);
            // 
            // menuDropDown
            // 
            this.menuDropDown.Name = "menuDropDown";
            this.menuDropDown.Size = new System.Drawing.Size(61, 4);
            this.menuDropDown.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnMenuDropDown_ItemClicked);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "AOI");
            this.imageList1.Images.SetKeyName(1, "LandingSite");
            this.imageList1.Images.SetKeyName(2, "MonthGear");
            this.imageList1.Images.SetKeyName(3, "jigs");
            this.imageList1.Images.SetKeyName(4, "others");
            this.imageList1.Images.SetKeyName(5, "lines");
            this.imageList1.Images.SetKeyName(6, "nets");
            this.imageList1.Images.SetKeyName(7, "impound");
            this.imageList1.Images.SetKeyName(8, "seines");
            this.imageList1.Images.SetKeyName(9, "traps");
            this.imageList1.Images.SetKeyName(10, "db");
            // 
            // labelErrorDetail
            // 
            this.labelErrorDetail.BackColor = System.Drawing.SystemColors.Window;
            this.labelErrorDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelErrorDetail.Location = new System.Drawing.Point(249, 271);
            this.labelErrorDetail.Name = "labelErrorDetail";
            this.labelErrorDetail.Size = new System.Drawing.Size(284, 108);
            this.labelErrorDetail.TabIndex = 6;
            this.labelErrorDetail.Text = "label1";
            this.labelErrorDetail.Visible = false;
            // 
            // lblErrorFormOpen
            // 
            this.lblErrorFormOpen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblErrorFormOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorFormOpen.Location = new System.Drawing.Point(-1, 179);
            this.lblErrorFormOpen.Name = "lblErrorFormOpen";
            this.lblErrorFormOpen.Size = new System.Drawing.Size(683, 92);
            this.lblErrorFormOpen.TabIndex = 4;
            this.lblErrorFormOpen.Text = "label1";
            this.lblErrorFormOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblErrorFormOpen.Visible = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(3, 3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(481, 22);
            this.lblTitle.TabIndex = 2;
            // 
            // panelSamplingButtons
            // 
            this.panelSamplingButtons.BackColor = System.Drawing.SystemColors.Window;
            this.panelSamplingButtons.Controls.Add(this.buttonMap);
            this.panelSamplingButtons.Controls.Add(this.buttonCatch);
            this.panelSamplingButtons.Controls.Add(this.buttonOK);
            this.panelSamplingButtons.Location = new System.Drawing.Point(575, 28);
            this.panelSamplingButtons.Name = "panelSamplingButtons";
            this.panelSamplingButtons.Size = new System.Drawing.Size(107, 133);
            this.panelSamplingButtons.TabIndex = 5;
            this.panelSamplingButtons.Visible = false;
            // 
            // buttonMap
            // 
            this.buttonMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMap.Location = new System.Drawing.Point(20, 88);
            this.buttonMap.Name = "buttonMap";
            this.buttonMap.Size = new System.Drawing.Size(69, 30);
            this.buttonMap.TabIndex = 2;
            this.buttonMap.Text = "Map";
            this.buttonMap.UseVisualStyleBackColor = true;
            this.buttonMap.Click += new System.EventHandler(this.OnbuttonSamplingClick);
            // 
            // buttonCatch
            // 
            this.buttonCatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCatch.Location = new System.Drawing.Point(20, 52);
            this.buttonCatch.Name = "buttonCatch";
            this.buttonCatch.Size = new System.Drawing.Size(69, 30);
            this.buttonCatch.TabIndex = 1;
            this.buttonCatch.Text = "Catch";
            this.buttonCatch.UseVisualStyleBackColor = true;
            this.buttonCatch.Click += new System.EventHandler(this.OnbuttonSamplingClick);
            // 
            // buttonOK
            // 
            this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.Location = new System.Drawing.Point(20, 16);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(69, 30);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.OnbuttonSamplingClick);
            // 
            // lvMain
            // 
            this.lvMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMain.ContextMenuStrip = this.menuDropDown;
            this.lvMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvMain.Location = new System.Drawing.Point(-1, 28);
            this.lvMain.MultiSelect = false;
            this.lvMain.Name = "lvMain";
            this.lvMain.Size = new System.Drawing.Size(683, 415);
            this.lvMain.TabIndex = 3;
            this.lvMain.UseCompatibleStateImageBehavior = false;
            this.lvMain.View = System.Windows.Forms.View.Details;
            this.lvMain.DoubleClick += new System.EventHandler(this.OnListView_DoubleClick);
            this.lvMain.Leave += new System.EventHandler(this.OnListViewLeave);
            this.lvMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListView_MouseDown);
            // 
            // menuMenuBar
            // 
            this.menuMenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuTools,
            this.menuHelp});
            this.menuMenuBar.Location = new System.Drawing.Point(0, 0);
            this.menuMenuBar.Name = "menuMenuBar";
            this.menuMenuBar.Size = new System.Drawing.Size(929, 24);
            this.menuMenuBar.TabIndex = 1;
            this.menuMenuBar.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripFileNewMenuItem,
            this.toolStripFileOpen,
            this.toolStripRecentlyOpened,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "File";
            this.menuFile.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnMenuFile_DropDownItemClicked);
            // 
            // toolStripFileNewMenuItem
            // 
            this.toolStripFileNewMenuItem.Image = global::FAD3.Properties.Resources.VSO_NewFile_16x;
            this.toolStripFileNewMenuItem.Name = "toolStripFileNewMenuItem";
            this.toolStripFileNewMenuItem.Size = new System.Drawing.Size(162, 22);
            this.toolStripFileNewMenuItem.Tag = "new";
            this.toolStripFileNewMenuItem.Text = "New ...";
            // 
            // toolStripFileOpen
            // 
            this.toolStripFileOpen.Image = global::FAD3.Properties.Resources.OpenFileFromProject_16x;
            this.toolStripFileOpen.Name = "toolStripFileOpen";
            this.toolStripFileOpen.Size = new System.Drawing.Size(162, 22);
            this.toolStripFileOpen.Tag = "open";
            this.toolStripFileOpen.Text = "Open ...";
            // 
            // toolStripRecentlyOpened
            // 
            this.toolStripRecentlyOpened.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripRecentOpenedList});
            this.toolStripRecentlyOpened.Image = global::FAD3.Properties.Resources.History_16x;
            this.toolStripRecentlyOpened.Name = "toolStripRecentlyOpened";
            this.toolStripRecentlyOpened.Size = new System.Drawing.Size(162, 22);
            this.toolStripRecentlyOpened.Text = "Recently opened";
            // 
            // testToolStripRecentOpenedList
            // 
            this.testToolStripRecentOpenedList.Name = "testToolStripRecentOpenedList";
            this.testToolStripRecentOpenedList.Size = new System.Drawing.Size(93, 22);
            this.testToolStripRecentOpenedList.Text = "test";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::FAD3.Properties.Resources.Close_16x;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.exitToolStripMenuItem.Tag = "exit";
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // menuTools
            // 
            this.menuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetReferenceNumbersToolStripMenuItem,
            this.referenceNumberRangeToolStripMenuItem,
            this.coordinateFormatToolStripMenuItem,
            this.symbolFontsToolStripMenuItem,
            this.generateGridMapToolStripMenuItem,
            this.showErrorMessagesToolStripMenuItem});
            this.menuTools.Name = "menuTools";
            this.menuTools.Size = new System.Drawing.Size(48, 20);
            this.menuTools.Text = "Tools";
            this.menuTools.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnMenuTools_DropDownItemClicked);
            // 
            // resetReferenceNumbersToolStripMenuItem
            // 
            this.resetReferenceNumbersToolStripMenuItem.Name = "resetReferenceNumbersToolStripMenuItem";
            this.resetReferenceNumbersToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.resetReferenceNumbersToolStripMenuItem.Tag = "resetRefNos";
            this.resetReferenceNumbersToolStripMenuItem.Text = "Reset reference numbers";
            // 
            // referenceNumberRangeToolStripMenuItem
            // 
            this.referenceNumberRangeToolStripMenuItem.Name = "referenceNumberRangeToolStripMenuItem";
            this.referenceNumberRangeToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.referenceNumberRangeToolStripMenuItem.Tag = "refNoRange";
            this.referenceNumberRangeToolStripMenuItem.Text = "Reference number range";
            // 
            // coordinateFormatToolStripMenuItem
            // 
            this.coordinateFormatToolStripMenuItem.Name = "coordinateFormatToolStripMenuItem";
            this.coordinateFormatToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.coordinateFormatToolStripMenuItem.Tag = "coordFormat";
            this.coordinateFormatToolStripMenuItem.Text = "Coordinate format";
            // 
            // symbolFontsToolStripMenuItem
            // 
            this.symbolFontsToolStripMenuItem.Name = "symbolFontsToolStripMenuItem";
            this.symbolFontsToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.symbolFontsToolStripMenuItem.Tag = "symbolFonts";
            this.symbolFontsToolStripMenuItem.Text = "Symbol fonts";
            // 
            // generateGridMapToolStripMenuItem
            // 
            this.generateGridMapToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uTMZone50ToolStripMenuItem,
            this.uTMZone51ToolStripMenuItem});
            this.generateGridMapToolStripMenuItem.Name = "generateGridMapToolStripMenuItem";
            this.generateGridMapToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.generateGridMapToolStripMenuItem.Text = "Generate grid map";
            this.generateGridMapToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.generateGridMapToolStripMenuItem_DropDownItemClicked);
            // 
            // uTMZone50ToolStripMenuItem
            // 
            this.uTMZone50ToolStripMenuItem.Name = "uTMZone50ToolStripMenuItem";
            this.uTMZone50ToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.uTMZone50ToolStripMenuItem.Tag = "zone50";
            this.uTMZone50ToolStripMenuItem.Text = "UTM zone 50";
            // 
            // uTMZone51ToolStripMenuItem
            // 
            this.uTMZone51ToolStripMenuItem.Name = "uTMZone51ToolStripMenuItem";
            this.uTMZone51ToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.uTMZone51ToolStripMenuItem.Tag = "zone51";
            this.uTMZone51ToolStripMenuItem.Text = "UTM zone 51";
            // 
            // showErrorMessagesToolStripMenuItem
            // 
            this.showErrorMessagesToolStripMenuItem.Name = "showErrorMessagesToolStripMenuItem";
            this.showErrorMessagesToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.showErrorMessagesToolStripMenuItem.Tag = "showError";
            this.showErrorMessagesToolStripMenuItem.Text = "Show error messages";
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.onlineManualToolStripMenuItem});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "Help";
            this.menuHelp.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnMenuHelp_DropDownItemClicked);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Tag = "about";
            this.aboutToolStripMenuItem.Text = "About...";
            // 
            // onlineManualToolStripMenuItem
            // 
            this.onlineManualToolStripMenuItem.Name = "onlineManualToolStripMenuItem";
            this.onlineManualToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.onlineManualToolStripMenuItem.Tag = "onlineManual";
            this.onlineManualToolStripMenuItem.Text = "Online manual";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusPanelDBPath,
            this.statusPanelTargetArea,
            this.statusPanelLandingSite,
            this.statusPanelGearUsed});
            this.statusStrip1.Location = new System.Drawing.Point(0, 504);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(929, 26);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusPanelDBPath
            // 
            this.statusPanelDBPath.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.statusPanelDBPath.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusPanelDBPath.DoubleClickEnabled = true;
            this.statusPanelDBPath.Name = "statusPanelDBPath";
            this.statusPanelDBPath.Size = new System.Drawing.Size(53, 21);
            this.statusPanelDBPath.Text = "DBPath";
            this.statusPanelDBPath.DoubleClick += new System.EventHandler(this.statusPanelDBPath_DoubleClick);
            // 
            // statusPanelTargetArea
            // 
            this.statusPanelTargetArea.AutoSize = false;
            this.statusPanelTargetArea.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.statusPanelTargetArea.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusPanelTargetArea.Name = "statusPanelTargetArea";
            this.statusPanelTargetArea.Size = new System.Drawing.Size(80, 21);
            this.statusPanelTargetArea.Text = "Target area";
            this.statusPanelTargetArea.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusPanelLandingSite
            // 
            this.statusPanelLandingSite.AutoSize = false;
            this.statusPanelLandingSite.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.statusPanelLandingSite.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusPanelLandingSite.Name = "statusPanelLandingSite";
            this.statusPanelLandingSite.Size = new System.Drawing.Size(4, 21);
            this.statusPanelLandingSite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusPanelGearUsed
            // 
            this.statusPanelGearUsed.AutoSize = false;
            this.statusPanelGearUsed.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.statusPanelGearUsed.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.statusPanelGearUsed.Name = "statusPanelGearUsed";
            this.statusPanelGearUsed.Size = new System.Drawing.Size(4, 21);
            this.statusPanelGearUsed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tsToolbar
            // 
            this.tsToolbar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbButtonAbout,
            this.tbButtonGears,
            this.tbButtonSpecies,
            this.tbButtonReport,
            this.tbButtonMap,
            this.tbButtonExit});
            this.tsToolbar.Location = new System.Drawing.Point(0, 24);
            this.tsToolbar.Name = "tsToolbar";
            this.tsToolbar.Size = new System.Drawing.Size(929, 31);
            this.tsToolbar.TabIndex = 3;
            this.tsToolbar.Text = "tsToolbar";
            this.tsToolbar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolbar_ItemClicked);
            // 
            // tbButtonAbout
            // 
            this.tbButtonAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbButtonAbout.Image = ((System.Drawing.Image)(resources.GetObject("tbButtonAbout.Image")));
            this.tbButtonAbout.ImageTransparentColor = System.Drawing.Color.White;
            this.tbButtonAbout.Name = "tbButtonAbout";
            this.tbButtonAbout.Size = new System.Drawing.Size(28, 28);
            this.tbButtonAbout.Tag = "about";
            this.tbButtonAbout.Text = "toolStripButton1";
            this.tbButtonAbout.ToolTipText = "About the software";
            // 
            // tbButtonGears
            // 
            this.tbButtonGears.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbButtonGears.Image = global::FAD3.Properties.Resources.imHook;
            this.tbButtonGears.ImageTransparentColor = System.Drawing.Color.White;
            this.tbButtonGears.Name = "tbButtonGears";
            this.tbButtonGears.Size = new System.Drawing.Size(28, 28);
            this.tbButtonGears.Tag = "gear";
            this.tbButtonGears.Text = "toolStripButton2";
            this.tbButtonGears.ToolTipText = "Fishing gears";
            // 
            // tbButtonSpecies
            // 
            this.tbButtonSpecies.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbButtonSpecies.Image = global::FAD3.Properties.Resources.fish2;
            this.tbButtonSpecies.ImageTransparentColor = System.Drawing.Color.White;
            this.tbButtonSpecies.Name = "tbButtonSpecies";
            this.tbButtonSpecies.Size = new System.Drawing.Size(28, 28);
            this.tbButtonSpecies.Tag = "fish";
            this.tbButtonSpecies.Text = "toolStripButton3";
            this.tbButtonSpecies.ToolTipText = "Species caught";
            // 
            // tbButtonReport
            // 
            this.tbButtonReport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbButtonReport.Image = global::FAD3.Properties.Resources.system_file_manager;
            this.tbButtonReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbButtonReport.Name = "tbButtonReport";
            this.tbButtonReport.Size = new System.Drawing.Size(28, 28);
            this.tbButtonReport.Tag = "report";
            this.tbButtonReport.Text = "Report";
            // 
            // tbButtonMap
            // 
            this.tbButtonMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbButtonMap.Image = global::FAD3.Properties.Resources.internet_web_browser;
            this.tbButtonMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbButtonMap.Name = "tbButtonMap";
            this.tbButtonMap.Size = new System.Drawing.Size(28, 28);
            this.tbButtonMap.Tag = "map";
            this.tbButtonMap.Text = "toolStripButton5";
            this.tbButtonMap.ToolTipText = "Map";
            // 
            // tbButtonExit
            // 
            this.tbButtonExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbButtonExit.Image = global::FAD3.Properties.Resources.im_exit;
            this.tbButtonExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbButtonExit.Name = "tbButtonExit";
            this.tbButtonExit.Size = new System.Drawing.Size(28, 28);
            this.tbButtonExit.Tag = "exit";
            this.tbButtonExit.Text = "toolStripButton6";
            this.tbButtonExit.ToolTipText = "Exit";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 530);
            this.Controls.Add(this.tsToolbar);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuMenuBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuMenuBar;
            this.Name = "frmMain";
            this.Text = "Fisheries Assessment Database";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.Load += new System.EventHandler(this.FrmMainLoad);
            this.ResizeEnd += new System.EventHandler(this.frmMain_ResizeEnd);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelSamplingButtons.ResumeLayout(false);
            this.menuMenuBar.ResumeLayout(false);
            this.menuMenuBar.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tsToolbar.ResumeLayout(false);
            this.tsToolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ToolStripStatusLabel statusPanelGearUsed;
		private System.Windows.Forms.ToolStripStatusLabel statusPanelLandingSite;
		private System.Windows.Forms.ToolStripStatusLabel statusPanelTargetArea;
		private System.Windows.Forms.ToolStripStatusLabel statusPanelDBPath;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ToolStripMenuItem toolStripFileOpen;
		private System.Windows.Forms.TreeView treeMain;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem toolStripFileNewMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem menuFile;
		private System.Windows.Forms.MenuStrip menuMenuBar;
		private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem toolStripRecentlyOpened;
        private System.Windows.Forms.ToolStripMenuItem testToolStripRecentOpenedList;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStrip tsToolbar;
        private System.Windows.Forms.ToolStripButton tbButtonAbout;
        private System.Windows.Forms.ToolStripButton tbButtonGears;
        private System.Windows.Forms.ToolStripButton tbButtonSpecies;
        private System.Windows.Forms.ToolStripButton tbButtonReport;
        private System.Windows.Forms.ToolStripMenuItem menuTools;
        private System.Windows.Forms.ToolStripMenuItem resetReferenceNumbersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem referenceNumberRangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coordinateFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem symbolFontsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateGridMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uTMZone50ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uTMZone51ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onlineManualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showErrorMessagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tbButtonMap;
        private System.Windows.Forms.ToolStripButton tbButtonExit;
        private System.Windows.Forms.Label lblErrorFormOpen;
        private System.Windows.Forms.ListView lvMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelSamplingButtons;
        private System.Windows.Forms.Button buttonMap;
        private System.Windows.Forms.Button buttonCatch;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ContextMenuStrip menuDropDown;
        private System.Windows.Forms.Label labelErrorDetail;
    }
}
