namespace FAD3.Database.Forms
{
    partial class ImportInventoryXMLForm
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
            this.rdbImportExisting = new System.Windows.Forms.RadioButton();
            this.rdbImportNew = new System.Windows.Forms.RadioButton();
            this.cboInventories = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtImportedProject = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // rdbImportExisting
            // 
            this.rdbImportExisting.AutoSize = true;
            this.rdbImportExisting.Location = new System.Drawing.Point(18, 25);
            this.rdbImportExisting.Name = "rdbImportExisting";
            this.rdbImportExisting.Size = new System.Drawing.Size(193, 17);
            this.rdbImportExisting.TabIndex = 0;
            this.rdbImportExisting.TabStop = true;
            this.rdbImportExisting.Text = "Import into existing inventory project";
            this.rdbImportExisting.UseVisualStyleBackColor = true;
            // 
            // rdbImportNew
            // 
            this.rdbImportNew.AutoSize = true;
            this.rdbImportNew.Location = new System.Drawing.Point(18, 76);
            this.rdbImportNew.Name = "rdbImportNew";
            this.rdbImportNew.Size = new System.Drawing.Size(133, 17);
            this.rdbImportNew.TabIndex = 1;
            this.rdbImportNew.TabStop = true;
            this.rdbImportNew.Text = "Import to a new project";
            this.rdbImportNew.UseVisualStyleBackColor = true;
            // 
            // cboInventories
            // 
            this.cboInventories.FormattingEnabled = true;
            this.cboInventories.Location = new System.Drawing.Point(39, 44);
            this.cboInventories.Name = "cboInventories";
            this.cboInventories.Size = new System.Drawing.Size(292, 21);
            this.cboInventories.TabIndex = 2;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(280, 138);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(50, 25);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(224, 138);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // txtImportedProject
            // 
            this.txtImportedProject.Location = new System.Drawing.Point(39, 95);
            this.txtImportedProject.Name = "txtImportedProject";
            this.txtImportedProject.Size = new System.Drawing.Size(292, 20);
            this.txtImportedProject.TabIndex = 5;
            // 
            // ImportInventoryXMLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 174);
            this.Controls.Add(this.txtImportedProject);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cboInventories);
            this.Controls.Add(this.rdbImportNew);
            this.Controls.Add(this.rdbImportExisting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ImportInventoryXMLForm";
            this.Text = "Import fisher, fishing vessel and gear inventory";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdbImportExisting;
        private System.Windows.Forms.RadioButton rdbImportNew;
        private System.Windows.Forms.ComboBox cboInventories;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtImportedProject;
    }
}