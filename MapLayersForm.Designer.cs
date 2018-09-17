namespace FAD3
{
    partial class MapLayersForm
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
            this.layerGrid = new System.Windows.Forms.DataGridView();
            this.Visible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Layer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Symbol = new System.Windows.Forms.DataGridViewImageColumn();
            this.buttonOK = new System.Windows.Forms.Button();
            this.menuLayers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemAddLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRemoveLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.itemLayerProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.itemLayerExport = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.layerGrid)).BeginInit();
            this.menuLayers.SuspendLayout();
            this.SuspendLayout();
            // 
            // layerGrid
            // 
            this.layerGrid.AllowDrop = true;
            this.layerGrid.AllowUserToAddRows = false;
            this.layerGrid.AllowUserToDeleteRows = false;
            this.layerGrid.AllowUserToOrderColumns = true;
            this.layerGrid.AllowUserToResizeColumns = false;
            this.layerGrid.AllowUserToResizeRows = false;
            this.layerGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layerGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.layerGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.layerGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.layerGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.layerGrid.ColumnHeadersVisible = false;
            this.layerGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Visible,
            this.Layer,
            this.Symbol});
            this.layerGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.layerGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.layerGrid.Location = new System.Drawing.Point(0, 37);
            this.layerGrid.Name = "layerGrid";
            this.layerGrid.RowHeadersVisible = false;
            this.layerGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.layerGrid.Size = new System.Drawing.Size(331, 281);
            this.layerGrid.TabIndex = 0;
            // 
            // Visible
            // 
            this.Visible.HeaderText = "V";
            this.Visible.Name = "Visible";
            this.Visible.Width = 20;
            // 
            // Layer
            // 
            this.Layer.HeaderText = "Name";
            this.Layer.Name = "Layer";
            this.Layer.Width = 228;
            // 
            // Symbol
            // 
            this.Symbol.HeaderText = "Symbol";
            this.Symbol.Name = "Symbol";
            this.Symbol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Symbol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Symbol.Width = 80;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(271, 329);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(51, 25);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // menuLayers
            // 
            this.menuLayers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemAddLayer,
            this.itemRemoveLayer,
            this.itemLayerProperty,
            this.itemLayerExport});
            this.menuLayers.Name = "menuLayers";
            this.menuLayers.Size = new System.Drawing.Size(181, 114);
            this.menuLayers.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnMenuLayers_ItemClicked);
            // 
            // itemAddLayer
            // 
            this.itemAddLayer.Name = "itemAddLayer";
            this.itemAddLayer.Size = new System.Drawing.Size(180, 22);
            this.itemAddLayer.Text = "Add layer...";
            // 
            // itemRemoveLayer
            // 
            this.itemRemoveLayer.Name = "itemRemoveLayer";
            this.itemRemoveLayer.Size = new System.Drawing.Size(180, 22);
            this.itemRemoveLayer.Text = "Remove layer";
            // 
            // itemLayerProperty
            // 
            this.itemLayerProperty.Name = "itemLayerProperty";
            this.itemLayerProperty.Size = new System.Drawing.Size(180, 22);
            this.itemLayerProperty.Text = "Properties...";
            // 
            // itemLayerExport
            // 
            this.itemLayerExport.Name = "itemLayerExport";
            this.itemLayerExport.Size = new System.Drawing.Size(180, 22);
            this.itemLayerExport.Text = "Export...";
            // 
            // MapLayersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 366);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.layerGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MapLayersForm";
            this.Text = "MapLayersForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MapLayersForm_FormClosed);
            this.Load += new System.EventHandler(this.OnMapLayersForm_Load);
            this.Resize += new System.EventHandler(this.MapLayersForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.layerGrid)).EndInit();
            this.menuLayers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView layerGrid;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Visible;
        private System.Windows.Forms.DataGridViewTextBoxColumn Layer;
        private System.Windows.Forms.DataGridViewImageColumn Symbol;
        private System.Windows.Forms.ContextMenuStrip menuLayers;
        private System.Windows.Forms.ToolStripMenuItem itemAddLayer;
        private System.Windows.Forms.ToolStripMenuItem itemRemoveLayer;
        private System.Windows.Forms.ToolStripMenuItem itemLayerProperty;
        private System.Windows.Forms.ToolStripMenuItem itemLayerExport;
    }
}