namespace FAD3.Database.Forms
{
    partial class ImportFishCatchMonitoringDestinationForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelImport = new System.Windows.Forms.Panel();
            this.rbtnImportToNew = new System.Windows.Forms.RadioButton();
            this.rbtnImportExisting = new System.Windows.Forms.RadioButton();
            this.txtNewTargetArea = new System.Windows.Forms.TextBox();
            this.cboTargetAreas = new System.Windows.Forms.ComboBox();
            this.lblError = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelImport.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(261, 161);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(50, 28);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(299, 30);
            this.lblTitle.TabIndex = 6;
            this.lblTitle.Text = "label3";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelImport
            // 
            this.panelImport.Controls.Add(this.rbtnImportToNew);
            this.panelImport.Controls.Add(this.rbtnImportExisting);
            this.panelImport.Controls.Add(this.txtNewTargetArea);
            this.panelImport.Controls.Add(this.cboTargetAreas);
            this.panelImport.Location = new System.Drawing.Point(12, 37);
            this.panelImport.Name = "panelImport";
            this.panelImport.Size = new System.Drawing.Size(299, 114);
            this.panelImport.TabIndex = 10;
            // 
            // rbtnImportToNew
            // 
            this.rbtnImportToNew.AutoSize = true;
            this.rbtnImportToNew.Location = new System.Drawing.Point(3, 65);
            this.rbtnImportToNew.Name = "rbtnImportToNew";
            this.rbtnImportToNew.Size = new System.Drawing.Size(151, 17);
            this.rbtnImportToNew.TabIndex = 16;
            this.rbtnImportToNew.TabStop = true;
            this.rbtnImportToNew.Text = "Import into new target area";
            this.rbtnImportToNew.UseVisualStyleBackColor = true;
            // 
            // rbtnImportExisting
            // 
            this.rbtnImportExisting.AutoSize = true;
            this.rbtnImportExisting.Checked = true;
            this.rbtnImportExisting.Location = new System.Drawing.Point(3, 4);
            this.rbtnImportExisting.Name = "rbtnImportExisting";
            this.rbtnImportExisting.Size = new System.Drawing.Size(166, 17);
            this.rbtnImportExisting.TabIndex = 15;
            this.rbtnImportExisting.TabStop = true;
            this.rbtnImportExisting.Text = "Import into existing target area";
            this.rbtnImportExisting.UseVisualStyleBackColor = true;
            // 
            // txtNewTargetArea
            // 
            this.txtNewTargetArea.Location = new System.Drawing.Point(3, 88);
            this.txtNewTargetArea.Name = "txtNewTargetArea";
            this.txtNewTargetArea.Size = new System.Drawing.Size(287, 20);
            this.txtNewTargetArea.TabIndex = 14;
            // 
            // cboTargetAreas
            // 
            this.cboTargetAreas.FormattingEnabled = true;
            this.cboTargetAreas.Location = new System.Drawing.Point(3, 27);
            this.cboTargetAreas.Name = "cboTargetAreas";
            this.cboTargetAreas.Size = new System.Drawing.Size(287, 21);
            this.cboTargetAreas.TabIndex = 13;
            // 
            // lblError
            // 
            this.lblError.Location = new System.Drawing.Point(1, 80);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(315, 38);
            this.lblError.TabIndex = 11;
            this.lblError.Text = "label1";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblError.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(205, 161);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 28);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // ImportFishCatchMonitoringDestinationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 197);
            this.ControlBox = false;
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.panelImport);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ImportFishCatchMonitoringDestinationForm";
            this.Text = "Import fish catch monitoring data";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.panelImport.ResumeLayout(false);
            this.panelImport.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelImport;
        private System.Windows.Forms.RadioButton rbtnImportToNew;
        private System.Windows.Forms.RadioButton rbtnImportExisting;
        private System.Windows.Forms.TextBox txtNewTargetArea;
        private System.Windows.Forms.ComboBox cboTargetAreas;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnCancel;
    }
}