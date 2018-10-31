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
        private string _gearVariationKey;
        private int _countCommercial;
        private int _countMunicipalMotorized;
        private int _countMunicipalNonMotorized;
        private int _countFishers;
        private ListBox _activeCatchCompList;
        private GearInventoryForm _parentForm;

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

        public GearInventoryEditForm(string treeLevel, aoi aoi, FishingGearInventory inventory, GearInventoryForm parent)
        {
            InitializeComponent();
            _treeLevel = treeLevel;
            _aoi = aoi;
            _inventory = inventory;
            _parentForm = parent;
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
            pnlBarangay.Visible = true;
            SetupProvinceComboBox();
        }

        private void SetupGearInventoryUI()
        {
            //setup the gearclass combo box
            cboGearClass.DataSource = new BindingSource(gear.GearClass, null);
            cboGearClass.DisplayMember = "Value";
            cboGearClass.ValueMember = "Key";
            cboGearClass.AutoCompleteMode = AutoCompleteMode.Suggest;
            cboGearClass.AutoCompleteSource = AutoCompleteSource.ListItems;

            //setup the combobox of gear local names
            cboSelectGearLocalName.Items.Clear();
            foreach (var item in gear.GearLocalNames)
            {
                cboSelectGearLocalName.Items.Add(item);
            }
            cboSelectGearLocalName.DisplayMember = "Value";
            cboSelectGearLocalName.ValueMember = "Key";
            cboSelectGearLocalName.AutoCompleteMode = AutoCompleteMode.Suggest;
            cboSelectGearLocalName.AutoCompleteSource = AutoCompleteSource.ListItems;

            //setup the listbox of accepted gear local names
            listBoxGearLocalNames.Items.Clear();
            listBoxGearLocalNames.DisplayMember = "value";
            listBoxGearLocalNames.ValueMember = "key";

            //setup the checklistbox of months of gear use and peak season
            var monthArr = new string[] { "January", "February", "March", "April", "May", "June",
                                           "July", "August", "September", "October", "November", "December" };

            for (int n = 0; n < monthArr.Length; n++)
            {
                chkListBoxMonthsSeason.Items.Add(monthArr[n]);
                chkListBoxMonthsUsed.Items.Add(monthArr[n]);
            }

            //setup listview of history of cpue
            SizeColumns(listViewHistoryCpue);
            int decadeNow = ((int)(DateTime.Now.Year / 10)) * 10;
            for (int y = 0; y < 5; y++)
            {
                decadeNow -= 10;
                listViewHistoryCpue.Items.Add(decadeNow.ToString(), decadeNow.ToString() + "s", null);
            }
            SizeColumns(listViewHistoryCpue, false);

            //setup combobox of catch local names
            cboCatchLocalName.Items.Clear();
            foreach (var item in names.LocalNameListDict)
            {
                cboCatchLocalName.Items.Add(item);
            }
            cboCatchLocalName.DisplayMember = "Value";
            cboCatchLocalName.ValueMember = "Key";
            cboCatchLocalName.AutoCompleteMode = AutoCompleteMode.Suggest;
            cboCatchLocalName.AutoCompleteSource = AutoCompleteSource.ListItems;

            //setup dominant catch listbox
            listBoxDominantCatch.Items.Clear();
            listBoxDominantCatch.DisplayMember = "value";
            listBoxDominantCatch.ValueMember = "key";
            listBoxDominantCatch.BackColor = Color.AliceBlue;
            _activeCatchCompList = listBoxDominantCatch;

            //setup non-dominant catch listbox
            listBoxOtherCatch.Items.Clear();
            listBoxOtherCatch.DisplayMember = "value";
            listBoxOtherCatch.ValueMember = "key";

            //setup catch unit combobox
            cboCatchUnit.Items.Add("kilo");
            cboCatchUnit.Items.Add("banyera");
            cboCatchUnit.Items.Add("ice box");
            cboCatchUnit.SelectedIndex = 0;
        }

        public void AddNewGearInventory(string barangayInventoryGuid, int countCommercial, int countMunicipalMotorized, int countMunicipalNonMotorized, int countFishers)
        {
            _dataStatus = fad3DataStatus.statusNew;
            _barangayInventoryGuid = barangayInventoryGuid;
            pnlGear.Visible = true;
            _countCommercial = countCommercial;
            _countFishers = countFishers;
            _countMunicipalMotorized = countMunicipalMotorized;
            _countMunicipalNonMotorized = countMunicipalNonMotorized;
            SetupGearInventoryUI();
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

        public void AddNewInventory()
        {
            pnlInventory.Visible = true;
            _dataStatus = fad3DataStatus.statusNew;
        }

        private bool ValidateGearInventory()
        {
            var proceed = cboGearClass.Text.Length > 0
                && cboGearVariation.Text.Length > 0
                && listBoxGearLocalNames.Items.Count > 0
                && txtCommercialUsage.Text.Length > 0
                && txtMunicipalMotorizedUsage.Text.Length > 0
                && txtMunicipalNonMotorizedUsage.Text.Length > 0
                && txtNoBoatGears.Text.Length > 0
                && txtNumberOfDaysPerMonth.Text.Length > 0
                && chkListBoxMonthsSeason.CheckedItems.Count > 0
                && chkListBoxMonthsUsed.CheckedItems.Count > 0
                && (txtRangeMax.Text.Length > 0 || txtRangeMin.Text.Length > 0)
                && (txtModeLower.Text.Length > 0 || txtModeUpper.Text.Length > 0)
                && listBoxDominantCatch.Items.Count > 0
                && listBoxOtherCatch.Items.Count > 0
                && txtDominantPercentage.Text.Length > 0
                && IsCatchValuesOK();

            if (proceed && listViewHistoryCpue.Items.Count == 0)
            {
                proceed = MessageBox.Show("No entry found for historical CPUE\r\nDo you still want to proceed?",
                    "Confirmation needed", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes;
            }

            return proceed;
        }

        private bool IsCatchValuesOK(bool FormValidation = true)
        {
            bool isOk = false;
            bool cpueDataComplete = false;
            if (txtRangeMax.Text.Length > 0 && txtRangeMin.Text.Length > 0
                && txtModeLower.Text.Length > 0 && txtModeUpper.Text.Length > 0)
            {
                cpueDataComplete = true;
                var maxCPUE = int.Parse(txtRangeMax.Text);
                var minCPUE = int.Parse(txtRangeMin.Text);
                var upperMode = int.Parse(txtModeUpper.Text);
                var lowerMode = int.Parse(txtModeLower.Text);

                isOk = (maxCPUE > minCPUE
                    && upperMode > lowerMode
                    && upperMode <= maxCPUE
                    && lowerMode >= minCPUE);

                if (!isOk)
                {
                    var msg = "Please review catch rate range and catch rate mode.\r\nThey must not conflict with each other.";
                    MessageBox.Show(msg, "Review CPUE range and mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (!FormValidation)
            {
                if (cpueDataComplete)
                {
                    return isOk;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return isOk;
            }
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

                case "sitio":
                    return ValidateGearInventory();
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

                                _parentForm.RefreshSitioLevelInventory(comboProvince.Text, comboMunicipality.Text, comboBarangays.Text);
                                break;

                            case "sitio":

                                if (_dataStatus == fad3DataStatus.statusFromDB)
                                {
                                    guid = _barangayInventoryGuid;
                                }

                                List<string> listGearLocalNameGuids = new List<string>();
                                foreach (var item in listBoxGearLocalNames.Items)
                                {
                                    listGearLocalNameGuids.Add(((KeyValuePair<string, string>)item).Key);
                                }

                                List<int> listMonthGearUse = new List<int>();
                                foreach (int item in chkListBoxMonthsUsed.CheckedIndices)
                                {
                                    listMonthGearUse.Add(item);
                                }

                                List<int> listMonthGearSeason = new List<int>();
                                foreach (int item in chkListBoxMonthsSeason.CheckedIndices)
                                {
                                    listMonthGearSeason.Add(item);
                                }

                                List<(int decade, int catchRate, string unit)> listHistoryCPUE = new List<(int decade, int catchRate, string unit)>();
                                foreach (ListViewItem item in listViewHistoryCpue.Items)
                                {
                                    if (item.SubItems.Count > 1)
                                    {
                                        listHistoryCPUE.Add((int.Parse(item.Text.Trim('s')), int.Parse(item.SubItems[1].Text), item.SubItems[2].Text));
                                    }
                                }

                                List<string> listDominantCatchNameGuid = new List<string>();
                                foreach (var item in listBoxDominantCatch.Items)
                                {
                                    listDominantCatchNameGuid.Add(((KeyValuePair<string, string>)item).Key);
                                }

                                List<string> listNonDominantCatchNameGuid = new List<string>();
                                foreach (var item in listBoxOtherCatch.Items)
                                {
                                    listNonDominantCatchNameGuid.Add(((KeyValuePair<string, string>)item).Key);
                                }

                                int? rangeCPUEMax = null;
                                if (int.TryParse(txtRangeMax.Text, out int v)) rangeCPUEMax = v;

                                int? rangeCPUEMin = null;
                                if (int.TryParse(txtRangeMin.Text, out v)) rangeCPUEMin = v;

                                int? modeCPUEUpper = null;
                                if (int.TryParse(txtModeUpper.Text, out v)) modeCPUEUpper = v;

                                int? modeCPUELower = null;
                                if (int.TryParse(txtModeLower.Text, out v)) modeCPUELower = v;

                                success = _inventory.SaveSitioGearInventoryMain(_barangayInventoryGuid, _gearVariationKey,
                                    int.Parse(txtCommercialUsage.Text), int.Parse(txtMunicipalMotorizedUsage.Text),
                                    int.Parse(txtMunicipalNonMotorizedUsage.Text), int.Parse(txtNoBoatGears.Text),
                                    int.Parse(txtNumberOfDaysPerMonth.Text), cboCatchUnit.Text, int.Parse(txtDominantPercentage.Text),
                                    guid, _dataStatus, rangeCPUEMax, rangeCPUEMin, modeCPUEUpper, modeCPUELower)

                                    && _inventory.SaveSitioGearInventoryGearLocalNames(guid, listGearLocalNameGuids)

                                    && _inventory.SaveSitioGearInventoryFishingMonths(guid, listMonthGearUse, listMonthGearSeason)

                                    && _inventory.SaveSitioGearInventoryCatchComposition(guid, listDominantCatchNameGuid, listNonDominantCatchNameGuid)

                                    && _inventory.SaveSitioGearInventoryHistoricalCPUE(guid, listHistoryCPUE);

                                _parentForm.RefreshSitioGearInventory();
                                break;
                        }

                        if (success)
                        {
                            Close();
                        }
                    }
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnAddLocalName":
                    if (cboSelectGearLocalName.Text.Length > 0)
                    {
                        if (!listBoxGearLocalNames.Items.Contains(cboSelectGearLocalName.SelectedItem))
                        {
                            listBoxGearLocalNames.Items.Add(cboSelectGearLocalName.SelectedItem);
                        }
                        else
                        {
                            MessageBox.Show("Item already in the list");
                        }
                        cboSelectGearLocalName.SelectedIndex = -1;
                    }
                    break;

                case "btnRemoveLocalName":
                    break;

                case "btnAddDominant":
                    if (cboCatchLocalName.Text.Length > 0)
                    {
                        if (!listBoxDominantCatch.Items.Contains(cboCatchLocalName.SelectedItem)
                            && !listBoxOtherCatch.Items.Contains(cboCatchLocalName.SelectedItem))
                        {
                            listBoxDominantCatch.Items.Add(cboCatchLocalName.SelectedItem);
                        }
                        else
                        {
                            MessageBox.Show("Item already in the list");
                        }
                        SetActiveCatchCompositionListBox(listBoxDominantCatch);
                        cboCatchLocalName.SelectedIndex = -1;
                    }
                    break;

                case "btnRemoveDominant":
                    break;

                case "btnAddOther":
                    if (cboCatchLocalName.Text.Length > 0)
                    {
                        if (!listBoxDominantCatch.Items.Contains(cboCatchLocalName.SelectedItem)
                            && !listBoxOtherCatch.Items.Contains(cboCatchLocalName.SelectedItem))
                        {
                            listBoxOtherCatch.Items.Add(cboCatchLocalName.SelectedItem);
                        }
                        else
                        {
                            MessageBox.Show("Item already in the list");
                        }
                        SetActiveCatchCompositionListBox(listBoxOtherCatch);
                        cboCatchLocalName.SelectedIndex = -1;
                    }
                    break;

                case "btnRemoveOther":
                    break;
            }
        }

        private void SetActiveCatchCompositionListBox(ListBox lBox)
        {
            listBoxDominantCatch.BackColor = Color.White;
            listBoxOtherCatch.BackColor = Color.White;
            lBox.BackColor = Color.AliceBlue;
            _activeCatchCompList = lBox;
        }

        private void OnTextBoxValidating(object sender, CancelEventArgs e)
        {
            var ctlName = ((TextBox)sender).Name;
            var s = ((TextBox)sender).Text;
            var showError = true;
            var msg = "";
            if (s.Length > 0)
            {
                switch (ctlName)
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

                    //all textboxes will receive an int
                    case "txtCommercialUsage":
                    case "txtMunicipalMotorizedUsage":
                    case "txtMunicipalNonMotorizedUsage":
                    case "txtNoBoatGears":
                    case "txtNumberOfDaysPerMonth":
                    case "txtRangeMax":
                    case "txtRangeMin":
                    case "txtModeUpper":
                    case "txtModeLower":
                    case "txtDominantPercentage":
                        msg = "Only whole numbers are accepted";
                        if (!int.TryParse(s, out int v))
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                            if (ctlName == "txtCommercialUsage"
                                || ctlName == "txtMunicipalMotorizedUsage"
                                || ctlName == "txtMunicipalNonMotorizedUsage"
                                || ctlName == "txtNoBoatGears")
                            {
                                e.Cancel = v < 0;
                                msg = "Cannot accept values less than zero";

                                if (ctlName == "txtCommercialUsage")
                                {
                                    e.Cancel = v > _countCommercial;
                                    msg = $"Gear distribution in commercial vessels ({v}) must not be more than total number of commercial vessels ({_countCommercial})";
                                }

                                if (ctlName == "txtMunicipalMotorizedUsage")
                                {
                                    e.Cancel = v > _countMunicipalMotorized;
                                    msg = $"Gear distribution in municipal motorized vessels ({v}) must not be more than total number of municipal motorized vessels ({_countMunicipalMotorized})";
                                }

                                if (ctlName == "txtMunicipalNonMotorizedUsage")
                                {
                                    e.Cancel = v > _countMunicipalNonMotorized;
                                    msg = $"Gear distribution in municipal non-motorized vessels ({v}) must not be more than total number of municipal non-motorized vessels ({_countMunicipalNonMotorized})";
                                }
                            }
                            else if (ctlName == "txtNumberOfDaysPerMonth")
                            {
                                e.Cancel = v < 1 || v > 31;
                                msg = "Number of days must be from 1 to 31";
                            }
                            else if (ctlName == "txtDominantPercentage")
                            {
                                e.Cancel = v < 50 || v > 100;
                                msg = "Dominant percent value cannot be less than 50 and more than 100";
                            }
                            else
                            {
                                e.Cancel = v <= 0;
                                msg = "Cannot accept values less than or equal to zero";
                            }

                            if (!e.Cancel)
                            {
                                e.Cancel = !IsCatchValuesOK(FormValidation: false);
                                if (e.Cancel)
                                {
                                    showError = false;
                                }
                            }
                        }
                        break;
                }
            }

            if (e.Cancel && showError)
            {
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (!e.Cancel)
            {
                if (_dataStatus != fad3DataStatus.statusNew)
                {
                    _dataStatus = fad3DataStatus.statusEdited;
                }
            }
        }

        private void SetGearVariationsCombo(string gearKey)
        {
            cboGearVariation.Items.Clear();
            foreach (var item in gear.GearVariationsUsage(gearKey))
            {
                cboGearVariation.Items.Add(item);
            }
            cboGearVariation.DisplayMember = "value";
            cboGearVariation.ValueMember = "key";
            cboGearVariation.AutoCompleteMode = AutoCompleteMode.Suggest;
            cboGearVariation.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void OnComboSelectedIndexChanged(object sender, EventArgs e)
        {
            long key;
            string gearKey;
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

                case "cboGearClass":
                    gearKey = ((KeyValuePair<string, string>)cboGearClass.SelectedItem).Key;
                    SetGearVariationsCombo(gearKey);
                    cboGearVariation.SelectedIndex = 0;
                    //_gearVariationKey = ((KeyValuePair<string, string>)cboGearVariation.SelectedItem).Key;
                    break;

                case "cboGearVariation":
                    _gearVariationKey = ((KeyValuePair<string, string>)cboGearVariation.SelectedItem).Key;
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

        private void OnFormLoad(object sender, EventArgs e)
        {
            pnlGear.Location = pnlInventory.Location;
            pnlBarangay.Location = pnlInventory.Location;

            Width = (pnlInventory.Left * 8) + pnlInventory.Width;
            global.LoadFormSettings(this, true);
        }

        private void OnListViewDblClick(object sender, EventArgs e)
        {
            var historicalCPUEForm = new CPUEHistoricalForm(this, listViewHistoryCpue.SelectedItems[0].Text);
            if (listViewHistoryCpue.SelectedItems[0].SubItems.Count > 1
                && listViewHistoryCpue.SelectedItems[0].SubItems[1].Text.Length > 0
                && listViewHistoryCpue.SelectedItems[0].SubItems[2].Text.Length > 0)
            {
                historicalCPUEForm.CatchValue(int.Parse(listViewHistoryCpue.SelectedItems[0].SubItems[1].Text),
                                              listViewHistoryCpue.SelectedItems[0].SubItems[2].Text);
            }
            historicalCPUEForm.ShowDialog(this);
        }

        public void HistoricalCPUE(int catchWeight, string unit)
        {
            if (listViewHistoryCpue.SelectedItems[0].SubItems.Count == 1)
            {
                listViewHistoryCpue.SelectedItems[0].SubItems.Add(catchWeight.ToString());
                listViewHistoryCpue.SelectedItems[0].SubItems.Add(unit);
            }
            else
            {
                listViewHistoryCpue.SelectedItems[0].SubItems[1].Text = catchWeight.ToString();
                listViewHistoryCpue.SelectedItems[0].SubItems[2].Text = unit;
            }
        }

        private void OnCheckBoxCheckChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            if (chk.Checked)
            {
                for (int n = 0; n < chkListBoxMonthsUsed.Items.Count; n++)
                {
                    chkListBoxMonthsUsed.SetItemChecked(n, true);
                }
            }
        }

        private void OnListBoxEnter(object sender, EventArgs e)
        {
            SetActiveCatchCompositionListBox((ListBox)sender);
        }

        private void OnComboKeyDown(object sender, KeyEventArgs e)
        {
            switch (((ComboBox)sender).Name)
            {
                case "cboSelectGearLocalName":
                    if (e.KeyData == Keys.Enter && cboSelectGearLocalName.Text.Length > 0)
                    {
                        OnButtonClick(btnAddLocalName, null);
                    }
                    break;

                case "cboCatchLocalName":
                    if (e.KeyData == Keys.Enter && cboCatchLocalName.Text.Length > 0)
                    {
                        if (_activeCatchCompList.Name == "listBoxDominantCatch")
                        {
                            OnButtonClick(btnAddDominant, null);
                        }
                        else
                        {
                            OnButtonClick(btnAddOther, null);
                        }
                    }
                    break;
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
        }
    }
}