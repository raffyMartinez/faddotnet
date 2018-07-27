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
        int _MouseX;
        int _MouseY;

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
                comboSubGrid.SelectedIndex = (int)FishingGrid.SubGridStyle;
            }
        }



        void FrmAOILoad(object sender, EventArgs e)
        {
            txtName.Focus();
            var lv = (ListView)tabAOI.TabPages["tabGrid25"].Controls["lvMaps"];
            lv.With(o =>
           {
               o.View = View.Details;
               o.Columns.Add("Description");
               o.Columns.Add("Upper left");
               o.Columns.Add("Lower right");
               o.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
               o.FullRowSelect = true;
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

        void OnButtonCancelClick(object sender, EventArgs e)
        {
            this.Close();
        }

        void OnButtonOKClick(object sender, EventArgs e)
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
            if (FishingGrid.GridType == FishingGrid.fadGridType.gridTypeGrid25)
                FishingGrid.SubGridStyle = (FishingGrid.fadSubgridSyle)comboSubGrid.SelectedIndex;

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

        private void lvMaps_MouseDown(object sender, MouseEventArgs e)
        {
            _MouseX = e.X;
            _MouseY = e.Y;
        }

        private void lvMaps_DoubleClick(object sender, EventArgs e)
        {
            var HitItem = lvMaps.HitTest(_MouseX, _MouseY);
            var lvi = HitItem.Item;
            if (lvi != null)
            {
                ShowGridMapForm(lvi);
            }
            else
            {
                ShowGridMapForm();
            }
        }

        private void ShowGridMapForm(ListViewItem lvi = null)
        {
            FGExtentForm fge;
            if (lvi == null)
            {
                fge = new FGExtentForm();
                fge.UTMZone = FishingGrid.ZoneFromZoneName(comboUTMZone.Text);
            }
            else
            {
                var UTMZone = FishingGrid.ZoneFromZoneName(comboUTMZone.Text);
                fge = new FGExtentForm(UTMZone, lvi.Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text);
            }
            fge.ShowDialog(this);
        }

        private void OnbuttonGrid25_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonAddMap":
                    if (comboUTMZone.Text.Length > 0)
                    {
                        if (lvMaps.SelectedItems.Count == 0)
                            ShowGridMapForm();
                        else
                            ShowGridMapForm(lvMaps.SelectedItems[0]);
                    }
                    else
                    {
                        MessageBox.Show("Please select a UTM zone", "UTM zone is missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        comboUTMZone.Select();
                    }

                    break;
                case "buttonRemoveMap":
                    break;
            }
        }
    }
}
