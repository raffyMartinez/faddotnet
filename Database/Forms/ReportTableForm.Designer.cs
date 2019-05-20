namespace FAD3.Database.Forms
{
    partial class ReportTableForm
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.btnCopyText = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.lvTable = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuitemCopyText = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExportToExcel,
            this.btnCopyText,
            this.toolStripSeparator1,
            this.btnClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(421, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolbarItemClicked);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExportToExcel.Image = global::FAD3.Properties.Resources.ExportToExcel_16x;
            this.btnExportToExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(23, 22);
            this.btnExportToExcel.Text = "toolStripButton1";
            this.btnExportToExcel.ToolTipText = "Export to Excel";
            // 
            // btnCopyText
            // 
            this.btnCopyText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCopyText.Image = global::FAD3.Properties.Resources.CopyLongTextToClipboard_16x;
            this.btnCopyText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyText.Name = "btnCopyText";
            this.btnCopyText.Size = new System.Drawing.Size(23, 22);
            this.btnCopyText.Text = "toolStripButton3";
            this.btnCopyText.ToolTipText = "Copy text";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClose
            // 
            this.btnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClose.Image = global::FAD3.Properties.Resources.im_exit;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(23, 22);
            this.btnClose.Text = "toolStripButton2";
            this.btnClose.ToolTipText = "Close";
            // 
            // lvTable
            // 
            this.lvTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvTable.ContextMenuStrip = this.contextMenuStrip1;
            this.lvTable.Location = new System.Drawing.Point(0, 28);
            this.lvTable.Name = "lvTable";
            this.lvTable.Size = new System.Drawing.Size(421, 424);
            this.lvTable.TabIndex = 1;
            this.lvTable.UseCompatibleStateImageBehavior = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuitemCopyText,
            this.toolStripSeparator2,
            this.menuItemClose});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 54);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnMenuItemClicked);
            // 
            // menuitemCopyText
            // 
            this.menuitemCopyText.Image = global::FAD3.Properties.Resources.CopyLongTextToClipboard_16x;
            this.menuitemCopyText.Name = "menuitemCopyText";
            this.menuitemCopyText.Size = new System.Drawing.Size(180, 22);
            this.menuitemCopyText.Text = "Copy text";
            // 
            // menuItemClose
            // 
            this.menuItemClose.Image = global::FAD3.Properties.Resources.im_exit;
            this.menuItemClose.Name = "menuItemClose";
            this.menuItemClose.Size = new System.Drawing.Size(180, 22);
            this.menuItemClose.Text = "Close";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // ReportTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 450);
            this.Controls.Add(this.lvTable);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ReportTableForm";
            this.Text = "ReportTableForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReportTableForm_FormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ListView lvTable;
        private System.Windows.Forms.ToolStripButton btnExportToExcel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuitemCopyText;
        private System.Windows.Forms.ToolStripButton btnCopyText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemClose;
    }
}