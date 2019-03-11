namespace FAD3.Mapping.Forms
{
    partial class ERDDAPConfigureDownloadForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ERDDAPConfigureDownloadForm));
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.Button();
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
            this.lblErrStartTime = new System.Windows.Forms.Label();
            this.txtAltitudeStride = new System.Windows.Forms.TextBox();
            this.txtStartAltitude = new System.Windows.Forms.TextBox();
            this.btnAltitudeHelp = new System.Windows.Forms.Button();
            this.btnTimeHelp = new System.Windows.Forms.Button();
            this.lblLonSpacing = new System.Windows.Forms.Label();
            this.lblLonSize = new System.Windows.Forms.Label();
            this.lblLatSpacing = new System.Windows.Forms.Label();
            this.lblLatSize = new System.Windows.Forms.Label();
            this.lblTimeSpacing = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLonHelp = new System.Windows.Forms.Button();
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
            this.chkAltitude = new System.Windows.Forms.CheckBox();
            this.txtEndAltitude = new System.Windows.Forms.TextBox();
            this.lblAltitudeSize = new System.Windows.Forms.Label();
            this.lblAltitudeSpacing = new System.Windows.Forms.Label();
            this.btnLatHelp = new System.Windows.Forms.Button();
            this.lblErrStartAltitude = new System.Windows.Forms.Label();
            this.lblErrStartLatitude = new System.Windows.Forms.Label();
            this.lblErrStartLongitude = new System.Windows.Forms.Label();
            this.lblErrStrideTime = new System.Windows.Forms.Label();
            this.lblErrStrideAltitude = new System.Windows.Forms.Label();
            this.lblErrStrideLatitude = new System.Windows.Forms.Label();
            this.lblErrStrideLongitude = new System.Windows.Forms.Label();
            this.lblErrEndTime = new System.Windows.Forms.Label();
            this.lblErrEndAltitude = new System.Windows.Forms.Label();
            this.lblErrEndLatitude = new System.Windows.Forms.Label();
            this.lblErrEndLongitude = new System.Windows.Forms.Label();
            this.cboFileType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
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
            // btnDownload
            // 
            this.btnDownload.ImageKey = "Download_16x.png";
            this.btnDownload.ImageList = this.imageList1;
            this.btnDownload.Location = new System.Drawing.Point(637, 539);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(30, 30);
            this.btnDownload.TabIndex = 3;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.CausesValidation = false;
            this.btnCancel.ImageKey = "im_exit.bmp";
            this.btnCancel.ImageList = this.imageList1;
            this.btnCancel.Location = new System.Drawing.Point(600, 539);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(30, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // txtboundNorth
            // 
            this.txtboundNorth.Location = new System.Drawing.Point(129, 205);
            this.txtboundNorth.Name = "txtboundNorth";
            this.txtboundNorth.Size = new System.Drawing.Size(118, 20);
            this.txtboundNorth.TabIndex = 5;
            this.txtboundNorth.Tag = "extent";
            this.txtboundNorth.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidate);
            // 
            // txtboundSouth
            // 
            this.txtboundSouth.Location = new System.Drawing.Point(344, 205);
            this.txtboundSouth.Name = "txtboundSouth";
            this.txtboundSouth.Size = new System.Drawing.Size(118, 20);
            this.txtboundSouth.TabIndex = 7;
            this.txtboundSouth.Tag = "extent";
            this.txtboundSouth.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidate);
            // 
            // txtboundWest
            // 
            this.txtboundWest.Location = new System.Drawing.Point(344, 232);
            this.txtboundWest.Name = "txtboundWest";
            this.txtboundWest.Size = new System.Drawing.Size(118, 20);
            this.txtboundWest.TabIndex = 8;
            this.txtboundWest.Tag = "extent";
            this.txtboundWest.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidate);
            // 
            // txtboundEast
            // 
            this.txtboundEast.Location = new System.Drawing.Point(129, 232);
            this.txtboundEast.Name = "txtboundEast";
            this.txtboundEast.Size = new System.Drawing.Size(118, 20);
            this.txtboundEast.TabIndex = 9;
            this.txtboundEast.Tag = "extent";
            this.txtboundEast.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidate);
            // 
            // txtLatStride
            // 
            this.txtLatStride.Location = new System.Drawing.Point(267, 205);
            this.txtLatStride.Name = "txtLatStride";
            this.txtLatStride.Size = new System.Drawing.Size(56, 20);
            this.txtLatStride.TabIndex = 20;
            this.txtLatStride.Text = "1";
            this.txtLatStride.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidate);
            // 
            // txtLonStride
            // 
            this.txtLonStride.Location = new System.Drawing.Point(267, 232);
            this.txtLonStride.Name = "txtLonStride";
            this.txtLonStride.Size = new System.Drawing.Size(56, 20);
            this.txtLonStride.TabIndex = 18;
            this.txtLonStride.Text = "1";
            this.txtLonStride.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidate);
            // 
            // lvGridParameters
            // 
            this.lvGridParameters.Location = new System.Drawing.Point(12, 283);
            this.lvGridParameters.Name = "lvGridParameters";
            this.lvGridParameters.Size = new System.Drawing.Size(658, 184);
            this.lvGridParameters.TabIndex = 11;
            this.lvGridParameters.UseCompatibleStateImageBehavior = false;
            this.lvGridParameters.View = System.Windows.Forms.View.Details;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 267);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Grid variables";
            // 
            // txtTimeStride
            // 
            this.txtTimeStride.Location = new System.Drawing.Point(267, 152);
            this.txtTimeStride.Name = "txtTimeStride";
            this.txtTimeStride.Size = new System.Drawing.Size(56, 20);
            this.txtTimeStride.TabIndex = 16;
            this.txtTimeStride.Text = "1";
            this.txtTimeStride.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidate);
            // 
            // dtPickerStart
            // 
            this.dtPickerStart.CustomFormat = "MMM- dd- yyyy ";
            this.dtPickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerStart.Location = new System.Drawing.Point(129, 152);
            this.dtPickerStart.Name = "dtPickerStart";
            this.dtPickerStart.Size = new System.Drawing.Size(118, 20);
            this.dtPickerStart.TabIndex = 0;
            this.dtPickerStart.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidate);
            // 
            // lblAbstract
            // 
            this.lblAbstract.AutoSize = true;
            this.lblAbstract.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbstract.Location = new System.Drawing.Point(298, 83);
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
            this.lblConstraints.Location = new System.Drawing.Point(404, 83);
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
            this.lblCredits.Location = new System.Drawing.Point(206, 83);
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
            this.lblURL.Location = new System.Drawing.Point(15, 528);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(102, 17);
            this.lblURL.TabIndex = 20;
            this.lblURL.TabStop = true;
            this.lblURL.Text = "Download URL";
            this.lblURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClick);
            // 
            // lblErrStartTime
            // 
            this.lblErrStartTime.AutoSize = true;
            this.lblErrStartTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrStartTime.ForeColor = System.Drawing.Color.Red;
            this.lblErrStartTime.Location = new System.Drawing.Point(246, 156);
            this.lblErrStartTime.Name = "lblErrStartTime";
            this.lblErrStartTime.Size = new System.Drawing.Size(11, 13);
            this.lblErrStartTime.TabIndex = 48;
            this.lblErrStartTime.Text = "!";
            this.lblErrStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblErrStartTime.Visible = false;
            // 
            // txtAltitudeStride
            // 
            this.txtAltitudeStride.Location = new System.Drawing.Point(267, 178);
            this.txtAltitudeStride.Name = "txtAltitudeStride";
            this.txtAltitudeStride.Size = new System.Drawing.Size(56, 20);
            this.txtAltitudeStride.TabIndex = 22;
            this.txtAltitudeStride.Text = "1";
            this.txtAltitudeStride.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidate);
            // 
            // txtStartAltitude
            // 
            this.txtStartAltitude.Location = new System.Drawing.Point(129, 178);
            this.txtStartAltitude.Name = "txtStartAltitude";
            this.txtStartAltitude.Size = new System.Drawing.Size(118, 20);
            this.txtStartAltitude.TabIndex = 21;
            this.txtStartAltitude.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidate);
            // 
            // btnAltitudeHelp
            // 
            this.btnAltitudeHelp.CausesValidation = false;
            this.btnAltitudeHelp.Location = new System.Drawing.Point(103, 178);
            this.btnAltitudeHelp.Name = "btnAltitudeHelp";
            this.btnAltitudeHelp.Size = new System.Drawing.Size(18, 20);
            this.btnAltitudeHelp.TabIndex = 43;
            this.btnAltitudeHelp.Text = "?";
            this.btnAltitudeHelp.UseVisualStyleBackColor = true;
            // 
            // btnTimeHelp
            // 
            this.btnTimeHelp.CausesValidation = false;
            this.btnTimeHelp.Location = new System.Drawing.Point(103, 152);
            this.btnTimeHelp.Name = "btnTimeHelp";
            this.btnTimeHelp.Size = new System.Drawing.Size(18, 20);
            this.btnTimeHelp.TabIndex = 21;
            this.btnTimeHelp.Text = "?";
            this.btnTimeHelp.UseVisualStyleBackColor = true;
            this.btnTimeHelp.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblLonSpacing
            // 
            this.lblLonSpacing.AutoSize = true;
            this.lblLonSpacing.Location = new System.Drawing.Point(574, 236);
            this.lblLonSpacing.Name = "lblLonSpacing";
            this.lblLonSpacing.Size = new System.Drawing.Size(61, 13);
            this.lblLonSpacing.TabIndex = 39;
            this.lblLonSpacing.Text = "lon spacing";
            this.lblLonSpacing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLonSize
            // 
            this.lblLonSize.AutoSize = true;
            this.lblLonSize.Location = new System.Drawing.Point(483, 236);
            this.lblLonSize.Name = "lblLonSize";
            this.lblLonSize.Size = new System.Drawing.Size(42, 13);
            this.lblLonSize.TabIndex = 38;
            this.lblLonSize.Text = "lon size";
            this.lblLonSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLatSpacing
            // 
            this.lblLatSpacing.AutoSize = true;
            this.lblLatSpacing.Location = new System.Drawing.Point(574, 209);
            this.lblLatSpacing.Name = "lblLatSpacing";
            this.lblLatSpacing.Size = new System.Drawing.Size(58, 13);
            this.lblLatSpacing.TabIndex = 37;
            this.lblLatSpacing.Text = "lat spacing";
            this.lblLatSpacing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLatSize
            // 
            this.lblLatSize.AutoSize = true;
            this.lblLatSize.Location = new System.Drawing.Point(483, 209);
            this.lblLatSize.Name = "lblLatSize";
            this.lblLatSize.Size = new System.Drawing.Size(39, 13);
            this.lblLatSize.TabIndex = 36;
            this.lblLatSize.Text = "lat size";
            this.lblLatSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTimeSpacing
            // 
            this.lblTimeSpacing.AutoSize = true;
            this.lblTimeSpacing.Location = new System.Drawing.Point(574, 156);
            this.lblTimeSpacing.Name = "lblTimeSpacing";
            this.lblTimeSpacing.Size = new System.Drawing.Size(66, 13);
            this.lblTimeSpacing.TabIndex = 35;
            this.lblTimeSpacing.Text = "time spacing";
            this.lblTimeSpacing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(576, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 15);
            this.label2.TabIndex = 33;
            this.label2.Text = "Spacing";
            // 
            // btnLonHelp
            // 
            this.btnLonHelp.CausesValidation = false;
            this.btnLonHelp.Location = new System.Drawing.Point(103, 232);
            this.btnLonHelp.Name = "btnLonHelp";
            this.btnLonHelp.Size = new System.Drawing.Size(18, 20);
            this.btnLonHelp.TabIndex = 41;
            this.btnLonHelp.Text = "?";
            this.btnLonHelp.UseVisualStyleBackColor = true;
            this.btnLonHelp.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(485, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 15);
            this.label1.TabIndex = 32;
            this.label1.Text = "Size";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(346, 127);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 15);
            this.label15.TabIndex = 29;
            this.label15.Text = "End";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(269, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 15);
            this.label8.TabIndex = 28;
            this.label8.Text = "Stride";
            // 
            // dtPickerEnd
            // 
            this.dtPickerEnd.CustomFormat = "MMM- dd- yyyy ";
            this.dtPickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerEnd.Location = new System.Drawing.Point(344, 152);
            this.dtPickerEnd.Name = "dtPickerEnd";
            this.dtPickerEnd.Size = new System.Drawing.Size(118, 20);
            this.dtPickerEnd.TabIndex = 27;
            this.dtPickerEnd.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidate);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(130, 127);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 15);
            this.label7.TabIndex = 24;
            this.label7.Text = "Start";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(18, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 15);
            this.label6.TabIndex = 23;
            this.label6.Text = "Dimension";
            // 
            // chkTime
            // 
            this.chkTime.AutoSize = true;
            this.chkTime.Checked = true;
            this.chkTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTime.Location = new System.Drawing.Point(28, 154);
            this.chkTime.Name = "chkTime";
            this.chkTime.Size = new System.Drawing.Size(45, 17);
            this.chkTime.TabIndex = 25;
            this.chkTime.Text = "time";
            this.chkTime.UseVisualStyleBackColor = true;
            // 
            // chkLat
            // 
            this.chkLat.AutoSize = true;
            this.chkLat.Checked = true;
            this.chkLat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLat.Location = new System.Drawing.Point(28, 207);
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
            this.chkLon.Location = new System.Drawing.Point(28, 234);
            this.chkLon.Name = "chkLon";
            this.chkLon.Size = new System.Drawing.Size(69, 17);
            this.chkLon.TabIndex = 31;
            this.chkLon.Text = "longitude";
            this.chkLon.UseVisualStyleBackColor = true;
            // 
            // lblTimeSize
            // 
            this.lblTimeSize.AutoSize = true;
            this.lblTimeSize.Location = new System.Drawing.Point(483, 156);
            this.lblTimeSize.Name = "lblTimeSize";
            this.lblTimeSize.Size = new System.Drawing.Size(47, 13);
            this.lblTimeSize.TabIndex = 34;
            this.lblTimeSize.Text = "time size";
            this.lblTimeSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkAltitude
            // 
            this.chkAltitude.AutoSize = true;
            this.chkAltitude.Checked = true;
            this.chkAltitude.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAltitude.Location = new System.Drawing.Point(28, 180);
            this.chkAltitude.Name = "chkAltitude";
            this.chkAltitude.Size = new System.Drawing.Size(61, 17);
            this.chkAltitude.TabIndex = 42;
            this.chkAltitude.Text = "Altitude";
            this.chkAltitude.UseVisualStyleBackColor = true;
            // 
            // txtEndAltitude
            // 
            this.txtEndAltitude.Location = new System.Drawing.Point(344, 178);
            this.txtEndAltitude.Name = "txtEndAltitude";
            this.txtEndAltitude.Size = new System.Drawing.Size(118, 20);
            this.txtEndAltitude.TabIndex = 44;
            this.txtEndAltitude.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidate);
            // 
            // lblAltitudeSize
            // 
            this.lblAltitudeSize.AutoSize = true;
            this.lblAltitudeSize.Location = new System.Drawing.Point(483, 182);
            this.lblAltitudeSize.Name = "lblAltitudeSize";
            this.lblAltitudeSize.Size = new System.Drawing.Size(62, 13);
            this.lblAltitudeSize.TabIndex = 45;
            this.lblAltitudeSize.Text = "altitude size";
            this.lblAltitudeSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAltitudeSpacing
            // 
            this.lblAltitudeSpacing.AutoSize = true;
            this.lblAltitudeSpacing.Location = new System.Drawing.Point(574, 182);
            this.lblAltitudeSpacing.Name = "lblAltitudeSpacing";
            this.lblAltitudeSpacing.Size = new System.Drawing.Size(81, 13);
            this.lblAltitudeSpacing.TabIndex = 46;
            this.lblAltitudeSpacing.Text = "altitude spacing";
            this.lblAltitudeSpacing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLatHelp
            // 
            this.btnLatHelp.CausesValidation = false;
            this.btnLatHelp.Location = new System.Drawing.Point(103, 205);
            this.btnLatHelp.Name = "btnLatHelp";
            this.btnLatHelp.Size = new System.Drawing.Size(18, 20);
            this.btnLatHelp.TabIndex = 40;
            this.btnLatHelp.Text = "?";
            this.btnLatHelp.UseVisualStyleBackColor = true;
            this.btnLatHelp.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblErrStartAltitude
            // 
            this.lblErrStartAltitude.AutoSize = true;
            this.lblErrStartAltitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrStartAltitude.ForeColor = System.Drawing.Color.Red;
            this.lblErrStartAltitude.Location = new System.Drawing.Point(246, 182);
            this.lblErrStartAltitude.Name = "lblErrStartAltitude";
            this.lblErrStartAltitude.Size = new System.Drawing.Size(11, 13);
            this.lblErrStartAltitude.TabIndex = 49;
            this.lblErrStartAltitude.Text = "!";
            this.lblErrStartAltitude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblErrStartAltitude.Visible = false;
            // 
            // lblErrStartLatitude
            // 
            this.lblErrStartLatitude.AutoSize = true;
            this.lblErrStartLatitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrStartLatitude.ForeColor = System.Drawing.Color.Red;
            this.lblErrStartLatitude.Location = new System.Drawing.Point(246, 209);
            this.lblErrStartLatitude.Name = "lblErrStartLatitude";
            this.lblErrStartLatitude.Size = new System.Drawing.Size(11, 13);
            this.lblErrStartLatitude.TabIndex = 50;
            this.lblErrStartLatitude.Text = "!";
            this.lblErrStartLatitude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblErrStartLatitude.Visible = false;
            // 
            // lblErrStartLongitude
            // 
            this.lblErrStartLongitude.AutoSize = true;
            this.lblErrStartLongitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrStartLongitude.ForeColor = System.Drawing.Color.Red;
            this.lblErrStartLongitude.Location = new System.Drawing.Point(246, 236);
            this.lblErrStartLongitude.Name = "lblErrStartLongitude";
            this.lblErrStartLongitude.Size = new System.Drawing.Size(11, 13);
            this.lblErrStartLongitude.TabIndex = 51;
            this.lblErrStartLongitude.Text = "!";
            this.lblErrStartLongitude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblErrStartLongitude.Visible = false;
            // 
            // lblErrStrideTime
            // 
            this.lblErrStrideTime.AutoSize = true;
            this.lblErrStrideTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrStrideTime.ForeColor = System.Drawing.Color.Red;
            this.lblErrStrideTime.Location = new System.Drawing.Point(321, 156);
            this.lblErrStrideTime.Name = "lblErrStrideTime";
            this.lblErrStrideTime.Size = new System.Drawing.Size(11, 13);
            this.lblErrStrideTime.TabIndex = 52;
            this.lblErrStrideTime.Text = "!";
            this.lblErrStrideTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblErrStrideTime.Visible = false;
            // 
            // lblErrStrideAltitude
            // 
            this.lblErrStrideAltitude.AutoSize = true;
            this.lblErrStrideAltitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrStrideAltitude.ForeColor = System.Drawing.Color.Red;
            this.lblErrStrideAltitude.Location = new System.Drawing.Point(321, 182);
            this.lblErrStrideAltitude.Name = "lblErrStrideAltitude";
            this.lblErrStrideAltitude.Size = new System.Drawing.Size(11, 13);
            this.lblErrStrideAltitude.TabIndex = 53;
            this.lblErrStrideAltitude.Text = "!";
            this.lblErrStrideAltitude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblErrStrideAltitude.Visible = false;
            // 
            // lblErrStrideLatitude
            // 
            this.lblErrStrideLatitude.AutoSize = true;
            this.lblErrStrideLatitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrStrideLatitude.ForeColor = System.Drawing.Color.Red;
            this.lblErrStrideLatitude.Location = new System.Drawing.Point(321, 209);
            this.lblErrStrideLatitude.Name = "lblErrStrideLatitude";
            this.lblErrStrideLatitude.Size = new System.Drawing.Size(11, 13);
            this.lblErrStrideLatitude.TabIndex = 54;
            this.lblErrStrideLatitude.Text = "!";
            this.lblErrStrideLatitude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblErrStrideLatitude.Visible = false;
            // 
            // lblErrStrideLongitude
            // 
            this.lblErrStrideLongitude.AutoSize = true;
            this.lblErrStrideLongitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrStrideLongitude.ForeColor = System.Drawing.Color.Red;
            this.lblErrStrideLongitude.Location = new System.Drawing.Point(321, 236);
            this.lblErrStrideLongitude.Name = "lblErrStrideLongitude";
            this.lblErrStrideLongitude.Size = new System.Drawing.Size(11, 13);
            this.lblErrStrideLongitude.TabIndex = 55;
            this.lblErrStrideLongitude.Text = "!";
            this.lblErrStrideLongitude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblErrStrideLongitude.Visible = false;
            // 
            // lblErrEndTime
            // 
            this.lblErrEndTime.AutoSize = true;
            this.lblErrEndTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrEndTime.ForeColor = System.Drawing.Color.Red;
            this.lblErrEndTime.Location = new System.Drawing.Point(460, 156);
            this.lblErrEndTime.Name = "lblErrEndTime";
            this.lblErrEndTime.Size = new System.Drawing.Size(11, 13);
            this.lblErrEndTime.TabIndex = 56;
            this.lblErrEndTime.Text = "!";
            this.lblErrEndTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblErrEndTime.Visible = false;
            // 
            // lblErrEndAltitude
            // 
            this.lblErrEndAltitude.AutoSize = true;
            this.lblErrEndAltitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrEndAltitude.ForeColor = System.Drawing.Color.Red;
            this.lblErrEndAltitude.Location = new System.Drawing.Point(460, 182);
            this.lblErrEndAltitude.Name = "lblErrEndAltitude";
            this.lblErrEndAltitude.Size = new System.Drawing.Size(11, 13);
            this.lblErrEndAltitude.TabIndex = 57;
            this.lblErrEndAltitude.Text = "!";
            this.lblErrEndAltitude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblErrEndAltitude.Visible = false;
            // 
            // lblErrEndLatitude
            // 
            this.lblErrEndLatitude.AutoSize = true;
            this.lblErrEndLatitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrEndLatitude.ForeColor = System.Drawing.Color.Red;
            this.lblErrEndLatitude.Location = new System.Drawing.Point(460, 209);
            this.lblErrEndLatitude.Name = "lblErrEndLatitude";
            this.lblErrEndLatitude.Size = new System.Drawing.Size(11, 13);
            this.lblErrEndLatitude.TabIndex = 58;
            this.lblErrEndLatitude.Text = "!";
            this.lblErrEndLatitude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblErrEndLatitude.Visible = false;
            // 
            // lblErrEndLongitude
            // 
            this.lblErrEndLongitude.AutoSize = true;
            this.lblErrEndLongitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrEndLongitude.ForeColor = System.Drawing.Color.Red;
            this.lblErrEndLongitude.Location = new System.Drawing.Point(460, 236);
            this.lblErrEndLongitude.Name = "lblErrEndLongitude";
            this.lblErrEndLongitude.Size = new System.Drawing.Size(11, 13);
            this.lblErrEndLongitude.TabIndex = 59;
            this.lblErrEndLongitude.Text = "!";
            this.lblErrEndLongitude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblErrEndLongitude.Visible = false;
            // 
            // cboFileType
            // 
            this.cboFileType.FormattingEnabled = true;
            this.cboFileType.Location = new System.Drawing.Point(67, 485);
            this.cboFileType.Name = "cboFileType";
            this.cboFileType.Size = new System.Drawing.Size(603, 21);
            this.cboFileType.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 488);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "File type";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRefresh
            // 
            this.btnRefresh.CausesValidation = false;
            this.btnRefresh.ImageKey = "Refresh_16x.png";
            this.btnRefresh.ImageList = this.imageList1;
            this.btnRefresh.Location = new System.Drawing.Point(563, 539);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(30, 30);
            this.btnRefresh.TabIndex = 39;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "im_exit.bmp");
            this.imageList1.Images.SetKeyName(1, "Refresh_16x.png");
            this.imageList1.Images.SetKeyName(2, "Download_16x.png");
            // 
            // ERDDAPDownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 577);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblErrStartTime);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtAltitudeStride);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnAltitudeHelp);
            this.Controls.Add(this.txtStartAltitude);
            this.Controls.Add(this.btnTimeHelp);
            this.Controls.Add(this.cboFileType);
            this.Controls.Add(this.lblLonSpacing);
            this.Controls.Add(this.lblLonSize);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.lblLatSpacing);
            this.Controls.Add(this.lblCredits);
            this.Controls.Add(this.lblLatSize);
            this.Controls.Add(this.lblConstraints);
            this.Controls.Add(this.lblTimeSpacing);
            this.Controls.Add(this.lblAbstract);
            this.Controls.Add(this.btnLonHelp);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lvGridParameters);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.txtLatStride);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtLonStride);
            this.Controls.Add(this.chkLon);
            this.Controls.Add(this.txtTimeStride);
            this.Controls.Add(this.chkAltitude);
            this.Controls.Add(this.dtPickerEnd);
            this.Controls.Add(this.chkLat);
            this.Controls.Add(this.dtPickerStart);
            this.Controls.Add(this.txtboundNorth);
            this.Controls.Add(this.txtboundWest);
            this.Controls.Add(this.txtboundEast);
            this.Controls.Add(this.txtboundSouth);
            this.Controls.Add(this.chkTime);
            this.Controls.Add(this.lblTimeSize);
            this.Controls.Add(this.lblErrEndLongitude);
            this.Controls.Add(this.txtEndAltitude);
            this.Controls.Add(this.lblErrEndLatitude);
            this.Controls.Add(this.lblAltitudeSize);
            this.Controls.Add(this.lblErrEndAltitude);
            this.Controls.Add(this.lblAltitudeSpacing);
            this.Controls.Add(this.lblErrEndTime);
            this.Controls.Add(this.btnLatHelp);
            this.Controls.Add(this.lblErrStrideLongitude);
            this.Controls.Add(this.lblErrStartAltitude);
            this.Controls.Add(this.lblErrStrideLatitude);
            this.Controls.Add(this.lblErrStartLatitude);
            this.Controls.Add(this.lblErrStrideAltitude);
            this.Controls.Add(this.lblErrStartLongitude);
            this.Controls.Add(this.lblErrStrideTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ERDDAPDownloadForm";
            this.Text = "ERDDAPDownloadForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnDownload;
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
        private System.Windows.Forms.ComboBox cboFileType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblErrStartTime;
        private System.Windows.Forms.Label lblErrStartAltitude;
        private System.Windows.Forms.Label lblErrStartLatitude;
        private System.Windows.Forms.Label lblErrStartLongitude;
        private System.Windows.Forms.Label lblErrStrideTime;
        private System.Windows.Forms.Label lblErrStrideAltitude;
        private System.Windows.Forms.Label lblErrStrideLatitude;
        private System.Windows.Forms.Label lblErrStrideLongitude;
        private System.Windows.Forms.Label lblErrEndTime;
        private System.Windows.Forms.Label lblErrEndAltitude;
        private System.Windows.Forms.Label lblErrEndLatitude;
        private System.Windows.Forms.Label lblErrEndLongitude;
        private System.Windows.Forms.ImageList imageList1;
    }
}