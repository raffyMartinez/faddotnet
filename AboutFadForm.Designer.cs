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
            this.labelCredits = new System.Windows.Forms.Label();
            this.labelNetFramework = new System.Windows.Forms.Label();
            this.axMap = new AxMapWinGIS.AxMap();
            ((System.ComponentModel.ISupportInitialize)(this.axMap)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(260, 201);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(66, 22);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // labelApp
            // 
            this.labelApp.AutoSize = true;
            this.labelApp.Location = new System.Drawing.Point(90, 34);
            this.labelApp.Name = "labelApp";
            this.labelApp.Size = new System.Drawing.Size(159, 13);
            this.labelApp.TabIndex = 1;
            this.labelApp.Text = "Fisheries Assessment Database ";
            // 
            // labelCredits
            // 
            this.labelCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCredits.Location = new System.Drawing.Point(4, 59);
            this.labelCredits.Name = "labelCredits";
            this.labelCredits.Size = new System.Drawing.Size(328, 78);
            this.labelCredits.TabIndex = 2;
            this.labelCredits.Text = "label1";
            this.labelCredits.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelNetFramework
            // 
            this.labelNetFramework.Location = new System.Drawing.Point(5, 150);
            this.labelNetFramework.Name = "labelNetFramework";
            this.labelNetFramework.Size = new System.Drawing.Size(331, 21);
            this.labelNetFramework.TabIndex = 3;
            this.labelNetFramework.Text = "label1";
            // 
            // axMap
            // 
            this.axMap.Enabled = true;
            this.axMap.Location = new System.Drawing.Point(12, 201);
            this.axMap.Name = "axMap";
            this.axMap.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMap.OcxState")));
            this.axMap.Size = new System.Drawing.Size(35, 28);
            this.axMap.TabIndex = 4;
            this.axMap.Visible = false;
            // 
            // AboutFadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(338, 235);
            this.Controls.Add(this.axMap);
            this.Controls.Add(this.labelNetFramework);
            this.Controls.Add(this.labelCredits);
            this.Controls.Add(this.labelApp);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AboutFadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About FAD";
            this.Load += new System.EventHandler(this.frmAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axMap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelApp;
        private System.Windows.Forms.Label labelCredits;
        private System.Windows.Forms.Label labelNetFramework;
        private AxMapWinGIS.AxMap axMap;
    }
}