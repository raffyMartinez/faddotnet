using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetaphoneCOM;

namespace FAD3
{
    public partial class GearCodesUsageForm : Form
    {
        private string _GearClassName = "";
        private string _GearClassGuid = "";
        private string _GearVarName = "";
        private string _GearVarGuid = "";
        private string _GearRefCode = "";
        private string _TargetAreaName = "";
        private string _TargetAreaGuid = "";
        private string _TargetAreaUsageRow = "";
        private string _LocalNameGuid = "";
        private string _LocalName;
        private global.fad3GearEditAction _action;

        private SamplingForm _Parent;
        private static GearCodesUsageForm _instance;

        public static GearCodesUsageForm GetInstance(string GearVarGuid = "", string TargetAreaGuid = "", string GearClassName = "")
        {
            if (_instance == null) _instance = new GearCodesUsageForm(GearVarGuid, TargetAreaGuid, GearClassName);
            return _instance;
        }

        public void TargetArea(string TargetAreaName, string TargetAreaGuid)
        {
            _TargetAreaName = TargetAreaName;
            _TargetAreaGuid = TargetAreaGuid;
        }

        public string GearRefCode
        {
            get { return _GearRefCode; }
            set { _GearRefCode = value; }
        }

        public void GearVariation(string VariationName, string VariationGUID)
        {
            _GearVarName = VariationName;
            _GearVarGuid = VariationGUID;
        }

        public string GearClassName
        {
            get { return _GearClassName; }
            set { _GearClassName = value; }
        }

