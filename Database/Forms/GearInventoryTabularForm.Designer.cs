namespace FAD3.Database.Forms
{
    partial class GearInventoryTabularForm
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
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Gear local names");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Count");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Months of fishing");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Peak season months");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Months of operation and season", new System.Windows.Forms.TreeNode[] {
            treeNode17,
            treeNode18});
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("CPUE historical trends");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("CPUE", new System.Windows.Forms.TreeNode[] {
            treeNode20});
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Catch composition");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Accessories");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Expenses");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Notes");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Fisher and vessel inventory", new System.Windows.Forms.TreeNode[] {
            treeNode15,
            treeNode16,
            treeNode19,
            treeNode21,
            treeNode22,
            treeNode23,
            treeNode24,
            treeNode25});
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Respondents");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Inventory Project", new System.Windows.Forms.TreeNode[] {
            treeNode26,
            treeNode27});
            this.treeInventory = new System.Windows.Forms.TreeView();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.listResults = new System.Windows.Forms.ListView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeInventory
            // 
            this.treeInventory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeInventory.Location = new System.Drawing.Point(0, 28);
            this.treeInventory.Name = "treeInventory";
            treeNode15.Name = "nodeGear";
            treeNode15.Text = "Gear local names";
            treeNode16.Name = "nodeGearCount";
            treeNode16.Text = "Count";
            treeNode17.Name = "nodeMonths";
            treeNode17.Text = "Months of fishing";
            treeNode18.Name = "nodePeak";
            treeNode18.Text = "Peak season months";
            treeNode19.Name = "nodeGearOperation";
            treeNode19.Text = "Months of operation and season";
            treeNode20.Name = "nodeGearCPUEHistory";
            treeNode20.Text = "CPUE historical trends";
            treeNode21.Name = "nodeCPUE";
            treeNode21.Text = "CPUE";
            treeNode22.Name = "nodeCatchComp";
            treeNode22.Text = "Catch composition";
            treeNode23.Name = "nodeAccessories";
            treeNode23.Text = "Accessories";
            treeNode24.Name = "nodeExpenses";
            treeNode24.Text = "Expenses";
            treeNode25.Name = "nodeNotes";
            treeNode25.Text = "Notes";
            treeNode26.Name = "nodeFisherVessel";
            treeNode26.Text = "Fisher and vessel inventory";
            treeNode27.Name = "nodeRespondents";
            treeNode27.Text = "Respondents";
            treeNode28.Name = "nodeProject";
            treeNode28.Text = "Inventory Project";
            this.treeInventory.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode28});
            this.treeInventory.Size = new System.Drawing.Size(222, 425);
            this.treeInventory.TabIndex = 0;
            this.treeInventory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeAfterSelect);
            // 
            // toolBar
            // 
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbExport,
            this.tsbClose});
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(732, 25);
            this.toolBar.TabIndex = 2;
            this.toolBar.Text = "toolStrip1";
            this.toolBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolBarItemClicked);
            // 
            // tsbExport
            // 
            this.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExport.Image = global::FAD3.Properties.Resources.ExportFile_16x;
            this.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Size = new System.Drawing.Size(23, 22);
            this.tsbExport.Text = "toolStripButton1";
            this.tsbExport.ToolTipText = "Export";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClose.Image = global::FAD3.Properties.Resources.im_exit;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(23, 22);
            this.tsbClose.Text = "toolStripButton1";
            // 
            // listResults
            // 
            this.listResults.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.listResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listResults.FullRowSelect = true;
            this.listResults.Location = new System.Drawing.Point(226, 28);
            this.listResults.Name = "listResults";
            this.listResults.Size = new System.Drawing.Size(506, 425);
            this.listResults.TabIndex = 3;
            this.listResults.UseCompatibleStateImageBehavior = false;
            this.listResults.View = System.Windows.Forms.View.Details;
            this.listResults.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListMouseDown);
            // 
            // contextMenu
            // 
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(181, 26);
            this.contextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnMenuItemClicked);
            // 
            // GearInventoryTabularForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 453);
            this.Controls.Add(this.listResults);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.treeInventory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "GearInventoryTabularForm";
            this.Text = "GearInventoryTabular";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeInventory;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ListView listResults;
        private System.Windows.Forms.ToolStripButton tsbExport;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
    }
}