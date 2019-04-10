namespace FAD3.Mapping.Forms
{
    partial class Grid25GeographicDisplayOptionsForm
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
            this.cboMinorGridDistance = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMinorGridLabelSize = new System.Windows.Forms.TextBox();
            this.txtMajorGridLabelSize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkShowSubgrids = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboMinorGridDistance
            // 
            this.cboMinorGridDistance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMinorGridDistance.FormattingEnabled = true;
            this.cboMinorGridDistance.Location = new System.Drawing.Point(144, 38);
            this.cboMinorGridDistance.Name = "cboMinorGridDistance";
            this.cboMinorGridDistance.Size = new System.Drawing.Size(94, 21);
            this.cboMinorGridDistance.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Minor grid label distance";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Minor grid label size";
            // 
            // txtMinorGridLabelSize
            // 
            this.txtMinorGridLabelSize.Location = new System.Drawing.Point(144, 74);
            this.txtMinorGridLabelSize.Name = "txtMinorGridLabelSize";
            this.txtMinorGridLabelSize.Size = new System.Drawing.Size(94, 20);
            this.txtMinorGridLabelSize.TabIndex = 3;
            this.txtMinorGridLabelSize.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidating);
            // 
            // txtMajorGridLabelSize
            // 
            this.txtMajorGridLabelSize.Location = new System.Drawing.Point(144, 109);
            this.txtMajorGridLabelSize.Name = "txtMajorGridLabelSize";
            this.txtMajorGridLabelSize.Size = new System.Drawing.Size(94, 20);
            this.txtMajorGridLabelSize.TabIndex = 5;
            this.txtMajorGridLabelSize.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Show subgrids";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(192, 175);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(44, 28);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(132, 175);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(54, 28);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // chkShowSubgrids
            // 
            this.chkShowSubgrids.AutoSize = true;
            this.chkShowSubgrids.Location = new System.Drawing.Point(144, 143);
            this.chkShowSubgrids.Name = "chkShowSubgrids";
            this.chkShowSubgrids.Size = new System.Drawing.Size(15, 14);
            this.chkShowSubgrids.TabIndex = 8;
            this.chkShowSubgrids.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Major grid label size";
            // 
            // Grid25GeographicDisplayOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 215);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkShowSubgrids);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtMajorGridLabelSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMinorGridLabelSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboMinorGridDistance);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Grid25GeographicDisplayOptionsForm";
            this.Text = "Grid25GeographicDisplayOptionsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboMinorGridDistance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMinorGridLabelSize;
        private System.Windows.Forms.TextBox txtMajorGridLabelSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkShowSubgrids;
        private System.Windows.Forms.Label label4;
    }
}