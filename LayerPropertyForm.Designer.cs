namespace FAD3
{
    partial class LayerPropertyForm
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
            this.txtLayerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGeoProjection = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLayerType = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtLayerName
            // 
            this.txtLayerName.Location = new System.Drawing.Point(114, 53);
            this.txtLayerName.Name = "txtLayerName";
            this.txtLayerName.Size = new System.Drawing.Size(252, 21);
            this.txtLayerName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Layer name";
            // 
            // txtGeoProjection
            // 
            this.txtGeoProjection.Location = new System.Drawing.Point(114, 80);
            this.txtGeoProjection.Name = "txtGeoProjection";
            this.txtGeoProjection.Size = new System.Drawing.Size(252, 21);
            this.txtGeoProjection.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Projection";
            // 
            // txtLayerType
            // 
            this.txtLayerType.Location = new System.Drawing.Point(114, 106);
            this.txtLayerType.Name = "txtLayerType";
            this.txtLayerType.Size = new System.Drawing.Size(252, 21);
            this.txtLayerType.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Filename";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(114, 132);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(252, 21);
            this.txtFileName.TabIndex = 6;
            // 
            // LayerPropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 545);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLayerType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtGeoProjection);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLayerName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LayerPropertyForm";
            this.Text = "LayerPropertyForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LayerPropertyForm_FormClosed);
            this.Load += new System.EventHandler(this.LayerPropertyForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLayerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGeoProjection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLayerType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFileName;
    }
}