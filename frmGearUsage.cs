using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FAD3
{
    public partial class frmGearUsage : Form
    {
        string _GearClassName = "";
        string _GearVarName = "";
        string _GearVarGuid = "";
        string _GearRefCode = "";
        string _TargetAreaName = "";
        string _TargetAreaGuid = "";

        frmSamplingDetail _Parent;

        public  void TargetArea(string TargetAreaName, string TargetAreaGuid)
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

        public frmSamplingDetail Parent_Form
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        public frmGearUsage()
        {
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {

            var WidthPercent = .8D;
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

            foreach(Control c in Controls)
            {
                if (c.GetType().ToString()=="System.Windows.Forms.ListView")
                {
                    ((ListView)c).With(o => {
                        o.View = View.Details;
                        o.HeaderStyle = ColumnHeaderStyle.Nonclickable;
                        o.FullRowSelect = true;
                        o.HideSelection = false;
                    });
                }
            }

            var lv = listViewVariations;
            ch = lv.Columns.Add("Variation");
            FillVariationsList();
            lv.Items[_GearVarGuid].Selected = true;
            ch.Width = (int)(lv.Width * WidthPercent);

            lv = listViewCodes;
            var ch1 = lv.Columns.Add("Code");
            var ch2 = lv.Columns.Add("Sub-variation");
            FillRefCodeList();
            ch1.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            ch2.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            lv.Items[_GearRefCode].Selected = true;

            lv = listViewWhereUsed;
            ch = lv.Columns.Add("Target area of use");
            FillRefCodeUsage();
            lv.Items[_TargetAreaGuid].Selected = true;
            ch.Width = (int)(lv.Width * WidthPercent);

            lv = listViewLocalNames;
            ch = lv.Columns.Add("Local name");
            FillLocalNames();
            ch.Width = (int)(lv.Width * WidthPercent);

            foreach (Control c in Controls)
            {
                if (c.GetType().ToString() == "System.Windows.Forms.ListView" && c.Name != "listViewLocalNames")
                {
                    var i = ((ListView)c).SelectedItems[0].Index;
                    ((ListView)c).EnsureVisible(i);
                }
            }

        }

        private void FillLocalNames()
        {
            listViewLocalNames.Items.Clear();
            var list = global.GearLocalName_TargetArea(_GearRefCode, _TargetAreaGuid);
            foreach (var i in list)
            {
                var lvi = new ListViewItem
                {
                    Name = i.Key,
                    Text = i.Value
                };
                listViewLocalNames.Items.Add(lvi);
            }

        }

        private void FillRefCodeUsage()
        {
            listViewWhereUsed.Items.Clear();
            var list = global.TargetAreaUsed_RefCode(_GearRefCode);
            foreach (var i in list)
            {
                var lvi = new ListViewItem
                {
                    Name = i.Key,
                    Text = i.Value
                };
                listViewWhereUsed.Items.Add(lvi);
            }
        }

        private void FillRefCodeList()
        {
            listViewCodes.Items.Clear();
            var list = global.GearSubVariations(_GearVarGuid);
            foreach (KeyValuePair<string, bool> kv in list)
            {
                var lvi = new ListViewItem
                {
                    Name=kv.Key,
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
            var list = global.GearVariationsUsage(key);
            foreach(KeyValuePair<string, string> kv in list)
            {
                var lvi = new ListViewItem
                {
                    Name = kv.Key,
                    Text = kv.Value
                };
                listViewVariations.Items.Add(lvi);
            }
        }

        private void ReadGearClass()
        {
            comboClass.With(o =>
            {
                o.DataSource = new BindingSource(global.GearClass, null);
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
                case "listViewWhereUsed":
                    _TargetAreaGuid = lvi.Name;
                    FillLocalNames();
                    break;
            } 
        }
    }
}
