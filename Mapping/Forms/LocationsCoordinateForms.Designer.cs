namespace FAD3.Mapping.Forms
{
    partial class LocationsCoordinateForm
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
            this.lvCoordinates = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.menuDropDown = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnMapPoints = new System.Windows.Forms.Button();
            this.chkShowOnMap = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lvCoordinates
            // 
            this.lvCoordinates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvCoordinates.Location = new System.Drawing.Point(1, 37);
            this.lvCoordinates.Name = "lvCoordinates";
            this.lvCoordinates.Size = new System.Drawing.Size(399, 362);
            this.lvCoordinates.TabIndex = 0;
            this.lvCoordinates.UseCompatibleStateImageBehavior = false;
            this.lvCoordinates.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListMouseDown);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Image = global::FAD3.Properties.Resources.im_exit;
            this.btnClose.Location = new System.Drawing.Point(360, 408);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 32);
            this.btnClose.TabIndex = 1;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(6, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(134, 13);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Coordinates of locations in ";
            // 
            // menuDropDown
            // 
            this.menuDropDown.Name = "menuDropDown";
            this.menuDropDown.Size = new System.Drawing.Size(61, 4);
            this.menuDropDown.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnDropDownItemClicked);
            // 
            // btnMapPoints
            // 
            this.btnMapPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMapPoints.Image = global::FAD3.Properties.Resources.MapTileLayer_16x_24;
            this.btnMapPoints.Location = new System.Drawing.Point(324, 408);
            this.btnMapPoints.Name = "btnMapPoints";
            this.btnMapPoints.Size = new System.Drawing.Size(30, 32);
            this.btnMapPoints.TabIndex = 4;
            this.btnMapPoints.UseVisualStyleBackColor = true;
            this.btnMapPoints.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // chkShowOnMap
            // 
            this.chkShowOnMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowOnMap.AutoSize = true;
            this.chkShowOnMap.Enabled = false;
            this.chkShowOnMap.Location = new System.Drawing.Point(9, 417);
            this.chkShowOnMap.Name = "chkShowOnMap";
            this.chkShowOnMap.Size = new System.Drawing.Size(149, 17);
            this.chkShowOnMap.TabIndex = 5;
            this.chkShowOnMap.Text = "Show coordinates on map";
            this.chkShowOnMap.UseVisualStyleBackColor = true;
            // 
            // LocationsCoordinateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 450);
            this.Controls.Add(this.chkShowOnMap);
            this.Controls.Add(this.btnMapPoints);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvCoordinates);
            this.Name = "LocationsCoordinateForm";
            this.Text = "Coordinates";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvCoordinates;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ContextMenuStrip menuDropDown;
        private System.Windows.Forms.Button btnMapPoints;
        private System.Windows.Forms.CheckBox chkShowOnMap;
    }
}