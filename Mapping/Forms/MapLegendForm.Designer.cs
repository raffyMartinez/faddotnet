namespace FAD3.Mapping.Forms
{
    partial class MapLegendForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapLegendForm));
            this.tsLegend = new System.Windows.Forms.ToolStrip();
            this.btnApplyLegend = new System.Windows.Forms.ToolStripButton();
            this.btnSelectPosition = new System.Windows.Forms.ToolStripDropDownButton();
            this.itemTopLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.itemTopRight = new System.Windows.Forms.ToolStripMenuItem();
            this.itemBottomRight = new System.Windows.Forms.ToolStripMenuItem();
            this.itemBottomLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.ilLegend = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabLegend = new System.Windows.Forms.TabPage();
            this.tabConfigure = new System.Windows.Forms.TabPage();
            this.dgLegend = new System.Windows.Forms.DataGridView();
            this.tsLegend.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabLegend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLegend)).BeginInit();
            this.SuspendLayout();
            // 
            // tsLegend
            // 
            this.tsLegend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnApplyLegend,
            this.btnSelectPosition,
            this.btnClose});
            this.tsLegend.Location = new System.Drawing.Point(0, 0);
            this.tsLegend.Name = "tsLegend";
            this.tsLegend.Size = new System.Drawing.Size(260, 25);
            this.tsLegend.TabIndex = 1;
            this.tsLegend.Text = "toolStrip1";
            this.tsLegend.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolBarItemClick);
            // 
            // btnApplyLegend
            // 
            this.btnApplyLegend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnApplyLegend.Image = global::FAD3.Properties.Resources.checkLegend2;
            this.btnApplyLegend.ImageTransparentColor = System.Drawing.Color.White;
            this.btnApplyLegend.Name = "btnApplyLegend";
            this.btnApplyLegend.Size = new System.Drawing.Size(23, 22);
            this.btnApplyLegend.Text = "toolStripButton1";
            // 
            // btnSelectPosition
            // 
            this.btnSelectPosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSelectPosition.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemTopLeft,
            this.itemTopRight,
            this.itemBottomRight,
            this.itemBottomLeft});
            this.btnSelectPosition.Image = global::FAD3.Properties.Resources.choosePosition;
            this.btnSelectPosition.ImageTransparentColor = System.Drawing.Color.White;
            this.btnSelectPosition.Name = "btnSelectPosition";
            this.btnSelectPosition.Size = new System.Drawing.Size(29, 22);
            this.btnSelectPosition.Text = "toolStripDropDownButton1";
            this.btnSelectPosition.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnSelectPositionDropdownClicked);
            // 
            // itemTopLeft
            // 
            this.itemTopLeft.Checked = true;
            this.itemTopLeft.CheckState = System.Windows.Forms.CheckState.Checked;
            this.itemTopLeft.Name = "itemTopLeft";
            this.itemTopLeft.Size = new System.Drawing.Size(142, 22);
            this.itemTopLeft.Text = "Top left";
            // 
            // itemTopRight
            // 
            this.itemTopRight.Name = "itemTopRight";
            this.itemTopRight.Size = new System.Drawing.Size(142, 22);
            this.itemTopRight.Text = "Top right";
            // 
            // itemBottomRight
            // 
            this.itemBottomRight.Name = "itemBottomRight";
            this.itemBottomRight.Size = new System.Drawing.Size(142, 22);
            this.itemBottomRight.Text = "Bottom right";
            // 
            // itemBottomLeft
            // 
            this.itemBottomLeft.Name = "itemBottomLeft";
            this.itemBottomLeft.Size = new System.Drawing.Size(142, 22);
            this.itemBottomLeft.Text = "Bottom left";
            // 
            // btnClose
            // 
            this.btnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClose.Image = global::FAD3.Properties.Resources.im_exit;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(23, 22);
            this.btnClose.Text = "toolStripButton1";
            // 
            // ilLegend
            // 
            this.ilLegend.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilLegend.ImageStream")));
            this.ilLegend.TransparentColor = System.Drawing.Color.Transparent;
            this.ilLegend.Images.SetKeyName(0, "checkLegend");
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabLegend);
            this.tabControl1.Controls.Add(this.tabConfigure);
            this.tabControl1.Location = new System.Drawing.Point(0, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(260, 367);
            this.tabControl1.TabIndex = 2;
            // 
            // tabLegend
            // 
            this.tabLegend.Controls.Add(this.dgLegend);
            this.tabLegend.Location = new System.Drawing.Point(4, 22);
            this.tabLegend.Name = "tabLegend";
            this.tabLegend.Padding = new System.Windows.Forms.Padding(3);
            this.tabLegend.Size = new System.Drawing.Size(252, 341);
            this.tabLegend.TabIndex = 0;
            this.tabLegend.Text = "Legend";
            this.tabLegend.UseVisualStyleBackColor = true;
            // 
            // tabConfigure
            // 
            this.tabConfigure.Location = new System.Drawing.Point(4, 22);
            this.tabConfigure.Name = "tabConfigure";
            this.tabConfigure.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfigure.Size = new System.Drawing.Size(252, 341);
            this.tabConfigure.TabIndex = 1;
            this.tabConfigure.Text = "Setup";
            this.tabConfigure.UseVisualStyleBackColor = true;
            // 
            // dgLegend
            // 
            this.dgLegend.AllowDrop = true;
            this.dgLegend.AllowUserToAddRows = false;
            this.dgLegend.AllowUserToDeleteRows = false;
            this.dgLegend.AllowUserToOrderColumns = true;
            this.dgLegend.AllowUserToResizeColumns = false;
            this.dgLegend.AllowUserToResizeRows = false;
            this.dgLegend.BackgroundColor = System.Drawing.Color.White;
            this.dgLegend.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgLegend.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgLegend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgLegend.ColumnHeadersVisible = false;
            this.dgLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgLegend.Location = new System.Drawing.Point(3, 3);
            this.dgLegend.Name = "dgLegend";
            this.dgLegend.RowHeadersVisible = false;
            this.dgLegend.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgLegend.Size = new System.Drawing.Size(246, 335);
            this.dgLegend.TabIndex = 1;
            this.dgLegend.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellClick);
            // 
            // MapLegendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 395);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tsLegend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MapLegendForm";
            this.Text = "Legend";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.tsLegend.ResumeLayout(false);
            this.tsLegend.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabLegend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgLegend)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip tsLegend;
        private System.Windows.Forms.ToolStripButton btnApplyLegend;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.ImageList ilLegend;
        private System.Windows.Forms.ToolStripDropDownButton btnSelectPosition;
        private System.Windows.Forms.ToolStripMenuItem itemTopLeft;
        private System.Windows.Forms.ToolStripMenuItem itemTopRight;
        private System.Windows.Forms.ToolStripMenuItem itemBottomRight;
        private System.Windows.Forms.ToolStripMenuItem itemBottomLeft;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLegend;
        private System.Windows.Forms.DataGridView dgLegend;
        private System.Windows.Forms.TabPage tabConfigure;
    }
}