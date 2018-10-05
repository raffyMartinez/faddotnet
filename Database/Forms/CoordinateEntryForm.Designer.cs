namespace FAD3
{
    partial class CoordinateEntryForm
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
            this.mtextLongitude = new System.Windows.Forms.MaskedTextBox();
            this.mtextLatitude = new System.Windows.Forms.MaskedTextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Longitude";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Latitude";
            // 
            // mtextLongitude
            // 
            this.mtextLongitude.Location = new System.Drawing.Point(80, 57);
            this.mtextLongitude.Name = "mtextLongitude";
            this.mtextLongitude.Size = new System.Drawing.Size(140, 21);
            this.mtextLongitude.TabIndex = 3;
            this.mtextLongitude.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.mtextLongitude.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Onmtext_KeyUp);
            this.mtextLongitude.Validating += new System.ComponentModel.CancelEventHandler(this.OnCoordinate_Validating);
            // 
            // mtextLatitude
            // 
            this.mtextLatitude.Location = new System.Drawing.Point(80, 25);
            this.mtextLatitude.Name = "mtextLatitude";
            this.mtextLatitude.Size = new System.Drawing.Size(140, 21);
            this.mtextLatitude.TabIndex = 4;
            this.mtextLatitude.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.mtextLatitude.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Onmtext_KeyUp);
            this.mtextLatitude.Validating += new System.ComponentModel.CancelEventHandler(this.OnCoordinate_Validating);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(182, 104);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(38, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(121, 104);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(55, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // CoordinateEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(230, 142);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.mtextLatitude);
            this.Controls.Add(this.mtextLongitude);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CoordinateEntryForm";
            this.Text = "Coordinates";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnCoordinateEntryForm_FormClosed);
            this.Load += new System.EventHandler(this.OnCoordinateEntryForm_Load);
            this.Shown += new System.EventHandler(this.CoordinateEntryForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox mtextLongitude;
        private System.Windows.Forms.MaskedTextBox mtextLatitude;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}