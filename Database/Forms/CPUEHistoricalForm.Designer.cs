namespace FAD3.Database.Forms
{
    partial class CPUEHistoricalForm
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
            this.txtCPUE = new System.Windows.Forms.TextBox();
            this.lblValue = new System.Windows.Forms.Label();
            this.cboUnit = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.txtHistoryYear = new System.Windows.Forms.TextBox();
            this.lblHistoryYear = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtCPUE
            // 
            this.txtCPUE.Location = new System.Drawing.Point(83, 38);
            this.txtCPUE.Name = "txtCPUE";
            this.txtCPUE.Size = new System.Drawing.Size(150, 20);
            this.txtCPUE.TabIndex = 0;
            this.txtCPUE.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextBoxValidating);
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(12, 41);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(65, 13);
            this.lblValue.TabIndex = 1;
            this.lblValue.Text = "CPUE value";
            // 
            // cboUnit
            // 
            this.cboUnit.FormattingEnabled = true;
            this.cboUnit.Location = new System.Drawing.Point(83, 64);
            this.cboUnit.Name = "cboUnit";
            this.cboUnit.Size = new System.Drawing.Size(150, 21);
            this.cboUnit.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Unit";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(187, 166);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(42, 24);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Location = new System.Drawing.Point(133, 166);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(48, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(130, 15);
            this.lblTitle.TabIndex = 6;
            this.lblTitle.Text = "Historical CPUE for";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Notes";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(83, 91);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(150, 66);
            this.txtNotes.TabIndex = 7;
            // 
            // txtHistoryYear
            // 
            this.txtHistoryYear.Location = new System.Drawing.Point(17, 169);
            this.txtHistoryYear.Name = "txtHistoryYear";
            this.txtHistoryYear.Size = new System.Drawing.Size(30, 20);
            this.txtHistoryYear.TabIndex = 9;
            this.txtHistoryYear.Visible = false;
            // 
            // lblHistoryYear
            // 
            this.lblHistoryYear.AutoSize = true;
            this.lblHistoryYear.Location = new System.Drawing.Point(12, 153);
            this.lblHistoryYear.Name = "lblHistoryYear";
            this.lblHistoryYear.Size = new System.Drawing.Size(65, 13);
            this.lblHistoryYear.TabIndex = 10;
            this.lblHistoryYear.Text = "CPUE value";
            this.lblHistoryYear.Visible = false;
            // 
            // CPUEHistoricalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 196);
            this.Controls.Add(this.lblHistoryYear);
            this.Controls.Add(this.txtHistoryYear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboUnit);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.txtCPUE);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CPUEHistoricalForm";
            this.ShowInTaskbar = false;
            this.Text = "Historical CPUE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CPUEHistoricalForm_FormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCPUE;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.ComboBox cboUnit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.TextBox txtHistoryYear;
        private System.Windows.Forms.Label lblHistoryYear;
    }
}