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
        static frmAOI _instance;
        private frmMain _parent_form;

        public frmMain Parent_form
        {
            get { return _parent_form; }
            set { _parent_form = value; }
        }

        public static frmAOI GetInstance()
        {
            if (_instance == null) _instance = new frmAOI();
            return _instance;
        }

        public string AOIGUID
        {
            get { return _AOIGUID; }
            set { _AOIGUID = value; }
        }

        public aoi AOI
        {
            get { return _AOI; }
            set
            {
                _AOI = value;
                ShowAOIProps();

            }
        }

        public void AddNew()
        {
            _IsNew = true;
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
        void ShowAOIProps()
        {
            textBoxOtherGrid.Text = "";

            var myAOIdata = _AOI.AOIDataEx();
            if (myAOIdata.Count > 0)
            {
                txtName.Text = myAOIdata["AOIName"].ToString();
                txtLetter.Text = myAOIdata["Code"].ToString();
                var tabPageName = "";
                switch (FishingGrid.GridType)
                {
                    case FishingGrid.fadGridType.gridTypeOther:
                        tabPageName = "tabOtherGrid";
                        break;
                    case FishingGrid.fadGridType.gridTypeGrid25:
                        tabPageName = "tabGrid25";
                        break;
                }
                if (FishingGrid.GridType != FishingGrid.fadGridType.gridTypeNone)
                {
                    var mytab = tabAOI.TabPages[tabPageName];
                    mytab.Select();
                }
                LoadGrid25Items(lvMaps);
                comboUTMZone.Text = "";
                foreach (var item in FishingGrid.UTMZones)
                {
                    comboUTMZone.Items.Add(item.ToString());
                }
                comboUTMZone.Text = FishingGrid.UTMZoneName;
            }
        }



        void FrmAOILoad(object sender, EventArgs e)
        {
            txtName.Focus();
            var lv = (ListView)tabAOI.TabPages["tabGrid25"].Controls["lvMaps"];
            lv.With( o=>
            {
                o.View = View.Details;
                o.Columns.Add("Description");
                o.Columns.Add("Upper left");
                o.Columns.Add("Lower right");
                o.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            });

        }

        void LoadGrid25Items(ListView lv)
        {
            lv.Items.Clear();
            if (FishingGrid.GridType == FishingGrid.fadGridType.gridTypeGrid25)
            {
                foreach (var item in FishingGrid.Grid25.Bounds)
                {
                    var lvi = lv.Items.Add(item.gridDescription);
                    lvi.SubItems.Add(item.ulGridName);
                    lvi.SubItems.Add(item.lrGridName);
                }
            }
        }

        void Button2Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void Button1Click(object sender, EventArgs e)
        {
            Dictionary<string, string> MyData = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txtName.Text) &&
               !string.IsNullOrEmpty(txtLetter.Text))
            {


                MyData.Add("AOIName", txtName.Text);
                MyData.Add("Letter", txtLetter.Text);
                string myGUID = _AOI.AOIGUID;
                if (_IsNew)
                {
                    myGUID = Guid.NewGuid().ToString();
                }
                MyData.Add("AOIGUID", myGUID);


                if (aoi.UpdateData(_IsNew, MyData))
                {
                    frmMain fr = new frmMain();
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Name == "frmMain")
                        {
                            fr = (frmMain)f;
                            fr.RefreshLV(MyData["AOIName"], "aoi", _IsNew, myGUID);
                        }
                    }
                    this.Close();
                }
            }
        }

        private void tabAOI_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmAOI_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        private void panelAOI_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabAOI_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TabPage tp = ((TabControl)sender).SelectedTab;
            //ShowTabPanels(tp.Name);
        }
    }
}
