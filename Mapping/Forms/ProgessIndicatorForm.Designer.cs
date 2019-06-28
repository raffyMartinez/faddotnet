namespace FAD3.Mapping.Forms
{
    partial class ProgessIndicatorForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDownloadError = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDownloadFile = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(329, 189);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(52, 27);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(273, 188);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 28);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblDownloadError
            // 
            this.lblDownloadError.Location = new System.Drawing.Point(9, 140);
            this.lblDownloadError.Name = "lblDownloadError";
            this.lblDownloadError.Size = new System.Drawing.Size(362, 33);
            this.lblDownloadError.TabIndex = 16;
            this.lblDownloadError.Text = "This is a download error";
            this.lblDownloadError.Visible = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(9, 19);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(369, 37);
            this.lblTitle.TabIndex = 15;
            this.lblTitle.Text = "Downloading file";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDownloadFile
            // 
            this.lblDownloadFile.Location = new System.Drawing.Point(9, 56);
            this.lblDownloadFile.Name = "lblDownloadFile";
            this.lblDownloadFile.Size = new System.Drawing.Size(369, 56);
            this.lblDownloadFile.TabIndex = 14;
            this.lblDownloadFile.Text = "this is a long label just to test how alignment looks alike when the text is long" +
    " enough";
            this.lblDownloadFile.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(9, 119);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(362, 20);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 13;
            // 
            // FileDownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 223);
            this.Controls.Add(this.lblDownloadError);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblDownloadFile);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FileDownloadForm";
            this.Text = "FileDownloadForm";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDownloadError;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDownloadFile;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}