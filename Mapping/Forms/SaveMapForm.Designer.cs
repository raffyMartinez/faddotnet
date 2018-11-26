namespace FAD3
{
    partial class SaveMapForm
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
            this.txtSave = new System.Windows.Forms.TextBox();
            this.lblSave = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtSave
            // 
            this.txtSave.Location = new System.Drawing.Point(107, 22);
            this.txtSave.Margin = new System.Windows.Forms.Padding(4);
            this.txtSave.Name = "txtSave";
            this.txtSave.Size = new System.Drawing.Size(154, 21);
            this.txtSave.TabIndex = 0;
            this.txtSave.Validating += new System.ComponentModel.CancelEventHandler(this.OnTxtSave_Validating);
            // 
            // lblSave
            // 
            this.lblSave.AutoSize = true;
            this.lblSave.Location = new System.Drawing.Point(3, 24);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(97, 15);
            this.lblSave.TabIndex = 1;
            this.lblSave.Text = "Resolution (DPI)";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(215, 59);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(46, 24);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(152, 59);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // SaveMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 95);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblSave);
            this.Controls.Add(this.txtSave);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SaveMapForm";
            this.ShowInTaskbar = false;
            this.Text = "Save fishing grid";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnGrid25SaveForm_FormClosed);
            this.Load += new System.EventHandler(this.OnGrid25SaveForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSave;
        private System.Windows.Forms.Label lblSave;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}