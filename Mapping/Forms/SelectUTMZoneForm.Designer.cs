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
            this.btnIgnore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbtnZone50N
            // 
            this.rbtnZone50N.AutoSize = true;
            this.rbtnZone50N.Location = new System.Drawing.Point(16, 66);
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
            this.rbtnZone51N.Location = new System.Drawing.Point(16, 89);
            this.rbtnZone51N.Name = "rbtnZone51N";
            this.rbtnZone51N.Size = new System.Drawing.Size(283, 17);
            this.rbtnZone51N.TabIndex = 1;
            this.rbtnZone51N.TabStop = true;
            this.rbtnZone51N.Text = "Zone 51N - Majority of the  Philippines except Palawan";
            this.rbtnZone51N.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(328, 35);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select a zone. The selected zone will be used in blanking out inland points. ";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(295, 136);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(42, 25);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(126, 136);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(49, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnIgnore
            // 
            this.btnIgnore.Location = new System.Drawing.Point(181, 136);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(102, 25);
            this.btnIgnore.TabIndex = 7;
            this.btnIgnore.Text = "Ignore UTM zone";
            this.btnIgnore.UseVisualStyleBackColor = true;
            this.btnIgnore.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // SelectUTMZoneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 172);
            this.Controls.Add(this.btnIgnore);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbtnZone51N);
            this.Controls.Add(this.rbtnZone50N);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SelectUTMZoneForm";
            this.Text = "Select UTM zone";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtnZone50N;
        private System.Windows.Forms.RadioButton rbtnZone51N;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnIgnore;
    }
}