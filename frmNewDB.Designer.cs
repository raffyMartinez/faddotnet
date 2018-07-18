namespace FAD3
{
    partial class frmNewDB
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
            this.label2 = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.group1 = new System.Windows.Forms.GroupBox();
            this.buttonFileName = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.group2 = new System.Windows.Forms.GroupBox();
            this.checkEnumerators = new System.Windows.Forms.CheckBox();
            this.checkLocalNames = new System.Windows.Forms.CheckBox();
            this.checkSciNames = new System.Windows.Forms.CheckBox();
            this.checkGearVar = new System.Windows.Forms.CheckBox();
            this.checkLandingSites = new System.Windows.Forms.CheckBox();
            this.checkAOI = new System.Windows.Forms.CheckBox();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 28);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(48, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Step 1";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(272, 333);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(56, 28);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(210, 333);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(56, 28);
            this.button3.TabIndex = 3;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // group1
            // 
            this.group1.Controls.Add(this.buttonFileName);
            this.group1.Controls.Add(this.label3);
            this.group1.Location = new System.Drawing.Point(12, 50);
            this.group1.Name = "group1";
            this.group1.Size = new System.Drawing.Size(316, 258);
            this.group1.TabIndex = 4;
            this.group1.TabStop = false;
            this.group1.Text = "Provide a file name";
            // 
            // buttonFileName
            // 
            this.buttonFileName.Location = new System.Drawing.Point(116, 124);
            this.buttonFileName.Name = "buttonFileName";
            this.buttonFileName.Size = new System.Drawing.Size(99, 29);
            this.buttonFileName.TabIndex = 1;
            this.buttonFileName.Text = "Provide file name";
            this.buttonFileName.UseVisualStyleBackColor = true;
            this.buttonFileName.Click += new System.EventHandler(this.buttonFileName_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(118, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Provide a file name";
            // 
            // group2
            // 
            this.group2.Controls.Add(this.checkEnumerators);
            this.group2.Controls.Add(this.checkLocalNames);
            this.group2.Controls.Add(this.checkSciNames);
            this.group2.Controls.Add(this.checkGearVar);
            this.group2.Controls.Add(this.checkLandingSites);
            this.group2.Controls.Add(this.checkAOI);
            this.group2.Location = new System.Drawing.Point(355, 50);
            this.group2.Name = "group2";
            this.group2.Size = new System.Drawing.Size(316, 258);
            this.group2.TabIndex = 5;
            this.group2.TabStop = false;
            this.group2.Text = "Select data to retain";
            // 
            // checkEnumerators
            // 
            this.checkEnumerators.AutoSize = true;
            this.checkEnumerators.Location = new System.Drawing.Point(42, 154);
            this.checkEnumerators.Name = "checkEnumerators";
            this.checkEnumerators.Size = new System.Drawing.Size(85, 17);
            this.checkEnumerators.TabIndex = 5;
            this.checkEnumerators.Text = "Enumerators";
            this.checkEnumerators.UseVisualStyleBackColor = true;
            // 
            // checkLocalNames
            // 
            this.checkLocalNames.AutoSize = true;
            this.checkLocalNames.Location = new System.Drawing.Point(42, 131);
            this.checkLocalNames.Name = "checkLocalNames";
            this.checkLocalNames.Size = new System.Drawing.Size(86, 17);
            this.checkLocalNames.TabIndex = 4;
            this.checkLocalNames.Text = "Local names";
            this.checkLocalNames.UseVisualStyleBackColor = true;
            // 
            // checkSciNames
            // 
            this.checkSciNames.AutoSize = true;
            this.checkSciNames.Location = new System.Drawing.Point(42, 108);
            this.checkSciNames.Name = "checkSciNames";
            this.checkSciNames.Size = new System.Drawing.Size(103, 17);
            this.checkSciNames.TabIndex = 3;
            this.checkSciNames.Text = "Scientific names";
            this.checkSciNames.UseVisualStyleBackColor = true;
            // 
            // checkGearVar
            // 
            this.checkGearVar.AutoSize = true;
            this.checkGearVar.Location = new System.Drawing.Point(42, 85);
            this.checkGearVar.Name = "checkGearVar";
            this.checkGearVar.Size = new System.Drawing.Size(97, 17);
            this.checkGearVar.TabIndex = 2;
            this.checkGearVar.Text = "Gear variations";
            this.checkGearVar.UseVisualStyleBackColor = true;
            // 
            // checkLandingSites
            // 
            this.checkLandingSites.AutoSize = true;
            this.checkLandingSites.Location = new System.Drawing.Point(42, 62);
            this.checkLandingSites.Name = "checkLandingSites";
            this.checkLandingSites.Size = new System.Drawing.Size(88, 17);
            this.checkLandingSites.TabIndex = 1;
            this.checkLandingSites.Text = "Landing sites";
            this.checkLandingSites.UseVisualStyleBackColor = true;
            // 
            // checkAOI
            // 
            this.checkAOI.AutoSize = true;
            this.checkAOI.Location = new System.Drawing.Point(42, 39);
            this.checkAOI.Name = "checkAOI";
            this.checkAOI.Size = new System.Drawing.Size(129, 17);
            this.checkAOI.TabIndex = 0;
            this.checkAOI.Text = "Areas of interest (AOI)";
            this.checkAOI.UseVisualStyleBackColor = true;
            this.checkAOI.CheckedChanged += new System.EventHandler(this.checkAOI_CheckedChanged);
            // 
            // frmNewDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 373);
            this.Controls.Add(this.group2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.group1);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmNewDB";
            this.Text = "Make a new database ";
            this.Load += new System.EventHandler(this.frmNewDB_Load);
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox group1;
        private System.Windows.Forms.Button buttonFileName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox group2;
        private System.Windows.Forms.CheckBox checkLocalNames;
        private System.Windows.Forms.CheckBox checkSciNames;
        private System.Windows.Forms.CheckBox checkGearVar;
        private System.Windows.Forms.CheckBox checkLandingSites;
        private System.Windows.Forms.CheckBox checkAOI;
        private System.Windows.Forms.CheckBox checkEnumerators;
    }
}