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
        string _AOIName = "";

        frmSamplingDetail _Parent;

        public string AOIName
        {
            get { return _AOIName; }
            set { _AOIName = value; }
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
            lv.Columns.Add("Variation");
            FillVariationsList();
            lv.Items[_GearVarGuid].Selected = true;

            lv = listViewCodes;
            ch = lv.Columns.Add("Code");
            ch = lv.Columns.Add("Sub-variation");
            FillRefCodeList();
            lv.Items[_GearRefCode].Selected = true;

            lv = listViewWhereUsed;
            ch = lv.Columns.Add("Target area of use");
            FillRefCodeUsage();
            lv.Items[_AOIName].Selected = true;

            lv = listViewLocalNames;
            ch = lv.Columns.Add("Local name");
           

        }

        private void FillRefCodeUsage()
        {
            listViewWhereUsed.Items.Clear();
            var list = global.TargetAreaUsed_RefCode(_GearRefCode);
            foreach (var i in list)
            {
                var lvi = new ListViewItem
                {
                    Name = i,
                    Text = i
                };
                listViewWhereUsed.Items.Add(lvi);
            }
            listViewWhereUsed.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
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
            listViewCodes.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewCodes.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
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
            listViewVariations.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
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
                    FillVariationsList();
                     break;
            }
        }
    }
}
