namespace FAD3
{
    partial class ManageMRUForm
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
            this.listBoxFiles = new System.Windows.Forms.ListBox();
            this.dropDownMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dropDownMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxFiles
            // 
            this.listBoxFiles.ContextMenuStrip = this.dropDownMenu;
            this.listBoxFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxFiles.FormattingEnabled = true;
            this.listBoxFiles.ItemHeight = 15;
            this.listBoxFiles.Location = new System.Drawing.Point(1, 45);
            this.listBoxFiles.Name = "listBoxFiles";
            this.listBoxFiles.Size = new System.Drawing.Size(392, 229);
            this.listBoxFiles.TabIndex = 0;
            this.listBoxFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxFiles_MouseDown);
            // 
            // dropDownMenu
            // 
            this.dropDownMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpen,
            this.menuRemove});
            this.dropDownMenu.Name = "contextMenuStrip1";
            this.dropDownMenu.Size = new System.Drawing.Size(118, 48);
            this.dropDownMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnMenuItem_ItemClicked);
            // 
            // menuOpen
            // 
            this.menuOpen.Name = "menuOpen";
            this.menuOpen.Size = new System.Drawing.Size(117, 22);
            this.menuOpen.Text = "Open";
            // 
            // menuRemove
            // 
            this.menuRemove.Name = "menuRemove";
            this.menuRemove.Size = new System.Drawing.Size(117, 22);
            this.menuRemove.Text = "Remove";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(330, 296);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(53, 26);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(271, 296);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(53, 26);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(363, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "The list below are the FAD database files that you recently opened";
            // 
            // ManageMRUForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 334);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.listBoxFiles);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ManageMRUForm";
            this.ShowInTaskbar = false;
            this.Text = "Manage recently opened files";
            this.dropDownMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxFiles;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ContextMenuStrip dropDownMenu;
        private System.Windows.Forms.ToolStripMenuItem menuOpen;
        private System.Windows.Forms.ToolStripMenuItem menuRemove;
        private System.Windows.Forms.Label label1;
    }
}