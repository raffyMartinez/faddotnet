/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/8/2016
 * Time: 8:17 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Drawing;
using System.Windows.Forms;

using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;

namespace FAD3
{
    /// <summary>
    /// Description of frmLandingSite.
    /// </summary>
    public partial class frmLandingSite : Form
    {
        private string _LSGUID = "";
        private Dictionary<long, string> Provinces = new Dictionary<long, string>();
        private bool _isNew = false;
        private aoi _AOI;

        private landingsite _LandingSite;

        private long _MunicipalityNumber;
        private (double? x, double? y) _LandingSitePosition;

        public string LSGUID
        {
            get { return _LSGUID; }
            set { _LSGUID = value; }
        }

        public void AddNew()
        {
            _isNew = true;
        }

        public landingsite LandingSite
        {
            get { return _LandingSite; }
            set { _LandingSite = value; }
        }

        public frmLandingSite(aoi aoi)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            _AOI = aoi;
            Provinces = global.provinceDict;
            comboProvince.DataSource = new BindingSource(Provinces, null);
            comboProvince.DisplayMember = "Value";
            comboProvince.ValueMember = "Key";
            comboProvince.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboProvince.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void OnButtoClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOk":
                    string myGUID = "";
                    Dictionary<string, string> LSData = new Dictionary<string, string>();
                    long key = ((KeyValuePair<long, string>)comboMunicipality.SelectedItem).Key;
                    LSData.Add("LSName", textLandingSiteName.Text);
                    LSData.Add("MunNo", _MunicipalityNumber.ToString());
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

                    if (landingsite.UpdateData(_isNew, LSData))
                    {
                        frmMain fr = new frmMain();
                        foreach (Form f in Application.OpenForms)
                        {
                            if (f.Name == "frmMain")
                            {
                                fr = (frmMain)f;
                                fr.RefreshLV(LSData["LSName"], "landing_site", _isNew, myGUID);
                            }
                        }
                        this.Close();
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;
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

        private void FrmLandingSiteLoad(object sender, EventArgs e)
        {
            textLandingSiteName.Focus();
            Text = "New landing site";
            if (_LandingSite != null)
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
                    if (myLSData["cx"].Length > 0 && myLSData["cy"].Length > 0)
                    {
                        if (double.TryParse(myLSData["cx"], out double x))
                            _LandingSitePosition.x = x;

                        if (double.TryParse(myLSData["cy"], out double y))
                            _LandingSitePosition.y = y;
                    }
                }
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

        private void frmLandingSite_Shown(object sender, EventArgs e)
        {
            textLandingSiteName.Focus();
        }

        private void OntextCoord_DoubleClick(object sender, EventArgs e)
        {
            CoordinateEntryForm cef = new CoordinateEntryForm(textCoord.Text.Length == 0, this, _LandingSitePosition);
            cef.Coordinate = _LandingSite.Coordinate;
            cef.ShowDialog(this);
        }

        public void LandingSiteCoordinate((double? x, double? y) Coordinate)
        {
        }
    }
}