namespace FAD3
{
    partial class CreateNewDatabaseForm
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.group1 = new System.Windows.Forms.GroupBox();
            this.buttonFileName = new System.Windows.Forms.Button();
            this.group2 = new System.Windows.Forms.GroupBox();
            this.checkGearLocalNames = new System.Windows.Forms.CheckBox();
            this.checkEnumerators = new System.Windows.Forms.CheckBox();
            this.checkFishLocalNames = new System.Windows.Forms.CheckBox();
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
            this.label2.Location = new System.Drawing.Point(10, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(48, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Step 1";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(300, 384);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(66, 28);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(228, 384);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(66, 28);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // group1
            // 
            this.group1.Controls.Add(this.buttonFileName);
            this.group1.Location = new System.Drawing.Point(6, 58);
            this.group1.Margin = new System.Windows.Forms.Padding(4);
            this.group1.Name = "group1";
            this.group1.Padding = new System.Windows.Forms.Padding(4);
            this.group1.Size = new System.Drawing.Size(368, 298);
            this.group1.TabIndex = 4;
            this.group1.TabStop = false;
            this.group1.Text = "Provide a file name";
            // 
            // buttonFileName
            // 
            this.buttonFileName.Location = new System.Drawing.Point(136, 143);
            this.buttonFileName.Margin = new System.Windows.Forms.Padding(4);
            this.buttonFileName.Name = "buttonFileName";
            this.buttonFileName.Size = new System.Drawing.Size(107, 28);
            this.buttonFileName.TabIndex = 1;
            this.buttonFileName.Text = "Provide file name";
            this.buttonFileName.UseVisualStyleBackColor = true;
            this.buttonFileName.Click += new System.EventHandler(this.Onbutton_Click);
            // 
            // group2
            // 
            this.group2.Controls.Add(this.checkGearLocalNames);
            this.group2.Controls.Add(this.checkEnumerators);
            this.group2.Controls.Add(this.checkFishLocalNames);
            this.group2.Controls.Add(this.checkSciNames);
            this.group2.Controls.Add(this.checkGearVar);
            this.group2.Controls.Add(this.checkLandingSites);
            this.group2.Controls.Add(this.checkAOI);
            this.group2.Location = new System.Drawing.Point(414, 58);
            this.group2.Margin = new System.Windows.Forms.Padding(4);
            this.group2.Name = "group2";
            this.group2.Padding = new System.Windows.Forms.Padding(4);
            this.group2.Size = new System.Drawing.Size(368, 298);
            this.group2.TabIndex = 5;
            this.group2.TabStop = false;
            this.group2.Text = "Select data to retain";
            // 
            // checkGearLocalNames
            // 
            this.checkGearLocalNames.AutoSize = true;
            this.checkGearLocalNames.Location = new System.Drawing.Point(29, 108);
            this.checkGearLocalNames.Margin = new System.Windows.Forms.Padding(4);
            this.checkGearLocalNames.Name = "checkGearLocalNames";
            this.checkGearLocalNames.Size = new System.Drawing.Size(183, 19);
            this.checkGearLocalNames.TabIndex = 6;
            this.checkGearLocalNames.Text = "Local names of fishing gears";
            this.checkGearLocalNames.UseVisualStyleBackColor = true;
            // 
            // checkEnumerators
            // 
            this.checkEnumerators.AutoSize = true;
            this.checkEnumerators.Location = new System.Drawing.Point(29, 187);
            this.checkEnumerators.Margin = new System.Windows.Forms.Padding(4);
            this.checkEnumerators.Name = "checkEnumerators";
            this.checkEnumerators.Size = new System.Drawing.Size(97, 19);
            this.checkEnumerators.TabIndex = 5;
            this.checkEnumerators.Text = "Enumerators";
            this.checkEnumerators.UseVisualStyleBackColor = true;
            // 
            // checkFishLocalNames
            // 
            this.checkFishLocalNames.AutoSize = true;
            this.checkFishLocalNames.Location = new System.Drawing.Point(29, 160);
            this.checkFishLocalNames.Margin = new System.Windows.Forms.Padding(4);
            this.checkFishLocalNames.Name = "checkFishLocalNames";
            this.checkFishLocalNames.Size = new System.Drawing.Size(132, 19);
            this.checkFishLocalNames.TabIndex = 4;
            this.checkFishLocalNames.Text = "Local names of fish";
            this.checkFishLocalNames.UseVisualStyleBackColor = true;
            // 
            // checkSciNames
            // 
            this.checkSciNames.AutoSize = true;
            this.checkSciNames.Location = new System.Drawing.Point(29, 134);
            this.checkSciNames.Margin = new System.Windows.Forms.Padding(4);
            this.checkSciNames.Name = "checkSciNames";
            this.checkSciNames.Size = new System.Drawing.Size(151, 19);
            this.checkSciNames.TabIndex = 3;
            this.checkSciNames.Text = "Scientific names of fish";
            this.checkSciNames.UseVisualStyleBackColor = true;
            // 
            // checkGearVar
            // 
            this.checkGearVar.AutoSize = true;
            this.checkGearVar.Location = new System.Drawing.Point(29, 82);
            this.checkGearVar.Margin = new System.Windows.Forms.Padding(4);
            this.checkGearVar.Name = "checkGearVar";
            this.checkGearVar.Size = new System.Drawing.Size(108, 19);
            this.checkGearVar.TabIndex = 2;
            this.checkGearVar.Text = "Gear variations";
            this.checkGearVar.UseVisualStyleBackColor = true;
            // 
            // checkLandingSites
            // 
            this.checkLandingSites.AutoSize = true;
            this.checkLandingSites.Location = new System.Drawing.Point(29, 54);
            this.checkLandingSites.Margin = new System.Windows.Forms.Padding(4);
            this.checkLandingSites.Name = "checkLandingSites";
            this.checkLandingSites.Size = new System.Drawing.Size(99, 19);
            this.checkLandingSites.TabIndex = 1;
            this.checkLandingSites.Text = "Landing sites";
            this.checkLandingSites.UseVisualStyleBackColor = true;
            // 
            // checkAOI
            // 
            this.checkAOI.AutoSize = true;
            this.checkAOI.Location = new System.Drawing.Point(29, 28);
            this.checkAOI.Margin = new System.Windows.Forms.Padding(4);
            this.checkAOI.Name = "checkAOI";
            this.checkAOI.Size = new System.Drawing.Size(95, 19);
            this.checkAOI.TabIndex = 0;
            this.checkAOI.Text = "Target areas";
            this.checkAOI.UseVisualStyleBackColor = true;
            this.checkAOI.CheckedChanged += new System.EventHandler(this.checkAOI_CheckedChanged);
            // 
            // CreateNewDatabaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 430);
            this.Controls.Add(this.group2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.group1);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CreateNewDatabaseForm";
            this.Text = "Make a new database ";
            this.Load += new System.EventHandler(this.frmNewDB_Load);
            this.group1.ResumeLayout(false);
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox group1;
        private System.Windows.Forms.Button buttonFileName;
        private System.Windows.Forms.GroupBox group2;
        private System.Windows.Forms.CheckBox checkFishLocalNames;
        private System.Windows.Forms.CheckBox checkSciNames;
        private System.Windows.Forms.CheckBox checkGearVar;
        private System.Windows.Forms.CheckBox checkLandingSites;
        private System.Windows.Forms.CheckBox checkAOI;
        private System.Windows.Forms.CheckBox checkEnumerators;
        private System.Windows.Forms.CheckBox checkGearLocalNames;
    }
}