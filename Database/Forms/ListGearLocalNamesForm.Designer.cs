namespace FAD3.Database.Forms
{
    partial class ListGearLocalNamesForm
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
            this.listBoxLocalNames = new System.Windows.Forms.ListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.dropDownMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCopyText = new System.Windows.Forms.ToolStripMenuItem();
            this.dropDownMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxLocalNames
            // 
            this.listBoxLocalNames.ContextMenuStrip = this.dropDownMenu;
            this.listBoxLocalNames.FormattingEnabled = true;
            this.listBoxLocalNames.Location = new System.Drawing.Point(1, 3);
            this.listBoxLocalNames.Name = "listBoxLocalNames";
            this.listBoxLocalNames.Size = new System.Drawing.Size(343, 407);
            this.listBoxLocalNames.Sorted = true;
            this.listBoxLocalNames.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Image = global::FAD3.Properties.Resources.im_exit;
            this.btnClose.Location = new System.Drawing.Point(300, 416);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(32, 33);
            this.btnClose.TabIndex = 1;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // dropDownMenu
            // 
            this.dropDownMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopyText});
            this.dropDownMenu.Name = "dropDownMenu";
            this.dropDownMenu.Size = new System.Drawing.Size(181, 48);
            this.dropDownMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnDropDownItemClicked);
            // 
            // menuCopyText
            // 
            this.menuCopyText.Name = "menuCopyText";
            this.menuCopyText.Size = new System.Drawing.Size(180, 22);
            this.menuCopyText.Text = "Copy text";
            // 
            // ListGearLocalNamesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 455);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.listBoxLocalNames);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ListGearLocalNamesForm";
            this.Text = "ListGearLocalNamesForm";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.dropDownMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxLocalNames;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip dropDownMenu;
        private System.Windows.Forms.ToolStripMenuItem menuCopyText;
    }
}