namespace FAD3.Database.Forms
{
    partial class DatabaseReportForm
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
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.tsbExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tvTopics = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.lvReports = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.lvYears = new System.Windows.Forms.ListView();
            this.toolBar.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolBar
            // 
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbExcel,
            this.toolStripSeparator1,
            this.tsbClose});
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(759, 25);
            this.toolBar.TabIndex = 1;
            this.toolBar.Text = "toolStrip1";
            this.toolBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolbarItemClicked);
            // 
            // tsbExcel
            // 
            this.tsbExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExcel.Image = global::FAD3.Properties.Resources.ExportToExcel_16x;
            this.tsbExcel.ImageTransparentColor = System.Drawing.Color.White;
            this.tsbExcel.Name = "tsbExcel";
            this.tsbExcel.Size = new System.Drawing.Size(23, 22);
            this.tsbExcel.Text = "toolStripButton1";
            this.tsbExcel.ToolTipText = "Export to Excel";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClose.Image = global::FAD3.Properties.Resources.im_exit;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(23, 22);
            this.tsbClose.Text = "toolStripButton2";
            this.tsbClose.ToolTipText = "Close";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tvTopics, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lvReports, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lvYears, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(759, 441);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Topic";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // tvTopics
            // 
            this.tvTopics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvTopics.Location = new System.Drawing.Point(3, 23);
            this.tvTopics.Name = "tvTopics";
            this.tvTopics.Size = new System.Drawing.Size(194, 254);
            this.tvTopics.TabIndex = 1;
            this.tvTopics.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnTreeNodeClick);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 280);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Years";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lvReports
            // 
            this.lvReports.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvReports.Location = new System.Drawing.Point(203, 23);
            this.lvReports.Name = "lvReports";
            this.tableLayoutPanel1.SetRowSpan(this.lvReports, 3);
            this.lvReports.Size = new System.Drawing.Size(553, 415);
            this.lvReports.TabIndex = 4;
            this.lvReports.UseCompatibleStateImageBehavior = false;
            this.lvReports.DoubleClick += new System.EventHandler(this.OnListDoubleClick);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(203, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(553, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Reports";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lvYears
            // 
            this.lvYears.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvYears.CheckBoxes = true;
            this.lvYears.Location = new System.Drawing.Point(3, 303);
            this.lvYears.Name = "lvYears";
            this.lvYears.Size = new System.Drawing.Size(194, 135);
            this.lvYears.TabIndex = 6;
            this.lvYears.UseCompatibleStateImageBehavior = false;
            this.lvYears.View = System.Windows.Forms.View.Details;
            // 
            // DatabaseReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 469);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "DatabaseReportForm";
            this.Text = "DatabaseReportForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView tvTopics;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lvReports;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripButton tsbExcel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ListView lvYears;
    }
}