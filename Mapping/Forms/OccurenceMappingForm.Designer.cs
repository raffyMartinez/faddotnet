namespace FAD3.Mapping.Forms
{
    partial class OccurenceMappingForm
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
            this.rdbAll = new System.Windows.Forms.RadioButton();
            this.rdbSelected = new System.Windows.Forms.RadioButton();
            this.chkAggregate = new System.Windows.Forms.CheckBox();
            this.chkExcludeOne = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkListYears = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(233, 192);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(48, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(179, 192);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(48, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // rdbAll
            // 
            this.rdbAll.AutoSize = true;
            this.rdbAll.Location = new System.Drawing.Point(19, 21);
            this.rdbAll.Name = "rdbAll";
            this.rdbAll.Size = new System.Drawing.Size(95, 17);
            this.rdbAll.TabIndex = 2;
            this.rdbAll.TabStop = true;
            this.rdbAll.Text = "All target areas";
            this.rdbAll.UseVisualStyleBackColor = true;
            this.rdbAll.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // rdbSelected
            // 
            this.rdbSelected.AutoSize = true;
            this.rdbSelected.Location = new System.Drawing.Point(19, 44);
            this.rdbSelected.Name = "rdbSelected";
            this.rdbSelected.Size = new System.Drawing.Size(121, 17);
            this.rdbSelected.TabIndex = 3;
            this.rdbSelected.TabStop = true;
            this.rdbSelected.Text = "Selected target area";
            this.rdbSelected.UseVisualStyleBackColor = true;
            this.rdbSelected.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // chkAggregate
            // 
            this.chkAggregate.AutoSize = true;
            this.chkAggregate.Location = new System.Drawing.Point(128, 130);
            this.chkAggregate.Name = "chkAggregate";
            this.chkAggregate.Size = new System.Drawing.Size(159, 17);
            this.chkAggregate.TabIndex = 4;
            this.chkAggregate.Text = "Aggregate on fishing ground";
            this.chkAggregate.UseVisualStyleBackColor = true;
            this.chkAggregate.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // chkExcludeOne
            // 
            this.chkExcludeOne.AutoSize = true;
            this.chkExcludeOne.Location = new System.Drawing.Point(128, 153);
            this.chkExcludeOne.Name = "chkExcludeOne";
            this.chkExcludeOne.Size = new System.Drawing.Size(85, 17);
            this.chkExcludeOne.TabIndex = 5;
            this.chkExcludeOne.Text = "Exclude n=1";
            this.chkExcludeOne.UseVisualStyleBackColor = true;
            this.chkExcludeOne.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbAll);
            this.groupBox1.Controls.Add(this.rdbSelected);
            this.groupBox1.Location = new System.Drawing.Point(122, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(165, 73);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chose coverage";
            // 
            // chkListYears
            // 
            this.chkListYears.CheckOnClick = true;
            this.chkListYears.FormattingEnabled = true;
            this.chkListYears.Location = new System.Drawing.Point(10, 48);
            this.chkListYears.Name = "chkListYears";
            this.chkListYears.Size = new System.Drawing.Size(102, 124);
            this.chkListYears.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Year of sampling";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(13, 7);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(47, 15);
            this.lblName.TabIndex = 9;
            this.lblName.Text = "label2";
            // 
            // OccurenceMappingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 221);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkListYears);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkExcludeOne);
            this.Controls.Add(this.chkAggregate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OccurenceMappingForm";
            this.Text = "Occurence mapping options";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rdbAll;
        private System.Windows.Forms.RadioButton rdbSelected;
        private System.Windows.Forms.CheckBox chkAggregate;
        private System.Windows.Forms.CheckBox chkExcludeOne;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox chkListYears;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblName;
    }
}