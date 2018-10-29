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
using FAD3.GUI.Classes;

namespace FAD3.Database.Forms
{
    public partial class GearInventoryEditForm : Form
    {
        private string _treeLevel;
        private aoi _aoi;
        private string _inventoryGuid;
        private string _barangayInventoryGuid;
        private FishingGearInventory _inventory;
        private fad3DataStatus _dataStatus;
        private string _newGUID;
        private long _municipalityNumber;

        public string InventoryGUID
        {
            get { return _inventoryGuid; }
            set
            {
                if (value != _inventoryGuid)
                {
                    _inventoryGuid = value;
                    var inventoryItem = _inventory.GetInventory(_inventoryGuid);
                    txtInventoryName.Text = inventoryItem.inventoryName;
                    txtDateImplemented.Text = inventoryItem.dateImplemented.ToString();
                    _dataStatus = fad3DataStatus.statusFromDB;
                }
            }
        }

        public GearInventoryEditForm(string treeLevel, aoi aoi, FishingGearInventory inventory)
        {
            InitializeComponent();
            _treeLevel = treeLevel;
            _aoi = aoi;
            _inventory = inventory;
        }

        private void SetupProvinceComboBox()
        {
            comboProvince.DataSource = new BindingSource(global.ProvincesDictionary, null);
            comboProvince.DisplayMember = "Value";
            comboProvince.ValueMember = "Key";
            comboProvince.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboProvince.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void SetMunicipalitiesCombo(long ProvNo)
        {
            global.MunicipalitiesFromProvinceNo(ProvNo);
            //comboBox2.Items.Clear();
            comboMunicipality.DataSource = new BindingSource(global.MunicipalitiesDictionary, null);
            comboMunicipality.DisplayMember = "Value";
            comboMunicipality.ValueMember = "Key";
            comboMunicipality.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboMunicipality.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void SetBarangaysCombo(long MunicipalityNumber)
        {
            comboBarangays.Items.Clear();
            global.BarangaysFromMunicipalityNo(MunicipalityNumber);
            foreach (var item in global.BarangaysList)
            {
                comboBarangays.Items.Add(item);
            }
        }

        public void AddNewBarangyInventory(string inventoryGuid, string province, string municipality, string barangay)
        {
            AddNewBarangyInventory(inventoryGuid);
            comboProvince.Text = province;
            comboMunicipality.Text = municipality;
            comboBarangays.Text = barangay;
        }

        public void AddNewBarangyInventory(string inventoryGuid, string province, string municipality)
        {
            AddNewBarangyInventory(inventoryGuid);
            comboProvince.Text = province;
            comboMunicipality.Text = municipality;
        }

        public void AddNewBarangyInventory(string inventoryGuid, string province)
        {
            AddNewBarangyInventory(inventoryGuid);
            comboProvince.Text = province;
        }

        public void AddNewBarangyInventory(string inventoryGuid)
        {
            _dataStatus = fad3DataStatus.statusNew;
            _inventoryGuid = inventoryGuid;
            pnlBarangay.Location = pnlInventory.Location;
            pnlBarangay.Visible = true;
            SetupProvinceComboBox();
        }

        public void AddNewInventory()
        {
            pnlInventory.Visible = true;
            _dataStatus = fad3DataStatus.statusNew;
        }

        private bool ValidateForm()
        {
            switch (_treeLevel)
            {
                case "root":
                    return txtDateImplemented.Text.Length > 0 && txtInventoryName.Text.Length > 0;

                case "targetAreaInventory":
                case "province":
                case "municipality":
                case "barangay":
                    return txtCountCommercial.Text.Length > 0
                        && txtCountFishers.Text.Length > 0
                        && txtCountMotorized.Text.Length > 0
                        && txtCountNonMotorized.Text.Length > 0
                        && comboBarangays.Text.Length > 0
                        && comboMunicipality.Text.Length > 0
                        && comboProvince.Text.Length > 0;
            }
            return false;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (ValidateForm())
                    {
                        var success = false;
                        var guid = "";
                        if (_dataStatus == fad3DataStatus.statusNew)
                        {
                            guid = Guid.NewGuid().ToString();
                        }
                        switch (_treeLevel)
                        {
                            case "root":
                                if (_dataStatus == fad3DataStatus.statusFromDB)
                                {
                                    guid = _inventoryGuid;
                                }
                                success = _inventory.SaveFishingGearInventory(_dataStatus, txtInventoryName.Text, DateTime.Parse(txtDateImplemented.Text), guid);
                                break;

                            case "targetAreaInventory":
                            case "province":
                            case "municipality":
                            case "barangay":
                                if (_dataStatus == fad3DataStatus.statusFromDB)
                                {
                                    guid = _barangayInventoryGuid;
                                }
                                success = _inventory.SaveBarangayInventory(_dataStatus, _inventoryGuid, _municipalityNumber, comboBarangays.Text,
                                                                           int.Parse(txtCountFishers.Text), int.Parse(txtCountCommercial.Text),
                                                                           int.Parse(txtCountMotorized.Text), int.Parse(txtCountNonMotorized.Text),
                                                                           guid, txtSitio.Text);
                                break;
                        }

                        if (success) Close();
                    }
                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }

        private void OnTextBoxValidating(object sender, CancelEventArgs e)
        {
            var s = ((TextBox)sender).Text;
            var showError = true;
            var msg = "";
            if (s.Length > 0)
            {
                switch (((TextBox)sender).Name)
                {
                    case "txtInventoryName":

                        e.Cancel = s.Length < 10;
                        msg = "Name must be at least 10 characters";
                        break;

                    case "txtDateImplemented":
                        DateTime date;
                        if (!DateTime.TryParse(s, out date))
                        {
                            e.Cancel = true;
                            msg = "Not a valid date";
                        }
                        else
                        {
                            var yearDiff = DateTime.Now.Year - date.Year;
                            if (date > DateTime.Now)
                            {
                                e.Cancel = true;
                                msg = "Cannot use a future date";
                            }
                            else if (yearDiff > 1)
                            {
                                showError = false;
                                var dialogResult = MessageBox.Show($"Accept this date of more than {yearDiff} year?", "Accept past date", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                e.Cancel = dialogResult != DialogResult.Yes;
                            }
                        }
                        break;
                }
            }

            if (e.Cancel && showError)
            {
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (_dataStatus != fad3DataStatus.statusNew)
                {
                    _dataStatus = fad3DataStatus.statusEdited;
                }
            }
        }

        private void OnComboSelectedIndexChanged(object sender, EventArgs e)
        {
            long key;
            switch (((ComboBox)sender).Name)
            {
                case "comboProvince":
                    key = ((KeyValuePair<long, string>)comboProvince.SelectedItem).Key;
                    SetMunicipalitiesCombo(key);
                    break;

                case "comboMunicipality":
                    _municipalityNumber = ((KeyValuePair<long, string>)comboMunicipality.SelectedItem).Key;
                    SetBarangaysCombo(_municipalityNumber);
                    break;
            }
        }

        private void OnComboValidating(object sender, CancelEventArgs e)
        {
            var s = ((ComboBox)sender).Text;
            var msg = "";
            if (s.Length > 0)
            {
                switch (((ComboBox)sender).Name)
                {
                    case "comboBarangays":
                        if (!comboBarangays.Items.Contains(s))
                        {
                            msg = $"'{s}' is not in list of barangays\r\nDo you want to add a new barangay?";
                            var result = MessageBox.Show(msg, "Create new barangay", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (result == DialogResult.Yes)
                            {
                                comboBarangays.Items.Add(s);
                            }
                            else
                            {
                                e.Cancel = true;
                            }
                        }
                        break;
                }
            }
        }
    }
}