/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/19/2016
 * Time: 1:45 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace FAD3
{
	partial class frmCatch
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblInFishbase = new System.Windows.Forms.Label();
			this.lblFB = new System.Windows.Forms.Label();
			this.lblCatchTaxa = new System.Windows.Forms.Label();
			this.lblTaxa = new System.Windows.Forms.LinkLabel();
			this.comboLocal = new System.Windows.Forms.ComboBox();
			this.checkLiveFish = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.comboName2 = new System.Windows.Forms.ComboBox();
			this.comboName1 = new System.Windows.Forms.ComboBox();
			this.radioLocalName = new System.Windows.Forms.RadioButton();
			this.radioSciName = new System.Windows.Forms.RadioButton();
			this.txtWt = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtCt = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtSubWt = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtSubCt = new System.Windows.Forms.TextBox();
			this.checkFromTotal = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblCompCt = new System.Windows.Forms.Label();
			this.lblCompWt = new System.Windows.Forms.Label();
			this.lblWtSample = new System.Windows.Forms.Label();
			this.lblWtCatch = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.chkSubsampled = new System.Windows.Forms.CheckBox();
			this.buttonNew = new System.Windows.Forms.Button();
			this.lblTitle = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(558, 34);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(63, 24);
			this.button1.TabIndex = 0;
			this.button1.Text = "Ok";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(558, 67);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(63, 24);
			this.button2.TabIndex = 1;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblInFishbase);
			this.groupBox1.Controls.Add(this.lblFB);
			this.groupBox1.Controls.Add(this.lblCatchTaxa);
			this.groupBox1.Controls.Add(this.lblTaxa);
			this.groupBox1.Controls.Add(this.comboLocal);
			this.groupBox1.Controls.Add(this.checkLiveFish);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.comboName2);
			this.groupBox1.Controls.Add(this.comboName1);
			this.groupBox1.Controls.Add(this.radioLocalName);
			this.groupBox1.Controls.Add(this.radioSciName);
			this.groupBox1.Location = new System.Drawing.Point(12, 34);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(268, 185);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Catch description";
			// 
			// lblInFishbase
			// 
			this.lblInFishbase.AutoSize = true;
			this.lblInFishbase.Location = new System.Drawing.Point(121, 149);
			this.lblInFishbase.Name = "lblInFishbase";
			this.lblInFishbase.Size = new System.Drawing.Size(25, 13);
			this.lblInFishbase.TabIndex = 21;
			this.lblInFishbase.Text = "Yes";
			// 
			// lblFB
			// 
			this.lblFB.AutoSize = true;
			this.lblFB.Location = new System.Drawing.Point(9, 149);
			this.lblFB.Name = "lblFB";
			this.lblFB.Size = new System.Drawing.Size(92, 13);
			this.lblFB.TabIndex = 19;
			this.lblFB.Text = "Listed in FishBase";
			// 
			// lblCatchTaxa
			// 
			this.lblCatchTaxa.AutoSize = true;
			this.lblCatchTaxa.Location = new System.Drawing.Point(121, 123);
			this.lblCatchTaxa.Name = "lblCatchTaxa";
			this.lblCatchTaxa.Size = new System.Drawing.Size(29, 13);
			this.lblCatchTaxa.TabIndex = 18;
			this.lblCatchTaxa.Text = "label";
			// 
			// lblTaxa
			// 
			this.lblTaxa.AutoSize = true;
			this.lblTaxa.Location = new System.Drawing.Point(8, 123);
			this.lblTaxa.Name = "lblTaxa";
			this.lblTaxa.Size = new System.Drawing.Size(103, 13);
			this.lblTaxa.TabIndex = 17;
			this.lblTaxa.TabStop = true;
			this.lblTaxa.Text = "Taxonomic category";
			// 
			// comboLocal
			// 
			this.comboLocal.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.comboLocal.FormattingEnabled = true;
			this.comboLocal.Location = new System.Drawing.Point(124, 6);
			this.comboLocal.Name = "comboLocal";
			this.comboLocal.Size = new System.Drawing.Size(129, 21);
			this.comboLocal.TabIndex = 16;
			this.comboLocal.Validating += new System.ComponentModel.CancelEventHandler(this.comboLocal_Validating);
			// 
			// checkLiveFish
			// 
			this.checkLiveFish.AutoSize = true;
			this.checkLiveFish.Location = new System.Drawing.Point(124, 99);
			this.checkLiveFish.Name = "checkLiveFish";
			this.checkLiveFish.Size = new System.Drawing.Size(15, 14);
			this.checkLiveFish.TabIndex = 15;
			this.checkLiveFish.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(9, 99);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(57, 13);
			this.label6.TabIndex = 14;
			this.label6.Text = "Live catch";
			// 
			// comboName2
			// 
			this.comboName2.FormattingEnabled = true;
			this.comboName2.Location = new System.Drawing.Point(125, 65);
			this.comboName2.Name = "comboName2";
			this.comboName2.Size = new System.Drawing.Size(129, 21);
			this.comboName2.TabIndex = 3;
			this.comboName2.Validating += new System.ComponentModel.CancelEventHandler(this.comboname2_validating);
			// 
			// comboName1
			// 
			this.comboName1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.comboName1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.comboName1.FormattingEnabled = true;
			this.comboName1.Location = new System.Drawing.Point(124, 36);
			this.comboName1.Name = "comboName1";
			this.comboName1.Size = new System.Drawing.Size(129, 21);
			this.comboName1.TabIndex = 2;
			this.comboName1.Validating += new System.ComponentModel.CancelEventHandler(this.comboname1_validating);
			// 
			// radioLocalName
			// 
			this.radioLocalName.AutoSize = true;
			this.radioLocalName.Location = new System.Drawing.Point(12, 69);
			this.radioLocalName.Name = "radioLocalName";
			this.radioLocalName.Size = new System.Drawing.Size(80, 17);
			this.radioLocalName.TabIndex = 1;
			this.radioLocalName.TabStop = true;
			this.radioLocalName.Text = "Local name";
			this.radioLocalName.UseVisualStyleBackColor = true;
			this.radioLocalName.CheckedChanged += new System.EventHandler(this.radioSciName_CheckedChanged);
			// 
			// radioSciName
			// 
			this.radioSciName.AutoSize = true;
			this.radioSciName.Location = new System.Drawing.Point(12, 40);
			this.radioSciName.Name = "radioSciName";
			this.radioSciName.Size = new System.Drawing.Size(97, 17);
			this.radioSciName.TabIndex = 0;
			this.radioSciName.TabStop = true;
			this.radioSciName.Text = "Scientific name";
			this.radioSciName.UseVisualStyleBackColor = true;
			this.radioSciName.CheckedChanged += new System.EventHandler(this.radioSciName_CheckedChanged);
			// 
			// txtWt
			// 
			this.txtWt.Location = new System.Drawing.Point(387, 67);
			this.txtWt.Name = "txtWt";
			this.txtWt.Size = new System.Drawing.Size(62, 20);
			this.txtWt.TabIndex = 3;
			this.txtWt.Tag = "wt";
			this.txtWt.Validating += new System.ComponentModel.CancelEventHandler(this.txtWt_Validating);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(295, 70);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Weight";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(295, 101);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Count";
			// 
			// txtCt
			// 
			this.txtCt.Location = new System.Drawing.Point(387, 99);
			this.txtCt.Name = "txtCt";
			this.txtCt.Size = new System.Drawing.Size(62, 20);
			this.txtCt.TabIndex = 5;
			this.txtCt.Tag = "ct";
			this.txtCt.Validating += new System.ComponentModel.CancelEventHandler(this.txtWt_Validating);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(295, 171);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(73, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Subsample wt";
			// 
			// txtSubWt
			// 
			this.txtSubWt.Location = new System.Drawing.Point(387, 168);
			this.txtSubWt.Name = "txtSubWt";
			this.txtSubWt.Size = new System.Drawing.Size(62, 20);
			this.txtSubWt.TabIndex = 7;
			this.txtSubWt.Tag = "wt";
			this.txtSubWt.Validating += new System.ComponentModel.CancelEventHandler(this.txtWt_Validating);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(295, 202);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 13);
			this.label4.TabIndex = 10;
			this.label4.Text = "Subsample ct";
			// 
			// txtSubCt
			// 
			this.txtSubCt.Location = new System.Drawing.Point(387, 199);
			this.txtSubCt.Name = "txtSubCt";
			this.txtSubCt.Size = new System.Drawing.Size(62, 20);
			this.txtSubCt.TabIndex = 9;
			this.txtSubCt.Tag = "ct";
			this.txtSubCt.Validating += new System.ComponentModel.CancelEventHandler(this.txtWt_Validating);
			// 
			// checkFromTotal
			// 
			this.checkFromTotal.AutoSize = true;
			this.checkFromTotal.Location = new System.Drawing.Point(388, 39);
			this.checkFromTotal.Name = "checkFromTotal";
			this.checkFromTotal.Size = new System.Drawing.Size(15, 14);
			this.checkFromTotal.TabIndex = 11;
			this.checkFromTotal.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(295, 40);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(83, 13);
			this.label5.TabIndex = 12;
			this.label5.Text = "From total catch";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lblCompCt);
			this.groupBox2.Controls.Add(this.lblCompWt);
			this.groupBox2.Controls.Add(this.lblWtSample);
			this.groupBox2.Controls.Add(this.lblWtCatch);
			this.groupBox2.Location = new System.Drawing.Point(13, 225);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(436, 80);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Computed values";
			// 
			// lblCompCt
			// 
			this.lblCompCt.AutoSize = true;
			this.lblCompCt.Location = new System.Drawing.Point(208, 56);
			this.lblCompCt.Name = "lblCompCt";
			this.lblCompCt.Size = new System.Drawing.Size(88, 13);
			this.lblCompCt.TabIndex = 3;
			this.lblCompCt.Text = "Computed count:";
			this.lblCompCt.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblCompWt
			// 
			this.lblCompWt.AutoSize = true;
			this.lblCompWt.Location = new System.Drawing.Point(208, 30);
			this.lblCompWt.Name = "lblCompWt";
			this.lblCompWt.Size = new System.Drawing.Size(92, 13);
			this.lblCompWt.TabIndex = 2;
			this.lblCompWt.Text = "Computed weight:";
			this.lblCompWt.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblWtSample
			// 
			this.lblWtSample.AutoSize = true;
			this.lblWtSample.Location = new System.Drawing.Point(22, 56);
			this.lblWtSample.Name = "lblWtSample";
			this.lblWtSample.Size = new System.Drawing.Size(92, 13);
			this.lblWtSample.TabIndex = 1;
			this.lblWtSample.Text = "Weight of sample:";
			this.lblWtSample.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblWtCatch
			// 
			this.lblWtCatch.AutoSize = true;
			this.lblWtCatch.Location = new System.Drawing.Point(28, 30);
			this.lblWtCatch.Name = "lblWtCatch";
			this.lblWtCatch.Size = new System.Drawing.Size(86, 13);
			this.lblWtCatch.TabIndex = 0;
			this.lblWtCatch.Text = "Weight of catch:";
			this.lblWtCatch.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(295, 141);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(65, 13);
			this.label7.TabIndex = 15;
			this.label7.Text = "Subsampled";
			// 
			// chkSubsampled
			// 
			this.chkSubsampled.AutoSize = true;
			this.chkSubsampled.Location = new System.Drawing.Point(388, 142);
			this.chkSubsampled.Name = "chkSubsampled";
			this.chkSubsampled.Size = new System.Drawing.Size(15, 14);
			this.chkSubsampled.TabIndex = 14;
			this.chkSubsampled.UseVisualStyleBackColor = true;
			this.chkSubsampled.CheckedChanged += new System.EventHandler(this.chkSubsampled_CheckedChanged);
			// 
			// buttonNew
			// 
			this.buttonNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonNew.Location = new System.Drawing.Point(558, 124);
			this.buttonNew.Name = "buttonNew";
			this.buttonNew.Size = new System.Drawing.Size(63, 24);
			this.buttonNew.TabIndex = 16;
			this.buttonNew.Text = "New";
			this.buttonNew.UseVisualStyleBackColor = true;
			this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.Location = new System.Drawing.Point(15, 9);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(105, 16);
			this.lblTitle.TabIndex = 17;
			this.lblTitle.Text = "Reference no:";
			// 
			// frmCatch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(633, 338);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.buttonNew);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.chkSubsampled);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.checkFromTotal);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtSubCt);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtSubWt);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtCt);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtWt);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "frmCatch";
			this.Text = "frmCatch";
			this.Load += new System.EventHandler(this.frmCatch_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboName2;
        private System.Windows.Forms.ComboBox comboName1;
        private System.Windows.Forms.RadioButton radioLocalName;
        private System.Windows.Forms.RadioButton radioSciName;
        private System.Windows.Forms.TextBox txtWt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSubWt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSubCt;
        private System.Windows.Forms.CheckBox checkFromTotal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkLiveFish;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkSubsampled;
        private System.Windows.Forms.Label lblWtSample;
        private System.Windows.Forms.Label lblWtCatch;
        private System.Windows.Forms.Label lblCompCt;
        private System.Windows.Forms.Label lblCompWt;
        private System.Windows.Forms.ComboBox comboLocal;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCatchTaxa;
        private System.Windows.Forms.LinkLabel lblTaxa;
        private System.Windows.Forms.Label lblFB;
        private System.Windows.Forms.Label lblInFishbase;
    }
}
