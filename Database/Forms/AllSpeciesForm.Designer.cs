namespace FAD3
{
    partial class AllSpeciesForm
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
            this.lvNames = new System.Windows.Forms.ListView();
            this.dropDownMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.buttonOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvTaxa = new System.Windows.Forms.ListView();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.chkShowWithRecords = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.tbAdd = new System.Windows.Forms.ToolStripButton();
            this.tbRemove = new System.Windows.Forms.ToolStripButton();
            this.tbEdit = new System.Windows.Forms.ToolStripButton();
            this.tbExport = new System.Windows.Forms.ToolStripButton();
            this.tbImport = new System.Windows.Forms.ToolStripButton();
            this.groupBox1.SuspendLayout();
            this.toolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvNames
            // 
            this.lvNames.ContextMenuStrip = this.dropDownMenu;
            this.lvNames.Location = new System.Drawing.Point(199, 46);
            this.lvNames.Name = "lvNames";
            this.lvNames.Size = new System.Drawing.Size(596, 411);
            this.lvNames.TabIndex = 0;
            this.lvNames.UseCompatibleStateImageBehavior = false;
            this.lvNames.DoubleClick += new System.EventHandler(this.OnlvNames_DoubleClick);
            this.lvNames.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvNames_MouseDown);
            // 
            // dropDownMenu
            // 
            this.dropDownMenu.Name = "dropDownMenu";
            this.dropDownMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOK.Location = new System.Drawing.Point(743, 463);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(51, 24);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.OnButtonOK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(196, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "List of species names";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvTaxa);
            this.groupBox1.Controls.Add(this.buttonReset);
            this.groupBox1.Controls.Add(this.buttonApply);
            this.groupBox1.Controls.Add(this.chkShowWithRecords);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSearch);
            this.groupBox1.Location = new System.Drawing.Point(10, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 418);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // lvTaxa
            // 
            this.lvTaxa.CheckBoxes = true;
            this.lvTaxa.Location = new System.Drawing.Point(8, 103);
            this.lvTaxa.Name = "lvTaxa";
            this.lvTaxa.Size = new System.Drawing.Size(166, 218);
            this.lvTaxa.TabIndex = 21;
            this.lvTaxa.UseCompatibleStateImageBehavior = false;
            this.lvTaxa.View = System.Windows.Forms.View.List;
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(27, 375);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(50, 24);
            this.buttonReset.TabIndex = 19;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(82, 376);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(50, 24);
            this.buttonApply.TabIndex = 18;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // chkShowWithRecords
            // 
            this.chkShowWithRecords.Location = new System.Drawing.Point(10, 325);
            this.chkShowWithRecords.Name = "chkShowWithRecords";
            this.chkShowWithRecords.Size = new System.Drawing.Size(164, 55);
            this.chkShowWithRecords.TabIndex = 20;
            this.chkShowWithRecords.Text = "Show only those with records in the database";
            this.chkShowWithRecords.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "Filter by taxa";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "Search";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(10, 52);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(153, 21);
            this.txtSearch.TabIndex = 14;
            // 
            // toolBar
            // 
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbAdd,
            this.tbRemove,
            this.tbEdit,
            this.tbExport,
            this.tbImport});
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(802, 25);
            this.toolBar.TabIndex = 23;
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
            this.tbAdd.ToolTipText = "Add species";
            // 
            // tbRemove
            // 
            this.tbRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbRemove.Image = global::FAD3.Properties.Resources.Remove_16xSM;
            this.tbRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRemove.Name = "tbRemove";
            this.tbRemove.Size = new System.Drawing.Size(23, 22);
            this.tbRemove.Text = "toolStripButton2";
            this.tbRemove.ToolTipText = "Remove species";
            // 
            // tbEdit
            // 
            this.tbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbEdit.Image = global::FAD3.Properties.Resources.Edit_16xMD;
            this.tbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbEdit.Name = "tbEdit";
            this.tbEdit.Size = new System.Drawing.Size(23, 22);
            this.tbEdit.Text = "toolStripButton3";
            this.tbEdit.ToolTipText = "Edit species";
            // 
            // tbExport
            // 
            this.tbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbExport.Image = global::FAD3.Properties.Resources.ExportFile_16x;
            this.tbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbExport.Name = "tbExport";
            this.tbExport.Size = new System.Drawing.Size(23, 22);
            this.tbExport.Text = "toolStripButton4";
            this.tbExport.ToolTipText = "Export species list";
            // 
            // tbImport
            // 
            this.tbImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbImport.Image = global::FAD3.Properties.Resources.ImportFile_16x;
            this.tbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbImport.Name = "tbImport";
            this.tbImport.Size = new System.Drawing.Size(23, 22);
            this.tbImport.Text = "toolStripButton5";
            this.tbImport.ToolTipText = "Import species list";
            // 
            // AllSpeciesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOK;
            this.ClientSize = new System.Drawing.Size(802, 495);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.lvNames);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AllSpeciesForm";
            this.Text = "AllSpeciesForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AllSpeciesForm_FormClosing);
            this.Load += new System.EventHandler(this.AllSpeciesForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvNames;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.CheckBox chkShowWithRecords;
        private System.Windows.Forms.ContextMenuStrip dropDownMenu;
        private System.Windows.Forms.ListView lvTaxa;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton tbAdd;
        private System.Windows.Forms.ToolStripButton tbRemove;
        private System.Windows.Forms.ToolStripButton tbEdit;
        private System.Windows.Forms.ToolStripButton tbExport;
        private System.Windows.Forms.ToolStripButton tbImport;
    }
}