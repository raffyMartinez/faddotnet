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
            this.xMap = new AxMapWinGIS.AxMap();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsButtonLayers = new System.Windows.Forms.ToolStripButton();
            this.tsButtonLayerAdd = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.xMap)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xMap
            // 
            this.xMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xMap.Enabled = true;
            this.xMap.Location = new System.Drawing.Point(0, 28);
            this.xMap.Name = "xMap";
            this.xMap.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("xMap.OcxState")));
            this.xMap.Size = new System.Drawing.Size(629, 297);
            this.xMap.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsButtonLayers,
            this.tsButtonLayerAdd});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(629, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
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
            // frmMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 327);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.xMap);
            this.Name = "frmMap";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMap_FormClosed);
            this.Load += new System.EventHandler(this.frmMap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xMap)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxMapWinGIS.AxMap xMap;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsButtonLayers;
        private System.Windows.Forms.ToolStripButton tsButtonLayerAdd;
    }
}