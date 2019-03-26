namespace FAD3
{
    partial class FishingGroundForm
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
            this.tabFG = new System.Windows.Forms.TabControl();
            this.tabGrid25 = new System.Windows.Forms.TabPage();
            this.lblSubGrid = new System.Windows.Forms.Label();
            this.textBoxSubGrid = new System.Windows.Forms.TextBox();
            this.lvGrids = new System.Windows.Forms.ListView();
            this.buttonRemoveAll = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxRow = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxColumn = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxGridNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxZone = new System.Windows.Forms.TextBox();
            this.tabText = new System.Windows.Forms.TabPage();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonGrids = new System.Windows.Forms.Button();
            this.tabFG.SuspendLayout();
            this.tabGrid25.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabFG
            // 
            this.tabFG.Controls.Add(this.tabGrid25);
            this.tabFG.Controls.Add(this.tabText);
            this.tabFG.Location = new System.Drawing.Point(8, 38);
            this.tabFG.Margin = new System.Windows.Forms.Padding(4);
            this.tabFG.Name = "tabFG";
            this.tabFG.SelectedIndex = 0;
            this.tabFG.Size = new System.Drawing.Size(349, 198);
            this.tabFG.TabIndex = 0;
            // 
            // tabGrid25
            // 
            this.tabGrid25.Controls.Add(this.lblSubGrid);
            this.tabGrid25.Controls.Add(this.textBoxSubGrid);
            this.tabGrid25.Controls.Add(this.lvGrids);
            this.tabGrid25.Controls.Add(this.buttonRemoveAll);
            this.tabGrid25.Controls.Add(this.buttonRemove);
            this.tabGrid25.Controls.Add(this.buttonAdd);
            this.tabGrid25.Controls.Add(this.label4);
            this.tabGrid25.Controls.Add(this.textBoxRow);
            this.tabGrid25.Controls.Add(this.label3);
            this.tabGrid25.Controls.Add(this.textBoxColumn);
            this.tabGrid25.Controls.Add(this.label2);
            this.tabGrid25.Controls.Add(this.textBoxGridNo);
            this.tabGrid25.Controls.Add(this.label1);
            this.tabGrid25.Controls.Add(this.textBoxZone);
            this.tabGrid25.Location = new System.Drawing.Point(4, 24);
            this.tabGrid25.Margin = new System.Windows.Forms.Padding(4);
            this.tabGrid25.Name = "tabGrid25";
            this.tabGrid25.Padding = new System.Windows.Forms.Padding(4);
            this.tabGrid25.Size = new System.Drawing.Size(341, 170);
            this.tabGrid25.TabIndex = 0;
            this.tabGrid25.Text = "Grid 25";
            this.tabGrid25.UseVisualStyleBackColor = true;
            // 
            // lblSubGrid
            // 
            this.lblSubGrid.AutoSize = true;
            this.lblSubGrid.Location = new System.Drawing.Point(13, 128);
            this.lblSubGrid.Name = "lblSubGrid";
            this.lblSubGrid.Size = new System.Drawing.Size(54, 15);
            this.lblSubGrid.TabIndex = 13;
            this.lblSubGrid.Text = "Sub-grid";
            // 
            // textBoxSubGrid
            // 
            this.textBoxSubGrid.Location = new System.Drawing.Point(70, 125);
            this.textBoxSubGrid.Name = "textBoxSubGrid";
            this.textBoxSubGrid.Size = new System.Drawing.Size(75, 21);
            this.textBoxSubGrid.TabIndex = 12;
            this.textBoxSubGrid.Validating += new System.ComponentModel.CancelEventHandler(this.OntextBoxValidating);
            // 
            // lvGrids
            // 
            this.lvGrids.Location = new System.Drawing.Point(225, 21);
            this.lvGrids.Name = "lvGrids";
            this.lvGrids.Size = new System.Drawing.Size(100, 99);
            this.lvGrids.TabIndex = 11;
            this.lvGrids.UseCompatibleStateImageBehavior = false;
            this.lvGrids.DoubleClick += new System.EventHandler(this.lvGrids_DoubleClick);
            this.lvGrids.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvGrids_MouseDown);
            // 
            // buttonRemoveAll
            // 
            this.buttonRemoveAll.Location = new System.Drawing.Point(163, 87);
            this.buttonRemoveAll.Name = "buttonRemoveAll";
            this.buttonRemoveAll.Size = new System.Drawing.Size(39, 23);
            this.buttonRemoveAll.TabIndex = 10;
            this.buttonRemoveAll.Text = "<<";
            this.buttonRemoveAll.UseVisualStyleBackColor = true;
            this.buttonRemoveAll.Click += new System.EventHandler(this.OnbuttonGrid25_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(163, 60);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(39, 23);
            this.buttonRemove.TabIndex = 9;
            this.buttonRemove.Text = "<";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.OnbuttonGrid25_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(163, 31);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(39, 23);
            this.buttonAdd.TabIndex = 8;
            this.buttonAdd.Text = ">";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.OnbuttonGrid25_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Row";
            // 
            // textBoxRow
            // 
            this.textBoxRow.Location = new System.Drawing.Point(70, 98);
            this.textBoxRow.Name = "textBoxRow";
            this.textBoxRow.Size = new System.Drawing.Size(75, 21);
            this.textBoxRow.TabIndex = 6;
            this.textBoxRow.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTextBox_KeyDown);
            this.textBoxRow.Validating += new System.ComponentModel.CancelEventHandler(this.OntextBoxValidating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Column";
            // 
            // textBoxColumn
            // 
            this.textBoxColumn.Location = new System.Drawing.Point(70, 72);
            this.textBoxColumn.Name = "textBoxColumn";
            this.textBoxColumn.Size = new System.Drawing.Size(75, 21);
            this.textBoxColumn.TabIndex = 4;
            this.textBoxColumn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTextBox_KeyDown);
            this.textBoxColumn.Validating += new System.ComponentModel.CancelEventHandler(this.OntextBoxValidating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Grid #";
            // 
            // textBoxGridNo
            // 
            this.textBoxGridNo.Location = new System.Drawing.Point(70, 46);
            this.textBoxGridNo.Name = "textBoxGridNo";
            this.textBoxGridNo.Size = new System.Drawing.Size(75, 21);
            this.textBoxGridNo.TabIndex = 2;
            this.textBoxGridNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTextBox_KeyDown);
            this.textBoxGridNo.Validating += new System.ComponentModel.CancelEventHandler(this.OntextBoxValidating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Zone";
            // 
            // textBoxZone
            // 
            this.textBoxZone.Location = new System.Drawing.Point(70, 21);
            this.textBoxZone.Name = "textBoxZone";
            this.textBoxZone.Size = new System.Drawing.Size(75, 21);
            this.textBoxZone.TabIndex = 0;
            // 
            // tabText
            // 
            this.tabText.Location = new System.Drawing.Point(4, 24);
            this.tabText.Margin = new System.Windows.Forms.Padding(4);
            this.tabText.Name = "tabText";
            this.tabText.Padding = new System.Windows.Forms.Padding(4);
            this.tabText.Size = new System.Drawing.Size(341, 170);
            this.tabText.TabIndex = 1;
            this.tabText.Text = "Text";
            this.tabText.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(314, 243);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(39, 23);
            this.buttonOK.TabIndex = 9;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(253, 243);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(55, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // buttonGrids
            // 
            this.buttonGrids.CausesValidation = false;
            this.buttonGrids.Location = new System.Drawing.Point(195, 243);
            this.buttonGrids.Name = "buttonGrids";
            this.buttonGrids.Size = new System.Drawing.Size(52, 23);
            this.buttonGrids.TabIndex = 11;
            this.buttonGrids.Text = "Grids";
            this.buttonGrids.UseVisualStyleBackColor = true;
            this.buttonGrids.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // FishingGroundForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(368, 279);
            this.Controls.Add(this.buttonGrids);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.tabFG);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FishingGroundForm";
            this.ShowInTaskbar = false;
            this.Text = "Fishing ground";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.tabFG.ResumeLayout(false);
            this.tabGrid25.ResumeLayout(false);
            this.tabGrid25.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabFG;
        private System.Windows.Forms.TabPage tabGrid25;
        private System.Windows.Forms.ListView lvGrids;
        private System.Windows.Forms.Button buttonRemoveAll;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxRow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxColumn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxGridNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxZone;
        private System.Windows.Forms.TabPage tabText;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonGrids;
        private System.Windows.Forms.Label lblSubGrid;
        private System.Windows.Forms.TextBox textBoxSubGrid;
    }
}