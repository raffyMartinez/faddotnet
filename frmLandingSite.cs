/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/8/2016
 * Time: 8:17 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;

namespace FAD3
{
	/// <summary>
	/// Description of frmLandingSite.
	/// </summary>
	public partial class frmLandingSite : Form
	{
		private  string _LSGUID="";
		private Dictionary<long,string> Provinces = new Dictionary<long,string>();
		private bool _isNew=false;
		private string _AOIGUID = "";
        //private landingsite _LandingSite = new landingsite();
        private landingsite _LandingSite;
        private TextBox _Coordinate;
		
		
		public string AOIGUID{
			get{return _AOIGUID;}
			set{_AOIGUID = value;}
		}
		
		public  string LSGUID
		{
			get{return _LSGUID;}
			set{_LSGUID = value;}
		}
		
		public void AddNew(){
			_isNew=true;
		}
		
		public landingsite LandingSite{
			get{return _LandingSite;}
			set{_LandingSite = value;}
		}
		
		public frmLandingSite()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
			Provinces = global.provinceDict;
			comboBox1.DataSource = new BindingSource(Provinces,null);
			comboBox1.DisplayMember = "Value";
			comboBox1.ValueMember = "Key";
			comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
		}
		
		void Button1Click(object sender, EventArgs e)
		{
            string myGUID = "";
            Dictionary<string,string>LSData = new Dictionary<string, string>();
			long key= ((KeyValuePair<long,string>)comboBox2.SelectedItem).Key;
			LSData.Add("LSName",textBox1.Text);
			LSData.Add("MunNo",key.ToString());
			LSData.Add("cx",textBox2.Text);
			LSData.Add("cy",textBox3.Text);
			LSData.Add("AOIGuid", _AOIGUID);
			if(_isNew){
				myGUID = Guid.NewGuid().ToString();
			}
            else
            {
                myGUID = _LandingSite.LandingSiteGUID;
            }
            LSData.Add("LSGUID", myGUID);
			
			if(landingsite.UpdateData(_isNew,LSData)){
				frmMain fr = new frmMain();
				foreach (Form f in Application.OpenForms) {
					if(f.Name == "frmMain"){
						fr=(frmMain)f;
						fr.RefreshLV(LSData["LSName"],"landing_site",_isNew,myGUID);
					}
				}
				this.Close();
			}
		}
		

		
		void SetMunicipalitiesCombo(long ProvNo )
		{
			global.ProvinceNo(ProvNo);
			//comboBox2.Items.Clear();
			comboBox2.DataSource = new BindingSource(global.munDict,null);
			comboBox2.DisplayMember = "Value";
			comboBox2.ValueMember = "Key";
		    comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
		    comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
		}
		
		void FrmLandingSiteLoad(object sender, EventArgs e)
		{
            radioButton1.Checked = true;
            textBox1.Focus();
            Text = "New landing site";
            if (_LandingSite != null)
            {
                Dictionary<string, string> myLSData = _LandingSite.LandingSiteDataEx();
                if (myLSData.Count > 0)
                {
                    textBox1.Text = myLSData["LSName"];
                    comboBox1.Text = myLSData["ProvinceName"];
                    long key = ((KeyValuePair<long, string>)comboBox1.SelectedItem).Key;
                    SetMunicipalitiesCombo(key);
                    comboBox2.Text = myLSData["Municipality"];
                    textBox2.Text = myLSData["cx"];
                    textBox3.Text = myLSData["cy"];
                }
            }
  


		
		}
		
		void ComboBox1Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
            string s = "";
            try {  s = comboBox1.SelectedItem.ToString(); }
            catch {s= ""; }
            if (s != "")
            {
                long key = ((KeyValuePair<long, string>)comboBox1.SelectedItem).Key;
                SetMunicipalitiesCombo(key);
            }
            else
            {
                MessageBox.Show("Item not in list");
                e.Cancel = true;
            }

        }
		
		void Button2Click(object sender, EventArgs e)
		{
			this.Close();
					
		}

        private void textBox2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _Coordinate = (TextBox)sender;
            double num;
            string c = _Coordinate.Text;
            bool isCancel = false;
            if(double.TryParse(c, out num))
            {

            }
            switch (_Coordinate.Tag.ToString())
            {
                case "x":
                    if(num <0 || num >180)
                    {
                        isCancel = true;
                    }
                    break;
                case "y":
                    if (num < 0 || num > 90)
                    {
                        isCancel = true;
                    }
                    break;
            }

            if (isCancel)
            {
                e.Cancel = true;
                MessageBox.Show("Please provide correct degree-decimal coordinate.");
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            _Coordinate = textBox2;
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            _Coordinate = textBox3;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton myButton;
            myButton = (RadioButton)sender;
            switch (myButton.Name)
            {
                case "radioButton1":
                    linkLabel1.Text = "Degree Decimal";
                    if (_LandingSite != null)
                    {
                        textBox2.Text = _LandingSite.xCoord.ToString();
                        textBox3.Text = _LandingSite.yCoord.ToString();
                    }
                    
                    break;
                case "radioButton2":
                    linkLabel1.Text = "Degree Minute";
                    if (_LandingSite != null)
                    {
                        textBox2.Text = _LandingSite.xCoordDegreeMinute;
                        textBox3.Text = _LandingSite.yCoordDegreeMinute;
                    }
                    break;
            }
            panel1.Visible = false;

           
            
        }

        private void panel1_Leave(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void frmLandingSite_Shown(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void comboBox2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string s = "";
            try { s = comboBox2.SelectedItem.ToString(); }
            catch { s = ""; }

            if (s == "")
            {
                MessageBox.Show("Item not in list");
                e.Cancel = true;
            }
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {

        }
    }
}
