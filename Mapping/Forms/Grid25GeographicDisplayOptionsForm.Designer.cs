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
            this.txtMajorGridLabelSize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkShowSubgrids = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMinorGridLabelSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboMinorGridDistance = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkMinorGridBoldLabels = new System.Windows.Forms.CheckBox();
            this.chkMajorGridBoldLabels = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMajorGridLabelSize
            // 
            this.txtMajorGridLabelSize.Location = new System.Drawing.Point(151, 23);
            this.txtMajorGridLabelSize.Name = "txtMajorGridLabelSize";
            this.txtMajorGridLabelSize.Size = new System.Drawing.Size(94, 20);
            this.txtMajorGridLabelSize.TabIndex = 5;
            this.txtMajorGridLabelSize.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Show subgrids";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(246, 277);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(44, 28);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(186, 277);
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
            this.chkShowSubgrids.Location = new System.Drawing.Point(153, 106);
            this.chkShowSubgrids.Name = "chkShowSubgrids";
            this.chkShowSubgrids.Size = new System.Drawing.Size(15, 14);
            this.chkShowSubgrids.TabIndex = 8;
            this.chkShowSubgrids.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Size";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkMinorGridBoldLabels);
            this.groupBox1.Controls.Add(this.txtMinorGridLabelSize);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chkShowSubgrids);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cboMinorGridDistance);
            this.groupBox1.Location = new System.Drawing.Point(12, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 138);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Minor grid";
            // 
            // txtMinorGridLabelSize
            // 
            this.txtMinorGridLabelSize.Location = new System.Drawing.Point(154, 49);
            this.txtMinorGridLabelSize.Name = "txtMinorGridLabelSize";
            this.txtMinorGridLabelSize.Size = new System.Drawing.Size(94, 20);
            this.txtMinorGridLabelSize.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Label distance";
            // 
            // cboMinorGridDistance
            // 
            this.cboMinorGridDistance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMinorGridDistance.FormattingEnabled = true;
            this.cboMinorGridDistance.Location = new System.Drawing.Point(153, 17);
            this.cboMinorGridDistance.Name = "cboMinorGridDistance";
            this.cboMinorGridDistance.Size = new System.Drawing.Size(94, 21);
            this.cboMinorGridDistance.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.chkMajorGridBoldLabels);
            this.groupBox2.Controls.Add(this.txtMajorGridLabelSize);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 176);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(277, 82);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Major grid";
            // 
            // chkMinorGridBoldLabels
            // 
            this.chkMinorGridBoldLabels.AutoSize = true;
            this.chkMinorGridBoldLabels.Location = new System.Drawing.Point(153, 81);
            this.chkMinorGridBoldLabels.Name = "chkMinorGridBoldLabels";
            this.chkMinorGridBoldLabels.Size = new System.Drawing.Size(15, 14);
            this.chkMinorGridBoldLabels.TabIndex = 9;
            this.chkMinorGridBoldLabels.UseVisualStyleBackColor = true;
            // 
            // chkMajorGridBoldLabels
            // 
            this.chkMajorGridBoldLabels.AutoSize = true;
            this.chkMajorGridBoldLabels.Location = new System.Drawing.Point(151, 53);
            this.chkMajorGridBoldLabels.Name = "chkMajorGridBoldLabels";
            this.chkMajorGridBoldLabels.Size = new System.Drawing.Size(15, 14);
            this.chkMajorGridBoldLabels.TabIndex = 10;
            this.chkMajorGridBoldLabels.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Bold";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Bold";
            // 
            // Grid25GeographicDisplayOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 314);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Grid25GeographicDisplayOptionsForm";
            this.Text = "Display options";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtMajorGridLabelSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkShowSubgrids;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkMinorGridBoldLabels;
        private System.Windows.Forms.TextBox txtMinorGridLabelSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboMinorGridDistance;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkMajorGridBoldLabels;
    }
}