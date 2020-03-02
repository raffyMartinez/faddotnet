namespace FAD3.Database.Forms
{
    partial class ViewGearSpec_ExpensesForm
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
            this.txtGearSpecs = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtExpenses = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblRefNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtGearSpecs
            // 
            this.txtGearSpecs.Location = new System.Drawing.Point(-1, 59);
            this.txtGearSpecs.Multiline = true;
            this.txtGearSpecs.Name = "txtGearSpecs";
            this.txtGearSpecs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtGearSpecs.Size = new System.Drawing.Size(303, 154);
            this.txtGearSpecs.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Gear specifications";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 237);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Fishing expenses";
            // 
            // txtExpenses
            // 
            this.txtExpenses.Location = new System.Drawing.Point(-1, 253);
            this.txtExpenses.Multiline = true;
            this.txtExpenses.Name = "txtExpenses";
            this.txtExpenses.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtExpenses.Size = new System.Drawing.Size(303, 154);
            this.txtExpenses.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(252, 419);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(47, 22);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // lblRefNumber
            // 
            this.lblRefNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRefNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRefNumber.Location = new System.Drawing.Point(2, 9);
            this.lblRefNumber.Name = "lblRefNumber";
            this.lblRefNumber.Size = new System.Drawing.Size(300, 25);
            this.lblRefNumber.TabIndex = 5;
            this.lblRefNumber.Text = "Reference no:";
            this.lblRefNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ViewGearSpec_ExpensesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 451);
            this.Controls.Add(this.lblRefNumber);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtExpenses);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtGearSpecs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ViewGearSpec_ExpensesForm";
            this.Text = "Gear specifications and fishing expenses";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtGearSpecs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtExpenses;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblRefNumber;
    }
}