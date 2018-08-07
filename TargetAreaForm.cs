/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/10/2016
 * Time: 4:19 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FAD3
{
    /// <summary>
    /// Description of TargetAreaForm.
    /// </summary>
    public partial class TargetAreaForm : Form
    {
        private aoi _AOI = new aoi();
        private bool _IsNew = false;
        private static TargetAreaForm _instance;
        private frmMain _parent_form;
        private int _MouseX;
        private int _MouseY;
        private string _TargetAreaName;

        public void SetFishingGround(string Name, string ULCorner, string LRCorner)
        {
            var lvi = new ListViewItem();
            if (lvMaps.Items.ContainsKey(Name))
            {
                lvi = lvMaps.Items[Name];
                lvi.SubItems[1].Text = ULCorner;
                lvi.SubItems[2].Text = LRCorner;
            }
            else
            {
                lvi = new ListViewItem(new string[] { Name, ULCorner, LRCorner });
                lvi.Name = Name;
                lvMaps.Items.Add(lvi);
            }
        }

        public frmMain Parent_form
        {
            get { return _parent_form; }
            set { _parent_form = value; }
        }

        public static TargetAreaForm GetInstance(frmMain Parent, bool IsNew = false)
        {
            if (_instance == null) _instance = new TargetAreaForm(Parent, IsNew);
            return _instance;
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

        public TargetAreaForm(frmMain Parent, bool IsNew = false)
        {
            InitializeComponent();
            _IsNew = IsNew;
            _parent_form = Parent;
        }

        private void ShowAOIProps()
        {
            textBoxOtherGrid.Text = "";

            var myAOIdata = _AOI.AOIDataEx();
            if (myAOIdata.Count > 0)
            {
                txtName.Text = myAOIdata["AOIName"].ToString();
                txtCode.Text = myAOIdata["Code"].ToString();
                var tabPageName = "";
                switch (FishingGrid.GridType)
                {
                    case FishingGrid.fadGridType.gridTypeOther:
                        tabPageName = "tabOtherGrid";
                        break;

                    case FishingGrid.fadGridType.gridTypeGrid25:
                        tabPageName = "tabGrid25";

                        LoadGrid25Items(lvMaps);
                        comboUTMZone.Text = "";
                        comboUTMZone.Text = FishingGrid.UTMZoneName;
                        comboSubGrid.SelectedIndex = (int)FishingGrid.SubGridStyle;

                        break;
                }

                if (FishingGrid.GridType != FishingGrid.fadGridType.gridTypeNone)
                {
                    var mytab = tabAOI.TabPages[tabPageName];
                    mytab.Select();
                }
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            txtName.Focus();
            var lv = (ListView)tabAOI.TabPages["tabGrid25"].Controls["lvMaps"];

            foreach (var item in FishingGrid.UTMZones)
            {
                comboUTMZone.Items.Add(item);
            }
            foreach (var item in FishingGrid.SubGridStyles)
            {
                comboSubGrid.Items.Add(item);
            }

            lv.With(o =>
           {
               o.View = View.Details;
               o.Columns.Add("Description");
               o.Columns.Add("Upper left");
               o.Columns.Add("Lower right");
               o.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
               o.FullRowSelect = true;
           });

            comboUTMZone.SelectedIndex = -1;
            comboSubGrid.SelectedIndex = -1;
            if (_IsNew)
            {
                comboUTMZone.SelectedIndex = 0;
                comboSubGrid.SelectedIndex = 0;
            }
        }

        private void LoadGrid25Items(ListView lv)
        {
            lv.Items.Clear();
            if (FishingGrid.GridType == FishingGrid.fadGridType.gridTypeGrid25)
            {
                foreach (var item in FishingGrid.Grid25.Bounds)
                {
                    var lvi = lv.Items.Add(item.gridDescription);
                    lvi.SubItems.Add(item.ulGridName);
                    lvi.SubItems.Add(item.lrGridName);
                    lvi.Name = lvi.Text;
                }
            }
        }

        private bool ValidateForm()
        {
            var msg = "Grid25 system requires UTM zone, subgrid style, and at least 1 map";
            bool FormValidated = false;
            if (txtName.Text.Length > 0 && txtCode.Text.Length > 0)
            {
                var tabPage = tabAOI.SelectedTab.Name;
                if (_IsNew)
                {
                    if (tabPage == "tabGrid25")
                    {
                        FormValidated = comboUTMZone.Text.Length > 0 &&
                                         comboSubGrid.Text.Length > 0 &&
                                         lvMaps.Items.Count > 0;
                    }
                    else
                    {
                        msg = "New target areas must use Grid25 system";
                    }
                }
                else
                {
                    switch (tabPage)
                    {
                        case "tabGrid25":
                            FormValidated = comboUTMZone.Text.Length > 0 &&
                                             comboSubGrid.Text.Length > 0 &&
                                             lvMaps.Items.Count > 0;
                            break;

                        case "tabOtherGrid":
                            if (FishingGrid.IsCompleteGrid25)
                            {
                                msg = "Cannot convert from Grid25 to other grid";
                            }
                            else
                            {
                                FormValidated = textBoxOtherGrid.Text.Length > 0;
                                msg = "Please fill up other grid information";
                            }
                            break;

                        case "tabMBR":
                            msg = "Please select Grid25 or Other grid";
                            break;
                    }
                }
            }
            else
            {
                msg = "Please fill up Target area name and code";
            }

            if (!FormValidated)
            {
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return FormValidated;
        }

        private bool SaveTargetArea()
        {
            var Success = false;
            var tabPage = tabAOI.SelectedTab.Name;

            if (_IsNew)
            {
                _AOI.AOIGUID = Guid.NewGuid().ToString();
            }

            Dictionary<string, string> TargetAreaData = new Dictionary<string, string>();
            TargetAreaData.Add("AOIName", txtName.Text);
            TargetAreaData.Add("Letter", txtCode.Text);
            TargetAreaData.Add("AOIGUID", _AOI.AOIGUID);
            TargetAreaData.Add("DataStatus", _IsNew ?
                                              global.fad3DataStatus.statusNew.ToString() :
                                              global.fad3DataStatus.statusEdited.ToString());

            if (aoi.UpdateData(TargetAreaData))
            {
                var Maps = new Dictionary<string, (string MapName, string ULGrid, string LRGrid)>();
                var FirstMap = "";
                var Zone = "";
                var SubGridStyle = 0;
                if (tabPage == "tabGrid25")
                {
                    //save grid25 data

                    foreach (ListViewItem lvi in lvMaps.Items)
                    {
                        var Map = (lvi.Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text);

                        //var Map = (MapName: lvi.Text, ULGrid: lvi.SubItems[1].Text, LRGrid: lvi.SubItems[2].Text);
                        Maps.Add(lvi.Text, Map);
                    }
                    FirstMap = lvMaps.Items[0].Text;
                    Zone = comboUTMZone.Text;
                    SubGridStyle = comboSubGrid.SelectedIndex;
                }
                else
                {
                    //save other grid data
                    Maps = null;
                }
                Success = FishingGrid.SaveTargetAreaGrid25(_AOI.AOIGUID, UseGrid25: tabPage == "tabGrid25",
                                Zone, SubGridStyle, Maps, FirstMap);
            }
            return Success;
        }

        private void OnMainButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (ValidateForm())
                    {
                        if (SaveTargetArea())
                        {
                            if (_IsNew)
                            {
                                _parent_form.NewTargetArea(txtName.Text, _AOI.AOIGUID);
                            }
                            else
                            {
                                FishingGrid.Refresh();
                                _parent_form.RefreshLVEx("aoi");
                            }
                            Close();
                        }
                    }

                    break;

                case "buttonCancel":
                    this.Close();
                    break;
            }
        }

        private void tabAOI_TabIndexChanged(object sender, EventArgs e)
        {
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
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
                fge = new FGExtentForm(this);
                fge.UTMZone = FishingGrid.ZoneFromZoneName(comboUTMZone.Text);
            }
            else
            {
                var UTMZone = FishingGrid.ZoneFromZoneName(comboUTMZone.Text);
                fge = new FGExtentForm(this, UTMZone, lvi.Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text);
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

        private void OnTextBoxValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var msg = "";
            ((TextBox)sender).With(o =>
            {
                if (o.Text.Length > 0)
                {
                    switch (o.Name)
                    {
                        case "txtName":
                            if (o.Text.Length < 4)
                            {
                                msg = "Target area name must be at least 5 letters long";
                                e.Cancel = true;
                            }
                            break;

                        case "txtCode":
                            if (o.Text.Length < 1 && o.Text.Length > 5)
                            {
                                msg = "Target area code must be from 1 to 5 letters long";
                                e.Cancel = true;
                            }
                            else
                            {
                                if (!global.TextIsAlphaNumeric(o.Text))
                                {
                                    msg = "Target area code must only contain letters and numbers";
                                    e.Cancel = true;
                                }
                            }
                            break;
                    }
                }
            });

            if (e.Cancel)
            {
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}