namespace FAD3.Database.Forms
{
    partial class CatchLocalNameSelectedForm
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
            this.listBox = new System.Windows.Forms.ListBox();
            this.dropDownMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lblList = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.ContextMenuStrip = this.dropDownMenu;
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(6, 84);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(253, 290);
            this.listBox.TabIndex = 0;
            this.listBox.DoubleClick += new System.EventHandler(this.OnListBoxDblClick);
            this.listBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListBoxMouseDown);
            // 
            // dropDownMenu
            // 
            this.dropDownMenu.Name = "contextMenuStrip";
            this.dropDownMenu.Size = new System.Drawing.Size(61, 4);
            this.dropDownMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnContextMenuItemClick);
            // 
            // lblList
            // 
            this.lblList.AutoSize = true;
            this.lblList.Location = new System.Drawing.Point(3, 68);
            this.lblList.Name = "lblList";
            this.lblList.Size = new System.Drawing.Size(69, 13);
            this.lblList.TabIndex = 1;
            this.lblList.Text = "List of names";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(208, 390);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(44, 24);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(8, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(251, 49);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Local names in language";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(6, 381);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(90, 13);
            this.lblCount.TabIndex = 4;
            this.lblCount.Text = "Number of names";
            // 
            // CatchLocalNameSelectedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 423);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblList);
            this.Controls.Add(this.listBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CatchLocalNameSelectedForm";
            this.ShowInTaskbar = false;
            this.Text = "CatchLocalNameSelectedForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClose);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Label lblList;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ContextMenuStrip dropDownMenu;
        private System.Windows.Forms.Label lblCount;
    }
}