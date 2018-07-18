/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/12/2016
 * Time: 9:29 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace FAD3
{
	partial class frmEffort
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtVesH = new System.Windows.Forms.TextBox();
            this.txtVesW = new System.Windows.Forms.TextBox();
            this.txtVesL = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtNoFishers = new System.Windows.Forms.TextBox();
            this.txtNoHauls = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.txtCatchWt = new System.Windows.Forms.TextBox();
            this.txtSampleWt = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.txtEngineHP = new System.Windows.Forms.TextBox();
            this.radioNoBoat = new System.Windows.Forms.RadioButton();
            this.radioNonMotorized = new System.Windows.Forms.RadioButton();
            this.radioMotorized = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.checkHasLiveFish = new System.Windows.Forms.CheckBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.comboAOI = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboLS = new System.Windows.Forms.ComboBox();
            this.listFG = new System.Windows.Forms.ListBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboGearClass = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.comboGearVariations = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.txtRefNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboEnumerators = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.maskDateSampling = new System.Windows.Forms.MaskedTextBox();
            this.maskDateSet = new System.Windows.Forms.MaskedTextBox();
            this.maskDateHaul = new System.Windows.Forms.MaskedTextBox();
            this.maskTimeSampling = new System.Windows.Forms.MaskedTextBox();
            this.maskTimeSet = new System.Windows.Forms.MaskedTextBox();
            this.maskTimeHaul = new System.Windows.Forms.MaskedTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 12;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.Controls.Add(this.buttonGenerate, 6, 12);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 10, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtNoFishers, 10, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtNoHauls, 10, 2);
            this.tableLayoutPanel1.Controls.Add(this.label14, 8, 1);
            this.tableLayoutPanel1.Controls.Add(this.label15, 8, 2);
            this.tableLayoutPanel1.Controls.Add(this.label6, 4, 6);
            this.tableLayoutPanel1.Controls.Add(this.label4, 4, 5);
            this.tableLayoutPanel1.Controls.Add(this.label5, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.label23, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label24, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtCatchWt, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtSampleWt, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 10, 4);
            this.tableLayoutPanel1.Controls.Add(this.label16, 8, 4);
            this.tableLayoutPanel1.Controls.Add(this.label25, 8, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkHasLiveFish, 10, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtNotes, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboAOI, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.comboLS, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.listFG, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.label12, 4, 7);
            this.tableLayoutPanel1.Controls.Add(this.comboGearClass, 6, 7);
            this.tableLayoutPanel1.Controls.Add(this.label13, 4, 8);
            this.tableLayoutPanel1.Controls.Add(this.comboGearVariations, 6, 8);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 8, 11);
            this.tableLayoutPanel1.Controls.Add(this.label18, 8, 7);
            this.tableLayoutPanel1.Controls.Add(this.label26, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.txtRefNo, 6, 11);
            this.tableLayoutPanel1.Controls.Add(this.label2, 4, 11);
            this.tableLayoutPanel1.Controls.Add(this.comboEnumerators, 6, 10);
            this.tableLayoutPanel1.Controls.Add(this.label22, 4, 10);
            this.tableLayoutPanel1.Controls.Add(this.linkLabel1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.maskDateSampling, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.maskDateSet, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.maskDateHaul, 6, 5);
            this.tableLayoutPanel1.Controls.Add(this.maskTimeSampling, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.maskTimeSet, 6, 4);
            this.tableLayoutPanel1.Controls.Add(this.maskTimeHaul, 6, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 14;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(755, 413);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(358, 366);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(119, 22);
            this.buttonGenerate.TabIndex = 27;
            this.buttonGenerate.Text = "Generate Ref #";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.ButtonGenerateClick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 10);
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(599, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Catch and effort documentation";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.90566F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.09434F));
            this.tableLayoutPanel2.Controls.Add(this.txtVesH, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtVesW, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtVesL, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label21, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label19, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label20, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(608, 213);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel1.SetRowSpan(this.tableLayoutPanel2, 3);
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(124, 84);
            this.tableLayoutPanel2.TabIndex = 20;
            // 
            // txtVesH
            // 
            this.txtVesH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVesH.Location = new System.Drawing.Point(46, 59);
            this.txtVesH.Name = "txtVesH";
            this.txtVesH.Size = new System.Drawing.Size(75, 20);
            this.txtVesH.TabIndex = 23;
            // 
            // txtVesW
            // 
            this.txtVesW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVesW.Location = new System.Drawing.Point(46, 31);
            this.txtVesW.Name = "txtVesW";
            this.txtVesW.Size = new System.Drawing.Size(75, 20);
            this.txtVesW.TabIndex = 22;
            // 
            // txtVesL
            // 
            this.txtVesL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVesL.Location = new System.Drawing.Point(46, 3);
            this.txtVesL.Name = "txtVesL";
            this.txtVesL.Size = new System.Drawing.Size(75, 20);
            this.txtVesL.TabIndex = 21;
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.Location = new System.Drawing.Point(3, 61);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(37, 17);
            this.label21.TabIndex = 35;
            this.label21.Text = "H";
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label19.Location = new System.Drawing.Point(3, 5);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(37, 17);
            this.label19.TabIndex = 33;
            this.label19.Text = "L";
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.Location = new System.Drawing.Point(3, 32);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(37, 17);
            this.label20.TabIndex = 34;
            this.label20.Text = "W";
            // 
            // txtNoFishers
            // 
            this.txtNoFishers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNoFishers.Location = new System.Drawing.Point(608, 35);
            this.txtNoFishers.Name = "txtNoFishers";
            this.txtNoFishers.Size = new System.Drawing.Size(124, 20);
            this.txtNoFishers.TabIndex = 16;
            this.txtNoFishers.Tag = "Number of fishers";
            this.txtNoFishers.Validating += new System.ComponentModel.CancelEventHandler(this.txtCatchWt_Validating);
            // 
            // txtNoHauls
            // 
            this.txtNoHauls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNoHauls.Location = new System.Drawing.Point(608, 65);
            this.txtNoHauls.Name = "txtNoHauls";
            this.txtNoHauls.Size = new System.Drawing.Size(124, 20);
            this.txtNoHauls.TabIndex = 17;
            this.txtNoHauls.Tag = "Number of hauls";
            this.txtNoHauls.Validating += new System.ComponentModel.CancelEventHandler(this.txtCatchWt_Validating);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.Location = new System.Drawing.Point(503, 36);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(84, 17);
            this.label14.TabIndex = 26;
            this.label14.Text = "# of fishers";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.Location = new System.Drawing.Point(503, 66);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(84, 17);
            this.label15.TabIndex = 27;
            this.label15.Text = "# of hauls";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(253, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 17);
            this.label6.TabIndex = 15;
            this.label6.Text = "Time of haul";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(253, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Date of haul";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(253, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "Time of set";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(253, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Date of set";
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label23.Location = new System.Drawing.Point(253, 36);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(84, 17);
            this.label23.TabIndex = 39;
            this.label23.Text = "Catch weight";
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label24.Location = new System.Drawing.Point(253, 66);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(84, 17);
            this.label24.TabIndex = 40;
            this.label24.Text = "Sample weight";
            // 
            // txtCatchWt
            // 
            this.txtCatchWt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCatchWt.Location = new System.Drawing.Point(358, 35);
            this.txtCatchWt.Name = "txtCatchWt";
            this.txtCatchWt.Size = new System.Drawing.Size(124, 20);
            this.txtCatchWt.TabIndex = 6;
            this.txtCatchWt.Tag = "Weight of catch";
            this.txtCatchWt.Validating += new System.ComponentModel.CancelEventHandler(this.txtCatchWt_Validating);
            // 
            // txtSampleWt
            // 
            this.txtSampleWt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSampleWt.Location = new System.Drawing.Point(358, 65);
            this.txtSampleWt.Name = "txtSampleWt";
            this.txtSampleWt.Size = new System.Drawing.Size(124, 20);
            this.txtSampleWt.TabIndex = 7;
            this.txtSampleWt.Tag = "Weight of sample";
            this.txtSampleWt.Validating += new System.ComponentModel.CancelEventHandler(this.txtCatchWt_Validating);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.txtEngineHP);
            this.panel1.Controls.Add(this.radioNoBoat);
            this.panel1.Controls.Add(this.radioNonMotorized);
            this.panel1.Controls.Add(this.radioMotorized);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(608, 123);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel1.SetRowSpan(this.panel1, 3);
            this.panel1.Size = new System.Drawing.Size(124, 84);
            this.panel1.TabIndex = 19;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.Location = new System.Drawing.Point(39, 25);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(20, 19);
            this.label17.TabIndex = 46;
            this.label17.Text = "hp";
            // 
            // txtEngineHP
            // 
            this.txtEngineHP.Location = new System.Drawing.Point(60, 23);
            this.txtEngineHP.Name = "txtEngineHP";
            this.txtEngineHP.Size = new System.Drawing.Size(46, 20);
            this.txtEngineHP.TabIndex = 20;
            this.txtEngineHP.TabStop = false;
            this.txtEngineHP.Tag = "Engine hp";
            // 
            // radioNoBoat
            // 
            this.radioNoBoat.Location = new System.Drawing.Point(7, 65);
            this.radioNoBoat.Name = "radioNoBoat";
            this.radioNoBoat.Size = new System.Drawing.Size(98, 22);
            this.radioNoBoat.TabIndex = 2;
            this.radioNoBoat.TabStop = true;
            this.radioNoBoat.Text = "No boat used";
            this.radioNoBoat.UseVisualStyleBackColor = true;
            // 
            // radioNonMotorized
            // 
            this.radioNonMotorized.Location = new System.Drawing.Point(7, 44);
            this.radioNonMotorized.Name = "radioNonMotorized";
            this.radioNonMotorized.Size = new System.Drawing.Size(98, 22);
            this.radioNonMotorized.TabIndex = 1;
            this.radioNonMotorized.TabStop = true;
            this.radioNonMotorized.Text = "Non-motorized";
            this.radioNonMotorized.UseVisualStyleBackColor = true;
            // 
            // radioMotorized
            // 
            this.radioMotorized.Location = new System.Drawing.Point(7, 1);
            this.radioMotorized.Name = "radioMotorized";
            this.radioMotorized.Size = new System.Drawing.Size(98, 22);
            this.radioMotorized.TabIndex = 19;
            this.radioMotorized.TabStop = true;
            this.radioMotorized.Text = "Motorized";
            this.radioMotorized.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.Location = new System.Drawing.Point(503, 126);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(84, 17);
            this.label16.TabIndex = 44;
            this.label16.Text = "Vessel type";
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.Location = new System.Drawing.Point(503, 96);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(84, 17);
            this.label25.TabIndex = 45;
            this.label25.Text = "Live food fish";
            // 
            // checkHasLiveFish
            // 
            this.checkHasLiveFish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.checkHasLiveFish.Location = new System.Drawing.Point(608, 95);
            this.checkHasLiveFish.Name = "checkHasLiveFish";
            this.checkHasLiveFish.Size = new System.Drawing.Size(124, 19);
            this.checkHasLiveFish.TabIndex = 18;
            this.checkHasLiveFish.Tag = "Live";
            this.checkHasLiveFish.UseVisualStyleBackColor = true;
            // 
            // txtNotes
            // 
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Location = new System.Drawing.Point(108, 273);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.tableLayoutPanel1.SetRowSpan(this.txtNotes, 4);
            this.txtNotes.Size = new System.Drawing.Size(124, 117);
            this.txtNotes.TabIndex = 24;
            this.txtNotes.Tag = "Notes";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(3, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "Date sampled";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Location = new System.Drawing.Point(3, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 17);
            this.label8.TabIndex = 17;
            this.label8.Text = "Time sampled";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.Location = new System.Drawing.Point(3, 96);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 17);
            this.label9.TabIndex = 18;
            this.label9.Text = "AOI";
            // 
            // comboAOI
            // 
            this.comboAOI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboAOI.Enabled = false;
            this.comboAOI.FormattingEnabled = true;
            this.comboAOI.Location = new System.Drawing.Point(108, 94);
            this.comboAOI.Name = "comboAOI";
            this.comboAOI.Size = new System.Drawing.Size(124, 21);
            this.comboAOI.TabIndex = 3;
            this.comboAOI.Tag = "Area of interest";
            this.comboAOI.Validating += new System.ComponentModel.CancelEventHandler(this.comboGearClass_Validating);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Location = new System.Drawing.Point(3, 126);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 17);
            this.label10.TabIndex = 19;
            this.label10.Text = "Landing site";
            // 
            // comboLS
            // 
            this.comboLS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboLS.FormattingEnabled = true;
            this.comboLS.Location = new System.Drawing.Point(108, 124);
            this.comboLS.Name = "comboLS";
            this.comboLS.Size = new System.Drawing.Size(124, 21);
            this.comboLS.TabIndex = 4;
            this.comboLS.Tag = "Landing site";
            this.comboLS.SelectedIndexChanged += new System.EventHandler(this.comboLS_SelectedIndexChanged);
            this.comboLS.Validating += new System.ComponentModel.CancelEventHandler(this.comboGearClass_Validating);
            // 
            // listFG
            // 
            this.listFG.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listFG.FormattingEnabled = true;
            this.listFG.Location = new System.Drawing.Point(108, 153);
            this.listFG.Name = "listFG";
            this.tableLayoutPanel1.SetRowSpan(this.listFG, 4);
            this.listFG.Size = new System.Drawing.Size(124, 108);
            this.listFG.TabIndex = 5;
            this.listFG.Tag = "Fishing ground";
            this.listFG.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listFG.DoubleClick += new System.EventHandler(this.ListBox1_DoubleClick);
            this.listFG.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listFG_KeyUp);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.Location = new System.Drawing.Point(253, 216);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(84, 17);
            this.label12.TabIndex = 22;
            this.label12.Text = "Gear class";
            // 
            // comboGearClass
            // 
            this.comboGearClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboGearClass.FormattingEnabled = true;
            this.comboGearClass.Location = new System.Drawing.Point(358, 214);
            this.comboGearClass.Name = "comboGearClass";
            this.comboGearClass.Size = new System.Drawing.Size(124, 21);
            this.comboGearClass.TabIndex = 12;
            this.comboGearClass.Tag = "GearClass";
            this.comboGearClass.Validating += new System.ComponentModel.CancelEventHandler(this.comboGearClass_Validating);
            this.comboGearClass.Validated += new System.EventHandler(this.ComboGearClassValidated);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.Location = new System.Drawing.Point(253, 246);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(84, 17);
            this.label13.TabIndex = 23;
            this.label13.Text = "Gear variation";
            // 
            // comboGearVariations
            // 
            this.comboGearVariations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboGearVariations.FormattingEnabled = true;
            this.comboGearVariations.Location = new System.Drawing.Point(358, 244);
            this.comboGearVariations.Name = "comboGearVariations";
            this.comboGearVariations.Size = new System.Drawing.Size(124, 21);
            this.comboGearVariations.TabIndex = 13;
            this.comboGearVariations.Tag = "Gear used";
            this.comboGearVariations.Validating += new System.ComponentModel.CancelEventHandler(this.comboGearClass_Validating);
            // 
            // panel2
            // 
            this.panel2.CausesValidation = false;
            this.tableLayoutPanel1.SetColumnSpan(this.panel2, 3);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(503, 336);
            this.panel2.Name = "panel2";
            this.tableLayoutPanel1.SetRowSpan(this.panel2, 2);
            this.panel2.Size = new System.Drawing.Size(229, 54);
            this.panel2.TabIndex = 50;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.CausesValidation = false;
            this.button2.Location = new System.Drawing.Point(87, 29);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 24);
            this.button2.TabIndex = 26;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.CausesValidation = false;
            this.button1.Location = new System.Drawing.Point(157, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 24);
            this.button1.TabIndex = 25;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.Location = new System.Drawing.Point(503, 216);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(84, 17);
            this.label18.TabIndex = 32;
            this.label18.Text = "Vessel size";
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label26.Location = new System.Drawing.Point(3, 276);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(84, 17);
            this.label26.TabIndex = 47;
            this.label26.Text = "Notes";
            // 
            // txtRefNo
            // 
            this.txtRefNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRefNo.Location = new System.Drawing.Point(358, 338);
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.Size = new System.Drawing.Size(124, 20);
            this.txtRefNo.TabIndex = 15;
            this.txtRefNo.Tag = "Reference #";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(253, 339);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Reference #";
            // 
            // comboEnumerators
            // 
            this.comboEnumerators.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboEnumerators.FormattingEnabled = true;
            this.comboEnumerators.Location = new System.Drawing.Point(358, 306);
            this.comboEnumerators.Name = "comboEnumerators";
            this.comboEnumerators.Size = new System.Drawing.Size(124, 21);
            this.comboEnumerators.TabIndex = 14;
            this.comboEnumerators.Tag = "Enumerator";
            this.comboEnumerators.Validating += new System.ComponentModel.CancelEventHandler(this.comboGearClass_Validating);
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label22.Location = new System.Drawing.Point(253, 308);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(84, 17);
            this.label22.TabIndex = 37;
            this.label22.Text = "Enumerator";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(3, 158);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(84, 13);
            this.linkLabel1.TabIndex = 51;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Fishing grounds";
            this.linkLabel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.linkLabel1_MouseClick);
            // 
            // maskDateSampling
            // 
            this.maskDateSampling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.maskDateSampling.Location = new System.Drawing.Point(108, 35);
            this.maskDateSampling.Mask = ">L<LL-00-0000";
            this.maskDateSampling.Name = "maskDateSampling";
            this.maskDateSampling.Size = new System.Drawing.Size(124, 20);
            this.maskDateSampling.TabIndex = 1;
            this.maskDateSampling.Tag = "Date of sampling";
            this.maskDateSampling.Validating += new System.ComponentModel.CancelEventHandler(this.maskDateSampling_Validating);
            // 
            // maskDateSet
            // 
            this.maskDateSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.maskDateSet.Location = new System.Drawing.Point(358, 95);
            this.maskDateSet.Mask = ">L<LL-00-0000";
            this.maskDateSet.Name = "maskDateSet";
            this.maskDateSet.Size = new System.Drawing.Size(124, 20);
            this.maskDateSet.TabIndex = 8;
            this.maskDateSet.Tag = "Date gear set";
            this.maskDateSet.KeyUp += new System.Windows.Forms.KeyEventHandler(this.maskDateSet_KeyUp);
            this.maskDateSet.Validating += new System.ComponentModel.CancelEventHandler(this.maskDateSampling_Validating);
            // 
            // maskDateHaul
            // 
            this.maskDateHaul.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.maskDateHaul.Location = new System.Drawing.Point(358, 155);
            this.maskDateHaul.Mask = ">L<LL-00-0000";
            this.maskDateHaul.Name = "maskDateHaul";
            this.maskDateHaul.Size = new System.Drawing.Size(124, 20);
            this.maskDateHaul.TabIndex = 10;
            this.maskDateHaul.Tag = "Date gear hauled";
            this.maskDateHaul.KeyUp += new System.Windows.Forms.KeyEventHandler(this.maskDateSet_KeyUp);
            this.maskDateHaul.Validating += new System.ComponentModel.CancelEventHandler(this.maskDateSampling_Validating);
            // 
            // maskTimeSampling
            // 
            this.maskTimeSampling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.maskTimeSampling.Location = new System.Drawing.Point(108, 65);
            this.maskTimeSampling.Mask = "00:00";
            this.maskTimeSampling.Name = "maskTimeSampling";
            this.maskTimeSampling.Size = new System.Drawing.Size(124, 20);
            this.maskTimeSampling.TabIndex = 2;
            this.maskTimeSampling.Tag = "Time of sampling";
            this.maskTimeSampling.ValidatingType = typeof(System.DateTime);
            this.maskTimeSampling.Validating += new System.ComponentModel.CancelEventHandler(this.maskDateSampling_Validating);
            // 
            // maskTimeSet
            // 
            this.maskTimeSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.maskTimeSet.Location = new System.Drawing.Point(358, 125);
            this.maskTimeSet.Mask = "00:00";
            this.maskTimeSet.Name = "maskTimeSet";
            this.maskTimeSet.Size = new System.Drawing.Size(124, 20);
            this.maskTimeSet.TabIndex = 9;
            this.maskTimeSet.Tag = "Time gear set";
            this.maskTimeSet.ValidatingType = typeof(System.DateTime);
            this.maskTimeSet.Validating += new System.ComponentModel.CancelEventHandler(this.maskDateSampling_Validating);
            // 
            // maskTimeHaul
            // 
            this.maskTimeHaul.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.maskTimeHaul.Location = new System.Drawing.Point(358, 185);
            this.maskTimeHaul.Mask = "00:00";
            this.maskTimeHaul.Name = "maskTimeHaul";
            this.maskTimeHaul.Size = new System.Drawing.Size(124, 20);
            this.maskTimeHaul.TabIndex = 11;
            this.maskTimeHaul.Tag = "Time gear hauled";
            this.maskTimeHaul.ValidatingType = typeof(System.DateTime);
            this.maskTimeHaul.Validating += new System.ComponentModel.CancelEventHandler(this.maskDateSampling_Validating);
            // 
            // frmEffort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 413);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmEffort";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmEffortLoad);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.Button buttonGenerate;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TextBox txtNotes;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.CheckBox checkHasLiveFish;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.RadioButton radioMotorized;
		private System.Windows.Forms.RadioButton radioNonMotorized;
		private System.Windows.Forms.RadioButton radioNoBoat;
		private System.Windows.Forms.TextBox txtEngineHP;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox txtSampleWt;
		private System.Windows.Forms.TextBox txtCatchWt;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.ComboBox comboEnumerators;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.TextBox txtVesL;
		private System.Windows.Forms.TextBox txtVesW;
		private System.Windows.Forms.TextBox txtVesH;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.ComboBox comboGearVariations;
		private System.Windows.Forms.ComboBox comboGearClass;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox txtNoFishers;
		private System.Windows.Forms.TextBox txtNoHauls;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.ListBox listFG;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox comboLS;
		private System.Windows.Forms.ComboBox comboAOI;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtRefNo;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.MaskedTextBox maskDateSampling;
        private System.Windows.Forms.MaskedTextBox maskDateSet;
        private System.Windows.Forms.MaskedTextBox maskDateHaul;
        private System.Windows.Forms.MaskedTextBox maskTimeSampling;
        private System.Windows.Forms.MaskedTextBox maskTimeSet;
        private System.Windows.Forms.MaskedTextBox maskTimeHaul;
    }
}
