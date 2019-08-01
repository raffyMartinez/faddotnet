namespace FAD3.Database.Forms
{
    partial class HTMLTableSelectColumnsForm
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboSpName = new System.Windows.Forms.ComboBox();
            this.lblSpName = new System.Windows.Forms.Label();
            this.lblLocalName = new System.Windows.Forms.Label();
            this.cboLocalName = new System.Windows.Forms.ComboBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.cboLanguage = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(189, 200);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(46, 25);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(131, 200);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(52, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // cboSpName
            // 
            this.cboSpName.Enabled = false;
            this.cboSpName.FormattingEnabled = true;
            this.cboSpName.Location = new System.Drawing.Point(15, 51);
            this.cboSpName.Name = "cboSpName";
            this.cboSpName.Size = new System.Drawing.Size(220, 21);
            this.cboSpName.TabIndex = 3;
            // 
            // lblSpName
            // 
            this.lblSpName.AutoSize = true;
            this.lblSpName.Enabled = false;
            this.lblSpName.Location = new System.Drawing.Point(12, 35);
            this.lblSpName.Name = "lblSpName";
            this.lblSpName.Size = new System.Drawing.Size(111, 13);
            this.lblSpName.TabIndex = 4;
            this.lblSpName.Text = "Species name column";
            // 
            // lblLocalName
            // 
            this.lblLocalName.AutoSize = true;
            this.lblLocalName.Enabled = false;
            this.lblLocalName.Location = new System.Drawing.Point(12, 91);
            this.lblLocalName.Name = "lblLocalName";
            this.lblLocalName.Size = new System.Drawing.Size(62, 13);
            this.lblLocalName.TabIndex = 6;
            this.lblLocalName.Text = "Local name";
            // 
            // cboLocalName
            // 
            this.cboLocalName.Enabled = false;
            this.cboLocalName.FormattingEnabled = true;
            this.cboLocalName.Location = new System.Drawing.Point(15, 107);
            this.cboLocalName.Name = "cboLocalName";
            this.cboLocalName.Size = new System.Drawing.Size(220, 21);
            this.cboLocalName.TabIndex = 5;
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Enabled = false;
            this.lblLanguage.Location = new System.Drawing.Point(12, 148);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(92, 13);
            this.lblLanguage.TabIndex = 8;
            this.lblLanguage.Text = "Language column";
            // 
            // cboLanguage
            // 
            this.cboLanguage.Enabled = false;
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.Location = new System.Drawing.Point(15, 164);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(220, 21);
            this.cboLanguage.TabIndex = 7;
            // 
            // HTMLTableSelectColumnsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 236);
            this.Controls.Add(this.lblLanguage);
            this.Controls.Add(this.cboLanguage);
            this.Controls.Add(this.lblLocalName);
            this.Controls.Add(this.cboLocalName);
            this.Controls.Add(this.lblSpName);
            this.Controls.Add(this.cboSpName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "HTMLTableSelectColumnsForm";
            this.ShowInTaskbar = false;
            this.Text = "HTMLTableSelectColumnsForm";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cboSpName;
        private System.Windows.Forms.Label lblSpName;
        private System.Windows.Forms.Label lblLocalName;
        private System.Windows.Forms.ComboBox cboLocalName;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.ComboBox cboLanguage;
    }
}