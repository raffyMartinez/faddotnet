/*
 *
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/8/2016
 * Time: 8:17 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ISO_Classes;

namespace FAD3
{
    /// <summary>
    /// Description of frmLandingSite.
    /// </summary>
    public partial class LandingSiteForm : Form
    {
        private aoi _aoi;
        private bool _isNew = false;
        private Landingsite _landingSite;
        private string _lsGUID = "";
        private long _municipalityNumber;
        private MainForm _parentForm;
        private LandingSiteFromKMLForm _parentKMLForm;
        private string _landingSiteName;
        private double _xCoordinate;
        private double _yCoordinate;
        private bool _definedFromKML;
        private string _coordinateformat;
        private bool _hasCoordinate;

        public LandingSiteForm(aoi aoi, LandingSiteFromKMLForm kmlParentForm, string name, double xCoord, double yCoord, bool isNew = false, bool definedFromKML = true)
        {
            InitializeComponent();
            _aoi = aoi;
            _landingSiteName = name;
            _xCoordinate = xCoord;
            _yCoordinate = yCoord;
            _isNew = isNew;
            _definedFromKML = definedFromKML;
            SetupProvinceComboBox();
            _parentKMLForm = kmlParentForm;
        }

        public LandingSiteForm(aoi aoi, MainForm parent, Landingsite landingSite, bool isNew = false)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            _aoi = aoi;
            _parentForm = parent;
            _isNew = isNew;
            _landingSite = landingSite;

            Text = "Landing site";

            if (_isNew)
            {
                Text = "New landing site";
                _landingSite.IsNew();
            }

            SetupProvinceComboBox();
        }

        private void SetupProvinceComboBox()
        {
            comboProvince.DataSource = new BindingSource(global.provinceDict, null);
            comboProvince.DisplayMember = "Value";
            comboProvince.ValueMember = "Key";
            comboProvince.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboProvince.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        public Landingsite LandingSite
        {
            get { return _landingSite; }
            set { _landingSite = value; }
        }

        public string LSGUID
        {
            get { return _lsGUID; }
            set { _lsGUID = value; }
        }

        private void OnFormShown(object sender, EventArgs e)
        {
            textLandingSiteName.Focus();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            textLandingSiteName.Focus();
            if (_definedFromKML)
            {
                //DefineCoordinateFormat();
                _coordinateformat = global.CoordinateFormatCode;
                textLandingSiteName.Text = _landingSiteName;
                var coordinate = new Coordinate((float)_yCoordinate, (float)_xCoordinate);
                textCoord.Text = $"{coordinate.ToString(false, _coordinateformat)}, {coordinate.ToString(true, _coordinateformat)}";
            }
            else if (!_isNew && _landingSite != null)
            {
                Dictionary<string, string> myLSData = _landingSite.LandingSiteDataEx();
                if (myLSData.Count > 0)
                {
                    textLandingSiteName.Text = myLSData["LSName"];
                    comboProvince.Text = myLSData["ProvinceName"];
                    long key = ((KeyValuePair<long, string>)comboProvince.SelectedItem).Key;
                    SetMunicipalitiesCombo(key);
                    comboMunicipality.Text = myLSData["Municipality"];
                    _municipalityNumber = ((KeyValuePair<long, string>)comboMunicipality.SelectedItem).Key;
                    textCoord.Text = myLSData["CoordinateStringXY"];
                }
            }
        }

        private bool ValidateForm()
        {
            if (textLandingSiteName.Text.Length > 0 && comboMunicipality.Text.Length > 0 && comboProvince.Text.Length > 0)
            {
                if (aoi.LandingSiteFromName(textLandingSiteName.Text, _aoi.AOIGUID) == null)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("A landing site with the same name already exists", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return false;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOk":
                    if (ValidateForm())
                    {
                        if (!_definedFromKML)
                        {
                            string myGUID = "";
                            Dictionary<string, string> LSData = new Dictionary<string, string>();
                            long key = ((KeyValuePair<long, string>)comboMunicipality.SelectedItem).Key;
                            LSData.Add("LSName", textLandingSiteName.Text);
                            LSData.Add("MunNo", _municipalityNumber.ToString());
                            LSData.Add("HasCoordinate", (textCoord.Text.Length > 0).ToString());
                            LSData.Add("AOIGuid", _aoi.AOIGUID);
                            if (_isNew)
                            {
                                myGUID = Guid.NewGuid().ToString();
                            }
                            else
                            {
                                myGUID = _landingSite.LandingSiteGUID;
                            }
                            LSData.Add("LSGUID", myGUID);

                            if (_landingSite.UpdateData(_isNew, LSData))
                            {
                                if (_isNew)
                                {
                                    _parentForm.NewLandingSite(LSData["LSName"], myGUID);
                                }
                                else
                                {
                                    _parentForm.RefreshLV("landing_site");
                                }
                            }
                        }
                        else
                        {
                            _parentKMLForm.LandingSiteName = textLandingSiteName.Text;
                            _parentKMLForm.LandingSiteMunicipalityNumber = (int)_municipalityNumber;
                            _parentKMLForm.LandingSiteMunicipalityName = $"{comboMunicipality.Text}, {comboProvince.Text}";
                        }
                        Close();
                    }
                    else
                    {
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;
            }
        }

        private void OnComboBoxValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var msg = "Item not in list";
            long key;
            ((ComboBox)sender).With(o =>
            {
                var s = o.Text;
                if (s.Length > 0)
                {
                    switch (o.Name)
                    {
                        case "comboProvince":
                            try
                            {
                                key = ((KeyValuePair<long, string>)comboProvince.SelectedItem).Key;
                                SetMunicipalitiesCombo(key);
                            }
                            catch (System.NullReferenceException ex)
                            {
                                e.Cancel = true;
                            }

                            break;

                        case "comboMunicipality":

                            try
                            {
                                _municipalityNumber = ((KeyValuePair<long, string>)comboMunicipality.SelectedItem).Key;
                            }
                            catch (System.NullReferenceException ex)
                            {
                                e.Cancel = true;
                            }

                            break;
                    }
                }
            });
            if (e.Cancel)
            {
                MessageBox.Show(msg, "Validation error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OntextCoord_DoubleClick(object sender, EventArgs e)
        {
            if (!_definedFromKML)
            {
                CoordinateEntryForm cef = new CoordinateEntryForm(textCoord.Text.Length == 0, this, _landingSite.Coordinate);
                cef.Coordinate = _landingSite.Coordinate;
                cef.ShowDialog(this);

                if (_landingSite.Coordinate.Latitude > 0 && _landingSite.Coordinate.Longitude > 0)
                    textCoord.Text = _landingSite.Coordinate.ToString(global.CoordinateFormatCode);
            }
        }

        private void SetMunicipalitiesCombo(long ProvNo)
        {
            global.MunicipalitiesFromProvinceNo(ProvNo);
            //comboBox2.Items.Clear();
            comboMunicipality.DataSource = new BindingSource(global.munDict, null);
            comboMunicipality.DisplayMember = "Value";
            comboMunicipality.ValueMember = "Key";
            comboMunicipality.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboMunicipality.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
    }
}