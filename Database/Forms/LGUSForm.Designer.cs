namespace FAD3.Database.Forms
{
    partial class LGUSForm
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
            this.lv = new System.Windows.Forms.ListView();
            this.treeLGUs = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // lv
            // 
            this.lv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lv.ContextMenuStrip = this.contextMenu;
            this.lv.Location = new System.Drawing.Point(253, 31);
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(345, 419);
            this.lv.TabIndex = 0;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // treeLGUs
            // 
            this.treeLGUs.AllowDrop = true;
            this.treeLGUs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeLGUs.ContextMenuStrip = this.contextMenu;
            this.treeLGUs.Location = new System.Drawing.Point(1, 31);
            this.treeLGUs.Name = "treeLGUs";
            this.treeLGUs.Size = new System.Drawing.Size(250, 419);
            this.treeLGUs.TabIndex = 1;
            this.treeLGUs.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.OnItemDrag);
            this.treeLGUs.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnTreeNodeClick);
            this.treeLGUs.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.treeLGUs.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this.treeLGUs.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
            this.treeLGUs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(599, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // contextMenu
            // 
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(181, 26);
            this.contextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnMenuItemClicked);
            // 
            // LGUSForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 450);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.treeLGUs);
            this.Controls.Add(this.lv);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "LGUSForm";
            this.Text = "LGUSForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lv;
        private System.Windows.Forms.TreeView treeLGUs;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
    }
}