        public SamplingForm Parent_Form
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        public GearCodesUsageForm(string GearVarGuid = "", string TargetAreaGuid = "", string GearClassName = "")
        {
            InitializeComponent();
            _GearVarGuid = GearVarGuid;
            _TargetAreaGuid = TargetAreaGuid;
            _GearClassName = GearClassName;
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
            var WidthPercent = .8D;

            //fill the combobox with gear class names
            ReadGearClass();

            if (_GearClassName.Length > 0)
            {
                comboClass.Text = _GearClassName;
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
            ch = lv.Columns.Add("Variation");
            var cw = ch.Width;
            lv.Columns.Add("Specs");
            FillVariationsList();
            if (_GearVarGuid.Length > 0)
                lv.Items[_GearVarGuid].Selected = true;
            else
            {
                lv.Items[0].Selected = true;
                _GearVarGuid = lv.Items[0].Name;
            }
            ch.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            if (ch.Width < cw) ch.Width = cw;
            FillGearVarSpecsColumn(lv);

            //fill this list view with gear codes belonging to a variation
            lv = listViewCodes;
            var ch1 = lv.Columns.Add("Code");
            var ch2 = lv.Columns.Add("Sub-variation");
            FillRefCodeList();
            ch1.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            ch2.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            if (_GearRefCode.Length > 0 && lv.Items.ContainsKey(_GearRefCode))
                lv.Items[_GearRefCode].Selected = true;
            else
            {
                lv.Items[0].Selected = true;
                _GearRefCode = lv.Items[0].Name;
            }

            //fill this list view with target areas where the variation is used
            if (_GearRefCode.Length > 0)
            {
                lv = listViewWhereUsed;
                ch = lv.Columns.Add("Target area of use");
                FillRefCodeUsage();
                if (_TargetAreaGuid.Length > 0)
                {
                    if (lv.Items.Count > 0)
                    {
                        lv.Items[_TargetAreaGuid].Selected = true;
                    }
                }
                else
                {
                    if (lv.Items.Count > 0)
                    {
                        lv.Items[0].Selected = true;
                        _TargetAreaGuid = lv.Items[0].Name;
                    }
                }
                ch.Width = (int)(lv.Width * WidthPercent);
            }

            //fill this list view with the local names used in a target area

            lv = listViewLocalNames;
            ch = lv.Columns.Add("Local name");
            if (_TargetAreaGuid.Length > 0)
            {
                FillLocalNames();
                ch.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            else
            {
                ch.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            PopulateLists();
        }

        private void FillLocalNames()
        {
            listViewLocalNames.Items.Clear();
            var list = gear.GearLocalName_TargetArea(_GearRefCode, _TargetAreaGuid);
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
            var list = gear.TargetAreaUsed_RefCode(_GearRefCode);
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
            var list = gear.GearSubVariations(_GearVarGuid);
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
                        if (listViewVariations.Items.ContainsKey(_GearVarGuid))
                        {
                            listViewVariations.Items[_GearVarGuid].Selected = true;
                        }
                        else
                        {
                            listViewVariations.Items[0].Selected = true;
                            _GearVarGuid = listViewVariations.Items[0].Name;
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
                    _GearRefCode = lvi.Name;

                    listViewWhereUsed.Items.Clear();
                    listViewLocalNames.Items.Clear();

                    FillRefCodeUsage();
                    if (listViewWhereUsed.Items.Count > 0)
                    {
                        if (!listViewWhereUsed.Items.ContainsKey(_TargetAreaGuid))
                        {
                            listViewWhereUsed.Items[0].Selected = true;
                            _TargetAreaGuid = listViewWhereUsed.Items[0].Name;
                        }
                        else
                        {
                            listViewWhereUsed.Items[_TargetAreaGuid].Selected = true;
                        }
                        FillLocalNames();
                    }
                    break;

                case "listViewLocalNames":
                    break;

                case "listViewVariations":
                    _GearVarGuid = lvi.Name;

                    listViewCodes.Items.Clear();
                    listViewWhereUsed.Items.Clear();
                    listViewLocalNames.Items.Clear();

                    FillRefCodeList();
                    if (listViewCodes.Items.Count > 0)
                    {
                        _GearRefCode = listViewCodes.Items[0].Name;
                        listViewCodes.Items[_GearRefCode].Selected = true;
                        FillRefCodeUsage();
                        if (listViewWhereUsed.Items.Count > 0)
                        {
                            if (!listViewWhereUsed.Items.ContainsKey(_TargetAreaGuid))
                            {
                                listViewWhereUsed.Items[0].Selected = true;
                                _TargetAreaGuid = listViewWhereUsed.Items[0].Name;
                            }
                            else
                            {
                                listViewWhereUsed.Items[_TargetAreaGuid].Selected = true;
                            }
                            FillLocalNames();
                        }
                    }
                    break;

                case "listViewWhereUsed":
                    _TargetAreaGuid = lvi.Name;
                    _TargetAreaUsageRow = lvi.Tag.ToString();
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
                            _TargetAreaGuid = "";
                            _TargetAreaUsageRow = "";
                            break;

                        case "listViewWhereUsed":
                            break;

                        case "listViewVariations":
                            _GearRefCode = "";
                            _TargetAreaGuid = "";
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
                        tsi.Enabled = _GearRefCode.Length > 0;

                        tsi = dropDownMenu.Items.Add("Delete target area");
                        tsi.Name = "itemDeleteTargetArea";
                        tsi.Enabled = info.Item != null;
                        break;

                    case "listViewVariations":
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
                        tsi.Enabled = _TargetAreaGuid.Length > 0;

                        tsi = dropDownMenu.Items.Add("Delete gear local name");
                        tsi.Name = "itemDeleteLocalName";
                        tsi.Enabled = info.Item != null;
                        break;

                    case "listViewCodes":
                        tsi = dropDownMenu.Items.Add("Add a gear code");
                        tsi.Name = "itemAddGearCode";
                        tsi.Enabled = _GearVarGuid.Length > 0;

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
                case "itemAddTargetArea":
                    _action = global.fad3GearEditAction.addAOI;
                    myList = listViewWhereUsed.Items.Cast<ListViewItem>()
                                                     .Select(item => item.Text)
                                                     .ToList();
                    break;

                case "itemAddGearVariation":
                    _action = global.fad3GearEditAction.addGearVariation;
                    break;

                case "itemAddLocalName":
                    _action = global.fad3GearEditAction.addLocalName;
                    myList = listViewLocalNames.Items.Cast<ListViewItem>()
                                                     .Select(item => item.Text)
                                                     .ToList();
                    break;

                case "itemAddGearCode":
                    _action = global.fad3GearEditAction.addGearCode;
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

                    if (_GearVarGuid.Length > 0)
                    {
                        if (MessageBox.Show("Please confirm that you want to delete this variation",
                                            "Confirmation needed", MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Exclamation) == DialogResult.OK)
                        {
                            var result = gear.DeleteGearVariation(_GearVarGuid);
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
                    _GearClassGuid = ((KeyValuePair<string, string>)comboClass.SelectedItem).Key;
                    GearEditoForm f = new GearEditoForm(this)
                    {
                        GearClassGuid = _GearClassGuid,
                        GearVariationGuid = _GearVarGuid,
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
            _TargetAreaGuid = targetAreaGuid;
            _TargetAreaName = targetAreaName;

            if (_GearRefCode.Length > 0 && _TargetAreaGuid.Length > 0)
            {
                var result = gear.AddGearCodeUsageTargetArea(_GearRefCode, _TargetAreaGuid);
                if (result.Success)
                {
                    var newLVI = listViewWhereUsed.Items.Add(_TargetAreaGuid, _TargetAreaName, null);
                    newLVI.Tag = result.NewRow;
                    _TargetAreaUsageRow = result.NewRow;
                }
            }
        }

        public void UsageLocalName(string localNameGuid, string localName)
        {
            _LocalNameGuid = localNameGuid;
            _LocalName = localName;

            if (_GearRefCode.Length > 0 && _TargetAreaGuid.Length > 0 && _LocalNameGuid.Length > 0 && _TargetAreaUsageRow.Length > 0)
            {
                var result = gear.AddUsageLocalName(_TargetAreaUsageRow, _LocalNameGuid);
                if (result.Success)
                {
                    var newLVI = listViewLocalNames.Items.Add(_LocalNameGuid, _LocalName, null);
                    newLVI.Tag = result.NewRow;
                }
            }
        }

        public void UsageGearCode(string gearCode, bool isVariation)
        {
            _GearRefCode = gearCode;
            if (_GearVarGuid.Length > 0)
            {
                if (gear.AddGearVariationReferenceCode(_GearRefCode, _GearVarGuid, isVariation))
                {
                    var lvi = listViewCodes.Items.Add(_GearRefCode, _GearRefCode, null);
                    lvi.SubItems.Add(isVariation.ToString());
                }
            }
        }

        public void UsageGearVariation(string gearVariationName)
        {
            _GearVarName = gearVariationName;
            if (_GearClassGuid.Length > 0)
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
                    var result = gear.AddGearVariation(_GearClassGuid, gearVariationName);
                    if (result.success)
                    {
                        _GearVarGuid = result.newGuid;
                        listViewVariations.Items.Add(_GearVarGuid, gearVariationName, null);
                    }
                }
            }
        }
    }
}