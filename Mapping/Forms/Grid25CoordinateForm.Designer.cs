namespace FAD3.Mapping.Forms
{
    partial class Grid25CoordinateForm
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
            this.txtCoord = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtCoord
            // 
            this.txtCoord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCoord.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCoord.Location = new System.Drawing.Point(0, 0);
            this.txtCoord.Multiline = true;
            this.txtCoord.Name = "txtCoord";
            this.txtCoord.Size = new System.Drawing.Size(253, 59);
            this.txtCoord.TabIndex = 0;
            // 
            // Grid25CoordinateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 59);
            this.Controls.Add(this.txtCoord);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Grid25CoordinateForm";
            this.Text = "Grid25CoordinateForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCoord;
    }
}