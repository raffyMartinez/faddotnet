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
            this.itemOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.itemConvertToGrid25 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.buttonAddLayer = new System.Windows.Forms.ToolStripButton();
            this.buttonRemoveLayer = new System.Windows.Forms.ToolStripButton();
            this.buttonAttributes = new System.Windows.Forms.ToolStripButton();
            this.buttonZoomToLayer = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.layerGrid)).BeginInit();
            this.menuLayers.SuspendLayout();
            this.toolBar.SuspendLayout();
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
            this.itemLayerExport,
            this.itemOptions});
            this.menuLayers.Name = "menuLayers";
            this.menuLayers.Size = new System.Drawing.Size(146, 114);
            this.menuLayers.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnMenuLayers_ItemClicked);
            // 
            // itemAddLayer
            // 
            this.itemAddLayer.Name = "itemAddLayer";
            this.itemAddLayer.Size = new System.Drawing.Size(145, 22);
            this.itemAddLayer.Text = "Add layer...";
            // 
            // itemRemoveLayer
            // 
            this.itemRemoveLayer.Name = "itemRemoveLayer";
            this.itemRemoveLayer.Size = new System.Drawing.Size(145, 22);
            this.itemRemoveLayer.Text = "Remove layer";
            // 
            // itemLayerProperty
            // 
            this.itemLayerProperty.Name = "itemLayerProperty";
            this.itemLayerProperty.Size = new System.Drawing.Size(145, 22);
            this.itemLayerProperty.Text = "Properties...";
            // 
            // itemLayerExport
            // 
            this.itemLayerExport.Name = "itemLayerExport";
            this.itemLayerExport.Size = new System.Drawing.Size(145, 22);
            this.itemLayerExport.Text = "Export...";
            // 
            // itemOptions
            // 
            this.itemOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemConvertToGrid25});
            this.itemOptions.Name = "itemOptions";
            this.itemOptions.Size = new System.Drawing.Size(145, 22);
            this.itemOptions.Text = "Options";
            this.itemOptions.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnoptionsToolStripMenuItem_DropDownItemClicked);
            // 
            // itemConvertToGrid25
            // 
            this.itemConvertToGrid25.Name = "itemConvertToGrid25";
            this.itemConvertToGrid25.Size = new System.Drawing.Size(167, 22);
            this.itemConvertToGrid25.Text = "Convert to Grid25";
            // 
            // toolBar
            // 
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonAddLayer,
            this.buttonRemoveLayer,
            this.buttonAttributes,
            this.buttonZoomToLayer});
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(334, 25);
            this.toolBar.TabIndex = 2;
            this.toolBar.Text = "toolStrip1";
            this.toolBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolbarItemClick);
            // 
            // buttonAddLayer
            // 
            this.buttonAddLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonAddLayer.Image = global::FAD3.Properties.Resources.layerAdd;
            this.buttonAddLayer.ImageTransparentColor = System.Drawing.Color.White;
            this.buttonAddLayer.Name = "buttonAddLayer";
            this.buttonAddLayer.Size = new System.Drawing.Size(23, 22);
            this.buttonAddLayer.Text = "toolStripButton1";
            // 
            // buttonRemoveLayer
            // 
            this.buttonRemoveLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonRemoveLayer.Image = global::FAD3.Properties.Resources.layerRemove;
            this.buttonRemoveLayer.ImageTransparentColor = System.Drawing.Color.White;
            this.buttonRemoveLayer.Name = "buttonRemoveLayer";
            this.buttonRemoveLayer.Size = new System.Drawing.Size(23, 22);
            this.buttonRemoveLayer.Text = "toolStripButton2";
            // 
            // buttonAttributes
            // 
            this.buttonAttributes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonAttributes.Image = global::FAD3.Properties.Resources.attrib;
            this.buttonAttributes.ImageTransparentColor = System.Drawing.Color.White;
            this.buttonAttributes.Name = "buttonAttributes";
            this.buttonAttributes.Size = new System.Drawing.Size(23, 22);
            this.buttonAttributes.Text = "toolStripButton3";
            // 
            // buttonZoomToLayer
            // 
            this.buttonZoomToLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonZoomToLayer.Image = global::FAD3.Properties.Resources.zoomSelection;
            this.buttonZoomToLayer.ImageTransparentColor = System.Drawing.Color.White;
            this.buttonZoomToLayer.Name = "buttonZoomToLayer";
            this.buttonZoomToLayer.Size = new System.Drawing.Size(23, 22);
            this.buttonZoomToLayer.Text = "toolStripButton4";
            // 
            // MapLayersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 366);
            this.Controls.Add(this.toolBar);
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
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.ToolStripMenuItem itemOptions;
        private System.Windows.Forms.ToolStripMenuItem itemConvertToGrid25;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton buttonAddLayer;
        private System.Windows.Forms.ToolStripButton buttonRemoveLayer;
        private System.Windows.Forms.ToolStripButton buttonAttributes;
        private System.Windows.Forms.ToolStripButton buttonZoomToLayer;
    }
}