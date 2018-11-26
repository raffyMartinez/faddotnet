namespace FAD3
{
    partial class SpeciesNameForm
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
            this.labelGenus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGenus = new System.Windows.Forms.TextBox();
            this.txtSpecies = new System.Windows.Forms.TextBox();
            this.cboTaxa = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkInFishbase = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.labelRecordCount = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelGenus
            // 
            this.labelGenus.AutoSize = true;
            this.labelGenus.Location = new System.Drawing.Point(10, 39);
            this.labelGenus.Name = "labelGenus";
            this.labelGenus.Size = new System.Drawing.Size(43, 15);
            this.labelGenus.TabIndex = 0;
            this.labelGenus.Text = "Genus";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "species";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Taxa";
            // 
            // txtGenus
            // 
            this.txtGenus.Location = new System.Drawing.Point(73, 37);
            this.txtGenus.Name = "txtGenus";
            this.txtGenus.Size = new System.Drawing.Size(180, 21);
            this.txtGenus.TabIndex = 3;
            this.txtGenus.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextBoxes_Validating);
            // 
            // txtSpecies
            // 
            this.txtSpecies.Location = new System.Drawing.Point(73, 63);
            this.txtSpecies.Name = "txtSpecies";
            this.txtSpecies.Size = new System.Drawing.Size(180, 21);
            this.txtSpecies.TabIndex = 4;
            this.txtSpecies.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextBoxes_Validating);
            // 
            // cboTaxa
            // 
            this.cboTaxa.FormattingEnabled = true;
            this.cboTaxa.Location = new System.Drawing.Point(73, 89);
            this.cboTaxa.Name = "cboTaxa";
            this.cboTaxa.Size = new System.Drawing.Size(180, 23);
            this.cboTaxa.TabIndex = 5;
            this.cboTaxa.SelectedIndexChanged += new System.EventHandler(this.OnTaxaIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 34);
            this.label4.TabIndex = 6;
            this.label4.Text = "Listed in Fishbase";
            // 
            // chkInFishbase
            // 
            this.chkInFishbase.AutoSize = true;
            this.chkInFishbase.Location = new System.Drawing.Point(79, 132);
            this.chkInFishbase.Name = "chkInFishbase";
            this.chkInFishbase.Size = new System.Drawing.Size(15, 14);
            this.chkInFishbase.TabIndex = 7;
            this.chkInFishbase.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 181);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Notes";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(73, 178);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(180, 67);
            this.txtNotes.TabIndex = 9;
            this.txtNotes.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextBoxes_Validating);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(137, 361);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 24);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(198, 361);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(56, 24);
            this.buttonOK.TabIndex = 13;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonEdit);
            this.groupBox1.Controls.Add(this.labelRecordCount);
            this.groupBox1.Location = new System.Drawing.Point(73, 255);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 91);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Records";
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(64, 50);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(56, 24);
            this.buttonEdit.TabIndex = 16;
            this.buttonEdit.Text = "Edit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // labelRecordCount
            // 
            this.labelRecordCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRecordCount.Location = new System.Drawing.Point(8, 22);
            this.labelRecordCount.Name = "labelRecordCount";
            this.labelRecordCount.Size = new System.Drawing.Size(166, 22);
            this.labelRecordCount.TabIndex = 15;
            this.labelRecordCount.Text = "0000";
            this.labelRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SpeciesNameForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(265, 391);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkInFishbase);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboTaxa);
            this.Controls.Add(this.txtSpecies);
            this.Controls.Add(this.txtGenus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelGenus);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SpeciesNameForm";
            this.ShowInTaskbar = false;
            this.Text = "Species name";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.SpeciesNameForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelGenus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGenus;
        private System.Windows.Forms.TextBox txtSpecies;
        private System.Windows.Forms.ComboBox cboTaxa;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkInFishbase;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Label labelRecordCount;
    }
}