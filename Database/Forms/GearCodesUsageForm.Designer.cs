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
            this.btnExport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewVariations
            // 
            this.listViewVariations.ContextMenuStrip = this.dropDownMenu;
            this.listViewVariations.Location = new System.Drawing.Point(10, 75);
            this.listViewVariations.Name = "listViewVariations";
            this.listViewVariations.Size = new System.Drawing.Size(227, 266);
            this.listViewVariations.TabIndex = 0;
            this.listViewVariations.UseCompatibleStateImageBehavior = false;
            this.listViewVariations.Click += new System.EventHandler(this.OnlistView_Click);
            this.listViewVariations.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnlistView_MouseClick);
            this.listViewVariations.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListView_MouseDown);
            // 
            // dropDownMenu
            // 
            this.dropDownMenu.Name = "dropDownMenu";
            this.dropDownMenu.Size = new System.Drawing.Size(61, 4);
            this.dropDownMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.dropDownMenu_ItemClicked);
            // 
            // listViewCodes
            // 
            this.listViewCodes.ContextMenuStrip = this.dropDownMenu;
            this.listViewCodes.Location = new System.Drawing.Point(242, 75);
            this.listViewCodes.Name = "listViewCodes";
            this.listViewCodes.Size = new System.Drawing.Size(159, 266);
            this.listViewCodes.TabIndex = 1;
            this.listViewCodes.UseCompatibleStateImageBehavior = false;
            this.listViewCodes.Click += new System.EventHandler(this.OnlistView_Click);
            this.listViewCodes.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnlistView_MouseClick);
            this.listViewCodes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListView_MouseDown);
            // 
            // listViewWhereUsed
            // 
            this.listViewWhereUsed.ContextMenuStrip = this.dropDownMenu;
            this.listViewWhereUsed.Location = new System.Drawing.Point(406, 75);
            this.listViewWhereUsed.Name = "listViewWhereUsed";
            this.listViewWhereUsed.Size = new System.Drawing.Size(193, 266);
            this.listViewWhereUsed.TabIndex = 2;
            this.listViewWhereUsed.UseCompatibleStateImageBehavior = false;
            this.listViewWhereUsed.Click += new System.EventHandler(this.OnlistView_Click);
            this.listViewWhereUsed.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnlistView_MouseClick);
            this.listViewWhereUsed.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListView_MouseDown);
            // 
            // listViewLocalNames
            // 
            this.listViewLocalNames.ContextMenuStrip = this.dropDownMenu;
            this.listViewLocalNames.Location = new System.Drawing.Point(604, 75);
            this.listViewLocalNames.Name = "listViewLocalNames";
            this.listViewLocalNames.Size = new System.Drawing.Size(193, 266);
            this.listViewLocalNames.TabIndex = 3;
            this.listViewLocalNames.UseCompatibleStateImageBehavior = false;
            this.listViewLocalNames.Click += new System.EventHandler(this.OnlistView_Click);
            this.listViewLocalNames.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnlistView_MouseClick);
            this.listViewLocalNames.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListView_MouseDown);
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOk.Location = new System.Drawing.Point(736, 353);
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
            this.comboClass.Location = new System.Drawing.Point(10, 28);
            this.comboClass.Name = "comboClass";
            this.comboClass.Size = new System.Drawing.Size(193, 23);
            this.comboClass.TabIndex = 5;
            this.comboClass.Validated += new System.EventHandler(this.comboClass_Validated);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Gear variation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Gear codes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(403, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Where used";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(601, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Local names of gear";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Class of gear";
            // 
            // btnExport
            // 
            this.btnExport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExport.Location = new System.Drawing.Point(673, 353);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(57, 25);
            this.btnExport.TabIndex = 11;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // GearCodesUsageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOk;
            this.ClientSize = new System.Drawing.Size(803, 390);
            this.Controls.Add(this.btnExport);
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
            this.Text = "Gear variations and target areas where used";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
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
        private System.Windows.Forms.Button btnExport;
    }
}