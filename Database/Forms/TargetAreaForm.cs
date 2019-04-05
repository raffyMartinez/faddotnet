﻿/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/10/2016
 * Time: 4:19 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using FAD3.Database.Classes;
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
        private TargetArea _targetArea = new TargetArea();
        private bool _isNew = false;
        private static TargetAreaForm _instance;
        private MainForm _parent_form;
        private int _MouseX;
        private int _MouseY;

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

        public MainForm Parent_form
        {
            get { return _parent_form; }
            set { _parent_form = value; }
        }

        public static TargetAreaForm GetInstance(MainForm Parent, bool IsNew = false)
        {
            if (_instance == null) _instance = new TargetAreaForm(Parent, IsNew);
            return _instance;
        }

        public TargetArea TargetArea
        {
            get { return _targetArea; }
            set
            {
                _targetArea = value;
            }
        }

        public void AddNew()
        {
            _isNew = true;
        }

        public TargetAreaForm(MainForm Parent, bool IsNew = false)
        {
            InitializeComponent();
            _isNew = IsNew;
            _parent_form = Parent;
        }

        public void ShowTargetAreaProperties()
        {
            textBoxOtherGrid.Text = "";

            var myAOIdata = _targetArea.TargetAreaDataEx();
            if (myAOIdata.Count > 0)
            {
                txtName.Text = myAOIdata["AOIName"];
                txtCode.Text = myAOIdata["Code"];
                lblTitle.Text = $"Properties of {txtName.Text} target area";
                var tabPageName = "";
                switch (FishingGrid.GridType)
                {
                    case fadGridType.gridTypeOther:
                        tabPageName = "tabOtherGrid";
                        break;

                    case fadGridType.gridTypeGrid25:
                        tabPageName = "tabGrid25";

                        LoadGrid25Items(lvMaps);
                        global.SizeListViewColumns(lvMaps, false);
                        comboUTMZone.Text = "";
                        comboUTMZone.Text = FishingGrid.UTMZoneName;
                        if(comboSubGrid.Items.Count==0)
                        {
                            foreach (var item in FishingGrid.SubGridStyles)
                            {
                                comboSubGrid.Items.Add(item);
                            }
                        }
                        switch(FishingGrid.SubGridStyle)
                        {
                            case fadSubgridSyle.SubgridStyleNone:
                                comboSubGrid.SelectedIndex = 0;
                                break;
                            case fadSubgridSyle.SubgridStyle4:
                                comboSubGrid.SelectedIndex = 1;
                                break;
                            case fadSubgridSyle.SubgridStyle9:
                                comboSubGrid.SelectedIndex = 2;
                                break;
                        }

                        break;
                }

                if (FishingGrid.GridType != fadGridType.gridTypeNone)
                {
                    var mytab = tabAOI.TabPages[tabPageName];
                    mytab.Select();
                }
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);
            if (_isNew)
            {
                lblTitle.Text = "Add new target area";
            }


            var lv = (ListView)tabAOI.TabPages["tabGrid25"].Controls["lvMaps"];

            foreach (var item in FishingGrid.UTMZones)
            {
                comboUTMZone.Items.Add(item);
            }

            if (comboSubGrid.Items.Count == 0)
            {
                foreach (var item in FishingGrid.SubGridStyles)
                {
                    comboSubGrid.Items.Add(item);
                }
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

            global.SizeListViewColumns(lv);

            comboUTMZone.SelectedIndex = -1;


            if (_isNew)
            {
                comboUTMZone.SelectedIndex = 0;
                comboSubGrid.SelectedIndex = 0;
            }

            if(_targetArea!=null)
            {
                ShowTargetAreaProperties();
            }
            txtName.Focus();
        }

        private void LoadGrid25Items(ListView lv)
        {
            lv.Items.Clear();
            if (FishingGrid.GridType == fadGridType.gridTypeGrid25)
            {
                foreach (var item in FishingGrid.Grid25.BoundsEx)
                {
                    var lvi = lv.Items.Add(item.Value.gridDescription);
                    lvi.SubItems.Add(item.Value.ulGridName);
                    lvi.SubItems.Add(item.Value.lrGridName);
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
                if (_isNew)
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

            if (_isNew)
            {
                _targetArea.TargetAreaGuid = Guid.NewGuid().ToString();
            }

            Dictionary<string, string> TargetAreaData = new Dictionary<string, string>();
            TargetAreaData.Add("AOIName", txtName.Text);
            TargetAreaData.Add("Letter", txtCode.Text);
            TargetAreaData.Add("AOIGUID", _targetArea.TargetAreaGuid);
            TargetAreaData.Add("DataStatus", _isNew ?
                                              fad3DataStatus.statusNew.ToString() :
                                              fad3DataStatus.statusEdited.ToString());
            TargetAreaData.Add("SubGridStyle", comboSubGrid.SelectedIndex.ToString());

            if (TargetArea.UpdateData(TargetAreaData))
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
                Success = FishingGrid.SaveTargetAreaGrid25(_targetArea.TargetAreaGuid, UseGrid25: tabPage == "tabGrid25",
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
                            if (_isNew)
                            {
                                _parent_form.NewTargetArea(txtName.Text, _targetArea.TargetAreaGuid);
                            }
                            else
                            {
                                FishingGrid.Refresh();
                                _parent_form.RefreshLV("target_area");
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


        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            if (FishingGrid.GridType == fadGridType.gridTypeGrid25)
                FishingGrid.SubGridStyle = (fadSubgridSyle)comboSubGrid.SelectedIndex;

            _instance = null;
            global.SaveFormSettings(this);
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
            FishingGroundDefinitionForm fge;
            if (lvi == null)
            {
                fge = new FishingGroundDefinitionForm(this);
                fge.UTMZone = FishingGrid.ZoneFromZoneName(comboUTMZone.Text);
            }
            else
            {
                var UTMZone = FishingGrid.ZoneFromZoneName(comboUTMZone.Text);
                fge = new FishingGroundDefinitionForm(this, UTMZone, lvi.Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text);
            }
            fge.ShowDialog(Parent_form);
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
                    var itemText = lvMaps.SelectedItems[0].Text;
                    if (FishingGrid.DeleteFishingGroundMap(itemText, _targetArea.TargetAreaGuid))
                    {
                        lvMaps.Items.RemoveByKey(itemText);
                    }
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
                            if (_isNew && o.Text.Length < 4)
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