namespace FAD3.Database.Forms
{
    partial class MappingBatchForm
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
            this.btnDirectory = new System.Windows.Forms.Button();
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.txtDPI = new System.Windows.Forms.TextBox();
            this.lblDPI = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDirectory
            // 
            this.btnDirectory.Location = new System.Drawing.Point(25, 22);
            this.btnDirectory.Name = "btnDirectory";
            this.btnDirectory.Size = new System.Drawing.Size(84, 25);
            this.btnDirectory.TabIndex = 0;
            this.btnDirectory.Text = "Save to folder";
            this.btnDirectory.UseVisualStyleBackColor = true;
            this.btnDirectory.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Location = new System.Drawing.Point(25, 53);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(300, 20);
            this.txtFolderPath.TabIndex = 1;
            // 
            // txtDPI
            // 
            this.txtDPI.Location = new System.Drawing.Point(25, 126);
            this.txtDPI.Name = "txtDPI";
            this.txtDPI.Size = new System.Drawing.Size(300, 20);
            this.txtDPI.TabIndex = 2;
            this.txtDPI.Text = "150";
            // 
            // lblDPI
            // 
            this.lblDPI.AutoSize = true;
            this.lblDPI.Location = new System.Drawing.Point(22, 110);
            this.lblDPI.Name = "lblDPI";
            this.lblDPI.Size = new System.Drawing.Size(155, 13);
            this.lblDPI.TabIndex = 3;
            this.lblDPI.Text = "Resolution of saved graphic file";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(272, 185);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(53, 25);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(213, 185);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(53, 25);
            this.button1.TabIndex = 5;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // MappingBatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 222);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblDPI);
            this.Controls.Add(this.txtDPI);
            this.Controls.Add(this.txtFolderPath);
            this.Controls.Add(this.btnDirectory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MappingBatchForm";
            this.Text = "MappingBatchForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDirectory;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.TextBox txtDPI;
        private System.Windows.Forms.Label lblDPI;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button button1;
    }
}