/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/13/2016
 * Time: 11:50 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using Microsoft.Win32;
using System.Collections.Generic;

namespace FAD3
{
	/// <summary>
	/// Description of frmEnumerator.
	/// </summary>
	public partial class frmEnumerator : Form
	{
		private string _EnumeratorGUID = "";
		private bool _IsNew = false;
		private aoi _AOI = new aoi();
        private frmMain _parentForm;
			
		public aoi AOI{
			get{return _AOI;}
			set{_AOI = value;}
		}

        public new frmMain ParentForm
        {
            get { return _parentForm; }
            set { _parentForm = value; }
        }
		
		public void AddNew(){
			_IsNew=true;
			this.Text = "Add a new enumerator for " + _AOI.AOIName;
			//button1.Anchor = AnchorStyles.Right;
			//button2.Anchor = AnchorStyles.None;			
			listEnumeratorSampling.Visible=false;
			textBox1.Left = textBox1.Left+20;
			label3.Visible=false;
			label2.Left = label1.Left;
			label2.Top = label1.Top + label1.Height + 10;	
			textBox2.Left = textBox1.Left;
			textBox2.Top = label2.Top;
			checkBox1.Left = textBox2.Left;
			checkBox1.Top = textBox2.Top + textBox2.Height + 10;	
			this.Width = textBox1.Left + textBox1.Width +  button2.Width + 60;
			this.Height = checkBox1.Top + checkBox1.Height + 60;
			button1.Top = textBox1.Top;
			button2.Top = button1.Top + button1.Height + 10;
			button1.Left = textBox1.Left + textBox1.Width + 30;
			button2.Left = button1.Left;
		}
		
		public frmEnumerator(){
			//default conructor
			InitializeComponent();
		}
		public frmEnumerator(string EnumeratorGUID)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			_EnumeratorGUID=EnumeratorGUID;
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			this.Close();
		}
		
		void FrmEnumeratorLoad(object sender, EventArgs e)
		{

			listEnumeratorSampling.View = View.Details;
			listEnumeratorSampling.FullRowSelect=true;
			listEnumeratorSampling.Columns.Add("Reference no.");
			listEnumeratorSampling.Columns.Add("Landing site");
			listEnumeratorSampling.Columns.Add("Gear used");
			listEnumeratorSampling.Columns.Add("Date sampled");
			listEnumeratorSampling.Columns.Add("Catch weight");
			listEnumeratorSampling.Columns.Add("Catch rows");
			
			if(!_IsNew){
				ReadData();
				ReadSamplings();
				this.Text = "Enumerator data and sampling list";
			}
			
			RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3\\ColWidth");
			try{
				string rv = rk.GetValue(global.lvContext.EnumeratorSampling.ToString() ,"NULL").ToString();
				string[] arr = rv.Split(',');
				int i=0;
				foreach (var item in listEnumeratorSampling.Columns) 
				{
					ColumnHeader ch = (ColumnHeader)item;
					ch.Width = Convert.ToInt32(arr[i]);
					i++;
				}
			}
			catch{
				ErrorLogger.Log ("Catch and effort column width not found in registry");
			}			
		}
		
