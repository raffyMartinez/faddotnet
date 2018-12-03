/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/13/2016
 * Time: 11:50 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace FAD3
{
	partial class EnumeratorForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.lvEnumerators = new System.Windows.Forms.ListView();
            this.labelSamplings = new System.Windows.Forms.Label();
            this.tree = new System.Windows.Forms.TreeView();
            this.panelTop = new System.Windows.Forms.Panel();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.labelHireDate = new System.Windows.Forms.Label();
            this.labelEnumeratorName = new System.Windows.Forms.Label();
            this.txtHireDate = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.tbAdd = new System.Windows.Forms.ToolStripButton();
            this.tbRemove = new System.Windows.Forms.ToolStripButton();
            this.tbExport = new System.Windows.Forms.ToolStripButton();
            this.tbImport = new System.Windows.Forms.ToolStripButton();
            this.panelTop.SuspendLayout();
            this.toolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(705, 274);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(47, 29);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "Ok";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(640, 275);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(58, 29);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lvEnumerators
            // 
            this.lvEnumerators.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvEnumerators.Location = new System.Drawing.Point(227, 79);
            this.lvEnumerators.Margin = new System.Windows.Forms.Padding(4);
            this.lvEnumerators.MultiSelect = false;
            this.lvEnumerators.Name = "lvEnumerators";
            this.lvEnumerators.Size = new System.Drawing.Size(540, 170);
            this.lvEnumerators.TabIndex = 3;
            this.lvEnumerators.UseCompatibleStateImageBehavior = false;
            this.lvEnumerators.DoubleClick += new System.EventHandler(this.OnlistEnumeratorSampling_DoubleClick);
            this.lvEnumerators.Leave += new System.EventHandler(this.OnListEnumeratorSamplingLeave);
            // 
            // labelSamplings
            // 
            this.labelSamplings.Location = new System.Drawing.Point(4, 58);
            this.labelSamplings.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSamplings.Name = "labelSamplings";
            this.labelSamplings.Size = new System.Drawing.Size(220, 17);
            this.labelSamplings.TabIndex = 9;
            this.labelSamplings.Text = "Samplings enumerated";
            // 
            // tree
            // 
            this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tree.Location = new System.Drawing.Point(0, 79);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(227, 170);
            this.tree.TabIndex = 10;
            this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeView_AfterSelect);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.chkActive);
            this.panelTop.Controls.Add(this.labelHireDate);
            this.panelTop.Controls.Add(this.labelSamplings);
            this.panelTop.Controls.Add(this.labelEnumeratorName);
            this.panelTop.Controls.Add(this.txtHireDate);
            this.panelTop.Controls.Add(this.txtName);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(766, 79);
            this.panelTop.TabIndex = 11;
            // 
            // chkActive
            // 
            this.chkActive.Location = new System.Drawing.Point(486, 32);
            this.chkActive.Margin = new System.Windows.Forms.Padding(4);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(92, 22);
            this.chkActive.TabIndex = 10;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // labelHireDate
            // 
            this.labelHireDate.Location = new System.Drawing.Point(271, 32);
            this.labelHireDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelHireDate.Name = "labelHireDate";
            this.labelHireDate.Size = new System.Drawing.Size(74, 23);
            this.labelHireDate.TabIndex = 12;
            this.labelHireDate.Text = "Hire date";
            // 
            // labelEnumeratorName
            // 
            this.labelEnumeratorName.Location = new System.Drawing.Point(6, 30);
            this.labelEnumeratorName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEnumeratorName.Name = "labelEnumeratorName";
            this.labelEnumeratorName.Size = new System.Drawing.Size(46, 23);
            this.labelEnumeratorName.TabIndex = 11;
            this.labelEnumeratorName.Text = "Name";
            // 
            // txtHireDate
            // 
            this.txtHireDate.Location = new System.Drawing.Point(341, 31);
            this.txtHireDate.Margin = new System.Windows.Forms.Padding(4);
            this.txtHireDate.Name = "txtHireDate";
            this.txtHireDate.Size = new System.Drawing.Size(122, 21);
            this.txtHireDate.TabIndex = 9;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(55, 30);
            this.txtName.Margin = new System.Windows.Forms.Padding(4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(196, 21);
            this.txtName.TabIndex = 8;
            // 
            // toolBar
            // 
            this.toolBar.Dock = System.Windows.Forms.DockStyle.None;
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbAdd,
            this.tbRemove,
            this.tbExport,
            this.tbImport});
            this.toolBar.Location = new System.Drawing.Point(55, 275);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(135, 25);
            this.toolBar.TabIndex = 13;
            this.toolBar.Text = "toolStrip1";
            this.toolBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolbarItemClick);
            // 
            // tbAdd
            // 
            this.tbAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbAdd.Image = global::FAD3.Properties.Resources.Add_16xSM;
            this.tbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbAdd.Name = "tbAdd";
            this.tbAdd.Size = new System.Drawing.Size(23, 22);
            this.tbAdd.Text = "toolStripButton1";
            this.tbAdd.ToolTipText = "Add enumerator";
            // 
            // tbRemove
            // 
            this.tbRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbRemove.Image = global::FAD3.Properties.Resources.Remove_16xSM;
            this.tbRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRemove.Name = "tbRemove";
            this.tbRemove.Size = new System.Drawing.Size(23, 22);
            this.tbRemove.Text = "toolStripButton2";
            this.tbRemove.ToolTipText = "Remove enumerator";
            // 
            // tbExport
            // 
            this.tbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbExport.Image = global::FAD3.Properties.Resources.ExportFile_16x;
            this.tbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbExport.Name = "tbExport";
            this.tbExport.Size = new System.Drawing.Size(23, 22);
            this.tbExport.Text = "toolStripButton3";
            this.tbExport.ToolTipText = "Export";
            // 
            // tbImport
            // 
            this.tbImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbImport.Image = global::FAD3.Properties.Resources.ImportFile_16x;
            this.tbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbImport.Name = "tbImport";
            this.tbImport.Size = new System.Drawing.Size(23, 22);
            this.tbImport.Text = "toolStripButton4";
            this.tbImport.ToolTipText = "Import";
            // 
            // EnumeratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 316);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.tree);
            this.Controls.Add(this.lvEnumerators);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EnumeratorForm";
            this.ShowInTaskbar = false;
            this.Text = "frmEnumerator";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EnumeratorForm_FormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Label labelSamplings;
		private System.Windows.Forms.ListView lvEnumerators;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label labelHireDate;
        private System.Windows.Forms.Label labelEnumeratorName;
        private System.Windows.Forms.TextBox txtHireDate;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton tbAdd;
        private System.Windows.Forms.ToolStripButton tbRemove;
        private System.Windows.Forms.ToolStripButton tbExport;
        private System.Windows.Forms.ToolStripButton tbImport;
    }
}
