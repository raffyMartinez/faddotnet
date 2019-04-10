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
            this.menuLayers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemAddLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRemoveLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.itemLayerProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.itemAttributes = new System.Windows.Forms.ToolStripMenuItem();
            this.itemLayerExport = new System.Windows.Forms.ToolStripMenuItem();
            this.itemOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.itemConvertToGrid25 = new System.Windows.Forms.ToolStripMenuItem();
            this.itemMoveLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.itemMoveTop = new System.Windows.Forms.ToolStripMenuItem();
            this.itemMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.itemMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.itemMoveBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.buttonAddLayer = new System.Windows.Forms.ToolStripButton();
            this.buttonRemoveLayer = new System.Windows.Forms.ToolStripButton();
            this.buttonAttributes = new System.Windows.Forms.ToolStripButton();
            this.buttonZoomToLayer = new System.Windows.Forms.ToolStripButton();
            this.buttonClose = new System.Windows.Forms.ToolStripButton();
            this.itemAlwaysKeepOnTop = new System.Windows.Forms.ToolStripMenuItem();
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
            this.layerGrid.Location = new System.Drawing.Point(0, 28);
            this.layerGrid.Name = "layerGrid";
            this.layerGrid.RowHeadersVisible = false;
            this.layerGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.layerGrid.Size = new System.Drawing.Size(331, 332);
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
            this.Symbol.Width = 50;
            // 
            // menuLayers
            // 
            this.menuLayers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemAddLayer,
            this.itemRemoveLayer,
            this.itemLayerProperty,
            this.itemAttributes,
            this.itemLayerExport,
            this.itemOptions,
            this.itemMoveLayer});
            this.menuLayers.Name = "menuLayers";
            this.menuLayers.Size = new System.Drawing.Size(181, 180);
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
            // itemAttributes
            // 
            this.itemAttributes.Name = "itemAttributes";
            this.itemAttributes.Size = new System.Drawing.Size(145, 22);
            this.itemAttributes.Text = "Attributes...";
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
            this.itemConvertToGrid25,
            this.itemAlwaysKeepOnTop});
            this.itemOptions.Name = "itemOptions";
            this.itemOptions.Size = new System.Drawing.Size(180, 22);
            this.itemOptions.Text = "Options";
            this.itemOptions.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnoptionsToolStripMenuItem_DropDownItemClicked);
            // 
            // itemConvertToGrid25
            // 
            this.itemConvertToGrid25.Name = "itemConvertToGrid25";
            this.itemConvertToGrid25.Size = new System.Drawing.Size(180, 22);
            this.itemConvertToGrid25.Text = "Convert to Grid25";
            // 
            // itemMoveLayer
            // 
            this.itemMoveLayer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemMoveTop,
            this.itemMoveUp,
            this.itemMoveDown,
            this.itemMoveBottom});
            this.itemMoveLayer.Name = "itemMoveLayer";
            this.itemMoveLayer.Size = new System.Drawing.Size(145, 22);
            this.itemMoveLayer.Text = "Move";
            this.itemMoveLayer.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnLayerMoveDropDownClick);
            // 
            // itemMoveTop
            // 
            this.itemMoveTop.Name = "itemMoveTop";
            this.itemMoveTop.Size = new System.Drawing.Size(161, 22);
            this.itemMoveTop.Text = "Move to top";
            // 
            // itemMoveUp
            // 
            this.itemMoveUp.Name = "itemMoveUp";
            this.itemMoveUp.Size = new System.Drawing.Size(161, 22);
            this.itemMoveUp.Text = "Move up";
            // 
            // itemMoveDown
            // 
            this.itemMoveDown.Name = "itemMoveDown";
            this.itemMoveDown.Size = new System.Drawing.Size(161, 22);
            this.itemMoveDown.Text = "Move down";
            // 
            // itemMoveBottom
            // 
            this.itemMoveBottom.Name = "itemMoveBottom";
            this.itemMoveBottom.Size = new System.Drawing.Size(161, 22);
            this.itemMoveBottom.Text = "Move to bottom";
            // 
            // toolBar
            // 
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonAddLayer,
            this.buttonRemoveLayer,
            this.buttonAttributes,
            this.buttonZoomToLayer,
            this.buttonClose});
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
            this.buttonAddLayer.Text = "Add layer";
            // 
            // buttonRemoveLayer
            // 
            this.buttonRemoveLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonRemoveLayer.Image = global::FAD3.Properties.Resources.layerRemove;
            this.buttonRemoveLayer.ImageTransparentColor = System.Drawing.Color.White;
            this.buttonRemoveLayer.Name = "buttonRemoveLayer";
            this.buttonRemoveLayer.Size = new System.Drawing.Size(23, 22);
            this.buttonRemoveLayer.Text = "Remove layer";
            // 
            // buttonAttributes
            // 
            this.buttonAttributes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonAttributes.Image = global::FAD3.Properties.Resources.attrib;
            this.buttonAttributes.ImageTransparentColor = System.Drawing.Color.White;
            this.buttonAttributes.Name = "buttonAttributes";
            this.buttonAttributes.Size = new System.Drawing.Size(23, 22);
            this.buttonAttributes.Text = "toolStripButton3";
            this.buttonAttributes.ToolTipText = "Layer attributes";
            // 
            // buttonZoomToLayer
            // 
            this.buttonZoomToLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonZoomToLayer.Image = global::FAD3.Properties.Resources.zoomSelection;
            this.buttonZoomToLayer.ImageTransparentColor = System.Drawing.Color.White;
            this.buttonZoomToLayer.Name = "buttonZoomToLayer";
            this.buttonZoomToLayer.Size = new System.Drawing.Size(23, 22);
            this.buttonZoomToLayer.Text = "toolStripButton4";
            this.buttonZoomToLayer.ToolTipText = "Zoom to layer";
            // 
            // buttonClose
            // 
            this.buttonClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonClose.Image = global::FAD3.Properties.Resources.im_exit;
            this.buttonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(23, 22);
            this.buttonClose.Text = "toolStripButton1";
            this.buttonClose.ToolTipText = "Close layers";
            // 
            // itemAlwaysKeepOnTop
            // 
            this.itemAlwaysKeepOnTop.Name = "itemAlwaysKeepOnTop";
            this.itemAlwaysKeepOnTop.Size = new System.Drawing.Size(180, 22);
            this.itemAlwaysKeepOnTop.Text = "Always keep on top";
            this.itemAlwaysKeepOnTop.CheckStateChanged += new System.EventHandler(this.onCheckStateChange);
            // 
            // MapLayersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 361);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.layerGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MapLayersForm";
            this.ShowInTaskbar = false;
            this.Text = "Layers";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
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
        private System.Windows.Forms.ToolStripButton buttonClose;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Visible;
        private System.Windows.Forms.DataGridViewTextBoxColumn Layer;
        private System.Windows.Forms.DataGridViewImageColumn Symbol;
        private System.Windows.Forms.ToolStripMenuItem itemMoveLayer;
        private System.Windows.Forms.ToolStripMenuItem itemMoveTop;
        private System.Windows.Forms.ToolStripMenuItem itemMoveUp;
        private System.Windows.Forms.ToolStripMenuItem itemMoveDown;
        private System.Windows.Forms.ToolStripMenuItem itemMoveBottom;
        private System.Windows.Forms.ToolStripMenuItem itemAttributes;
        private System.Windows.Forms.ToolStripMenuItem itemAlwaysKeepOnTop;
    }
}