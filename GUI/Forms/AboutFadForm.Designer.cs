namespace FAD3
{
    partial class AboutFadForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutFadForm));
            this.button1 = new System.Windows.Forms.Button();
            this.labelApp = new System.Windows.Forms.Label();
            this.axMap = new AxMapWinGIS.AxMap();
            this.panelUI = new System.Windows.Forms.Panel();
            this.labelCredits = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.axMap)).BeginInit();
            this.panelUI.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(266, 420);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(66, 22);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // labelApp
            // 
            this.labelApp.AutoSize = true;
            this.labelApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApp.Location = new System.Drawing.Point(24, 25);
            this.labelApp.Name = "labelApp";
            this.labelApp.Size = new System.Drawing.Size(302, 22);
            this.labelApp.TabIndex = 1;
            this.labelApp.Text = "Fisheries Assessment Database ";
            // 
            // axMap
            // 
            this.axMap.Enabled = true;
            this.axMap.Location = new System.Drawing.Point(6, 382);
            this.axMap.Name = "axMap";
            this.axMap.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMap.OcxState")));
            this.axMap.Size = new System.Drawing.Size(35, 28);
            this.axMap.TabIndex = 4;
            this.axMap.Visible = false;
            // 
            // panelUI
            // 
            this.panelUI.AutoScroll = true;
            this.panelUI.Controls.Add(this.labelCredits);
            this.panelUI.Location = new System.Drawing.Point(6, 75);
            this.panelUI.Name = "panelUI";
            this.panelUI.Size = new System.Drawing.Size(326, 301);
            this.panelUI.TabIndex = 5;
            // 
            // labelCredits
            // 
            this.labelCredits.AutoSize = true;
            this.labelCredits.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCredits.Location = new System.Drawing.Point(30, 2);
            this.labelCredits.Name = "labelCredits";
            this.labelCredits.Size = new System.Drawing.Size(31, 14);
            this.labelCredits.TabIndex = 3;
            this.labelCredits.Text = "label1";
            this.labelCredits.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(28, 47);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(287, 25);
            this.lblVersion.TabIndex = 6;
            this.lblVersion.Text = "label1";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAuthor
            // 
            this.lblAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthor.Location = new System.Drawing.Point(24, 378);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(287, 39);
            this.lblAuthor.TabIndex = 7;
            this.lblAuthor.Text = "label1";
            this.lblAuthor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AboutFadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(338, 448);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.panelUI);
            this.Controls.Add(this.axMap);
            this.Controls.Add(this.labelApp);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AboutFadForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About FAD";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AboutFadForm_FormClosed);
            this.Load += new System.EventHandler(this.frmAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axMap)).EndInit();
            this.panelUI.ResumeLayout(false);
            this.panelUI.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelApp;
        private AxMapWinGIS.AxMap axMap;
        private System.Windows.Forms.Panel panelUI;
        private System.Windows.Forms.Label labelCredits;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblAuthor;
    }
}