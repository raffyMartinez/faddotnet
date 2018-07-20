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
	partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.menuDropDown = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panelSamplingButtons = new System.Windows.Forms.Panel();
            this.buttonMap = new System.Windows.Forms.Button();
            this.buttonCatch = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.lblErrorFormOpen = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.lblTitle = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripFileNewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripRecentlyOpened = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripRecentOpenedList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetReferenceNumbersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.referenceNumberRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coordinateFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.symbolFontsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateGridMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uTMZone50ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uTMZone51ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showErrorMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onlineManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusPanelDBPath = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusPanelTargetArea = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusPanelLandingSite = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusPanelGearUsed = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelSamplingButtons.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelSamplingButtons);
            this.splitContainer1.Panel2.Controls.Add(this.lblErrorFormOpen);
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Panel2.Controls.Add(this.lblTitle);
            this.splitContainer1.Size = new System.Drawing.Size(929, 451);
            this.splitContainer1.SplitterDistance = 240;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.ContextMenuStrip = this.menuDropDown;
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.RightToLeftLayout = true;
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(234, 445);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1AfterExpand);
            this.treeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeView1BeforeSelect);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1AfterSelect);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            // 
            // menuDropDown
            // 
            this.menuDropDown.Name = "menuDropDown";
            this.menuDropDown.Size = new System.Drawing.Size(61, 4);
            this.menuDropDown.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuDropDown_ItemClicked);
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
            this.buttonMap.Click += new System.EventHandler(this.buttonSamplingClick);
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
            this.buttonCatch.Click += new System.EventHandler(this.buttonSamplingClick);
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
            this.buttonOK.Click += new System.EventHandler(this.buttonSamplingClick);
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
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.ContextMenuStrip = this.menuDropDown;
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.Location = new System.Drawing.Point(-1, 28);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(683, 415);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(3, 3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(481, 22);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "label1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(929, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripFileNewMenuItem,
            this.toolStripMenuItem2,
            this.toolStripRecentlyOpened,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.fileToolStripMenuItem_DropDownItemClicked);
            // 
            // toolStripFileNewMenuItem
            // 
            this.toolStripFileNewMenuItem.Image = global::FAD3.Properties.Resources.VSO_NewFile_16x;
            this.toolStripFileNewMenuItem.Name = "toolStripFileNewMenuItem";
            this.toolStripFileNewMenuItem.Size = new System.Drawing.Size(162, 22);
            this.toolStripFileNewMenuItem.Tag = "new";
            this.toolStripFileNewMenuItem.Text = "New ...";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Image = global::FAD3.Properties.Resources.OpenFileFromProject_16x;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(162, 22);
            this.toolStripMenuItem2.Tag = "open";
            this.toolStripMenuItem2.Text = "Open ...";
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
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetReferenceNumbersToolStripMenuItem,
            this.referenceNumberRangeToolStripMenuItem,
            this.coordinateFormatToolStripMenuItem,
            this.symbolFontsToolStripMenuItem,
            this.generateGridMapToolStripMenuItem,
            this.showErrorMessagesToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            this.toolsToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolsToolStripMenuItem_DropDownItemClicked);
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
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.onlineManualToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.helpToolStripMenuItem_DropDownItemClicked);
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
            this.statusPanelDBPath.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
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
            this.statusPanelTargetArea.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
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
            this.statusPanelLandingSite.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
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
            this.statusPanelGearUsed.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.statusPanelGearUsed.Name = "statusPanelGearUsed";
            this.statusPanelGearUsed.Size = new System.Drawing.Size(4, 21);
            this.statusPanelGearUsed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripButton6});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(929, 31);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton1.Tag = "about";
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "About the software";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::FAD3.Properties.Resources.imHook;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton2.Tag = "gear";
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.ToolTipText = "Fishing gears";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::FAD3.Properties.Resources.fish2;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton3.Tag = "fish";
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.ToolTipText = "Species caught";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::FAD3.Properties.Resources.system_file_manager;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton4.Tag = "report";
            this.toolStripButton4.Text = "Report";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::FAD3.Properties.Resources.internet_web_browser;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton5.Tag = "map";
            this.toolStripButton5.Text = "toolStripButton5";
            this.toolStripButton5.ToolTipText = "Map";
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::FAD3.Properties.Resources.im_exit;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton6.Tag = "exit";
            this.toolStripButton6.Text = "toolStripButton6";
            this.toolStripButton6.ToolTipText = "Exit";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 530);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Fisheries Assessment Database";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.Load += new System.EventHandler(this.FrmMainLoad);
            this.ResizeEnd += new System.EventHandler(this.frmMain_ResizeEnd);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyUp);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelSamplingButtons.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ToolStripStatusLabel statusPanelGearUsed;
		private System.Windows.Forms.ToolStripStatusLabel statusPanelLandingSite;
		private System.Windows.Forms.ToolStripStatusLabel statusPanelTargetArea;
		private System.Windows.Forms.ToolStripStatusLabel statusPanelDBPath;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem toolStripFileNewMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem toolStripRecentlyOpened;
        private System.Windows.Forms.ToolStripMenuItem testToolStripRecentOpenedList;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetReferenceNumbersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem referenceNumberRangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coordinateFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem symbolFontsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateGridMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uTMZone50ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uTMZone51ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onlineManualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showErrorMessagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.Label lblErrorFormOpen;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelSamplingButtons;
        private System.Windows.Forms.Button buttonMap;
        private System.Windows.Forms.Button buttonCatch;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ContextMenuStrip menuDropDown;
    }
}
