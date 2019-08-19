namespace FAD3.Database.Forms
{
    partial class EditOperatingExpenseItemForm
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
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCost = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboSelection = new System.Windows.Forms.ComboBox();
            this.cboUnit = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUnitQuantity = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(223, 198);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(42, 28);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Expense item";
            // 
            // txtCost
            // 
            this.txtCost.Location = new System.Drawing.Point(109, 74);
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(156, 20);
            this.txtCost.TabIndex = 4;
            this.txtCost.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Cost";
            // 
            // btnCancel
            // 
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Location = new System.Drawing.Point(161, 198);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 28);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // cboSelection
            // 
            this.cboSelection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSelection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSelection.FormattingEnabled = true;
            this.cboSelection.Location = new System.Drawing.Point(109, 39);
            this.cboSelection.Name = "cboSelection";
            this.cboSelection.Size = new System.Drawing.Size(156, 21);
            this.cboSelection.Sorted = true;
            this.cboSelection.TabIndex = 6;
            this.cboSelection.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidating);
            // 
            // cboUnit
            // 
            this.cboUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboUnit.FormattingEnabled = true;
            this.cboUnit.Location = new System.Drawing.Point(109, 109);
            this.cboUnit.Name = "cboUnit";
            this.cboUnit.Size = new System.Drawing.Size(152, 21);
            this.cboUnit.Sorted = true;
            this.cboUnit.TabIndex = 8;
            this.cboUnit.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Unit";
            // 
            // txtUnitQuantity
            // 
            this.txtUnitQuantity.Location = new System.Drawing.Point(109, 146);
            this.txtUnitQuantity.Name = "txtUnitQuantity";
            this.txtUnitQuantity.Size = new System.Drawing.Size(152, 20);
            this.txtUnitQuantity.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Number of units";
            // 
            // EditOperatingExpenseItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 238);
            this.Controls.Add(this.txtUnitQuantity);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboUnit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboSelection);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtCost);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditOperatingExpenseItemForm";
            this.Text = "Edit expense item";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCost;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cboSelection;
        private System.Windows.Forms.ComboBox cboUnit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUnitQuantity;
        private System.Windows.Forms.Label label4;
    }
}