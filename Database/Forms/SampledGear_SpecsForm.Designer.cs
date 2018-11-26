namespace FAD3
{
    partial class SampledGear_SpecsForm
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
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.panelTip = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelUI
            // 
            this.panelUI.AutoScroll = true;
            this.panelUI.Location = new System.Drawing.Point(1, 56);
            this.panelUI.Margin = new System.Windows.Forms.Padding(4);
            this.panelUI.Name = "panelUI";
            this.panelUI.Size = new System.Drawing.Size(343, 319);
            this.panelUI.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(293, 391);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(44, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(233, 391);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(55, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.CausesValidation = false;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(1, 8);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(336, 44);
            this.labelTitle.TabIndex = 3;
            this.labelTitle.Text = "label1";
            // 
            // statusBar
            // 
            this.statusBar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.panelTip});
            this.statusBar.Location = new System.Drawing.Point(0, 427);
            this.statusBar.Name = "statusBar";
            this.statusBar.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusBar.Size = new System.Drawing.Size(347, 22);
            this.statusBar.TabIndex = 4;
            this.statusBar.Text = "statusStrip1";
            // 
            // panelTip
            // 
            this.panelTip.Name = "panelTip";
            this.panelTip.Size = new System.Drawing.Size(131, 17);
            this.panelTip.Text = "toolStripStatusLabel1";
            // 
            // SampledGear_SpecsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(347, 449);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.panelUI);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SampledGear_SpecsForm";
            this.ShowInTaskbar = false;
            this.Text = "SampledGear_SpecsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnSpecsForm_FormClosed);
            this.Load += new System.EventHandler(this.OnSpecsForm_Load);
            this.Click += new System.EventHandler(this.Onbutton_Click);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelUI;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel panelTip;
    }
}