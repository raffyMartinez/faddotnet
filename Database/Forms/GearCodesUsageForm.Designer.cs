namespace FAD3
{
    partial class GearCodesUsageForm
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
            this.listViewVariations = new System.Windows.Forms.ListView();
            this.dropDownMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.listViewCodes = new System.Windows.Forms.ListView();
            this.listViewWhereUsed = new System.Windows.Forms.ListView();
            this.listViewLocalNames = new System.Windows.Forms.ListView();
            this.buttonOk = new System.Windows.Forms.Button();
            this.comboClass = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.tbAdd = new System.Windows.Forms.ToolStripButton();
            this.tbRemove = new System.Windows.Forms.ToolStripButton();
            this.tbEdit = new System.Windows.Forms.ToolStripButton();
            this.tbExport = new System.Windows.Forms.ToolStripButton();
            this.tbImport = new System.Windows.Forms.ToolStripButton();
            this.toolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewVariations
            // 
            this.listViewVariations.ContextMenuStrip = this.dropDownMenu;
            this.listViewVariations.Location = new System.Drawing.Point(10, 103);
            this.listViewVariations.Name = "listViewVariations";
            this.listViewVariations.Size = new System.Drawing.Size(227, 266);
            this.listViewVariations.TabIndex = 0;
            this.listViewVariations.UseCompatibleStateImageBehavior = false;
            this.listViewVariations.Click += new System.EventHandler(this.OnlistView_Click);
            this.listViewVariations.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListView_MouseDown);
            // 
            // dropDownMenu
            // 
            this.dropDownMenu.Name = "dropDownMenu";
            this.dropDownMenu.Size = new System.Drawing.Size(61, 4);
            this.dropDownMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnDropDownMenu_ItemClicked);
            // 
            // listViewCodes
            // 
            this.listViewCodes.ContextMenuStrip = this.dropDownMenu;
            this.listViewCodes.Location = new System.Drawing.Point(242, 103);
            this.listViewCodes.Name = "listViewCodes";
            this.listViewCodes.Size = new System.Drawing.Size(159, 266);
            this.listViewCodes.TabIndex = 1;
            this.listViewCodes.UseCompatibleStateImageBehavior = false;
            this.listViewCodes.Click += new System.EventHandler(this.OnlistView_Click);
            this.listViewCodes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListView_MouseDown);
            // 
            // listViewWhereUsed
            // 
            this.listViewWhereUsed.ContextMenuStrip = this.dropDownMenu;
            this.listViewWhereUsed.Location = new System.Drawing.Point(406, 103);
            this.listViewWhereUsed.Name = "listViewWhereUsed";
            this.listViewWhereUsed.Size = new System.Drawing.Size(193, 266);
            this.listViewWhereUsed.TabIndex = 2;
            this.listViewWhereUsed.UseCompatibleStateImageBehavior = false;
            this.listViewWhereUsed.Click += new System.EventHandler(this.OnlistView_Click);
            this.listViewWhereUsed.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListView_MouseDown);
            // 
            // listViewLocalNames
            // 
            this.listViewLocalNames.ContextMenuStrip = this.dropDownMenu;
            this.listViewLocalNames.Location = new System.Drawing.Point(604, 103);
            this.listViewLocalNames.Name = "listViewLocalNames";
            this.listViewLocalNames.Size = new System.Drawing.Size(193, 266);
            this.listViewLocalNames.TabIndex = 3;
            this.listViewLocalNames.UseCompatibleStateImageBehavior = false;
            this.listViewLocalNames.Click += new System.EventHandler(this.OnlistView_Click);
            this.listViewLocalNames.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListView_MouseDown);
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOk.Location = new System.Drawing.Point(736, 381);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(57, 25);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // comboClass
            // 
            this.comboClass.FormattingEnabled = true;
            this.comboClass.Location = new System.Drawing.Point(10, 56);
            this.comboClass.Name = "comboClass";
            this.comboClass.Size = new System.Drawing.Size(193, 23);
            this.comboClass.TabIndex = 5;
            this.comboClass.SelectedIndexChanged += new System.EventHandler(this.OnComboSelectedIndexChanged);
            this.comboClass.Validated += new System.EventHandler(this.OnComboClass_Validated);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Gear variation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Gear codes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(403, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Where used";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(601, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Local names of gear";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Class of gear";
            // 
            // toolBar
            // 
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbAdd,
            this.tbRemove,
            this.tbEdit,
            this.tbExport,
            this.tbImport});
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(803, 25);
            this.toolBar.TabIndex = 12;
            this.toolBar.Text = "toolStrip1";
            this.toolBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnToolbarItemClick);
            // 
            // tbAdd
            // 
            this.tbAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbAdd.Image = global::FAD3.Properties.Resources.Add_16xSM;
            this.tbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbAdd.Name = "tbAdd";
            this.tbAdd.Size = new System.Drawing.Size(23, 22);
            this.tbAdd.Text = "toolStripButton1";
            this.tbAdd.ToolTipText = "Add";
            // 
            // tbRemove
            // 
            this.tbRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbRemove.Image = global::FAD3.Properties.Resources.Remove_16xSM;
            this.tbRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRemove.Name = "tbRemove";
            this.tbRemove.Size = new System.Drawing.Size(23, 22);
            this.tbRemove.Text = "toolStripButton2";
            this.tbRemove.ToolTipText = "Remove";
            // 
            // tbEdit
            // 
            this.tbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbEdit.Image = global::FAD3.Properties.Resources.Edit_16xMD;
            this.tbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbEdit.Name = "tbEdit";
            this.tbEdit.Size = new System.Drawing.Size(23, 22);
            this.tbEdit.Text = "toolStripButton3";
            this.tbEdit.ToolTipText = "Edit";
            // 
            // tbExport
            // 
            this.tbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbExport.Image = global::FAD3.Properties.Resources.ExportFile_16x;
            this.tbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbExport.Name = "tbExport";
            this.tbExport.Size = new System.Drawing.Size(23, 22);
            this.tbExport.Text = "toolStripButton5";
            this.tbExport.ToolTipText = "Export";
            // 
            // tbImport
            // 
            this.tbImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbImport.Image = global::FAD3.Properties.Resources.ImportFile_16x;
            this.tbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbImport.Name = "tbImport";
            this.tbImport.Size = new System.Drawing.Size(23, 22);
            this.tbImport.Text = "toolStripButton4";
            this.tbImport.ToolTipText = "Import";
            // 
            // GearCodesUsageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOk;
            this.ClientSize = new System.Drawing.Size(803, 415);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboClass);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.listViewLocalNames);
            this.Controls.Add(this.listViewWhereUsed);
            this.Controls.Add(this.listViewCodes);
            this.Controls.Add(this.listViewVariations);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GearCodesUsageForm";
            this.ShowInTaskbar = false;
            this.Text = "Gear variations and target areas where used";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewVariations;
        private System.Windows.Forms.ListView listViewCodes;
        private System.Windows.Forms.ListView listViewWhereUsed;
        private System.Windows.Forms.ListView listViewLocalNames;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.ComboBox comboClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ContextMenuStrip dropDownMenu;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton tbAdd;
        private System.Windows.Forms.ToolStripButton tbRemove;
        private System.Windows.Forms.ToolStripButton tbEdit;
        private System.Windows.Forms.ToolStripButton tbImport;
        private System.Windows.Forms.ToolStripButton tbExport;
    }
}