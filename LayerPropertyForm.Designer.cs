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
            this.btnLabelFeatures = new System.Windows.Forms.Button();
            this.btnLabelCategories = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnFeatureSymbols = new System.Windows.Forms.Button();
            this.btnFeatureCategories = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            // btnLabelFeatures
            // 
            this.btnLabelFeatures.Location = new System.Drawing.Point(38, 28);
            this.btnLabelFeatures.Name = "btnLabelFeatures";
            this.btnLabelFeatures.Size = new System.Drawing.Size(78, 27);
            this.btnLabelFeatures.TabIndex = 8;
            this.btnLabelFeatures.Text = "Features";
            this.btnLabelFeatures.UseVisualStyleBackColor = true;
            this.btnLabelFeatures.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnLabelCategories
            // 
            this.btnLabelCategories.Location = new System.Drawing.Point(38, 65);
            this.btnLabelCategories.Name = "btnLabelCategories";
            this.btnLabelCategories.Size = new System.Drawing.Size(78, 27);
            this.btnLabelCategories.TabIndex = 9;
            this.btnLabelCategories.Text = "Categories";
            this.btnLabelCategories.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLabelFeatures);
            this.groupBox1.Controls.Add(this.btnLabelCategories);
            this.groupBox1.Location = new System.Drawing.Point(27, 195);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(142, 113);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Labels";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnFeatureSymbols);
            this.groupBox2.Controls.Add(this.btnFeatureCategories);
            this.groupBox2.Location = new System.Drawing.Point(214, 195);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(142, 113);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Symbols";
            // 
            // btnFeatureSymbols
            // 
            this.btnFeatureSymbols.Location = new System.Drawing.Point(36, 28);
            this.btnFeatureSymbols.Name = "btnFeatureSymbols";
            this.btnFeatureSymbols.Size = new System.Drawing.Size(80, 27);
            this.btnFeatureSymbols.TabIndex = 8;
            this.btnFeatureSymbols.Text = "Features";
            this.btnFeatureSymbols.UseVisualStyleBackColor = true;
            // 
            // btnFeatureCategories
            // 
            this.btnFeatureCategories.Location = new System.Drawing.Point(36, 65);
            this.btnFeatureCategories.Name = "btnFeatureCategories";
            this.btnFeatureCategories.Size = new System.Drawing.Size(80, 27);
            this.btnFeatureCategories.TabIndex = 9;
            this.btnFeatureCategories.Text = "Categories";
            this.btnFeatureCategories.UseVisualStyleBackColor = true;
            // 
            // LayerPropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 356);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnLabelFeatures;
        private System.Windows.Forms.Button btnLabelCategories;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnFeatureSymbols;
        private System.Windows.Forms.Button btnFeatureCategories;
    }
}