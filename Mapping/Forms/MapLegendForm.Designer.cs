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
            this.dgLegend = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgLegend)).BeginInit();
            this.SuspendLayout();
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
            this.dgLegend.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgLegend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgLegend.ColumnHeadersVisible = false;
            this.dgLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgLegend.Location = new System.Drawing.Point(0, 0);
            this.dgLegend.Name = "dgLegend";
            this.dgLegend.RowHeadersVisible = false;
            this.dgLegend.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgLegend.Size = new System.Drawing.Size(260, 395);
            this.dgLegend.TabIndex = 0;
            this.dgLegend.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellClick);
            // 
            // MapLegendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 395);
            this.Controls.Add(this.dgLegend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MapLegendForm";
            this.Text = "Legend";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.dgLegend)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgLegend;
    }
}