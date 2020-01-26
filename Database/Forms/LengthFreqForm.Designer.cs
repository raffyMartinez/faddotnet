namespace FAD3
{
    partial class LengthFreqForm
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.checkUseSize = new System.Windows.Forms.CheckBox();
            this.textIntervalSize = new System.Windows.Forms.TextBox();
            this.checkUniqueIntervals = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.lblTotalFreq = new System.Windows.Forms.Label();
            this.lblLen = new System.Windows.Forms.Label();
            this.lblFreq = new System.Windows.Forms.Label();
            this.lblCatchCompCount = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelUI
            // 
            this.panelUI.AutoScroll = true;
            this.panelUI.Location = new System.Drawing.Point(0, 150);
            this.panelUI.Margin = new System.Windows.Forms.Padding(4);
            this.panelUI.Name = "panelUI";
            this.panelUI.Size = new System.Drawing.Size(242, 278);
            this.panelUI.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(241, 447);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(52, 25);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(182, 447);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(53, 25);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // labelTitle
            // 
            this.labelTitle.Location = new System.Drawing.Point(3, 8);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(290, 36);
            this.labelTitle.TabIndex = 3;
            this.labelTitle.Text = "label1";
            // 
            // checkUseSize
            // 
            this.checkUseSize.AutoSize = true;
            this.checkUseSize.Location = new System.Drawing.Point(9, 21);
            this.checkUseSize.Name = "checkUseSize";
            this.checkUseSize.Size = new System.Drawing.Size(123, 19);
            this.checkUseSize.TabIndex = 6;
            this.checkUseSize.Text = "Class interval size";
            this.checkUseSize.UseVisualStyleBackColor = true;
            // 
            // textIntervalSize
            // 
            this.textIntervalSize.Location = new System.Drawing.Point(29, 45);
            this.textIntervalSize.Name = "textIntervalSize";
            this.textIntervalSize.Size = new System.Drawing.Size(53, 21);
            this.textIntervalSize.TabIndex = 7;
            this.textIntervalSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnText_KeyDown);
            this.textIntervalSize.Validating += new System.ComponentModel.CancelEventHandler(this.OntextIntervalSize_Validating);
            // 
            // checkUniqueIntervals
            // 
            this.checkUniqueIntervals.AutoSize = true;
            this.checkUniqueIntervals.Location = new System.Drawing.Point(138, 21);
            this.checkUniqueIntervals.Name = "checkUniqueIntervals";
            this.checkUniqueIntervals.Size = new System.Drawing.Size(147, 19);
            this.checkUniqueIntervals.TabIndex = 8;
            this.checkUniqueIntervals.Text = "Unique length classes";
            this.checkUniqueIntervals.UseVisualStyleBackColor = true;
            this.checkUniqueIntervals.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkUniqueIntervals);
            this.groupBox1.Controls.Add(this.textIntervalSize);
            this.groupBox1.Controls.Add(this.checkUseSize);
            this.groupBox1.Location = new System.Drawing.Point(4, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 76);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // buttonRemove
            // 
            this.buttonRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemove.Location = new System.Drawing.Point(250, 173);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(35, 28);
            this.buttonRemove.TabIndex = 11;
            this.buttonRemove.Text = "-";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.OnButtonClick);
            this.buttonRemove.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnButtonMouseDown);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAdd.Location = new System.Drawing.Point(250, 137);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(35, 28);
            this.buttonAdd.TabIndex = 10;
            this.buttonAdd.Text = "+";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblTotalFreq
            // 
            this.lblTotalFreq.AutoSize = true;
            this.lblTotalFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalFreq.Location = new System.Drawing.Point(2, 454);
            this.lblTotalFreq.Name = "lblTotalFreq";
            this.lblTotalFreq.Size = new System.Drawing.Size(125, 15);
            this.lblTotalFreq.TabIndex = 12;
            this.lblTotalFreq.Text = "Total of frequency:";
            // 
            // lblLen
            // 
            this.lblLen.AutoSize = true;
            this.lblLen.Location = new System.Drawing.Point(54, 131);
            this.lblLen.Name = "lblLen";
            this.lblLen.Size = new System.Drawing.Size(45, 15);
            this.lblLen.TabIndex = 13;
            this.lblLen.Text = "Length";
            // 
            // lblFreq
            // 
            this.lblFreq.AutoSize = true;
            this.lblFreq.Location = new System.Drawing.Point(139, 131);
            this.lblFreq.Name = "lblFreq";
            this.lblFreq.Size = new System.Drawing.Size(64, 15);
            this.lblFreq.TabIndex = 14;
            this.lblFreq.Text = "Frequency";
            // 
            // lblCatchCompCount
            // 
            this.lblCatchCompCount.AutoSize = true;
            this.lblCatchCompCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCatchCompCount.Location = new System.Drawing.Point(2, 432);
            this.lblCatchCompCount.Name = "lblCatchCompCount";
            this.lblCatchCompCount.Size = new System.Drawing.Size(129, 15);
            this.lblCatchCompCount.TabIndex = 15;
            this.lblCatchCompCount.Text = "Catch comp. count:";
            // 
            // LengthFreqForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(300, 476);
            this.Controls.Add(this.lblCatchCompCount);
            this.Controls.Add(this.lblFreq);
            this.Controls.Add(this.lblLen);
            this.Controls.Add(this.lblTotalFreq);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panelUI);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LengthFreqForm";
            this.ShowInTaskbar = false;
            this.Text = "Length frequency";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelUI;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.CheckBox checkUseSize;
        private System.Windows.Forms.TextBox textIntervalSize;
        private System.Windows.Forms.CheckBox checkUniqueIntervals;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Label lblTotalFreq;
        private System.Windows.Forms.Label lblLen;
        private System.Windows.Forms.Label lblFreq;
        private System.Windows.Forms.Label lblCatchCompCount;
    }
}