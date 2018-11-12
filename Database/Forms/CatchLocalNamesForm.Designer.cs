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
            this.treeView = new System.Windows.Forms.TreeView();
            this.lblList = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTree = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cboSelectId = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(1, 34);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(224, 313);
            this.treeView.TabIndex = 0;
            this.treeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.OnAfterLabelEdit);
            this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterExpand);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeClick);
            this.treeView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            // 
            // lblList
            // 
            this.lblList.AutoSize = true;
            this.lblList.Location = new System.Drawing.Point(231, 18);
            this.lblList.Name = "lblList";
            this.lblList.Size = new System.Drawing.Size(108, 13);
            this.lblList.TabIndex = 2;
            this.lblList.Text = "List of species names";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(611, 353);
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
            this.btnCancel.Location = new System.Drawing.Point(553, 353);
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
            this.lblTree.Location = new System.Drawing.Point(1, 18);
            this.lblTree.Name = "lblTree";
            this.lblTree.Size = new System.Drawing.Size(140, 13);
            this.lblTree.TabIndex = 5;
            this.lblTree.Text = "Local names and languages";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Image = global::FAD3.Properties.Resources.Add_16xSM;
            this.btnAdd.Location = new System.Drawing.Point(662, 31);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(29, 29);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Image = global::FAD3.Properties.Resources.Remove_16xSM;
            this.btnRemove.Location = new System.Drawing.Point(662, 61);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(29, 29);
            this.btnRemove.TabIndex = 7;
            this.btnRemove.Text = "-";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Image = global::FAD3.Properties.Resources.Import_16x;
            this.btnImport.Location = new System.Drawing.Point(662, 106);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(29, 29);
            this.btnImport.TabIndex = 8;
            this.btnImport.Text = "-";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(231, 34);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(421, 313);
            this.listView.TabIndex = 9;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.DoubleClick += new System.EventHandler(this.OnListViewDblClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Language";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Species names";
            // 
            // cboSelectId
            // 
            this.cboSelectId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboSelectId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectId.FormattingEnabled = true;
            this.cboSelectId.Items.AddRange(new object[] {
            "Local/common names",
            "Species names"});
            this.cboSelectId.Location = new System.Drawing.Point(1, 356);
            this.cboSelectId.Name = "cboSelectId";
            this.cboSelectId.Size = new System.Drawing.Size(224, 21);
            this.cboSelectId.TabIndex = 10;
            this.cboSelectId.SelectedIndexChanged += new System.EventHandler(this.OnIndexChanged);
            // 
            // CatchLocalNamesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 385);
            this.Controls.Add(this.cboSelectId);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Label lblList;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblTree;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ComboBox cboSelectId;
    }
}