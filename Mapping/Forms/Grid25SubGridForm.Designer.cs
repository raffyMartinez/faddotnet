namespace FAD3.Mapping.Forms
{
    partial class Grid25SubGridForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb100 = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rb25 = new System.Windows.Forms.RadioButton();
            this.rb16 = new System.Windows.Forms.RadioButton();
            this.rb9 = new System.Windows.Forms.RadioButton();
            this.rb4 = new System.Windows.Forms.RadioButton();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rb64 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb64);
            this.groupBox1.Controls.Add(this.rb100);
            this.groupBox1.Controls.Add(this.rbNone);
            this.groupBox1.Controls.Add(this.rb25);
            this.groupBox1.Controls.Add(this.rb16);
            this.groupBox1.Controls.Add(this.rb9);
            this.groupBox1.Controls.Add(this.rb4);
            this.groupBox1.Location = new System.Drawing.Point(12, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(127, 185);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sub grid options";
            // 
            // rb100
            // 
            this.rb100.AutoSize = true;
            this.rb100.Location = new System.Drawing.Point(15, 155);
            this.rb100.Name = "rb100";
            this.rb100.Size = new System.Drawing.Size(90, 17);
            this.rb100.TabIndex = 5;
            this.rb100.Text = "100  - (200 m)";
            this.rb100.UseVisualStyleBackColor = true;
            this.rb100.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(15, 21);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 4;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            this.rbNone.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // rb25
            // 
            this.rb25.AutoSize = true;
            this.rb25.Location = new System.Drawing.Point(15, 109);
            this.rb25.Name = "rb25";
            this.rb25.Size = new System.Drawing.Size(84, 17);
            this.rb25.TabIndex = 4;
            this.rb25.Text = "25  - (400 m)";
            this.rb25.UseVisualStyleBackColor = true;
            this.rb25.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // rb16
            // 
            this.rb16.AutoSize = true;
            this.rb16.Location = new System.Drawing.Point(15, 87);
            this.rb16.Name = "rb16";
            this.rb16.Size = new System.Drawing.Size(84, 17);
            this.rb16.TabIndex = 3;
            this.rb16.Text = "16  - (500 m)";
            this.rb16.UseVisualStyleBackColor = true;
            this.rb16.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // rb9
            // 
            this.rb9.AutoSize = true;
            this.rb9.Location = new System.Drawing.Point(15, 65);
            this.rb9.Name = "rb9";
            this.rb9.Size = new System.Drawing.Size(78, 17);
            this.rb9.TabIndex = 2;
            this.rb9.Text = "9  - (666 m)";
            this.rb9.UseVisualStyleBackColor = true;
            this.rb9.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // rb4
            // 
            this.rb4.AutoSize = true;
            this.rb4.Location = new System.Drawing.Point(15, 43);
            this.rb4.Name = "rb4";
            this.rb4.Size = new System.Drawing.Size(84, 17);
            this.rb4.TabIndex = 1;
            this.rb4.Text = "4  - (1000 m)";
            this.rb4.UseVisualStyleBackColor = true;
            this.rb4.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(171, 42);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(136, 69);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "4 subgrids with sides 1000 meters long will be created";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(296, 176);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(36, 30);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(240, 176);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // rb64
            // 
            this.rb64.AutoSize = true;
            this.rb64.Location = new System.Drawing.Point(15, 132);
            this.rb64.Name = "rb64";
            this.rb64.Size = new System.Drawing.Size(84, 17);
            this.rb64.TabIndex = 6;
            this.rb64.Text = "64  - (250 m)";
            this.rb64.UseVisualStyleBackColor = true;
            this.rb64.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // Grid25SubGridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 223);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Grid25SubGridForm";
            this.Tag = "";
            this.Text = "Sub grid";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb25;
        private System.Windows.Forms.RadioButton rb16;
        private System.Windows.Forms.RadioButton rb9;
        private System.Windows.Forms.RadioButton rb4;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rb100;
        private System.Windows.Forms.RadioButton rb64;
    }
}