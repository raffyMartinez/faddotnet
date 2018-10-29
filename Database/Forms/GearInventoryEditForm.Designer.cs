namespace FAD3.Database.Forms
{
    partial class GearInventoryEditForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtInventoryName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDateImplemented = new System.Windows.Forms.TextBox();
            this.pnlInventory = new System.Windows.Forms.Panel();
            this.pnlBarangay = new System.Windows.Forms.Panel();
            this.comboBarangays = new System.Windows.Forms.ComboBox();
            this.txtCountCommercial = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCountNonMotorized = new System.Windows.Forms.TextBox();
            this.txtCountMotorized = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCountFishers = new System.Windows.Forms.TextBox();
            this.txtSitio = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboMunicipality = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboProvince = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pnlInventory.SuspendLayout();
            this.pnlBarangay.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(310, 462);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(55, 26);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(249, 462);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 26);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // txtInventoryName
            // 
            this.txtInventoryName.Location = new System.Drawing.Point(83, 32);
            this.txtInventoryName.Name = "txtInventoryName";
            this.txtInventoryName.Size = new System.Drawing.Size(262, 20);
            this.txtInventoryName.TabIndex = 0;
            this.txtInventoryName.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextBoxValidating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date start";
            // 
            // txtDateImplemented
            // 
            this.txtDateImplemented.Location = new System.Drawing.Point(83, 71);
            this.txtDateImplemented.Name = "txtDateImplemented";
            this.txtDateImplemented.Size = new System.Drawing.Size(105, 20);
            this.txtDateImplemented.TabIndex = 3;
            this.txtDateImplemented.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextBoxValidating);
            // 
            // pnlInventory
            // 
            this.pnlInventory.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlInventory.Controls.Add(this.txtInventoryName);
            this.pnlInventory.Controls.Add(this.txtDateImplemented);
            this.pnlInventory.Controls.Add(this.label1);
            this.pnlInventory.Controls.Add(this.label2);
            this.pnlInventory.Location = new System.Drawing.Point(2, 26);
            this.pnlInventory.Name = "pnlInventory";
            this.pnlInventory.Size = new System.Drawing.Size(373, 108);
            this.pnlInventory.TabIndex = 4;
            this.pnlInventory.Visible = false;
            // 
            // pnlBarangay
            // 
            this.pnlBarangay.Controls.Add(this.comboBarangays);
            this.pnlBarangay.Controls.Add(this.groupBox1);
            this.pnlBarangay.Controls.Add(this.label7);
            this.pnlBarangay.Controls.Add(this.txtCountFishers);
            this.pnlBarangay.Controls.Add(this.txtSitio);
            this.pnlBarangay.Controls.Add(this.label6);
            this.pnlBarangay.Controls.Add(this.label5);
            this.pnlBarangay.Controls.Add(this.label4);
            this.pnlBarangay.Controls.Add(this.comboMunicipality);
            this.pnlBarangay.Controls.Add(this.label3);
            this.pnlBarangay.Controls.Add(this.comboProvince);
            this.pnlBarangay.Location = new System.Drawing.Point(2, 140);
            this.pnlBarangay.Name = "pnlBarangay";
            this.pnlBarangay.Size = new System.Drawing.Size(373, 316);
            this.pnlBarangay.TabIndex = 5;
            this.pnlBarangay.Visible = false;
            // 
            // comboBarangays
            // 
            this.comboBarangays.FormattingEnabled = true;
            this.comboBarangays.Location = new System.Drawing.Point(83, 70);
            this.comboBarangays.Name = "comboBarangays";
            this.comboBarangays.Size = new System.Drawing.Size(161, 21);
            this.comboBarangays.TabIndex = 16;
            this.comboBarangays.Validating += new System.ComponentModel.CancelEventHandler(this.OnComboValidating);
            // 
            // txtCountCommercial
            // 
            this.txtCountCommercial.Location = new System.Drawing.Point(75, 30);
            this.txtCountCommercial.Name = "txtCountCommercial";
            this.txtCountCommercial.Size = new System.Drawing.Size(68, 20);
            this.txtCountCommercial.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtCountCommercial);
            this.groupBox1.Controls.Add(this.txtCountNonMotorized);
            this.groupBox1.Controls.Add(this.txtCountMotorized);
            this.groupBox1.Location = new System.Drawing.Point(17, 177);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(343, 105);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fishing vessel numbers";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(174, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Non-motorized ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Motorized ";
            // 
            // txtCountNonMotorized
            // 
            this.txtCountNonMotorized.Location = new System.Drawing.Point(256, 66);
            this.txtCountNonMotorized.Name = "txtCountNonMotorized";
            this.txtCountNonMotorized.Size = new System.Drawing.Size(68, 20);
            this.txtCountNonMotorized.TabIndex = 14;
            // 
            // txtCountMotorized
            // 
            this.txtCountMotorized.Location = new System.Drawing.Point(75, 66);
            this.txtCountMotorized.Name = "txtCountMotorized";
            this.txtCountMotorized.Size = new System.Drawing.Size(68, 20);
            this.txtCountMotorized.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(14, 131);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 26);
            this.label7.TabIndex = 11;
            this.label7.Text = "Number of fishers";
            // 
            // txtCountFishers
            // 
            this.txtCountFishers.Location = new System.Drawing.Point(83, 133);
            this.txtCountFishers.Name = "txtCountFishers";
            this.txtCountFishers.Size = new System.Drawing.Size(68, 20);
            this.txtCountFishers.TabIndex = 10;
            // 
            // txtSitio
            // 
            this.txtSitio.Location = new System.Drawing.Point(83, 97);
            this.txtSitio.Name = "txtSitio";
            this.txtSitio.Size = new System.Drawing.Size(161, 20);
            this.txtSitio.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Sitio";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Barangay";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Municipality";
            // 
            // comboMunicipality
            // 
            this.comboMunicipality.FormattingEnabled = true;
            this.comboMunicipality.Location = new System.Drawing.Point(83, 44);
            this.comboMunicipality.Name = "comboMunicipality";
            this.comboMunicipality.Size = new System.Drawing.Size(161, 21);
            this.comboMunicipality.TabIndex = 3;
            this.comboMunicipality.SelectedIndexChanged += new System.EventHandler(this.OnComboSelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Province";
            // 
            // comboProvince
            // 
            this.comboProvince.FormattingEnabled = true;
            this.comboProvince.Location = new System.Drawing.Point(83, 17);
            this.comboProvince.Name = "comboProvince";
            this.comboProvince.Size = new System.Drawing.Size(161, 21);
            this.comboProvince.TabIndex = 0;
            this.comboProvince.SelectedIndexChanged += new System.EventHandler(this.OnComboSelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Commercial";
            // 
            // GearInventoryEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 500);
            this.Controls.Add(this.pnlBarangay);
            this.Controls.Add(this.pnlInventory);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GearInventoryEditForm";
            this.Text = "GearInventoryEditForm";
            this.pnlInventory.ResumeLayout(false);
            this.pnlInventory.PerformLayout();
            this.pnlBarangay.ResumeLayout(false);
            this.pnlBarangay.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtDateImplemented;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInventoryName;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlInventory;
        private System.Windows.Forms.Panel pnlBarangay;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCountFishers;
        private System.Windows.Forms.TextBox txtSitio;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboMunicipality;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboProvince;
        private System.Windows.Forms.TextBox txtCountCommercial;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCountNonMotorized;
        private System.Windows.Forms.TextBox txtCountMotorized;
        private System.Windows.Forms.ComboBox comboBarangays;
        private System.Windows.Forms.Label label10;
    }
}