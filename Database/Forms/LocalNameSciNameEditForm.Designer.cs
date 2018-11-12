namespace FAD3.Database.Forms
{
    partial class LocalNameSciNameEditForm
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
            this.cboLocalName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboSpeciesName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboLanguage = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cboLocalName
            // 
            this.cboLocalName.FormattingEnabled = true;
            this.cboLocalName.Location = new System.Drawing.Point(88, 53);
            this.cboLocalName.Name = "cboLocalName";
            this.cboLocalName.Size = new System.Drawing.Size(192, 21);
            this.cboLocalName.TabIndex = 0;
            this.cboLocalName.Validating += new System.ComponentModel.CancelEventHandler(this.OnComboValidating);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 28);
            this.label1.TabIndex = 1;
            this.label1.Text = "Local/common name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Species name";
            // 
            // cboSpeciesName
            // 
            this.cboSpeciesName.FormattingEnabled = true;
            this.cboSpeciesName.Location = new System.Drawing.Point(88, 90);
            this.cboSpeciesName.Name = "cboSpeciesName";
            this.cboSpeciesName.Size = new System.Drawing.Size(192, 21);
            this.cboSpeciesName.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Language";
            // 
            // cboLanguage
            // 
            this.cboLanguage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLanguage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.Location = new System.Drawing.Point(88, 127);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(192, 21);
            this.cboLanguage.TabIndex = 4;
            this.cboLanguage.Validating += new System.ComponentModel.CancelEventHandler(this.OnComboValidating);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(221, 167);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(55, 25);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(160, 167);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // LocalNameSciNameEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 207);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboLanguage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboSpeciesName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboLocalName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LocalNameSciNameEditForm";
            this.Text = "Edit local/common name - species name pair";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboLocalName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboSpeciesName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboLanguage;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}