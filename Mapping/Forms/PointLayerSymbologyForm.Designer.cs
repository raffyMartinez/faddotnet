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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabsSymbology = new System.Windows.Forms.TabControl();
            this.tabPointSymbol = new System.Windows.Forms.TabPage();
            this.tabFontSymbol = new System.Windows.Forms.TabPage();
            this.lblFont = new System.Windows.Forms.Label();
            this.comboCharacterFont = new System.Windows.Forms.ComboBox();
            this.tabIconSymbols = new System.Windows.Forms.TabPage();
            this.tabOptions = new System.Windows.Forms.TabPage();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.characterControl1 = new FAD3.Mapping.UserControls.CharacterControl();
            this.symbolControl1 = new FAD3.Mapping.UserControls.SymbolControl();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabsSymbology.SuspendLayout();
            this.tabPointSymbol.SuspendLayout();
            this.tabFontSymbol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(507, 388);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(45, 27);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(447, 388);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(54, 27);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // tabsSymbology
            // 
            this.tabsSymbology.Controls.Add(this.tabPointSymbol);
            this.tabsSymbology.Controls.Add(this.tabFontSymbol);
            this.tabsSymbology.Controls.Add(this.tabIconSymbols);
            this.tabsSymbology.Controls.Add(this.tabOptions);
            this.tabsSymbology.Location = new System.Drawing.Point(167, 31);
            this.tabsSymbology.Name = "tabsSymbology";
            this.tabsSymbology.SelectedIndex = 0;
            this.tabsSymbology.Size = new System.Drawing.Size(384, 347);
            this.tabsSymbology.TabIndex = 2;
            // 
            // tabPointSymbol
            // 
            this.tabPointSymbol.Controls.Add(this.textBox5);
            this.tabPointSymbol.Controls.Add(this.label8);
            this.tabPointSymbol.Controls.Add(this.textBox4);
            this.tabPointSymbol.Controls.Add(this.label7);
            this.tabPointSymbol.Controls.Add(this.textBox3);
            this.tabPointSymbol.Controls.Add(this.label6);
            this.tabPointSymbol.Controls.Add(this.symbolControl1);
            this.tabPointSymbol.Location = new System.Drawing.Point(4, 22);
            this.tabPointSymbol.Name = "tabPointSymbol";
            this.tabPointSymbol.Padding = new System.Windows.Forms.Padding(3);
            this.tabPointSymbol.Size = new System.Drawing.Size(376, 321);
            this.tabPointSymbol.TabIndex = 0;
            this.tabPointSymbol.Text = "Point symbol";
            this.tabPointSymbol.UseVisualStyleBackColor = true;
            // 
            // tabFontSymbol
            // 
            this.tabFontSymbol.Controls.Add(this.characterControl1);
            this.tabFontSymbol.Controls.Add(this.lblFont);
            this.tabFontSymbol.Controls.Add(this.comboCharacterFont);
            this.tabFontSymbol.Location = new System.Drawing.Point(4, 22);
            this.tabFontSymbol.Name = "tabFontSymbol";
            this.tabFontSymbol.Padding = new System.Windows.Forms.Padding(3);
            this.tabFontSymbol.Size = new System.Drawing.Size(376, 321);
            this.tabFontSymbol.TabIndex = 1;
            this.tabFontSymbol.Text = "Character symbols";
            this.tabFontSymbol.UseVisualStyleBackColor = true;
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
            this.comboCharacterFont.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            // 
            // tabIconSymbols
            // 
            this.tabIconSymbols.Location = new System.Drawing.Point(4, 22);
            this.tabIconSymbols.Name = "tabIconSymbols";
            this.tabIconSymbols.Padding = new System.Windows.Forms.Padding(3);
            this.tabIconSymbols.Size = new System.Drawing.Size(376, 321);
            this.tabIconSymbols.TabIndex = 2;
            this.tabIconSymbols.Text = "Icons";
            this.tabIconSymbols.UseVisualStyleBackColor = true;
            // 
            // tabOptions
            // 
            this.tabOptions.Location = new System.Drawing.Point(4, 22);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptions.Size = new System.Drawing.Size(376, 321);
            this.tabOptions.TabIndex = 3;
            this.tabOptions.Text = "Options";
            this.tabOptions.UseVisualStyleBackColor = true;
            // 
            // picPreview
            // 
            this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPreview.Location = new System.Drawing.Point(12, 54);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(142, 142);
            this.picPreview.TabIndex = 3;
            this.picPreview.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Preview:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.shapeContainer1);
            this.groupBox1.Location = new System.Drawing.Point(12, 225);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(142, 148);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Appearance";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Color";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Rotation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Size";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(71, 70);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(52, 20);
            this.textBox2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(71, 31);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(52, 20);
            this.textBox1.TabIndex = 0;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(3, 16);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(136, 129);
            this.shapeContainer1.TabIndex = 8;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape1
            // 
            this.rectangleShape1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.rectangleShape1.Location = new System.Drawing.Point(71, 95);
            this.rectangleShape1.Name = "rectangleShape1";
            this.rectangleShape1.Size = new System.Drawing.Size(37, 18);
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
            this.symbolControl1.Location = new System.Drawing.Point(52, 203);
            this.symbolControl1.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.symbolControl1.Name = "symbolControl1";
            this.symbolControl1.SelectedIndex = -1;
            this.symbolControl1.Size = new System.Drawing.Size(276, 78);
            this.symbolControl1.TabIndex = 0;
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
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(173, 79);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(52, 20);
            this.textBox3.TabIndex = 6;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(173, 117);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(52, 20);
            this.textBox4.TabIndex = 8;
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
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(173, 153);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(52, 20);
            this.textBox5.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(51, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Transparency";
            // 
            // PointLayerSymbologyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 422);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.tabsSymbology);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PointLayerSymbologyForm";
            this.Text = "LayerSymbologyForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.tabsSymbology.ResumeLayout(false);
            this.tabPointSymbol.ResumeLayout(false);
            this.tabPointSymbol.PerformLayout();
            this.tabFontSymbol.ResumeLayout(false);
            this.tabFontSymbol.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabsSymbology;
        private System.Windows.Forms.TabPage tabPointSymbol;
        private System.Windows.Forms.TabPage tabFontSymbol;
        private System.Windows.Forms.TabPage tabIconSymbols;
        private System.Windows.Forms.TabPage tabOptions;
        private System.Windows.Forms.ComboBox comboCharacterFont;
        private System.Windows.Forms.Label lblFont;
        private Mapping.UserControls.CharacterControl characterControl1;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape1;
        private Mapping.UserControls.SymbolControl symbolControl1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label6;
    }
}