namespace FAD3.Database.Forms
{
    partial class FishingOperationCostsForm
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCostOfFishing = new System.Windows.Forms.TextBox();
            this.txtROI = new System.Windows.Forms.TextBox();
            this.txtIncomeSales = new System.Windows.Forms.TextBox();
            this.txtWeightConsumed = new System.Windows.Forms.TextBox();
            this.lvExpenseItems = new System.Windows.Forms.ListView();
            this.colCostItemName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCostItemCost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label5 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.colCostItemUnit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCostItemUnitQuantity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(394, 374);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(45, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(335, 374);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(53, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Cost of fishing";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Return of investment";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Income from fish sold";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(161, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Weight of catch for consumption";
            // 
            // txtCostOfFishing
            // 
            this.txtCostOfFishing.Location = new System.Drawing.Point(205, 42);
            this.txtCostOfFishing.Name = "txtCostOfFishing";
            this.txtCostOfFishing.Size = new System.Drawing.Size(120, 20);
            this.txtCostOfFishing.TabIndex = 6;
            this.txtCostOfFishing.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidating);
            // 
            // txtROI
            // 
            this.txtROI.Location = new System.Drawing.Point(205, 72);
            this.txtROI.Name = "txtROI";
            this.txtROI.Size = new System.Drawing.Size(120, 20);
            this.txtROI.TabIndex = 7;
            this.txtROI.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidating);
            // 
            // txtIncomeSales
            // 
            this.txtIncomeSales.Location = new System.Drawing.Point(205, 107);
            this.txtIncomeSales.Name = "txtIncomeSales";
            this.txtIncomeSales.Size = new System.Drawing.Size(120, 20);
            this.txtIncomeSales.TabIndex = 8;
            this.txtIncomeSales.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidating);
            // 
            // txtWeightConsumed
            // 
            this.txtWeightConsumed.Location = new System.Drawing.Point(205, 138);
            this.txtWeightConsumed.Name = "txtWeightConsumed";
            this.txtWeightConsumed.Size = new System.Drawing.Size(120, 20);
            this.txtWeightConsumed.TabIndex = 9;
            this.txtWeightConsumed.Validating += new System.ComponentModel.CancelEventHandler(this.OnFieldValidating);
            // 
            // lvExpenseItems
            // 
            this.lvExpenseItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCostItemName,
            this.colCostItemCost,
            this.colCostItemUnit,
            this.colCostItemUnitQuantity});
            this.lvExpenseItems.FullRowSelect = true;
            this.lvExpenseItems.Location = new System.Drawing.Point(27, 202);
            this.lvExpenseItems.Name = "lvExpenseItems";
            this.lvExpenseItems.Size = new System.Drawing.Size(412, 153);
            this.lvExpenseItems.TabIndex = 10;
            this.lvExpenseItems.UseCompatibleStateImageBehavior = false;
            this.lvExpenseItems.View = System.Windows.Forms.View.Details;
            this.lvExpenseItems.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // colCostItemName
            // 
            this.colCostItemName.Text = "Cost item";
            this.colCostItemName.Width = 195;
            // 
            // colCostItemCost
            // 
            this.colCostItemCost.Text = "Cost (Pesos)";
            this.colCostItemCost.Width = 81;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Cost items";
            // 
            // btnRemove
            // 
            this.btnRemove.Image = global::FAD3.Properties.Resources.removel_l;
            this.btnRemove.Location = new System.Drawing.Point(445, 233);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(25, 25);
            this.btnRemove.TabIndex = 12;
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::FAD3.Properties.Resources.add_l;
            this.btnAdd.Location = new System.Drawing.Point(445, 202);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(25, 25);
            this.btnAdd.TabIndex = 13;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(26, 374);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(53, 25);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // colCostItemUnit
            // 
            this.colCostItemUnit.Text = "Unit";
            // 
            // colCostItemUnitQuantity
            // 
            this.colCostItemUnitQuantity.Text = "# of units";
            // 
            // FishingOperationCostsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 411);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lvExpenseItems);
            this.Controls.Add(this.txtWeightConsumed);
            this.Controls.Add(this.txtIncomeSales);
            this.Controls.Add(this.txtROI);
            this.Controls.Add(this.txtCostOfFishing);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FishingOperationCostsForm";
            this.Text = "Cost of fishing operation";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCostOfFishing;
        private System.Windows.Forms.TextBox txtROI;
        private System.Windows.Forms.TextBox txtIncomeSales;
        private System.Windows.Forms.TextBox txtWeightConsumed;
        private System.Windows.Forms.ListView lvExpenseItems;
        private System.Windows.Forms.ColumnHeader colCostItemName;
        private System.Windows.Forms.ColumnHeader colCostItemCost;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ColumnHeader colCostItemUnit;
        private System.Windows.Forms.ColumnHeader colCostItemUnitQuantity;
    }
}