using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace FAD3.Database.Forms
{
    public partial class ImportFishCatchMonitoringDestinationForm : Form
    {
        public string TargetAreaGUID { get; set; }
        public string TargetAreaName { get; set; }
        private MainForm _parentForm;
        public bool TargetAreaExists { get; set; }
        public string ImportXMLFileName { get; set; }
        private string _targetAreaNameFromXML;
        private string _targetAreaGuidFromXML;
        private string _targetAreaCodeFromXML { get; set; }
        public bool NewTargetArea { get; private set; }

        public ImportFishCatchMonitoringDestinationForm(string targetAreaName, string targetAreaGuid, MainForm parentForm)
        {
            InitializeComponent();
            TargetAreaName = targetAreaName;
            TargetAreaGUID = targetAreaGuid;
            _parentForm = parentForm;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (rbtnImportExisting.Checked)
                    {
                        NewTargetArea = false;
                        KeyValuePair<string, string> kv = (KeyValuePair<string, string>)cboTargetAreas.SelectedItem;
                        TargetAreaName = kv.Value;
                        TargetAreaGUID = kv.Key;
                    }
                    else if (rbtnImportToNew.Checked)
                    {
                        NewTargetArea = true;
                        TargetAreaName = txtNewTargetArea.Text;
                        TargetAreaGUID = Guid.NewGuid().ToString();
                    }
                    DialogResult = DialogResult.OK;
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            int elementCounter = 0;
            bool proceed = false;
            XmlTextReader xmlReader = new XmlTextReader(ImportXMLFileName);
            while (((!proceed && elementCounter == 0) || proceed) && xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlReader.Name)
                    {
                        case "FishCatchMonitoring":
                            if (elementCounter == 0)
                            {
                                _targetAreaNameFromXML = xmlReader.GetAttribute("TargetAreaname");
                                _targetAreaGuidFromXML = xmlReader.GetAttribute("TargetAreaGUID");
                                _targetAreaCodeFromXML = xmlReader.GetAttribute("TargetAreaCode");
                                lblTitle.Text = $"Importing {_targetAreaNameFromXML}";
                                proceed = true;
                            }
                            break;
                    }
                    elementCounter++;
                }

                if (elementCounter > 0 && !proceed)
                {
                    proceed = false;
                    lblTitle.Text = "Error";
                    lblError.Visible = true;
                    lblError.Text = "XML file does not contain fish catch monitoring data";
                    panelImport.Hide();
                    btnOk.Enabled = false;
                    break;
                }
                else if (proceed)
                {
                    break;
                }
            }

            if (proceed)
            {
                foreach (var targetArea in _parentForm.TargetArea.TargetAreas)
                {
                    cboTargetAreas.Items.Add(targetArea);
                    if (targetArea.Key == _targetAreaGuidFromXML)
                    {
                        TargetAreaExists = true;
                    }
                }
                cboTargetAreas.DisplayMember = "Value";
                cboTargetAreas.ValueMember = "Key";

                txtNewTargetArea.Text = _targetAreaNameFromXML;
                if (TargetAreaExists)
                {
                    rbtnImportToNew.Enabled = false;
                    rbtnImportExisting.Checked = true;
                    cboTargetAreas.Text = _targetAreaNameFromXML;
                    cboTargetAreas.Enabled = false;
                    txtNewTargetArea.Enabled = false;
                }
                else if (cboTargetAreas.Items.Count > 0)
                {
                    cboTargetAreas.SelectedIndex = 0;
                }
                else
                {
                    rbtnImportToNew.Checked = true;
                    rbtnImportExisting.Enabled = false;
                }
            }
        }
    }
}