namespace FAD3.Mapping.Forms
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
            this.btnOk = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.chkHasHeader = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboLongitude = new System.Windows.Forms.ComboBox();
            this.cboLatitude = new System.Windows.Forms.ComboBox();
            this.txtRows = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPreview = new System.Windows.Forms.TabPage();
            this.sheetGrid = new System.Windows.Forms.DataGridView();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.listSheets = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnReadWorkbook = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnReadSheet = new System.Windows.Forms.Button();
            this.cboFirstData = new System.Windows.Forms.ComboBox();
            this.cboLastData = new System.Windows.Forms.ComboBox();
            this.tabsPages.SuspendLayout();
            this.tabInstructions.SuspendLayout();
            this.tabUI.SuspendLayout();
            this.tabPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            this.tabsPages.Controls.Add(this.tabPreview);
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
            this.tabInstructions.Size = new System.Drawing.Size(462, 316);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(184, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Longitude data column";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(184, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "First data column";
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
            // cboLongitude
            // 
            this.cboLongitude.FormattingEnabled = true;
            this.cboLongitude.Location = new System.Drawing.Point(304, 29);
            this.cboLongitude.Name = "cboLongitude";
            this.cboLongitude.Size = new System.Drawing.Size(114, 21);
            this.cboLongitude.TabIndex = 10;
            // 
            // cboLatitude
            // 
            this.cboLatitude.FormattingEnabled = true;
            this.cboLatitude.Location = new System.Drawing.Point(304, 66);
            this.cboLatitude.Name = "cboLatitude";
            this.cboLatitude.Size = new System.Drawing.Size(114, 21);
            this.cboLatitude.TabIndex = 11;
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
            // tabPreview
            // 
            this.tabPreview.Controls.Add(this.sheetGrid);
            this.tabPreview.Location = new System.Drawing.Point(4, 22);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPreview.Size = new System.Drawing.Size(462, 316);
            this.tabPreview.TabIndex = 2;
            this.tabPreview.Text = "Preview";
            this.tabPreview.UseVisualStyleBackColor = true;
            // 
            // sheetGrid
            // 
            this.sheetGrid.AllowUserToAddRows = false;
            this.sheetGrid.AllowUserToDeleteRows = false;
            this.sheetGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sheetGrid.Location = new System.Drawing.Point(6, 18);
            this.sheetGrid.Name = "sheetGrid";
            this.sheetGrid.ReadOnly = true;
            this.sheetGrid.Size = new System.Drawing.Size(448, 292);
            this.sheetGrid.TabIndex = 0;
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
            // listSheets
            // 
            this.listSheets.FormattingEnabled = true;
            this.listSheets.Location = new System.Drawing.Point(10, 30);
            this.listSheets.Name = "listSheets";
            this.listSheets.Size = new System.Drawing.Size(153, 147);
            this.listSheets.TabIndex = 17;
            this.listSheets.Click += new System.EventHandler(this.OnListBoxClick);
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
            // cboFirstData
            // 
            this.cboFirstData.FormattingEnabled = true;
            this.cboFirstData.Location = new System.Drawing.Point(305, 103);
            this.cboFirstData.Name = "cboFirstData";
            this.cboFirstData.Size = new System.Drawing.Size(114, 21);
            this.cboFirstData.TabIndex = 20;
            // 
            // cboLastData
            // 
            this.cboLastData.FormattingEnabled = true;
            this.cboLastData.Location = new System.Drawing.Point(304, 139);
            this.cboLastData.Name = "cboLastData";
            this.cboLastData.Size = new System.Drawing.Size(114, 21);
            this.cboLastData.TabIndex = 21;
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
            this.tabPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sheetGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.TabPage tabPreview;
        private System.Windows.Forms.DataGridView sheetGrid;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox listSheets;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnReadSheet;
        private System.Windows.Forms.Button btnReadWorkbook;
        private System.Windows.Forms.ComboBox cboLastData;
        private System.Windows.Forms.ComboBox cboFirstData;
    }
}