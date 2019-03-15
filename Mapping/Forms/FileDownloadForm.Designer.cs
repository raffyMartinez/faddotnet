namespace FAD3.Mapping.Forms
{
    partial class FileDownloadForm
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblDownloadFile = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblDownloadError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 109);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(362, 20);
            this.progressBar.TabIndex = 0;
            // 
            // lblDownloadFile
            // 
            this.lblDownloadFile.Location = new System.Drawing.Point(12, 46);
            this.lblDownloadFile.Name = "lblDownloadFile";
            this.lblDownloadFile.Size = new System.Drawing.Size(362, 56);
            this.lblDownloadFile.TabIndex = 1;
            this.lblDownloadFile.Text = "this is a long label just to test how alignment looks alike when the text is long" +
    " enough";
            this.lblDownloadFile.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(136, 22);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(115, 15);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Downloading file";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(337, 184);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(37, 27);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblDownloadError
            // 
            this.lblDownloadError.Location = new System.Drawing.Point(12, 132);
            this.lblDownloadError.Name = "lblDownloadError";
            this.lblDownloadError.Size = new System.Drawing.Size(362, 49);
            this.lblDownloadError.TabIndex = 4;
            this.lblDownloadError.Text = "This is a download error";
            this.lblDownloadError.Visible = false;
            // 
            // FileDownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 223);
            this.Controls.Add(this.lblDownloadError);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblDownloadFile);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FileDownloadForm";
            this.Text = "FileDownloadForm";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblDownloadFile;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblDownloadError;
    }
}