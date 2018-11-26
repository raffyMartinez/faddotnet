namespace FAD3.Mapping.Forms
{
    partial class GraticuleTextHelperForm
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
            this.txtText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkBold = new System.Windows.Forms.CheckBox();
            this.udFontSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboTextAlign = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.udFontSize)).BeginInit();
            this.SuspendLayout();
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(26, 44);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(332, 20);
            this.txtText.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Text";
            // 
            // chkBold
            // 
            this.chkBold.AutoSize = true;
            this.chkBold.Location = new System.Drawing.Point(60, 87);
            this.chkBold.Name = "chkBold";
            this.chkBold.Size = new System.Drawing.Size(15, 14);
            this.chkBold.TabIndex = 2;
            this.chkBold.UseVisualStyleBackColor = true;
            // 
            // udFontSize
            // 
            this.udFontSize.Location = new System.Drawing.Point(140, 85);
            this.udFontSize.Name = "udFontSize";
            this.udFontSize.Size = new System.Drawing.Size(56, 20);
            this.udFontSize.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Size";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Bold";
            // 
            // cboTextAlign
            // 
            this.cboTextAlign.FormattingEnabled = true;
            this.cboTextAlign.Location = new System.Drawing.Point(276, 85);
            this.cboTextAlign.Name = "cboTextAlign";
            this.cboTextAlign.Size = new System.Drawing.Size(82, 21);
            this.cboTextAlign.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(217, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Alignment";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(303, 141);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(43, 25);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(240, 141);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(51, 25);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // GraticuleTextHelperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 183);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboTextAlign);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.udFontSize);
            this.Controls.Add(this.chkBold);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GraticuleTextHelperForm";
            this.ShowInTaskbar = false;
            this.Text = "ConfigureMapTextForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnGraticuleTextHelperForm_Closed);
            this.Load += new System.EventHandler(this.OnGraticuleTextHelperForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.udFontSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkBold;
        private System.Windows.Forms.NumericUpDown udFontSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboTextAlign;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}