﻿namespace FAD3.Mapping.Forms
{
    partial class ChlorophyllForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChlorophyllForm));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabsPages = new System.Windows.Forms.TabControl();
            this.tabInstructions = new System.Windows.Forms.TabPage();
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
            this.btnCategorize = new System.Windows.Forms.Button();
            this.txtCategoryCount = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnShowGrid = new System.Windows.Forms.Button();
            this.txtValuesCount = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lvBreaks = new System.Windows.Forms.ListView();
            this.colCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabExtract = new System.Windows.Forms.TabPage();
            this.btnMapSelected = new System.Windows.Forms.Button();
            this.listSelectedSheets = new System.Windows.Forms.CheckedListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDefineCell = new System.Windows.Forms.Button();
            this.tabsPages.SuspendLayout();
            this.tabInstructions.SuspendLayout();
            this.tabUI.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabCategorize.SuspendLayout();
            this.tabExtract.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(454, 293);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // tabsPages
            // 
            this.tabsPages.Controls.Add(this.tabInstructions);
            this.tabsPages.Controls.Add(this.tabUI);
            this.tabsPages.Controls.Add(this.tabCategorize);
            this.tabsPages.Controls.Add(this.tabExtract);
            this.tabsPages.Location = new System.Drawing.Point(3, 22);
            this.tabsPages.Name = "tabsPages";
            this.tabsPages.SelectedIndex = 0;
            this.tabsPages.Size = new System.Drawing.Size(470, 376);
            this.tabsPages.TabIndex = 1;
            // 
            // tabInstructions
            // 
            this.tabInstructions.Controls.Add(this.richTextBox1);
            this.tabInstructions.Location = new System.Drawing.Point(4, 22);
            this.tabInstructions.Name = "tabInstructions";
            this.tabInstructions.Padding = new System.Windows.Forms.Padding(3);
            this.tabInstructions.Size = new System.Drawing.Size(462, 350);
            this.tabInstructions.TabIndex = 0;
            this.tabInstructions.Text = "Instructions";
            this.tabInstructions.UseVisualStyleBackColor = true;
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
            this.tabUI.Size = new System.Drawing.Size(462, 350);
            this.tabUI.TabIndex = 1;
            this.tabUI.Text = "Start";
            this.tabUI.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
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
            this.groupBox1.Location = new System.Drawing.Point(9, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 223);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // cboLastData
            // 
            this.cboLastData.FormattingEnabled = true;
            this.cboLastData.Location = new System.Drawing.Point(304, 139);
            this.cboLastData.Name = "cboLastData";
            this.cboLastData.Size = new System.Drawing.Size(114, 21);
            this.cboLastData.TabIndex = 21;
            this.cboLastData.SelectedIndexChanged += new System.EventHandler(this.OnComboIndexChanged);
            // 
            // cboFirstData
            // 
            this.cboFirstData.FormattingEnabled = true;
            this.cboFirstData.Location = new System.Drawing.Point(305, 103);
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
            this.txtRows.Location = new System.Drawing.Point(305, 176);
            this.txtRows.Name = "txtRows";
            this.txtRows.ReadOnly = true;
            this.txtRows.Size = new System.Drawing.Size(113, 20);
            this.txtRows.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(185, 179);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Data row count";
            // 
            // cboLatitude
            // 
            this.cboLatitude.FormattingEnabled = true;
            this.cboLatitude.Location = new System.Drawing.Point(304, 66);
            this.cboLatitude.Name = "cboLatitude";
            this.cboLatitude.Size = new System.Drawing.Size(114, 21);
            this.cboLatitude.TabIndex = 11;
            this.cboLatitude.SelectedIndexChanged += new System.EventHandler(this.OnComboIndexChanged);
            // 
            // cboLongitude
            // 
            this.cboLongitude.FormattingEnabled = true;
            this.cboLongitude.Location = new System.Drawing.Point(304, 29);
            this.cboLongitude.Name = "cboLongitude";
            this.cboLongitude.Size = new System.Drawing.Size(114, 21);
            this.cboLongitude.TabIndex = 10;
            this.cboLongitude.SelectedIndexChanged += new System.EventHandler(this.OnComboIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(184, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Last data column";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(184, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "First data column";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Latitude data column";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(184, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Longitude data column";
            // 
            // btnReadWorkbook
            // 
            this.btnReadWorkbook.Location = new System.Drawing.Point(260, 68);
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
            this.txtFile.Location = new System.Drawing.Point(75, 25);
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(379, 20);
            this.txtFile.TabIndex = 16;
            // 
            // chkHasHeader
            // 
            this.chkHasHeader.AutoSize = true;
            this.chkHasHeader.Location = new System.Drawing.Point(91, 73);
            this.chkHasHeader.Name = "chkHasHeader";
            this.chkHasHeader.Size = new System.Drawing.Size(144, 17);
            this.chkHasHeader.TabIndex = 5;
            this.chkHasHeader.Text = "First row contain headers";
            this.chkHasHeader.UseVisualStyleBackColor = true;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(19, 22);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(50, 25);
            this.btnOpen.TabIndex = 4;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // tabCategorize
            // 
            this.tabCategorize.Controls.Add(this.btnCategorize);
            this.tabCategorize.Controls.Add(this.txtCategoryCount);
            this.tabCategorize.Controls.Add(this.label10);
            this.tabCategorize.Controls.Add(this.btnShowGrid);
            this.tabCategorize.Controls.Add(this.txtValuesCount);
            this.tabCategorize.Controls.Add(this.label9);
            this.tabCategorize.Controls.Add(this.label8);
            this.tabCategorize.Controls.Add(this.lvBreaks);
            this.tabCategorize.Location = new System.Drawing.Point(4, 22);
            this.tabCategorize.Name = "tabCategorize";
            this.tabCategorize.Padding = new System.Windows.Forms.Padding(3);
            this.tabCategorize.Size = new System.Drawing.Size(462, 350);
            this.tabCategorize.TabIndex = 3;
            this.tabCategorize.Text = "Categories";
            this.tabCategorize.UseVisualStyleBackColor = true;
            // 
            // btnCategorize
            // 
            this.btnCategorize.Location = new System.Drawing.Point(145, 47);
            this.btnCategorize.Name = "btnCategorize";
            this.btnCategorize.Size = new System.Drawing.Size(67, 25);
            this.btnCategorize.TabIndex = 17;
            this.btnCategorize.Text = "Categorize";
            this.btnCategorize.UseVisualStyleBackColor = true;
            this.btnCategorize.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // txtCategoryCount
            // 
            this.txtCategoryCount.Location = new System.Drawing.Point(148, 21);
            this.txtCategoryCount.Name = "txtCategoryCount";
            this.txtCategoryCount.Size = new System.Drawing.Size(64, 20);
            this.txtCategoryCount.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(108, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Number of categories";
            // 
            // btnShowGrid
            // 
            this.btnShowGrid.Location = new System.Drawing.Point(346, 301);
            this.btnShowGrid.Name = "btnShowGrid";
            this.btnShowGrid.Size = new System.Drawing.Size(97, 25);
            this.btnShowGrid.TabIndex = 13;
            this.btnShowGrid.Text = "Map data points";
            this.btnShowGrid.UseVisualStyleBackColor = true;
            this.btnShowGrid.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // txtValuesCount
            // 
            this.txtValuesCount.Location = new System.Drawing.Point(130, 253);
            this.txtValuesCount.Name = "txtValuesCount";
            this.txtValuesCount.Size = new System.Drawing.Size(64, 20);
            this.txtValuesCount.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 256);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Number of values";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Categories";
            // 
            // lvBreaks
            // 
            this.lvBreaks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCategory,
            this.colCount});
            this.lvBreaks.FullRowSelect = true;
            this.lvBreaks.Location = new System.Drawing.Point(19, 106);
            this.lvBreaks.Name = "lvBreaks";
            this.lvBreaks.ShowItemToolTips = true;
            this.lvBreaks.Size = new System.Drawing.Size(424, 141);
            this.lvBreaks.TabIndex = 9;
            this.lvBreaks.UseCompatibleStateImageBehavior = false;
            this.lvBreaks.View = System.Windows.Forms.View.Details;
            // 
            // colCategory
            // 
            this.colCategory.Text = "Category";
            this.colCategory.Width = 0;
            // 
            // colCount
            // 
            this.colCount.Text = "Count";
            // 
            // tabExtract
            // 
            this.tabExtract.Controls.Add(this.btnDefineCell);
            this.tabExtract.Controls.Add(this.btnMapSelected);
            this.tabExtract.Controls.Add(this.listSelectedSheets);
            this.tabExtract.Controls.Add(this.label7);
            this.tabExtract.Location = new System.Drawing.Point(4, 22);
            this.tabExtract.Name = "tabExtract";
            this.tabExtract.Padding = new System.Windows.Forms.Padding(3);
            this.tabExtract.Size = new System.Drawing.Size(462, 350);
            this.tabExtract.TabIndex = 2;
            this.tabExtract.Text = "Mapping";
            this.tabExtract.UseVisualStyleBackColor = true;
            // 
            // btnMapSelected
            // 
            this.btnMapSelected.Location = new System.Drawing.Point(240, 156);
            this.btnMapSelected.Name = "btnMapSelected";
            this.btnMapSelected.Size = new System.Drawing.Size(100, 39);
            this.btnMapSelected.TabIndex = 15;
            this.btnMapSelected.Text = "Map selected sheets";
            this.btnMapSelected.UseVisualStyleBackColor = true;
            this.btnMapSelected.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // listSelectedSheets
            // 
            this.listSelectedSheets.FormattingEnabled = true;
            this.listSelectedSheets.Location = new System.Drawing.Point(15, 32);
            this.listSelectedSheets.Name = "listSelectedSheets";
            this.listSelectedSheets.Size = new System.Drawing.Size(137, 304);
            this.listSelectedSheets.TabIndex = 4;
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
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(416, 404);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(45, 25);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(360, 404);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 25);
            this.button1.TabIndex = 3;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnDefineCell
            // 
            this.btnDefineCell.Location = new System.Drawing.Point(240, 101);
            this.btnDefineCell.Name = "btnDefineCell";
            this.btnDefineCell.Size = new System.Drawing.Size(100, 39);
            this.btnDefineCell.TabIndex = 16;
            this.btnDefineCell.Text = "Define grid shape";
            this.btnDefineCell.UseVisualStyleBackColor = true;
            this.btnDefineCell.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // ChlorophyllForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 437);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabsPages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ChlorophyllForm";
            this.Text = "ChlorophyllForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.tabsPages.ResumeLayout(false);
            this.tabInstructions.ResumeLayout(false);
            this.tabUI.ResumeLayout(false);
            this.tabUI.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabCategorize.ResumeLayout(false);
            this.tabCategorize.PerformLayout();
            this.tabExtract.ResumeLayout(false);
            this.tabExtract.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TabControl tabsPages;
        private System.Windows.Forms.TabPage tabInstructions;
        private System.Windows.Forms.TabPage tabUI;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button button1;
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
        private System.Windows.Forms.CheckedListBox listSelectedSheets;
        private System.Windows.Forms.TabPage tabCategorize;
        private System.Windows.Forms.TextBox txtValuesCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListView lvBreaks;
        private System.Windows.Forms.ColumnHeader colCategory;
        private System.Windows.Forms.ColumnHeader colCount;
        private System.Windows.Forms.Button btnShowGrid;
        private System.Windows.Forms.Button btnCategorize;
        private System.Windows.Forms.TextBox txtCategoryCount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnMapSelected;
        private System.Windows.Forms.Button btnDefineCell;
    }
}