namespace FAD3.Mapping.Forms
{
    partial class Grid25LayoutHelperForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Grid25LayoutHelperForm));
            this.txtRows = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtColumns = new System.Windows.Forms.TextBox();
            this.txtOverlap = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnApplyDimension = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtPageHeight = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPageWidth = new System.Windows.Forms.TextBox();
            this.tabsLayout = new System.Windows.Forms.TabControl();
            this.tabLayout = new System.Windows.Forms.TabPage();
            this.btnInputTitles = new System.Windows.Forms.Button();
            this.tabFishingGround = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.textFishingGround = new System.Windows.Forms.TextBox();
            this.tabResults = new System.Windows.Forms.TabPage();
            this.btnGridSettings = new System.Windows.Forms.Button();
            this.btnLocateSourceFolder = new System.Windows.Forms.Button();
            this.lblProvideTitles = new System.Windows.Forms.Label();
            this.buttonSubGrid = new System.Windows.Forms.Button();
            this.lvResults = new System.Windows.Forms.ListView();
            this.menuDropDown = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabSave = new System.Windows.Forms.TabPage();
            this.groupSaveTemplate = new System.Windows.Forms.GroupBox();
            this.btnSaveTemplate = new System.Windows.Forms.Button();
            this.textLayoutTemplateFileName = new System.Windows.Forms.TextBox();
            this.groupSaveGrids = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.textFolderToSave = new System.Windows.Forms.TextBox();
            this.btnSelectFolderSave = new System.Windows.Forms.Button();
            this.chkAutoExpand = new System.Windows.Forms.CheckBox();
            this.tabExport = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.txtFolderExportPath = new System.Windows.Forms.TextBox();
            this.btnFolderExportImage = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.txtDPI = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.listLayers = new System.Windows.Forms.CheckedListBox();
            this.tabsLayout.SuspendLayout();
            this.tabLayout.SuspendLayout();
            this.tabFishingGround.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.tabSave.SuspendLayout();
            this.groupSaveTemplate.SuspendLayout();
            this.groupSaveGrids.SuspendLayout();
            this.tabExport.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtRows
            // 
            this.txtRows.Location = new System.Drawing.Point(93, 69);
            this.txtRows.Name = "txtRows";
            this.txtRows.Size = new System.Drawing.Size(72, 20);
            this.txtRows.TabIndex = 0;
            this.txtRows.Text = "1";
            this.txtRows.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextValidating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Rows";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Columns";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtColumns
            // 
            this.txtColumns.Location = new System.Drawing.Point(93, 95);
            this.txtColumns.Name = "txtColumns";
            this.txtColumns.Size = new System.Drawing.Size(72, 20);
            this.txtColumns.TabIndex = 3;
            this.txtColumns.Text = "1";
            this.txtColumns.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextValidating);
            // 
            // txtOverlap
            // 
            this.txtOverlap.Location = new System.Drawing.Point(93, 121);
            this.txtOverlap.Name = "txtOverlap";
            this.txtOverlap.Size = new System.Drawing.Size(72, 20);
            this.txtOverlap.TabIndex = 4;
            this.txtOverlap.Text = "0";
            this.txtOverlap.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextValidating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Overlap";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // btnApplyDimension
            // 
            this.btnApplyDimension.ImageKey = "applyGrid";
            this.btnApplyDimension.ImageList = this.imageList1;
            this.btnApplyDimension.Location = new System.Drawing.Point(329, 210);
            this.btnApplyDimension.Name = "btnApplyDimension";
            this.btnApplyDimension.Size = new System.Drawing.Size(28, 28);
            this.btnApplyDimension.TabIndex = 6;
            this.btnApplyDimension.UseVisualStyleBackColor = true;
            this.btnApplyDimension.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            this.imageList1.Images.SetKeyName(0, "applyGrid");
            this.imageList1.Images.SetKeyName(1, "cancel");
            this.imageList1.Images.SetKeyName(2, "save");
            this.imageList1.Images.SetKeyName(3, "addToFolder");
            this.imageList1.Images.SetKeyName(4, "addLayout");
            this.imageList1.Images.SetKeyName(5, "openLayoutGrid");
            this.imageList1.Images.SetKeyName(6, "saveLayout");
            this.imageList1.Images.SetKeyName(7, "layoutTitles.bmp");
            this.imageList1.Images.SetKeyName(8, "subgrid");
            this.imageList1.Images.SetKeyName(9, "grid_layout");
            this.imageList1.Images.SetKeyName(10, "Folder_16x");
            this.imageList1.Images.SetKeyName(11, "settings");
            this.imageList1.Images.SetKeyName(12, "image");
            // 
            // btnCancel
            // 
            this.btnCancel.ImageKey = "cancel";
            this.btnCancel.ImageList = this.imageList1;
            this.btnCancel.Location = new System.Drawing.Point(343, 305);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(28, 28);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // txtPageHeight
            // 
            this.txtPageHeight.Location = new System.Drawing.Point(93, 43);
            this.txtPageHeight.Name = "txtPageHeight";
            this.txtPageHeight.Size = new System.Drawing.Size(72, 20);
            this.txtPageHeight.TabIndex = 12;
            this.txtPageHeight.Text = "1";
            this.txtPageHeight.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextValidating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Page height";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Page width";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtPageWidth
            // 
            this.txtPageWidth.Location = new System.Drawing.Point(93, 17);
            this.txtPageWidth.Name = "txtPageWidth";
            this.txtPageWidth.Size = new System.Drawing.Size(72, 20);
            this.txtPageWidth.TabIndex = 9;
            this.txtPageWidth.Text = "1";
            this.txtPageWidth.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextValidating);
            // 
            // tabsLayout
            // 
            this.tabsLayout.Controls.Add(this.tabLayout);
            this.tabsLayout.Controls.Add(this.tabFishingGround);
            this.tabsLayout.Controls.Add(this.tabResults);
            this.tabsLayout.Controls.Add(this.tabSave);
            this.tabsLayout.Controls.Add(this.tabExport);
            this.tabsLayout.Location = new System.Drawing.Point(-2, 23);
            this.tabsLayout.Name = "tabsLayout";
            this.tabsLayout.SelectedIndex = 0;
            this.tabsLayout.Size = new System.Drawing.Size(380, 275);
            this.tabsLayout.TabIndex = 13;
            this.tabsLayout.SelectedIndexChanged += new System.EventHandler(this.OnTabsSelectionChanged);
            // 
            // tabLayout
            // 
            this.tabLayout.Controls.Add(this.btnInputTitles);
            this.tabLayout.Controls.Add(this.txtPageWidth);
            this.tabLayout.Controls.Add(this.label2);
            this.tabLayout.Controls.Add(this.btnApplyDimension);
            this.tabLayout.Controls.Add(this.txtPageHeight);
            this.tabLayout.Controls.Add(this.txtColumns);
            this.tabLayout.Controls.Add(this.label5);
            this.tabLayout.Controls.Add(this.txtRows);
            this.tabLayout.Controls.Add(this.txtOverlap);
            this.tabLayout.Controls.Add(this.label1);
            this.tabLayout.Controls.Add(this.label3);
            this.tabLayout.Controls.Add(this.label4);
            this.tabLayout.Location = new System.Drawing.Point(4, 22);
            this.tabLayout.Name = "tabLayout";
            this.tabLayout.Padding = new System.Windows.Forms.Padding(3);
            this.tabLayout.Size = new System.Drawing.Size(372, 249);
            this.tabLayout.TabIndex = 0;
            this.tabLayout.Text = "Layout";
            this.tabLayout.UseVisualStyleBackColor = true;
            // 
            // btnInputTitles
            // 
            this.btnInputTitles.ImageKey = "layoutTitles.bmp";
            this.btnInputTitles.ImageList = this.imageList1;
            this.btnInputTitles.Location = new System.Drawing.Point(295, 210);
            this.btnInputTitles.Name = "btnInputTitles";
            this.btnInputTitles.Size = new System.Drawing.Size(28, 28);
            this.btnInputTitles.TabIndex = 13;
            this.btnInputTitles.UseVisualStyleBackColor = true;
            this.btnInputTitles.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // tabFishingGround
            // 
            this.tabFishingGround.Controls.Add(this.label7);
            this.tabFishingGround.Controls.Add(this.textFishingGround);
            this.tabFishingGround.Location = new System.Drawing.Point(4, 22);
            this.tabFishingGround.Name = "tabFishingGround";
            this.tabFishingGround.Padding = new System.Windows.Forms.Padding(3);
            this.tabFishingGround.Size = new System.Drawing.Size(372, 249);
            this.tabFishingGround.TabIndex = 1;
            this.tabFishingGround.Text = "Fishing ground";
            this.tabFishingGround.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Fishing ground";
            // 
            // textFishingGround
            // 
            this.textFishingGround.Location = new System.Drawing.Point(24, 84);
            this.textFishingGround.Name = "textFishingGround";
            this.textFishingGround.Size = new System.Drawing.Size(330, 20);
            this.textFishingGround.TabIndex = 10;
            this.textFishingGround.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextValidating);
            // 
            // tabResults
            // 
            this.tabResults.Controls.Add(this.btnGridSettings);
            this.tabResults.Controls.Add(this.btnLocateSourceFolder);
            this.tabResults.Controls.Add(this.lblProvideTitles);
            this.tabResults.Controls.Add(this.buttonSubGrid);
            this.tabResults.Controls.Add(this.lvResults);
            this.tabResults.Location = new System.Drawing.Point(4, 22);
            this.tabResults.Name = "tabResults";
            this.tabResults.Size = new System.Drawing.Size(372, 249);
            this.tabResults.TabIndex = 2;
            this.tabResults.Text = "Results";
            this.tabResults.UseVisualStyleBackColor = true;
            // 
            // btnGridSettings
            // 
            this.btnGridSettings.ImageKey = "settings";
            this.btnGridSettings.ImageList = this.imageList1;
            this.btnGridSettings.Location = new System.Drawing.Point(330, 84);
            this.btnGridSettings.Name = "btnGridSettings";
            this.btnGridSettings.Size = new System.Drawing.Size(28, 28);
            this.btnGridSettings.TabIndex = 15;
            this.btnGridSettings.UseVisualStyleBackColor = true;
            this.btnGridSettings.Visible = false;
            this.btnGridSettings.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnLocateSourceFolder
            // 
            this.btnLocateSourceFolder.ImageKey = "Folder_16x";
            this.btnLocateSourceFolder.ImageList = this.imageList1;
            this.btnLocateSourceFolder.Location = new System.Drawing.Point(330, 50);
            this.btnLocateSourceFolder.Name = "btnLocateSourceFolder";
            this.btnLocateSourceFolder.Size = new System.Drawing.Size(28, 28);
            this.btnLocateSourceFolder.TabIndex = 14;
            this.btnLocateSourceFolder.UseVisualStyleBackColor = true;
            this.btnLocateSourceFolder.Visible = false;
            this.btnLocateSourceFolder.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblProvideTitles
            // 
            this.lblProvideTitles.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProvideTitles.Location = new System.Drawing.Point(24, 101);
            this.lblProvideTitles.Name = "lblProvideTitles";
            this.lblProvideTitles.Size = new System.Drawing.Size(324, 51);
            this.lblProvideTitles.TabIndex = 13;
            this.lblProvideTitles.Text = "Provide titles for all panels in the layout and name of fishing ground";
            this.lblProvideTitles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSubGrid
            // 
            this.buttonSubGrid.ImageKey = "subgrid";
            this.buttonSubGrid.ImageList = this.imageList1;
            this.buttonSubGrid.Location = new System.Drawing.Point(330, 16);
            this.buttonSubGrid.Name = "buttonSubGrid";
            this.buttonSubGrid.Size = new System.Drawing.Size(28, 28);
            this.buttonSubGrid.TabIndex = 12;
            this.buttonSubGrid.UseVisualStyleBackColor = true;
            this.buttonSubGrid.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lvResults
            // 
            this.lvResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvResults.ContextMenuStrip = this.menuDropDown;
            this.lvResults.Location = new System.Drawing.Point(3, 3);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(309, 243);
            this.lvResults.TabIndex = 0;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.SelectedIndexChanged += new System.EventHandler(this.OnListViewSelectedIndexChange);
            this.lvResults.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListViewMouseDown);
            // 
            // menuDropDown
            // 
            this.menuDropDown.Name = "menuDropDown";
            this.menuDropDown.Size = new System.Drawing.Size(61, 4);
            this.menuDropDown.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnDropDownItemClicked);
            // 
            // tabSave
            // 
            this.tabSave.Controls.Add(this.groupSaveTemplate);
            this.tabSave.Controls.Add(this.groupSaveGrids);
            this.tabSave.Location = new System.Drawing.Point(4, 22);
            this.tabSave.Name = "tabSave";
            this.tabSave.Padding = new System.Windows.Forms.Padding(3);
            this.tabSave.Size = new System.Drawing.Size(372, 249);
            this.tabSave.TabIndex = 3;
            this.tabSave.Text = "Save";
            this.tabSave.UseVisualStyleBackColor = true;
            // 
            // groupSaveTemplate
            // 
            this.groupSaveTemplate.Controls.Add(this.btnSaveTemplate);
            this.groupSaveTemplate.Controls.Add(this.textLayoutTemplateFileName);
            this.groupSaveTemplate.Location = new System.Drawing.Point(17, 131);
            this.groupSaveTemplate.Name = "groupSaveTemplate";
            this.groupSaveTemplate.Size = new System.Drawing.Size(333, 106);
            this.groupSaveTemplate.TabIndex = 17;
            this.groupSaveTemplate.TabStop = false;
            this.groupSaveTemplate.Text = "Save fishing ground layout template";
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.ImageKey = "save";
            this.btnSaveTemplate.ImageList = this.imageList1;
            this.btnSaveTemplate.Location = new System.Drawing.Point(295, 61);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(28, 28);
            this.btnSaveTemplate.TabIndex = 18;
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // textLayoutTemplateFileName
            // 
            this.textLayoutTemplateFileName.Enabled = false;
            this.textLayoutTemplateFileName.Location = new System.Drawing.Point(6, 26);
            this.textLayoutTemplateFileName.Name = "textLayoutTemplateFileName";
            this.textLayoutTemplateFileName.Size = new System.Drawing.Size(321, 20);
            this.textLayoutTemplateFileName.TabIndex = 13;
            // 
            // groupSaveGrids
            // 
            this.groupSaveGrids.Controls.Add(this.btnSave);
            this.groupSaveGrids.Controls.Add(this.textFolderToSave);
            this.groupSaveGrids.Controls.Add(this.btnSelectFolderSave);
            this.groupSaveGrids.Location = new System.Drawing.Point(17, 15);
            this.groupSaveGrids.Name = "groupSaveGrids";
            this.groupSaveGrids.Size = new System.Drawing.Size(333, 106);
            this.groupSaveGrids.TabIndex = 16;
            this.groupSaveGrids.TabStop = false;
            this.groupSaveGrids.Text = "Save all fishing ground grid maps";
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.ImageKey = "save";
            this.btnSave.ImageList = this.imageList1;
            this.btnSave.Location = new System.Drawing.Point(261, 62);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(28, 28);
            this.btnSave.TabIndex = 17;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // textFolderToSave
            // 
            this.textFolderToSave.Enabled = false;
            this.textFolderToSave.Location = new System.Drawing.Point(6, 29);
            this.textFolderToSave.Name = "textFolderToSave";
            this.textFolderToSave.Size = new System.Drawing.Size(321, 20);
            this.textFolderToSave.TabIndex = 12;
            // 
            // btnSelectFolderSave
            // 
            this.btnSelectFolderSave.ImageKey = "addToFolder";
            this.btnSelectFolderSave.ImageList = this.imageList1;
            this.btnSelectFolderSave.Location = new System.Drawing.Point(295, 62);
            this.btnSelectFolderSave.Name = "btnSelectFolderSave";
            this.btnSelectFolderSave.Size = new System.Drawing.Size(28, 28);
            this.btnSelectFolderSave.TabIndex = 11;
            this.btnSelectFolderSave.UseVisualStyleBackColor = true;
            this.btnSelectFolderSave.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // chkAutoExpand
            // 
            this.chkAutoExpand.AutoSize = true;
            this.chkAutoExpand.Location = new System.Drawing.Point(12, 311);
            this.chkAutoExpand.Name = "chkAutoExpand";
            this.chkAutoExpand.Size = new System.Drawing.Size(129, 17);
            this.chkAutoExpand.TabIndex = 14;
            this.chkAutoExpand.Text = "Auto-expand selected";
            this.chkAutoExpand.UseVisualStyleBackColor = true;
            // 
            // tabExport
            // 
            this.tabExport.Controls.Add(this.groupBox1);
            this.tabExport.Location = new System.Drawing.Point(4, 22);
            this.tabExport.Name = "tabExport";
            this.tabExport.Padding = new System.Windows.Forms.Padding(3);
            this.tabExport.Size = new System.Drawing.Size(372, 249);
            this.tabExport.TabIndex = 4;
            this.tabExport.Text = "Export grid maps";
            this.tabExport.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listLayers);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtDPI);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.txtFolderExportPath);
            this.groupBox1.Controls.Add(this.btnFolderExportImage);
            this.groupBox1.Location = new System.Drawing.Point(3, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 232);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export all grid maps to TIFF image files";
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.ImageKey = "image";
            this.btnExport.ImageList = this.imageList1;
            this.btnExport.Location = new System.Drawing.Point(294, 24);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(28, 28);
            this.btnExport.TabIndex = 17;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // txtFolderExportPath
            // 
            this.txtFolderExportPath.Enabled = false;
            this.txtFolderExportPath.Location = new System.Drawing.Point(6, 29);
            this.txtFolderExportPath.Name = "txtFolderExportPath";
            this.txtFolderExportPath.Size = new System.Drawing.Size(280, 20);
            this.txtFolderExportPath.TabIndex = 12;
            // 
            // btnFolderExportImage
            // 
            this.btnFolderExportImage.ImageKey = "addToFolder";
            this.btnFolderExportImage.ImageList = this.imageList1;
            this.btnFolderExportImage.Location = new System.Drawing.Point(326, 24);
            this.btnFolderExportImage.Name = "btnFolderExportImage";
            this.btnFolderExportImage.Size = new System.Drawing.Size(28, 28);
            this.btnFolderExportImage.TabIndex = 11;
            this.btnFolderExportImage.UseVisualStyleBackColor = true;
            this.btnFolderExportImage.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(154, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Hide these layers in front image";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(8, 65);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(187, 17);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Export front and back map images";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // txtDPI
            // 
            this.txtDPI.Location = new System.Drawing.Point(237, 63);
            this.txtDPI.Name = "txtDPI";
            this.txtDPI.Size = new System.Drawing.Size(48, 20);
            this.txtDPI.TabIndex = 21;
            this.txtDPI.Text = "150";
            this.txtDPI.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextValidating);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(206, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "DPI";
            // 
            // listLayers
            // 
            this.listLayers.FormattingEnabled = true;
            this.listLayers.Location = new System.Drawing.Point(8, 111);
            this.listLayers.Name = "listLayers";
            this.listLayers.Size = new System.Drawing.Size(278, 109);
            this.listLayers.TabIndex = 23;
            // 
            // Grid25LayoutHelperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 340);
            this.Controls.Add(this.chkAutoExpand);
            this.Controls.Add(this.tabsLayout);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Grid25LayoutHelperForm";
            this.Text = "Layout grids";
            this.Activated += new System.EventHandler(this.OnFormActivated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.tabsLayout.ResumeLayout(false);
            this.tabLayout.ResumeLayout(false);
            this.tabLayout.PerformLayout();
            this.tabFishingGround.ResumeLayout(false);
            this.tabFishingGround.PerformLayout();
            this.tabResults.ResumeLayout(false);
            this.tabSave.ResumeLayout(false);
            this.groupSaveTemplate.ResumeLayout(false);
            this.groupSaveTemplate.PerformLayout();
            this.groupSaveGrids.ResumeLayout(false);
            this.groupSaveGrids.PerformLayout();
            this.tabExport.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRows;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtColumns;
        private System.Windows.Forms.TextBox txtOverlap;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnApplyDimension;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtPageHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPageWidth;
        private System.Windows.Forms.TabControl tabsLayout;
        private System.Windows.Forms.TabPage tabLayout;
        private System.Windows.Forms.TabPage tabFishingGround;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textFishingGround;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tabResults;
        private System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.CheckBox chkAutoExpand;
        private System.Windows.Forms.ContextMenuStrip menuDropDown;
        private System.Windows.Forms.Button btnInputTitles;
        private System.Windows.Forms.Button buttonSubGrid;
        private System.Windows.Forms.TabPage tabSave;
        private System.Windows.Forms.GroupBox groupSaveGrids;
        private System.Windows.Forms.TextBox textFolderToSave;
        private System.Windows.Forms.Button btnSelectFolderSave;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupSaveTemplate;
        private System.Windows.Forms.Button btnSaveTemplate;
        private System.Windows.Forms.TextBox textLayoutTemplateFileName;
        private System.Windows.Forms.Label lblProvideTitles;
        private System.Windows.Forms.Button btnLocateSourceFolder;
        private System.Windows.Forms.Button btnGridSettings;
        private System.Windows.Forms.TabPage tabExport;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TextBox txtFolderExportPath;
        private System.Windows.Forms.Button btnFolderExportImage;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDPI;
        private System.Windows.Forms.CheckedListBox listLayers;
    }
}