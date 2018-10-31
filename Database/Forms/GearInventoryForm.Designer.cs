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
            this.lvInventory = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
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
            this.split.Panel2.Controls.Add(this.lvInventory);
            this.split.Size = new System.Drawing.Size(896, 521);
            this.split.SplitterDistance = 298;
            this.split.TabIndex = 2;
            // 
            // treeInventory
            // 
            this.treeInventory.ContextMenuStrip = this.contextMenu;
            this.treeInventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeInventory.Location = new System.Drawing.Point(0, 0);
            this.treeInventory.Name = "treeInventory";
            this.treeInventory.Size = new System.Drawing.Size(298, 521);
            this.treeInventory.TabIndex = 1;
            this.treeInventory.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeAfterExpand);
            this.treeInventory.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeClicked);
            // 
            // lvInventory
            // 
            this.lvInventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvInventory.Location = new System.Drawing.Point(0, 0);
            this.lvInventory.Name = "lvInventory";
            this.lvInventory.Size = new System.Drawing.Size(594, 521);
            this.lvInventory.TabIndex = 2;
            this.lvInventory.UseCompatibleStateImageBehavior = false;
            // 
            // GearInventoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 557);
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
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.TreeView treeInventory;
        private System.Windows.Forms.ListView lvInventory;
    }
}