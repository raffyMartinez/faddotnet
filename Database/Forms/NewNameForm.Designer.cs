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
            this.lblNewType = new System.Windows.Forms.Label();
            this.txtLocalName = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.listBoxSimilar = new System.Windows.Forms.ListBox();
            this.lblSimilar = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblNewType
            // 
            this.lblNewType.AutoSize = true;
            this.lblNewType.Location = new System.Drawing.Point(5, 31);
            this.lblNewType.Name = "lblNewType";
            this.lblNewType.Size = new System.Drawing.Size(83, 13);
            this.lblNewType.TabIndex = 0;
            this.lblNewType.Text = "New local name";
            // 
            // txtLocalName
            // 
            this.txtLocalName.Location = new System.Drawing.Point(4, 47);
            this.txtLocalName.Name = "txtLocalName";
            this.txtLocalName.Size = new System.Drawing.Size(224, 20);
            this.txtLocalName.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(181, 189);
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
            this.btnCancel.Location = new System.Drawing.Point(116, 189);
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
            this.listBoxSimilar.Location = new System.Drawing.Point(4, 94);
            this.listBoxSimilar.Name = "listBoxSimilar";
            this.listBoxSimilar.Size = new System.Drawing.Size(224, 82);
            this.listBoxSimilar.TabIndex = 4;
            this.listBoxSimilar.Visible = false;
            this.listBoxSimilar.DoubleClick += new System.EventHandler(this.OnListDblClick);
            // 
            // lblSimilar
            // 
            this.lblSimilar.AutoSize = true;
            this.lblSimilar.Location = new System.Drawing.Point(3, 78);
            this.lblSimilar.Name = "lblSimilar";
            this.lblSimilar.Size = new System.Drawing.Size(71, 13);
            this.lblSimilar.TabIndex = 5;
            this.lblSimilar.Text = "Similar names";
            this.lblSimilar.Visible = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(6, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(186, 15);
            this.lblTitle.TabIndex = 6;
            this.lblTitle.Text = "Add a name to the database";
            // 
            // NewNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 222);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSimilar);
            this.Controls.Add(this.listBoxSimilar);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtLocalName);
            this.Controls.Add(this.lblNewType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewNameForm";
            this.Text = "NewCatchLocalNameForm";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNewType;
        private System.Windows.Forms.TextBox txtLocalName;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox listBoxSimilar;
        private System.Windows.Forms.Label lblSimilar;
        private System.Windows.Forms.Label lblTitle;
    }
}