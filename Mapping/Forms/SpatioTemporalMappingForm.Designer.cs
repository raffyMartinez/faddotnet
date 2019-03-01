namespace FAD3.Mapping.Forms
{
    partial class SpatioTemporalMappingForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpatioTemporalMappingForm));
            this.tabsPages = new System.Windows.Forms.TabControl();
            this.tabUI = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboLastData = new System.Windows.Forms.ComboBox();
            this.cboFirstData = new System.Windows.Forms.ComboBox();
            this.btnReadSheet = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.listSheets = new System.Windows.Forms.ListBox();
            this.txtRows = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboLatitude = new System.Windows.Forms.ComboBox();
            this.cboLongitude = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReadWorkbook = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.chkHasHeader = new System.Windows.Forms.CheckBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.tabCategorize = new System.Windows.Forms.TabPage();
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
            this.tabExtract = new System.Windows.Forms.TabPage();
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
            this.listSelectedSheets = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnInstructions = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtInlandPoints = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.icbColorScheme = new FAD3.Mapping.UserControls.ImageCombo();
            this.cboClassificationScheme = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabsPages.SuspendLayout();
            this.tabUI.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabCategorize.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCategories)).BeginInit();
            this.tabExtract.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.graphSheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSheetSummary)).BeginInit();
            this.SuspendLayout();
            // 
            // tabsPages
            // 
            this.tabsPages.Controls.Add(this.tabUI);
            this.tabsPages.Controls.Add(this.tabCategorize);
            this.tabsPages.Controls.Add(this.tabExtract);
            this.tabsPages.Location = new System.Drawing.Point(7, 22);
            this.tabsPages.Name = "tabsPages";
            this.tabsPages.SelectedIndex = 0;
            this.tabsPages.Size = new System.Drawing.Size(470, 481);
            this.tabsPages.TabIndex = 1;
            // 
            // tabUI
            // 
            this.tabUI.Controls.Add(this.groupBox1);
            this.tabUI.Controls.Add(this.btnReadWorkbook);
            this.tabUI.Controls.Add(this.txtFile);
            this.tabUI.Controls.Add(this.chkHasHeader);
            this.tabUI.Controls.Add(this.btnOpen);
            this.tabUI.Location = new System.Drawing.Point(4, 22);
            this.tabUI.Name = "tabUI";
            this.tabUI.Padding = new System.Windows.Forms.Padding(3);
            this.tabUI.Size = new System.Drawing.Size(462, 418);
            this.tabUI.TabIndex = 1;
            this.tabUI.Text = "Start";
            this.tabUI.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtInlandPoints);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.cboLastData);
            this.groupBox1.Controls.Add(this.cboFirstData);
            this.groupBox1.Controls.Add(this.btnReadSheet);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.listSheets);
            this.groupBox1.Controls.Add(this.txtRows);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboLatitude);
            this.groupBox1.Controls.Add(this.cboLongitude);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 111);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 248);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // cboLastData
            // 
            this.cboLastData.Enabled = false;
            this.cboLastData.FormattingEnabled = true;
            this.cboLastData.Location = new System.Drawing.Point(299, 178);
            this.cboLastData.Name = "cboLastData";
            this.cboLastData.Size = new System.Drawing.Size(114, 21);
            this.cboLastData.TabIndex = 21;
            this.cboLastData.SelectedIndexChanged += new System.EventHandler(this.OnComboIndexChanged);
            // 
            // cboFirstData
            // 
            this.cboFirstData.Enabled = false;
            this.cboFirstData.FormattingEnabled = true;
            this.cboFirstData.Location = new System.Drawing.Point(300, 142);
            this.cboFirstData.Name = "cboFirstData";
            this.cboFirstData.Size = new System.Drawing.Size(114, 21);
            this.cboFirstData.TabIndex = 20;
            this.cboFirstData.SelectedIndexChanged += new System.EventHandler(this.OnComboIndexChanged);
            // 
            // btnReadSheet
            // 
            this.btnReadSheet.Location = new System.Drawing.Point(10, 184);
            this.btnReadSheet.Name = "btnReadSheet";
            this.btnReadSheet.Size = new System.Drawing.Size(117, 25);
            this.btnReadSheet.TabIndex = 19;
            this.btnReadSheet.Text = "Read selected sheet";
            this.btnReadSheet.UseVisualStyleBackColor = true;
            this.btnReadSheet.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Sheets";
            // 
            // listSheets
            // 
            this.listSheets.FormattingEnabled = true;
            this.listSheets.Location = new System.Drawing.Point(10, 30);
            this.listSheets.Name = "listSheets";
            this.listSheets.Size = new System.Drawing.Size(153, 147);
            this.listSheets.TabIndex = 17;
            this.listSheets.Click += new System.EventHandler(this.OnListBoxClick);
            // 
            // txtRows
            // 
            this.txtRows.BackColor = System.Drawing.SystemColors.Window;
            this.txtRows.Enabled = false;
            this.txtRows.Location = new System.Drawing.Point(299, 30);
            this.txtRows.Name = "txtRows";
            this.txtRows.ReadOnly = true;
            this.txtRows.Size = new System.Drawing.Size(113, 20);
            this.txtRows.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(179, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Data row count";
            // 
            // cboLatitude
            // 
            this.cboLatitude.Enabled = false;
            this.cboLatitude.FormattingEnabled = true;
            this.cboLatitude.Location = new System.Drawing.Point(299, 105);
            this.cboLatitude.Name = "cboLatitude";
            this.cboLatitude.Size = new System.Drawing.Size(114, 21);
            this.cboLatitude.TabIndex = 11;
            this.cboLatitude.SelectedIndexChanged += new System.EventHandler(this.OnComboIndexChanged);
            // 
            // cboLongitude
            // 
            this.cboLongitude.Enabled = false;
            this.cboLongitude.FormattingEnabled = true;
            this.cboLongitude.Location = new System.Drawing.Point(299, 68);
            this.cboLongitude.Name = "cboLongitude";
            this.cboLongitude.Size = new System.Drawing.Size(114, 21);
            this.cboLongitude.TabIndex = 10;
            this.cboLongitude.SelectedIndexChanged += new System.EventHandler(this.OnComboIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(179, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Last data column";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(179, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "First data column";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(179, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Latitude data column";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(179, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Longitude data column";
            // 
            // btnReadWorkbook
            // 
            this.btnReadWorkbook.Enabled = false;
            this.btnReadWorkbook.Location = new System.Drawing.Point(260, 69);
            this.btnReadWorkbook.Name = "btnReadWorkbook";
            this.btnReadWorkbook.Size = new System.Drawing.Size(98, 25);
            this.btnReadWorkbook.TabIndex = 19;
            this.btnReadWorkbook.Text = "Read workbook";
            this.btnReadWorkbook.UseVisualStyleBackColor = true;
            this.btnReadWorkbook.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // txtFile
            // 
            this.txtFile.BackColor = System.Drawing.SystemColors.Window;
            this.txtFile.Location = new System.Drawing.Point(75, 26);
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(379, 20);
            this.txtFile.TabIndex = 16;
            // 
            // chkHasHeader
            // 
            this.chkHasHeader.AutoSize = true;
            this.chkHasHeader.Location = new System.Drawing.Point(91, 74);
            this.chkHasHeader.Name = "chkHasHeader";
            this.chkHasHeader.Size = new System.Drawing.Size(144, 17);
            this.chkHasHeader.TabIndex = 5;
            this.chkHasHeader.Text = "First row contain headers";
            this.chkHasHeader.UseVisualStyleBackColor = true;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(19, 23);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(50, 25);
            this.btnOpen.TabIndex = 4;
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
            this.tabCategorize.Size = new System.Drawing.Size(462, 455);
            this.tabCategorize.TabIndex = 3;
            this.tabCategorize.Text = "Categories";
            this.tabCategorize.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtMaximum);
            this.groupBox3.Controls.Add(this.txtMinimum);
            this.groupBox3.Controls.Add(this.txtValuesCount);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(7, 343);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(192, 101);
            this.groupBox3.TabIndex = 28;
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
            this.groupBox2.Location = new System.Drawing.Point(250, 343);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 101);
            this.groupBox2.TabIndex = 27;
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
            this.label13.Location = new System.Drawing.Point(12, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(76, 13);
            this.label13.TabIndex = 25;
            this.label13.Text = "Color schemes";
            // 
            // btnColorScheme
            // 
            this.btnColorScheme.Location = new System.Drawing.Point(366, 16);
            this.btnColorScheme.Name = "btnColorScheme";
            this.btnColorScheme.Size = new System.Drawing.Size(90, 25);
            this.btnColorScheme.TabIndex = 23;
            this.btnColorScheme.Text = "Choose colors";
            this.btnColorScheme.UseVisualStyleBackColor = true;
            this.btnColorScheme.Visible = false;
            this.btnColorScheme.Click += new System.EventHandler(this.OnButtonClick);
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
            this.dgCategories.Location = new System.Drawing.Point(15, 130);
            this.dgCategories.Name = "dgCategories";
            this.dgCategories.ReadOnly = true;
            this.dgCategories.RowHeadersVisible = false;
            this.dgCategories.Size = new System.Drawing.Size(439, 194);
            this.dgCategories.TabIndex = 22;
            this.dgCategories.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellDblClick);
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
            this.btnCategorize.Location = new System.Drawing.Point(282, 75);
            this.btnCategorize.Name = "btnCategorize";
            this.btnCategorize.Size = new System.Drawing.Size(90, 25);
            this.btnCategorize.TabIndex = 17;
            this.btnCategorize.Text = "Categorize";
            this.btnCategorize.UseVisualStyleBackColor = true;
            this.btnCategorize.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // txtCategoryCount
            // 
            this.txtCategoryCount.Location = new System.Drawing.Point(126, 78);
            this.txtCategoryCount.Name = "txtCategoryCount";
            this.txtCategoryCount.Size = new System.Drawing.Size(146, 20);
            this.txtCategoryCount.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 81);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(108, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Number of categories";
            // 
            // tabExtract
            // 
            this.tabExtract.Controls.Add(this.btnExport);
            this.tabExtract.Controls.Add(this.graphSheet);
            this.tabExtract.Controls.Add(this.btnDown);
            this.tabExtract.Controls.Add(this.btnUp);
            this.tabExtract.Controls.Add(this.dgSheetSummary);
            this.tabExtract.Controls.Add(this.lblMappedSheet);
            this.tabExtract.Controls.Add(this.listSelectedSheets);
            this.tabExtract.Controls.Add(this.label7);
            this.tabExtract.Location = new System.Drawing.Point(4, 22);
            this.tabExtract.Name = "tabExtract";
            this.tabExtract.Padding = new System.Windows.Forms.Padding(3);
            this.tabExtract.Size = new System.Drawing.Size(462, 418);
            this.tabExtract.TabIndex = 2;
            this.tabExtract.Text = "Mapping";
            this.tabExtract.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(23, 353);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(121, 25);
            this.btnExport.TabIndex = 22;
            this.btnExport.Text = "Export to time series";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // graphSheet
            // 
            chartArea2.AxisX.LabelStyle.Enabled = false;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.MajorTickMark.Enabled = false;
            chartArea2.AxisY.IsLabelAutoFit = false;
            chartArea2.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.Name = "ChartArea1";
            chartArea2.Position.Auto = false;
            chartArea2.Position.Height = 94F;
            chartArea2.Position.Width = 100F;
            this.graphSheet.ChartAreas.Add(chartArea2);
            legend2.Enabled = false;
            legend2.Name = "Legend1";
            this.graphSheet.Legends.Add(legend2);
            this.graphSheet.Location = new System.Drawing.Point(189, 273);
            this.graphSheet.Name = "graphSheet";
            series2.ChartArea = "ChartArea1";
            series2.IsVisibleInLegend = false;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.graphSheet.Series.Add(series2);
            this.graphSheet.Size = new System.Drawing.Size(265, 105);
            this.graphSheet.TabIndex = 24;
            this.graphSheet.Text = "chart1";
            // 
            // btnDown
            // 
            this.btnDown.ImageKey = "down";
            this.btnDown.ImageList = this.imageList1;
            this.btnDown.Location = new System.Drawing.Point(164, 154);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(30, 30);
            this.btnDown.TabIndex = 23;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            this.imageList1.Images.SetKeyName(0, "down");
            this.imageList1.Images.SetKeyName(1, "up");
            // 
            // btnUp
            // 
            this.btnUp.ImageKey = "up";
            this.btnUp.ImageList = this.imageList1;
            this.btnUp.Location = new System.Drawing.Point(164, 109);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(30, 30);
            this.btnUp.TabIndex = 22;
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
            this.dgSheetSummary.Location = new System.Drawing.Point(200, 32);
            this.dgSheetSummary.Name = "dgSheetSummary";
            this.dgSheetSummary.ReadOnly = true;
            this.dgSheetSummary.RowHeadersVisible = false;
            this.dgSheetSummary.Size = new System.Drawing.Size(254, 235);
            this.dgSheetSummary.TabIndex = 19;
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
            this.lblMappedSheet.AutoSize = true;
            this.lblMappedSheet.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMappedSheet.Location = new System.Drawing.Point(270, 381);
            this.lblMappedSheet.Name = "lblMappedSheet";
            this.lblMappedSheet.Size = new System.Drawing.Size(113, 18);
            this.lblMappedSheet.TabIndex = 18;
            this.lblMappedSheet.Text = "Mapped sheet";
            this.lblMappedSheet.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // listSelectedSheets
            // 
            this.listSelectedSheets.FormattingEnabled = true;
            this.listSelectedSheets.Location = new System.Drawing.Point(15, 32);
            this.listSelectedSheets.Name = "listSelectedSheets";
            this.listSelectedSheets.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listSelectedSheets.Size = new System.Drawing.Size(143, 303);
            this.listSelectedSheets.TabIndex = 17;
            this.listSelectedSheets.Click += new System.EventHandler(this.OnSelectedSheetsClick);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Sheets";
            // 
            // btnInstructions
            // 
            this.btnInstructions.Location = new System.Drawing.Point(7, 509);
            this.btnInstructions.Name = "btnInstructions";
            this.btnInstructions.Size = new System.Drawing.Size(77, 25);
            this.btnInstructions.TabIndex = 21;
            this.btnInstructions.Text = "Instructions";
            this.btnInstructions.UseVisualStyleBackColor = true;
            this.btnInstructions.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(421, 509);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(45, 25);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(365, 509);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // txtInlandPoints
            // 
            this.txtInlandPoints.BackColor = System.Drawing.SystemColors.Window;
            this.txtInlandPoints.Enabled = false;
            this.txtInlandPoints.Location = new System.Drawing.Point(299, 213);
            this.txtInlandPoints.Name = "txtInlandPoints";
            this.txtInlandPoints.ReadOnly = true;
            this.txtInlandPoints.Size = new System.Drawing.Size(113, 20);
            this.txtInlandPoints.TabIndex = 23;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(179, 216);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(67, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "Inland points";
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
            this.icbColorScheme.Location = new System.Drawing.Point(126, 16);
            this.icbColorScheme.Name = "icbColorScheme";
            this.icbColorScheme.OutlineColor = System.Drawing.Color.Black;
            this.icbColorScheme.Size = new System.Drawing.Size(146, 21);
            this.icbColorScheme.TabIndex = 24;
            // 
            // cboClassificationScheme
            // 
            this.cboClassificationScheme.FormattingEnabled = true;
            this.cboClassificationScheme.Location = new System.Drawing.Point(126, 47);
            this.cboClassificationScheme.Name = "cboClassificationScheme";
            this.cboClassificationScheme.Size = new System.Drawing.Size(145, 21);
            this.cboClassificationScheme.TabIndex = 29;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "Classification scheme";
            // 
            // SpatioTemporalMappingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 541);
            this.Controls.Add(this.btnInstructions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabsPages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SpatioTemporalMappingForm";
            this.ShowInTaskbar = false;
            this.Text = "ChlorophyllForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.tabsPages.ResumeLayout(false);
            this.tabUI.ResumeLayout(false);
            this.tabUI.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabCategorize.ResumeLayout(false);
            this.tabCategorize.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCategories)).EndInit();
            this.tabExtract.ResumeLayout(false);
            this.tabExtract.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.graphSheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSheetSummary)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabsPages;
        private System.Windows.Forms.TabPage tabUI;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ComboBox cboLatitude;
        private System.Windows.Forms.ComboBox cboLongitude;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkHasHeader;
        private System.Windows.Forms.TextBox txtRows;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabExtract;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox listSheets;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnReadSheet;
        private System.Windows.Forms.Button btnReadWorkbook;
        private System.Windows.Forms.ComboBox cboLastData;
        private System.Windows.Forms.ComboBox cboFirstData;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabCategorize;
        private System.Windows.Forms.Button btnShowGridPoints;
        private System.Windows.Forms.Button btnCategorize;
        private System.Windows.Forms.TextBox txtCategoryCount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListBox listSelectedSheets;
        private System.Windows.Forms.Label lblMappedSheet;
        private System.Windows.Forms.DataGridView dgCategories;
        private System.Windows.Forms.Button btnColorScheme;
        private System.Windows.Forms.Label label13;
        private UserControls.ImageCombo icbColorScheme;
        private System.Windows.Forms.Button btnInstructions;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnShowGridPolygons;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMaximum;
        private System.Windows.Forms.TextBox txtMinimum;
        private System.Windows.Forms.TextBox txtValuesCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView dgSheetSummary;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.DataVisualization.Charting.Chart graphSheet;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSymbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcolCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcolCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgCol;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TextBox txtInlandPoints;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboClassificationScheme;
    }
}