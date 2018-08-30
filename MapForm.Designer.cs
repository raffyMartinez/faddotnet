namespace FAD3
{
    partial class MapForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapForm));
            this.axMap = new AxMapWinGIS.AxMap();
            this.toolstripToolBar = new System.Windows.Forms.ToolStrip();
            this.ilCursors = new System.Windows.Forms.ImageList(this.components);
            this.tsButtonLayers = new System.Windows.Forms.ToolStripButton();
            this.tsButtonLayerAdd = new System.Windows.Forms.ToolStripButton();
            this.tsButtonAttributes = new System.Windows.Forms.ToolStripButton();
            this.tsButtonZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tsButtonZoomOut = new System.Windows.Forms.ToolStripButton();
            this.tsButtonZoomAll = new System.Windows.Forms.ToolStripButton();
            this.tsButtonFitMap = new System.Windows.Forms.ToolStripButton();
            this.tsButtonZoomPrevious = new System.Windows.Forms.ToolStripButton();
            this.tsButtonPan = new System.Windows.Forms.ToolStripButton();
            this.tsButtonBlackArrow = new System.Windows.Forms.ToolStripButton();
            this.tsButtonMeasure = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.axMap)).BeginInit();
            this.toolstripToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // axMap
            // 
            this.axMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axMap.Enabled = true;
            this.axMap.Location = new System.Drawing.Point(0, 28);
            this.axMap.Name = "axMap";
            this.axMap.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMap.OcxState")));
            this.axMap.Size = new System.Drawing.Size(629, 297);
            this.axMap.TabIndex = 0;
            // 
            // toolstripToolBar
            // 
            this.toolstripToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButtonLayers,
            this.tsButtonLayerAdd,
            this.tsButtonAttributes,
            this.tsButtonZoomIn,
            this.tsButtonZoomOut,
            this.tsButtonZoomAll,
            this.tsButtonFitMap,
            this.tsButtonZoomPrevious,
            this.tsButtonPan,
            this.tsButtonBlackArrow,
            this.tsButtonMeasure});
            this.toolstripToolBar.Location = new System.Drawing.Point(0, 0);
            this.toolstripToolBar.Name = "toolstripToolBar";
            this.toolstripToolBar.Size = new System.Drawing.Size(629, 25);
            this.toolstripToolBar.TabIndex = 1;
            this.toolstripToolBar.Text = "toolStrip1";
            this.toolstripToolBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolbarClicked);
            // 
            // ilCursors
            // 
            this.ilCursors.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCursors.ImageStream")));
            this.ilCursors.TransparentColor = System.Drawing.Color.White;
            this.ilCursors.Images.SetKeyName(0, "arrow32");
            // 
            // tsButtonLayers
            // 
            this.tsButtonLayers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonLayers.Image = global::FAD3.Properties.Resources.layer;
            this.tsButtonLayers.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonLayers.Name = "tsButtonLayers";
            this.tsButtonLayers.Size = new System.Drawing.Size(23, 22);
            this.tsButtonLayers.Text = "toolStripButton1";
            this.tsButtonLayers.ToolTipText = "Layers";
            // 
            // tsButtonLayerAdd
            // 
            this.tsButtonLayerAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonLayerAdd.Image = global::FAD3.Properties.Resources.layerAdd;
            this.tsButtonLayerAdd.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonLayerAdd.Name = "tsButtonLayerAdd";
            this.tsButtonLayerAdd.Size = new System.Drawing.Size(23, 22);
            this.tsButtonLayerAdd.Text = "toolStripButton2";
            this.tsButtonLayerAdd.ToolTipText = "Add layer";
            // 
            // tsButtonAttributes
            // 
            this.tsButtonAttributes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonAttributes.Image = global::FAD3.Properties.Resources.attrib;
            this.tsButtonAttributes.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonAttributes.Name = "tsButtonAttributes";
            this.tsButtonAttributes.Size = new System.Drawing.Size(23, 22);
            this.tsButtonAttributes.Text = "toolStripButton1";
            this.tsButtonAttributes.ToolTipText = "View layer attributes";
            // 
            // tsButtonZoomIn
            // 
            this.tsButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonZoomIn.Image = global::FAD3.Properties.Resources.zoom_plus;
            this.tsButtonZoomIn.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonZoomIn.Name = "tsButtonZoomIn";
            this.tsButtonZoomIn.Size = new System.Drawing.Size(23, 22);
            this.tsButtonZoomIn.Text = "toolStripButton2";
            this.tsButtonZoomIn.ToolTipText = "Zoom in";
            // 
            // tsButtonZoomOut
            // 
            this.tsButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonZoomOut.Image = global::FAD3.Properties.Resources.zoom_minus;
            this.tsButtonZoomOut.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonZoomOut.Name = "tsButtonZoomOut";
            this.tsButtonZoomOut.Size = new System.Drawing.Size(23, 22);
            this.tsButtonZoomOut.Text = "toolStripButton3";
            this.tsButtonZoomOut.ToolTipText = "Zoom out";
            // 
            // tsButtonZoomAll
            // 
            this.tsButtonZoomAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonZoomAll.Image = global::FAD3.Properties.Resources.zoomEntire;
            this.tsButtonZoomAll.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonZoomAll.Name = "tsButtonZoomAll";
            this.tsButtonZoomAll.Size = new System.Drawing.Size(23, 22);
            this.tsButtonZoomAll.Text = "tsButtonZoomAll";
            this.tsButtonZoomAll.ToolTipText = "Zoom all";
            // 
            // tsButtonFitMap
            // 
            this.tsButtonFitMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonFitMap.Image = global::FAD3.Properties.Resources.fitScreen;
            this.tsButtonFitMap.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonFitMap.Name = "tsButtonFitMap";
            this.tsButtonFitMap.Size = new System.Drawing.Size(23, 22);
            this.tsButtonFitMap.Text = "tsButtonFitMap";
            this.tsButtonFitMap.ToolTipText = "Fit map to window";
            // 
            // tsButtonZoomPrevious
            // 
            this.tsButtonZoomPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonZoomPrevious.Image = global::FAD3.Properties.Resources.imZoomPrev;
            this.tsButtonZoomPrevious.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonZoomPrevious.Name = "tsButtonZoomPrevious";
            this.tsButtonZoomPrevious.Size = new System.Drawing.Size(23, 22);
            this.tsButtonZoomPrevious.Text = "tsButtonZoomPrevious";
            this.tsButtonZoomPrevious.ToolTipText = "Previous zoom";
            // 
            // tsButtonPan
            // 
            this.tsButtonPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonPan.Image = global::FAD3.Properties.Resources.pan;
            this.tsButtonPan.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonPan.Name = "tsButtonPan";
            this.tsButtonPan.Size = new System.Drawing.Size(23, 22);
            this.tsButtonPan.Text = "tsButtonPan";
            this.tsButtonPan.ToolTipText = "Pan";
            // 
            // tsButtonBlackArrow
            // 
            this.tsButtonBlackArrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonBlackArrow.Image = global::FAD3.Properties.Resources.arrow;
            this.tsButtonBlackArrow.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonBlackArrow.Name = "tsButtonBlackArrow";
            this.tsButtonBlackArrow.Size = new System.Drawing.Size(23, 22);
            this.tsButtonBlackArrow.Text = "tsButtonBlackArrow";
            this.tsButtonBlackArrow.ToolTipText = "Select";
            // 
            // tsButtonMeasure
            // 
            this.tsButtonMeasure.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonMeasure.Image = global::FAD3.Properties.Resources.ruler;
            this.tsButtonMeasure.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonMeasure.Name = "tsButtonMeasure";
            this.tsButtonMeasure.Size = new System.Drawing.Size(23, 22);
            this.tsButtonMeasure.Text = "toolStripButton1";
            this.tsButtonMeasure.ToolTipText = "Measure";
            // 
            // MapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 327);
            this.Controls.Add(this.toolstripToolBar);
            this.Controls.Add(this.axMap);
            this.Name = "MapForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMap_FormClosed);
            this.Load += new System.EventHandler(this.frmMap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axMap)).EndInit();
            this.toolstripToolBar.ResumeLayout(false);
            this.toolstripToolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxMapWinGIS.AxMap axMap;
        private System.Windows.Forms.ToolStrip toolstripToolBar;
        private System.Windows.Forms.ToolStripButton tsButtonLayers;
        private System.Windows.Forms.ToolStripButton tsButtonLayerAdd;
        private System.Windows.Forms.ToolStripButton tsButtonAttributes;
        private System.Windows.Forms.ToolStripButton tsButtonZoomIn;
        private System.Windows.Forms.ToolStripButton tsButtonZoomOut;
        private System.Windows.Forms.ToolStripButton tsButtonZoomAll;
        private System.Windows.Forms.ToolStripButton tsButtonFitMap;
        private System.Windows.Forms.ToolStripButton tsButtonZoomPrevious;
        private System.Windows.Forms.ToolStripButton tsButtonPan;
        private System.Windows.Forms.ToolStripButton tsButtonBlackArrow;
        private System.Windows.Forms.ImageList ilCursors;
        private System.Windows.Forms.ToolStripButton tsButtonMeasure;
    }
}