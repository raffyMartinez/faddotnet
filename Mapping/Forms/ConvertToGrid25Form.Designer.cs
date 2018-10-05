namespace FAD3
{
    partial class ConvertToGrid25Form
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbInlandNote = new System.Windows.Forms.RadioButton();
            this.rbInlandRemove = new System.Windows.Forms.RadioButton();
            this.rbInlandIgnore = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSelectMap = new System.Windows.Forms.Button();
            this.rbOutsideNote = new System.Windows.Forms.RadioButton();
            this.rbOutsideRemove = new System.Windows.Forms.RadioButton();
            this.rbOutsideIgnore = new System.Windows.Forms.RadioButton();
            this.lblTitle = new System.Windows.Forms.Label();
            this.chkIncludeCoordinates = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(242, 232);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(54, 28);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(180, 232);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(54, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbInlandNote);
            this.groupBox1.Controls.Add(this.rbInlandRemove);
            this.groupBox1.Controls.Add(this.rbInlandIgnore);
            this.groupBox1.Location = new System.Drawing.Point(12, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(110, 126);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Inland locations";
            // 
            // rbInlandNote
            // 
            this.rbInlandNote.AutoSize = true;
            this.rbInlandNote.Location = new System.Drawing.Point(17, 42);
            this.rbInlandNote.Name = "rbInlandNote";
            this.rbInlandNote.Size = new System.Drawing.Size(74, 17);
            this.rbInlandNote.TabIndex = 2;
            this.rbInlandNote.Text = "Take note";
            this.rbInlandNote.UseVisualStyleBackColor = true;
            // 
            // rbInlandRemove
            // 
            this.rbInlandRemove.AutoSize = true;
            this.rbInlandRemove.Location = new System.Drawing.Point(17, 65);
            this.rbInlandRemove.Name = "rbInlandRemove";
            this.rbInlandRemove.Size = new System.Drawing.Size(65, 17);
            this.rbInlandRemove.TabIndex = 1;
            this.rbInlandRemove.Text = "Remove";
            this.rbInlandRemove.UseVisualStyleBackColor = true;
            // 
            // rbInlandIgnore
            // 
            this.rbInlandIgnore.AutoSize = true;
            this.rbInlandIgnore.Checked = true;
            this.rbInlandIgnore.Location = new System.Drawing.Point(17, 19);
            this.rbInlandIgnore.Name = "rbInlandIgnore";
            this.rbInlandIgnore.Size = new System.Drawing.Size(55, 17);
            this.rbInlandIgnore.TabIndex = 0;
            this.rbInlandIgnore.TabStop = true;
            this.rbInlandIgnore.Text = "Ignore";
            this.rbInlandIgnore.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSelectMap);
            this.groupBox2.Controls.Add(this.rbOutsideNote);
            this.groupBox2.Controls.Add(this.rbOutsideRemove);
            this.groupBox2.Controls.Add(this.rbOutsideIgnore);
            this.groupBox2.Location = new System.Drawing.Point(146, 53);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(150, 126);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Outside map boundary";
            // 
            // btnSelectMap
            // 
            this.btnSelectMap.Enabled = false;
            this.btnSelectMap.Location = new System.Drawing.Point(55, 88);
            this.btnSelectMap.Name = "btnSelectMap";
            this.btnSelectMap.Size = new System.Drawing.Size(71, 24);
            this.btnSelectMap.TabIndex = 6;
            this.btnSelectMap.Text = "Select map";
            this.btnSelectMap.UseVisualStyleBackColor = true;
            // 
            // rbOutsideNote
            // 
            this.rbOutsideNote.AutoSize = true;
            this.rbOutsideNote.Location = new System.Drawing.Point(24, 42);
            this.rbOutsideNote.Name = "rbOutsideNote";
            this.rbOutsideNote.Size = new System.Drawing.Size(74, 17);
            this.rbOutsideNote.TabIndex = 5;
            this.rbOutsideNote.Text = "Take note";
            this.rbOutsideNote.UseVisualStyleBackColor = true;
            this.rbOutsideNote.CheckedChanged += new System.EventHandler(this.OnButtonsCheckedChange);
            // 
            // rbOutsideRemove
            // 
            this.rbOutsideRemove.AutoSize = true;
            this.rbOutsideRemove.Location = new System.Drawing.Point(24, 65);
            this.rbOutsideRemove.Name = "rbOutsideRemove";
            this.rbOutsideRemove.Size = new System.Drawing.Size(65, 17);
            this.rbOutsideRemove.TabIndex = 4;
            this.rbOutsideRemove.Text = "Remove";
            this.rbOutsideRemove.UseVisualStyleBackColor = true;
            this.rbOutsideRemove.CheckedChanged += new System.EventHandler(this.OnButtonsCheckedChange);
            // 
            // rbOutsideIgnore
            // 
            this.rbOutsideIgnore.AutoSize = true;
            this.rbOutsideIgnore.Checked = true;
            this.rbOutsideIgnore.Location = new System.Drawing.Point(24, 19);
            this.rbOutsideIgnore.Name = "rbOutsideIgnore";
            this.rbOutsideIgnore.Size = new System.Drawing.Size(55, 17);
            this.rbOutsideIgnore.TabIndex = 3;
            this.rbOutsideIgnore.TabStop = true;
            this.rbOutsideIgnore.Text = "Ignore";
            this.rbOutsideIgnore.UseVisualStyleBackColor = true;
            this.rbOutsideIgnore.CheckedChanged += new System.EventHandler(this.OnButtonsCheckedChange);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 19);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(47, 15);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "label1";
            // 
            // chkIncludeCoordinates
            // 
            this.chkIncludeCoordinates.AutoSize = true;
            this.chkIncludeCoordinates.Location = new System.Drawing.Point(15, 195);
            this.chkIncludeCoordinates.Name = "chkIncludeCoordinates";
            this.chkIncludeCoordinates.Size = new System.Drawing.Size(119, 17);
            this.chkIncludeCoordinates.TabIndex = 6;
            this.chkIncludeCoordinates.Text = "Include coordinates";
            this.chkIncludeCoordinates.UseVisualStyleBackColor = true;
            // 
            // ConvertToGrid25Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 272);
            this.Controls.Add(this.chkIncludeCoordinates);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ConvertToGrid25Form";
            this.Text = "Convert point shapefile to grid25";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnConvertToGrid25Form_FormClosed);
            this.Load += new System.EventHandler(this.OnConvertToGrid25Form_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbInlandNote;
        private System.Windows.Forms.RadioButton rbInlandRemove;
        private System.Windows.Forms.RadioButton rbInlandIgnore;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbOutsideNote;
        private System.Windows.Forms.RadioButton rbOutsideRemove;
        private System.Windows.Forms.RadioButton rbOutsideIgnore;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnSelectMap;
        private System.Windows.Forms.CheckBox chkIncludeCoordinates;
    }
}