namespace FAD3.Database.Forms
{
    partial class EditGearVariationNameForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.cboGearClasses = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboGearVariation = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLocalNameToAdd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(10, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(281, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Edit this gear to";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboGearClasses
            // 
            this.cboGearClasses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGearClasses.FormattingEnabled = true;
            this.cboGearClasses.Location = new System.Drawing.Point(94, 68);
            this.cboGearClasses.Name = "cboGearClasses";
            this.cboGearClasses.Size = new System.Drawing.Size(198, 21);
            this.cboGearClasses.TabIndex = 1;
            this.cboGearClasses.SelectedIndexChanged += new System.EventHandler(this.OnGearClassChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Gear class";
            // 
            // cboGearVariation
            // 
            this.cboGearVariation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGearVariation.FormattingEnabled = true;
            this.cboGearVariation.Location = new System.Drawing.Point(94, 106);
            this.cboGearVariation.Name = "cboGearVariation";
            this.cboGearVariation.Size = new System.Drawing.Size(198, 21);
            this.cboGearVariation.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Gear variation";
            // 
            // txtLocalNameToAdd
            // 
            this.txtLocalNameToAdd.Location = new System.Drawing.Point(94, 146);
            this.txtLocalNameToAdd.Name = "txtLocalNameToAdd";
            this.txtLocalNameToAdd.Size = new System.Drawing.Size(198, 20);
            this.txtLocalNameToAdd.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Add local name";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(242, 214);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(49, 29);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(184, 214);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(53, 29);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // EditGearVariationNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 255);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLocalNameToAdd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboGearVariation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboGearClasses);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditGearVariationNameForm";
            this.Text = "Edit gear variation name";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox cboGearClasses;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboGearVariation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLocalNameToAdd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}