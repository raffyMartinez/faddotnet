namespace FAD3.Database.Forms
{
    partial class GearInventoryForm
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
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.split = new System.Windows.Forms.SplitContainer();
            this.treeInventory = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lvInventory = new System.Windows.Forms.ListView();
            this.lblGuide = new System.Windows.Forms.Label();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsButtonAddInventory = new System.Windows.Forms.ToolStripButton();
            this.tsButtonAddSitioLevelInventory = new System.Windows.Forms.ToolStripButton();
            this.tsButtonAddGear = new System.Windows.Forms.ToolStripButton();
            this.tsButtonExit = new System.Windows.Forms.ToolStripButton();
            this.tsButtonExport = new System.Windows.Forms.ToolStripButton();
            this.tsButtonImport = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(61, 4);
            this.contextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnContextMenu_ItemClicked);
            // 
            // split
            // 
            this.split.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.split.Location = new System.Drawing.Point(1, 35);
            this.split.Name = "split";
            // 
            // split.Panel1
            // 
            this.split.Panel1.Controls.Add(this.treeInventory);
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.split.Size = new System.Drawing.Size(896, 521);
            this.split.SplitterDistance = 298;
            this.split.TabIndex = 2;
            // 
            // treeInventory
            // 
            this.treeInventory.ContextMenuStrip = this.contextMenu;
            this.treeInventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeInventory.HideSelection = false;
            this.treeInventory.Location = new System.Drawing.Point(0, 0);
            this.treeInventory.Name = "treeInventory";
            this.treeInventory.Size = new System.Drawing.Size(298, 521);
            this.treeInventory.TabIndex = 1;
            this.treeInventory.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeAfterExpand);
            this.treeInventory.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeClicked);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lvInventory, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblGuide, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(594, 521);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // lvInventory
            // 
            this.lvInventory.ContextMenuStrip = this.contextMenu;
            this.lvInventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvInventory.Location = new System.Drawing.Point(3, 23);
            this.lvInventory.Name = "lvInventory";
            this.lvInventory.Size = new System.Drawing.Size(588, 495);
            this.lvInventory.TabIndex = 4;
            this.lvInventory.UseCompatibleStateImageBehavior = false;
            this.lvInventory.DoubleClick += new System.EventHandler(this.OnListViewDoubleClick);
            this.lvInventory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // lblGuide
            // 
            this.lblGuide.AutoSize = true;
            this.lblGuide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGuide.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuide.Location = new System.Drawing.Point(3, 0);
            this.lblGuide.Name = "lblGuide";
            this.lblGuide.Size = new System.Drawing.Size(588, 20);
            this.lblGuide.TabIndex = 3;
            this.lblGuide.Text = "label1";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButtonAddInventory,
            this.tsButtonAddSitioLevelInventory,
            this.tsButtonAddGear,
            this.tsButtonExport,
            this.tsButtonImport,
            this.tsButtonExit});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(898, 25);
            this.toolStrip.TabIndex = 3;
            this.toolStrip.Text = "toolStrip1";
            this.toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolStripItemClicked);
            // 
            // tsButtonAddInventory
            // 
            this.tsButtonAddInventory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonAddInventory.Image = global::FAD3.Properties.Resources.AddFolder_16x;
            this.tsButtonAddInventory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButtonAddInventory.Name = "tsButtonAddInventory";
            this.tsButtonAddInventory.Size = new System.Drawing.Size(23, 22);
            this.tsButtonAddInventory.Text = "toolStripButton1";
            this.tsButtonAddInventory.ToolTipText = "Add fishery inventory";
            // 
            // tsButtonAddSitioLevelInventory
            // 
            this.tsButtonAddSitioLevelInventory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonAddSitioLevelInventory.Image = global::FAD3.Properties.Resources.AddFile_16x;
            this.tsButtonAddSitioLevelInventory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButtonAddSitioLevelInventory.Name = "tsButtonAddSitioLevelInventory";
            this.tsButtonAddSitioLevelInventory.Size = new System.Drawing.Size(23, 22);
            this.tsButtonAddSitioLevelInventory.Text = "toolStripButton1";
            this.tsButtonAddSitioLevelInventory.ToolTipText = "Add fisher and boat inventory";
            // 
            // tsButtonAddGear
            // 
            this.tsButtonAddGear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonAddGear.Image = global::FAD3.Properties.Resources.AddBehavior_16x;
            this.tsButtonAddGear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButtonAddGear.Name = "tsButtonAddGear";
            this.tsButtonAddGear.Size = new System.Drawing.Size(23, 22);
            this.tsButtonAddGear.Text = "toolStripButton1";
            this.tsButtonAddGear.ToolTipText = "Add fishing gear inventory";
            // 
            // tsButtonExit
            // 
            this.tsButtonExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonExit.Image = global::FAD3.Properties.Resources.im_exit;
            this.tsButtonExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButtonExit.Name = "tsButtonExit";
            this.tsButtonExit.Size = new System.Drawing.Size(23, 22);
            this.tsButtonExit.Text = "toolStripButton1";
            this.tsButtonExit.ToolTipText = "Exit";
            // 
            // tsButtonExport
            // 
            this.tsButtonExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonExport.Image = global::FAD3.Properties.Resources.ExportFile_16x;
            this.tsButtonExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButtonExport.Name = "tsButtonExport";
            this.tsButtonExport.Size = new System.Drawing.Size(23, 22);
            this.tsButtonExport.Text = "toolStripButton1";
            this.tsButtonExport.ToolTipText = "Export inventory";
            // 
            // tsButtonImport
            // 
            this.tsButtonImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonImport.Image = global::FAD3.Properties.Resources.ImportFile_16x;
            this.tsButtonImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButtonImport.Name = "tsButtonImport";
            this.tsButtonImport.Size = new System.Drawing.Size(23, 22);
            this.tsButtonImport.Text = "toolStripButton2";
            this.tsButtonImport.ToolTipText = "Import inventory";
            // 
            // GearInventoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 557);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.split);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "GearInventoryForm";
            this.Text = "GearInventoryForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.TreeView treeInventory;
        private System.Windows.Forms.Label lblGuide;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView lvInventory;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsButtonExit;
        private System.Windows.Forms.ToolStripButton tsButtonAddInventory;
        private System.Windows.Forms.ToolStripButton tsButtonAddSitioLevelInventory;
        private System.Windows.Forms.ToolStripButton tsButtonAddGear;
        private System.Windows.Forms.ToolStripButton tsButtonExport;
        private System.Windows.Forms.ToolStripButton tsButtonImport;
    }
}