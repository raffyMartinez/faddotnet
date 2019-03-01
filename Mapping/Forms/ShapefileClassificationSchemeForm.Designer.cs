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
            this.tabUserDefined = new System.Windows.Forms.TabPage();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tabControl.SuspendLayout();
            this.tabEqualInterval.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabEqualInterval);
            this.tabControl.Controls.Add(this.tabUserDefined);
            this.tabControl.Location = new System.Drawing.Point(0, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(334, 211);
            this.tabControl.TabIndex = 0;
            // 
            // tabEqualInterval
            // 
            this.tabEqualInterval.Controls.Add(this.listView1);
            this.tabEqualInterval.Controls.Add(this.label1);
            this.tabEqualInterval.Controls.Add(this.textBox1);
            this.tabEqualInterval.Location = new System.Drawing.Point(4, 22);
            this.tabEqualInterval.Name = "tabEqualInterval";
            this.tabEqualInterval.Padding = new System.Windows.Forms.Padding(3);
            this.tabEqualInterval.Size = new System.Drawing.Size(326, 185);
            this.tabEqualInterval.TabIndex = 0;
            this.tabEqualInterval.Text = "Equal interval";
            this.tabEqualInterval.UseVisualStyleBackColor = true;
            // 
            // tabUserDefined
            // 
            this.tabUserDefined.Location = new System.Drawing.Point(4, 22);
            this.tabUserDefined.Name = "tabUserDefined";
            this.tabUserDefined.Padding = new System.Windows.Forms.Padding(3);
            this.tabUserDefined.Size = new System.Drawing.Size(326, 185);
            this.tabUserDefined.TabIndex = 1;
            this.tabUserDefined.Text = "User defined";
            this.tabUserDefined.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(278, 229);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(44, 25);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(221, 229);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(51, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(108, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(40, 20);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number of classes";
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(11, 47);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(307, 132);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // ShapefileClassificationSchemeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 262);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabEqualInterval;
        private System.Windows.Forms.TabPage tabUserDefined;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
    }
}