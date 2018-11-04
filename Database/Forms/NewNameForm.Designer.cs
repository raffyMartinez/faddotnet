namespace FAD3.Database.Forms
{
    partial class NewNameForm
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
            this.txtLocalName = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.listBoxSimilar = new System.Windows.Forms.ListBox();
            this.lblSimilar = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "New local name";
            // 
            // txtLocalName
            // 
            this.txtLocalName.Location = new System.Drawing.Point(15, 40);
            this.txtLocalName.Name = "txtLocalName";
            this.txtLocalName.Size = new System.Drawing.Size(172, 20);
            this.txtLocalName.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(140, 186);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(47, 24);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Location = new System.Drawing.Point(75, 186);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 24);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // listBoxSimilar
            // 
            this.listBoxSimilar.FormattingEnabled = true;
            this.listBoxSimilar.Location = new System.Drawing.Point(15, 88);
            this.listBoxSimilar.Name = "listBoxSimilar";
            this.listBoxSimilar.Size = new System.Drawing.Size(172, 82);
            this.listBoxSimilar.TabIndex = 4;
            this.listBoxSimilar.Visible = false;
            this.listBoxSimilar.DoubleClick += new System.EventHandler(this.OnListDblClick);
            // 
            // lblSimilar
            // 
            this.lblSimilar.AutoSize = true;
            this.lblSimilar.Location = new System.Drawing.Point(12, 72);
            this.lblSimilar.Name = "lblSimilar";
            this.lblSimilar.Size = new System.Drawing.Size(71, 13);
            this.lblSimilar.TabIndex = 5;
            this.lblSimilar.Text = "Similar names";
            this.lblSimilar.Visible = false;
            // 
            // NewLocalNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(197, 222);
            this.Controls.Add(this.lblSimilar);
            this.Controls.Add(this.listBoxSimilar);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtLocalName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewLocalNameForm";
            this.Text = "NewCatchLocalNameForm";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLocalName;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox listBoxSimilar;
        private System.Windows.Forms.Label lblSimilar;
    }
}