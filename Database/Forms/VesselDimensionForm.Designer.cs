namespace FAD3
{
    partial class VesselDimensionForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textLength = new System.Windows.Forms.TextBox();
            this.textWidth = new System.Windows.Forms.TextBox();
            this.textHeight = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Length";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Width";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Height";
            // 
            // textLength
            // 
            this.textLength.Location = new System.Drawing.Point(88, 38);
            this.textLength.Name = "textLength";
            this.textLength.Size = new System.Drawing.Size(64, 21);
            this.textLength.TabIndex = 3;
            this.textLength.Validating += new System.ComponentModel.CancelEventHandler(this.textVessel_Validating);
            // 
            // textWidth
            // 
            this.textWidth.Location = new System.Drawing.Point(88, 68);
            this.textWidth.Name = "textWidth";
            this.textWidth.Size = new System.Drawing.Size(64, 21);
            this.textWidth.TabIndex = 4;
            this.textWidth.Validating += new System.ComponentModel.CancelEventHandler(this.textVessel_Validating);
            // 
            // textHeight
            // 
            this.textHeight.Location = new System.Drawing.Point(88, 98);
            this.textHeight.Name = "textHeight";
            this.textHeight.Size = new System.Drawing.Size(64, 21);
            this.textHeight.TabIndex = 5;
            this.textHeight.Validating += new System.ComponentModel.CancelEventHandler(this.textVessel_Validating);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(184, 34);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(54, 23);
            this.buttonOk.TabIndex = 6;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonVessel_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.Location = new System.Drawing.Point(184, 68);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(54, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonVessel_Click);
            // 
            // VesselDimensionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 144);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.textHeight);
            this.Controls.Add(this.textWidth);
            this.Controls.Add(this.textLength);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "VesselDimensionForm";
            this.ShowInTaskbar = false;
            this.Text = "Vessel dimension";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textLength;
        private System.Windows.Forms.TextBox textWidth;
        private System.Windows.Forms.TextBox textHeight;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
    }
}