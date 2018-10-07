namespace FAD3.Mapping.Forms
{
    partial class PolygonLayerSymbologyForm
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
            this.tabProperties = new System.Windows.Forms.TabControl();
            this.tabFill = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.grpGradient = new System.Windows.Forms.GroupBox();
            this.clpGradient2 = new Owf.Controls.Office2007ColorPicker(this.components);
            this.label11 = new System.Windows.Forms.Label();
            this.udGradientRotation = new MWLite.Symbology.Controls.NumericUpDownExt(this.components);
            this.cboGradientBounds = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cboGradientType = new System.Windows.Forms.ComboBox();
            this.grpHatch = new System.Windows.Forms.GroupBox();
            this.chkTransparentBackground = new System.Windows.Forms.CheckBox();
            this.clpHatchBack = new Owf.Controls.Office2007ColorPicker(this.components);
            this.lblHatch = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.icbHatchType = new FAD3.Mapping.UserControls.ImageCombo();
            this.cboFillType = new System.Windows.Forms.ComboBox();
            this.clpFillColor = new Owf.Controls.Office2007ColorPicker(this.components);
            this.transpFill = new FAD3.Mapping.UserControls.TransparencyControl();
            this.tabOutline = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.icbLineStyle = new FAD3.Mapping.UserControls.ImageCombo();
            this.icbLineWidth = new FAD3.Mapping.UserControls.ImageCombo();
            this.transpOutline = new FAD3.Mapping.UserControls.TransparencyControl();
            this.clpOutline = new Owf.Controls.Office2007ColorPicker(this.components);
            this.tabVertices = new System.Windows.Forms.TabPage();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pctPreview = new System.Windows.Forms.PictureBox();
            this.chkFill = new System.Windows.Forms.CheckBox();
            this.chkOutline = new System.Windows.Forms.CheckBox();
            this.chkVertices = new System.Windows.Forms.CheckBox();
            this.udVerticesSize = new MWLite.Symbology.Controls.NumericUpDownExt(this.components);
            this.chkVerticesFillVisible = new System.Windows.Forms.CheckBox();
            this.label29 = new System.Windows.Forms.Label();
            this.cboVerticesType = new System.Windows.Forms.ComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.clpVertexColor = new Owf.Controls.Office2007ColorPicker(this.components);
            this.tabProperties.SuspendLayout();
            this.tabFill.SuspendLayout();
            this.grpGradient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udGradientRotation)).BeginInit();
            this.grpHatch.SuspendLayout();
            this.tabOutline.SuspendLayout();
            this.tabVertices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udVerticesSize)).BeginInit();
            this.SuspendLayout();
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.tabFill);
            this.tabProperties.Controls.Add(this.tabOutline);
            this.tabProperties.Controls.Add(this.tabVertices);
            this.tabProperties.Location = new System.Drawing.Point(160, 18);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.SelectedIndex = 0;
            this.tabProperties.Size = new System.Drawing.Size(392, 359);
            this.tabProperties.TabIndex = 0;
            // 
            // tabFill
            // 
            this.tabFill.Controls.Add(this.label6);
            this.tabFill.Controls.Add(this.label5);
            this.tabFill.Controls.Add(this.label4);
            this.tabFill.Controls.Add(this.grpGradient);
            this.tabFill.Controls.Add(this.grpHatch);
            this.tabFill.Controls.Add(this.cboFillType);
            this.tabFill.Controls.Add(this.clpFillColor);
            this.tabFill.Controls.Add(this.transpFill);
            this.tabFill.Location = new System.Drawing.Point(4, 22);
            this.tabFill.Name = "tabFill";
            this.tabFill.Padding = new System.Windows.Forms.Padding(3);
            this.tabFill.Size = new System.Drawing.Size(384, 333);
            this.tabFill.TabIndex = 0;
            this.tabFill.Text = "Fill";
            this.tabFill.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(56, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Fill color";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(56, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Transparency";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Type of fill";
            // 
            // grpGradient
            // 
            this.grpGradient.Controls.Add(this.clpGradient2);
            this.grpGradient.Controls.Add(this.label11);
            this.grpGradient.Controls.Add(this.udGradientRotation);
            this.grpGradient.Controls.Add(this.cboGradientBounds);
            this.grpGradient.Controls.Add(this.label7);
            this.grpGradient.Controls.Add(this.label26);
            this.grpGradient.Controls.Add(this.label8);
            this.grpGradient.Controls.Add(this.cboGradientType);
            this.grpGradient.Location = new System.Drawing.Point(209, 36);
            this.grpGradient.Name = "grpGradient";
            this.grpGradient.Size = new System.Drawing.Size(364, 125);
            this.grpGradient.TabIndex = 4;
            this.grpGradient.TabStop = false;
            this.grpGradient.Text = "Gradient";
            this.grpGradient.Visible = false;
            // 
            // clpGradient2
            // 
            this.clpGradient2.Color = System.Drawing.Color.Black;
            this.clpGradient2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.clpGradient2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clpGradient2.FormattingEnabled = true;
            this.clpGradient2.Items.AddRange(new object[] {
            "Color"});
            this.clpGradient2.Location = new System.Drawing.Point(101, 95);
            this.clpGradient2.Name = "clpGradient2";
            this.clpGradient2.Size = new System.Drawing.Size(50, 21);
            this.clpGradient2.TabIndex = 108;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 98);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 13);
            this.label11.TabIndex = 107;
            this.label11.Text = "Color 2";
            // 
            // udGradientRotation
            // 
            this.udGradientRotation.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.udGradientRotation.Location = new System.Drawing.Point(288, 20);
            this.udGradientRotation.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.udGradientRotation.Name = "udGradientRotation";
            this.udGradientRotation.Size = new System.Drawing.Size(56, 20);
            this.udGradientRotation.TabIndex = 106;
            // 
            // cboGradientBounds
            // 
            this.cboGradientBounds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGradientBounds.FormattingEnabled = true;
            this.cboGradientBounds.Location = new System.Drawing.Point(101, 57);
            this.cboGradientBounds.Name = "cboGradientBounds";
            this.cboGradientBounds.Size = new System.Drawing.Size(131, 21);
            this.cboGradientBounds.TabIndex = 105;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 104;
            this.label7.Text = "Gradient bounds";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(248, 22);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(34, 13);
            this.label26.TabIndex = 103;
            this.label26.Text = "Angle";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 13);
            this.label8.TabIndex = 101;
            this.label8.Text = "Gradient type";
            // 
            // cboGradientType
            // 
            this.cboGradientType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGradientType.FormattingEnabled = true;
            this.cboGradientType.Location = new System.Drawing.Point(101, 19);
            this.cboGradientType.Name = "cboGradientType";
            this.cboGradientType.Size = new System.Drawing.Size(131, 21);
            this.cboGradientType.TabIndex = 102;
            // 
            // grpHatch
            // 
            this.grpHatch.Controls.Add(this.chkTransparentBackground);
            this.grpHatch.Controls.Add(this.clpHatchBack);
            this.grpHatch.Controls.Add(this.lblHatch);
            this.grpHatch.Controls.Add(this.label2);
            this.grpHatch.Controls.Add(this.icbHatchType);
            this.grpHatch.Location = new System.Drawing.Point(10, 167);
            this.grpHatch.Name = "grpHatch";
            this.grpHatch.Size = new System.Drawing.Size(364, 125);
            this.grpHatch.TabIndex = 3;
            this.grpHatch.TabStop = false;
            this.grpHatch.Text = "Hatch";
            this.grpHatch.Visible = false;
            // 
            // chkTransparentBackground
            // 
            this.chkTransparentBackground.AutoSize = true;
            this.chkTransparentBackground.Location = new System.Drawing.Point(174, 67);
            this.chkTransparentBackground.Name = "chkTransparentBackground";
            this.chkTransparentBackground.Size = new System.Drawing.Size(83, 17);
            this.chkTransparentBackground.TabIndex = 5;
            this.chkTransparentBackground.Text = "Transparent";
            this.chkTransparentBackground.UseVisualStyleBackColor = true;
            // 
            // clpHatchBack
            // 
            this.clpHatchBack.Color = System.Drawing.Color.Black;
            this.clpHatchBack.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.clpHatchBack.DropDownHeight = 1;
            this.clpHatchBack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clpHatchBack.FormattingEnabled = true;
            this.clpHatchBack.IntegralHeight = false;
            this.clpHatchBack.Items.AddRange(new object[] {
            "Color"});
            this.clpHatchBack.Location = new System.Drawing.Point(107, 64);
            this.clpHatchBack.Name = "clpHatchBack";
            this.clpHatchBack.Size = new System.Drawing.Size(50, 21);
            this.clpHatchBack.TabIndex = 3;
            // 
            // lblHatch
            // 
            this.lblHatch.AutoSize = true;
            this.lblHatch.Location = new System.Drawing.Point(18, 67);
            this.lblHatch.Name = "lblHatch";
            this.lblHatch.Size = new System.Drawing.Size(65, 13);
            this.lblHatch.TabIndex = 2;
            this.lblHatch.Text = "Background";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Hatch type";
            // 
            // icbHatchType
            // 
            this.icbHatchType.Color1 = System.Drawing.Color.Gray;
            this.icbHatchType.Color2 = System.Drawing.Color.Gray;
            this.icbHatchType.ColorSchemes = null;
            this.icbHatchType.ComboStyle = FAD3.Mapping.UserControls.ImageComboStyle.Common;
            this.icbHatchType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.icbHatchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.icbHatchType.FormattingEnabled = true;
            this.icbHatchType.Location = new System.Drawing.Point(107, 25);
            this.icbHatchType.Name = "icbHatchType";
            this.icbHatchType.OutlineColor = System.Drawing.Color.Black;
            this.icbHatchType.Size = new System.Drawing.Size(136, 21);
            this.icbHatchType.TabIndex = 0;
            // 
            // cboFillType
            // 
            this.cboFillType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFillType.FormattingEnabled = true;
            this.cboFillType.Location = new System.Drawing.Point(166, 29);
            this.cboFillType.Name = "cboFillType";
            this.cboFillType.Size = new System.Drawing.Size(128, 21);
            this.cboFillType.TabIndex = 2;
            this.cboFillType.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            // 
            // clpFillColor
            // 
            this.clpFillColor.Color = System.Drawing.Color.Black;
            this.clpFillColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.clpFillColor.DropDownHeight = 1;
            this.clpFillColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clpFillColor.FormattingEnabled = true;
            this.clpFillColor.IntegralHeight = false;
            this.clpFillColor.Items.AddRange(new object[] {
            "Color"});
            this.clpFillColor.Location = new System.Drawing.Point(166, 110);
            this.clpFillColor.Name = "clpFillColor";
            this.clpFillColor.Size = new System.Drawing.Size(50, 21);
            this.clpFillColor.TabIndex = 1;
            // 
            // transpFill
            // 
            this.transpFill.BandColor = System.Drawing.Color.Empty;
            this.transpFill.Location = new System.Drawing.Point(159, 72);
            this.transpFill.MaximumSize = new System.Drawing.Size(1024, 32);
            this.transpFill.MinimumSize = new System.Drawing.Size(128, 32);
            this.transpFill.Name = "transpFill";
            this.transpFill.Size = new System.Drawing.Size(146, 32);
            this.transpFill.TabIndex = 0;
            this.transpFill.Value = ((byte)(255));
            // 
            // tabOutline
            // 
            this.tabOutline.Controls.Add(this.label12);
            this.tabOutline.Controls.Add(this.label10);
            this.tabOutline.Controls.Add(this.label9);
            this.tabOutline.Controls.Add(this.label3);
            this.tabOutline.Controls.Add(this.icbLineStyle);
            this.tabOutline.Controls.Add(this.icbLineWidth);
            this.tabOutline.Controls.Add(this.transpOutline);
            this.tabOutline.Controls.Add(this.clpOutline);
            this.tabOutline.Location = new System.Drawing.Point(4, 22);
            this.tabOutline.Name = "tabOutline";
            this.tabOutline.Padding = new System.Windows.Forms.Padding(3);
            this.tabOutline.Size = new System.Drawing.Size(384, 333);
            this.tabOutline.TabIndex = 1;
            this.tabOutline.Text = "Outline";
            this.tabOutline.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 186);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(30, 13);
            this.label12.TabIndex = 116;
            this.label12.Text = "Style";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 144);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 115;
            this.label10.Text = "Width";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 114;
            this.label9.Text = "Transparency";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 113;
            this.label3.Text = "Color";
            // 
            // icbLineStyle
            // 
            this.icbLineStyle.Color1 = System.Drawing.Color.Gray;
            this.icbLineStyle.Color2 = System.Drawing.Color.Gray;
            this.icbLineStyle.ColorSchemes = null;
            this.icbLineStyle.ComboStyle = FAD3.Mapping.UserControls.ImageComboStyle.Common;
            this.icbLineStyle.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.icbLineStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.icbLineStyle.FormattingEnabled = true;
            this.icbLineStyle.Location = new System.Drawing.Point(99, 183);
            this.icbLineStyle.Name = "icbLineStyle";
            this.icbLineStyle.OutlineColor = System.Drawing.Color.Black;
            this.icbLineStyle.Size = new System.Drawing.Size(80, 21);
            this.icbLineStyle.TabIndex = 112;
            // 
            // icbLineWidth
            // 
            this.icbLineWidth.Color1 = System.Drawing.Color.Gray;
            this.icbLineWidth.Color2 = System.Drawing.Color.Gray;
            this.icbLineWidth.ColorSchemes = null;
            this.icbLineWidth.ComboStyle = FAD3.Mapping.UserControls.ImageComboStyle.Common;
            this.icbLineWidth.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.icbLineWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.icbLineWidth.FormattingEnabled = true;
            this.icbLineWidth.Location = new System.Drawing.Point(99, 141);
            this.icbLineWidth.Name = "icbLineWidth";
            this.icbLineWidth.OutlineColor = System.Drawing.Color.Black;
            this.icbLineWidth.Size = new System.Drawing.Size(80, 21);
            this.icbLineWidth.TabIndex = 111;
            // 
            // transpOutline
            // 
            this.transpOutline.BandColor = System.Drawing.Color.Empty;
            this.transpOutline.Location = new System.Drawing.Point(90, 77);
            this.transpOutline.MaximumSize = new System.Drawing.Size(1024, 32);
            this.transpOutline.MinimumSize = new System.Drawing.Size(128, 32);
            this.transpOutline.Name = "transpOutline";
            this.transpOutline.Size = new System.Drawing.Size(146, 32);
            this.transpOutline.TabIndex = 110;
            this.transpOutline.Value = ((byte)(255));
            // 
            // clpOutline
            // 
            this.clpOutline.Color = System.Drawing.Color.Black;
            this.clpOutline.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.clpOutline.DropDownHeight = 1;
            this.clpOutline.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clpOutline.FormattingEnabled = true;
            this.clpOutline.IntegralHeight = false;
            this.clpOutline.Items.AddRange(new object[] {
            "Color"});
            this.clpOutline.Location = new System.Drawing.Point(99, 36);
            this.clpOutline.Name = "clpOutline";
            this.clpOutline.Size = new System.Drawing.Size(50, 21);
            this.clpOutline.TabIndex = 109;
            // 
            // tabVertices
            // 
            this.tabVertices.Controls.Add(this.clpVertexColor);
            this.tabVertices.Controls.Add(this.udVerticesSize);
            this.tabVertices.Controls.Add(this.chkVerticesFillVisible);
            this.tabVertices.Controls.Add(this.label29);
            this.tabVertices.Controls.Add(this.cboVerticesType);
            this.tabVertices.Controls.Add(this.label28);
            this.tabVertices.Controls.Add(this.label27);
            this.tabVertices.Location = new System.Drawing.Point(4, 22);
            this.tabVertices.Name = "tabVertices";
            this.tabVertices.Padding = new System.Windows.Forms.Padding(3);
            this.tabVertices.Size = new System.Drawing.Size(384, 333);
            this.tabVertices.TabIndex = 2;
            this.tabVertices.Text = "Vertices";
            this.tabVertices.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(444, 384);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(54, 27);
            this.btnApply.TabIndex = 9;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(386, 384);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(54, 27);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(502, 384);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(45, 27);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Preview:";
            // 
            // pctPreview
            // 
            this.pctPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pctPreview.Location = new System.Drawing.Point(12, 35);
            this.pctPreview.Name = "pctPreview";
            this.pctPreview.Size = new System.Drawing.Size(127, 127);
            this.pctPreview.TabIndex = 10;
            this.pctPreview.TabStop = false;
            // 
            // chkFill
            // 
            this.chkFill.AutoSize = true;
            this.chkFill.Location = new System.Drawing.Point(33, 207);
            this.chkFill.Name = "chkFill";
            this.chkFill.Size = new System.Drawing.Size(65, 17);
            this.chkFill.TabIndex = 12;
            this.chkFill.Text = "Show fill";
            this.chkFill.UseVisualStyleBackColor = true;
            // 
            // chkOutline
            // 
            this.chkOutline.AutoSize = true;
            this.chkOutline.Location = new System.Drawing.Point(33, 246);
            this.chkOutline.Name = "chkOutline";
            this.chkOutline.Size = new System.Drawing.Size(87, 17);
            this.chkOutline.TabIndex = 13;
            this.chkOutline.Text = "Show outline";
            this.chkOutline.UseVisualStyleBackColor = true;
            // 
            // chkVertices
            // 
            this.chkVertices.AutoSize = true;
            this.chkVertices.Location = new System.Drawing.Point(33, 284);
            this.chkVertices.Name = "chkVertices";
            this.chkVertices.Size = new System.Drawing.Size(94, 17);
            this.chkVertices.TabIndex = 14;
            this.chkVertices.Text = "Show Vertices";
            this.chkVertices.UseVisualStyleBackColor = true;
            // 
            // udVerticesSize
            // 
            this.udVerticesSize.Location = new System.Drawing.Point(283, 69);
            this.udVerticesSize.Name = "udVerticesSize";
            this.udVerticesSize.Size = new System.Drawing.Size(57, 20);
            this.udVerticesSize.TabIndex = 23;
            // 
            // chkVerticesFillVisible
            // 
            this.chkVerticesFillVisible.AutoSize = true;
            this.chkVerticesFillVisible.Location = new System.Drawing.Point(234, 119);
            this.chkVerticesFillVisible.Name = "chkVerticesFillVisible";
            this.chkVerticesFillVisible.Size = new System.Drawing.Size(70, 17);
            this.chkVerticesFillVisible.TabIndex = 22;
            this.chkVerticesFillVisible.Text = "Fill visible";
            this.chkVerticesFillVisible.UseVisualStyleBackColor = true;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(231, 71);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(27, 13);
            this.label29.TabIndex = 21;
            this.label29.Text = "Size";
            // 
            // cboVerticesType
            // 
            this.cboVerticesType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVerticesType.FormattingEnabled = true;
            this.cboVerticesType.Location = new System.Drawing.Point(91, 116);
            this.cboVerticesType.Name = "cboVerticesType";
            this.cboVerticesType.Size = new System.Drawing.Size(72, 21);
            this.cboVerticesType.TabIndex = 20;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(50, 119);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(31, 13);
            this.label28.TabIndex = 19;
            this.label28.Text = "Type";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(50, 72);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(31, 13);
            this.label27.TabIndex = 18;
            this.label27.Text = "Color";
            // 
            // clpVertexColor
            // 
            this.clpVertexColor.Color = System.Drawing.Color.Black;
            this.clpVertexColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.clpVertexColor.DropDownHeight = 1;
            this.clpVertexColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clpVertexColor.FormattingEnabled = true;
            this.clpVertexColor.IntegralHeight = false;
            this.clpVertexColor.Items.AddRange(new object[] {
            "Color"});
            this.clpVertexColor.Location = new System.Drawing.Point(91, 68);
            this.clpVertexColor.Name = "clpVertexColor";
            this.clpVertexColor.Size = new System.Drawing.Size(50, 21);
            this.clpVertexColor.TabIndex = 110;
            // 
            // PolygonLayerSymbologyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 418);
            this.Controls.Add(this.chkVertices);
            this.Controls.Add(this.chkOutline);
            this.Controls.Add(this.chkFill);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pctPreview);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabProperties);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PolygonLayerSymbologyForm";
            this.Text = "PolygonLayerSymbologyForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.tabProperties.ResumeLayout(false);
            this.tabFill.ResumeLayout(false);
            this.tabFill.PerformLayout();
            this.grpGradient.ResumeLayout(false);
            this.grpGradient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udGradientRotation)).EndInit();
            this.grpHatch.ResumeLayout(false);
            this.grpHatch.PerformLayout();
            this.tabOutline.ResumeLayout(false);
            this.tabOutline.PerformLayout();
            this.tabVertices.ResumeLayout(false);
            this.tabVertices.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udVerticesSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabProperties;
        private System.Windows.Forms.TabPage tabFill;
        private System.Windows.Forms.TabPage tabOutline;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pctPreview;
        private UserControls.TransparencyControl transpFill;
        private Owf.Controls.Office2007ColorPicker clpFillColor;
        private System.Windows.Forms.CheckBox chkFill;
        private System.Windows.Forms.CheckBox chkOutline;
        private System.Windows.Forms.CheckBox chkVertices;
        private System.Windows.Forms.TabPage tabVertices;
        private System.Windows.Forms.ComboBox cboFillType;
        private System.Windows.Forms.GroupBox grpGradient;
        private System.Windows.Forms.GroupBox grpHatch;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkTransparentBackground;
        private Owf.Controls.Office2007ColorPicker clpHatchBack;
        private System.Windows.Forms.Label lblHatch;
        private System.Windows.Forms.Label label2;
        private UserControls.ImageCombo icbHatchType;
        private System.Windows.Forms.Label label11;
        private MWLite.Symbology.Controls.NumericUpDownExt udGradientRotation;
        private System.Windows.Forms.ComboBox cboGradientBounds;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboGradientType;
        private Owf.Controls.Office2007ColorPicker clpGradient2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private UserControls.ImageCombo icbLineStyle;
        private UserControls.ImageCombo icbLineWidth;
        private UserControls.TransparencyControl transpOutline;
        private Owf.Controls.Office2007ColorPicker clpOutline;
        private Owf.Controls.Office2007ColorPicker clpVertexColor;
        private MWLite.Symbology.Controls.NumericUpDownExt udVerticesSize;
        private System.Windows.Forms.CheckBox chkVerticesFillVisible;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox cboVerticesType;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
    }
}