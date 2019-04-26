using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Database.Classes;

namespace FAD3.Database.Forms
{
    public partial class EditGearVariationNameForm : Form
    {
        private string _variationName;
        private Dictionary<string, string> _gearVarDict = new Dictionary<string, string>();
        public string TargetGearVariationName { get; internal set; }
        public string TargetGearVariationGuid { get; internal set; }
        public string LocalName { get; internal set; }

        public EditGearVariationNameForm(string variationName)
        {
            InitializeComponent();
            _variationName = variationName;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (cboGearVariation.Text.Length > 0)
                    {
                        TargetGearVariationName = cboGearVariation.Text;
                        TargetGearVariationGuid = ((KeyValuePair<string, string>)cboGearVariation.SelectedItem).Key;
                        LocalName = txtLocalNameToAdd.Text;

                        DialogResult dr = DialogResult.Yes;
                        if (txtLocalNameToAdd.Text.Length == 0)
                        {
                            dr = MessageBox.Show("Leave local name blank?", "Verify if local name is blank", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        }
                        if (dr == DialogResult.Yes)
                        {
                            DialogResult = DialogResult.OK;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please provide gear variation", "Gear variation is missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            lblTitle.Text = $"Editing the gear:\r\n{_variationName}";
            cboGearClasses.DisplayMember = "Value";
            cboGearClasses.ValueMember = "Key";
            foreach (var item in Gear.GetGearClassDictionary())
            {
                KeyValuePair<string, string> gearClass = new KeyValuePair<string, string>(item.Key, item.Value.gearClassName);
                cboGearClasses.Items.Add(gearClass);
            }
        }

        private void OnGearClassChanged(object sender, EventArgs e)
        {
            cboGearVariation.Items.Clear();
            foreach (var item in Gear.GearVariationsUsage(((KeyValuePair<string, string>)cboGearClasses.SelectedItem).Key))
            {
                cboGearVariation.Items.Add(item);
            }
            cboGearVariation.DisplayMember = "value";
            cboGearVariation.ValueMember = "key";
            //cboGearVariation.AutoCompleteMode = AutoCompleteMode.Suggest;
            //cboGearVariation.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
    }
}