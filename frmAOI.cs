/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/10/2016
 * Time: 4:19 PM
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
	/// Description of frmAOI.
	/// </summary>
	public partial class frmAOI : Form
	{
		private string _AOIGUID = "";
		private aoi _AOI = new aoi();
		private bool _IsNew = false;
		
		public string AOIGUID
		{
			get{ return _AOIGUID;}
			set{_AOIGUID = value;}
		}
		
		public aoi AOI{
			get{return _AOI;}
			set{_AOI = value;}
		}
		
		public void AddNew(){
			_IsNew=true;
		}
		
		public frmAOI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void FrmAOILoad(object sender, EventArgs e)
		{
			var myAOIdata = _AOI.AOIDataEx();
            if (myAOIdata.Count>0) {
                txtName.Text = myAOIdata["AOIName"].ToString();
                txtLetter.Text = myAOIdata["Letter"].ToString();
                txtGrids.Text = myAOIdata["MajorGridList"].ToString();
            }
            txtName.Focus();
		}
		
		void Button2Click(object sender, EventArgs e)
		{

            this.Close();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			Dictionary<string,string>MyData = new Dictionary<string, string>();
			if(!string.IsNullOrEmpty(txtName.Text) && 
			   !string.IsNullOrEmpty(txtLetter.Text) &&
			   !string.IsNullOrEmpty(txtGrids.Text)) {
				
			
					MyData.Add("AOIName", txtName.Text);
					MyData.Add("Letter", txtLetter.Text);
					MyData.Add("MajorGridList", txtGrids.Text);
					string myGUID = _AOI.AOIGUID;
					if(_IsNew){
						myGUID = Guid.NewGuid().ToString();
					}
					MyData.Add("AOIGUID", myGUID);
					
					
					if(aoi.UpdateData(_IsNew,MyData))
					{
						frmMain fr = new frmMain();
						foreach (Form f in Application.OpenForms) {
							if(f.Name == "frmMain"){
								fr=(frmMain)f;
								fr.RefreshLV(MyData["AOIName"],"aoi", _IsNew,myGUID);
							}
						}
						this.Close();
					}

						
			   }
		}
	}
}
