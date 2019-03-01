namespace FAD3.Mapping.Forms
{
    partial class SelectUTMZoneForm
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
            this.rbtnZone50N = new System.Windows.Forms.RadioButton();
            this.rbtnZone51N = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkCreateFile = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // rbtnZone50N
            // 
            this.rbtnZone50N.AutoSize = true;
            this.rbtnZone50N.Location = new System.Drawing.Point(11, 33);
            this.rbtnZone50N.Name = "rbtnZone50N";
            this.rbtnZone50N.Size = new System.Drawing.Size(211, 17);
            this.rbtnZone50N.TabIndex = 0;
            this.rbtnZone50N.Text = "Zone 50N - Mostly in Palawan province";
            this.rbtnZone50N.UseVisualStyleBackColor = true;
            // 
            // rbtnZone51N
            // 
            this.rbtnZone51N.AutoSize = true;
            this.rbtnZone51N.Checked = true;
            this.rbtnZone51N.Location = new System.Drawing.Point(11, 56);
            this.rbtnZone51N.Name = "rbtnZone51N";
            this.rbtnZone51N.Size = new System.Drawing.Size(283, 17);
            this.rbtnZone51N.TabIndex = 1;
            this.rbtnZone51N.TabStop = true;
            this.rbtnZone51N.Text = "Zone 51N - Majority of the  Philippines except Palawan";
            this.rbtnZone51N.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select zone";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(251, 131);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(42, 25);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(196, 131);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(49, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // chkCreateFile
            // 
            this.chkCreateFile.AutoSize = true;
            this.chkCreateFile.Location = new System.Drawing.Point(11, 95);
            this.chkCreateFile.Name = "chkCreateFile";
            this.chkCreateFile.Size = new System.Drawing.Size(172, 17);
            this.chkCreateFile.TabIndex = 6;
            this.chkCreateFile.Text = "Create file without inland points";
            this.chkCreateFile.UseVisualStyleBackColor = true;
            // 
            // SelectUTMZoneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 165);
            this.Controls.Add(this.chkCreateFile);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbtnZone51N);
            this.Controls.Add(this.rbtnZone50N);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SelectUTMZoneForm";
            this.Text = "Select UTM zone";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtnZone50N;
        private System.Windows.Forms.RadioButton rbtnZone51N;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkCreateFile;
    }
}