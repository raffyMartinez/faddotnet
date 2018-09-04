using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FAD3
{
    public partial class MapEffortHelperForm : Form
    {
        private static MapEffortHelperForm _instance;
        private MainForm _parentForm;
        private string _treeLevel;

        public static MapEffortHelperForm GetInstance(MainForm parentForm)
        {
            if (_instance == null) _instance = new MapEffortHelperForm(parentForm);
            return _instance;
        }

        public MapEffortHelperForm(MainForm parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
        }

        private void OnMapForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            _parentForm.EffortMapperClosed();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    var fgmh = new FishingGroundMappingHandler();
                    fgmh.MapLayersHandler = global.MappingForm.MapLayersHandler;
                    var samplingYears = "";
                    foreach (ListViewItem lvi in lvYears.Items)
                    {
                        if (lvi.Checked)
                        {
                            samplingYears += $"{lvi.Text},";
                        }
                    }
                    if (samplingYears.Length > 0)
                    {
                        samplingYears = samplingYears.Trim(',');
                    }

                    if (global.MappingForm.NumLayers() > 0)
                    {
                        fgmh.set_GeoProjection(global.MappingForm.geoProjection);
                        bool aggregated = chkAggregate.Checked;
                        switch (_treeLevel)
                        {
                            case "aoi":
                                fgmh.MapFishingGrounds(_parentForm.AOIGUID, samplingYears, FishingGrid.UTMZone, aggregated);
                                break;

                            case "landing_site":
                                fgmh.MapFishingGrounds(_parentForm.AOIGUID, samplingYears, FishingGrid.UTMZone, aggregated, _parentForm.LandingSiteGUID);
                                break;

                            case "gear":
                                fgmh.MapFishingGrounds(_parentForm.AOIGUID, samplingYears, FishingGrid.UTMZone, aggregated, _parentForm.LandingSiteGUID, _parentForm.GearVariationGUID);
                                break;

                            case "sampling":
                                labelTitle.Text = $"Mapping of fishing effort on {_parentForm.SamplingMonth} using {_parentForm.GearVariationName} in {_parentForm.LandingSiteName}, {_parentForm.AOIName}";
                                break;
                        }
                    }
                    break;

                case "btnCancel":

                    Close();
                    break;
            }
        }

        private void FillUpSampledYears(List<(int year, int countSamplings)> sampledYears)
        {
            lvYears.Items.Clear();
            foreach (var item in sampledYears)
            {
                string sampledYear = item.year.ToString();
                ListViewItem lvi = new ListViewItem(new string[] { sampledYear, item.countSamplings.ToString() });
                lvi.Name = sampledYear;
                lvYears.Items.Add(lvi);
            }
            SizeColumns(lvYears, false);
        }

        public void SetUpMapping(string treeLevel)
        {
            _treeLevel = treeLevel;
            switch (_treeLevel)
            {
                case "aoi":
                    var aoi = _parentForm.AOI;
                    FillUpSampledYears(aoi.SampledYearsEx());
                    labelTitle.Text = $"Mapping of fishing effort in {_parentForm.AOIName}";
                    break;

                case "landing_site":
                    var landingSite = _parentForm.LandingSite;
                    FillUpSampledYears(landingSite.SampledYears());
                    labelTitle.Text = $"Mapping of fishing effort in {_parentForm.LandingSiteName}, {_parentForm.AOIName}";
                    break;

                case "gear":
                    landingSite = _parentForm.LandingSite;
                    FillUpSampledYears(landingSite.SampledYears(_parentForm.GearVariationGUID));
                    labelTitle.Text = $"Mapping of fishing effort using {_parentForm.GearVariationName} in {_parentForm.LandingSiteName}, {_parentForm.AOIName}";
                    break;

                case "sampling":
                    labelTitle.Text = $"Mapping of fishing effort on {_parentForm.SamplingMonth} using {_parentForm.GearVariationName} in {_parentForm.LandingSiteName}, {_parentForm.AOIName}";
                    break;
            }
        }

        /// <summary>
        /// Sizes all columns so that it fits the widest column content or the column header content
        /// </summary>
        private void SizeColumns(ListView lv, bool init = true, bool lastColBalnk = true)
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
            if (lastColBalnk)
            {
                lv.Columns[lv.Columns.Count - 1].Width = 0;
            }
        }

        private void MapEffortHelperForm_Load(object sender, EventArgs e)
        {
            lvYears.Columns.Clear();
            lvYears.View = View.Details;
            lvYears.FullRowSelect = true;
            lvYears.Columns.Add("colYear", "Year");
            lvYears.Columns.Add("colSamplings", "n");
            lvYears.Columns.Add("colBlank", "");
            SizeColumns(lvYears);
        }
    }
}