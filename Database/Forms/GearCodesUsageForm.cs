using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetaphoneCOM;
using FAD3.GUI.Classes;

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
                if (gear.GearVarHasSpecsTemplate(lvi.Name))
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
            lv.Columns.Add("Variation");
            lv.Columns.Add("Specs");
            SizeColumns(lv);
            FillVariationsList();
            if (_gearVarGuid.Length > 0)
                lv.Items[_gearVarGuid].Selected = true;
            else
            {
                lv.Items[0].Selected = true;
                _gearVarGuid = lv.Items[0].Name;
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
                lv.Items[0].Selected = true;
                _gearRefCode = lv.Items[0].Name;
            }

            //fill this list view with target areas where the variation is used
            if (_gearRefCode.Length > 0)
            {
                lv = listViewWhereUsed;
                lv.Columns.Add("Target area of use");
                SizeColumns(lv);
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
            PopulateLists();
        }

        private void FillLocalNames()
        {
            listViewLocalNames.Items.Clear();
            var list = gear.GearLocalName_TargetArea(_gearRefCode, _targetAreaGuid);
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
            var list = gear.TargetAreaUsed_RefCode(_gearRefCode);
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
            var list = gear.GearSubVariations(_gearVarGuid);
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
            var key = ((KeyValuePair<string, string>)comboClass.SelectedItem).Key;
            var list = gear.GearVariationsWithSpecs(key);
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

        private void ReadGearClass()
        {
            comboClass.With(o =>
            {
                o.DataSource = new BindingSource(gear.GearClass, null);
                o.DisplayMember = "Value";
                o.ValueMember = "Key";
                o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                o.AutoCompleteSource = AutoCompleteSource.ListItems;
            });
        }

        private void comboClass_Validated(object sender, EventArgs e)
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

        private void OnlistView_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void OnListView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var lv = (ListView)sender;
                ListViewHitTestInfo info = lv.HitTest(e.X, e.Y);
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
                var lv = (ListView)sender;
                ListViewHitTestInfo info = lv.HitTest(e.X, e.Y);

                switch (lv.Name)
                {
                    case "listViewWhereUsed":
                        var tsi = dropDownMenu.Items.Add("Add target area where used");
                        tsi.Name = "itemAddTargetArea";
                        tsi.Enabled = _gearRefCode.Length > 0;

                        tsi = dropDownMenu.Items.Add("Delete target area");
                        tsi.Name = "itemDeleteTargetArea";
                        tsi.Enabled = info.Item != null;
                        break;

                    case "listViewVariations":
                        tsi = dropDownMenu.Items.Add("Samplings using this gear");
                        tsi.Name = "itemGearSamplings";
                        tsi.Enabled = info.Item != null;

                        tsi = dropDownMenu.Items.Add("Add a gear variation");
                        tsi.Name = "itemAddGearVariation";

                        tsi = dropDownMenu.Items.Add("Edit gear variation");
                        tsi.Name = "itemEditGearVariation";
                        tsi.Enabled = info.Item != null;

                        tsi = dropDownMenu.Items.Add("Delete gear variation");
                        tsi.Name = "itemDeleteGearVariation";
                        tsi.Enabled = info.Item != null;

                        dropDownMenu.Items.Add("-");
                        tsi = dropDownMenu.Items.Add("Gear specs");
                        tsi.Name = "itemManageGearSpecs";
                        tsi.Enabled = info.Item != null;
                        break;

                    case "listViewLocalNames":
                        tsi = dropDownMenu.Items.Add("Add a gear local name");
                        tsi.Name = "itemAddLocalName";
                        tsi.Enabled = _targetAreaGuid.Length > 0;

                        tsi = dropDownMenu.Items.Add("Delete gear local name");
                        tsi.Name = "itemDeleteLocalName";
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

        private void dropDownMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var myList = new List<string>();
            e.ClickedItem.Owner.Hide();
            var RowNumberForDeletion = "";
            switch (e.ClickedItem.Name)
            {
                case "itemGearSamplings":

                    var ef = GearSpeciesSamplingsForm.GetInstance(_gearVarGuid, _gearVarName, this);
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
                        var result = gear.DeleteTargetAreaUsage(RowNumberForDeletion);
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
                        if (gear.DeleteLocalNameUsage(RowNumberForDeletion))
                        {
                            listViewLocalNames.Items.Remove(listViewLocalNames.SelectedItems[0]);
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
                            var result = gear.DeleteGearVariation(_gearVarGuid);
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
            }

            switch (e.ClickedItem.Name)
            {
                case "itemAddTargetArea":
                case "itemAddGearVariation":
                case "itemAddLocalName":
                case "itemAddGearCode":
                    _gearClassGuid = ((KeyValuePair<string, string>)comboClass.SelectedItem).Key;
                    GearEditorForm f = new GearEditorForm(this)
                    {
                        GearClassGuid = _gearClassGuid,
                        GearVariationGuid = _gearVarGuid,
                        Action = _action,
                        InList = myList
                    };
                    f.ShowDialog(this);
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
                var result = gear.AddGearCodeUsageTargetArea(_gearRefCode, _targetAreaGuid);
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
                var result = gear.AddUsageLocalName(_targetAreaUsageRow, _localNameGuid);
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
                if (gear.AddGearVariationReferenceCode(_gearRefCode, _gearVarGuid, isVariation))
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
                var similarNames = gear.GearSoundsLike(key1, key2);
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
                    var result = gear.AddGearVariation(_gearClassGuid, gearVariationName);
                    if (result.success)
                    {
                        _gearVarGuid = result.newGuid;
                        listViewVariations.Items.Add(_gearVarGuid, gearVariationName, null);
                    }
                }
            }
        }
    }
}