namespace FAD3
{
    partial class LandingSiteFromKMLForm
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
            this.btnKML = new System.Windows.Forms.Button();
            this.lvLS = new System.Windows.Forms.ListView();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.contextMenuLV = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemSetMunicipality = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuLV.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnKML
            // 
            this.btnKML.Location = new System.Drawing.Point(7, 35);
            this.btnKML.Name = "btnKML";
            this.btnKML.Size = new System.Drawing.Size(68, 26);
            this.btnKML.TabIndex = 0;
            this.btnKML.Text = "Open KML";
            this.btnKML.UseVisualStyleBackColor = true;
            this.btnKML.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lvLS
            // 
            this.lvLS.Location = new System.Drawing.Point(7, 67);
            this.lvLS.Name = "lvLS";
            this.lvLS.Size = new System.Drawing.Size(550, 223);
            this.lvLS.TabIndex = 1;
            this.lvLS.UseCompatibleStateImageBehavior = false;
            this.lvLS.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnItemMouseUp);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(507, 307);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(50, 26);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(451, 307);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 26);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // contextMenuLV
            // 
            this.contextMenuLV.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemSetMunicipality});
            this.contextMenuLV.Name = "contextMenuLV";
            this.contextMenuLV.Size = new System.Drawing.Size(160, 26);
            this.contextMenuLV.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnContextMenuClick);
            // 
            // itemSetMunicipality
            // 
            this.itemSetMunicipality.Name = "itemSetMunicipality";
            this.itemSetMunicipality.Size = new System.Drawing.Size(159, 22);
            this.itemSetMunicipality.Text = "Set municipality";
            // 
            // LandingSiteFromKMLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 345);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lvLS);
            this.Controls.Add(this.btnKML);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LandingSiteFromKMLForm";
            this.ShowInTaskbar = false;
            this.Text = "LandingSiteFromKMLForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LandingSiteFromKMLForm_FormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.contextMenuLV.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnKML;
        private System.Windows.Forms.ListView lvLS;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ContextMenuStrip contextMenuLV;
        private System.Windows.Forms.ToolStripMenuItem itemSetMunicipality;
    }
}