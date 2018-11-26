namespace FAD3
{
    partial class ReferenceNumberForm
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.lvCodes = new System.Windows.Forms.ListView();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelRefNo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(340, 182);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(60, 26);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(273, 182);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(60, 26);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // lvCodes
            // 
            this.lvCodes.Location = new System.Drawing.Point(10, 37);
            this.lvCodes.Name = "lvCodes";
            this.lvCodes.Size = new System.Drawing.Size(391, 139);
            this.lvCodes.TabIndex = 2;
            this.lvCodes.UseCompatibleStateImageBehavior = false;
            this.lvCodes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnlvCodes_MouseDown);
            // 
            // labelTitle
            // 
            this.labelTitle.Location = new System.Drawing.Point(10, 8);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(390, 29);
            this.labelTitle.TabIndex = 3;
            this.labelTitle.Text = "label1";
            // 
            // labelRefNo
            // 
            this.labelRefNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRefNo.Location = new System.Drawing.Point(90, 96);
            this.labelRefNo.Name = "labelRefNo";
            this.labelRefNo.Size = new System.Drawing.Size(228, 31);
            this.labelRefNo.TabIndex = 5;
            this.labelRefNo.Text = "label1";
            this.labelRefNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReferenceNumberForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(412, 220);
            this.Controls.Add(this.labelRefNo);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.lvCodes);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ReferenceNumberForm";
            this.ShowInTaskbar = false;
            this.Text = "GenerateRefNumberForm";
            this.Load += new System.EventHandler(this.OnForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ListView lvCodes;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelRefNo;
    }
}