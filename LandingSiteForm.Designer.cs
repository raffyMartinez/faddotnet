/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/8/2016
 * Time: 8:17 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace FAD3
{
	partial class LandingSiteForm
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
            this.textLandingSiteName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboProvince = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboMunicipality = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textCoord = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textLandingSiteName
            // 
            this.textLandingSiteName.Location = new System.Drawing.Point(132, 48);
            this.textLandingSiteName.Margin = new System.Windows.Forms.Padding(4);
            this.textLandingSiteName.Name = "textLandingSiteName";
            this.textLandingSiteName.Size = new System.Drawing.Size(185, 22);
            this.textLandingSiteName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // comboProvince
            // 
            this.comboProvince.FormattingEnabled = true;
            this.comboProvince.Location = new System.Drawing.Point(133, 81);
            this.comboProvince.Margin = new System.Windows.Forms.Padding(4);
            this.comboProvince.Name = "comboProvince";
            this.comboProvince.Size = new System.Drawing.Size(183, 24);
            this.comboProvince.TabIndex = 1;
            this.comboProvince.Validating += new System.ComponentModel.CancelEventHandler(this.OnComboBoxValidating);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 81);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Province";
            // 
            // comboMunicipality
            // 
            this.comboMunicipality.FormattingEnabled = true;
            this.comboMunicipality.Location = new System.Drawing.Point(133, 114);
            this.comboMunicipality.Margin = new System.Windows.Forms.Padding(4);
            this.comboMunicipality.Name = "comboMunicipality";
            this.comboMunicipality.Size = new System.Drawing.Size(183, 24);
            this.comboMunicipality.TabIndex = 2;
            this.comboMunicipality.Validating += new System.ComponentModel.CancelEventHandler(this.OnComboBoxValidating);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 118);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "Municipality";
            // 
            // textCoord
            // 
            this.textCoord.Location = new System.Drawing.Point(131, 181);
            this.textCoord.Margin = new System.Windows.Forms.Padding(4);
            this.textCoord.Name = "textCoord";
            this.textCoord.ReadOnly = true;
            this.textCoord.Size = new System.Drawing.Size(185, 22);
            this.textCoord.TabIndex = 3;
            this.textCoord.Tag = "x";
            this.textCoord.DoubleClick += new System.EventHandler(this.OntextCoord_DoubleClick);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 185);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Coordinate";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(276, 260);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(57, 26);
            this.buttonOk.TabIndex = 5;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(196, 260);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(72, 26);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.OnButtonClick);
            // 
            // frmLandingSite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(349, 300);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textCoord);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboMunicipality);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboProvince);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textLandingSiteName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmLandingSite";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Landing site";
            this.Load += new System.EventHandler(this.FrmLandingSiteLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textCoord;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboMunicipality;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboProvince;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textLandingSiteName;
    }
}
