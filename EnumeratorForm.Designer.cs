﻿/*
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
            this.labelListView = new System.Windows.Forms.Label();
            this.tree = new System.Windows.Forms.TreeView();
            this.panelTop = new System.Windows.Forms.Panel();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.labelHireDate = new System.Windows.Forms.Label();
            this.labelEnumeratorName = new System.Windows.Forms.Label();
            this.txtHireDate = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(806, 292);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(54, 31);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "Ok";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(738, 293);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(60, 31);
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
            this.lvEnumerators.Location = new System.Drawing.Point(259, 84);
            this.lvEnumerators.Margin = new System.Windows.Forms.Padding(4);
            this.lvEnumerators.MultiSelect = false;
            this.lvEnumerators.Name = "lvEnumerators";
            this.lvEnumerators.Size = new System.Drawing.Size(617, 181);
            this.lvEnumerators.TabIndex = 3;
            this.lvEnumerators.UseCompatibleStateImageBehavior = false;
            this.lvEnumerators.DoubleClick += new System.EventHandler(this.OnlistEnumeratorSampling_DoubleClick);
            this.lvEnumerators.Leave += new System.EventHandler(this.OnListEnumeratorSamplingLeave);
            // 
            // labelListView
            // 
            this.labelListView.Location = new System.Drawing.Point(4, 62);
            this.labelListView.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelListView.Name = "labelListView";
            this.labelListView.Size = new System.Drawing.Size(251, 18);
            this.labelListView.TabIndex = 9;
            this.labelListView.Text = "Samplings enumerated";
            // 
            // tree
            // 
            this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tree.Location = new System.Drawing.Point(0, 84);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(259, 181);
            this.tree.TabIndex = 10;
            this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.chkActive);
            this.panelTop.Controls.Add(this.labelHireDate);
            this.panelTop.Controls.Add(this.labelListView);
            this.panelTop.Controls.Add(this.labelEnumeratorName);
            this.panelTop.Controls.Add(this.txtHireDate);
            this.panelTop.Controls.Add(this.txtName);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(876, 84);
            this.panelTop.TabIndex = 11;
            // 
            // chkActive
            // 
            this.chkActive.Location = new System.Drawing.Point(555, 25);
            this.chkActive.Margin = new System.Windows.Forms.Padding(4);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(105, 23);
            this.chkActive.TabIndex = 10;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // labelHireDate
            // 
            this.labelHireDate.Location = new System.Drawing.Point(310, 25);
            this.labelHireDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelHireDate.Name = "labelHireDate";
            this.labelHireDate.Size = new System.Drawing.Size(84, 25);
            this.labelHireDate.TabIndex = 12;
            this.labelHireDate.Text = "Hire date";
            // 
            // labelEnumeratorName
            // 
            this.labelEnumeratorName.Location = new System.Drawing.Point(7, 22);
            this.labelEnumeratorName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEnumeratorName.Name = "labelEnumeratorName";
            this.labelEnumeratorName.Size = new System.Drawing.Size(53, 25);
            this.labelEnumeratorName.TabIndex = 11;
            this.labelEnumeratorName.Text = "Name";
            // 
            // txtHireDate
            // 
            this.txtHireDate.Location = new System.Drawing.Point(390, 23);
            this.txtHireDate.Margin = new System.Windows.Forms.Padding(4);
            this.txtHireDate.Name = "txtHireDate";
            this.txtHireDate.Size = new System.Drawing.Size(139, 22);
            this.txtHireDate.TabIndex = 9;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(63, 22);
            this.txtName.Margin = new System.Windows.Forms.Padding(4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(224, 22);
            this.txtName.TabIndex = 8;
            // 
            // EnumeratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 337);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.tree);
            this.Controls.Add(this.lvEnumerators);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EnumeratorForm";
            this.Text = "frmEnumerator";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EnumeratorForm_FormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.Label labelListView;
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
    }
}
