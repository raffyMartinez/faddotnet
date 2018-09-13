namespace FAD3
{
    partial class MapperForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapperForm));
            this.axMap = new AxMapWinGIS.AxMap();
            this.ilCursors = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuDropDown = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolstripToolBar = new ToolStripExtensions.ToolStripEx();
            this.tsButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
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
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsButtonClearSelection = new System.Windows.Forms.ToolStripButton();
            this.tsButtonClearAllSelection = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsButtonGraticule = new System.Windows.Forms.ToolStripButton();
            this.tsButtonSaveImage = new System.Windows.Forms.ToolStripButton();
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
            this.axMap.Margin = new System.Windows.Forms.Padding(4);
            this.axMap.Name = "axMap";
            this.axMap.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMap.OcxState")));
            this.axMap.Size = new System.Drawing.Size(1067, 243);
            this.axMap.TabIndex = 0;
            // 
            // ilCursors
            // 
            this.ilCursors.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCursors.ImageStream")));
            this.ilCursors.TransparentColor = System.Drawing.Color.White;
            this.ilCursors.Images.SetKeyName(0, "arrow32");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 275);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1067, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuDropDown
            // 
            this.menuDropDown.Name = "menuDropDown";
            this.menuDropDown.Size = new System.Drawing.Size(61, 4);
            // 
            // toolstripToolBar
            // 
            this.toolstripToolBar.ClickThrough = false;
            this.toolstripToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButtonSave,
            this.toolStripSeparator2,
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
            this.tsButtonMeasure,
            this.toolStripSeparator1,
            this.tsButtonClearSelection,
            this.tsButtonClearAllSelection,
            this.toolStripSeparator3,
            this.tsButtonGraticule,
            this.tsButtonSaveImage});
            this.toolstripToolBar.Location = new System.Drawing.Point(0, 0);
            this.toolstripToolBar.Name = "toolstripToolBar";
            this.toolstripToolBar.Size = new System.Drawing.Size(1067, 25);
            this.toolstripToolBar.SuppressHighlighting = true;
            this.toolstripToolBar.TabIndex = 1;
            this.toolstripToolBar.Text = "toolStripEx1";
            this.toolstripToolBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolbarClicked);
            // 
            // tsButtonSave
            // 
            this.tsButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonSave.Image = global::FAD3.Properties.Resources.document_save;
            this.tsButtonSave.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonSave.Name = "tsButtonSave";
            this.tsButtonSave.Size = new System.Drawing.Size(23, 22);
            this.tsButtonSave.Text = "toolStripButton1";
            this.tsButtonSave.ToolTipText = "Save map state";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            this.tsButtonAttributes.Text = "toolStripButton3";
            this.tsButtonAttributes.ToolTipText = "View layer attributes";
            // 
            // tsButtonZoomIn
            // 
            this.tsButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonZoomIn.Image = global::FAD3.Properties.Resources.zoom_plus;
            this.tsButtonZoomIn.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonZoomIn.Name = "tsButtonZoomIn";
            this.tsButtonZoomIn.Size = new System.Drawing.Size(23, 22);
            this.tsButtonZoomIn.Text = "toolStripButton4";
            this.tsButtonZoomIn.ToolTipText = "Zoom in";
            // 
            // tsButtonZoomOut
            // 
            this.tsButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonZoomOut.Image = global::FAD3.Properties.Resources.zoom_minus;
            this.tsButtonZoomOut.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonZoomOut.Name = "tsButtonZoomOut";
            this.tsButtonZoomOut.Size = new System.Drawing.Size(23, 22);
            this.tsButtonZoomOut.Text = "toolStripButton5";
            this.tsButtonZoomOut.ToolTipText = "Zoom out";
            // 
            // tsButtonZoomAll
            // 
            this.tsButtonZoomAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonZoomAll.Image = global::FAD3.Properties.Resources.zoomEntire;
            this.tsButtonZoomAll.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonZoomAll.Name = "tsButtonZoomAll";
            this.tsButtonZoomAll.Size = new System.Drawing.Size(23, 22);
            this.tsButtonZoomAll.Text = "toolStripButton6";
            this.tsButtonZoomAll.ToolTipText = "Zoom all";
            // 
            // tsButtonFitMap
            // 
            this.tsButtonFitMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonFitMap.Image = global::FAD3.Properties.Resources.fitScreen;
            this.tsButtonFitMap.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonFitMap.Name = "tsButtonFitMap";
            this.tsButtonFitMap.Size = new System.Drawing.Size(23, 22);
            this.tsButtonFitMap.Text = "toolStripButton7";
            this.tsButtonFitMap.ToolTipText = "Fit map to window";
            // 
            // tsButtonZoomPrevious
            // 
            this.tsButtonZoomPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonZoomPrevious.Image = global::FAD3.Properties.Resources.imZoomPrev;
            this.tsButtonZoomPrevious.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonZoomPrevious.Name = "tsButtonZoomPrevious";
            this.tsButtonZoomPrevious.Size = new System.Drawing.Size(23, 22);
            this.tsButtonZoomPrevious.Text = "toolStripButton8";
            this.tsButtonZoomPrevious.ToolTipText = "Previous zoom";
            // 
            // tsButtonPan
            // 
            this.tsButtonPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonPan.Image = global::FAD3.Properties.Resources.pan;
            this.tsButtonPan.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonPan.Name = "tsButtonPan";
            this.tsButtonPan.Size = new System.Drawing.Size(23, 22);
            this.tsButtonPan.Text = "toolStripButton9";
            this.tsButtonPan.ToolTipText = "Pan";
            // 
            // tsButtonBlackArrow
            // 
            this.tsButtonBlackArrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonBlackArrow.Image = global::FAD3.Properties.Resources.arrow;
            this.tsButtonBlackArrow.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonBlackArrow.Name = "tsButtonBlackArrow";
            this.tsButtonBlackArrow.Size = new System.Drawing.Size(23, 22);
            this.tsButtonBlackArrow.Text = "toolStripButton10";
            this.tsButtonBlackArrow.ToolTipText = "Select";
            // 
            // tsButtonMeasure
            // 
            this.tsButtonMeasure.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonMeasure.Image = global::FAD3.Properties.Resources.ruler;
            this.tsButtonMeasure.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonMeasure.Name = "tsButtonMeasure";
            this.tsButtonMeasure.Size = new System.Drawing.Size(23, 22);
            this.tsButtonMeasure.Text = "toolStripButton11";
            this.tsButtonMeasure.ToolTipText = "Measure";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsButtonClearSelection
            // 
            this.tsButtonClearSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonClearSelection.Image = global::FAD3.Properties.Resources.clear_selection;
            this.tsButtonClearSelection.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonClearSelection.Name = "tsButtonClearSelection";
            this.tsButtonClearSelection.Size = new System.Drawing.Size(23, 22);
            this.tsButtonClearSelection.Text = "toolStripButton1";
            this.tsButtonClearSelection.ToolTipText = "Clear selection";
            // 
            // tsButtonClearAllSelection
            // 
            this.tsButtonClearAllSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonClearAllSelection.Image = global::FAD3.Properties.Resources.clear_all_selection;
            this.tsButtonClearAllSelection.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonClearAllSelection.Name = "tsButtonClearAllSelection";
            this.tsButtonClearAllSelection.Size = new System.Drawing.Size(23, 22);
            this.tsButtonClearAllSelection.Text = "toolStripButton1";
            this.tsButtonClearAllSelection.ToolTipText = "Clears selection from all layers";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsButtonGraticule
            // 
            this.tsButtonGraticule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonGraticule.Image = global::FAD3.Properties.Resources.graticule;
            this.tsButtonGraticule.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonGraticule.Name = "tsButtonGraticule";
            this.tsButtonGraticule.Size = new System.Drawing.Size(23, 22);
            this.tsButtonGraticule.Tag = "Setup graticule";
            this.tsButtonGraticule.Text = "toolStripButton1";
            // 
            // tsButtonSaveImage
            // 
            this.tsButtonSaveImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonSaveImage.Image = global::FAD3.Properties.Resources.image;
            this.tsButtonSaveImage.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonSaveImage.Name = "tsButtonSaveImage";
            this.tsButtonSaveImage.Size = new System.Drawing.Size(23, 22);
            this.tsButtonSaveImage.Text = "Save map as image";
            // 
            // MapperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 297);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolstripToolBar);
            this.Controls.Add(this.axMap);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MapperForm";
            this.Text = "MapperForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnMapperForm_Closed);
            this.Load += new System.EventHandler(this.OnMapperForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axMap)).EndInit();
            this.toolstripToolBar.ResumeLayout(false);
            this.toolstripToolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxMapWinGIS.AxMap axMap;
        private ToolStripExtensions.ToolStripEx toolstripToolBar;
        private System.Windows.Forms.ToolStripButton tsButtonLayers;
        private System.Windows.Forms.ImageList ilCursors;
        private System.Windows.Forms.ToolStripButton tsButtonLayerAdd;
        private System.Windows.Forms.ToolStripButton tsButtonAttributes;
        private System.Windows.Forms.ToolStripButton tsButtonZoomIn;
        private System.Windows.Forms.ToolStripButton tsButtonZoomOut;
        private System.Windows.Forms.ToolStripButton tsButtonZoomAll;
        private System.Windows.Forms.ToolStripButton tsButtonFitMap;
        private System.Windows.Forms.ToolStripButton tsButtonZoomPrevious;
        private System.Windows.Forms.ToolStripButton tsButtonPan;
        private System.Windows.Forms.ToolStripButton tsButtonBlackArrow;
        private System.Windows.Forms.ToolStripButton tsButtonMeasure;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton tsButtonClearSelection;
        private System.Windows.Forms.ToolStripButton tsButtonClearAllSelection;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsButtonSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip menuDropDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsButtonGraticule;
        private System.Windows.Forms.ToolStripButton tsButtonSaveImage;
    }
}