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
	partial class TargetAreaForm
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
            this.txtCode = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabAOI = new System.Windows.Forms.TabControl();
            this.tabGrid25 = new System.Windows.Forms.TabPage();
            this.comboSubGrid = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
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
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(128, 23);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(252, 21);
            this.txtName.TabIndex = 0;
            this.txtName.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextBoxValidating);
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Location = new System.Drawing.Point(128, 53);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(252, 21);
            this.txtCode.TabIndex = 1;
            this.txtCode.Validating += new System.ComponentModel.CancelEventHandler(this.OnTextBoxValidating);
            // 
            // buttonOK
            // 
            this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.Location = new System.Drawing.Point(319, 497);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(61, 26);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "Ok";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.OnMainButtonClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(248, 497);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(64, 26);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.OnMainButtonClick);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 22);
            this.label1.TabIndex = 5;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 22);
            this.label2.TabIndex = 6;
            this.label2.Text = "Letter code";
            // 
            // tabAOI
            // 
            this.tabAOI.Controls.Add(this.tabGrid25);
            this.tabAOI.Controls.Add(this.tabOtherGrid);
            this.tabAOI.Controls.Add(this.tabMBR);
            this.tabAOI.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabAOI.Location = new System.Drawing.Point(13, 92);
            this.tabAOI.Name = "tabAOI";
            this.tabAOI.SelectedIndex = 0;
            this.tabAOI.Size = new System.Drawing.Size(370, 396);
            this.tabAOI.TabIndex = 8;
            this.tabAOI.SelectedIndexChanged += new System.EventHandler(this.tabAOI_SelectedIndexChanged);
            this.tabAOI.TabIndexChanged += new System.EventHandler(this.tabAOI_TabIndexChanged);
            // 
            // tabGrid25
            // 
            this.tabGrid25.Controls.Add(this.comboSubGrid);
            this.tabGrid25.Controls.Add(this.label5);
            this.tabGrid25.Controls.Add(this.buttonRemoveMap);
            this.tabGrid25.Controls.Add(this.buttonAddMap);
            this.tabGrid25.Controls.Add(this.label3);
            this.tabGrid25.Controls.Add(this.lvMaps);
            this.tabGrid25.Controls.Add(this.label4);
            this.tabGrid25.Controls.Add(this.comboUTMZone);
            this.tabGrid25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabGrid25.Location = new System.Drawing.Point(4, 25);
            this.tabGrid25.Name = "tabGrid25";
            this.tabGrid25.Padding = new System.Windows.Forms.Padding(3);
            this.tabGrid25.Size = new System.Drawing.Size(362, 367);
            this.tabGrid25.TabIndex = 0;
            this.tabGrid25.Text = "Grid25";
            this.tabGrid25.UseVisualStyleBackColor = true;
            // 
            // comboSubGrid
            // 
            this.comboSubGrid.FormattingEnabled = true;
            this.comboSubGrid.Location = new System.Drawing.Point(122, 57);
            this.comboSubGrid.Name = "comboSubGrid";
            this.comboSubGrid.Size = new System.Drawing.Size(108, 23);
            this.comboSubGrid.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Subgrid style";
            // 
            // buttonRemoveMap
            // 
            this.buttonRemoveMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemoveMap.Location = new System.Drawing.Point(252, 318);
            this.buttonRemoveMap.Name = "buttonRemoveMap";
            this.buttonRemoveMap.Size = new System.Drawing.Size(45, 31);
            this.buttonRemoveMap.TabIndex = 9;
            this.buttonRemoveMap.Text = "-";
            this.buttonRemoveMap.UseVisualStyleBackColor = true;
            this.buttonRemoveMap.Click += new System.EventHandler(this.OnbuttonGrid25_Click);
            // 
            // buttonAddMap
            // 
            this.buttonAddMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddMap.Location = new System.Drawing.Point(304, 318);
            this.buttonAddMap.Name = "buttonAddMap";
            this.buttonAddMap.Size = new System.Drawing.Size(45, 31);
            this.buttonAddMap.TabIndex = 8;
            this.buttonAddMap.Text = "+";
            this.buttonAddMap.UseVisualStyleBackColor = true;
            this.buttonAddMap.Click += new System.EventHandler(this.OnbuttonGrid25_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(189, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Fishing ground maps and extents";
            // 
            // lvMaps
            // 
            this.lvMaps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvMaps.Location = new System.Drawing.Point(7, 115);
            this.lvMaps.Name = "lvMaps";
            this.lvMaps.Size = new System.Drawing.Size(342, 187);
            this.lvMaps.TabIndex = 6;
            this.lvMaps.UseCompatibleStateImageBehavior = false;
            this.lvMaps.DoubleClick += new System.EventHandler(this.lvMaps_DoubleClick);
            this.lvMaps.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvMaps_MouseDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "UTM zone";
            // 
            // comboUTMZone
            // 
            this.comboUTMZone.FormattingEnabled = true;
            this.comboUTMZone.Location = new System.Drawing.Point(122, 20);
            this.comboUTMZone.Name = "comboUTMZone";
            this.comboUTMZone.Size = new System.Drawing.Size(108, 23);
            this.comboUTMZone.TabIndex = 4;
            // 
            // tabOtherGrid
            // 
            this.tabOtherGrid.Controls.Add(this.buttonDefine);
            this.tabOtherGrid.Controls.Add(this.textBoxOtherGrid);
            this.tabOtherGrid.Location = new System.Drawing.Point(4, 25);
            this.tabOtherGrid.Name = "tabOtherGrid";
            this.tabOtherGrid.Padding = new System.Windows.Forms.Padding(3);
            this.tabOtherGrid.Size = new System.Drawing.Size(362, 367);
            this.tabOtherGrid.TabIndex = 1;
            this.tabOtherGrid.Text = "Other grid";
            this.tabOtherGrid.UseVisualStyleBackColor = true;
            // 
            // buttonDefine
            // 
            this.buttonDefine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDefine.Location = new System.Drawing.Point(287, 267);
            this.buttonDefine.Name = "buttonDefine";
            this.buttonDefine.Size = new System.Drawing.Size(66, 31);
            this.buttonDefine.TabIndex = 8;
            this.buttonDefine.Text = "Define";
            this.buttonDefine.UseVisualStyleBackColor = true;
            // 
            // textBoxOtherGrid
            // 
            this.textBoxOtherGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOtherGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOtherGrid.Location = new System.Drawing.Point(3, 3);
            this.textBoxOtherGrid.Multiline = true;
            this.textBoxOtherGrid.Name = "textBoxOtherGrid";
            this.textBoxOtherGrid.Size = new System.Drawing.Size(353, 246);
            this.textBoxOtherGrid.TabIndex = 7;
            // 
            // tabMBR
            // 
            this.tabMBR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabMBR.Location = new System.Drawing.Point(4, 25);
            this.tabMBR.Name = "tabMBR";
            this.tabMBR.Size = new System.Drawing.Size(362, 367);
            this.tabMBR.TabIndex = 2;
            this.tabMBR.Text = "MBR";
            this.tabMBR.UseVisualStyleBackColor = true;
            // 
            // TargetAreaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(388, 533);
            this.Controls.Add(this.tabAOI);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.txtName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TargetAreaForm";
            this.Text = "Target area";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
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
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.TextBox txtCode;
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
        private System.Windows.Forms.ComboBox comboSubGrid;
        private System.Windows.Forms.Label label5;
    }
}
