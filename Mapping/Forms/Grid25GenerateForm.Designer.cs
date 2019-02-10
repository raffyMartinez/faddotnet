namespace FAD3
{
    partial class Grid25GenerateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Grid25GenerateForm));
            this.txtMapTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupLabels = new System.Windows.Forms.GroupBox();
            this.btnLabelsFromKML = new System.Windows.Forms.Button();
            this.imList = new System.Windows.Forms.ImageList(this.components);
            this.buttonLabel = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chkBold = new System.Windows.Forms.CheckBox();
            this.chkWrapLabels = new System.Windows.Forms.CheckBox();
            this.chkBottom = new System.Windows.Forms.CheckBox();
            this.chkRight = new System.Windows.Forms.CheckBox();
            this.chkTop = new System.Windows.Forms.CheckBox();
            this.chkLeft = new System.Windows.Forms.CheckBox();
            this.txtMinorGridThickness = new System.Windows.Forms.TextBox();
            this.txtMajorGridThickness = new System.Windows.Forms.TextBox();
            this.txtBorderThickness = new System.Windows.Forms.TextBox();
            this.txtMajorGridLabelSize = new System.Windows.Forms.TextBox();
            this.txtMinorGridLabelSize = new System.Windows.Forms.TextBox();
            this.txtMinorGridLabelDistance = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.shapeMinorGridLineColor = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.shapeMajorGridLineColor = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.shapeBorderColor = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.shapeMajorGridLabelColor = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.shapeMinorGridLabelColor = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonGrid = new System.Windows.Forms.Button();
            this.toolbar = new ToolStripExtensions.ToolStripEx();
            this.tsButtonFitMap = new System.Windows.Forms.ToolStripButton();
            this.tsButtonSaveShapefile = new System.Windows.Forms.ToolStripButton();
            this.tsButtonSaveImage = new System.Windows.Forms.ToolStripButton();
            this.tsButtonMBRs = new System.Windows.Forms.ToolStripButton();
            this.tsButtonRetrieve = new System.Windows.Forms.ToolStripButton();
            this.tsButtonExit = new System.Windows.Forms.ToolStripButton();
            this.lblGridStatus = new System.Windows.Forms.Label();
            this.buttonLocateGrid = new System.Windows.Forms.Button();
            this.groupLabels.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMapTitle
            // 
            this.txtMapTitle.Location = new System.Drawing.Point(4, 62);
            this.txtMapTitle.Margin = new System.Windows.Forms.Padding(4);
            this.txtMapTitle.Name = "txtMapTitle";
            this.txtMapTitle.Size = new System.Drawing.Size(245, 21);
            this.txtMapTitle.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Map title";
            // 
            // groupLabels
            // 
            this.groupLabels.Controls.Add(this.btnLabelsFromKML);
            this.groupLabels.Controls.Add(this.buttonLabel);
            this.groupLabels.Controls.Add(this.label12);
            this.groupLabels.Controls.Add(this.label11);
            this.groupLabels.Controls.Add(this.label10);
            this.groupLabels.Controls.Add(this.label9);
            this.groupLabels.Controls.Add(this.label8);
            this.groupLabels.Controls.Add(this.chkBold);
            this.groupLabels.Controls.Add(this.chkWrapLabels);
            this.groupLabels.Controls.Add(this.chkBottom);
            this.groupLabels.Controls.Add(this.chkRight);
            this.groupLabels.Controls.Add(this.chkTop);
            this.groupLabels.Controls.Add(this.chkLeft);
            this.groupLabels.Controls.Add(this.txtMinorGridThickness);
            this.groupLabels.Controls.Add(this.txtMajorGridThickness);
            this.groupLabels.Controls.Add(this.txtBorderThickness);
            this.groupLabels.Controls.Add(this.txtMajorGridLabelSize);
            this.groupLabels.Controls.Add(this.txtMinorGridLabelSize);
            this.groupLabels.Controls.Add(this.txtMinorGridLabelDistance);
            this.groupLabels.Controls.Add(this.label7);
            this.groupLabels.Controls.Add(this.label6);
            this.groupLabels.Controls.Add(this.label5);
            this.groupLabels.Controls.Add(this.label4);
            this.groupLabels.Controls.Add(this.label3);
            this.groupLabels.Controls.Add(this.label2);
            this.groupLabels.Controls.Add(this.shapeContainer1);
            this.groupLabels.Location = new System.Drawing.Point(4, 89);
            this.groupLabels.Name = "groupLabels";
            this.groupLabels.Size = new System.Drawing.Size(245, 352);
            this.groupLabels.TabIndex = 4;
            this.groupLabels.TabStop = false;
            this.groupLabels.Text = "Labels";
            // 
            // btnLabelsFromKML
            // 
            this.btnLabelsFromKML.ImageKey = "label_add";
            this.btnLabelsFromKML.ImageList = this.imList;
            this.btnLabelsFromKML.Location = new System.Drawing.Point(172, 323);
            this.btnLabelsFromKML.Name = "btnLabelsFromKML";
            this.btnLabelsFromKML.Size = new System.Drawing.Size(23, 23);
            this.btnLabelsFromKML.TabIndex = 27;
            this.btnLabelsFromKML.UseVisualStyleBackColor = true;
            // 
            // imList
            // 
            this.imList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imList.ImageStream")));
            this.imList.TransparentColor = System.Drawing.Color.White;
            this.imList.Images.SetKeyName(0, "gridCursor");
            this.imList.Images.SetKeyName(1, "clearSelection");
            this.imList.Images.SetKeyName(2, "label");
            this.imList.Images.SetKeyName(3, "label_add");
            this.imList.Images.SetKeyName(4, "Ruler_16x.png");
            this.imList.Images.SetKeyName(5, "gridLayout");
            // 
            // buttonLabel
            // 
            this.buttonLabel.ImageKey = "label";
            this.buttonLabel.ImageList = this.imList;
            this.buttonLabel.Location = new System.Drawing.Point(201, 323);
            this.buttonLabel.Name = "buttonLabel";
            this.buttonLabel.Size = new System.Drawing.Size(23, 23);
            this.buttonLabel.TabIndex = 26;
            this.buttonLabel.UseVisualStyleBackColor = true;
            this.buttonLabel.Click += new System.EventHandler(this.OnButtons_Click);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(146, 285);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 37);
            this.label12.TabIndex = 24;
            this.label12.Text = "Line color";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(145, 244);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 37);
            this.label11.TabIndex = 23;
            this.label11.Text = "Line color";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(145, 202);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 37);
            this.label10.TabIndex = 22;
            this.label10.Text = "Border color";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(145, 161);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 37);
            this.label9.TabIndex = 21;
            this.label9.Text = "Label color";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(145, 120);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 37);
            this.label8.TabIndex = 20;
            this.label8.Text = "Label color";
            // 
            // chkBold
            // 
            this.chkBold.AutoSize = true;
            this.chkBold.Location = new System.Drawing.Point(172, 20);
            this.chkBold.Name = "chkBold";
            this.chkBold.Size = new System.Drawing.Size(51, 19);
            this.chkBold.TabIndex = 19;
            this.chkBold.Text = "Bold";
            this.chkBold.UseVisualStyleBackColor = true;
            // 
            // chkWrapLabels
            // 
            this.chkWrapLabels.AutoSize = true;
            this.chkWrapLabels.Location = new System.Drawing.Point(10, 20);
            this.chkWrapLabels.Name = "chkWrapLabels";
            this.chkWrapLabels.Size = new System.Drawing.Size(91, 19);
            this.chkWrapLabels.TabIndex = 18;
            this.chkWrapLabels.Text = "Wrap labels";
            this.chkWrapLabels.UseVisualStyleBackColor = true;
            // 
            // chkBottom
            // 
            this.chkBottom.AutoSize = true;
            this.chkBottom.Location = new System.Drawing.Point(172, 45);
            this.chkBottom.Name = "chkBottom";
            this.chkBottom.Size = new System.Drawing.Size(65, 19);
            this.chkBottom.TabIndex = 17;
            this.chkBottom.Text = "Bottom";
            this.chkBottom.UseVisualStyleBackColor = true;
            // 
            // chkRight
            // 
            this.chkRight.AutoSize = true;
            this.chkRight.Location = new System.Drawing.Point(113, 45);
            this.chkRight.Name = "chkRight";
            this.chkRight.Size = new System.Drawing.Size(55, 19);
            this.chkRight.TabIndex = 16;
            this.chkRight.Text = "Right";
            this.chkRight.UseVisualStyleBackColor = true;
            // 
            // chkTop
            // 
            this.chkTop.AutoSize = true;
            this.chkTop.Location = new System.Drawing.Point(60, 45);
            this.chkTop.Name = "chkTop";
            this.chkTop.Size = new System.Drawing.Size(47, 19);
            this.chkTop.TabIndex = 15;
            this.chkTop.Text = "Top";
            this.chkTop.UseVisualStyleBackColor = true;
            // 
            // chkLeft
            // 
            this.chkLeft.AutoSize = true;
            this.chkLeft.Location = new System.Drawing.Point(10, 45);
            this.chkLeft.Name = "chkLeft";
            this.chkLeft.Size = new System.Drawing.Size(46, 19);
            this.chkLeft.TabIndex = 14;
            this.chkLeft.Text = "Left";
            this.chkLeft.UseVisualStyleBackColor = true;
            // 
            // txtMinorGridThickness
            // 
            this.txtMinorGridThickness.Location = new System.Drawing.Point(73, 289);
            this.txtMinorGridThickness.Margin = new System.Windows.Forms.Padding(4);
            this.txtMinorGridThickness.Name = "txtMinorGridThickness";
            this.txtMinorGridThickness.Size = new System.Drawing.Size(50, 21);
            this.txtMinorGridThickness.TabIndex = 13;
            // 
            // txtMajorGridThickness
            // 
            this.txtMajorGridThickness.Location = new System.Drawing.Point(72, 248);
            this.txtMajorGridThickness.Margin = new System.Windows.Forms.Padding(4);
            this.txtMajorGridThickness.Name = "txtMajorGridThickness";
            this.txtMajorGridThickness.Size = new System.Drawing.Size(50, 21);
            this.txtMajorGridThickness.TabIndex = 12;
            // 
            // txtBorderThickness
            // 
            this.txtBorderThickness.Location = new System.Drawing.Point(73, 206);
            this.txtBorderThickness.Margin = new System.Windows.Forms.Padding(4);
            this.txtBorderThickness.Name = "txtBorderThickness";
            this.txtBorderThickness.Size = new System.Drawing.Size(50, 21);
            this.txtBorderThickness.TabIndex = 11;
            // 
            // txtMajorGridLabelSize
            // 
            this.txtMajorGridLabelSize.Location = new System.Drawing.Point(72, 165);
            this.txtMajorGridLabelSize.Margin = new System.Windows.Forms.Padding(4);
            this.txtMajorGridLabelSize.Name = "txtMajorGridLabelSize";
            this.txtMajorGridLabelSize.Size = new System.Drawing.Size(50, 21);
            this.txtMajorGridLabelSize.TabIndex = 10;
            // 
            // txtMinorGridLabelSize
            // 
            this.txtMinorGridLabelSize.Location = new System.Drawing.Point(72, 124);
            this.txtMinorGridLabelSize.Margin = new System.Windows.Forms.Padding(4);
            this.txtMinorGridLabelSize.Name = "txtMinorGridLabelSize";
            this.txtMinorGridLabelSize.Size = new System.Drawing.Size(50, 21);
            this.txtMinorGridLabelSize.TabIndex = 9;
            // 
            // txtMinorGridLabelDistance
            // 
            this.txtMinorGridLabelDistance.Location = new System.Drawing.Point(73, 82);
            this.txtMinorGridLabelDistance.Margin = new System.Windows.Forms.Padding(4);
            this.txtMinorGridLabelDistance.Name = "txtMinorGridLabelDistance";
            this.txtMinorGridLabelDistance.Size = new System.Drawing.Size(50, 21);
            this.txtMinorGridLabelDistance.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(7, 284);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 30);
            this.label7.TabIndex = 7;
            this.label7.Text = "Minor grid thickness";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(7, 243);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 30);
            this.label6.TabIndex = 6;
            this.label6.Text = "Major grid thickness";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 30);
            this.label5.TabIndex = 5;
            this.label5.Text = "Border thickness";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(7, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 30);
            this.label4.TabIndex = 4;
            this.label4.Text = "Major grid label size";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 30);
            this.label3.TabIndex = 3;
            this.label3.Text = "Minor grid label size";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 37);
            this.label2.TabIndex = 2;
            this.label2.Text = "Label distance";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(3, 17);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.shapeMinorGridLineColor,
            this.shapeMajorGridLineColor,
            this.shapeBorderColor,
            this.shapeMajorGridLabelColor,
            this.shapeMinorGridLabelColor});
            this.shapeContainer1.Size = new System.Drawing.Size(239, 332);
            this.shapeContainer1.TabIndex = 25;
            this.shapeContainer1.TabStop = false;
            // 
            // shapeMinorGridLineColor
            // 
            this.shapeMinorGridLineColor.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.shapeMinorGridLineColor.Location = new System.Drawing.Point(197, 277);
            this.shapeMinorGridLineColor.Name = "shapeMinorGridLineColor";
            this.shapeMinorGridLineColor.Size = new System.Drawing.Size(23, 18);
            this.shapeMinorGridLineColor.DoubleClick += new System.EventHandler(this.OnShapeColor_DoubleClick);
            // 
            // shapeMajorGridLineColor
            // 
            this.shapeMajorGridLineColor.FillColor = System.Drawing.Color.Red;
            this.shapeMajorGridLineColor.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.shapeMajorGridLineColor.Location = new System.Drawing.Point(197, 236);
            this.shapeMajorGridLineColor.Name = "shapeMajorGridLineColor";
            this.shapeMajorGridLineColor.Size = new System.Drawing.Size(23, 18);
            this.shapeMajorGridLineColor.DoubleClick += new System.EventHandler(this.OnShapeColor_DoubleClick);
            // 
            // shapeBorderColor
            // 
            this.shapeBorderColor.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.shapeBorderColor.Location = new System.Drawing.Point(197, 193);
            this.shapeBorderColor.Name = "shapeBorderColor";
            this.shapeBorderColor.Size = new System.Drawing.Size(23, 18);
            this.shapeBorderColor.DoubleClick += new System.EventHandler(this.OnShapeColor_DoubleClick);
            // 
            // shapeMajorGridLabelColor
            // 
            this.shapeMajorGridLabelColor.FillColor = System.Drawing.Color.Red;
            this.shapeMajorGridLabelColor.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.shapeMajorGridLabelColor.Location = new System.Drawing.Point(197, 149);
            this.shapeMajorGridLabelColor.Name = "shapeMajorGridLabelColor";
            this.shapeMajorGridLabelColor.Size = new System.Drawing.Size(23, 18);
            this.shapeMajorGridLabelColor.DoubleClick += new System.EventHandler(this.OnShapeColor_DoubleClick);
            // 
            // shapeMinorGridLabelColor
            // 
            this.shapeMinorGridLabelColor.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.shapeMinorGridLabelColor.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.shapeMinorGridLabelColor.Location = new System.Drawing.Point(197, 108);
            this.shapeMinorGridLabelColor.Name = "shapeMinorGridLabelColor";
            this.shapeMinorGridLabelColor.Size = new System.Drawing.Size(23, 18);
            this.shapeMinorGridLabelColor.DoubleClick += new System.EventHandler(this.OnShapeColor_DoubleClick);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(225, 480);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(48, 28);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.OnButtons_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.ImageIndex = 1;
            this.buttonClear.ImageList = this.imList;
            this.buttonClear.Location = new System.Drawing.Point(257, 130);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(23, 23);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.OnButtons_Click);
            // 
            // buttonGrid
            // 
            this.buttonGrid.ImageIndex = 0;
            this.buttonGrid.ImageList = this.imList;
            this.buttonGrid.Location = new System.Drawing.Point(256, 102);
            this.buttonGrid.Name = "buttonGrid";
            this.buttonGrid.Size = new System.Drawing.Size(23, 23);
            this.buttonGrid.TabIndex = 2;
            this.buttonGrid.UseVisualStyleBackColor = true;
            this.buttonGrid.Click += new System.EventHandler(this.OnButtons_Click);
            // 
            // toolbar
            // 
            this.toolbar.ClickThrough = true;
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButtonFitMap,
            this.tsButtonSaveShapefile,
            this.tsButtonSaveImage,
            this.tsButtonMBRs,
            this.tsButtonRetrieve,
            this.tsButtonExit});
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(285, 25);
            this.toolbar.SuppressHighlighting = true;
            this.toolbar.TabIndex = 7;
            this.toolbar.Text = "toolStripEx1";
            this.toolbar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolbar_ItemClicked);
            // 
            // tsButtonFitMap
            // 
            this.tsButtonFitMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonFitMap.Image = global::FAD3.Properties.Resources.fit;
            this.tsButtonFitMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButtonFitMap.Name = "tsButtonFitMap";
            this.tsButtonFitMap.Size = new System.Drawing.Size(23, 22);
            this.tsButtonFitMap.Text = "toolStripButton1";
            this.tsButtonFitMap.ToolTipText = "Fit grid to map";
            // 
            // tsButtonSaveShapefile
            // 
            this.tsButtonSaveShapefile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonSaveShapefile.Image = global::FAD3.Properties.Resources.document_save;
            this.tsButtonSaveShapefile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButtonSaveShapefile.Name = "tsButtonSaveShapefile";
            this.tsButtonSaveShapefile.Size = new System.Drawing.Size(23, 22);
            this.tsButtonSaveShapefile.Text = "toolStripButton1";
            this.tsButtonSaveShapefile.ToolTipText = "Save grid as shapefile";
            // 
            // tsButtonSaveImage
            // 
            this.tsButtonSaveImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonSaveImage.Image = global::FAD3.Properties.Resources.image;
            this.tsButtonSaveImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButtonSaveImage.Name = "tsButtonSaveImage";
            this.tsButtonSaveImage.Size = new System.Drawing.Size(23, 22);
            this.tsButtonSaveImage.Text = "toolStripButton1";
            this.tsButtonSaveImage.ToolTipText = "Save grid as image";
            // 
            // tsButtonMBRs
            // 
            this.tsButtonMBRs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonMBRs.Image = global::FAD3.Properties.Resources.mbr;
            this.tsButtonMBRs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButtonMBRs.Name = "tsButtonMBRs";
            this.tsButtonMBRs.Size = new System.Drawing.Size(23, 22);
            this.tsButtonMBRs.Text = "toolStripButton1";
            this.tsButtonMBRs.ToolTipText = "Show MBRs";
            // 
            // tsButtonRetrieve
            // 
            this.tsButtonRetrieve.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonRetrieve.Image = global::FAD3.Properties.Resources.im_boundary;
            this.tsButtonRetrieve.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButtonRetrieve.Name = "tsButtonRetrieve";
            this.tsButtonRetrieve.Size = new System.Drawing.Size(23, 22);
            this.tsButtonRetrieve.Text = "toolStripButton1";
            this.tsButtonRetrieve.ToolTipText = "Get grid boundaries";
            // 
            // tsButtonExit
            // 
            this.tsButtonExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonExit.Image = global::FAD3.Properties.Resources.im_exit;
            this.tsButtonExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsButtonExit.Name = "tsButtonExit";
            this.tsButtonExit.Size = new System.Drawing.Size(23, 22);
            this.tsButtonExit.Text = "toolStripButton1";
            this.tsButtonExit.ToolTipText = "Close";
            // 
            // lblGridStatus
            // 
            this.lblGridStatus.Location = new System.Drawing.Point(4, 455);
            this.lblGridStatus.Name = "lblGridStatus";
            this.lblGridStatus.Size = new System.Drawing.Size(215, 68);
            this.lblGridStatus.TabIndex = 8;
            this.lblGridStatus.Text = "Grid status:";
            // 
            // buttonLocateGrid
            // 
            this.buttonLocateGrid.ImageIndex = 5;
            this.buttonLocateGrid.ImageList = this.imList;
            this.buttonLocateGrid.Location = new System.Drawing.Point(257, 159);
            this.buttonLocateGrid.Name = "buttonLocateGrid";
            this.buttonLocateGrid.Size = new System.Drawing.Size(23, 23);
            this.buttonLocateGrid.TabIndex = 9;
            this.buttonLocateGrid.UseVisualStyleBackColor = true;
            this.buttonLocateGrid.Click += new System.EventHandler(this.OnButtons_Click);
            // 
            // Grid25GenerateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 532);
            this.Controls.Add(this.buttonLocateGrid);
            this.Controls.Add(this.lblGridStatus);
            this.Controls.Add(this.toolbar);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.groupLabels);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonGrid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMapTitle);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Grid25GenerateForm";
            this.ShowInTaskbar = false;
            this.Text = "Grid 25";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Grid25GenerateForm_FormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.groupLabels.ResumeLayout(false);
            this.groupLabels.PerformLayout();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMapTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonGrid;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.GroupBox groupLabels;
        private System.Windows.Forms.CheckBox chkBottom;
        private System.Windows.Forms.CheckBox chkRight;
        private System.Windows.Forms.CheckBox chkTop;
        private System.Windows.Forms.CheckBox chkLeft;
        private System.Windows.Forms.TextBox txtMinorGridThickness;
        private System.Windows.Forms.TextBox txtMajorGridThickness;
        private System.Windows.Forms.TextBox txtBorderThickness;
        private System.Windows.Forms.TextBox txtMajorGridLabelSize;
        private System.Windows.Forms.TextBox txtMinorGridLabelSize;
        private System.Windows.Forms.TextBox txtMinorGridLabelDistance;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkBold;
        private System.Windows.Forms.CheckBox chkWrapLabels;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape shapeMinorGridLabelColor;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape shapeMinorGridLineColor;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape shapeMajorGridLineColor;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape shapeBorderColor;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape shapeMajorGridLabelColor;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ImageList imList;
        private System.Windows.Forms.Button buttonLabel;
        private ToolStripExtensions.ToolStripEx toolbar;
        private System.Windows.Forms.ToolStripButton tsButtonFitMap;
        private System.Windows.Forms.ToolStripButton tsButtonSaveShapefile;
        private System.Windows.Forms.ToolStripButton tsButtonSaveImage;
        private System.Windows.Forms.ToolStripButton tsButtonMBRs;
        private System.Windows.Forms.ToolStripButton tsButtonRetrieve;
        private System.Windows.Forms.ToolStripButton tsButtonExit;
        private System.Windows.Forms.Button btnLabelsFromKML;
        private System.Windows.Forms.Label lblGridStatus;
        private System.Windows.Forms.Button buttonLocateGrid;
    }
}