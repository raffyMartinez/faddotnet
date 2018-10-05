namespace FAD3
{
    partial class VisibilityQueryForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.chkShowValues = new System.Windows.Forms.CheckBox();
            this.btnShowValues = new System.Windows.Forms.Button();
            this.chkShowDynamically = new System.Windows.Forms.CheckBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textQuery = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvValues = new System.Windows.Forms.DataGridView();
            this.cmnCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxFields = new System.Windows.Forms.ListBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNot = new System.Windows.Forms.Button();
            this.btnOr = new System.Windows.Forms.Button();
            this.btnLP = new System.Windows.Forms.Button();
            this.btnAnd = new System.Windows.Forms.Button();
            this.btnNE = new System.Windows.Forms.Button();
            this.btnLT = new System.Windows.Forms.Button();
            this.btnE = new System.Windows.Forms.Button();
            this.btnLTE = new System.Windows.Forms.Button();
            this.btnGT = new System.Windows.Forms.Button();
            this.btnGTE = new System.Windows.Forms.Button();
            this.btnRP = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkShowValues
            // 
            this.chkShowValues.AutoSize = true;
            this.chkShowValues.Location = new System.Drawing.Point(483, 200);
            this.chkShowValues.Name = "chkShowValues";
            this.chkShowValues.Size = new System.Drawing.Size(87, 17);
            this.chkShowValues.TabIndex = 56;
            this.chkShowValues.Text = "Show values";
            this.chkShowValues.UseVisualStyleBackColor = true;
            // 
            // btnShowValues
            // 
            this.btnShowValues.Location = new System.Drawing.Point(482, 161);
            this.btnShowValues.Name = "btnShowValues";
            this.btnShowValues.Size = new System.Drawing.Size(95, 23);
            this.btnShowValues.TabIndex = 54;
            this.btnShowValues.Text = "Get values";
            this.btnShowValues.UseVisualStyleBackColor = true;
            this.btnShowValues.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // chkShowDynamically
            // 
            this.chkShowDynamically.AutoSize = true;
            this.chkShowDynamically.Location = new System.Drawing.Point(483, 232);
            this.chkShowDynamically.Name = "chkShowDynamically";
            this.chkShowDynamically.Size = new System.Drawing.Size(106, 17);
            this.chkShowDynamically.TabIndex = 50;
            this.chkShowDynamically.Text = "Update selection";
            this.chkShowDynamically.UseVisualStyleBackColor = true;
            // 
            // lblResult
            // 
            this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblResult.Location = new System.Drawing.Point(8, 356);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(368, 19);
            this.lblResult.TabIndex = 49;
            this.lblResult.Text = "Results";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textQuery);
            this.groupBox3.Location = new System.Drawing.Point(8, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(469, 101);
            this.groupBox3.TabIndex = 53;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Expression";
            // 
            // textQuery
            // 
            this.textQuery.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textQuery.Location = new System.Drawing.Point(3, 16);
            this.textQuery.Name = "textQuery";
            this.textQuery.Size = new System.Drawing.Size(463, 82);
            this.textQuery.TabIndex = 15;
            this.textQuery.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvValues);
            this.groupBox2.Location = new System.Drawing.Point(228, 145);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(249, 198);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Values";
            // 
            // dgvValues
            // 
            this.dgvValues.AllowUserToAddRows = false;
            this.dgvValues.AllowUserToDeleteRows = false;
            this.dgvValues.AllowUserToResizeColumns = false;
            this.dgvValues.AllowUserToResizeRows = false;
            this.dgvValues.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvValues.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvValues.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.dgvValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvValues.ColumnHeadersVisible = false;
            this.dgvValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cmnCount,
            this.cmnValue});
            this.dgvValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvValues.Location = new System.Drawing.Point(3, 16);
            this.dgvValues.MultiSelect = false;
            this.dgvValues.Name = "dgvValues";
            this.dgvValues.ReadOnly = true;
            this.dgvValues.RowHeadersVisible = false;
            this.dgvValues.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvValues.Size = new System.Drawing.Size(243, 179);
            this.dgvValues.TabIndex = 24;
            this.dgvValues.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OndgvValues_CellDoubleClick);
            // 
            // cmnCount
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            this.cmnCount.DefaultCellStyle = dataGridViewCellStyle2;
            this.cmnCount.HeaderText = "Count";
            this.cmnCount.Name = "cmnCount";
            this.cmnCount.ReadOnly = true;
            this.cmnCount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cmnCount.Width = 35;
            // 
            // cmnValue
            // 
            this.cmnValue.HeaderText = "Value";
            this.cmnValue.Name = "cmnValue";
            this.cmnValue.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxFields);
            this.groupBox1.Location = new System.Drawing.Point(8, 145);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 198);
            this.groupBox1.TabIndex = 51;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fields";
            // 
            // listBoxFields
            // 
            this.listBoxFields.FormattingEnabled = true;
            this.listBoxFields.Location = new System.Drawing.Point(6, 16);
            this.listBoxFields.Name = "listBoxFields";
            this.listBoxFields.Size = new System.Drawing.Size(202, 173);
            this.listBoxFields.TabIndex = 0;
            this.listBoxFields.Click += new System.EventHandler(this.OnlistBoxFields_Click);
            this.listBoxFields.DoubleClick += new System.EventHandler(this.OnlistBoxFields_DoubleClick);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(382, 349);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(95, 26);
            this.btnOk.TabIndex = 48;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(483, 51);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(95, 26);
            this.btnClear.TabIndex = 55;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(483, 19);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(95, 26);
            this.btnTest.TabIndex = 46;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(484, 349);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 26);
            this.btnCancel.TabIndex = 47;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnNot
            // 
            this.btnNot.Location = new System.Drawing.Point(323, 117);
            this.btnNot.Name = "btnNot";
            this.btnNot.Size = new System.Drawing.Size(45, 25);
            this.btnNot.TabIndex = 43;
            this.btnNot.Tag = "";
            this.btnNot.Text = "NOT";
            this.btnNot.UseVisualStyleBackColor = true;
            this.btnNot.Click += new System.EventHandler(this.OnQueryButtonClick);
            // 
            // btnOr
            // 
            this.btnOr.Location = new System.Drawing.Point(282, 117);
            this.btnOr.Name = "btnOr";
            this.btnOr.Size = new System.Drawing.Size(35, 25);
            this.btnOr.TabIndex = 42;
            this.btnOr.Tag = "";
            this.btnOr.Text = "OR";
            this.btnOr.UseVisualStyleBackColor = true;
            this.btnOr.Click += new System.EventHandler(this.OnQueryButtonClick);
            // 
            // btnLP
            // 
            this.btnLP.Location = new System.Drawing.Point(384, 117);
            this.btnLP.Name = "btnLP";
            this.btnLP.Size = new System.Drawing.Size(18, 25);
            this.btnLP.TabIndex = 44;
            this.btnLP.Tag = "";
            this.btnLP.Text = "(";
            this.btnLP.UseVisualStyleBackColor = true;
            this.btnLP.Click += new System.EventHandler(this.OnQueryButtonClick);
            // 
            // btnAnd
            // 
            this.btnAnd.Location = new System.Drawing.Point(237, 117);
            this.btnAnd.Name = "btnAnd";
            this.btnAnd.Size = new System.Drawing.Size(39, 25);
            this.btnAnd.TabIndex = 41;
            this.btnAnd.Tag = "";
            this.btnAnd.Text = "AND";
            this.btnAnd.UseVisualStyleBackColor = true;
            this.btnAnd.Click += new System.EventHandler(this.OnQueryButtonClick);
            // 
            // btnNE
            // 
            this.btnNE.Location = new System.Drawing.Point(188, 117);
            this.btnNE.Name = "btnNE";
            this.btnNE.Size = new System.Drawing.Size(34, 25);
            this.btnNE.TabIndex = 40;
            this.btnNE.Tag = "";
            this.btnNE.Text = "<>";
            this.btnNE.UseVisualStyleBackColor = true;
            this.btnNE.Click += new System.EventHandler(this.OnQueryButtonClick);
            // 
            // btnLT
            // 
            this.btnLT.Location = new System.Drawing.Point(8, 117);
            this.btnLT.Name = "btnLT";
            this.btnLT.Size = new System.Drawing.Size(34, 25);
            this.btnLT.TabIndex = 38;
            this.btnLT.Tag = "";
            this.btnLT.Text = "<";
            this.btnLT.UseVisualStyleBackColor = true;
            this.btnLT.Click += new System.EventHandler(this.OnQueryButtonClick);
            // 
            // btnE
            // 
            this.btnE.Location = new System.Drawing.Point(152, 117);
            this.btnE.Name = "btnE";
            this.btnE.Size = new System.Drawing.Size(34, 25);
            this.btnE.TabIndex = 39;
            this.btnE.Tag = "";
            this.btnE.Text = "=";
            this.btnE.UseVisualStyleBackColor = true;
            this.btnE.Click += new System.EventHandler(this.OnQueryButtonClick);
            // 
            // btnLTE
            // 
            this.btnLTE.Location = new System.Drawing.Point(44, 117);
            this.btnLTE.Name = "btnLTE";
            this.btnLTE.Size = new System.Drawing.Size(34, 25);
            this.btnLTE.TabIndex = 37;
            this.btnLTE.Tag = "";
            this.btnLTE.Text = "<=";
            this.btnLTE.UseVisualStyleBackColor = true;
            this.btnLTE.Click += new System.EventHandler(this.OnQueryButtonClick);
            // 
            // btnGT
            // 
            this.btnGT.Location = new System.Drawing.Point(116, 117);
            this.btnGT.Name = "btnGT";
            this.btnGT.Size = new System.Drawing.Size(34, 25);
            this.btnGT.TabIndex = 35;
            this.btnGT.Tag = "";
            this.btnGT.Text = ">";
            this.btnGT.UseVisualStyleBackColor = true;
            this.btnGT.Click += new System.EventHandler(this.OnQueryButtonClick);
            // 
            // btnGTE
            // 
            this.btnGTE.Location = new System.Drawing.Point(80, 117);
            this.btnGTE.Name = "btnGTE";
            this.btnGTE.Size = new System.Drawing.Size(34, 25);
            this.btnGTE.TabIndex = 36;
            this.btnGTE.Tag = "";
            this.btnGTE.Text = ">=";
            this.btnGTE.UseVisualStyleBackColor = true;
            this.btnGTE.Click += new System.EventHandler(this.OnQueryButtonClick);
            // 
            // btnRP
            // 
            this.btnRP.Location = new System.Drawing.Point(408, 117);
            this.btnRP.Name = "btnRP";
            this.btnRP.Size = new System.Drawing.Size(19, 25);
            this.btnRP.TabIndex = 45;
            this.btnRP.Tag = "";
            this.btnRP.Text = ")";
            this.btnRP.UseVisualStyleBackColor = true;
            this.btnRP.Click += new System.EventHandler(this.OnQueryButtonClick);
            // 
            // VisibilityQueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 387);
            this.Controls.Add(this.chkShowValues);
            this.Controls.Add(this.btnShowValues);
            this.Controls.Add(this.chkShowDynamically);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNot);
            this.Controls.Add(this.btnOr);
            this.Controls.Add(this.btnLP);
            this.Controls.Add(this.btnAnd);
            this.Controls.Add(this.btnNE);
            this.Controls.Add(this.btnLT);
            this.Controls.Add(this.btnE);
            this.Controls.Add(this.btnLTE);
            this.Controls.Add(this.btnGT);
            this.Controls.Add(this.btnGTE);
            this.Controls.Add(this.btnRP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "VisibilityQueryForm";
            this.Text = "VisibilityQueryForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkShowValues;
        private System.Windows.Forms.Button btnShowValues;
        private System.Windows.Forms.CheckBox chkShowDynamically;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox textQuery;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnValue;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNot;
        private System.Windows.Forms.Button btnOr;
        private System.Windows.Forms.Button btnLP;
        private System.Windows.Forms.Button btnAnd;
        private System.Windows.Forms.Button btnNE;
        private System.Windows.Forms.Button btnLT;
        private System.Windows.Forms.Button btnE;
        private System.Windows.Forms.Button btnLTE;
        private System.Windows.Forms.Button btnGT;
        private System.Windows.Forms.Button btnGTE;
        private System.Windows.Forms.Button btnRP;
        private System.Windows.Forms.ListBox listBoxFields;
    }
}