namespace FAD3
{
    partial class GraticuleForm
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
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnRemove = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkBold = new System.Windows.Forms.CheckBox();
            this.chkBottom = new System.Windows.Forms.CheckBox();
            this.chkRight = new System.Windows.Forms.CheckBox();
            this.chkTop = new System.Windows.Forms.CheckBox();
            this.chkLeft = new System.Windows.Forms.CheckBox();
            this.chkShowGrid = new System.Windows.Forms.CheckBox();
            this.txtGridlineWidth = new System.Windows.Forms.TextBox();
            this.txtBordeWidth = new System.Windows.Forms.TextBox();
            this.txtNumberOfGridlines = new System.Windows.Forms.TextBox();
            this.txtLabelSize = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkTitle = new System.Windows.Forms.CheckBox();
            this.chkNote = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lnkTitle = new System.Windows.Forms.LinkLabel();
            this.lnkNote = new System.Windows.Forms.LinkLabel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(302, 447);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(53, 28);
            this.btnOk.TabIndex = 17;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(251, 447);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(47, 28);
            this.btnApply.TabIndex = 18;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(181, 447);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 28);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(7, 22);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(352, 419);
            this.tabControl1.TabIndex = 21;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnRemove);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.txtName);
            this.tabPage1.Controls.Add(this.chkBold);
            this.tabPage1.Controls.Add(this.chkBottom);
            this.tabPage1.Controls.Add(this.chkRight);
            this.tabPage1.Controls.Add(this.chkTop);
            this.tabPage1.Controls.Add(this.chkLeft);
            this.tabPage1.Controls.Add(this.chkShowGrid);
            this.tabPage1.Controls.Add(this.txtGridlineWidth);
            this.tabPage1.Controls.Add(this.txtBordeWidth);
            this.tabPage1.Controls.Add(this.txtNumberOfGridlines);
            this.tabPage1.Controls.Add(this.txtLabelSize);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(344, 391);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Graticule";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(88, 301);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(166, 35);
            this.btnRemove.TabIndex = 33;
            this.btnRemove.Text = "Remove graticule";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 15);
            this.label5.TabIndex = 32;
            this.label5.Text = "Name of graticule";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(21, 29);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(292, 21);
            this.txtName.TabIndex = 31;
            // 
            // chkBold
            // 
            this.chkBold.AutoSize = true;
            this.chkBold.Location = new System.Drawing.Point(226, 224);
            this.chkBold.Name = "chkBold";
            this.chkBold.Size = new System.Drawing.Size(87, 19);
            this.chkBold.TabIndex = 30;
            this.chkBold.Text = "Bold labels";
            this.chkBold.UseVisualStyleBackColor = true;
            // 
            // chkBottom
            // 
            this.chkBottom.AutoSize = true;
            this.chkBottom.Location = new System.Drawing.Point(226, 251);
            this.chkBottom.Name = "chkBottom";
            this.chkBottom.Size = new System.Drawing.Size(65, 19);
            this.chkBottom.TabIndex = 29;
            this.chkBottom.Text = "Bottom";
            this.chkBottom.UseVisualStyleBackColor = true;
            // 
            // chkRight
            // 
            this.chkRight.AutoSize = true;
            this.chkRight.Location = new System.Drawing.Point(155, 251);
            this.chkRight.Name = "chkRight";
            this.chkRight.Size = new System.Drawing.Size(55, 19);
            this.chkRight.TabIndex = 28;
            this.chkRight.Text = "Right";
            this.chkRight.UseVisualStyleBackColor = true;
            // 
            // chkTop
            // 
            this.chkTop.AutoSize = true;
            this.chkTop.Location = new System.Drawing.Point(88, 251);
            this.chkTop.Name = "chkTop";
            this.chkTop.Size = new System.Drawing.Size(47, 19);
            this.chkTop.TabIndex = 27;
            this.chkTop.Text = "Top";
            this.chkTop.UseVisualStyleBackColor = true;
            // 
            // chkLeft
            // 
            this.chkLeft.AutoSize = true;
            this.chkLeft.Location = new System.Drawing.Point(21, 251);
            this.chkLeft.Name = "chkLeft";
            this.chkLeft.Size = new System.Drawing.Size(46, 19);
            this.chkLeft.TabIndex = 26;
            this.chkLeft.Text = "Left";
            this.chkLeft.UseVisualStyleBackColor = true;
            // 
            // chkShowGrid
            // 
            this.chkShowGrid.AutoSize = true;
            this.chkShowGrid.Location = new System.Drawing.Point(21, 224);
            this.chkShowGrid.Name = "chkShowGrid";
            this.chkShowGrid.Size = new System.Drawing.Size(81, 19);
            this.chkShowGrid.TabIndex = 25;
            this.chkShowGrid.Text = "Show grid";
            this.chkShowGrid.UseVisualStyleBackColor = true;
            // 
            // txtGridlineWidth
            // 
            this.txtGridlineWidth.Location = new System.Drawing.Point(166, 171);
            this.txtGridlineWidth.Name = "txtGridlineWidth";
            this.txtGridlineWidth.Size = new System.Drawing.Size(59, 21);
            this.txtGridlineWidth.TabIndex = 24;
            // 
            // txtBordeWidth
            // 
            this.txtBordeWidth.Location = new System.Drawing.Point(166, 137);
            this.txtBordeWidth.Name = "txtBordeWidth";
            this.txtBordeWidth.Size = new System.Drawing.Size(59, 21);
            this.txtBordeWidth.TabIndex = 23;
            // 
            // txtNumberOfGridlines
            // 
            this.txtNumberOfGridlines.Location = new System.Drawing.Point(166, 104);
            this.txtNumberOfGridlines.Name = "txtNumberOfGridlines";
            this.txtNumberOfGridlines.Size = new System.Drawing.Size(59, 21);
            this.txtNumberOfGridlines.TabIndex = 22;
            // 
            // txtLabelSize
            // 
            this.txtLabelSize.Location = new System.Drawing.Point(166, 71);
            this.txtLabelSize.Name = "txtLabelSize";
            this.txtLabelSize.Size = new System.Drawing.Size(59, 21);
            this.txtLabelSize.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 15);
            this.label4.TabIndex = 20;
            this.label4.Text = "Border width";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 19;
            this.label3.Text = "Gridline width";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 15);
            this.label2.TabIndex = 18;
            this.label2.Text = "Number of gridlines";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "Size of coordinate labels";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lnkNote);
            this.tabPage2.Controls.Add(this.lnkTitle);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.pictureBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(344, 391);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Labels";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkTitle);
            this.groupBox1.Controls.Add(this.chkNote);
            this.groupBox1.Location = new System.Drawing.Point(85, 328);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(164, 50);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Elements";
            // 
            // chkTitle
            // 
            this.chkTitle.AutoSize = true;
            this.chkTitle.Location = new System.Drawing.Point(26, 21);
            this.chkTitle.Name = "chkTitle";
            this.chkTitle.Size = new System.Drawing.Size(49, 19);
            this.chkTitle.TabIndex = 3;
            this.chkTitle.Text = "Title";
            this.chkTitle.UseVisualStyleBackColor = true;
            this.chkTitle.CheckedChanged += new System.EventHandler(this.OnCheckChange);
            // 
            // chkNote
            // 
            this.chkNote.AutoSize = true;
            this.chkNote.Location = new System.Drawing.Point(97, 21);
            this.chkNote.Name = "chkNote";
            this.chkNote.Size = new System.Drawing.Size(52, 19);
            this.chkNote.TabIndex = 4;
            this.chkNote.Text = "Note";
            this.chkNote.UseVisualStyleBackColor = true;
            this.chkNote.CheckedChanged += new System.EventHandler(this.OnCheckChange);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::FAD3.Properties.Resources.mapPreview;
            this.pictureBox1.Location = new System.Drawing.Point(46, 37);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 254);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // lnkTitle
            // 
            this.lnkTitle.AutoSize = true;
            this.lnkTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkTitle.Location = new System.Drawing.Point(95, 15);
            this.lnkTitle.Name = "lnkTitle";
            this.lnkTitle.Size = new System.Drawing.Size(150, 18);
            this.lnkTitle.TabIndex = 8;
            this.lnkTitle.TabStop = true;
            this.lnkTitle.Text = "Configure map title";
            this.lnkTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lnkTitle.Visible = false;
            this.lnkTitle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClick);
            // 
            // lnkNote
            // 
            this.lnkNote.AutoSize = true;
            this.lnkNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkNote.Location = new System.Drawing.Point(121, 294);
            this.lnkNote.Name = "lnkNote";
            this.lnkNote.Size = new System.Drawing.Size(99, 13);
            this.lnkNote.TabIndex = 9;
            this.lnkNote.TabStop = true;
            this.lnkNote.Text = "Configure map note";
            this.lnkNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkNote.Visible = false;
            this.lnkNote.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClick);
            // 
            // GraticuleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 483);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GraticuleForm";
            this.Text = "Graticule";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GraticuleForm_FormClosed);
            this.Load += new System.EventHandler(this.OnGraticuleForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.CheckBox chkBold;
        private System.Windows.Forms.CheckBox chkBottom;
        private System.Windows.Forms.CheckBox chkRight;
        private System.Windows.Forms.CheckBox chkTop;
        private System.Windows.Forms.CheckBox chkLeft;
        private System.Windows.Forms.CheckBox chkShowGrid;
        private System.Windows.Forms.TextBox txtGridlineWidth;
        private System.Windows.Forms.TextBox txtBordeWidth;
        private System.Windows.Forms.TextBox txtNumberOfGridlines;
        private System.Windows.Forms.TextBox txtLabelSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox chkNote;
        private System.Windows.Forms.CheckBox chkTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel lnkNote;
        private System.Windows.Forms.LinkLabel lnkTitle;
    }
}