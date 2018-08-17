namespace FAD3
{
    partial class CatchCompositionForm
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
            this.panelUI = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.labelCol6 = new System.Windows.Forms.Label();
            this.labelCol5 = new System.Windows.Forms.Label();
            this.labelCol4 = new System.Windows.Forms.Label();
            this.labelCol3 = new System.Windows.Forms.Label();
            this.labelCol2 = new System.Windows.Forms.Label();
            this.labelCol1 = new System.Windows.Forms.Label();
            this.labelCol10 = new System.Windows.Forms.Label();
            this.labelCol9 = new System.Windows.Forms.Label();
            this.labelCol8 = new System.Windows.Forms.Label();
            this.labelCol7 = new System.Windows.Forms.Label();
            this.labelSubSample = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panelUI
            // 
            this.panelUI.AutoScroll = true;
            this.panelUI.Location = new System.Drawing.Point(3, 76);
            this.panelUI.Name = "panelUI";
            this.panelUI.Size = new System.Drawing.Size(795, 347);
            this.panelUI.TabIndex = 0;
            this.panelUI.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnPanelUI_Scroll);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(743, 436);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(54, 27);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(679, 436);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(58, 27);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAdd.Location = new System.Drawing.Point(804, 74);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(29, 27);
            this.buttonAdd.TabIndex = 3;
            this.buttonAdd.Text = "+";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemove.Location = new System.Drawing.Point(804, 107);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(29, 27);
            this.buttonRemove.TabIndex = 4;
            this.buttonRemove.Text = "-";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // labelCol6
            // 
            this.labelCol6.AutoSize = true;
            this.labelCol6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCol6.Location = new System.Drawing.Point(310, 55);
            this.labelCol6.Name = "labelCol6";
            this.labelCol6.Size = new System.Drawing.Size(44, 15);
            this.labelCol6.TabIndex = 15;
            this.labelCol6.Text = "Count";
            // 
            // labelCol5
            // 
            this.labelCol5.AutoSize = true;
            this.labelCol5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCol5.Location = new System.Drawing.Point(230, 55);
            this.labelCol5.Name = "labelCol5";
            this.labelCol5.Size = new System.Drawing.Size(51, 15);
            this.labelCol5.TabIndex = 14;
            this.labelCol5.Text = "Weight";
            // 
            // labelCol4
            // 
            this.labelCol4.AutoSize = true;
            this.labelCol4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCol4.Location = new System.Drawing.Point(177, 55);
            this.labelCol4.Name = "labelCol4";
            this.labelCol4.Size = new System.Drawing.Size(57, 15);
            this.labelCol4.TabIndex = 13;
            this.labelCol4.Text = "Name 2";
            // 
            // labelCol3
            // 
            this.labelCol3.AutoSize = true;
            this.labelCol3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCol3.Location = new System.Drawing.Point(122, 55);
            this.labelCol3.Name = "labelCol3";
            this.labelCol3.Size = new System.Drawing.Size(57, 15);
            this.labelCol3.TabIndex = 12;
            this.labelCol3.Text = "Name 1";
            // 
            // labelCol2
            // 
            this.labelCol2.AutoSize = true;
            this.labelCol2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCol2.Location = new System.Drawing.Point(72, 55);
            this.labelCol2.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelCol2.Name = "labelCol2";
            this.labelCol2.Size = new System.Drawing.Size(90, 15);
            this.labelCol2.TabIndex = 11;
            this.labelCol2.Text = "Identification";
            // 
            // labelCol1
            // 
            this.labelCol1.AutoSize = true;
            this.labelCol1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCol1.Location = new System.Drawing.Point(16, 55);
            this.labelCol1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelCol1.Name = "labelCol1";
            this.labelCol1.Size = new System.Drawing.Size(35, 15);
            this.labelCol1.TabIndex = 10;
            this.labelCol1.Text = "Row";
            // 
            // labelCol10
            // 
            this.labelCol10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCol10.Location = new System.Drawing.Point(625, 40);
            this.labelCol10.Name = "labelCol10";
            this.labelCol10.Size = new System.Drawing.Size(41, 30);
            this.labelCol10.TabIndex = 19;
            this.labelCol10.Text = "Live fish";
            // 
            // labelCol9
            // 
            this.labelCol9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCol9.Location = new System.Drawing.Point(545, 40);
            this.labelCol9.Name = "labelCol9";
            this.labelCol9.Size = new System.Drawing.Size(45, 30);
            this.labelCol9.TabIndex = 18;
            this.labelCol9.Text = "From total";
            // 
            // labelCol8
            // 
            this.labelCol8.AutoSize = true;
            this.labelCol8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCol8.Location = new System.Drawing.Point(445, 55);
            this.labelCol8.Name = "labelCol8";
            this.labelCol8.Size = new System.Drawing.Size(42, 15);
            this.labelCol8.TabIndex = 17;
            this.labelCol8.Text = "count";
            // 
            // labelCol7
            // 
            this.labelCol7.AutoSize = true;
            this.labelCol7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCol7.Location = new System.Drawing.Point(363, 55);
            this.labelCol7.Name = "labelCol7";
            this.labelCol7.Size = new System.Drawing.Size(49, 15);
            this.labelCol7.TabIndex = 16;
            this.labelCol7.Text = "weight";
            // 
            // labelSubSample
            // 
            this.labelSubSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSubSample.Location = new System.Drawing.Point(363, 40);
            this.labelSubSample.Name = "labelSubSample";
            this.labelSubSample.Size = new System.Drawing.Size(124, 15);
            this.labelSubSample.TabIndex = 20;
            this.labelSubSample.Text = "Subsample";
            this.labelSubSample.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(-1, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(57, 20);
            this.labelTitle.TabIndex = 21;
            this.labelTitle.Text = "label1";
            // 
            // CatchCompositionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(845, 475);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.labelSubSample);
            this.Controls.Add(this.labelCol10);
            this.Controls.Add(this.labelCol9);
            this.Controls.Add(this.labelCol8);
            this.Controls.Add(this.labelCol7);
            this.Controls.Add(this.labelCol6);
            this.Controls.Add(this.labelCol5);
            this.Controls.Add(this.labelCol4);
            this.Controls.Add(this.labelCol3);
            this.Controls.Add(this.labelCol2);
            this.Controls.Add(this.labelCol1);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panelUI);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CatchCompositionForm";
            this.Text = "CatchCompositionForm";
            this.Load += new System.EventHandler(this.OnForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelUI;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Label labelCol6;
        private System.Windows.Forms.Label labelCol5;
        private System.Windows.Forms.Label labelCol4;
        private System.Windows.Forms.Label labelCol3;
        private System.Windows.Forms.Label labelCol2;
        private System.Windows.Forms.Label labelCol1;
        private System.Windows.Forms.Label labelCol10;
        private System.Windows.Forms.Label labelCol9;
        private System.Windows.Forms.Label labelCol8;
        private System.Windows.Forms.Label labelCol7;
        private System.Windows.Forms.Label labelSubSample;
        private System.Windows.Forms.Label labelTitle;
    }
}