		void ReadSamplings(){
			DataTable dt = new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try
				{
					conection.Open();
					string query = "SELECT tblSampling.SamplingDate, tblSampling.SamplingGUID,  tblSampling.GearVarGUID, tblSampling.LSGUID, " +
                                   "tblSampling.RefNo, tblLandingSites.LSName, " +
                                   "tblGearVariations.Variation, tblSampling.WtCatch, Count(tblCatchComp.RowGUID) AS n " +
                                   "FROM tblLandingSites INNER JOIN (tblGearVariations INNER JOIN(tblSampling LEFT " +
                                   "JOIN tblCatchComp ON tblSampling.SamplingGUID = tblCatchComp.SamplingGUID) ON " +
                                   "tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON tblLandingSites.LSGUID = tblSampling.LSGUID " +
                                   "WHERE tblSampling.Enumerator = '{" + _EnumeratorGUID + "}' " +
                                   "GROUP BY tblSampling.SamplingDate, tblSampling.SamplingGUID, tblSampling.GearVarGUID, tblSampling.LSGUID,  tblSampling.RefNo, tblLandingSites.LSName, " +
                                   "tblGearVariations.Variation, tblSampling.WtCatch ORDER BY tblSampling.SamplingDate";
					
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(dt);
					ListViewItem lvi = new ListViewItem();
					for(int i=0;i<dt.Rows.Count;i++){
						DataRow dr = dt.Rows[i];
						lvi = listEnumeratorSampling.Items.Add(dr["RefNo"].ToString());
						lvi.Tag = dr["SamplingGUID"].ToString() + "|" + dr["GearVarGUID"].ToString() + "|" + dr["LSGUID"].ToString() + "|" + dr["SamplingDate"].ToString();
						lvi.SubItems.Add(dr["LSName"].ToString());
						lvi.SubItems.Add(dr["Variation"].ToString());
						DateTime dtm = (DateTime)dr["SamplingDate"];
						lvi.SubItems.Add(string.Format("{0:MMM-dd-yyyy}",dtm));
						lvi.SubItems.Add(dr["WtCatch"].ToString());
						lvi.SubItems.Add(dr["n"].ToString());
					}
				}
				catch(Exception ex){
					ErrorLogger.Log(ex);
				}
			}
		}
		
		void ReadData()
		{
			DataTable dt = new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try
				{
					conection.Open();
					string query="Select * from tblEnumerators where EnumeratorID =\"" + _EnumeratorGUID + "\"";
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(dt);
					if(dt.Rows.Count > 0)
					{
					  DataRow dr = dt.Rows[0];
					  textBox1.Text = dr["EnumeratorName"].ToString();
					  DateTime dtm = (DateTime)dr["HireDate"];						  
					  textBox2.Text = string.Format("{0:MMM-dd-yyyy}", dtm);
					  checkBox1.Checked = Convert.ToBoolean(dr["Active"]);
					}

				}
				catch (Exception ex)
				{
					ErrorLogger.Log(ex);
				}
			}
		}
		
	
		
		void ListEnumeratorSamplingLeave(object sender, EventArgs e)
		{
			using(frmMain frm = new frmMain())
			{
				frmMain.SaveColumnWidthEx(sender, myContext:global.lvContext.EnumeratorSampling);
			}
			
		}
		
		protected bool CheckDate(String date)
        {  
	        DateTime Temp;

	        if (DateTime.TryParse(date, out Temp) == true)
	            return true;
	        else
	            return false;
        }
		
		void Button1Click(object sender, EventArgs e)
		{
			Dictionary<string,string>myData = new Dictionary<string, string>();
			myData.Add("TargetArea",_AOI.AOIGUID);
			if(_IsNew){
			  myData.Add("EnumeratorId",Guid.NewGuid().ToString());
			}
			else{
				myData.Add("EnumeratorId", _EnumeratorGUID);
			}
			myData.Add("EnumeratorName",textBox1.Text);
			myData.Add("HireDate",textBox2.Text.ToString());
			myData.Add("Active", checkBox1.Checked.ToString());
			if(aoi.UpdateEnumeratorData(_IsNew,myData)){
                _AOI.HaveEnumerators = true;
                frmMain fr = new frmMain();
				foreach(Form f in Application.OpenForms){
					if(f.Name=="frmMain"){
						fr=(frmMain)f;
						fr.RefreshLVEx("aoi");
					}
				}
				this.Close();
			}
		}
		
		void TextBox2Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string c = textBox2.Text;
			if(c!=""){
				if(!CheckDate(c)){
					MessageBox.Show("Please provide a proper date");
					e.Cancel=true;
				}
			}
		}

        private void listEnumeratorSampling_DoubleClick(object sender, EventArgs e)
        {

            string[] arr = listEnumeratorSampling.SelectedItems[0].Tag.ToString().Split('|');
            Dictionary<string, string> mySampling = new Dictionary<string, string>();
            mySampling.Add("SamplingID", arr[0]);
            mySampling.Add("GearID",arr[1]);
            mySampling.Add("LSGUID", arr[2]);
            mySampling.Add("SamplingDate", arr[3]);
            _parentForm.EnumeratorSelectedSampling(mySampling);
        }
    }
}
