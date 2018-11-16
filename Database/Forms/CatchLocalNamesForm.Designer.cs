namespace FAD3.Database.Forms
{
    partial class CatchLocalNamesForm
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
            this.treeView = new System.Windows.Forms.TreeView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemLocateOnNavigationTree = new System.Windows.Forms.ToolStripMenuItem();
            this.itemBrowseWWW = new System.Windows.Forms.ToolStripMenuItem();
            this.itemWiki = new System.Windows.Forms.ToolStripMenuItem();
            this.itemFishBase = new System.Windows.Forms.ToolStripMenuItem();
            this.lblList = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTree = new System.Windows.Forms.Label();
            this.listView = new System.Windows.Forms.ListView();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.tbAdd = new System.Windows.Forms.ToolStripButton();
            this.tbRemove = new System.Windows.Forms.ToolStripButton();
            this.tbEdit = new System.Windows.Forms.ToolStripButton();
            this.tbExport = new System.Windows.Forms.ToolStripButton();
            this.tbImport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tbCombo = new System.Windows.Forms.ToolStripComboBox();
            this.contextMenu.SuspendLayout();
            this.toolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(1, 51);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(224, 296);
            this.treeView.TabIndex = 0;
            this.treeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.OnAfterLabelEdit);
            this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterExpand);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeClick);
            this.treeView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnControlMouseDown);
            this.treeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnTreeMouseUp);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemLocateOnNavigationTree,
            this.itemBrowseWWW});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(209, 48);
            this.contextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnContextMenuClick);
            // 
            // itemLocateOnNavigationTree
            // 
            this.itemLocateOnNavigationTree.Name = "itemLocateOnNavigationTree";
            this.itemLocateOnNavigationTree.Size = new System.Drawing.Size(208, 22);
            this.itemLocateOnNavigationTree.Text = "Locate on navigation tree";
            // 
            // itemBrowseWWW
            // 
            this.itemBrowseWWW.Name = "itemBrowseWWW";
            this.itemBrowseWWW.Size = new System.Drawing.Size(208, 22);
            // 
            // itemWiki
            // 
            this.itemWiki.Name = "itemWiki";
            this.itemWiki.Size = new System.Drawing.Size(32, 19);
            // 
            // itemFishBase
            // 
            this.itemFishBase.Name = "itemFishBase";
            this.itemFishBase.Size = new System.Drawing.Size(32, 19);
            // 
            // lblList
            // 
            this.lblList.AutoSize = true;
            this.lblList.Location = new System.Drawing.Point(231, 35);
            this.lblList.Name = "lblList";
            this.lblList.Size = new System.Drawing.Size(108, 13);
            this.lblList.TabIndex = 2;
            this.lblList.Text = "List of species names";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(640, 353);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(45, 24);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Location = new System.Drawing.Point(582, 353);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(52, 24);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblTree
            // 
            this.lblTree.AutoSize = true;
            this.lblTree.Location = new System.Drawing.Point(1, 34);
            this.lblTree.Name = "lblTree";
            this.lblTree.Size = new System.Drawing.Size(140, 13);
            this.lblTree.TabIndex = 5;
            this.lblTree.Text = "Local names and languages";
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(230, 51);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(461, 295);
            this.listView.TabIndex = 9;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.DoubleClick += new System.EventHandler(this.OnListViewDblClick);
            this.listView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnControlMouseDown);
            // 
            // toolBar
            // 
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbAdd,
            this.tbRemove,
            this.tbEdit,
            this.tbExport,
            this.tbImport,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.tbCombo});
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(694, 25);
            this.toolBar.TabIndex = 12;
            this.toolBar.Text = "toolStrip1";
            this.toolBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolBarItemClick);
            // 
            // tbAdd
            // 
            this.tbAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbAdd.Image = global::FAD3.Properties.Resources.Add_16xSM;
            this.tbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbAdd.Name = "tbAdd";
            this.tbAdd.Size = new System.Drawing.Size(23, 22);
            this.tbAdd.Text = "toolStripButton1";
            this.tbAdd.ToolTipText = "Add";
            // 
            // tbRemove
            // 
            this.tbRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbRemove.Image = global::FAD3.Properties.Resources.Remove_16xSM;
            this.tbRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRemove.Name = "tbRemove";
            this.tbRemove.Size = new System.Drawing.Size(23, 22);
            this.tbRemove.Text = "toolStripButton2";
            this.tbRemove.ToolTipText = "Remove";
            // 
            // tbEdit
            // 
            this.tbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbEdit.Image = global::FAD3.Properties.Resources.Edit_16xMD;
            this.tbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbEdit.Name = "tbEdit";
            this.tbEdit.Size = new System.Drawing.Size(23, 22);
            this.tbEdit.Text = "toolStripButton3";
            this.tbEdit.ToolTipText = "Edit";
            // 
            // tbExport
            // 
            this.tbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbExport.Image = global::FAD3.Properties.Resources.ExportFile_16x;
            this.tbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbExport.Name = "tbExport";
            this.tbExport.Size = new System.Drawing.Size(23, 22);
            this.tbExport.Text = "toolStripButton4";
            this.tbExport.ToolTipText = "Export";
            // 
            // tbImport
            // 
            this.tbImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbImport.Image = global::FAD3.Properties.Resources.ImportFile_16x;
            this.tbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbImport.Name = "tbImport";
            this.tbImport.Size = new System.Drawing.Size(23, 22);
            this.tbImport.Text = "toolStripButton5";
            this.tbImport.ToolTipText = "Import";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel1.Text = "View";
            // 
            // tbCombo
            // 
            this.tbCombo.AutoSize = false;
            this.tbCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbCombo.DropDownWidth = 150;
            this.tbCombo.Items.AddRange(new object[] {
            "Species names",
            "Local/common names"});
            this.tbCombo.Name = "tbCombo";
            this.tbCombo.Size = new System.Drawing.Size(121, 23);
            this.tbCombo.SelectedIndexChanged += new System.EventHandler(this.OnToolBarComboSelectedIndexChange);
            // 
            // CatchLocalNamesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 385);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.lblTree);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblList);
            this.Controls.Add(this.treeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "CatchLocalNamesForm";
            this.Text = "CatchLocalNamesForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClose);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.Resize += new System.EventHandler(this.OnFormResize);
            this.contextMenu.ResumeLayout(false);
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Label lblList;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblTree;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem itemLocateOnNavigationTree;
        private System.Windows.Forms.ToolStripMenuItem itemBrowseWWW;
        private System.Windows.Forms.ToolStripMenuItem itemWiki;
        private System.Windows.Forms.ToolStripMenuItem itemFishBase;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton tbAdd;
        private System.Windows.Forms.ToolStripButton tbRemove;
        private System.Windows.Forms.ToolStripButton tbEdit;
        private System.Windows.Forms.ToolStripButton tbExport;
        private System.Windows.Forms.ToolStripButton tbImport;
        private System.Windows.Forms.ToolStripComboBox tbCombo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}