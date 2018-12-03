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
        private TargetArea _targetArea;
        private string _inventoryGuid;
        private string _barangayInventoryGuid;
        private string _gearInventoryGuid;
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
        private List<string> _sitios = new List<string>();
        private ListViewItem _hitItem;
        private DateTime _dateProjectImplemented;

        public void SelectedSimilarName(string similarName, FisheryObjectNameType localNameType)
        {
            if (similarName.Length > 0)
            {
                if (localNameType == FisheryObjectNameType.CatchLocalName)
                {
                    cboCatchLocalName.Text = similarName;
                }
                else if (localNameType == FisheryObjectNameType.GearLocalName)
                {
                    cboSelectGearLocalName.Text = similarName;
                }
            }
        }

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

        public GearInventoryEditForm(string treeLevel, TargetArea aoi, FishingGearInventory inventory, GearInventoryForm parent)
        {
            InitializeComponent();
            _treeLevel = treeLevel;
            _targetArea = aoi;
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
            _sitios.Clear();
            _sitios = _inventory.GetBarangaySitios(province, municipality, barangay);
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
            SetupEnumeratorsComboBox();
            _dateProjectImplemented = _inventory.Inventories[_inventoryGuid].DateConducted;
            Text = "Add barangay fisher and fishing boat inventory data";
        }

        private void SetupEnumeratorsComboBox()
        {
            var surveyEnumerators = Enumerators.AOIEnumeratorsList(_targetArea.TargetAreaGuid);
            cboBarangaySurveyEnumerator.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboBarangaySurveyEnumerator.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBarangaySurveyEnumerator.ValueMember = "Key";
            cboBarangaySurveyEnumerator.DisplayMember = "Value";
            foreach (var item in surveyEnumerators)
            {
                cboBarangaySurveyEnumerator.Items.Add(item);
            }
            if (cboBarangaySurveyEnumerator.Items.Count > 0)
            {
                cboBarangaySurveyEnumerator.SelectedIndex = 0;
            }
        }

        private void FillUpMonths()
        {
            //setup the checklistbox of months of gear use and peak season
            var monthArr = new string[] { "January", "February", "March", "April", "May", "June",
                                           "July", "August", "September", "October", "November", "December" };

            for (int n = 0; n < monthArr.Length; n++)
            {
                chkListBoxMonthsSeason.Items.Add(monthArr[n]);
                chkListBoxMonthsUsed.Items.Add(monthArr[n]);
            }
        }

        private void SetupGearInventoryUI()
        {
            //setup the gearclass combo box
            cboGearClass.DataSource = new BindingSource(Gear.GearClass, null);
            cboGearClass.DisplayMember = "Value";
            cboGearClass.ValueMember = "Key";
            cboGearClass.AutoCompleteMode = AutoCompleteMode.Suggest;
            cboGearClass.AutoCompleteSource = AutoCompleteSource.ListItems;

            //setup the combobox of gear local names
            cboSelectGearLocalName.Items.Clear();
            foreach (var item in Gear.GearLocalNames)
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

            FillUpMonths();

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
            foreach (var item in Names.LocalNameListDict)
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

            cboSelectAccessory.Items.Clear();
            foreach (var item in Gear.Accessories)
            {
                cboSelectAccessory.Items.Add(item);
            }
            cboSelectAccessory.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboSelectAccessory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSelectAccessory.Sorted = true;

            listViewExpenses.Items.Clear();
            listViewExpenses.FullRowSelect = true;
            SizeColumns(listViewExpenses);
        }

        public void AddNewGearInventory(string barangayInventoryGuid, int countCommercial, int countMunicipalMotorized, int countMunicipalNonMotorized, int countFishers)
        {
            var item = _inventory.GetMunicipalityBrangaySitioFromBarangayInventory(barangayInventoryGuid);
            _dataStatus = fad3DataStatus.statusNew;
            _barangayInventoryGuid = barangayInventoryGuid;
            pnlGear.Visible = true;
            _countCommercial = countCommercial;
            _countFishers = countFishers;
            _countMunicipalMotorized = countMunicipalMotorized;
            _countMunicipalNonMotorized = countMunicipalNonMotorized;
            SetupGearInventoryUI();
            var sitio = item.sitio.Length > 0 ? item.sitio : "Entire barangay";
            Text = "Add fishing gear inventory data";
            lblLocation.Text = $"{sitio}, {item.barangay}, {item.municipality}, {item.province}";
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
            Text = "Add fishery inventory project";
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
                && txtDominantPercentage.Text.Length > 0
                && IsCatchValuesOK();

            if (proceed && (listBoxAccessories.Items.Count == 0 || listViewExpenses.Items.Count == 0 || listViewHistoryCpue.Items.Count == 0))
            {
                var n = 0;
                var msg = "";
                if (listBoxAccessories.Items.Count == 0)
                {
                    msg += "Fishing accessories\r\n";
                    n++;
                }
                if (listViewExpenses.Items.Count == 0)
                {
                    msg += "Expenses\r\n";
                    n++;
                }
                if (listViewHistoryCpue.Items.Count == 0)
                {
                    msg += "CPUE history";
                    n++;
                }
                msg = $"The following {(n == 1 ? "item is " : "items are")} not filled up:\r\n{msg}";

                proceed = MessageBox.Show($"{msg}\r\nDo you still want to proceed?",
                    "Confirmation needed", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes;
            }

            //if (proceed)
            //{
            //    proceed = MessageBox.Show("List of accessories or expenses is not filled up\r\nDo you still want to proceed?",
            //        "Confirmation needed", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes;
            //}
            if (!proceed)
            {
                MessageBox.Show("Cannot save because there are required information that are missing",
                                "Failed to save",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
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
                        && comboProvince.Text.Length > 0
                        && txtBarangaySurveyDate.Text.Length > 0
                        && cboBarangaySurveyEnumerator.Text.Length > 0;

                case "sitio":
                    return ValidateGearInventory();
            }
            return false;
        }

        public void EditInventoryLevel(string inventoryGuid, string InventoryName, DateTime dateImplemented)
        {
            _treeLevel = "root";
            _inventoryGuid = inventoryGuid;
            pnlInventory.Visible = true;
            _dataStatus = fad3DataStatus.statusFromDB;

            txtInventoryName.Text = InventoryName;
            txtDateImplemented.Text = string.Format("{0:MMM-dd-yyyy}", dateImplemented);

            Text = "Edit fishery inventory project";
        }

        public void EditInventoryLevel(string sitioGearInventoryGuid)
        {
            _treeLevel = "sitio";
            var gearData = _inventory.GetGearVariationInventoryData(sitioGearInventoryGuid);
            _gearInventoryGuid = sitioGearInventoryGuid;
            pnlGear.Visible = true;
            _dataStatus = fad3DataStatus.statusFromDB;
            SetupGearInventoryUI();

            cboGearClass.Text = gearData.gearClass;
            cboGearVariation.Text = gearData.gearVariation;

            foreach (var item in gearData.gearLocalNames)
            {
                if (Gear.GearLocalNames.Values.Contains(item))
                {
                    foreach (var localName in Gear.GearLocalNames)
                    {
                        if (localName.Value == item)
                        {
                            listBoxGearLocalNames.Items.Add(localName);
                            break;
                        }
                    }
                }
            }

            txtCommercialUsage.Text = gearData.commercialCount.ToString();
            txtMunicipalMotorizedUsage.Text = gearData.motorizedCount.ToString();
            txtMunicipalNonMotorizedUsage.Text = gearData.nonMotorizedCount.ToString();
            txtNoBoatGears.Text = gearData.noBoatCount.ToString();

            foreach (var item in gearData.monthsInUse)
            {
                chkListBoxMonthsUsed.SetItemCheckState(item - 1, CheckState.Checked);
            }
            chkListBoxMonthsSeason.Enabled = chkListBoxMonthsUsed.CheckedItems.Count > 0;

            foreach (var item in gearData.peakMonths)
            {
                chkListBoxMonthsSeason.SetItemCheckState(item - 1, CheckState.Checked);
            }
            txtNumberOfDaysPerMonth.Text = gearData.numberDaysGearUsedPerMonth.ToString();

            cboCatchUnit.Text = gearData.cpueUnit;
            txtRangeMax.Text = gearData.cpueRangeMax.ToString();
            txtRangeMin.Text = gearData.cpueRangeMin.ToString();
            txtModeUpper.Text = gearData.cpueModeUpper.ToString();
            txtModeLower.Text = gearData.cpueModeLower.ToString();

            foreach (var item in gearData.historicalCPUE)
            {
                foreach (ListViewItem lvi in listViewHistoryCpue.Items)
                {
                    if (lvi.Text == item.decade.ToString() + "s")
                    {
                        lvi.SubItems.Add(item.cpue.ToString());
                        lvi.SubItems.Add(item.unit);
                        break;
                    }
                }
            }

            foreach (var item in gearData.dominantCatch)
            {
                foreach (var name in Names.LocalNameListDict)
                {
                    if (name.Value == item)
                    {
                        listBoxDominantCatch.Items.Add(name);
                        break;
                    }
                }
            }

            foreach (var item in gearData.nonDominantCatch)
            {
                foreach (var name in Names.LocalNameListDict)
                {
                    if (name.Value == item)
                    {
                        listBoxOtherCatch.Items.Add(name);
                        break;
                    }
                }
            }

            foreach (var item in gearData.accessories)
            {
                listBoxAccessories.Items.Add(item);
            }

            foreach (var item in gearData.expenses)
            {
                var lvi = listViewExpenses.Items.Add(item.expense, item.expense, null);
                lvi.SubItems.Add(item.cost.ToString());
                lvi.SubItems.Add(item.source);
                lvi.SubItems.Add(item.notes);
            }
            txtNotes.Text = gearData.notes;
            txtDominantPercentage.Text = gearData.percentageOfDominance.ToString();

            Text = "Edit fishing gear inventory data";
            var location = _inventory.GetMunicipalityBrangaySitioFromGearInventory(sitioGearInventoryGuid);
            var sitio = location.sitio.Length > 0 ? location.sitio : "Entire barangay";
            lblLocation.Text = $"{sitio}, {location.barangay}, {location.municipality}, {location.province}";
        }

        public void EditInventoryLevel(string barangayInventoryGuid, int municipalityNumber, string barangay, string sitio,
                                    int fisherCount, int motorizedCount, int nonMotorizedCount, int commercialCount,
                                    DateTime? surveyDate, string enumeratorName)
        {
            SetupProvinceComboBox();
            SetupEnumeratorsComboBox();
            _municipalityNumber = municipalityNumber;
            _treeLevel = "barangay";
            _barangayInventoryGuid = barangayInventoryGuid;
            pnlBarangay.Visible = true;
            _dataStatus = fad3DataStatus.statusFromDB;
            txtCountFishers.Text = fisherCount.ToString();
            txtCountMotorized.Text = motorizedCount.ToString();
            txtCountNonMotorized.Text = nonMotorizedCount.ToString();
            txtCountCommercial.Text = commercialCount.ToString();
            txtBarangaySurveyDate.Text = surveyDate == null ? "" : string.Format("{0:MMM-dd-yyyy}", surveyDate);
            cboBarangaySurveyEnumerator.Text = enumeratorName;

            var location = global.ProvinceMunicipalityNamesFromMunicipalityNumber(municipalityNumber);
            comboProvince.Text = location.province;
            comboMunicipality.Text = location.municipality;
            comboBarangays.Text = barangay;
            if (sitio != "Entire barangay")
            {
                txtSitio.Text = sitio;
            }

            Text = "Edit barangay fisher and fishing boat inventory data";
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
                                if (_dataStatus != fad3DataStatus.statusNew)
                                {
                                    guid = _inventoryGuid;
                                }
                                success = _inventory.SaveFishingGearInventory(_dataStatus, txtInventoryName.Text, DateTime.Parse(txtDateImplemented.Text), guid);
                                if (success)
                                {
                                    if (_dataStatus == fad3DataStatus.statusNew)
                                    {
                                        _parentForm.RefreshMainInventory();
                                    }
                                    else if (_dataStatus == fad3DataStatus.statusEdited)
                                    {
                                        _parentForm.RefreshMainInventory(txtInventoryName.Text);
                                    }
                                }
                                break;

                            case "targetAreaInventory":
                            case "province":
                            case "municipality":
                            case "barangay":
                                if (_dataStatus != fad3DataStatus.statusNew)
                                {
                                    guid = _barangayInventoryGuid;
                                }
                                var enumeratorGuid = ((KeyValuePair<string, string>)cboBarangaySurveyEnumerator.SelectedItem).Key;
                                success = _inventory.SaveBarangayInventory(_dataStatus, _inventoryGuid, _municipalityNumber, comboBarangays.Text,
                                                                           int.Parse(txtCountFishers.Text), int.Parse(txtCountCommercial.Text),
                                                                           int.Parse(txtCountMotorized.Text), int.Parse(txtCountNonMotorized.Text),
                                                                           guid, DateTime.Parse(txtBarangaySurveyDate.Text), enumeratorGuid,
                                                                           txtSitio.Text);

                                if (success) _parentForm.RefreshSitioLevelInventory(comboProvince.Text, comboMunicipality.Text, comboBarangays.Text, txtSitio.Text);
                                break;

                            case "sitio":

                                if (_dataStatus != fad3DataStatus.statusNew)
                                {
                                    guid = _gearInventoryGuid;
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

                                List<string> listAccessories = new List<string>();
                                foreach (string item in listBoxAccessories.Items)
                                {
                                    listAccessories.Add(item);
                                }

                                List<(string expenseItem, double cost, string source, string notes)> listExpenses = new List<(string expenseItem, double cost, string source, string notes)>();
                                foreach (ListViewItem item in listViewExpenses.Items)
                                {
                                    if (item.SubItems.Count > 1)
                                    {
                                        listExpenses.Add((item.Text, double.Parse(item.SubItems[1].Text), item.SubItems[2].Text, item.SubItems[3].Text));
                                    }
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
                                    guid, _dataStatus, rangeCPUEMax, rangeCPUEMin, modeCPUEUpper, modeCPUELower, txtNotes.Text)

                                    && _inventory.SaveSitioGearInventoryGearLocalNames(guid, listGearLocalNameGuids)

                                    && _inventory.SaveSitioGearInventoryFishingMonths(guid, listMonthGearUse, listMonthGearSeason)

                                    && _inventory.SaveSitioGearInventoryCatchComposition(guid, listDominantCatchNameGuid, listNonDominantCatchNameGuid);

                                if (success)
                                {
                                    _inventory.SaveSitioGearInventoryAccessories(guid, listAccessories);
                                    _inventory.SaveSitioGearInventoryExpenses(guid, listExpenses);
                                    _inventory.SaveSitioGearInventoryHistoricalCPUE(guid, listHistoryCPUE);

                                    _parentForm.RefreshSitioGearInventory(guid, _dataStatus == fad3DataStatus.statusNew);
                                }
                                break;
                        }

                        if (success)
                        {
                            Close();
                        }
                    }
                    else
                    {
                        switch (_treeLevel)
                        {
                            case "root":
                            case "targetAreaInventory":
                            case "province":
                            case "municipality":
                            case "barangay":
                                MessageBox.Show("Cannot save because there are required data that are missing",
                                                 "Failed to save",
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Information);
                                break;
                        }
                    }
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnAddLocalName":
                    if (cboSelectGearLocalName.Text.Length > 0)
                    {
                        if (cboSelectGearLocalName.SelectedItem != null)
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
                    }
                    break;

                case "btnRemoveLocalName":
                    listBoxGearLocalNames.Items.Remove(listBoxGearLocalNames.SelectedItem);
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
                    listBoxDominantCatch.Items.Remove(listBoxDominantCatch.SelectedItem);
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
                    listBoxOtherCatch.Items.Remove(listBoxOtherCatch.SelectedItem);
                    break;

                case "btnAddAccessory":
                    if (cboSelectAccessory.Text.Length > 0)
                    {
                        if (!listBoxAccessories.Items.Contains(cboSelectAccessory.Text))
                        {
                            listBoxAccessories.Items.Add(cboSelectAccessory.Text);
                        }
                        else
                        {
                            MessageBox.Show("Item already in the list");
                        }
                        cboSelectAccessory.SelectedIndex = -1;
                    }
                    break;

                case "btnRemoveAccessory":
                    listBoxAccessories.Items.Remove(listBoxAccessories.SelectedItem);
                    break;

                case "btnAddExpense":
                    AddExpense();
                    break;
            }
        }

        private void AddExpense(ListViewItem lvi = null)
        {
            using (FishingExpenseForm fef = new FishingExpenseForm(this))
            {
                if (lvi != null)
                {
                    fef.ExpenseItem = lvi.Text;
                    fef.Cost = double.Parse(lvi.SubItems[1].Text);
                    fef.Source = lvi.SubItems[2].Text;
                    fef.Notes = lvi.SubItems[3].Text;
                }
                fef.ShowDialog(this);
                if (fef.DialogResult == DialogResult.OK)
                {
                    if (lvi == null)
                    {
                        var item = listViewExpenses.Items.Add(fef.ExpenseItem, fef.ExpenseItem, null);
                        item.SubItems.Add(fef.Cost.ToString());
                        item.SubItems.Add(fef.Source);
                        item.SubItems.Add(fef.Notes);
                    }
                    else
                    {
                        var item = listViewExpenses.Items[lvi.Text];
                        item.Text = fef.ExpenseItem;
                        item.SubItems[1].Text = fef.Cost.ToString();
                        item.SubItems[2].Text = fef.Source;
                        item.SubItems[3].Text = fef.Notes;
                    }
                    SizeColumns(listViewExpenses, false);
                }
            }
        }

        public void RefreshFishingExpense(string oldExpenseItem, string expenseItem, double cost, string source, string notes)
        {
            if (listViewExpenses.Items.Count == 0)
            {
                AddNewExpenseItem(expenseItem, cost, source, notes);
            }
            else if (listViewExpenses.Items.ContainsKey(oldExpenseItem))
            {
                var lvi = listViewExpenses.Items[oldExpenseItem];
                lvi.Text = expenseItem;
                lvi.SubItems[1].Text = cost.ToString();
                lvi.SubItems[2].Text = source;
                lvi.SubItems[3].Text = notes;
            }
            else
            {
                AddNewExpenseItem(expenseItem, cost, source, notes);
            }
        }

        private void AddNewExpenseItem(string expenseItem, double cost, string source, string notes)
        {
            var item = listViewExpenses.Items.Add(expenseItem, expenseItem, null);
            item.SubItems.Add(cost.ToString());
            item.SubItems.Add(source);
            item.SubItems.Add(notes);
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

                    case "txtBarangaySurveyDate":

                        if (DateTime.TryParse(s, out DateTime sDate))
                        {
                            if (sDate < _dateProjectImplemented || sDate > DateTime.Now)
                            {
                                e.Cancel = true;
                                msg = "Survey date must be between date of implementation of project and the current date";
                            }
                        }
                        else
                        {
                            msg = "Not a valid date";
                            e.Cancel = true;
                        }

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
                                var dialogResult = MessageBox.Show($"This date is more than {yearDiff} {(yearDiff == 1 ? "year" : "years")} ago.\r\nDo you want to accept this date?", "Accept past date", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                e.Cancel = dialogResult != DialogResult.Yes;
                            }
                        }
                        break;

                    case "txtSitio":
                        e.Cancel = _sitios.Contains(txtSitio.Text);
                        msg = "Sitio is already listed";
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
                    case "txtCountFishers":
                    case "txtCountCommercial":
                    case "txtCountMotorized":
                    case "txtCountNonMotorized":
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
                                || ctlName == "txtNoBoatGears"
                                || ctlName == "txtCountCommercial"
                                || ctlName == "txtCountMotorized"
                                || ctlName == "txtCountNonMotorized")
                            {
                                e.Cancel = v < 0;
                                msg = "Cannot accept values less than zero";

                                //if (ctlName == "txtCommercialUsage")
                                //{
                                //    e.Cancel = v > _countCommercial;
                                //    msg = $"Gear distribution in commercial vessels ({v}) must not be more than total number of commercial vessels ({_countCommercial})";
                                //}

                                //if (ctlName == "txtMunicipalMotorizedUsage")
                                //{
                                //    e.Cancel = v > _countMunicipalMotorized;
                                //    msg = $"Gear distribution in municipal motorized vessels ({v}) must not be more than total number of municipal motorized vessels ({_countMunicipalMotorized})";
                                //}

                                //if (ctlName == "txtMunicipalNonMotorizedUsage")
                                //{
                                //    e.Cancel = v > _countMunicipalNonMotorized;
                                //    msg = $"Gear distribution in municipal non-motorized vessels ({v}) must not be more than total number of municipal non-motorized vessels ({_countMunicipalNonMotorized})";
                                //}
                            }
                            else if (ctlName == "txtNumberOfDaysPerMonth")
                            {
                                e.Cancel = v < 1 || v > 31;
                                msg = "Number of days must be from 1 to 31";
                            }
                            else if (ctlName == "txtDominantPercentage")
                            {
                                e.Cancel = v < 51 || v > 100;
                                msg = "Dominant percent value must be more than 50 but not exceed 100";
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
            foreach (var item in Gear.GearVariationsUsage(gearKey))
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

        private bool ComboItemsContains(ComboBox cbo, string itemName)
        {
            bool isContaining = false;
            if (cbo.Name == "cboSelectAccessory")
            {
                foreach (string item in cbo.Items)
                {
                    isContaining = item == itemName;
                    if (isContaining)
                    {
                        return isContaining;
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> kv in cbo.Items)
                {
                    isContaining = kv.Value == itemName;
                    if (isContaining)
                    {
                        return isContaining;
                    }
                }
            }
            return false;
        }

        private void OnComboValidating(object sender, CancelEventArgs e)
        {
            if (sender.GetType().Name == "ComboBox")
            {
                var ctlName = ((ComboBox)sender).Name;
                var s = ((ComboBox)sender).Text;
                var cbo = (ComboBox)sender;
                var msg = "";
                var gearClass = "";
                if (s.Length > 0)
                {
                    switch (ctlName)
                    {
                        case "cboCatchLocalName":
                        case "cboSelectGearLocalName":
                        case "cboGearVariation":
                        case "cboSelectAccessory":
                            if (ComboItemsContains(cbo, s))
                            {
                                if (ctlName == "cboGearVariation")
                                {
                                    //we check database if gear variation is already in the inventory then cancel if true
                                    if (_dataStatus == fad3DataStatus.statusNew && _inventory.IsGearInInventory(_barangayInventoryGuid, _gearVariationKey))
                                    {
                                        msg = "Cannot accept selected gear\r\nbecause it has been inventoried";
                                        MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        e.Cancel = true;
                                    }
                                }
                            }
                            else
                            {
                                FisheryObjectNameType nameType = FisheryObjectNameType.CatchLocalName;
                                switch (ctlName)
                                {
                                    case "cboSelectAccessory":
                                        msg = $"{s} is not in the list of accessories.\r\nDo you want to add a new accessory?";
                                        nameType = FisheryObjectNameType.FishingAccessory;
                                        break;

                                    case "cboCatchLocalName":
                                        msg = $"{s} is not in the list of catch local names.\r\nDo you want to add a new local name?";
                                        break;

                                    case "cboSelectGearLocalName":
                                        msg = $"{s} is not in the list of gear local names.\r\nDo you want to add a new local name?";
                                        nameType = FisheryObjectNameType.GearLocalName;
                                        break;

                                    case "cboGearVariation":
                                        msg = $"{s} is not in the list of gear variations.\r\nDo you want to add a new gear variation name?";
                                        nameType = FisheryObjectNameType.GearVariationName;
                                        gearClass = ((KeyValuePair<string, string>)cboGearClass.SelectedItem).Key;
                                        break;
                                }
                                var result = MessageBox.Show(msg, "Add new name", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (result == DialogResult.Yes)
                                {
                                    NewFisheryObjectName newName = new NewFisheryObjectName(s, nameType);
                                    using (NewNameForm nmf = new NewNameForm(newName, gearClass))
                                    {
                                        nmf.ShowDialog(this);
                                        if (nmf.DialogResult == DialogResult.OK)
                                        {
                                            if (ctlName == "cboSelectAccessory")
                                            {
                                                cboSelectAccessory.Items.Add(newName.NewName);
                                                listBoxAccessories.Items.Add(newName.NewName);
                                                cbo.Text = "";
                                            }
                                            else
                                            {
                                                KeyValuePair<string, string> kv = new KeyValuePair<string, string>(newName.ObjectGUID, newName.NewName);
                                                cbo.Items.Add(kv);
                                                switch (ctlName)
                                                {
                                                    case "cboCatchLocalName":
                                                        _activeCatchCompList.Items.Add(kv);
                                                        cbo.Text = "";
                                                        break;

                                                    case "cboSelectGearLocalName":
                                                        listBoxGearLocalNames.Items.Add(kv);
                                                        cbo.Text = "";
                                                        break;

                                                    case "cboGearVariation":

                                                        break;
                                                }
                                            }
                                        }
                                        else if (nmf.DialogResult == DialogResult.Cancel && newName.UseThisName != null)
                                        {
                                            cbo.Text = newName.UseThisName;
                                        }
                                    }
                                }
                                else
                                {
                                    e.Cancel = true;
                                }
                            }
                            break;

                        case "comboBarangays":
                            if (!comboBarangays.Items.Contains(s))
                            {
                                msg = $"'{s}' is not in list of barangays\r\nDo you want to add a new barangay?";
                                var result = MessageBox.Show(msg, "Create new barangay", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (result == DialogResult.Yes)
                                {
                                    comboBarangays.Items.Add(s);
                                    _sitios.Clear();
                                    _sitios = _inventory.GetBarangaySitios(comboProvince.Text, comboMunicipality.Text, s);
                                }
                                else
                                {
                                    e.Cancel = true;
                                }
                            }
                            break;

                        case "cboBarangaySurveyEnumerator":
                            bool enumeratorFound = false;
                            foreach (KeyValuePair<string, string> kv in cboBarangaySurveyEnumerator.Items)
                            {
                                enumeratorFound = kv.Value == s;
                                if (enumeratorFound) break;
                            }
                            if (!enumeratorFound)
                            {
                                msg = $"'{s}' is not in list of enumerators\r\nDo you want to add a new enumerator?";
                                var result = MessageBox.Show(msg, "Create new enumerator", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (result == DialogResult.Yes)
                                {
                                    EnumeratorEntryForm enf = new EnumeratorEntryForm(s, _targetArea.TargetAreaGuid);
                                    enf.ShowDialog(this);
                                    if (enf.DialogResult == DialogResult.OK)
                                    {
                                        KeyValuePair<string, string> newEnumerator = new KeyValuePair<string, string>(enf.EnumeratorGuid, enf.EnumeratorName);
                                        cboBarangaySurveyEnumerator.Items.Add(newEnumerator);
                                        cboBarangaySurveyEnumerator.Text = newEnumerator.Value;
                                    }
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

        private void OnFormLoad(object sender, EventArgs e)
        {
            pnlGear.Location = pnlInventory.Location;
            pnlBarangay.Location = pnlInventory.Location;

            Width = (pnlInventory.Left * 8) + pnlInventory.Width;
            global.LoadFormSettings(this, true);
            if (pnlGear.Visible)
            {
                cboGearVariation.Focus();
            }
        }

        private void OnListViewDblClick(object sender, EventArgs e)
        {
            switch (((ListView)sender).Name)
            {
                case "listViewHistoryCpue":

                    var historicalCPUEForm = new CPUEHistoricalForm(this, listViewHistoryCpue.SelectedItems[0].Text);
                    if (listViewHistoryCpue.SelectedItems[0].SubItems.Count > 1
                        && listViewHistoryCpue.SelectedItems[0].SubItems[1].Text.Length > 0
                        && listViewHistoryCpue.SelectedItems[0].SubItems[2].Text.Length > 0)
                    {
                        historicalCPUEForm.CatchValue(int.Parse(listViewHistoryCpue.SelectedItems[0].SubItems[1].Text),
                                                      listViewHistoryCpue.SelectedItems[0].SubItems[2].Text);
                    }
                    historicalCPUEForm.ShowDialog(this);
                    break;

                case "listViewExpenses":

                    AddExpense(listViewExpenses.SelectedItems[0]);

                    break;
            }
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
                chkListBoxMonthsSeason.Enabled = true;
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
                        CancelEventArgs ee = new CancelEventArgs(false);
                        OnComboValidating(cboSelectGearLocalName, ee);
                        if (!ee.Cancel)
                        {
                            OnButtonClick(btnAddLocalName, null);
                        }
                    }
                    break;

                case "cboSelectAccessory":
                    if (e.KeyData == Keys.Enter && cboSelectAccessory.Text.Length > 0)
                    {
                        CancelEventArgs ee = new CancelEventArgs(false);
                        OnComboValidating(cboSelectAccessory, ee);
                        if (!ee.Cancel)
                        {
                            OnButtonClick(btnAddAccessory, null);
                        }
                    }
                    break;

                case "cboCatchLocalName":
                    if (e.KeyData == Keys.Enter && cboCatchLocalName.Text.Length > 0)
                    {
                        CancelEventArgs ee = new CancelEventArgs(false);
                        OnComboValidating(cboCatchLocalName, ee);
                        if (!ee.Cancel)
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
                    }
                    break;
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
        }

        private void OnCheckListValidating(object sender, CancelEventArgs e)
        {
            switch (((CheckedListBox)sender).Name)
            {
                case "chkListBoxMonthsUsed":
                    break;

                case "chkListBoxMonthsSeason":
                    if (chkListBoxMonthsSeason.CheckedIndices.Count > 0)
                    {
                        var isFound = false;
                        foreach (int seasonMonth in chkListBoxMonthsSeason.CheckedIndices)
                        {
                            isFound = false;
                            foreach (int fishingMonth in chkListBoxMonthsUsed.CheckedIndices)
                            {
                                if (seasonMonth == fishingMonth)
                                {
                                    isFound = true;
                                    break;
                                }
                            }
                            if (!isFound) break;
                        }
                        e.Cancel = !isFound;
                        if (e.Cancel)
                        {
                            MessageBox.Show("A peak season month is not found in month(s) of operation", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    break;
            }
        }

        private void OnCheckListMouseUp(object sender, MouseEventArgs e)
        {
            chkListBoxMonthsSeason.Enabled = chkListBoxMonthsUsed.CheckedIndices.Count > 0;
            if (chkListBoxMonthsUsed.CheckedIndices.Count == 0 && chkListBoxMonthsSeason.CheckedIndices.Count > 0)
            {
                foreach (int item in chkListBoxMonthsSeason.CheckedIndices)
                {
                    chkListBoxMonthsSeason.SetItemChecked(item, false);
                }
            }
        }

        private void OnListViewMouseDown(object sender, MouseEventArgs e)
        {
            var lv = (ListView)sender;
            _hitItem = lv.HitTest(e.X, e.Y).Item;
            if (e.Clicks == 1)
            {
                switch (lv.Name)
                {
                    case "listViewExpenses":

                        break;
                }
            }
            else if (e.Clicks == 2)
            {
                switch (lv.Name)
                {
                    case "listViewExpenses":
                        AddExpense(_hitItem);
                        break;
                }
            }
        }
    }
}