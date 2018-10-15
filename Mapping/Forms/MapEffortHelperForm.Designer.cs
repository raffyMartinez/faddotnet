namespace FAD3
{
    partial class MapEffortHelperForm
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
            this.lvYears = new System.Windows.Forms.ListView();
            this.colYear = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chkAggregate = new System.Windows.Forms.CheckBox();
            this.chkNotInclude1 = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvYears
            // 
            this.lvYears.CheckBoxes = true;
            this.lvYears.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colYear});
            this.lvYears.HideSelection = false;
            this.lvYears.Location = new System.Drawing.Point(10, 81);
            this.lvYears.Name = "lvYears";
            this.lvYears.Size = new System.Drawing.Size(140, 194);
            this.lvYears.TabIndex = 0;
            this.lvYears.UseCompatibleStateImageBehavior = false;
            this.lvYears.View = System.Windows.Forms.View.List;
            // 
            // colYear
            // 
            this.colYear.Text = "Year";
            // 
            // chkAggregate
            // 
            this.chkAggregate.AutoSize = true;
            this.chkAggregate.Location = new System.Drawing.Point(160, 82);
            this.chkAggregate.Name = "chkAggregate";
            this.chkAggregate.Size = new System.Drawing.Size(121, 19);
            this.chkAggregate.TabIndex = 1;
            this.chkAggregate.Text = "Aggregate results";
            this.chkAggregate.UseVisualStyleBackColor = true;
            // 
            // chkNotInclude1
            // 
            this.chkNotInclude1.AutoSize = true;
            this.chkNotInclude1.Location = new System.Drawing.Point(160, 116);
            this.chkNotInclude1.Name = "chkNotInclude1";
            this.chkNotInclude1.Size = new System.Drawing.Size(129, 19);
            this.chkNotInclude1.TabIndex = 2;
            this.chkNotInclude1.Text = "Do not include n=1";
            this.chkNotInclude1.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(218, 292);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(56, 25);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(155, 292);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select sampling year";
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(10, 8);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(263, 54);
            this.labelTitle.TabIndex = 6;
            this.labelTitle.Text = "label2";
            // 
            // MapEffortHelperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 328);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkNotInclude1);
            this.Controls.Add(this.chkAggregate);
            this.Controls.Add(this.lvYears);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MapEffortHelperForm";
            this.Text = "MapEffortHelperForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnMapForm_FormClosed);
            this.Load += new System.EventHandler(this.MapEffortHelperForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvYears;
        private System.Windows.Forms.ColumnHeader colYear;
        private System.Windows.Forms.CheckBox chkAggregate;
        private System.Windows.Forms.CheckBox chkNotInclude1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTitle;
    }
}