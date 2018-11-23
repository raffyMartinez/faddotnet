using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.GUI.Classes;
using FAD3.Mapping.Classes;

namespace FAD3.Mapping.Forms
{
    public partial class OccurenceMappingForm : Form
    {
        public bool MapInSelectedTargetArea { get; internal set; }
        public bool Aggregate { get; internal set; }
        public bool ExcludeOne { get; internal set; }
        public string SelectedTargetAreaGuid { get; set; }

        public List<int> SamplingYears { get; internal set; }
        private Form _parentForm;
        private OccurenceDataType _occurenceType;

        //private string _targetAreaGUid;
        private List<int> _sampledYears;
        private string _speciesName;
        private string _speciesGuid;
        private string _gearVariation;
        private string _variationGuid;

        public OccurenceMappingForm(OccurenceDataType occurenceType, Form parent)
        {
            InitializeComponent();
            _occurenceType = occurenceType;
            _parentForm = parent;
        }

        public void SpeciesToMap(string speciesName, string speciesGuid)
        {
            _speciesName = speciesName;
            _speciesGuid = speciesGuid;
            lblName.Text = _speciesName;
        }

        public void GearToMap(string gearVariation, string variationGUID)
        {
            _gearVariation = gearVariation;
            _variationGuid = variationGUID;
            lblName.Text = _gearVariation;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (chkListYears.CheckedItems.Count > 0)
                    {
                        SamplingYears.Clear();
                        foreach (string item in chkListYears.CheckedItems)
                        {
                            SamplingYears.Add(int.Parse(item));
                        }
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Select at least one sampling year", "Selection needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
        }

        private void FillSampledYearList()
        {
            chkListYears.Items.Clear();
            foreach (var item in _sampledYears)
            {
                chkListYears.Items.Add(item.ToString());
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
            SamplingYears = new List<int>();
            if (SelectedTargetAreaGuid == null || SelectedTargetAreaGuid.Length == 0)
            {
                SelectedTargetAreaGuid = global.mainForm.TargetAreaGuid;
            }
            rdbAll.Enabled = SelectedTargetAreaGuid.Length > 0;
            rdbSelected.Enabled = rdbAll.Enabled;

            if (rdbAll.Enabled)
            {
                rdbSelected.Checked = true;
                MapInSelectedTargetArea = true;
            }
            else
            {
                rdbAll.Checked = true;
                MapInSelectedTargetArea = false;
            }
            btnOk.Enabled = _sampledYears.Count > 0;
        }

        private void OnCheckChanged(object sender, EventArgs e)
        {
            if (sender.GetType().Name == "RadioButton")
            {
                switch (((RadioButton)sender).Name)
                {
                    case "rdbAll":
                        MapInSelectedTargetArea = false;
                        _sampledYears = Sampling.GetSamplingYears();
                        SelectedTargetAreaGuid = "";
                        break;

                    case "rdbSelected":
                        MapInSelectedTargetArea = true;
                        _sampledYears = Sampling.GetSamplingYears(SelectedTargetAreaGuid);
                        break;
                }
                FillSampledYearList();
            }
            else
            {
                switch (((CheckBox)sender).Name)
                {
                    case "chkAggregate":
                        Aggregate = chkAggregate.Checked;
                        break;

                    case "chkExcludeOne":
                        ExcludeOne = chkExcludeOne.Checked;
                        break;
                }
            }
        }
    }
}