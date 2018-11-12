namespace FAD3.Database.Forms
{
    partial class EditSelectedLocalNameForm
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
            this.txtLocalName = new System.Windows.Forms.TextBox();
            this.lblEdit = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtLocalName
            // 
            this.txtLocalName.Location = new System.Drawing.Point(12, 58);
            this.txtLocalName.Name = "txtLocalName";
            this.txtLocalName.Size = new System.Drawing.Size(262, 20);
            this.txtLocalName.TabIndex = 0;
            // 
            // lblEdit
            // 
            this.lblEdit.AutoSize = true;
            this.lblEdit.Location = new System.Drawing.Point(12, 42);
            this.lblEdit.Name = "lblEdit";
            this.lblEdit.Size = new System.Drawing.Size(62, 13);
            this.lblEdit.TabIndex = 1;
            this.lblEdit.Text = "Local name";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(226, 97);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(48, 25);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(172, 97);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(48, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // comboBox
            // 
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(140, 31);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(134, 21);
            this.comboBox.TabIndex = 4;
            this.comboBox.Visible = false;
            // 
            // EditSelectedLocalNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 136);
            this.Controls.Add(this.comboBox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblEdit);
            this.Controls.Add(this.txtLocalName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditSelectedLocalNameForm";
            this.Text = "Edit selected local name";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLocalName;
        private System.Windows.Forms.Label lblEdit;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox comboBox;
    }
}