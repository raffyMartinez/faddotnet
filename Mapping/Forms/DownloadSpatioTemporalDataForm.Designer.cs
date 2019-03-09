namespace FAD3.Mapping.Forms
{
    partial class DownloadSpatioTemporalDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadSpatioTemporalDataForm));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtMetadataFolderPath = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.btnGetMetadataFolder = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnDownload = new System.Windows.Forms.Button();
            this.lvERDDAP = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMaxLon = new System.Windows.Forms.TextBox();
            this.txtMinLon = new System.Windows.Forms.TextBox();
            this.txtMaxLat = new System.Windows.Forms.TextBox();
            this.txtMinLat = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.rbtnManual = new System.Windows.Forms.RadioButton();
            this.btnCreateExtent = new System.Windows.Forms.Button();
            this.rbtnUseSelectedLayer = new System.Windows.Forms.RadioButton();
            this.rbtnUseSelectionBox = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblItemsCount = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblItemsCount);
            this.groupBox4.Controls.Add(this.txtMetadataFolderPath);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.btnGetMetadataFolder);
            this.groupBox4.Controls.Add(this.btnDownload);
            this.groupBox4.Controls.Add(this.lvERDDAP);
            this.groupBox4.Location = new System.Drawing.Point(12, 181);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(613, 283);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Data to download";
            // 
            // txtMetadataFolderPath
            // 
            this.txtMetadataFolderPath.Location = new System.Drawing.Point(58, 20);
            this.txtMetadataFolderPath.Name = "txtMetadataFolderPath";
            this.txtMetadataFolderPath.Size = new System.Drawing.Size(488, 20);
            this.txtMetadataFolderPath.TabIndex = 16;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(14, 25);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(36, 13);
            this.label20.TabIndex = 15;
            this.label20.Text = "Folder";
            // 
            // btnGetMetadataFolder
            // 
            this.btnGetMetadataFolder.ImageKey = "Folder_16x.png";
            this.btnGetMetadataFolder.ImageList = this.imageList1;
            this.btnGetMetadataFolder.Location = new System.Drawing.Point(556, 16);
            this.btnGetMetadataFolder.Name = "btnGetMetadataFolder";
            this.btnGetMetadataFolder.Size = new System.Drawing.Size(36, 32);
            this.btnGetMetadataFolder.TabIndex = 14;
            this.btnGetMetadataFolder.UseVisualStyleBackColor = true;
            this.btnGetMetadataFolder.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Folder_16x.png");
            this.imageList1.Images.SetKeyName(1, "WebFile_16x.png");
            this.imageList1.Images.SetKeyName(2, "im_exit.bmp");
            // 
            // btnDownload
            // 
            this.btnDownload.ImageKey = "WebFile_16x.png";
            this.btnDownload.ImageList = this.imageList1;
            this.btnDownload.Location = new System.Drawing.Point(556, 245);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(36, 32);
            this.btnDownload.TabIndex = 12;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lvERDDAP
            // 
            this.lvERDDAP.FullRowSelect = true;
            this.lvERDDAP.HideSelection = false;
            this.lvERDDAP.Location = new System.Drawing.Point(12, 61);
            this.lvERDDAP.Name = "lvERDDAP";
            this.lvERDDAP.Size = new System.Drawing.Size(580, 175);
            this.lvERDDAP.TabIndex = 0;
            this.lvERDDAP.UseCompatibleStateImageBehavior = false;
            this.lvERDDAP.View = System.Windows.Forms.View.Details;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMaxLon);
            this.groupBox1.Controls.Add(this.txtMinLon);
            this.groupBox1.Controls.Add(this.txtMaxLat);
            this.groupBox1.Controls.Add(this.txtMinLat);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.rbtnManual);
            this.groupBox1.Controls.Add(this.btnCreateExtent);
            this.groupBox1.Controls.Add(this.rbtnUseSelectedLayer);
            this.groupBox1.Controls.Add(this.rbtnUseSelectionBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(613, 145);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Set extents";
            // 
            // txtMaxLon
            // 
            this.txtMaxLon.Location = new System.Drawing.Point(236, 103);
            this.txtMaxLon.Name = "txtMaxLon";
            this.txtMaxLon.Size = new System.Drawing.Size(155, 20);
            this.txtMaxLon.TabIndex = 11;
            this.txtMaxLon.Tag = "extent";
            // 
            // txtMinLon
            // 
            this.txtMinLon.Location = new System.Drawing.Point(236, 76);
            this.txtMinLon.Name = "txtMinLon";
            this.txtMinLon.Size = new System.Drawing.Size(155, 20);
            this.txtMinLon.TabIndex = 10;
            this.txtMinLon.Tag = "extent";
            // 
            // txtMaxLat
            // 
            this.txtMaxLat.Location = new System.Drawing.Point(236, 50);
            this.txtMaxLat.Name = "txtMaxLat";
            this.txtMaxLat.Size = new System.Drawing.Size(155, 20);
            this.txtMaxLat.TabIndex = 9;
            this.txtMaxLat.Tag = "extent";
            // 
            // txtMinLat
            // 
            this.txtMinLat.Location = new System.Drawing.Point(236, 22);
            this.txtMinLat.Name = "txtMinLat";
            this.txtMinLat.Size = new System.Drawing.Size(155, 20);
            this.txtMinLat.TabIndex = 8;
            this.txtMinLat.Tag = "extent";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(154, 106);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(76, 13);
            this.label19.TabIndex = 7;
            this.label19.Text = "Max. longitude";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(154, 79);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(73, 13);
            this.label18.TabIndex = 6;
            this.label18.Text = "Min. longitude";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(154, 53);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(67, 13);
            this.label17.TabIndex = 5;
            this.label17.Text = "Max. latitude";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(154, 25);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(64, 13);
            this.label16.TabIndex = 4;
            this.label16.Text = "Min. latitude";
            // 
            // rbtnManual
            // 
            this.rbtnManual.AutoSize = true;
            this.rbtnManual.Location = new System.Drawing.Point(12, 68);
            this.rbtnManual.Name = "rbtnManual";
            this.rbtnManual.Size = new System.Drawing.Size(100, 17);
            this.rbtnManual.TabIndex = 3;
            this.rbtnManual.TabStop = true;
            this.rbtnManual.Text = "Define manually";
            this.rbtnManual.UseVisualStyleBackColor = true;
            this.rbtnManual.CheckedChanged += new System.EventHandler(this.OnRadioButtonCheckChange);
            // 
            // btnCreateExtent
            // 
            this.btnCreateExtent.Location = new System.Drawing.Point(22, 99);
            this.btnCreateExtent.Name = "btnCreateExtent";
            this.btnCreateExtent.Size = new System.Drawing.Size(78, 27);
            this.btnCreateExtent.TabIndex = 2;
            this.btnCreateExtent.Text = "Create extent";
            this.btnCreateExtent.UseVisualStyleBackColor = true;
            this.btnCreateExtent.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // rbtnUseSelectedLayer
            // 
            this.rbtnUseSelectedLayer.AutoSize = true;
            this.rbtnUseSelectedLayer.Location = new System.Drawing.Point(12, 45);
            this.rbtnUseSelectedLayer.Name = "rbtnUseSelectedLayer";
            this.rbtnUseSelectedLayer.Size = new System.Drawing.Size(112, 17);
            this.rbtnUseSelectedLayer.TabIndex = 1;
            this.rbtnUseSelectedLayer.TabStop = true;
            this.rbtnUseSelectedLayer.Text = "Use selected layer";
            this.rbtnUseSelectedLayer.UseVisualStyleBackColor = true;
            this.rbtnUseSelectedLayer.CheckedChanged += new System.EventHandler(this.OnRadioButtonCheckChange);
            // 
            // rbtnUseSelectionBox
            // 
            this.rbtnUseSelectionBox.AutoSize = true;
            this.rbtnUseSelectionBox.Location = new System.Drawing.Point(12, 22);
            this.rbtnUseSelectionBox.Name = "rbtnUseSelectionBox";
            this.rbtnUseSelectionBox.Size = new System.Drawing.Size(118, 17);
            this.rbtnUseSelectionBox.TabIndex = 0;
            this.rbtnUseSelectionBox.TabStop = true;
            this.rbtnUseSelectionBox.Text = "Use a selection box";
            this.rbtnUseSelectionBox.UseVisualStyleBackColor = true;
            this.rbtnUseSelectionBox.CheckedChanged += new System.EventHandler(this.OnRadioButtonCheckChange);
            // 
            // btnOk
            // 
            this.btnOk.ImageKey = "im_exit.bmp";
            this.btnOk.ImageList = this.imageList1;
            this.btnOk.Location = new System.Drawing.Point(636, 37);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(36, 32);
            this.btnOk.TabIndex = 4;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblItemsCount
            // 
            this.lblItemsCount.AutoSize = true;
            this.lblItemsCount.Location = new System.Drawing.Point(9, 251);
            this.lblItemsCount.Name = "lblItemsCount";
            this.lblItemsCount.Size = new System.Drawing.Size(92, 13);
            this.lblItemsCount.TabIndex = 17;
            this.lblItemsCount.Text = "No items in the list";
            // 
            // DownloadSpatioTemporalDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 476);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DownloadSpatioTemporalDataForm";
            this.Text = "DownloadSpatioTemporalDataForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtMetadataFolderPath;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnGetMetadataFolder;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.ListView lvERDDAP;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMaxLon;
        private System.Windows.Forms.TextBox txtMinLon;
        private System.Windows.Forms.TextBox txtMaxLat;
        private System.Windows.Forms.TextBox txtMinLat;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.RadioButton rbtnManual;
        private System.Windows.Forms.Button btnCreateExtent;
        private System.Windows.Forms.RadioButton rbtnUseSelectedLayer;
        private System.Windows.Forms.RadioButton rbtnUseSelectionBox;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblItemsCount;
    }
}