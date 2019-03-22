namespace FAD3.Mapping.Forms
{
    partial class ShapefileClassificationSchemeForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabEqualInterval = new System.Windows.Forms.TabPage();
            this.txtIntervals = new System.Windows.Forms.TextBox();
            this.btnMakeIntervals = new System.Windows.Forms.Button();
            this.lblMaxValue = new System.Windows.Forms.Label();
            this.lblMinValue = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnEndTarget = new System.Windows.Forms.RadioButton();
            this.txtEndTarget = new System.Windows.Forms.TextBox();
            this.rbtnNumberOfSteps = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStartValue = new System.Windows.Forms.TextBox();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.txtNumberOfSteps = new System.Windows.Forms.TextBox();
            this.tabUserDefined = new System.Windows.Forms.TabPage();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabEqualInterval.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabEqualInterval);
            this.tabControl.Controls.Add(this.tabUserDefined);
            this.tabControl.Location = new System.Drawing.Point(0, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(334, 334);
            this.tabControl.TabIndex = 0;
            // 
            // tabEqualInterval
            // 
            this.tabEqualInterval.Controls.Add(this.txtIntervals);
            this.tabEqualInterval.Controls.Add(this.btnMakeIntervals);
            this.tabEqualInterval.Controls.Add(this.lblMaxValue);
            this.tabEqualInterval.Controls.Add(this.lblMinValue);
            this.tabEqualInterval.Controls.Add(this.groupBox1);
            this.tabEqualInterval.Location = new System.Drawing.Point(4, 22);
            this.tabEqualInterval.Name = "tabEqualInterval";
            this.tabEqualInterval.Padding = new System.Windows.Forms.Padding(3);
            this.tabEqualInterval.Size = new System.Drawing.Size(326, 308);
            this.tabEqualInterval.TabIndex = 0;
            this.tabEqualInterval.Text = "Equal interval";
            this.tabEqualInterval.UseVisualStyleBackColor = true;
            // 
            // txtIntervals
            // 
            this.txtIntervals.Location = new System.Drawing.Point(214, 68);
            this.txtIntervals.Multiline = true;
            this.txtIntervals.Name = "txtIntervals";
            this.txtIntervals.Size = new System.Drawing.Size(92, 228);
            this.txtIntervals.TabIndex = 7;
            // 
            // btnMakeIntervals
            // 
            this.btnMakeIntervals.Image = global::FAD3.Properties.Resources.right_arrow;
            this.btnMakeIntervals.Location = new System.Drawing.Point(173, 174);
            this.btnMakeIntervals.Name = "btnMakeIntervals";
            this.btnMakeIntervals.Size = new System.Drawing.Size(24, 24);
            this.btnMakeIntervals.TabIndex = 6;
            this.btnMakeIntervals.UseVisualStyleBackColor = true;
            this.btnMakeIntervals.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblMaxValue
            // 
            this.lblMaxValue.AutoSize = true;
            this.lblMaxValue.Location = new System.Drawing.Point(17, 38);
            this.lblMaxValue.Name = "lblMaxValue";
            this.lblMaxValue.Size = new System.Drawing.Size(83, 13);
            this.lblMaxValue.TabIndex = 5;
            this.lblMaxValue.Text = "Maximum value:";
            // 
            // lblMinValue
            // 
            this.lblMinValue.AutoSize = true;
            this.lblMinValue.Location = new System.Drawing.Point(17, 16);
            this.lblMinValue.Name = "lblMinValue";
            this.lblMinValue.Size = new System.Drawing.Size(80, 13);
            this.lblMinValue.TabIndex = 4;
            this.lblMinValue.Text = "Minimum value:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnEndTarget);
            this.groupBox1.Controls.Add(this.txtEndTarget);
            this.groupBox1.Controls.Add(this.rbtnNumberOfSteps);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtStartValue);
            this.groupBox1.Controls.Add(this.txtSize);
            this.groupBox1.Controls.Add(this.txtNumberOfSteps);
            this.groupBox1.Location = new System.Drawing.Point(11, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(148, 240);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Create intervals";
            // 
            // rbtnEndTarget
            // 
            this.rbtnEndTarget.AutoSize = true;
            this.rbtnEndTarget.Location = new System.Drawing.Point(10, 188);
            this.rbtnEndTarget.Name = "rbtnEndTarget";
            this.rbtnEndTarget.Size = new System.Drawing.Size(70, 17);
            this.rbtnEndTarget.TabIndex = 10;
            this.rbtnEndTarget.TabStop = true;
            this.rbtnEndTarget.Text = "Ending at";
            this.rbtnEndTarget.UseVisualStyleBackColor = true;
            this.rbtnEndTarget.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // txtEndTarget
            // 
            this.txtEndTarget.Location = new System.Drawing.Point(29, 208);
            this.txtEndTarget.Name = "txtEndTarget";
            this.txtEndTarget.Size = new System.Drawing.Size(79, 20);
            this.txtEndTarget.TabIndex = 9;
            this.txtEndTarget.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextValidating);
            // 
            // rbtnNumberOfSteps
            // 
            this.rbtnNumberOfSteps.AutoSize = true;
            this.rbtnNumberOfSteps.Location = new System.Drawing.Point(10, 132);
            this.rbtnNumberOfSteps.Name = "rbtnNumberOfSteps";
            this.rbtnNumberOfSteps.Size = new System.Drawing.Size(116, 17);
            this.rbtnNumberOfSteps.TabIndex = 3;
            this.rbtnNumberOfSteps.TabStop = true;
            this.rbtnNumberOfSteps.Text = "Number of intervals";
            this.rbtnNumberOfSteps.UseVisualStyleBackColor = true;
            this.rbtnNumberOfSteps.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Interval size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Starting from";
            // 
            // txtStartValue
            // 
            this.txtStartValue.Location = new System.Drawing.Point(29, 49);
            this.txtStartValue.Name = "txtStartValue";
            this.txtStartValue.Size = new System.Drawing.Size(79, 20);
            this.txtStartValue.TabIndex = 4;
            this.txtStartValue.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextValidating);
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(29, 98);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(79, 20);
            this.txtSize.TabIndex = 3;
            this.txtSize.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextValidating);
            // 
            // txtNumberOfSteps
            // 
            this.txtNumberOfSteps.Location = new System.Drawing.Point(29, 152);
            this.txtNumberOfSteps.Name = "txtNumberOfSteps";
            this.txtNumberOfSteps.Size = new System.Drawing.Size(79, 20);
            this.txtNumberOfSteps.TabIndex = 2;
            this.txtNumberOfSteps.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextValidating);
            // 
            // tabUserDefined
            // 
            this.tabUserDefined.Location = new System.Drawing.Point(4, 22);
            this.tabUserDefined.Name = "tabUserDefined";
            this.tabUserDefined.Padding = new System.Windows.Forms.Padding(3);
            this.tabUserDefined.Size = new System.Drawing.Size(326, 308);
            this.tabUserDefined.TabIndex = 1;
            this.tabUserDefined.Text = "User defined";
            this.tabUserDefined.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(286, 352);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(44, 25);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(229, 352);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(51, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // ShapefileClassificationSchemeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 384);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ShapefileClassificationSchemeForm";
            this.Text = "ShapefileClassificationSchemeForm";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.tabControl.ResumeLayout(false);
            this.tabEqualInterval.ResumeLayout(false);
            this.tabEqualInterval.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabEqualInterval;
        private System.Windows.Forms.TabPage tabUserDefined;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblMaxValue;
        private System.Windows.Forms.Label lblMinValue;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.TextBox txtNumberOfSteps;
        private System.Windows.Forms.Button btnMakeIntervals;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStartValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbtnEndTarget;
        private System.Windows.Forms.TextBox txtEndTarget;
        private System.Windows.Forms.RadioButton rbtnNumberOfSteps;
        private System.Windows.Forms.TextBox txtIntervals;
    }
}