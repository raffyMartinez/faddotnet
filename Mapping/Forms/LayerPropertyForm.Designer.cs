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
            this.components = new System.ComponentModel.Container();
            this.txtLayerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGeoProjection = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLayerType = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabAppearance = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnFeatureSymbols = new System.Windows.Forms.Button();
            this.btnFeatureCategories = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLabelFeatures = new System.Windows.Forms.Button();
            this.btnLabelCategories = new System.Windows.Forms.Button();
            this.tabCategories = new System.Windows.Forms.TabPage();
            this.tabVisibility = new System.Windows.Forms.TabPage();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnApplyVisibility = new System.Windows.Forms.Button();
            this.btnDefineVisibilityExpression = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.txtVisibilityExpression = new System.Windows.Forms.TextBox();
            this.tabSelection = new System.Windows.Forms.TabPage();
            this.transpSelection = new FAD3.Mapping.UserControls.TransparencyControl();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.lvLayerProps = new System.Windows.Forms.ListView();
            this.btnApply = new System.Windows.Forms.Button();
            this.menuShortCut = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuSavePropsToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabAppearance.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabVisibility.SuspendLayout();
            this.tabSelection.SuspendLayout();
            this.tabProperties.SuspendLayout();
            this.menuShortCut.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLayerName
            // 
            this.txtLayerName.Location = new System.Drawing.Point(114, 16);
            this.txtLayerName.Name = "txtLayerName";
            this.txtLayerName.Size = new System.Drawing.Size(252, 21);
            this.txtLayerName.TabIndex = 0;
            this.txtLayerName.TextChanged += new System.EventHandler(this.OntxtLayerName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Layer name";
            // 
            // txtGeoProjection
            // 
            this.txtGeoProjection.Location = new System.Drawing.Point(114, 43);
            this.txtGeoProjection.Name = "txtGeoProjection";
            this.txtGeoProjection.Size = new System.Drawing.Size(252, 21);
            this.txtGeoProjection.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Projection";
            // 
            // txtLayerType
            // 
            this.txtLayerType.Location = new System.Drawing.Point(114, 69);
            this.txtLayerType.Name = "txtLayerType";
            this.txtLayerType.Size = new System.Drawing.Size(252, 21);
            this.txtLayerType.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Filename";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(114, 95);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(252, 21);
            this.txtFileName.TabIndex = 6;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(313, 368);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(60, 27);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabAppearance);
            this.tabControl1.Controls.Add(this.tabCategories);
            this.tabControl1.Controls.Add(this.tabVisibility);
            this.tabControl1.Controls.Add(this.tabSelection);
            this.tabControl1.Controls.Add(this.tabProperties);
            this.tabControl1.Location = new System.Drawing.Point(4, 135);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(373, 227);
            this.tabControl1.TabIndex = 13;
            // 
            // tabAppearance
            // 
            this.tabAppearance.Controls.Add(this.groupBox2);
            this.tabAppearance.Controls.Add(this.groupBox1);
            this.tabAppearance.Location = new System.Drawing.Point(4, 24);
            this.tabAppearance.Name = "tabAppearance";
            this.tabAppearance.Padding = new System.Windows.Forms.Padding(3);
            this.tabAppearance.Size = new System.Drawing.Size(365, 199);
            this.tabAppearance.TabIndex = 0;
            this.tabAppearance.Text = "Appearance";
            this.tabAppearance.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnFeatureSymbols);
            this.groupBox2.Controls.Add(this.btnFeatureCategories);
            this.groupBox2.Location = new System.Drawing.Point(206, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(142, 113);
            this.groupBox2.TabIndex = 12;
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
            this.btnFeatureSymbols.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnFeatureCategories
            // 
            this.btnFeatureCategories.Location = new System.Drawing.Point(36, 65);
            this.btnFeatureCategories.Name = "btnFeatureCategories";
            this.btnFeatureCategories.Size = new System.Drawing.Size(80, 27);
            this.btnFeatureCategories.TabIndex = 9;
            this.btnFeatureCategories.Text = "Categories";
            this.btnFeatureCategories.UseVisualStyleBackColor = true;
            this.btnFeatureCategories.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLabelFeatures);
            this.groupBox1.Controls.Add(this.btnLabelCategories);
            this.groupBox1.Location = new System.Drawing.Point(17, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(142, 113);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Labels";
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
            this.btnLabelCategories.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // tabCategories
            // 
            this.tabCategories.Location = new System.Drawing.Point(4, 24);
            this.tabCategories.Name = "tabCategories";
            this.tabCategories.Padding = new System.Windows.Forms.Padding(3);
            this.tabCategories.Size = new System.Drawing.Size(365, 199);
            this.tabCategories.TabIndex = 1;
            this.tabCategories.Text = "Categories";
            this.tabCategories.UseVisualStyleBackColor = true;
            // 
            // tabVisibility
            // 
            this.tabVisibility.Controls.Add(this.btnClear);
            this.tabVisibility.Controls.Add(this.btnApplyVisibility);
            this.tabVisibility.Controls.Add(this.btnDefineVisibilityExpression);
            this.tabVisibility.Controls.Add(this.label23);
            this.tabVisibility.Controls.Add(this.txtVisibilityExpression);
            this.tabVisibility.Location = new System.Drawing.Point(4, 24);
            this.tabVisibility.Name = "tabVisibility";
            this.tabVisibility.Padding = new System.Windows.Forms.Padding(3);
            this.tabVisibility.Size = new System.Drawing.Size(365, 199);
            this.tabVisibility.TabIndex = 2;
            this.tabVisibility.Text = "Visibility";
            this.tabVisibility.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(293, 107);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(52, 24);
            this.btnClear.TabIndex = 54;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnApplyVisibility
            // 
            this.btnApplyVisibility.Location = new System.Drawing.Point(293, 67);
            this.btnApplyVisibility.Name = "btnApplyVisibility";
            this.btnApplyVisibility.Size = new System.Drawing.Size(52, 24);
            this.btnApplyVisibility.TabIndex = 53;
            this.btnApplyVisibility.Text = "Apply";
            this.btnApplyVisibility.UseVisualStyleBackColor = true;
            this.btnApplyVisibility.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnDefineVisibilityExpression
            // 
            this.btnDefineVisibilityExpression.Location = new System.Drawing.Point(293, 37);
            this.btnDefineVisibilityExpression.Name = "btnDefineVisibilityExpression";
            this.btnDefineVisibilityExpression.Size = new System.Drawing.Size(52, 24);
            this.btnDefineVisibilityExpression.TabIndex = 52;
            this.btnDefineVisibilityExpression.Text = "Define";
            this.btnDefineVisibilityExpression.UseVisualStyleBackColor = true;
            this.btnDefineVisibilityExpression.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(14, 16);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(113, 15);
            this.label23.TabIndex = 51;
            this.label23.Text = "Visibility expression";
            // 
            // txtVisibilityExpression
            // 
            this.txtVisibilityExpression.Enabled = false;
            this.txtVisibilityExpression.Location = new System.Drawing.Point(17, 37);
            this.txtVisibilityExpression.Multiline = true;
            this.txtVisibilityExpression.Name = "txtVisibilityExpression";
            this.txtVisibilityExpression.Size = new System.Drawing.Size(270, 107);
            this.txtVisibilityExpression.TabIndex = 50;
            // 
            // tabSelection
            // 
            this.tabSelection.Controls.Add(this.transpSelection);
            this.tabSelection.Controls.Add(this.label6);
            this.tabSelection.Controls.Add(this.label5);
            this.tabSelection.Controls.Add(this.shapeContainer1);
            this.tabSelection.Location = new System.Drawing.Point(4, 24);
            this.tabSelection.Name = "tabSelection";
            this.tabSelection.Padding = new System.Windows.Forms.Padding(3);
            this.tabSelection.Size = new System.Drawing.Size(365, 199);
            this.tabSelection.TabIndex = 3;
            this.tabSelection.Text = "Selection appearance";
            this.tabSelection.UseVisualStyleBackColor = true;
            // 
            // transpSelection
            // 
            this.transpSelection.BandColor = System.Drawing.Color.Empty;
            this.transpSelection.Location = new System.Drawing.Point(129, 76);
            this.transpSelection.MaximumSize = new System.Drawing.Size(1024, 32);
            this.transpSelection.MinimumSize = new System.Drawing.Size(128, 32);
            this.transpSelection.Name = "transpSelection";
            this.transpSelection.Size = new System.Drawing.Size(179, 32);
            this.transpSelection.TabIndex = 14;
            this.transpSelection.Value = ((byte)(255));
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(35, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "Color";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Transparency";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(3, 3);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(359, 193);
            this.shapeContainer1.TabIndex = 3;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape1
            // 
            this.rectangleShape1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.rectangleShape1.Location = new System.Drawing.Point(137, 32);
            this.rectangleShape1.Name = "rectangleShape1";
            this.rectangleShape1.Size = new System.Drawing.Size(29, 15);
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.lvLayerProps);
            this.tabProperties.Location = new System.Drawing.Point(4, 24);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(365, 199);
            this.tabProperties.TabIndex = 4;
            this.tabProperties.Text = "Properties";
            this.tabProperties.UseVisualStyleBackColor = true;
            // 
            // lvLayerProps
            // 
            this.lvLayerProps.ContextMenuStrip = this.menuShortCut;
            this.lvLayerProps.Location = new System.Drawing.Point(5, 3);
            this.lvLayerProps.Name = "lvLayerProps";
            this.lvLayerProps.Size = new System.Drawing.Size(357, 193);
            this.lvLayerProps.TabIndex = 0;
            this.lvLayerProps.UseCompatibleStateImageBehavior = false;
            // 
            // btnApply
            // 
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(247, 368);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(60, 27);
            this.btnApply.TabIndex = 14;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // menuShortCut
            // 
            this.menuShortCut.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSavePropsToFile});
            this.menuShortCut.Name = "menuShortCut";
            this.menuShortCut.Size = new System.Drawing.Size(181, 48);
            this.menuShortCut.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnMenuItemClicked);
            // 
            // menuSavePropsToFile
            // 
            this.menuSavePropsToFile.Name = "menuSavePropsToFile";
            this.menuSavePropsToFile.Size = new System.Drawing.Size(180, 22);
            this.menuSavePropsToFile.Text = "Save to file...";
            // 
            // LayerPropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 403);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
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
            this.ShowInTaskbar = false;
            this.Text = "Layer properties";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnLayerPropertyForm_FormClosed);
            this.Load += new System.EventHandler(this.OnLayerPropertyForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabAppearance.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabVisibility.ResumeLayout(false);
            this.tabVisibility.PerformLayout();
            this.tabSelection.ResumeLayout(false);
            this.tabSelection.PerformLayout();
            this.tabProperties.ResumeLayout(false);
            this.menuShortCut.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabAppearance;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnFeatureSymbols;
        private System.Windows.Forms.Button btnFeatureCategories;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLabelFeatures;
        private System.Windows.Forms.Button btnLabelCategories;
        private System.Windows.Forms.TabPage tabCategories;
        private System.Windows.Forms.TabPage tabVisibility;
        private System.Windows.Forms.Button btnDefineVisibilityExpression;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtVisibilityExpression;
        private System.Windows.Forms.Button btnApplyVisibility;
        private System.Windows.Forms.TabPage tabSelection;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape1;
        private Mapping.UserControls.TransparencyControl transpSelection;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TabPage tabProperties;
        private System.Windows.Forms.ListView lvLayerProps;
        private System.Windows.Forms.ContextMenuStrip menuShortCut;
        private System.Windows.Forms.ToolStripMenuItem menuSavePropsToFile;
    }
}