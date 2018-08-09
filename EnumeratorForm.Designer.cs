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
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtHireDate = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.lvEnumerators = new System.Windows.Forms.ListView();
            this.labelEnumeratorName = new System.Windows.Forms.Label();
            this.labelHireDate = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.labelListView = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(80, 36);
            this.txtName.Margin = new System.Windows.Forms.Padding(4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(224, 22);
            this.txtName.TabIndex = 0;
            // 
            // txtHireDate
            // 
            this.txtHireDate.Location = new System.Drawing.Point(407, 36);
            this.txtHireDate.Margin = new System.Windows.Forms.Padding(4);
            this.txtHireDate.Name = "txtHireDate";
            this.txtHireDate.Size = new System.Drawing.Size(139, 22);
            this.txtHireDate.TabIndex = 1;
            this.txtHireDate.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextBoxValidating);
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
            this.lvEnumerators.Location = new System.Drawing.Point(16, 110);
            this.lvEnumerators.Margin = new System.Windows.Forms.Padding(4);
            this.lvEnumerators.MultiSelect = false;
            this.lvEnumerators.Name = "lvEnumerators";
            this.lvEnumerators.Size = new System.Drawing.Size(843, 159);
            this.lvEnumerators.TabIndex = 3;
            this.lvEnumerators.UseCompatibleStateImageBehavior = false;
            this.lvEnumerators.DoubleClick += new System.EventHandler(this.OnlistEnumeratorSampling_DoubleClick);
            this.lvEnumerators.Leave += new System.EventHandler(this.OnListEnumeratorSamplingLeave);
            // 
            // labelEnumeratorName
            // 
            this.labelEnumeratorName.Location = new System.Drawing.Point(19, 39);
            this.labelEnumeratorName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEnumeratorName.Name = "labelEnumeratorName";
            this.labelEnumeratorName.Size = new System.Drawing.Size(53, 25);
            this.labelEnumeratorName.TabIndex = 6;
            this.labelEnumeratorName.Text = "Name";
            // 
            // labelHireDate
            // 
            this.labelHireDate.Location = new System.Drawing.Point(327, 38);
            this.labelHireDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelHireDate.Name = "labelHireDate";
            this.labelHireDate.Size = new System.Drawing.Size(84, 25);
            this.labelHireDate.TabIndex = 7;
            this.labelHireDate.Text = "Hire date";
            // 
            // chkActive
            // 
            this.chkActive.Location = new System.Drawing.Point(572, 38);
            this.chkActive.Margin = new System.Windows.Forms.Padding(4);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(105, 23);
            this.chkActive.TabIndex = 2;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // labelListView
            // 
            this.labelListView.Location = new System.Drawing.Point(16, 87);
            this.labelListView.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelListView.Name = "labelListView";
            this.labelListView.Size = new System.Drawing.Size(251, 18);
            this.labelListView.TabIndex = 9;
            this.labelListView.Text = "Samplings enumerated";
            // 
            // EnumeratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 337);
            this.Controls.Add(this.labelListView);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.labelHireDate);
            this.Controls.Add(this.labelEnumeratorName);
            this.Controls.Add(this.lvEnumerators);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.txtHireDate);
            this.Controls.Add(this.txtName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EnumeratorForm";
            this.Text = "frmEnumerator";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EnumeratorForm_FormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Label labelListView;
		private System.Windows.Forms.CheckBox chkActive;
		private System.Windows.Forms.Label labelHireDate;
		private System.Windows.Forms.Label labelEnumeratorName;
		private System.Windows.Forms.ListView lvEnumerators;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.TextBox txtHireDate;
		private System.Windows.Forms.TextBox txtName;
	}
}
