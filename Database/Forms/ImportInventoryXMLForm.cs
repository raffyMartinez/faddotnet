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
using System.IO;
using System.Xml;

namespace FAD3.Database.Forms
{
    public partial class ImportInventoryXMLForm : Form
    {
        private FishingGearInventory _inventory;
        private string _importedProjectName;
        private DateTime _importedProjectDateImplemented;
        private string _importedProjectGUID;
        private bool _importedProjectFound;
        public ImportInventoryAction ImportInventoryAction { get; internal set; }
        public string ImportedInventoryProjectName { get; internal set; }
        public string ImportedInventoryProjectGuid { get; internal set; }
        public DateTime ImportedInventoryProjectDate { get; internal set; }
        public string ImportedInventoryFileName { get; internal set; }
        public string ImportIntoExistingProjectGuid { get; internal set; }
        private string _fileName;
        private bool _isInventoryXML;

        public ImportInventoryXMLForm(FishingGearInventory inventory)
        {
            InitializeComponent();
            _inventory = inventory;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            FileDialogHelper.Title = "Get filename for importing fisher, vessel and fishing gear inventory";
            FileDialogHelper.DialogType = FileDialogType.FileOpen;
            FileDialogHelper.DataFileType = DataFileType.XML;
            FileDialogHelper.ShowDialog();
            _fileName = FileDialogHelper.FileName;
            if (_fileName.Length > 0 && string.Equals(Path.GetExtension(_fileName), ".xml", StringComparison.OrdinalIgnoreCase))
            {
                var elementCounter = 0;
                XmlTextReader xmlReader = new XmlTextReader(_fileName);
                while ((elementCounter == 0 || (elementCounter > 0)) && xmlReader.Read())
                {
                    if (xmlReader.Name == "FisherVesselGearInventoryProject")
                    {
                        _importedProjectGUID = xmlReader.GetAttribute("ProjectGuid");
                        _importedProjectName = xmlReader.GetAttribute("ProjectName");
                        _importedProjectDateImplemented = DateTime.Parse(xmlReader.GetAttribute("DateStart"));
                        _isInventoryXML = true;
                        break;
                    }
                    else if (elementCounter > 1 && !_isInventoryXML)
                    {
                        break;
                    }
                    elementCounter++;
                }

                if (_isInventoryXML)
                {
                    foreach (var inventory in _inventory.Inventories)
                    {
                        KeyValuePair<string, string> kv = new KeyValuePair<string, string>(inventory.Key, inventory.Value.InventoryName);
                        cboInventories.Items.Add(kv);

                        if (!_importedProjectFound
                            && inventory.Value.InventoryName == _importedProjectName
                            && inventory.Value.DateConducted == _importedProjectDateImplemented)
                        {
                            _importedProjectFound = true;
                        }
                    }

                    cboInventories.DisplayMember = "Value";
                    cboInventories.ValueMember = "Key";

                    if (!_importedProjectFound)
                    {
                        txtImportedProject.Text = _importedProjectName;
                        rdbImportExisting.Enabled = cboInventories.Items.Count > 0;
                        cboInventories.Enabled = cboInventories.Items.Count > 0;
                        if (cboInventories.Items.Count > 0)
                        {
                            cboInventories.SelectedIndex = 0;
                            //ImportIntoExistingProjectGuid = ((KeyValuePair<string, string>)cboInventories.SelectedItem).Key;
                        }
                    }
                    else
                    {
                        cboInventories.SelectedIndex = 0;
                        rdbImportNew.Enabled = false;
                        txtImportedProject.Enabled = false;
                    }
                }
                else
                {
                    MessageBox.Show("File does not contain inventory data", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnOk.Enabled = false;
                }
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (_isInventoryXML)
                    {
                        if (rdbImportExisting.Checked)
                        {
                            ImportInventoryAction = ImportInventoryAction.ImportIntoExisting;
                            ImportedInventoryProjectName = cboInventories.Text;
                            ImportIntoExistingProjectGuid = ((KeyValuePair<string, string>)cboInventories.SelectedItem).Key;
                        }
                        else if (rdbImportNew.Checked)
                        {
                            ImportInventoryAction = ImportInventoryAction.ImportIntoNew;
                            ImportedInventoryProjectName = txtImportedProject.Text;
                            ImportIntoExistingProjectGuid = "";
                        }
                        ImportedInventoryProjectGuid = _importedProjectGUID;
                        ImportedInventoryProjectDate = _importedProjectDateImplemented;
                        ImportedInventoryFileName = _fileName;
                    }
                    DialogResult = DialogResult.OK;
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }
    }
}