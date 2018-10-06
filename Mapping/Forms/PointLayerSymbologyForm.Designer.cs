namespace FAD3
{
    partial class PointLayerSymbologyForm
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabSettings = new System.Windows.Forms.TabControl();
            this.tabPointSymbol = new System.Windows.Forms.TabPage();
            this.udSideRatio = new MWLite.Symbology.Controls.NumericUpDownExt(this.components);
            this.udNumberOfSides = new MWLite.Symbology.Controls.NumericUpDownExt(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboPointType = new FAD3.Mapping.UserControls.ImageCombo();
            this.symbolControl1 = new FAD3.Mapping.UserControls.SymbolControl();
            this.tabFontSymbol = new System.Windows.Forms.TabPage();
            this.characterControl1 = new FAD3.Mapping.UserControls.CharacterControl();
            this.lblFont = new System.Windows.Forms.Label();
            this.comboCharacterFont = new System.Windows.Forms.ComboBox();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chkOutlineVisible = new System.Windows.Forms.CheckBox();
            this.chkFillVisible = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rectOutlineColor = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectSymbolColor = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.btnApply = new System.Windows.Forms.Button();
            this.comboLineWidth = new FAD3.Mapping.UserControls.ImageCombo();
            this.udTransparency = new MWLite.Symbology.Controls.NumericUpDownExt(this.components);
            this.udRotation = new MWLite.Symbology.Controls.NumericUpDownExt(this.components);
            this.udSize = new MWLite.Symbology.Controls.NumericUpDownExt(this.components);
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.tabSettings.SuspendLayout();
            this.tabPointSymbol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udSideRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udNumberOfSides)).BeginInit();
            this.tabFontSymbol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTransparency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRotation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udSize)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(507, 383);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(45, 27);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(391, 383);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(54, 27);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.tabPointSymbol);
            this.tabSettings.Controls.Add(this.tabFontSymbol);
            this.tabSettings.Location = new System.Drawing.Point(160, 18);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.SelectedIndex = 0;
            this.tabSettings.Size = new System.Drawing.Size(392, 359);
            this.tabSettings.TabIndex = 2;
            // 
            // tabPointSymbol
            // 
            this.tabPointSymbol.Controls.Add(this.udSideRatio);
            this.tabPointSymbol.Controls.Add(this.udNumberOfSides);
            this.tabPointSymbol.Controls.Add(this.label5);
            this.tabPointSymbol.Controls.Add(this.label7);
            this.tabPointSymbol.Controls.Add(this.label6);
            this.tabPointSymbol.Controls.Add(this.comboPointType);
            this.tabPointSymbol.Controls.Add(this.symbolControl1);
            this.tabPointSymbol.Location = new System.Drawing.Point(4, 22);
            this.tabPointSymbol.Name = "tabPointSymbol";
            this.tabPointSymbol.Padding = new System.Windows.Forms.Padding(3);
            this.tabPointSymbol.Size = new System.Drawing.Size(384, 333);
            this.tabPointSymbol.TabIndex = 0;
            this.tabPointSymbol.Text = "Point symbol";
            this.tabPointSymbol.UseVisualStyleBackColor = true;
            // 
            // udSideRatio
            // 
            this.udSideRatio.Location = new System.Drawing.Point(173, 118);
            this.udSideRatio.Name = "udSideRatio";
            this.udSideRatio.Size = new System.Drawing.Size(49, 20);
            this.udSideRatio.TabIndex = 15;
            this.udSideRatio.ValueChanged += new System.EventHandler(this.ApplyOptionsToGUI);
            // 
            // udNumberOfSides
            // 
            this.udNumberOfSides.Location = new System.Drawing.Point(173, 80);
            this.udNumberOfSides.Name = "udNumberOfSides";
            this.udNumberOfSides.Size = new System.Drawing.Size(49, 20);
            this.udNumberOfSides.TabIndex = 14;
            this.udNumberOfSides.ValueChanged += new System.EventHandler(this.ApplyOptionsToGUI);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(51, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Point type";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(51, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Side ratio";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(51, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Number of sides";
            // 
            // comboPointType
            // 
            this.comboPointType.Color1 = System.Drawing.Color.Gray;
            this.comboPointType.Color2 = System.Drawing.Color.Gray;
            this.comboPointType.ColorSchemes = null;
            this.comboPointType.ComboStyle = FAD3.Mapping.UserControls.ImageComboStyle.Common;
            this.comboPointType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboPointType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPointType.FormattingEnabled = true;
            this.comboPointType.Location = new System.Drawing.Point(173, 31);
            this.comboPointType.Name = "comboPointType";
            this.comboPointType.OutlineColor = System.Drawing.Color.Black;
            this.comboPointType.Size = new System.Drawing.Size(143, 21);
            this.comboPointType.TabIndex = 12;
            // 
            // symbolControl1
            // 
            this.symbolControl1.BackColor = System.Drawing.Color.Transparent;
            this.symbolControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.symbolControl1.CellHeight = 24;
            this.symbolControl1.CellWidth = 24;
            this.symbolControl1.Font = new System.Drawing.Font("Arial", 25.6F);
            this.symbolControl1.GridColor = System.Drawing.Color.Black;
            this.symbolControl1.GridVisible = false;
            this.symbolControl1.ItemCount = 17;
            this.symbolControl1.Location = new System.Drawing.Point(52, 184);
            this.symbolControl1.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.symbolControl1.Name = "symbolControl1";
            this.symbolControl1.SelectedIndex = -1;
            this.symbolControl1.Size = new System.Drawing.Size(276, 97);
            this.symbolControl1.TabIndex = 0;
            // 
            // tabFontSymbol
            // 
            this.tabFontSymbol.Controls.Add(this.characterControl1);
            this.tabFontSymbol.Controls.Add(this.lblFont);
            this.tabFontSymbol.Controls.Add(this.comboCharacterFont);
            this.tabFontSymbol.Location = new System.Drawing.Point(4, 22);
            this.tabFontSymbol.Name = "tabFontSymbol";
            this.tabFontSymbol.Padding = new System.Windows.Forms.Padding(3);
            this.tabFontSymbol.Size = new System.Drawing.Size(384, 333);
            this.tabFontSymbol.TabIndex = 1;
            this.tabFontSymbol.Text = "Character symbols";
            this.tabFontSymbol.UseVisualStyleBackColor = true;
            // 
            // characterControl1
            // 
            this.characterControl1.BackColor = System.Drawing.Color.Transparent;
            this.characterControl1.CellHeight = 32;
            this.characterControl1.CellWidth = 32;
            this.characterControl1.Font = new System.Drawing.Font("Arial", 25.6F);
            this.characterControl1.GridColor = System.Drawing.Color.Black;
            this.characterControl1.GridVisible = true;
            this.characterControl1.ItemCount = 224;
            this.characterControl1.Location = new System.Drawing.Point(10, 40);
            this.characterControl1.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.characterControl1.Name = "characterControl1";
            this.characterControl1.SelectedCharacterCode = ((byte)(0));
            this.characterControl1.SelectedIndex = -1;
            this.characterControl1.Size = new System.Drawing.Size(356, 262);
            this.characterControl1.TabIndex = 3;
            // 
            // lblFont
            // 
            this.lblFont.AutoSize = true;
            this.lblFont.Location = new System.Drawing.Point(11, 14);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(88, 13);
            this.lblFont.TabIndex = 2;
            this.lblFont.Text = "Character symbol";
            // 
            // comboCharacterFont
            // 
            this.comboCharacterFont.FormattingEnabled = true;
            this.comboCharacterFont.Location = new System.Drawing.Point(104, 9);
            this.comboCharacterFont.Name = "comboCharacterFont";
            this.comboCharacterFont.Size = new System.Drawing.Size(225, 21);
            this.comboCharacterFont.TabIndex = 1;
            this.comboCharacterFont.SelectedIndexChanged += new System.EventHandler(this.OnComboFontSelectionIndexChanged);
            // 
            // picPreview
            // 
            this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPreview.Location = new System.Drawing.Point(12, 34);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(127, 77);
            this.picPreview.TabIndex = 3;
            this.picPreview.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Preview:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(29, 344);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Width";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(29, 316);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Color";
            // 
            // chkOutlineVisible
            // 
            this.chkOutlineVisible.AutoSize = true;
            this.chkOutlineVisible.Location = new System.Drawing.Point(16, 289);
            this.chkOutlineVisible.Name = "chkOutlineVisible";
            this.chkOutlineVisible.Size = new System.Drawing.Size(91, 17);
            this.chkOutlineVisible.TabIndex = 19;
            this.chkOutlineVisible.Text = "Outline visible";
            this.chkOutlineVisible.UseVisualStyleBackColor = true;
            this.chkOutlineVisible.CheckedChanged += new System.EventHandler(this.ApplyOptionsToGUI);
            // 
            // chkFillVisible
            // 
            this.chkFillVisible.AutoSize = true;
            this.chkFillVisible.Location = new System.Drawing.Point(16, 233);
            this.chkFillVisible.Name = "chkFillVisible";
            this.chkFillVisible.Size = new System.Drawing.Size(70, 17);
            this.chkFillVisible.TabIndex = 7;
            this.chkFillVisible.Text = "Fill visible";
            this.chkFillVisible.UseVisualStyleBackColor = true;
            this.chkFillVisible.CheckedChanged += new System.EventHandler(this.ApplyOptionsToGUI);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 193);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Transparency";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Color";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Rotation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Size";
            // 
            // rectOutlineColor
            // 
            this.rectOutlineColor.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.rectOutlineColor.Location = new System.Drawing.Point(87, 315);
            this.rectOutlineColor.Name = "rectOutlineColor";
            this.rectOutlineColor.Size = new System.Drawing.Size(26, 15);
            this.rectOutlineColor.DoubleClick += new System.EventHandler(this.OnColorDoubleClick);
            // 
            // rectSymbolColor
            // 
            this.rectSymbolColor.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.rectSymbolColor.Location = new System.Drawing.Point(89, 255);
            this.rectSymbolColor.Name = "rectSymbolColor";
            this.rectSymbolColor.Size = new System.Drawing.Size(26, 15);
            this.rectSymbolColor.DoubleClick += new System.EventHandler(this.OnColorDoubleClick);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(449, 383);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(54, 27);
            this.btnApply.TabIndex = 6;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // comboLineWidth
            // 
            this.comboLineWidth.Color1 = System.Drawing.Color.Gray;
            this.comboLineWidth.Color2 = System.Drawing.Color.Gray;
            this.comboLineWidth.ColorSchemes = null;
            this.comboLineWidth.ComboStyle = FAD3.Mapping.UserControls.ImageComboStyle.Common;
            this.comboLineWidth.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboLineWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLineWidth.FormattingEnabled = true;
            this.comboLineWidth.Location = new System.Drawing.Point(87, 341);
            this.comboLineWidth.Name = "comboLineWidth";
            this.comboLineWidth.OutlineColor = System.Drawing.Color.Black;
            this.comboLineWidth.Size = new System.Drawing.Size(54, 21);
            this.comboLineWidth.TabIndex = 21;
            this.comboLineWidth.SelectedIndexChanged += new System.EventHandler(this.ApplyOptionsToGUI);
            // 
            // udTransparency
            // 
            this.udTransparency.Location = new System.Drawing.Point(89, 191);
            this.udTransparency.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.udTransparency.Name = "udTransparency";
            this.udTransparency.Size = new System.Drawing.Size(49, 20);
            this.udTransparency.TabIndex = 18;
            this.udTransparency.ValueChanged += new System.EventHandler(this.ApplyOptionsToGUI);
            // 
            // udRotation
            // 
            this.udRotation.Location = new System.Drawing.Point(90, 160);
            this.udRotation.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.udRotation.Name = "udRotation";
            this.udRotation.Size = new System.Drawing.Size(49, 20);
            this.udRotation.TabIndex = 17;
            this.udRotation.ValueChanged += new System.EventHandler(this.ApplyOptionsToGUI);
            // 
            // udSize
            // 
            this.udSize.Location = new System.Drawing.Point(90, 130);
            this.udSize.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udSize.Name = "udSize";
            this.udSize.Size = new System.Drawing.Size(49, 20);
            this.udSize.TabIndex = 16;
            this.udSize.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udSize.ValueChanged += new System.EventHandler(this.ApplyOptionsToGUI);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectOutlineColor,
            this.rectSymbolColor});
            this.shapeContainer1.Size = new System.Drawing.Size(563, 422);
            this.shapeContainer1.TabIndex = 19;
            this.shapeContainer1.TabStop = false;
            // 
            // PointLayerSymbologyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 422);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.comboLineWidth);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.chkOutlineVisible);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkFillVisible);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.udTransparency);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.udRotation);
            this.Controls.Add(this.tabSettings);
            this.Controls.Add(this.udSize);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.shapeContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PointLayerSymbologyForm";
            this.Text = "LayerSymbologyForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.tabSettings.ResumeLayout(false);
            this.tabPointSymbol.ResumeLayout(false);
            this.tabPointSymbol.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udSideRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udNumberOfSides)).EndInit();
            this.tabFontSymbol.ResumeLayout(false);
            this.tabFontSymbol.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTransparency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRotation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabSettings;
        private System.Windows.Forms.TabPage tabPointSymbol;
        private System.Windows.Forms.TabPage tabFontSymbol;
        private System.Windows.Forms.ComboBox comboCharacterFont;
        private System.Windows.Forms.Label lblFont;
        private Mapping.UserControls.CharacterControl characterControl1;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectSymbolColor;
        private Mapping.UserControls.SymbolControl symbolControl1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label label5;
        private Mapping.UserControls.ImageCombo comboPointType;
        private System.Windows.Forms.Label label8;
        private MWLite.Symbology.Controls.NumericUpDownExt udSideRatio;
        private MWLite.Symbology.Controls.NumericUpDownExt udNumberOfSides;
        private MWLite.Symbology.Controls.NumericUpDownExt udTransparency;
        private MWLite.Symbology.Controls.NumericUpDownExt udRotation;
        private MWLite.Symbology.Controls.NumericUpDownExt udSize;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkOutlineVisible;
        private System.Windows.Forms.CheckBox chkFillVisible;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectOutlineColor;
        private Mapping.UserControls.ImageCombo comboLineWidth;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
    }
}