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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapForm));
            this.axMap = new AxMapWinGIS.AxMap();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.tsButtonLayers = new System.Windows.Forms.ToolStripButton();
            this.tsButtonLayerAdd = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.axMap)).BeginInit();
            this.toolBar.SuspendLayout();
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
            // toolBar
            // 
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButtonLayers,
            this.tsButtonLayerAdd});
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(629, 25);
            this.toolBar.TabIndex = 1;
            this.toolBar.Text = "toolStrip1";
            this.toolBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolbarClicked);
            // 
            // tsButtonLayers
            // 
            this.tsButtonLayers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonLayers.Image = global::FAD3.Properties.Resources.layer;
            this.tsButtonLayers.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonLayers.Name = "tsButtonLayers";
            this.tsButtonLayers.Size = new System.Drawing.Size(23, 22);
            this.tsButtonLayers.Text = "toolStripButton1";
            // 
            // tsButtonLayerAdd
            // 
            this.tsButtonLayerAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsButtonLayerAdd.Image = global::FAD3.Properties.Resources.layerAdd;
            this.tsButtonLayerAdd.ImageTransparentColor = System.Drawing.Color.White;
            this.tsButtonLayerAdd.Name = "tsButtonLayerAdd";
            this.tsButtonLayerAdd.Size = new System.Drawing.Size(23, 22);
            this.tsButtonLayerAdd.Text = "toolStripButton2";
            // 
            // MapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 327);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.axMap);
            this.Name = "MapForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMap_FormClosed);
            this.Load += new System.EventHandler(this.frmMap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axMap)).EndInit();
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxMapWinGIS.AxMap axMap;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton tsButtonLayers;
        private System.Windows.Forms.ToolStripButton tsButtonLayerAdd;
    }
}