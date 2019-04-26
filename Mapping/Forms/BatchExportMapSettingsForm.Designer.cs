namespace FAD3.Mapping.Forms
{
    partial class BatchExportMapSettingsForm
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
            this.dgSettings = new System.Windows.Forms.DataGridView();
            this.colLayer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShowFront = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colShowLabelFront = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colShowReverse = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colShowLabelsReverse = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dropdownSettings = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemSetupLabels = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgSettings)).BeginInit();
            this.dropdownSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgSettings
            // 
            this.dgSettings.AllowUserToAddRows = false;
            this.dgSettings.AllowUserToDeleteRows = false;
            this.dgSettings.AllowUserToResizeColumns = false;
            this.dgSettings.AllowUserToResizeRows = false;
            this.dgSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLayer,
            this.colShowFront,
            this.colShowLabelFront,
            this.colShowReverse,
            this.colShowLabelsReverse});
            this.dgSettings.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgSettings.Location = new System.Drawing.Point(3, 40);
            this.dgSettings.Name = "dgSettings";
            this.dgSettings.RowHeadersVisible = false;
            this.dgSettings.Size = new System.Drawing.Size(504, 265);
            this.dgSettings.TabIndex = 0;
            this.dgSettings.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellEnter);
            this.dgSettings.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnCellMouseDown);
            this.dgSettings.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnDatagridMouseUp);
            this.dgSettings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellValueChanged);
            // 
            // colLayer
            // 
            this.colLayer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLayer.HeaderText = "Layer";
            this.colLayer.Name = "colLayer";
            this.colLayer.Width = 58;
            // 
            // colShowFront
            // 
            this.colShowFront.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colShowFront.HeaderText = "Show in front";
            this.colShowFront.Name = "colShowFront";
            this.colShowFront.Width = 49;
            // 
            // colShowLabelFront
            // 
            this.colShowLabelFront.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colShowLabelFront.HeaderText = "Show labels in front";
            this.colShowLabelFront.Name = "colShowLabelFront";
            this.colShowLabelFront.Width = 76;
            // 
            // colShowReverse
            // 
            this.colShowReverse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colShowReverse.HeaderText = "Show in reverse";
            this.colShowReverse.Name = "colShowReverse";
            this.colShowReverse.Width = 80;
            // 
            // colShowLabelsReverse
            // 
            this.colShowLabelsReverse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colShowLabelsReverse.HeaderText = "Show labels in reverse";
            this.colShowLabelsReverse.Name = "colShowLabelsReverse";
            this.colShowLabelsReverse.Width = 76;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(466, 320);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(39, 32);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(407, 320);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(53, 32);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // dropdownSettings
            // 
            this.dropdownSettings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSetupLabels});
            this.dropdownSettings.Name = "dropdownSettings";
            this.dropdownSettings.Size = new System.Drawing.Size(219, 48);
            this.dropdownSettings.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnDropDownItemClicked);
            // 
            // menuItemSetupLabels
            // 
            this.menuItemSetupLabels.Name = "menuItemSetupLabels";
            this.menuItemSetupLabels.Size = new System.Drawing.Size(218, 22);
            this.menuItemSetupLabels.Text = "Setup labels on reverse side";
            // 
            // BatchExportMapSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 357);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dgSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BatchExportMapSettingsForm";
            this.Text = "Front and reverse grid map settings";
            this.Load += new System.EventHandler(this.OnFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.dgSettings)).EndInit();
            this.dropdownSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgSettings;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLayer;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colShowFront;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colShowLabelFront;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colShowReverse;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colShowLabelsReverse;
        private System.Windows.Forms.ContextMenuStrip dropdownSettings;
        private System.Windows.Forms.ToolStripMenuItem menuItemSetupLabels;
    }
}