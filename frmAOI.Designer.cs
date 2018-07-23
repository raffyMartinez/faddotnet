/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/10/2016
 * Time: 4:19 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace FAD3
{
	partial class frmAOI
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtLetter = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabAOI = new System.Windows.Forms.TabControl();
            this.tabGrid25 = new System.Windows.Forms.TabPage();
            this.buttonRemoveMap = new System.Windows.Forms.Button();
            this.buttonAddMap = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lvMaps = new System.Windows.Forms.ListView();
            this.label4 = new System.Windows.Forms.Label();
            this.comboUTMZone = new System.Windows.Forms.ComboBox();
            this.tabOtherGrid = new System.Windows.Forms.TabPage();
            this.buttonDefine = new System.Windows.Forms.Button();
            this.textBoxOtherGrid = new System.Windows.Forms.TextBox();
            this.tabMBR = new System.Windows.Forms.TabPage();
            this.tabAOI.SuspendLayout();
            this.tabGrid25.SuspendLayout();
            this.tabOtherGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(112, 38);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(217, 23);
            this.txtName.TabIndex = 0;
            // 
            // txtLetter
            // 
            this.txtLetter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLetter.Location = new System.Drawing.Point(112, 64);
            this.txtLetter.Name = "txtLetter";
            this.txtLetter.Size = new System.Drawing.Size(217, 23);
            this.txtLetter.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(277, 435);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 31);
            this.button1.TabIndex = 3;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(208, 435);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(63, 31);
            this.button2.TabIndex = 4;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 19);
            this.label2.TabIndex = 6;
            this.label2.Text = "Letter code";
            // 
            // tabAOI
            // 
            this.tabAOI.Controls.Add(this.tabGrid25);
            this.tabAOI.Controls.Add(this.tabOtherGrid);
            this.tabAOI.Controls.Add(this.tabMBR);
            this.tabAOI.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabAOI.Location = new System.Drawing.Point(12, 117);
            this.tabAOI.Name = "tabAOI";
            this.tabAOI.SelectedIndex = 0;
            this.tabAOI.Size = new System.Drawing.Size(317, 302);
            this.tabAOI.TabIndex = 8;
            this.tabAOI.SelectedIndexChanged += new System.EventHandler(this.tabAOI_SelectedIndexChanged);
            this.tabAOI.TabIndexChanged += new System.EventHandler(this.tabAOI_TabIndexChanged);
            // 
            // tabGrid25
            // 
            this.tabGrid25.Controls.Add(this.buttonRemoveMap);
            this.tabGrid25.Controls.Add(this.buttonAddMap);
            this.tabGrid25.Controls.Add(this.label3);
            this.tabGrid25.Controls.Add(this.lvMaps);
            this.tabGrid25.Controls.Add(this.label4);
            this.tabGrid25.Controls.Add(this.comboUTMZone);
            this.tabGrid25.Location = new System.Drawing.Point(4, 25);
            this.tabGrid25.Name = "tabGrid25";
            this.tabGrid25.Padding = new System.Windows.Forms.Padding(3);
            this.tabGrid25.Size = new System.Drawing.Size(309, 273);
            this.tabGrid25.TabIndex = 0;
            this.tabGrid25.Text = "Grid25";
            this.tabGrid25.UseVisualStyleBackColor = true;
            // 
            // buttonRemoveMap
            // 
            this.buttonRemoveMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemoveMap.Location = new System.Drawing.Point(216, 240);
            this.buttonRemoveMap.Name = "buttonRemoveMap";
            this.buttonRemoveMap.Size = new System.Drawing.Size(39, 27);
            this.buttonRemoveMap.TabIndex = 9;
            this.buttonRemoveMap.Text = "-";
            this.buttonRemoveMap.UseVisualStyleBackColor = true;
            // 
            // buttonAddMap
            // 
            this.buttonAddMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddMap.Location = new System.Drawing.Point(261, 240);
            this.buttonAddMap.Name = "buttonAddMap";
            this.buttonAddMap.Size = new System.Drawing.Size(39, 27);
            this.buttonAddMap.TabIndex = 8;
            this.buttonAddMap.Text = "+";
            this.buttonAddMap.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(217, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Fishing ground maps and extents";
            // 
            // lvMaps
            // 
            this.lvMaps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvMaps.Location = new System.Drawing.Point(6, 71);
            this.lvMaps.Name = "lvMaps";
            this.lvMaps.Size = new System.Drawing.Size(294, 163);
            this.lvMaps.TabIndex = 6;
            this.lvMaps.UseCompatibleStateImageBehavior = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "UTM zone";
            // 
            // comboUTMZone
            // 
            this.comboUTMZone.FormattingEnabled = true;
            this.comboUTMZone.Location = new System.Drawing.Point(85, 17);
            this.comboUTMZone.Name = "comboUTMZone";
            this.comboUTMZone.Size = new System.Drawing.Size(93, 24);
            this.comboUTMZone.TabIndex = 4;
            // 
            // tabOtherGrid
            // 
            this.tabOtherGrid.Controls.Add(this.buttonDefine);
            this.tabOtherGrid.Controls.Add(this.textBoxOtherGrid);
            this.tabOtherGrid.Location = new System.Drawing.Point(4, 25);
            this.tabOtherGrid.Name = "tabOtherGrid";
            this.tabOtherGrid.Padding = new System.Windows.Forms.Padding(3);
            this.tabOtherGrid.Size = new System.Drawing.Size(309, 273);
            this.tabOtherGrid.TabIndex = 1;
            this.tabOtherGrid.Text = "Other grid";
            this.tabOtherGrid.UseVisualStyleBackColor = true;
            // 
            // buttonDefine
            // 
            this.buttonDefine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDefine.Location = new System.Drawing.Point(246, 231);
            this.buttonDefine.Name = "buttonDefine";
            this.buttonDefine.Size = new System.Drawing.Size(57, 27);
            this.buttonDefine.TabIndex = 8;
            this.buttonDefine.Text = "Define";
            this.buttonDefine.UseVisualStyleBackColor = true;
            // 
            // textBoxOtherGrid
            // 
            this.textBoxOtherGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOtherGrid.Location = new System.Drawing.Point(3, 3);
            this.textBoxOtherGrid.Multiline = true;
            this.textBoxOtherGrid.Name = "textBoxOtherGrid";
            this.textBoxOtherGrid.Size = new System.Drawing.Size(303, 213);
            this.textBoxOtherGrid.TabIndex = 7;
            // 
            // tabMBR
            // 
            this.tabMBR.Location = new System.Drawing.Point(4, 25);
            this.tabMBR.Name = "tabMBR";
            this.tabMBR.Size = new System.Drawing.Size(309, 273);
            this.tabMBR.TabIndex = 2;
            this.tabMBR.Text = "MBR";
            this.tabMBR.UseVisualStyleBackColor = true;
            // 
            // frmAOI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(342, 478);
            this.Controls.Add(this.tabAOI);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtLetter);
            this.Controls.Add(this.txtName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmAOI";
            this.Text = "frmAOI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAOI_FormClosed);
            this.Load += new System.EventHandler(this.FrmAOILoad);
            this.tabAOI.ResumeLayout(false);
            this.tabGrid25.ResumeLayout(false);
            this.tabGrid25.PerformLayout();
            this.tabOtherGrid.ResumeLayout(false);
            this.tabOtherGrid.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox txtLetter;
		private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TabControl tabAOI;
        private System.Windows.Forms.TabPage tabGrid25;
        private System.Windows.Forms.TabPage tabOtherGrid;
        private System.Windows.Forms.TabPage tabMBR;
        private System.Windows.Forms.Button buttonDefine;
        private System.Windows.Forms.TextBox textBoxOtherGrid;
        private System.Windows.Forms.Button buttonRemoveMap;
        private System.Windows.Forms.Button buttonAddMap;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView lvMaps;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboUTMZone;
    }
}
