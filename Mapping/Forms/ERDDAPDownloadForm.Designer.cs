namespace FAD3.Mapping.Forms
{
    partial class ERDDAPDownloadForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtboundNorth = new System.Windows.Forms.TextBox();
            this.txtboundSouth = new System.Windows.Forms.TextBox();
            this.txtboundWest = new System.Windows.Forms.TextBox();
            this.txtboundEast = new System.Windows.Forms.TextBox();
            this.txtLatStride = new System.Windows.Forms.TextBox();
            this.txtLonStride = new System.Windows.Forms.TextBox();
            this.lvGridParameters = new System.Windows.Forms.ListView();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTimeStride = new System.Windows.Forms.TextBox();
            this.dtPickerStart = new System.Windows.Forms.DateTimePicker();
            this.lblAbstract = new System.Windows.Forms.LinkLabel();
            this.lblConstraints = new System.Windows.Forms.LinkLabel();
            this.lblCredits = new System.Windows.Forms.LinkLabel();
            this.lblURL = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblLonSpacing = new System.Windows.Forms.Label();
            this.lblLonSize = new System.Windows.Forms.Label();
            this.lblLatSpacing = new System.Windows.Forms.Label();
            this.lblLatSize = new System.Windows.Forms.Label();
            this.lblTimeSpacing = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dtPickerEnd = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkTime = new System.Windows.Forms.CheckBox();
            this.chkLat = new System.Windows.Forms.CheckBox();
            this.chkLon = new System.Windows.Forms.CheckBox();
            this.lblTimeSize = new System.Windows.Forms.Label();
            this.btnTimeHelp = new System.Windows.Forms.Button();
            this.btnLatHelp = new System.Windows.Forms.Button();
            this.btnLonHelp = new System.Windows.Forms.Button();
            this.chkAltitude = new System.Windows.Forms.CheckBox();
            this.btnAltitudeHelp = new System.Windows.Forms.Button();
            this.txtStartAltitude = new System.Windows.Forms.TextBox();
            this.txtAltitudeStride = new System.Windows.Forms.TextBox();
            this.txtEndAltitude = new System.Windows.Forms.TextBox();
            this.lblAltitudeSize = new System.Windows.Forms.Label();
            this.lblAltitudeSpacing = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(661, 55);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title of data to download";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(618, 484);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(55, 28);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(557, 484);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 28);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtboundNorth
            // 
            this.txtboundNorth.Location = new System.Drawing.Point(122, 84);
            this.txtboundNorth.Name = "txtboundNorth";
            this.txtboundNorth.Size = new System.Drawing.Size(123, 20);
            this.txtboundNorth.TabIndex = 5;
            // 
            // txtboundSouth
            // 
            this.txtboundSouth.Location = new System.Drawing.Point(323, 84);
            this.txtboundSouth.Name = "txtboundSouth";
            this.txtboundSouth.Size = new System.Drawing.Size(123, 20);
            this.txtboundSouth.TabIndex = 7;
            // 
            // txtboundWest
            // 
            this.txtboundWest.Location = new System.Drawing.Point(323, 111);
            this.txtboundWest.Name = "txtboundWest";
            this.txtboundWest.Size = new System.Drawing.Size(123, 20);
            this.txtboundWest.TabIndex = 8;
            // 
            // txtboundEast
            // 
            this.txtboundEast.Location = new System.Drawing.Point(122, 111);
            this.txtboundEast.Name = "txtboundEast";
            this.txtboundEast.Size = new System.Drawing.Size(123, 20);
            this.txtboundEast.TabIndex = 9;
            // 
            // txtLatStride
            // 
            this.txtLatStride.Location = new System.Drawing.Point(256, 84);
            this.txtLatStride.Name = "txtLatStride";
            this.txtLatStride.Size = new System.Drawing.Size(58, 20);
            this.txtLatStride.TabIndex = 20;
            this.txtLatStride.Text = "1";
            // 
            // txtLonStride
            // 
            this.txtLonStride.Location = new System.Drawing.Point(256, 111);
            this.txtLonStride.Name = "txtLonStride";
            this.txtLonStride.Size = new System.Drawing.Size(58, 20);
            this.txtLonStride.TabIndex = 18;
            this.txtLonStride.Text = "1";
            // 
            // lvGridParameters
            // 
            this.lvGridParameters.Location = new System.Drawing.Point(12, 297);
            this.lvGridParameters.Name = "lvGridParameters";
            this.lvGridParameters.Size = new System.Drawing.Size(558, 170);
            this.lvGridParameters.TabIndex = 11;
            this.lvGridParameters.UseCompatibleStateImageBehavior = false;
            this.lvGridParameters.View = System.Windows.Forms.View.Details;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 281);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Grid variables";
            // 
            // txtTimeStride
            // 
            this.txtTimeStride.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTimeStride.Location = new System.Drawing.Point(256, 30);
            this.txtTimeStride.Name = "txtTimeStride";
            this.txtTimeStride.Size = new System.Drawing.Size(61, 20);
            this.txtTimeStride.TabIndex = 16;
            this.txtTimeStride.Text = "1";
            // 
            // dtPickerStart
            // 
            this.dtPickerStart.CustomFormat = "MMM- dd- yyyy ";
            this.dtPickerStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtPickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerStart.Location = new System.Drawing.Point(122, 30);
            this.dtPickerStart.Name = "dtPickerStart";
            this.dtPickerStart.Size = new System.Drawing.Size(128, 20);
            this.dtPickerStart.TabIndex = 0;
            // 
            // lblAbstract
            // 
            this.lblAbstract.AutoSize = true;
            this.lblAbstract.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbstract.Location = new System.Drawing.Point(274, 83);
            this.lblAbstract.Name = "lblAbstract";
            this.lblAbstract.Size = new System.Drawing.Size(60, 17);
            this.lblAbstract.TabIndex = 17;
            this.lblAbstract.TabStop = true;
            this.lblAbstract.Text = "Abstract";
            this.lblAbstract.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClick);
            // 
            // lblConstraints
            // 
            this.lblConstraints.AutoSize = true;
            this.lblConstraints.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConstraints.Location = new System.Drawing.Point(380, 83);
            this.lblConstraints.Name = "lblConstraints";
            this.lblConstraints.Size = new System.Drawing.Size(116, 17);
            this.lblConstraints.TabIndex = 18;
            this.lblConstraints.TabStop = true;
            this.lblConstraints.Text = "Legal constraints";
            this.lblConstraints.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClick);
            // 
            // lblCredits
            // 
            this.lblCredits.AutoSize = true;
            this.lblCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredits.Location = new System.Drawing.Point(182, 83);
            this.lblCredits.Name = "lblCredits";
            this.lblCredits.Size = new System.Drawing.Size(52, 17);
            this.lblCredits.TabIndex = 19;
            this.lblCredits.TabStop = true;
            this.lblCredits.Text = "Credits";
            this.lblCredits.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClick);
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblURL.Location = new System.Drawing.Point(15, 490);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(102, 17);
            this.lblURL.TabIndex = 20;
            this.lblURL.TabStop = true;
            this.lblURL.Text = "Download URL";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.02146F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.45923F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.72961F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.45922F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.66524F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.66524F));
            this.tableLayoutPanel1.Controls.Add(this.txtAltitudeStride, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtStartAltitude, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnAltitudeHelp, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnTimeHelp, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblLonSpacing, 6, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblLonSize, 5, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblLatSpacing, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblLatSize, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTimeSpacing, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnLonHelp, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label15, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label8, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtLatStride, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtLonStride, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtTimeStride, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.dtPickerEnd, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkTime, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dtPickerStart, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtboundWest, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtboundEast, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.chkLat, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkLon, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtboundNorth, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtboundSouth, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTimeSize, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkAltitude, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtEndAltitude, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblAltitudeSize, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblAltitudeSpacing, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnLatHelp, 1, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(18, 125);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(652, 135);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblLonSpacing
            // 
            this.lblLonSpacing.AutoSize = true;
            this.lblLonSpacing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLonSpacing.Location = new System.Drawing.Point(555, 108);
            this.lblLonSpacing.Name = "lblLonSpacing";
            this.lblLonSpacing.Size = new System.Drawing.Size(94, 27);
            this.lblLonSpacing.TabIndex = 39;
            this.lblLonSpacing.Text = "lon spacing";
            this.lblLonSpacing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLonSize
            // 
            this.lblLonSize.AutoSize = true;
            this.lblLonSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLonSize.Location = new System.Drawing.Point(457, 108);
            this.lblLonSize.Name = "lblLonSize";
            this.lblLonSize.Size = new System.Drawing.Size(92, 27);
            this.lblLonSize.TabIndex = 38;
            this.lblLonSize.Text = "lon size";
            this.lblLonSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLatSpacing
            // 
            this.lblLatSpacing.AutoSize = true;
            this.lblLatSpacing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLatSpacing.Location = new System.Drawing.Point(555, 81);
            this.lblLatSpacing.Name = "lblLatSpacing";
            this.lblLatSpacing.Size = new System.Drawing.Size(94, 27);
            this.lblLatSpacing.TabIndex = 37;
            this.lblLatSpacing.Text = "lat spacing";
            this.lblLatSpacing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLatSize
            // 
            this.lblLatSize.AutoSize = true;
            this.lblLatSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLatSize.Location = new System.Drawing.Point(457, 81);
            this.lblLatSize.Name = "lblLatSize";
            this.lblLatSize.Size = new System.Drawing.Size(92, 27);
            this.lblLatSize.TabIndex = 36;
            this.lblLatSize.Text = "lat size";
            this.lblLatSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTimeSpacing
            // 
            this.lblTimeSpacing.AutoSize = true;
            this.lblTimeSpacing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimeSpacing.Location = new System.Drawing.Point(555, 27);
            this.lblTimeSpacing.Name = "lblTimeSpacing";
            this.lblTimeSpacing.Size = new System.Drawing.Size(94, 27);
            this.lblTimeSpacing.TabIndex = 35;
            this.lblTimeSpacing.Text = "time spacing";
            this.lblTimeSpacing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(555, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 27);
            this.label2.TabIndex = 33;
            this.label2.Text = "Spacing";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(457, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 27);
            this.label1.TabIndex = 32;
            this.label1.Text = "Size";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(323, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(128, 27);
            this.label15.TabIndex = 29;
            this.label15.Text = "End";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(256, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 27);
            this.label8.TabIndex = 28;
            this.label8.Text = "Stride";
            // 
            // dtPickerEnd
            // 
            this.dtPickerEnd.CustomFormat = "MMM- dd- yyyy ";
            this.dtPickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerEnd.Location = new System.Drawing.Point(323, 30);
            this.dtPickerEnd.Name = "dtPickerEnd";
            this.dtPickerEnd.Size = new System.Drawing.Size(123, 20);
            this.dtPickerEnd.TabIndex = 27;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(122, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 27);
            this.label7.TabIndex = 24;
            this.label7.Text = "Start";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 27);
            this.label6.TabIndex = 23;
            this.label6.Text = "Dimension";
            // 
            // chkTime
            // 
            this.chkTime.AutoSize = true;
            this.chkTime.Checked = true;
            this.chkTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkTime.Location = new System.Drawing.Point(3, 30);
            this.chkTime.Name = "chkTime";
            this.chkTime.Size = new System.Drawing.Size(88, 21);
            this.chkTime.TabIndex = 25;
            this.chkTime.Text = "time";
            this.chkTime.UseVisualStyleBackColor = true;
            // 
            // chkLat
            // 
            this.chkLat.AutoSize = true;
            this.chkLat.Checked = true;
            this.chkLat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLat.Location = new System.Drawing.Point(3, 84);
            this.chkLat.Name = "chkLat";
            this.chkLat.Size = new System.Drawing.Size(63, 17);
            this.chkLat.TabIndex = 30;
            this.chkLat.Text = "latittude";
            this.chkLat.UseVisualStyleBackColor = true;
            // 
            // chkLon
            // 
            this.chkLon.AutoSize = true;
            this.chkLon.Checked = true;
            this.chkLon.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLon.Location = new System.Drawing.Point(3, 111);
            this.chkLon.Name = "chkLon";
            this.chkLon.Size = new System.Drawing.Size(69, 17);
            this.chkLon.TabIndex = 31;
            this.chkLon.Text = "longitude";
            this.chkLon.UseVisualStyleBackColor = true;
            // 
            // lblTimeSize
            // 
            this.lblTimeSize.AutoSize = true;
            this.lblTimeSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimeSize.Location = new System.Drawing.Point(457, 27);
            this.lblTimeSize.Name = "lblTimeSize";
            this.lblTimeSize.Size = new System.Drawing.Size(92, 27);
            this.lblTimeSize.TabIndex = 34;
            this.lblTimeSize.Text = "time size";
            this.lblTimeSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTimeHelp
            // 
            this.btnTimeHelp.Location = new System.Drawing.Point(97, 30);
            this.btnTimeHelp.Name = "btnTimeHelp";
            this.btnTimeHelp.Size = new System.Drawing.Size(18, 20);
            this.btnTimeHelp.TabIndex = 21;
            this.btnTimeHelp.Text = "?";
            this.btnTimeHelp.UseVisualStyleBackColor = true;
            this.btnTimeHelp.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnLatHelp
            // 
            this.btnLatHelp.Location = new System.Drawing.Point(97, 84);
            this.btnLatHelp.Name = "btnLatHelp";
            this.btnLatHelp.Size = new System.Drawing.Size(18, 20);
            this.btnLatHelp.TabIndex = 40;
            this.btnLatHelp.Text = "?";
            this.btnLatHelp.UseVisualStyleBackColor = true;
            this.btnLatHelp.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnLonHelp
            // 
            this.btnLonHelp.Location = new System.Drawing.Point(97, 111);
            this.btnLonHelp.Name = "btnLonHelp";
            this.btnLonHelp.Size = new System.Drawing.Size(18, 20);
            this.btnLonHelp.TabIndex = 41;
            this.btnLonHelp.Text = "?";
            this.btnLonHelp.UseVisualStyleBackColor = true;
            this.btnLonHelp.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // chkAltitude
            // 
            this.chkAltitude.AutoSize = true;
            this.chkAltitude.Checked = true;
            this.chkAltitude.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAltitude.Location = new System.Drawing.Point(3, 57);
            this.chkAltitude.Name = "chkAltitude";
            this.chkAltitude.Size = new System.Drawing.Size(61, 17);
            this.chkAltitude.TabIndex = 42;
            this.chkAltitude.Text = "Altitude";
            this.chkAltitude.UseVisualStyleBackColor = true;
            // 
            // btnAltitudeHelp
            // 
            this.btnAltitudeHelp.Location = new System.Drawing.Point(97, 57);
            this.btnAltitudeHelp.Name = "btnAltitudeHelp";
            this.btnAltitudeHelp.Size = new System.Drawing.Size(18, 20);
            this.btnAltitudeHelp.TabIndex = 43;
            this.btnAltitudeHelp.Text = "?";
            this.btnAltitudeHelp.UseVisualStyleBackColor = true;
            // 
            // txtStartAltitude
            // 
            this.txtStartAltitude.Location = new System.Drawing.Point(122, 57);
            this.txtStartAltitude.Name = "txtStartAltitude";
            this.txtStartAltitude.Size = new System.Drawing.Size(123, 20);
            this.txtStartAltitude.TabIndex = 21;
            // 
            // txtAltitudeStride
            // 
            this.txtAltitudeStride.Location = new System.Drawing.Point(256, 57);
            this.txtAltitudeStride.Name = "txtAltitudeStride";
            this.txtAltitudeStride.Size = new System.Drawing.Size(61, 20);
            this.txtAltitudeStride.TabIndex = 22;
            this.txtAltitudeStride.Text = "1";
            // 
            // txtEndAltitude
            // 
            this.txtEndAltitude.Location = new System.Drawing.Point(323, 57);
            this.txtEndAltitude.Name = "txtEndAltitude";
            this.txtEndAltitude.Size = new System.Drawing.Size(123, 20);
            this.txtEndAltitude.TabIndex = 44;
            // 
            // lblAltitudeSize
            // 
            this.lblAltitudeSize.AutoSize = true;
            this.lblAltitudeSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAltitudeSize.Location = new System.Drawing.Point(457, 54);
            this.lblAltitudeSize.Name = "lblAltitudeSize";
            this.lblAltitudeSize.Size = new System.Drawing.Size(92, 27);
            this.lblAltitudeSize.TabIndex = 45;
            this.lblAltitudeSize.Text = "altitude size";
            this.lblAltitudeSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAltitudeSpacing
            // 
            this.lblAltitudeSpacing.AutoSize = true;
            this.lblAltitudeSpacing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAltitudeSpacing.Location = new System.Drawing.Point(555, 54);
            this.lblAltitudeSpacing.Name = "lblAltitudeSpacing";
            this.lblAltitudeSpacing.Size = new System.Drawing.Size(94, 27);
            this.lblAltitudeSpacing.TabIndex = 46;
            this.lblAltitudeSpacing.Text = "altitude spacing";
            this.lblAltitudeSpacing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ERDDAPDownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 521);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.lblCredits);
            this.Controls.Add(this.lblConstraints);
            this.Controls.Add(this.lblAbstract);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lvGridParameters);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ERDDAPDownloadForm";
            this.Text = "ERDDAPDownloadForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtboundNorth;
        private System.Windows.Forms.TextBox txtboundSouth;
        private System.Windows.Forms.TextBox txtboundWest;
        private System.Windows.Forms.TextBox txtboundEast;
        private System.Windows.Forms.ListView lvGridParameters;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtPickerStart;
        private System.Windows.Forms.TextBox txtTimeStride;
        private System.Windows.Forms.LinkLabel lblAbstract;
        private System.Windows.Forms.LinkLabel lblConstraints;
        private System.Windows.Forms.LinkLabel lblCredits;
        private System.Windows.Forms.TextBox txtLonStride;
        private System.Windows.Forms.TextBox txtLatStride;
        private System.Windows.Forms.LinkLabel lblURL;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtPickerEnd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkTime;
        private System.Windows.Forms.CheckBox chkLat;
        private System.Windows.Forms.CheckBox chkLon;
        private System.Windows.Forms.Label lblLonSpacing;
        private System.Windows.Forms.Label lblLonSize;
        private System.Windows.Forms.Label lblLatSpacing;
        private System.Windows.Forms.Label lblLatSize;
        private System.Windows.Forms.Label lblTimeSpacing;
        private System.Windows.Forms.Label lblTimeSize;
        private System.Windows.Forms.Button btnTimeHelp;
        private System.Windows.Forms.Button btnLatHelp;
        private System.Windows.Forms.Button btnLonHelp;
        private System.Windows.Forms.TextBox txtAltitudeStride;
        private System.Windows.Forms.TextBox txtStartAltitude;
        private System.Windows.Forms.Button btnAltitudeHelp;
        private System.Windows.Forms.CheckBox chkAltitude;
        private System.Windows.Forms.TextBox txtEndAltitude;
        private System.Windows.Forms.Label lblAltitudeSize;
        private System.Windows.Forms.Label lblAltitudeSpacing;
    }
}