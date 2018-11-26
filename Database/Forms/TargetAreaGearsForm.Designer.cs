namespace FAD3
{
    partial class TargetAreaGearsForm
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
            this.btnOk = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lvGears = new System.Windows.Forms.ListView();
            this.contexMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuMapThisGear = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBatch = new System.Windows.Forms.Button();
            this.txtCondition = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.contexMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(359, 455);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(49, 24);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(317, 16);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Fishing gears that are used in the target area";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "List of fishing gears";
            // 
            // lvGears
            // 
            this.lvGears.CheckBoxes = true;
            this.lvGears.ContextMenuStrip = this.contexMenu;
            this.lvGears.Location = new System.Drawing.Point(15, 56);
            this.lvGears.Name = "lvGears";
            this.lvGears.Size = new System.Drawing.Size(393, 372);
            this.lvGears.TabIndex = 5;
            this.lvGears.UseCompatibleStateImageBehavior = false;
            this.lvGears.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnListViewMouseClick);
            this.lvGears.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnListViewMouseDown);
            // 
            // contexMenu
            // 
            this.contexMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuMapThisGear});
            this.contexMenu.Name = "contexMenu";
            this.contexMenu.Size = new System.Drawing.Size(147, 26);
            this.contexMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.OnContextMenuItemClicked);
            // 
            // mnuMapThisGear
            // 
            this.mnuMapThisGear.Name = "mnuMapThisGear";
            this.mnuMapThisGear.Size = new System.Drawing.Size(146, 22);
            this.mnuMapThisGear.Text = "Map this gear";
            // 
            // btnBatch
            // 
            this.btnBatch.Location = new System.Drawing.Point(304, 455);
            this.btnBatch.Name = "btnBatch";
            this.btnBatch.Size = new System.Drawing.Size(49, 24);
            this.btnBatch.TabIndex = 6;
            this.btnBatch.Text = "Batch";
            this.btnBatch.UseVisualStyleBackColor = true;
            this.btnBatch.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // txtCondition
            // 
            this.txtCondition.Location = new System.Drawing.Point(15, 459);
            this.txtCondition.Name = "txtCondition";
            this.txtCondition.Size = new System.Drawing.Size(62, 20);
            this.txtCondition.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 440);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Select gears more than";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(92, 456);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(49, 24);
            this.btnSelect.TabIndex = 9;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // TargetAreaGearsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 496);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCondition);
            this.Controls.Add(this.btnBatch);
            this.Controls.Add(this.lvGears);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TargetAreaGearsForm";
            this.ShowInTaskbar = false;
            this.Text = "TargetAreaGearsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TargetAreaGearsForm_FormClosed);
            this.Load += new System.EventHandler(this.TargetAreaGearsForm_Load);
            this.contexMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvGears;
        private System.Windows.Forms.ContextMenuStrip contexMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuMapThisGear;
        private System.Windows.Forms.Button btnBatch;
        private System.Windows.Forms.TextBox txtCondition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelect;
    }
}