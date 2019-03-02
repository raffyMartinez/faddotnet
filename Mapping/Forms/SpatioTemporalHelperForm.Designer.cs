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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpatioTemporalHelperForm));
            this.tabMap = new System.Windows.Forms.TabControl();
            this.tabStart = new System.Windows.Forms.TabPage();
            this.tabCategorize = new System.Windows.Forms.TabPage();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.cboLatitude = new System.Windows.Forms.ComboBox();
            this.cboLongitude = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTemporal = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboValue = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnReadFile = new System.Windows.Forms.Button();
            this.txtInlandPoints = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cboLastData = new System.Windows.Forms.ComboBox();
            this.cboFirstData = new System.Windows.Forms.ComboBox();
            this.txtRows = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnOk = new System.Windows.Forms.Button();
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
            this.btnExport = new System.Windows.Forms.Button();
            this.graphSheet = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.dgSheetSummary = new System.Windows.Forms.DataGridView();
            this.colVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSymbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblMappedSheet = new System.Windows.Forms.Label();
            this.listSelectedSheets = new System.Windows.Forms.ListBox();
            this.label15 = new System.Windows.Forms.Label();
            this.icbColorScheme = new FAD3.Mapping.UserControls.ImageCombo();
            this.lblParameter = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabMap.SuspendLayout();
            this.tabStart.SuspendLayout();
            this.tabCategorize.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCategories)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.graphSheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSheetSummary)).BeginInit();
            this.SuspendLayout();
            // 
            // tabMap
            // 
            this.tabMap.Controls.Add(this.tabStart);
            this.tabMap.Controls.Add(this.tabCategorize);
            this.tabMap.Controls.Add(this.tabPage1);
            this.tabMap.Location = new System.Drawing.Point(2, 38);
            this.tabMap.Name = "tabMap";
            this.tabMap.SelectedIndex = 0;
            this.tabMap.Size = new System.Drawing.Size(454, 477);
            this.tabMap.TabIndex = 0;
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
            this.label6.Size = new System.Drawing.Size(88, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Last data column";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 315);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "First data column";
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
            this.tabPage1.Controls.Add(this.listSelectedSheets);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(446, 451);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Mapping";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(368, 526);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(63, 25);
            this.btnOk.TabIndex = 28;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
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
            chartArea6.AxisX.LabelStyle.Enabled = false;
            chartArea6.AxisX.MajorGrid.Enabled = false;
            chartArea6.AxisX.MajorTickMark.Enabled = false;
            chartArea6.AxisY.IsLabelAutoFit = false;
            chartArea6.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            chartArea6.AxisY.MajorGrid.Enabled = false;
            chartArea6.Name = "ChartArea1";
            chartArea6.Position.Auto = false;
            chartArea6.Position.Height = 94F;
            chartArea6.Position.Width = 100F;
            this.graphSheet.ChartAreas.Add(chartArea6);
            legend6.Enabled = false;
            legend6.Name = "Legend1";
            this.graphSheet.Legends.Add(legend6);
            this.graphSheet.Location = new System.Drawing.Point(179, 291);
            this.graphSheet.Name = "graphSheet";
            series6.ChartArea = "ChartArea1";
            series6.IsVisibleInLegend = false;
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            series6.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.graphSheet.Series.Add(series6);
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
            // listSelectedSheets
            // 
            this.listSelectedSheets.FormattingEnabled = true;
            this.listSelectedSheets.Location = new System.Drawing.Point(5, 50);
            this.listSelectedSheets.Name = "listSelectedSheets";
            this.listSelectedSheets.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listSelectedSheets.Size = new System.Drawing.Size(143, 303);
            this.listSelectedSheets.TabIndex = 26;
            this.listSelectedSheets.Click += new System.EventHandler(this.OnSelectedSheetsClick);
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
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "go-down.png");
            this.imageList1.Images.SetKeyName(1, "go-up.png");
            // 
            // SpatioTemporalHelperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 567);
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
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCategories)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.graphSheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSheetSummary)).EndInit();
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
        private System.Windows.Forms.ListBox listSelectedSheets;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblParameter;
        private System.Windows.Forms.ImageList imageList1;
    }
}