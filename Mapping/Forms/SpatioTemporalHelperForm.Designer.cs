namespace FAD3.Mapping.Forms
{
    partial class SpatioTemporalHelperForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpatioTemporalHelperForm));
            this.tabMap = new System.Windows.Forms.TabControl();
            this.tabStart = new System.Windows.Forms.TabPage();
            this.txtInlandPoints = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cboLastData = new System.Windows.Forms.ComboBox();
            this.cboFirstData = new System.Windows.Forms.ComboBox();
            this.txtRows = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnReadFile = new System.Windows.Forms.Button();
            this.cboValue = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboTemporal = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboLatitude = new System.Windows.Forms.ComboBox();
            this.cboLongitude = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.tabCategorize = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.cboClassificationScheme = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMaximum = new System.Windows.Forms.TextBox();
            this.txtMinimum = new System.Windows.Forms.TextBox();
            this.txtValuesCount = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnShowGridPoints = new System.Windows.Forms.Button();
            this.btnShowGridPolygons = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.btnColorScheme = new System.Windows.Forms.Button();
            this.dgCategories = new System.Windows.Forms.DataGridView();
            this.dgcolCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcolCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCategorize = new System.Windows.Forms.Button();
            this.txtCategoryCount = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblParameter = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.graphSheet = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnDown = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnUp = new System.Windows.Forms.Button();
            this.dgSheetSummary = new System.Windows.Forms.DataGridView();
            this.colVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSymbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblMappedSheet = new System.Windows.Forms.Label();
            this.listSelectedTimePeriods = new System.Windows.Forms.ListBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tabDownload = new System.Windows.Forms.TabPage();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.icbColorScheme = new FAD3.Mapping.UserControls.ImageCombo();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnUseSelectionBox = new System.Windows.Forms.RadioButton();
            this.rbtnUseSelectedLayer = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnCreateExtent = new System.Windows.Forms.Button();
            this.rbtnManual = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtMinLat = new System.Windows.Forms.TextBox();
            this.txtMaxLat = new System.Windows.Forms.TextBox();
            this.txtMinLon = new System.Windows.Forms.TextBox();
            this.txtMaxLon = new System.Windows.Forms.TextBox();
            this.tabCitation = new System.Windows.Forms.TabPage();
            this.lvERDDAP = new System.Windows.Forms.ListView();
            this.btnDownload = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.txtMetadataFolderPath = new System.Windows.Forms.TextBox();
            this.btnGetMetadataFolder = new System.Windows.Forms.Button();
            this.tabMap.SuspendLayout();
            this.tabStart.SuspendLayout();
            this.tabCategorize.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCategories)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.graphSheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSheetSummary)).BeginInit();
            this.tabDownload.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMap
            // 
            this.tabMap.Controls.Add(this.tabStart);
            this.tabMap.Controls.Add(this.tabCategorize);
            this.tabMap.Controls.Add(this.tabPage1);
            this.tabMap.Controls.Add(this.tabDownload);
            this.tabMap.Controls.Add(this.tabCitation);
            this.tabMap.Location = new System.Drawing.Point(2, 38);
            this.tabMap.Name = "tabMap";
            this.tabMap.SelectedIndex = 0;
            this.tabMap.Size = new System.Drawing.Size(454, 477);
            this.tabMap.TabIndex = 0;
            this.tabMap.SelectedIndexChanged += new System.EventHandler(this.OnTabMapIndexChanged);
            // 
            // tabStart
            // 
            this.tabStart.Controls.Add(this.txtInlandPoints);
            this.tabStart.Controls.Add(this.label14);
            this.tabStart.Controls.Add(this.cboLastData);
            this.tabStart.Controls.Add(this.cboFirstData);
            this.tabStart.Controls.Add(this.txtRows);
            this.tabStart.Controls.Add(this.label5);
            this.tabStart.Controls.Add(this.label6);
            this.tabStart.Controls.Add(this.label7);
            this.tabStart.Controls.Add(this.btnReadFile);
            this.tabStart.Controls.Add(this.cboValue);
            this.tabStart.Controls.Add(this.label4);
            this.tabStart.Controls.Add(this.cboTemporal);
            this.tabStart.Controls.Add(this.label3);
            this.tabStart.Controls.Add(this.cboLatitude);
            this.tabStart.Controls.Add(this.cboLongitude);
            this.tabStart.Controls.Add(this.label2);
            this.tabStart.Controls.Add(this.label1);
            this.tabStart.Controls.Add(this.txtFile);
            this.tabStart.Controls.Add(this.btnOpen);
            this.tabStart.Location = new System.Drawing.Point(4, 22);
            this.tabStart.Name = "tabStart";
            this.tabStart.Padding = new System.Windows.Forms.Padding(3);
            this.tabStart.Size = new System.Drawing.Size(446, 451);
            this.tabStart.TabIndex = 0;
            this.tabStart.Text = "Start";
            this.tabStart.UseVisualStyleBackColor = true;
            // 
            // txtInlandPoints
            // 
            this.txtInlandPoints.BackColor = System.Drawing.SystemColors.Window;
            this.txtInlandPoints.Enabled = false;
            this.txtInlandPoints.Location = new System.Drawing.Point(153, 383);
            this.txtInlandPoints.Name = "txtInlandPoints";
            this.txtInlandPoints.ReadOnly = true;
            this.txtInlandPoints.Size = new System.Drawing.Size(185, 20);
            this.txtInlandPoints.TabIndex = 35;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(33, 386);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(67, 13);
            this.label14.TabIndex = 34;
            this.label14.Text = "Inland points";
            // 
            // cboLastData
            // 
            this.cboLastData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLastData.Enabled = false;
            this.cboLastData.FormattingEnabled = true;
            this.cboLastData.Location = new System.Drawing.Point(153, 348);
            this.cboLastData.Name = "cboLastData";
            this.cboLastData.Size = new System.Drawing.Size(186, 21);
            this.cboLastData.TabIndex = 33;
            this.cboLastData.SelectedIndexChanged += new System.EventHandler(this.OnComboSelectionIndexChanged);
            // 
            // cboFirstData
            // 
            this.cboFirstData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFirstData.Enabled = false;
            this.cboFirstData.FormattingEnabled = true;
            this.cboFirstData.Location = new System.Drawing.Point(154, 312);
            this.cboFirstData.Name = "cboFirstData";
            this.cboFirstData.Size = new System.Drawing.Size(186, 21);
            this.cboFirstData.TabIndex = 32;
            this.cboFirstData.SelectedIndexChanged += new System.EventHandler(this.OnComboSelectionIndexChanged);
            // 
            // txtRows
            // 
            this.txtRows.BackColor = System.Drawing.SystemColors.Window;
            this.txtRows.Enabled = false;
            this.txtRows.Location = new System.Drawing.Point(153, 277);
            this.txtRows.Name = "txtRows";
            this.txtRows.ReadOnly = true;
            this.txtRows.Size = new System.Drawing.Size(185, 20);
            this.txtRows.TabIndex = 31;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 280);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Data row count";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 351);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "End time period";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 315);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "Start time period";
            // 
            // btnReadFile
            // 
            this.btnReadFile.Enabled = false;
            this.btnReadFile.Location = new System.Drawing.Point(195, 219);
            this.btnReadFile.Name = "btnReadFile";
            this.btnReadFile.Size = new System.Drawing.Size(63, 25);
            this.btnReadFile.TabIndex = 27;
            this.btnReadFile.Text = "Read file";
            this.btnReadFile.UseVisualStyleBackColor = true;
            this.btnReadFile.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // cboValue
            // 
            this.cboValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboValue.Enabled = false;
            this.cboValue.FormattingEnabled = true;
            this.cboValue.Location = new System.Drawing.Point(153, 179);
            this.cboValue.Name = "cboValue";
            this.cboValue.Size = new System.Drawing.Size(185, 21);
            this.cboValue.TabIndex = 26;
            this.cboValue.Tag = "dc";
            this.cboValue.SelectedIndexChanged += new System.EventHandler(this.OnComboSelectionIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Values column";
            // 
            // cboTemporal
            // 
            this.cboTemporal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTemporal.Enabled = false;
            this.cboTemporal.FormattingEnabled = true;
            this.cboTemporal.Location = new System.Drawing.Point(153, 139);
            this.cboTemporal.Name = "cboTemporal";
            this.cboTemporal.Size = new System.Drawing.Size(185, 21);
            this.cboTemporal.TabIndex = 24;
            this.cboTemporal.Tag = "dc";
            this.cboTemporal.SelectedIndexChanged += new System.EventHandler(this.OnComboSelectionIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Temporal column";
            // 
            // cboLatitude
            // 
            this.cboLatitude.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLatitude.Enabled = false;
            this.cboLatitude.FormattingEnabled = true;
            this.cboLatitude.Location = new System.Drawing.Point(153, 101);
            this.cboLatitude.Name = "cboLatitude";
            this.cboLatitude.Size = new System.Drawing.Size(185, 21);
            this.cboLatitude.TabIndex = 22;
            this.cboLatitude.Tag = "dc";
            this.cboLatitude.SelectedIndexChanged += new System.EventHandler(this.OnComboSelectionIndexChanged);
            // 
            // cboLongitude
            // 
            this.cboLongitude.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLongitude.Enabled = false;
            this.cboLongitude.FormattingEnabled = true;
            this.cboLongitude.Location = new System.Drawing.Point(153, 64);
            this.cboLongitude.Name = "cboLongitude";
            this.cboLongitude.Size = new System.Drawing.Size(185, 21);
            this.cboLongitude.TabIndex = 21;
            this.cboLongitude.Tag = "dc";
            this.cboLongitude.SelectedIndexChanged += new System.EventHandler(this.OnComboSelectionIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Latitude column";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Longitude column";
            // 
            // txtFile
            // 
            this.txtFile.BackColor = System.Drawing.SystemColors.Window;
            this.txtFile.Location = new System.Drawing.Point(66, 21);
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(359, 20);
            this.txtFile.TabIndex = 18;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(10, 18);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(50, 25);
            this.btnOpen.TabIndex = 17;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // tabCategorize
            // 
            this.tabCategorize.Controls.Add(this.label8);
            this.tabCategorize.Controls.Add(this.cboClassificationScheme);
            this.tabCategorize.Controls.Add(this.groupBox3);
            this.tabCategorize.Controls.Add(this.groupBox2);
            this.tabCategorize.Controls.Add(this.label13);
            this.tabCategorize.Controls.Add(this.btnColorScheme);
            this.tabCategorize.Controls.Add(this.dgCategories);
            this.tabCategorize.Controls.Add(this.btnCategorize);
            this.tabCategorize.Controls.Add(this.txtCategoryCount);
            this.tabCategorize.Controls.Add(this.label10);
            this.tabCategorize.Controls.Add(this.icbColorScheme);
            this.tabCategorize.Location = new System.Drawing.Point(4, 22);
            this.tabCategorize.Name = "tabCategorize";
            this.tabCategorize.Padding = new System.Windows.Forms.Padding(3);
            this.tabCategorize.Size = new System.Drawing.Size(446, 451);
            this.tabCategorize.TabIndex = 1;
            this.tabCategorize.Text = "Categorize";
            this.tabCategorize.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(-2, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "Classification scheme";
            // 
            // cboClassificationScheme
            // 
            this.cboClassificationScheme.FormattingEnabled = true;
            this.cboClassificationScheme.Location = new System.Drawing.Point(112, 37);
            this.cboClassificationScheme.Name = "cboClassificationScheme";
            this.cboClassificationScheme.Size = new System.Drawing.Size(145, 21);
            this.cboClassificationScheme.TabIndex = 40;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtMaximum);
            this.groupBox3.Controls.Add(this.txtMinimum);
            this.groupBox3.Controls.Add(this.txtValuesCount);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(-7, 333);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(192, 101);
            this.groupBox3.TabIndex = 39;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Summary";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 74);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Maximum";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 49);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "Minimum";
            // 
            // txtMaximum
            // 
            this.txtMaximum.Location = new System.Drawing.Point(108, 73);
            this.txtMaximum.Name = "txtMaximum";
            this.txtMaximum.Size = new System.Drawing.Size(64, 20);
            this.txtMaximum.TabIndex = 25;
            // 
            // txtMinimum
            // 
            this.txtMinimum.Location = new System.Drawing.Point(108, 46);
            this.txtMinimum.Name = "txtMinimum";
            this.txtMinimum.Size = new System.Drawing.Size(64, 20);
            this.txtMinimum.TabIndex = 24;
            // 
            // txtValuesCount
            // 
            this.txtValuesCount.Location = new System.Drawing.Point(108, 20);
            this.txtValuesCount.Name = "txtValuesCount";
            this.txtValuesCount.Size = new System.Drawing.Size(64, 20);
            this.txtValuesCount.TabIndex = 23;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Number of values";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnShowGridPoints);
            this.groupBox2.Controls.Add(this.btnShowGridPolygons);
            this.groupBox2.Location = new System.Drawing.Point(236, 333);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 101);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Show on map";
            // 
            // btnShowGridPoints
            // 
            this.btnShowGridPoints.Location = new System.Drawing.Point(68, 20);
            this.btnShowGridPoints.Name = "btnShowGridPoints";
            this.btnShowGridPoints.Size = new System.Drawing.Size(73, 25);
            this.btnShowGridPoints.TabIndex = 13;
            this.btnShowGridPoints.Text = "Data points";
            this.btnShowGridPoints.UseVisualStyleBackColor = true;
            this.btnShowGridPoints.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnShowGridPolygons
            // 
            this.btnShowGridPolygons.Location = new System.Drawing.Point(68, 62);
            this.btnShowGridPolygons.Name = "btnShowGridPolygons";
            this.btnShowGridPolygons.Size = new System.Drawing.Size(73, 25);
            this.btnShowGridPolygons.TabIndex = 26;
            this.btnShowGridPolygons.Text = "Grid";
            this.btnShowGridPolygons.UseVisualStyleBackColor = true;
            this.btnShowGridPolygons.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(-2, 8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(76, 13);
            this.label13.TabIndex = 37;
            this.label13.Text = "Color schemes";
            // 
            // btnColorScheme
            // 
            this.btnColorScheme.Location = new System.Drawing.Point(352, 6);
            this.btnColorScheme.Name = "btnColorScheme";
            this.btnColorScheme.Size = new System.Drawing.Size(90, 25);
            this.btnColorScheme.TabIndex = 35;
            this.btnColorScheme.Text = "Choose colors";
            this.btnColorScheme.UseVisualStyleBackColor = true;
            this.btnColorScheme.Visible = false;
            // 
            // dgCategories
            // 
            this.dgCategories.AllowUserToAddRows = false;
            this.dgCategories.AllowUserToDeleteRows = false;
            this.dgCategories.AllowUserToResizeColumns = false;
            this.dgCategories.AllowUserToResizeRows = false;
            this.dgCategories.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCategories.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgcolCategory,
            this.dgcolCount,
            this.dgCol});
            this.dgCategories.GridColor = System.Drawing.SystemColors.Window;
            this.dgCategories.Location = new System.Drawing.Point(1, 120);
            this.dgCategories.Name = "dgCategories";
            this.dgCategories.ReadOnly = true;
            this.dgCategories.RowHeadersVisible = false;
            this.dgCategories.Size = new System.Drawing.Size(439, 194);
            this.dgCategories.TabIndex = 34;
            // 
            // dgcolCategory
            // 
            this.dgcolCategory.HeaderText = "Category";
            this.dgcolCategory.Name = "dgcolCategory";
            this.dgcolCategory.ReadOnly = true;
            // 
            // dgcolCount
            // 
            this.dgcolCount.HeaderText = "Count";
            this.dgcolCount.Name = "dgcolCount";
            this.dgcolCount.ReadOnly = true;
            // 
            // dgCol
            // 
            this.dgCol.HeaderText = "Symbol";
            this.dgCol.Name = "dgCol";
            this.dgCol.ReadOnly = true;
            this.dgCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btnCategorize
            // 
            this.btnCategorize.Location = new System.Drawing.Point(268, 65);
            this.btnCategorize.Name = "btnCategorize";
            this.btnCategorize.Size = new System.Drawing.Size(90, 25);
            this.btnCategorize.TabIndex = 33;
            this.btnCategorize.Text = "Categorize";
            this.btnCategorize.UseVisualStyleBackColor = true;
            this.btnCategorize.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // txtCategoryCount
            // 
            this.txtCategoryCount.Location = new System.Drawing.Point(112, 68);
            this.txtCategoryCount.Name = "txtCategoryCount";
            this.txtCategoryCount.Size = new System.Drawing.Size(146, 20);
            this.txtCategoryCount.TabIndex = 32;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(-2, 71);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(108, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "Number of categories";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblParameter);
            this.tabPage1.Controls.Add(this.btnExport);
            this.tabPage1.Controls.Add(this.graphSheet);
            this.tabPage1.Controls.Add(this.btnDown);
            this.tabPage1.Controls.Add(this.btnUp);
            this.tabPage1.Controls.Add(this.dgSheetSummary);
            this.tabPage1.Controls.Add(this.lblMappedSheet);
            this.tabPage1.Controls.Add(this.listSelectedTimePeriods);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(446, 451);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Mapping";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblParameter
            // 
            this.lblParameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParameter.Location = new System.Drawing.Point(188, 421);
            this.lblParameter.Name = "lblParameter";
            this.lblParameter.Size = new System.Drawing.Size(254, 18);
            this.lblParameter.TabIndex = 33;
            this.lblParameter.Text = "Mapped parameter";
            this.lblParameter.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(13, 371);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(121, 25);
            this.btnExport.TabIndex = 29;
            this.btnExport.Text = "Export to time series";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // graphSheet
            // 
            chartArea1.AxisX.LabelStyle.Enabled = false;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 94F;
            chartArea1.Position.Width = 100F;
            this.graphSheet.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.graphSheet.Legends.Add(legend1);
            this.graphSheet.Location = new System.Drawing.Point(179, 291);
            this.graphSheet.Name = "graphSheet";
            series1.ChartArea = "ChartArea1";
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.graphSheet.Series.Add(series1);
            this.graphSheet.Size = new System.Drawing.Size(265, 105);
            this.graphSheet.TabIndex = 32;
            this.graphSheet.Text = "chart1";
            // 
            // btnDown
            // 
            this.btnDown.ImageKey = "go-down.png";
            this.btnDown.ImageList = this.imageList1;
            this.btnDown.Location = new System.Drawing.Point(154, 172);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(30, 30);
            this.btnDown.TabIndex = 31;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "go-down.png");
            this.imageList1.Images.SetKeyName(1, "go-up.png");
            // 
            // btnUp
            // 
            this.btnUp.ImageKey = "go-up.png";
            this.btnUp.ImageList = this.imageList1;
            this.btnUp.Location = new System.Drawing.Point(154, 127);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(30, 30);
            this.btnUp.TabIndex = 30;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // dgSheetSummary
            // 
            this.dgSheetSummary.AllowUserToAddRows = false;
            this.dgSheetSummary.AllowUserToDeleteRows = false;
            this.dgSheetSummary.AllowUserToResizeColumns = false;
            this.dgSheetSummary.AllowUserToResizeRows = false;
            this.dgSheetSummary.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgSheetSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSheetSummary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVisible,
            this.colSymbol,
            this.colCount,
            this.colPercent});
            this.dgSheetSummary.Location = new System.Drawing.Point(190, 50);
            this.dgSheetSummary.Name = "dgSheetSummary";
            this.dgSheetSummary.ReadOnly = true;
            this.dgSheetSummary.RowHeadersVisible = false;
            this.dgSheetSummary.Size = new System.Drawing.Size(254, 235);
            this.dgSheetSummary.TabIndex = 28;
            this.dgSheetSummary.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnGridSummaryCellClick);
            // 
            // colVisible
            // 
            this.colVisible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colVisible.Frozen = true;
            this.colVisible.HeaderText = "";
            this.colVisible.MinimumWidth = 25;
            this.colVisible.Name = "colVisible";
            this.colVisible.ReadOnly = true;
            this.colVisible.Width = 25;
            // 
            // colSymbol
            // 
            this.colSymbol.Frozen = true;
            this.colSymbol.HeaderText = "Symbol";
            this.colSymbol.Name = "colSymbol";
            this.colSymbol.ReadOnly = true;
            // 
            // colCount
            // 
            this.colCount.Frozen = true;
            this.colCount.HeaderText = "Count";
            this.colCount.Name = "colCount";
            this.colCount.ReadOnly = true;
            // 
            // colPercent
            // 
            this.colPercent.Frozen = true;
            this.colPercent.HeaderText = "Percent";
            this.colPercent.Name = "colPercent";
            this.colPercent.ReadOnly = true;
            // 
            // lblMappedSheet
            // 
            this.lblMappedSheet.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMappedSheet.Location = new System.Drawing.Point(190, 399);
            this.lblMappedSheet.Name = "lblMappedSheet";
            this.lblMappedSheet.Size = new System.Drawing.Size(254, 18);
            this.lblMappedSheet.TabIndex = 27;
            this.lblMappedSheet.Text = "Mapped sheet";
            this.lblMappedSheet.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // listSelectedTimePeriods
            // 
            this.listSelectedTimePeriods.FormattingEnabled = true;
            this.listSelectedTimePeriods.Location = new System.Drawing.Point(5, 50);
            this.listSelectedTimePeriods.Name = "listSelectedTimePeriods";
            this.listSelectedTimePeriods.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listSelectedTimePeriods.Size = new System.Drawing.Size(143, 303);
            this.listSelectedTimePeriods.TabIndex = 26;
            this.listSelectedTimePeriods.Click += new System.EventHandler(this.OnSelectedSheetsClick);
            this.listSelectedTimePeriods.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnListBoxKeyDown);
            this.listSelectedTimePeriods.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnListBoxKeyUp);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(2, 34);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(40, 13);
            this.label15.TabIndex = 25;
            this.label15.Text = "Sheets";
            // 
            // tabDownload
            // 
            this.tabDownload.Controls.Add(this.groupBox4);
            this.tabDownload.Controls.Add(this.groupBox1);
            this.tabDownload.Location = new System.Drawing.Point(4, 22);
            this.tabDownload.Name = "tabDownload";
            this.tabDownload.Padding = new System.Windows.Forms.Padding(3);
            this.tabDownload.Size = new System.Drawing.Size(446, 451);
            this.tabDownload.TabIndex = 3;
            this.tabDownload.Text = "Download";
            this.tabDownload.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(389, 526);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(63, 25);
            this.btnOk.TabIndex = 28;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(3, 532);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(343, 18);
            this.lblStatus.TabIndex = 29;
            // 
            // icbColorScheme
            // 
            this.icbColorScheme.Color1 = System.Drawing.Color.Gray;
            this.icbColorScheme.Color2 = System.Drawing.Color.Gray;
            this.icbColorScheme.ColorSchemes = null;
            this.icbColorScheme.ComboStyle = FAD3.Mapping.UserControls.ImageComboStyle.ColorSchemeGraduated;
            this.icbColorScheme.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.icbColorScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.icbColorScheme.FormattingEnabled = true;
            this.icbColorScheme.Location = new System.Drawing.Point(112, 6);
            this.icbColorScheme.Name = "icbColorScheme";
            this.icbColorScheme.OutlineColor = System.Drawing.Color.Black;
            this.icbColorScheme.Size = new System.Drawing.Size(146, 21);
            this.icbColorScheme.TabIndex = 36;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMaxLon);
            this.groupBox1.Controls.Add(this.txtMinLon);
            this.groupBox1.Controls.Add(this.txtMaxLat);
            this.groupBox1.Controls.Add(this.txtMinLat);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.rbtnManual);
            this.groupBox1.Controls.Add(this.btnCreateExtent);
            this.groupBox1.Controls.Add(this.rbtnUseSelectedLayer);
            this.groupBox1.Controls.Add(this.rbtnUseSelectionBox);
            this.groupBox1.Location = new System.Drawing.Point(6, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(432, 145);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Set extents";
            // 
            // rbtnUseSelectionBox
            // 
            this.rbtnUseSelectionBox.AutoSize = true;
            this.rbtnUseSelectionBox.Location = new System.Drawing.Point(12, 22);
            this.rbtnUseSelectionBox.Name = "rbtnUseSelectionBox";
            this.rbtnUseSelectionBox.Size = new System.Drawing.Size(118, 17);
            this.rbtnUseSelectionBox.TabIndex = 0;
            this.rbtnUseSelectionBox.TabStop = true;
            this.rbtnUseSelectionBox.Text = "Use a selection box";
            this.rbtnUseSelectionBox.UseVisualStyleBackColor = true;
            this.rbtnUseSelectionBox.CheckedChanged += new System.EventHandler(this.OnRadioButtonCheckChange);
            // 
            // rbtnUseSelectedLayer
            // 
            this.rbtnUseSelectedLayer.AutoSize = true;
            this.rbtnUseSelectedLayer.Location = new System.Drawing.Point(12, 45);
            this.rbtnUseSelectedLayer.Name = "rbtnUseSelectedLayer";
            this.rbtnUseSelectedLayer.Size = new System.Drawing.Size(112, 17);
            this.rbtnUseSelectedLayer.TabIndex = 1;
            this.rbtnUseSelectedLayer.TabStop = true;
            this.rbtnUseSelectedLayer.Text = "Use selected layer";
            this.rbtnUseSelectedLayer.UseVisualStyleBackColor = true;
            this.rbtnUseSelectedLayer.CheckedChanged += new System.EventHandler(this.OnRadioButtonCheckChange);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtMetadataFolderPath);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.btnGetMetadataFolder);
            this.groupBox4.Controls.Add(this.btnDownload);
            this.groupBox4.Controls.Add(this.lvERDDAP);
            this.groupBox4.Location = new System.Drawing.Point(6, 181);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(432, 264);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Data to download";
            // 
            // btnCreateExtent
            // 
            this.btnCreateExtent.Location = new System.Drawing.Point(22, 99);
            this.btnCreateExtent.Name = "btnCreateExtent";
            this.btnCreateExtent.Size = new System.Drawing.Size(78, 27);
            this.btnCreateExtent.TabIndex = 2;
            this.btnCreateExtent.Text = "Create extent";
            this.btnCreateExtent.UseVisualStyleBackColor = true;
            this.btnCreateExtent.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // rbtnManual
            // 
            this.rbtnManual.AutoSize = true;
            this.rbtnManual.Location = new System.Drawing.Point(12, 68);
            this.rbtnManual.Name = "rbtnManual";
            this.rbtnManual.Size = new System.Drawing.Size(100, 17);
            this.rbtnManual.TabIndex = 3;
            this.rbtnManual.TabStop = true;
            this.rbtnManual.Text = "Define manually";
            this.rbtnManual.UseVisualStyleBackColor = true;
            this.rbtnManual.CheckedChanged += new System.EventHandler(this.OnRadioButtonCheckChange);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(154, 25);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(64, 13);
            this.label16.TabIndex = 4;
            this.label16.Text = "Min. latitude";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(154, 53);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(67, 13);
            this.label17.TabIndex = 5;
            this.label17.Text = "Max. latitude";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(154, 79);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(73, 13);
            this.label18.TabIndex = 6;
            this.label18.Text = "Min. longitude";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(154, 106);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(76, 13);
            this.label19.TabIndex = 7;
            this.label19.Text = "Max. longitude";
            // 
            // txtMinLat
            // 
            this.txtMinLat.Location = new System.Drawing.Point(236, 22);
            this.txtMinLat.Name = "txtMinLat";
            this.txtMinLat.Size = new System.Drawing.Size(155, 20);
            this.txtMinLat.TabIndex = 8;
            // 
            // txtMaxLat
            // 
            this.txtMaxLat.Location = new System.Drawing.Point(236, 50);
            this.txtMaxLat.Name = "txtMaxLat";
            this.txtMaxLat.Size = new System.Drawing.Size(155, 20);
            this.txtMaxLat.TabIndex = 9;
            // 
            // txtMinLon
            // 
            this.txtMinLon.Location = new System.Drawing.Point(236, 76);
            this.txtMinLon.Name = "txtMinLon";
            this.txtMinLon.Size = new System.Drawing.Size(155, 20);
            this.txtMinLon.TabIndex = 10;
            // 
            // txtMaxLon
            // 
            this.txtMaxLon.Location = new System.Drawing.Point(236, 103);
            this.txtMaxLon.Name = "txtMaxLon";
            this.txtMaxLon.Size = new System.Drawing.Size(155, 20);
            this.txtMaxLon.TabIndex = 11;
            // 
            // tabCitation
            // 
            this.tabCitation.Location = new System.Drawing.Point(4, 22);
            this.tabCitation.Name = "tabCitation";
            this.tabCitation.Padding = new System.Windows.Forms.Padding(3);
            this.tabCitation.Size = new System.Drawing.Size(446, 451);
            this.tabCitation.TabIndex = 4;
            this.tabCitation.Text = "Citation";
            this.tabCitation.UseVisualStyleBackColor = true;
            // 
            // lvERDDAP
            // 
            this.lvERDDAP.CheckBoxes = true;
            this.lvERDDAP.Location = new System.Drawing.Point(12, 50);
            this.lvERDDAP.Name = "lvERDDAP";
            this.lvERDDAP.Size = new System.Drawing.Size(408, 175);
            this.lvERDDAP.TabIndex = 0;
            this.lvERDDAP.UseCompatibleStateImageBehavior = false;
            this.lvERDDAP.View = System.Windows.Forms.View.Details;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(347, 231);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(73, 27);
            this.btnDownload.TabIndex = 12;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(14, 25);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(36, 13);
            this.label20.TabIndex = 15;
            this.label20.Text = "Folder";
            // 
            // txtMetadataFolderPath
            // 
            this.txtMetadataFolderPath.Location = new System.Drawing.Point(58, 20);
            this.txtMetadataFolderPath.Name = "txtMetadataFolderPath";
            this.txtMetadataFolderPath.Size = new System.Drawing.Size(325, 20);
            this.txtMetadataFolderPath.TabIndex = 16;
            // 
            // btnGetMetadataFolder
            // 
            this.btnGetMetadataFolder.Location = new System.Drawing.Point(393, 20);
            this.btnGetMetadataFolder.Name = "btnGetMetadataFolder";
            this.btnGetMetadataFolder.Size = new System.Drawing.Size(27, 24);
            this.btnGetMetadataFolder.TabIndex = 14;
            this.btnGetMetadataFolder.Text = "F";
            this.btnGetMetadataFolder.UseVisualStyleBackColor = true;
            this.btnGetMetadataFolder.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // SpatioTemporalHelperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 567);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SpatioTemporalHelperForm";
            this.Text = "SpatioTemporalHelperForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.tabMap.ResumeLayout(false);
            this.tabStart.ResumeLayout(false);
            this.tabStart.PerformLayout();
            this.tabCategorize.ResumeLayout(false);
            this.tabCategorize.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCategories)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.graphSheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSheetSummary)).EndInit();
            this.tabDownload.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMap;
        private System.Windows.Forms.TabPage tabStart;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.TabPage tabCategorize;
        private System.Windows.Forms.ComboBox cboValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboTemporal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboLatitude;
        private System.Windows.Forms.ComboBox cboLongitude;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReadFile;
        private System.Windows.Forms.TextBox txtInlandPoints;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cboLastData;
        private System.Windows.Forms.ComboBox cboFirstData;
        private System.Windows.Forms.TextBox txtRows;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboClassificationScheme;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMaximum;
        private System.Windows.Forms.TextBox txtMinimum;
        private System.Windows.Forms.TextBox txtValuesCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnShowGridPoints;
        private System.Windows.Forms.Button btnShowGridPolygons;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnColorScheme;
        private System.Windows.Forms.DataGridView dgCategories;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcolCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcolCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgCol;
        private System.Windows.Forms.Button btnCategorize;
        private System.Windows.Forms.TextBox txtCategoryCount;
        private System.Windows.Forms.Label label10;
        private UserControls.ImageCombo icbColorScheme;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DataVisualization.Charting.Chart graphSheet;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.DataGridView dgSheetSummary;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSymbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPercent;
        private System.Windows.Forms.Label lblMappedSheet;
        private System.Windows.Forms.ListBox listSelectedTimePeriods;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblParameter;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TabPage tabDownload;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnUseSelectedLayer;
        private System.Windows.Forms.RadioButton rbtnUseSelectionBox;
        private System.Windows.Forms.TextBox txtMaxLon;
        private System.Windows.Forms.TextBox txtMinLon;
        private System.Windows.Forms.TextBox txtMaxLat;
        private System.Windows.Forms.TextBox txtMinLat;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.RadioButton rbtnManual;
        private System.Windows.Forms.Button btnCreateExtent;
        private System.Windows.Forms.TabPage tabCitation;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.ListView lvERDDAP;
        private System.Windows.Forms.TextBox txtMetadataFolderPath;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnGetMetadataFolder;
    }
}