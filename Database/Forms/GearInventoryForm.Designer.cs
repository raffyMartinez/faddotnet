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
            this.treeInventory = new System.Windows.Forms.TreeView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lvInventory = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // treeInventory
            // 
            this.treeInventory.ContextMenuStrip = this.contextMenu;
            this.treeInventory.Location = new System.Drawing.Point(0, 32);
            this.treeInventory.Name = "treeInventory";
            this.treeInventory.Size = new System.Drawing.Size(199, 418);
            this.treeInventory.TabIndex = 0;
            this.treeInventory.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeAfterExpand);
            this.treeInventory.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeClicked);
            // 
            // contextMenu
            // 
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(61, 4);
            this.contextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnContextMenu_ItemClicked);
            // 
            // listViewInventory
            // 
            this.lvInventory.Location = new System.Drawing.Point(205, 32);
            this.lvInventory.Name = "listViewInventory";
            this.lvInventory.Size = new System.Drawing.Size(421, 418);
            this.lvInventory.TabIndex = 1;
            this.lvInventory.UseCompatibleStateImageBehavior = false;
            // 
            // GearInventoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 450);
            this.Controls.Add(this.lvInventory);
            this.Controls.Add(this.treeInventory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GearInventoryForm";
            this.Text = "GearInventoryForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeInventory;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ListView lvInventory;
    }
}