/*
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

namespace FAD3
{
    /// <summary>
    /// Description of frmLandingSite.
    /// </summary>
    public partial class frmLandingSite : Form
    {
        private aoi _AOI;
        private bool _isNew = false;
        private landingsite _LandingSite;
        private string _LSGUID = "";
        private long _MunicipalityNumber;
        private frmMain _ParentForm;
        private Dictionary<long, string> Provinces = new Dictionary<long, string>();

        public frmLandingSite(aoi aoi, frmMain Parent, landingsite LandingSite, bool IsNew = false)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            _AOI = aoi;
            _ParentForm = Parent;
            _isNew = IsNew;
            _LandingSite = LandingSite;
            if (_isNew) _LandingSite.IsNew();
            Provinces = global.provinceDict;
            comboProvince.DataSource = new BindingSource(Provinces, null);
            comboProvince.DisplayMember = "Value";
            comboProvince.ValueMember = "Key";
            comboProvince.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboProvince.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        public landingsite LandingSite
        {
            get { return _LandingSite; }
            set { _LandingSite = value; }
        }

        public string LSGUID
        {
            get { return _LSGUID; }
            set { _LSGUID = value; }
        }

        public void RefreshCoordinate()
        {
            var format = "D";
            switch (global.CoordinateDisplay)
            {
                case global.CoordinateDisplayFormat.DegreeDecimal:
                    break;

                case global.CoordinateDisplayFormat.DegreeMinute:
                    format = "DM";
                    break;

                case global.CoordinateDisplayFormat.DegreeMinuteSecond:
                    format = "DMS";
                    break;

                case global.CoordinateDisplayFormat.UTM:
                    break;
            }
            textCoord.Text = _LandingSite.Coordinate.ToString(format);
        }

        private void frmLandingSite_Shown(object sender, EventArgs e)
        {
            textLandingSiteName.Focus();
        }

        private void FrmLandingSiteLoad(object sender, EventArgs e)
        {
            textLandingSiteName.Focus();
            Text = "New landing site";
            if (!_isNew && _LandingSite != null)
            {
                Dictionary<string, string> myLSData = _LandingSite.LandingSiteDataEx();
                if (myLSData.Count > 0)
                {
                    textLandingSiteName.Text = myLSData["LSName"];
                    comboProvince.Text = myLSData["ProvinceName"];
                    long key = ((KeyValuePair<long, string>)comboProvince.SelectedItem).Key;
                    SetMunicipalitiesCombo(key);
                    comboMunicipality.Text = myLSData["Municipality"];
                    _MunicipalityNumber = ((KeyValuePair<long, string>)comboMunicipality.SelectedItem).Key;
                    textCoord.Text = myLSData["CoordinateStringXY"];
                }
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOk":
                    string myGUID = "";
                    Dictionary<string, string> LSData = new Dictionary<string, string>();
                    long key = ((KeyValuePair<long, string>)comboMunicipality.SelectedItem).Key;
                    LSData.Add("LSName", textLandingSiteName.Text);
                    LSData.Add("MunNo", _MunicipalityNumber.ToString());
                    LSData.Add("HasCoordinate", (textCoord.Text.Length > 0).ToString());
                    LSData.Add("AOIGuid", _AOI.AOIGUID);
                    if (_isNew)
                    {
                        myGUID = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        myGUID = _LandingSite.LandingSiteGUID;
                    }
                    LSData.Add("LSGUID", myGUID);

                    if (_LandingSite.UpdateData(_isNew, LSData))
                    {
                        if (_isNew) _ParentForm.NewLandingSite(LSData["LSName"], myGUID);
                        else _ParentForm.RefreshLV("landing_site");
                        Close();
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
                                _MunicipalityNumber = ((KeyValuePair<long, string>)comboMunicipality.SelectedItem).Key;
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
            CoordinateEntryForm cef = new CoordinateEntryForm(textCoord.Text.Length == 0, this, _LandingSite.Coordinate);
            cef.Coordinate = _LandingSite.Coordinate;
            cef.ShowDialog(this);
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