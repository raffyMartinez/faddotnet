using FAD3.Database.Classes;
using FAD3.Database.Forms;
using FAD3.Mapping.Classes;
using FAD3.Mapping.Forms;
using MetaphoneCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace FAD3
{
    public partial class GearCodesUsageForm : Form
    {
        private string _gearClassName = "";
        private string _gearClassGuid = "";
        private string _gearVarName = "";
        private string _gearVarGuid = "";
        private string _gearRefCode = "";
        private string _targetAreaName = "";
        private string _targetAreaGuid = "";
        private string _targetAreaUsageRow = "";
        private string _localNameGuid = "";
        private string _localName;
        private fad3GearEditAction _action;
        private string _currentList;

        private SamplingForm _parentForm;
        private static GearCodesUsageForm _instance;

        public static GearCodesUsageForm GetInstance(string gearVarGuid = "", string targetAreaGuid = "", string gearClassName = "")
        {
            if (_instance == null) _instance = new GearCodesUsageForm(gearVarGuid, targetAreaGuid, gearClassName);
            return _instance;
        }

        public void TargetArea(string targetAreaName, string targetAreaGuid)
        {
            _targetAreaName = targetAreaName;
            _targetAreaGuid = targetAreaGuid;
        }

        public string GearRefCode
        {
            get { return _gearRefCode; }
            set { _gearRefCode = value; }
        }

        public void GearVariation(string variationName, string variationGUID)
        {
            _gearVarName = variationName;
            _gearVarGuid = variationGUID;
        }

        public string GearClassName
        {
            get { return _gearClassName; }
            set { _gearClassName = value; }
        }

        public SamplingForm Parent_Form
        {
            get { return _parentForm; }
            set { _parentForm = value; }
        }

        public GearCodesUsageForm(string gearVarGuid = "", string targetAreaGuid = "", string gearClassName = "")
        {
            InitializeComponent();
            _gearVarGuid = gearVarGuid;
            _targetAreaGuid = targetAreaGuid;
            _gearClassName = gearClassName;
        }

        private static void FillGearVarSpecsColumn(ListView lv)
        {
            foreach (ListViewItem lvi in lv.Items)
            {
                lvi.SubItems.Add("");
                if (Gears.GearVarHasSpecsTemplate(lvi.Name))
                    lvi.SubItems[1].Text = "x";
            }
        }

        public void PopulateLists()
        {
            //fill the combobox with gear class names
            ReadGearClass();

            if (_gearClassName.Length > 0)
            {
                comboClass.Text = _gearClassName;
            }
            else
            {
                comboClass.SelectedItem = comboClass.Items[0];
            }

            var ch = new ColumnHeader();

            foreach (Control c in Controls)
            {
                if (c.GetType().Name == "ListView")
                {
                    ((ListView)c).With(o =>
                    {
                        o.View = View.Details;
                        o.HeaderStyle = ColumnHeaderStyle.Nonclickable;
                        o.FullRowSelect = true;
                        o.HideSelection = false;
                    });
                }
            }

            //fill this listview with gear variations belonging to a class
            var lv = listViewVariations;
            FillVariationsList();
            if (_gearVarGuid.Length > 0)
                lv.Items[_gearVarGuid].Selected = true;
            else
            {
                if (lv.Items.Count > 0)
                {
                    lv.Items[0].Selected = true;
                    _gearVarGuid = lv.Items[0].Name;
                }
            }
            FillGearVarSpecsColumn(lv);
            SizeColumns(lv, false);

            //fill this list view with gear codes belonging to a variation
            lv = listViewCodes;
            lv.Columns.Add("Code");
            lv.Columns.Add("Sub-variation");
            SizeColumns(lv);
            FillRefCodeList();
            SizeColumns(lv, false);
            if (_gearRefCode.Length > 0 && lv.Items.ContainsKey(_gearRefCode))
                lv.Items[_gearRefCode].Selected = true;
            else
            {
                if (lv.Items.Count > 0)
                {
                    lv.Items[0].Selected = true;
                    _gearRefCode = lv.Items[0].Name;
                }
            }

            //fill this list view with target areas where the variation is used
            lv = listViewWhereUsed;
            lv.Columns.Add("Target area of use");
            SizeColumns(lv);
            if (_gearRefCode.Length > 0)
            {
                FillRefCodeUsage();
                SizeColumns(lv, false);
                if (_targetAreaGuid.Length > 0)
                {
                    if (lv.Items.Count > 0)
                    {
                        lv.Items[_targetAreaGuid].Selected = true;
                    }
                }
                else
                {
                    if (lv.Items.Count > 0)
                    {
                        lv.Items[0].Selected = true;
                        _targetAreaGuid = lv.Items[0].Name;
                    }
                }
            }

            //fill this list view with the local names used in a target area

            lv = listViewLocalNames;
            ch = lv.Columns.Add("Local name");
            SizeColumns(lv);
            if (_targetAreaGuid.Length > 0)
            {
                FillLocalNames();
                SizeColumns(lv, false);
            }
        }

        /// <summary>
        /// Sizes all columns so that it fits the widest column content or the column header content
        /// </summary>
        private void SizeColumns(ListView lv, bool init = true)
        {
            foreach (ColumnHeader c in lv.Columns)
            {
                if (init)
                {
                    c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                    c.Tag = c.Width;
                }
                else
                {
                    c.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    c.Width = c.Width > (int)c.Tag ? c.Width : (int)c.Tag;
                }
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            var lv = listViewVariations;
            lv.Columns.Add("Variation");
            lv.Columns.Add("Specs");
            SizeColumns(lv);
            PopulateLists();
        }

        private void FillLocalNames()
        {
            listViewLocalNames.Items.Clear();
            var list = Gears.GearLocalName_TargetArea(_gearRefCode, _targetAreaGuid);
            foreach (var i in list)
            {
                var lvi = new ListViewItem
                {
                    Name = i.Key,
                    Text = i.Value.LocalName,
                    Tag = i.Value.RowNumber
                };
                listViewLocalNames.Items.Add(lvi);
            }
        }

        private void FillRefCodeUsage()
        {
            listViewWhereUsed.Items.Clear();
            var list = Gears.TargetAreaUsed_RefCode(_gearRefCode);
            foreach (var i in list)
            {
                var lvi = new ListViewItem
                {
                    Name = i.Key,
                    Text = i.Value.AOIName,
                    Tag = i.Value.RowNumber
                };
                listViewWhereUsed.Items.Add(lvi);
            }
        }

        private void FillRefCodeList()
        {
            listViewCodes.Items.Clear();
            var list = Gears.GearSubVariations(_gearVarGuid);
            foreach (KeyValuePair<string, bool> kv in list)
            {
                var lvi = new ListViewItem
                {
                    Name = kv.Key,
                    Text = kv.Key
                };
                listViewCodes.Items.Add(lvi);
                lvi.SubItems.Add(kv.Value.ToString());
            }
        }

        private void FillVariationsList()
        {
            listViewVariations.Items.Clear();
            if (comboClass.Items.Count > 0)
            {
                var key = ((KeyValuePair<string, string>)comboClass.SelectedItem).Key;
                var list = Gears.GearVariationsWithSpecs(key);
                foreach (var item in list)
                {
                    var lvi = new ListViewItem
                    {
                        Name = item.Item1,
                        Text = item.Item2
                    };
                    lvi.SubItems.Add(item.Item3 ? "x" : "");
                    listViewVariations.Items.Add(lvi);
                }
            }
        }

        private void ReadGearClass()
        {
            comboClass.Items.Clear();
            Gears.RefreshGearClasses();
            foreach (var item in Gears.GearClasses)
            {
                KeyValuePair<string, string> gear = new KeyValuePair<string, string>(item.Key, item.Value.GearClassName);
                comboClass.Items.Add(gear);
            }
            comboClass.With(o =>
            {
                o.DisplayMember = "Value";
                o.ValueMember = "Key";
                o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                o.AutoCompleteSource = AutoCompleteSource.ListItems;
            });
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnlistView_Click(object sender, EventArgs e)
        {
            var lv = (ListView)sender;
            var lvi = lv.SelectedItems[0];
            switch (lv.Name)
            {
                case "listViewCodes":
                    _gearRefCode = lvi.Name;

                    listViewWhereUsed.Items.Clear();
                    listViewLocalNames.Items.Clear();

                    FillRefCodeUsage();
                    if (listViewWhereUsed.Items.Count > 0)
                    {
                        if (!listViewWhereUsed.Items.ContainsKey(_targetAreaGuid))
                        {
                            listViewWhereUsed.Items[0].Selected = true;
                            _targetAreaGuid = listViewWhereUsed.Items[0].Name;
                        }
                        else
                        {
                            listViewWhereUsed.Items[_targetAreaGuid].Selected = true;
                        }
                        FillLocalNames();
                    }
                    break;

                case "listViewLocalNames":
                    break;

                case "listViewVariations":
                    _gearVarGuid = lvi.Name;
                    _gearVarName = lvi.Text;

                    listViewCodes.Items.Clear();
                    listViewWhereUsed.Items.Clear();
                    listViewLocalNames.Items.Clear();

                    FillRefCodeList();
                    if (listViewCodes.Items.Count > 0)
                    {
                        _gearRefCode = listViewCodes.Items[0].Name;
                        listViewCodes.Items[_gearRefCode].Selected = true;
                        FillRefCodeUsage();
                        if (listViewWhereUsed.Items.Count > 0)
                        {
                            if (!listViewWhereUsed.Items.ContainsKey(_targetAreaGuid))
                            {
                                listViewWhereUsed.Items[0].Selected = true;
                                _targetAreaGuid = listViewWhereUsed.Items[0].Name;
                            }
                            else
                            {
                                listViewWhereUsed.Items[_targetAreaGuid].Selected = true;
                            }
                            FillLocalNames();
                        }
                    }
                    break;

                case "listViewWhereUsed":
                    _targetAreaGuid = lvi.Name;
                    _targetAreaUsageRow = lvi.Tag.ToString();
                    FillLocalNames();
                    break;
            }
        }

        private void OnListView_MouseDown(object sender, MouseEventArgs e)
        {
            var lv = (ListView)sender;
            _currentList = lv.Name;
            ListViewHitTestInfo info = lv.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                if (info.Item != null)
                {
                    switch (lv.Name)
                    {
                        case "listViewCodes":
                            _targetAreaGuid = "";
                            _targetAreaUsageRow = "";
                            break;

                        case "listViewWhereUsed":
                            break;

                        case "listViewVariations":
                            _gearRefCode = "";
                            _targetAreaGuid = "";
                            break;

                        case "listViewLocalNames":
                            break;
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                dropDownMenu.Items.Clear();

                switch (_currentList)
                {
                    case "listViewWhereUsed":
                        var tsi = dropDownMenu.Items.Add("Add target area where used");
                        tsi.Name = "itemAddTargetArea";
                        tsi.Enabled = _gearRefCode.Length > 0;

                        tsi = dropDownMenu.Items.Add("Delete target area");
                        tsi.Name = "itemDeleteTargetArea";
                        tsi.Enabled = info.Item != null;

                        dropDownMenu.Items.Add("-");

                        tsi = dropDownMenu.Items.Add("Map fishing ground");
                        tsi.Name = "itemMapGearFishingGround";
                        tsi.Enabled = global.MapIsOpen;

                        break;

                    case "listViewVariations":
                        tsi = dropDownMenu.Items.Add("Samplings using this gear");
                        tsi.Name = "itemGearSamplings";
                        tsi.Enabled = info.Item != null;

                        tsi = dropDownMenu.Items.Add("Add a gear variation");
                        tsi.Name = "itemAddGearVariation";
                        tsi.Enabled = comboClass.SelectedItem != null;

                        tsi = dropDownMenu.Items.Add("Edit gear variation");
                        tsi.Name = "itemEditGearVariation";
                        tsi.Enabled = info.Item != null;

                        tsi = dropDownMenu.Items.Add("Delete gear variation");
                        tsi.Name = "itemDeleteGearVariation";
                        tsi.Enabled = info.Item != null;

                        tsi = dropDownMenu.Items.Add("Delete gear variation (mulitple)");
                        tsi.Name = "itemDeleteGearVariationMultiple";
                        tsi.Enabled = info.Item != null;

                        dropDownMenu.Items.Add("-");

                        tsi = dropDownMenu.Items.Add("Gear specs");
                        tsi.Name = "itemManageGearSpecs";
                        tsi.Enabled = info.Item != null;

                        dropDownMenu.Items.Add("-");

                        tsi = dropDownMenu.Items.Add("Map fishing ground");
                        tsi.Name = "itemMapGearFishingGround";
                        tsi.Enabled = global.MapIsOpen;
                        break;

                    case "listViewLocalNames":
                        tsi = dropDownMenu.Items.Add("Add a gear local name");
                        tsi.Name = "itemAddLocalName";
                        tsi.Enabled = _targetAreaGuid.Length > 0;

                        tsi = dropDownMenu.Items.Add("Delete gear local name");
                        tsi.Name = "itemDeleteLocalName";
                        tsi.Enabled = info.Item != null;

                        tsi = dropDownMenu.Items.Add("Edit gear local name");
                        tsi.Name = "itemEditLocalName";
                        tsi.Enabled = info.Item != null;
                        break;

                    case "listViewCodes":
                        tsi = dropDownMenu.Items.Add("Add a gear code");
                        tsi.Name = "itemAddGearCode";
                        tsi.Enabled = _gearVarGuid.Length > 0;

                        tsi = dropDownMenu.Items.Add("Delete gear code");
                        tsi.Name = "itemDeleteGearCode";
                        tsi.Enabled = info.Item != null;

                        break;
                }
            }
        }

        private void OnDropDownMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var myList = new List<string>();
            e.ClickedItem.Owner.Hide();
            var RowNumberForDeletion = "";
            switch (e.ClickedItem.Name)
            {
                case "itemGearSamplings":

                    var ef = GearSpeciesSamplingsForm.GetInstance(_gearVarGuid, _gearVarName, this, OccurenceDataType.Gear);
                    if (!ef.Visible)
                    {
                        ef.Show(this);
                    }
                    else
                    {
                        ef.BringToFront();
                        ef.setItemGuid_Name_Parent(_gearVarGuid, _gearVarName, this);
                    }
                    break;

                case "itemAddTargetArea":
                    _action = fad3GearEditAction.addAOI;
                    myList = listViewWhereUsed.Items.Cast<ListViewItem>()
                                                     .Select(item => item.Text)
                                                     .ToList();
                    break;

                case "itemAddGearVariation":
                    _action = fad3GearEditAction.addGearVariation;
                    break;

                case "itemAddLocalName":
                    _action = fad3GearEditAction.addLocalName;
                    myList = listViewLocalNames.Items.Cast<ListViewItem>()
                                                     .Select(item => item.Text)
                                                     .ToList();
                    break;

                case "itemAddGearCode":
                    _action = fad3GearEditAction.addGearCode;
                    break;

                case "itemDeleteGearCode":
                    MessageBox.Show("Deleting a gear code is not enabled");
                    break;

                case "itemManageGearSpecs":
                    //reserved
                    break;

                case "itemDeleteTargetArea":
                    RowNumberForDeletion = listViewWhereUsed.SelectedItems[0].Tag.ToString();
                    if (MessageBox.Show("Please confirm that you want to delete this target area",
                                        "Confirmation needed", MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Exclamation) == DialogResult.OK)
                    {
                        var result = Gears.DeleteTargetAreaUsage(RowNumberForDeletion);
                        if (result.success)
                        {
                            listViewWhereUsed.Items.Remove(listViewWhereUsed.SelectedItems[0]);
                        }
                        else
                        {
                            MessageBox.Show(result.message, "Deletion status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    break;

                case "itemDeleteLocalName":
                    RowNumberForDeletion = listViewLocalNames.SelectedItems[0].Tag.ToString();
                    if (MessageBox.Show("Please confirm that you want to delete this local name",
                                        "Confirmation needed", MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Exclamation) == DialogResult.OK)
                    {
                        if (Gears.DeleteLocalNameUsage(RowNumberForDeletion))
                        {
                            listViewLocalNames.Items.Remove(listViewLocalNames.SelectedItems[0]);
                        }
                    }
                    break;

                case "itemEditLocalName":
                    using (GearEditorForm gef = new GearEditorForm(this))
                    {
                        gef.GearLocalName = listViewLocalNames.SelectedItems[0].Text;
                        gef.Action = fad3GearEditAction.editLocalName;
                        DialogResult dr = gef.ShowDialog(this);
                        if (dr == DialogResult.OK)
                        {
                            listViewLocalNames.SelectedItems[0].Text = gef.GearLocalName;
                        }
                    }
                    break;

                case "itemDeleteGearVariationMultiple":
                    using (DeleteMultipleGearVariationsForm dmgf = new DeleteMultipleGearVariationsForm())
                    {
                        dmgf.ShowDialog();
                        if (dmgf.DialogResult == DialogResult.OK)
                        {
                        }
                    }

                    break;

                case "itemEditGearVariation":
                    using (GearEditorForm gef = new GearEditorForm(this))
                    {
                        gef.GearVariationName = listViewVariations.SelectedItems[0].Text;
                        gef.Action = fad3GearEditAction.editGearVariation;
                        DialogResult dr = gef.ShowDialog(this);
                        if (dr == DialogResult.OK)
                        {
                            listViewVariations.SelectedItems[0].Text = gef.GearVariationName;
                        }
                    }
                    break;

                case "itemDeleteGearVariation":

                    if (_gearVarGuid.Length > 0)
                    {
                        if (MessageBox.Show("Please confirm that you want to delete this variation",
                                            "Confirmation needed", MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Exclamation) == DialogResult.OK)
                        {
                            var result = Gears.DeleteGearVariation(_gearVarGuid);
                            if (!result.success)
                            {
                                var reason = result.reason;
                                if (reason.Length == 0)
                                    reason = $"Was not able to delete because the gear is used in {result.recordCount} samplings";

                                MessageBox.Show(reason, "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                //refresh the list of gear variations
                                FillVariationsList();
                            }
                        }
                    }
                    break;

                case "itemMapGearFishingGround":
                    var targetAreaGuid = "";
                    switch (_currentList)
                    {
                        case "listViewWhereUsed":
                            targetAreaGuid = listViewWhereUsed.SelectedItems[0].Name;
                            break;

                        case "listViewVariations":
                            break;
                    }
                    OccurenceMapping mso = new OccurenceMapping(OccurenceDataType.Gear, _gearVarName, global.MappingForm.MapControl)
                    {
                        MapLayersHandler = global.MappingForm.MapLayersHandler,
                        MapInteractionHandler = global.MappingForm.MapInterActionHandler
                    };
                    mso.RequestOccurenceInfo += OnRequestOccurenceInfo;
                    mso.MapOccurence();
                    break;
            }

            switch (e.ClickedItem.Name)
            {
                case "itemAddTargetArea":
                case "itemAddGearVariation":
                case "itemAddLocalName":
                case "itemAddGearCode":
                    if (comboClass.SelectedItem != null)
                    {
                        _gearClassGuid = ((KeyValuePair<string, string>)comboClass.SelectedItem).Key;
                        GearEditorForm f = new GearEditorForm(this)
                        {
                            GearClassGuid = _gearClassGuid,
                            GearVariationGuid = _gearVarGuid,
                            Action = _action,
                            InList = myList
                        };
                        f.ShowDialog(this);
                    }
                    break;

                case "itemManageGearSpecs":
                    ManageGearSpecsForm ff = new ManageGearSpecsForm(listViewVariations.SelectedItems[0].Name,
                                                             listViewVariations.SelectedItems[0].Text);
                    ff.ShowDialog(this);

                    //refresh the list to show new changes
                    FillVariationsList();
                    break;
            }
        }

        private void OnRequestOccurenceInfo(object sender, OccurenceMapEventArgs e)
        {
            if (e.OccurenceDataType == OccurenceDataType.Gear)
            {
                var nameToMap = _gearVarName;
                var gearVarGuid = _gearVarGuid;
                using (OccurenceMappingForm omf = new OccurenceMappingForm(e.OccurenceDataType, this))
                {
                    if (_currentList == "listViewVariations")
                    {
                        omf.SelectedTargetAreaGuid = "";
                    }
                    else
                    {
                        omf.SelectedTargetAreaGuid = _targetAreaGuid;
                    }
                    omf.GearToMap(nameToMap, gearVarGuid);
                    omf.ShowDialog(this);
                    if (omf.DialogResult == DialogResult.OK)
                    {
                        e.Aggregate = omf.Aggregate;
                        e.ExcludeOne = omf.ExcludeOne;
                        e.MapInSelectedTargetArea = omf.MapInSelectedTargetArea;
                        e.SelectedTargetAreaGuid = omf.SelectedTargetAreaGuid;
                        e.SamplingYears = omf.SamplingYears;
                        e.ItemToMapGuid = gearVarGuid;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        public void UsageTargetArea(string targetAreaGuid, string targetAreaName)
        {
            _targetAreaGuid = targetAreaGuid;
            _targetAreaName = targetAreaName;

            if (_gearRefCode.Length > 0 && _targetAreaGuid.Length > 0)
            {
                var result = Gears.AddGearCodeUsageTargetArea(_gearRefCode, _targetAreaGuid);
                if (result.Success)
                {
                    var newLVI = listViewWhereUsed.Items.Add(_targetAreaGuid, _targetAreaName, null);
                    newLVI.Tag = result.NewRow;
                    _targetAreaUsageRow = result.NewRow;

                    if (_parentForm != null)
                        _parentForm.GearVariationUseRefresh();
                }
            }
        }

        public void UsageLocalName(string localNameGuid, string localName)
        {
            _localNameGuid = localNameGuid;
            _localName = localName;

            if (_gearRefCode.Length > 0 && _targetAreaGuid.Length > 0 && _localNameGuid.Length > 0 && _targetAreaUsageRow.Length > 0)
            {
                var result = Gears.AddUsageLocalName(_targetAreaUsageRow, _localNameGuid);
                if (result.Success)
                {
                    var newLVI = listViewLocalNames.Items.Add(_localNameGuid, _localName, null);
                    newLVI.Tag = result.NewRow;
                }
            }
        }

        public void UsageGearCode(string gearCode, bool isVariation)
        {
            _gearRefCode = gearCode;
            if (_gearVarGuid.Length > 0)
            {
                if (Gears.AddGearVariationReferenceCode(_gearRefCode, _gearVarGuid, isVariation))
                {
                    var lvi = listViewCodes.Items.Add(_gearRefCode, _gearRefCode, null);
                    lvi.SubItems.Add(isVariation.ToString());
                }
            }
        }

        public void UsageGearVariation(string gearVariationName)
        {
            _gearVarName = gearVariationName;
            if (_gearClassGuid.Length > 0)
            {
                var dms = new DoubleMetaphoneShort();
                dms.ComputeMetaphoneKeys(gearVariationName, out short key1, out short key2);
                var similarNames = Gears.GearSoundsLike(key1, key2);
                var proceed = true;

                var sb = new StringBuilder();
                if (similarNames.Count > 0)
                {
                    sb.Capacity = similarNames.Count + 3;
                    sb.Append("Selected gear has possible duplicates\r\n\r\n");
                    sb.Append("Gear name\tMatch strength\r\n");
                    foreach ((string gearName, string matchQuality) item in similarNames)
                    {
                        sb.Append($"{item.gearName}\t{item.matchQuality}\r\n");
                    }
                    sb.Append("\r\nPlease confirm if you still want to add a new gear");
                }

                proceed = (similarNames.Count == 0 ||
                           MessageBox.Show(sb.ToString(), $"Possible duplicates for {gearVariationName}",
                           MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK);

                if (proceed)
                {
                    var result = Gears.AddGearVariation(_gearClassGuid, gearVariationName);
                    if (result.success)
                    {
                        _gearVarGuid = result.newGuid;
                        listViewVariations.Items.Add(_gearVarGuid, gearVariationName, null);
                    }
                }
            }
        }

        private void Export(ExportImportDataType whatToExport)
        {
            FileDialogHelper.Title = "Provide file name for exported data";
            FileDialogHelper.DialogType = FileDialogType.FileSave;
            FileDialogHelper.DataFileType = DataFileType.Text | DataFileType.XML | DataFileType.CSV;
            FileDialogHelper.ShowDialog();
            var fileName = FileDialogHelper.FileName;
            if (fileName.Length > 0)
            {
                switch (Path.GetExtension(fileName))
                {
                    case ".txt":
                        break;

                    case ".csv":
                        break;

                    case ".xml":
                    case ".XML":
                        switch (whatToExport)
                        {
                            case ExportImportDataType.GearsRefCode:
                                var codes = Gears.GetGearRefCodes();
                                var count = codes.Count;

                                if (count > 0)
                                {
                                    var n = 0;
                                    XmlWriter writer = XmlWriter.Create(fileName);
                                    writer.WriteStartDocument();
                                    writer.WriteStartElement("GearReferenceCodes");
                                    foreach (var code in codes)
                                    {
                                        writer.WriteStartElement("GearReferenceCode");
                                        writer.WriteAttributeString("code", code.Key);
                                        writer.WriteAttributeString("gearVariationGuid", code.Value.variationGuid);
                                        writer.WriteAttributeString("gearVariationName", code.Value.variationName);
                                        writer.WriteAttributeString("isSubVariation", code.Value.isSubVariation.ToString());
                                        if (count == 1)
                                        {
                                            writer.WriteEndDocument();
                                        }
                                        else
                                        {
                                            if (n < (count - 1))
                                            {
                                                writer.WriteEndElement();
                                            }
                                            else
                                            {
                                                writer.WriteEndDocument();
                                            }
                                        }
                                        n++;
                                    }
                                    writer.Close();
                                    if (n > 0 && count > 0)
                                    {
                                        MessageBox.Show($"Succesfully exported {count} gear reference codes", "Export successful");
                                    }
                                }
                                break;

                            case ExportImportDataType.GearsVariation:
                                var gearsDict = Gears.GetAllVariations();
                                count = gearsDict.Count;

                                if (count > 0)
                                {
                                    var n = 0;
                                    XmlWriter writer = XmlWriter.Create(fileName);
                                    writer.WriteStartDocument();
                                    writer.WriteStartElement("GearVariations");
                                    foreach (var gear in gearsDict)
                                    {
                                        writer.WriteStartElement("GearVariation");
                                        writer.WriteAttributeString("guid", gear.Key);
                                        writer.WriteAttributeString("name", gear.Value.VariationName);
                                        writer.WriteAttributeString("gear_class", gear.Value.GearClassGuid);
                                        writer.WriteAttributeString("mph1", gear.Value.MetaphoneKey1.ToString());
                                        writer.WriteAttributeString("mph2", gear.Value.MetaphoneKey2.ToString());
                                        writer.WriteAttributeString("name2", gear.Value.VariationName2);
                                        if (count == 1)
                                        {
                                            writer.WriteEndDocument();
                                        }
                                        else
                                        {
                                            if (n < (count - 1))
                                            {
                                                writer.WriteEndElement();
                                            }
                                            else
                                            {
                                                writer.WriteEndDocument();
                                            }
                                        }
                                        n++;
                                    }
                                    writer.Close();
                                    if (n > 0 && count > 0)
                                    {
                                        MessageBox.Show($"Succesfully exported {count} gear variations", "Export successful");
                                    }
                                }
                                break;

                            case ExportImportDataType.GearsClass:
                                //var gearClassDict = Gears.GetGearClassEx();
                                Gears.RefreshGearClasses();
                                count = Gears.GearClasses.Count();

                                if (count > 0)
                                {
                                    var n = 0;
                                    XmlWriter writer = XmlWriter.Create(fileName);
                                    writer.WriteStartDocument();
                                    writer.WriteStartElement("GearClasses");
                                    foreach (var gearClass in Gears.GearClasses)
                                    {
                                        writer.WriteStartElement("GearClass");
                                        writer.WriteAttributeString("guid", gearClass.Key);
                                        writer.WriteAttributeString("name", gearClass.Value.GearClassName);
                                        writer.WriteAttributeString("code", gearClass.Value.GearClassLetter);
                                        if (count == 1)
                                        {
                                            writer.WriteEndDocument();
                                        }
                                        else
                                        {
                                            if (n < (count - 1))
                                            {
                                                writer.WriteEndElement();
                                            }
                                            else
                                            {
                                                writer.WriteEndDocument();
                                            }
                                        }
                                        n++;
                                    }
                                    writer.Close();
                                    if (n > 0 && count > 0)
                                    {
                                        MessageBox.Show($"Succesfully exported {count} gear classes", "Export successful");
                                    }
                                }
                                break;

                            case ExportImportDataType.GearsLocalName:
                                break;
                        }
                        break;
                }
            }
        }

        private void Import(ExportImportDataType whatToImport)
        {
            FileDialogHelper.Title = "Select file to import";
            FileDialogHelper.DialogType = FileDialogType.FileOpen;
            FileDialogHelper.DataFileType = DataFileType.Text | DataFileType.XML | DataFileType.CSV;
            FileDialogHelper.ShowDialog();
            var fileName = FileDialogHelper.FileName;
            var saveCounter = 0;
            var elementCounter = 0;
            var proceed = false;
            var msg = "";
            if (fileName.Length > 0)
            {
                switch (Path.GetExtension(fileName))
                {
                    case ".txt":
                        break;

                    case ".csv":
                        break;

                    case ".xml":
                    case ".XML":
                        switch (whatToImport)
                        {
                            case ExportImportDataType.GearsClass:
                                XmlTextReader xmlReader = new XmlTextReader(fileName);
                                string gearClassGuid = "";
                                string gearClass = "";
                                string gearClassCode = "";
                                while ((elementCounter == 0 || (elementCounter > 0 && proceed)) && xmlReader.Read())
                                {
                                    switch (xmlReader.NodeType)
                                    {
                                        case XmlNodeType.Element:
                                            if (elementCounter == 0 && xmlReader.Name == "GearClasses")
                                            {
                                                proceed = true;
                                            }
                                            if (xmlReader.Name == "GearClass")
                                            {
                                                gearClassGuid = xmlReader.GetAttribute("guid");
                                                gearClass = xmlReader.GetAttribute("name");
                                                gearClassCode = xmlReader.GetAttribute("code");
                                                elementCounter++;
                                            }

                                            break;
                                    }

                                    if (gearClassGuid?.Length > 0 && gearClass?.Length > 0 && gearClassCode.Length > 0)
                                    {
                                        var success = Gears.SaveNewGearClass(gearClassGuid, gearClass, gearClassCode);
                                        if (success)
                                        {
                                            saveCounter++;
                                        }
                                        gearClassGuid = "";
                                        gearClass = "";
                                        gearClassCode = "";
                                    }
                                }
                                if (saveCounter == 0)
                                {
                                    msg = "No gear classes was imported into the database";
                                }
                                else
                                {
                                    msg = $"{saveCounter.ToString()} gear classes saved to the database";
                                    ReadGearClass();
                                }
                                MessageBox.Show(msg, "Import gear classes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;

                            case ExportImportDataType.GearsVariation:
                                xmlReader = new XmlTextReader(fileName);
                                string gearVariation = "";
                                string gearVariationGuid = "";
                                gearClassGuid = "";
                                int mph1 = 0;
                                int mph2 = 0;
                                string name2 = "";
                                while ((elementCounter == 0 || (elementCounter > 0 && proceed)) && xmlReader.Read())
                                {
                                    switch (xmlReader.NodeType)
                                    {
                                        case XmlNodeType.Element:
                                            if (elementCounter == 0 && xmlReader.Name == "GearVariations")
                                            {
                                                proceed = true;
                                            }
                                            if (xmlReader.Name == "GearVariation")
                                            {
                                                gearVariationGuid = xmlReader.GetAttribute("guid");
                                                gearVariation = xmlReader.GetAttribute("name");
                                                gearClassGuid = xmlReader.GetAttribute("gear_class");
                                                mph1 = int.Parse(xmlReader.GetAttribute("mph1"));
                                                mph2 = int.Parse(xmlReader.GetAttribute("mph2"));
                                                name2 = xmlReader.GetAttribute("name2");
                                                elementCounter++;
                                            }

                                            break;
                                    }

                                    if (gearVariation?.Length > 0 && gearVariationGuid?.Length > 0 && gearClassGuid.Length > 0)
                                    {
                                        NewFisheryObjectName nfon = new NewFisheryObjectName(gearVariation, FisheryObjectNameType.GearVariationName);
                                        var result = Gears.SaveNewVariationName(nfon, gearClassGuid, gearVariationGuid);
                                        if (result.success)
                                        {
                                            saveCounter++;
                                        }
                                        gearVariation = "";
                                        gearVariationGuid = "";
                                        gearClassGuid = "";
                                        mph1 = 0;
                                        mph2 = 0;
                                        name2 = "";
                                    }
                                }
                                if (saveCounter == 0)
                                {
                                    msg = "No gear variations was imported into the database";
                                }
                                else
                                {
                                    msg = $"{saveCounter.ToString()} gear variation saved to the database";
                                    FillVariationsList();
                                    SizeColumns(listViewVariations, false);
                                }
                                MessageBox.Show(msg, "Import gear variation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;

                            case ExportImportDataType.GearsRefCode:
                                xmlReader = new XmlTextReader(fileName);
                                string gearCode = "";
                                gearVariationGuid = "";
                                gearVariation = "";
                                bool isSubVariation = false;
                                while ((elementCounter == 0 || (elementCounter > 0 && proceed)) && xmlReader.Read())
                                {
                                    switch (xmlReader.NodeType)
                                    {
                                        case XmlNodeType.Element:
                                            if (elementCounter == 0 && xmlReader.Name == "GearReferenceCodes")
                                            {
                                                proceed = true;
                                            }
                                            if (xmlReader.Name == "GearReferenceCode")
                                            {
                                                gearCode = xmlReader.GetAttribute("code");
                                                gearVariation = xmlReader.GetAttribute("gearVariationName");
                                                gearVariationGuid = xmlReader.GetAttribute("gearVariationGuid");
                                                isSubVariation = bool.Parse(xmlReader.GetAttribute("isSubVariation"));
                                                elementCounter++;
                                            }

                                            break;
                                    }

                                    if (gearVariation?.Length > 0 && gearVariationGuid?.Length > 0 && gearCode.Length > 0)
                                    {
                                        var success = Gears.SaveNewGearReferenceCode(gearCode, gearVariationGuid, isSubVariation);
                                        if (success)
                                        {
                                            saveCounter++;
                                        }
                                        gearCode = "";
                                        gearVariationGuid = "";
                                        gearVariation = "";
                                        isSubVariation = false;
                                    }
                                }
                                if (saveCounter == 0)
                                {
                                    msg = "No gear reference codes was imported into the database";
                                }
                                else
                                {
                                    msg = $"{saveCounter.ToString()} gear reference codes saved to the database";
                                    FillRefCodeList();
                                    SizeColumns(listViewCodes, false);
                                }
                                MessageBox.Show(msg, "Import gear variation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;

                            case ExportImportDataType.GearsLocalName:
                                break;
                        }
                        break;
                }
            }
        }

        private void OnToolbarItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "tbClose":
                    Close();
                    break;

                case "tbAdd":
                    break;

                case "tbRemove":
                    break;

                case "tbEdit":
                    break;

                case "tbExport":
                    using (ExportImportDialogForm eidf = new ExportImportDialogForm(ExportImportDataType.GearNameDataSelect, ExportImportDeleteAction.ActionExport))
                    {
                        eidf.ShowDialog(this);
                        if (eidf.DialogResult == DialogResult.OK)
                        {
                            if ((eidf.Selection & ExportImportDataType.GearsVariation) == ExportImportDataType.GearsVariation)
                            {
                                Export(ExportImportDataType.GearsVariation);
                            }
                            else if ((eidf.Selection & ExportImportDataType.GearsLocalName) == ExportImportDataType.GearsLocalName)
                            {
                            }
                            else if ((eidf.Selection & ExportImportDataType.GearsClass) == ExportImportDataType.GearsClass)
                            {
                                Export(ExportImportDataType.GearsClass);
                            }
                            else if ((eidf.Selection & ExportImportDataType.GearsRefCode) == ExportImportDataType.GearsRefCode)
                            {
                                Export(ExportImportDataType.GearsRefCode);
                            }
                        }
                    }
                    break;

                case "tbImport":
                    using (ExportImportDialogForm eidf = new ExportImportDialogForm(ExportImportDataType.GearNameDataSelect, ExportImportDeleteAction.ActionImport))
                    {
                        eidf.ShowDialog(this);
                        if (eidf.DialogResult == DialogResult.OK)
                        {
                            if ((eidf.Selection & ExportImportDataType.GearsVariation) == ExportImportDataType.GearsVariation)
                            {
                                Import(ExportImportDataType.GearsVariation);
                            }
                            else if ((eidf.Selection & ExportImportDataType.GearsLocalName) == ExportImportDataType.GearsLocalName)
                            {
                            }
                            else if ((eidf.Selection & ExportImportDataType.GearsClass) == ExportImportDataType.GearsClass)
                            {
                                Import(ExportImportDataType.GearsClass);
                            }
                            else if ((eidf.Selection & ExportImportDataType.GearsRefCode) == ExportImportDataType.GearsRefCode)
                            {
                                Import(ExportImportDataType.GearsRefCode);
                            }
                        }
                    }
                    break;
            }
        }

        private void ExportGearVariations()
        {
            FileDialogHelper.Title = "Export gear variations";
            FileDialogHelper.DialogType = FileDialogType.FileSave;
            FileDialogHelper.DataFileType = DataFileType.Text | DataFileType.XML | DataFileType.CSV;
            FileDialogHelper.ShowDialog();
            var fileName = FileDialogHelper.FileName;
            if (fileName.Length > 0)
            {
                switch (Path.GetExtension(fileName))
                {
                    case ".txt":

                        break;

                    case ".XML":
                    case ".xml":
                        break;
                }
            }
        }

        private void OnComboSelectedIndexChanged(object sender, EventArgs e)
        {
            var cbo = (ComboBox)sender;

            switch (cbo.Name)
            {
                case "comboClass":

                    listViewCodes.Items.Clear();
                    listViewWhereUsed.Items.Clear();
                    listViewLocalNames.Items.Clear();
                    listViewVariations.Items.Clear();

                    FillVariationsList();
                    var ch = listViewVariations.Columns[0];
                    var cw = ch.Width;
                    ch.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    if (ch.Width < cw) ch.Width = cw;
                    FillGearVarSpecsColumn(listViewVariations);

                    if (listViewVariations.Items.Count > 0)
                    {
                        if (listViewVariations.Items.ContainsKey(_gearVarGuid))
                        {
                            listViewVariations.Items[_gearVarGuid].Selected = true;
                        }
                        else
                        {
                            listViewVariations.Items[0].Selected = true;
                            _gearVarGuid = listViewVariations.Items[0].Name;
                        }

                        EventArgs ea = new EventArgs();
                        OnlistView_Click(listViewVariations, ea);
                    }
                    break;
            }
        }
    }
}