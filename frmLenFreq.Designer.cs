namespace FAD3
{
    partial class frmLenFreq
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabLFGMS = new System.Windows.Forms.TabControl();
            this.tabLF = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lvLF = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabGMS = new System.Windows.Forms.TabPage();
            this.lvGMS = new System.Windows.Forms.ListView();
            this.buttonOk = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabLFGMS.SuspendLayout();
            this.tabLF.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tabGMS.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabLFGMS
            // 
            this.tabLFGMS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabLFGMS.Controls.Add(this.tabLF);
            this.tabLFGMS.Controls.Add(this.tabGMS);
            this.tabLFGMS.Location = new System.Drawing.Point(-2, 38);
            this.tabLFGMS.Name = "tabLFGMS";
            this.tabLFGMS.SelectedIndex = 0;
            this.tabLFGMS.Size = new System.Drawing.Size(404, 266);
            this.tabLFGMS.TabIndex = 0;
            this.tabLFGMS.SelectedIndexChanged += new System.EventHandler(this.tabLFGMS_SelectedIndexChanged);
            // 
            // tabLF
            // 
            this.tabLF.Controls.Add(this.chart1);
            this.tabLF.Controls.Add(this.lvLF);
            this.tabLF.Location = new System.Drawing.Point(4, 22);
            this.tabLF.Name = "tabLF";
            this.tabLF.Padding = new System.Windows.Forms.Padding(3);
            this.tabLF.Size = new System.Drawing.Size(396, 240);
            this.tabLF.TabIndex = 0;
            this.tabLF.Text = "LF";
            this.tabLF.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(164, 28);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(226, 204);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // lvLF
            // 
            this.lvLF.ContextMenuStrip = this.contextMenuStrip1;
            this.lvLF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLF.Location = new System.Drawing.Point(3, 3);
            this.lvLF.MultiSelect = false;
            this.lvLF.Name = "lvLF";
            this.lvLF.Size = new System.Drawing.Size(390, 234);
            this.lvLF.TabIndex = 0;
            this.lvLF.UseCompatibleStateImageBehavior = false;
            this.lvLF.DoubleClick += new System.EventHandler(this.lvLF_DoubleClick);
            this.lvLF.Leave += new System.EventHandler(this.lvLF_Leave);
            this.lvLF.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvLF_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(108, 48);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Image = global::FAD3.Properties.Resources.Add_16xSM;
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::FAD3.Properties.Resources.Remove_16xSM;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // tabGMS
            // 
            this.tabGMS.Controls.Add(this.lvGMS);
            this.tabGMS.Location = new System.Drawing.Point(4, 22);
            this.tabGMS.Name = "tabGMS";
            this.tabGMS.Padding = new System.Windows.Forms.Padding(3);
            this.tabGMS.Size = new System.Drawing.Size(396, 240);
            this.tabGMS.TabIndex = 1;
            this.tabGMS.Text = "GMS";
            this.tabGMS.UseVisualStyleBackColor = true;
            // 
            // lvGMS
            // 
            this.lvGMS.ContextMenuStrip = this.contextMenuStrip1;
            this.lvGMS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvGMS.Location = new System.Drawing.Point(3, 3);
            this.lvGMS.Name = "lvGMS";
            this.lvGMS.Size = new System.Drawing.Size(390, 234);
            this.lvGMS.TabIndex = 0;
            this.lvGMS.UseCompatibleStateImageBehavior = false;
            this.lvGMS.DoubleClick += new System.EventHandler(this.lvGMS_DoubleClick);
            this.lvGMS.Leave += new System.EventHandler(this.lvGMS_Leave);
            this.lvGMS.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvGMS_MouseDown);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOk.Location = new System.Drawing.Point(410, 57);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(66, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(-1, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(47, 15);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "label1";
            // 
            // frmLenFreq
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOk;
            this.ClientSize = new System.Drawing.Size(488, 304);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.tabLFGMS);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLenFreq";
            this.Text = "frmLenFreq";
            this.Load += new System.EventHandler(this.frmLenFreq_Load);
            this.tabLFGMS.ResumeLayout(false);
            this.tabLF.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabGMS.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabLFGMS;
        private System.Windows.Forms.TabPage tabLF;
        private System.Windows.Forms.TabPage tabGMS;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.ListView lvLF;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ListView lvGMS;
    }
}