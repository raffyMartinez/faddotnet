namespace FAD3
{
    partial class ShapefileAttributesForm
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
            this.lvAttributes = new System.Windows.Forms.ListView();
            this.labelShapeFileName = new System.Windows.Forms.Label();
            this.chkRemember = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lvAttributes
            // 
            this.lvAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvAttributes.Location = new System.Drawing.Point(0, 31);
            this.lvAttributes.Name = "lvAttributes";
            this.lvAttributes.Size = new System.Drawing.Size(699, 452);
            this.lvAttributes.TabIndex = 2;
            this.lvAttributes.UseCompatibleStateImageBehavior = false;
            // 
            // labelShapeFileName
            // 
            this.labelShapeFileName.AutoSize = true;
            this.labelShapeFileName.Location = new System.Drawing.Point(10, 8);
            this.labelShapeFileName.Name = "labelShapeFileName";
            this.labelShapeFileName.Size = new System.Drawing.Size(41, 15);
            this.labelShapeFileName.TabIndex = 3;
            this.labelShapeFileName.Text = "label1";
            // 
            // chkRemember
            // 
            this.chkRemember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkRemember.AutoSize = true;
            this.chkRemember.Location = new System.Drawing.Point(8, 491);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new System.Drawing.Size(89, 19);
            this.chkRemember.TabIndex = 4;
            this.chkRemember.Text = "Remember";
            this.chkRemember.UseVisualStyleBackColor = true;
            // 
            // ShapefileAttributesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 519);
            this.Controls.Add(this.chkRemember);
            this.Controls.Add(this.labelShapeFileName);
            this.Controls.Add(this.lvAttributes);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ShapefileAttributesForm";
            this.Text = "ShapefileAttributesForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ShapefileAttributesForm_FormClosed);
            this.Load += new System.EventHandler(this.ShapefileAttributesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView lvAttributes;
        private System.Windows.Forms.Label labelShapeFileName;
        private System.Windows.Forms.CheckBox chkRemember;
    }
}