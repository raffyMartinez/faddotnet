using FAD3.Database.Classes;
using FAD3.Database.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FAD3
{
    public partial class MapEffortHelperForm : Form
    {
        private static MapEffortHelperForm _instance;
        private MainForm _parentForm;
        private string _treeLevel;
        private MapperForm _mapperForm;
        private string _gearVariationGuid;
        private string _targetAreaGuid;
        public bool BatchMode { get; set; }
        public bool CombineYearsInOneMap { get; set; }

        public static MapEffortHelperForm GetInstance(MainForm parentForm)
        {
            if (_instance == null) _instance = new MapEffortHelperForm(parentForm);
            return _instance;
        }

        public static MapEffortHelperForm GetInstance()
        {
            if (_instance == null) _instance = new MapEffortHelperForm();
            return _instance;
        }

        public MapEffortHelperForm(MainForm parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
            _gearVariationGuid = "";
        }

        public MapEffortHelperForm()
        {
            InitializeComponent();
        }

        private void OnMapForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            _parentForm?.EffortMapperClosed();
            global.MappingMode = fad3MappingMode.defaultMode;
        }

        private void MapCheckedYears()
        {
            var fgmh = new FishingGroundMappingHandler();
            fgmh.MapControl = global.MappingForm.MapControl;
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
                fgmh.set_GeoProjection(global.MappingForm.GeoProjection);
                bool aggregated = chkAggregate.Checked;
                bool notInclude1 = chkNotInclude1.Checked;
                switch (_treeLevel)
                {
                    case "target_area":
                        fgmh.MapFishingGrounds(_parentForm.TargetAreaGuid, samplingYears, FishingGrid.UTMZone, aggregated, notInclude1);
                        break;

                    case "landing_site":
                        fgmh.MapFishingGrounds(_parentForm.TargetAreaGuid, samplingYears, FishingGrid.UTMZone, aggregated, notInclude1, _parentForm.LandingSiteGUID);
                        break;

                    case "gear":
                        fgmh.MapFishingGrounds(_parentForm.TargetAreaGuid, samplingYears, FishingGrid.UTMZone, aggregated, notInclude1, _parentForm.LandingSiteGUID, _parentForm.GearVariationGUID);
                        break;

                    case "sampling":
                        labelTitle.Text = $"Mapping of fishing effort on {_parentForm.SamplingMonth} using {_parentForm.GearVariationName} in {_parentForm.LandingSiteName}, {_parentForm.TargetAreaName}";
                        break;
                }
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (_gearVariationGuid.Length == 0)
                    {
                        MapCheckedYears();
                    }
                    else
                    {
                        MapTargetAreaGearFishingGround();
                    }
                    break;

                case "btnCancel":

                    Close();
                    break;
            }
        }

        private void MapGear(List<int> years)
        {
            var sf = Mapping.Classes.FishingGearMapping.MapThisGear(_targetAreaGuid, _gearVariationGuid, years, chkAggregate.Checked, chkNotInclude1.Checked, chkRemoveInland.Checked);
            if (sf != null)
            {
                global.MappingForm.MapLayersHandler.AddLayer(sf, "Fishing ground of gears", true, true);
                sf.DefaultDrawingOptions.PointShape = MapWinGIS.tkPointShapeType.ptShapeCircle;
                sf.DefaultDrawingOptions.PointSize = 7;
                sf.DefaultDrawingOptions.FillColor = new MapWinGIS.Utils().ColorByName(MapWinGIS.tkMapColor.Red);
                sf.DefaultDrawingOptions.LineColor = new MapWinGIS.Utils().ColorByName(MapWinGIS.tkMapColor.White);
                sf.SelectionAppearance = MapWinGIS.tkSelectionAppearance.saDrawingOptions;
                sf.SelectionDrawingOptions.PointShape = sf.DefaultDrawingOptions.PointShape;
                sf.SelectionDrawingOptions.PointSize = sf.DefaultDrawingOptions.PointSize;
            }
        }

        public void MapTargetAreaGearFishingGroundBatch(MappingBatchForm f)
        {
            List<int> years = new List<int>();
            if (BatchMode)
            {
                if (!CombineYearsInOneMap)
                {
                    foreach (ListViewItem lvi in lvYears.Items)
                    {
                        years.Clear();
                        years.Add(int.Parse(lvi.Text));
                        MapGear(years);
                        f.MappedYears(years);
                    }
                }
                else
                {
                    foreach (ListViewItem lvi in lvYears.Items)
                    {
                        years.Add(int.Parse(lvi.Text));
                    }
                    MapGear(years);
                    f.MappedYears(years);
                }
            }
        }

        public void MapTargetAreaGearFishingGround()
        {
            List<int> years = new List<int>();

            foreach (ListViewItem lvi in lvYears.Items)
            {
                if (lvi.Checked)
                {
                    years.Add(int.Parse(lvi.Text));
                }
            }

            MapGear(years);
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

            if (!BatchMode)
                SizeColumns(lvYears, false);
        }

        /// <summary>
        /// map out fishing ground of gears in a target area
        /// </summary>
        /// <param name="targetAreaGuid"></param>
        /// <param name="gearVariationGuid"></param>
        public void SetUpMapping(string targetAreaGuid, string gearVariationGuid, string gearVariationName, string targetArea, bool MapFirstItemInList = false, bool RemoveInlandPoints = false)
        {
            FillUpSampledYears(Gears.GearUseCountInTargetArea(targetAreaGuid, gearVariationGuid));
            labelTitle.Text = $"Mapping of fishing grounds of {gearVariationName} in {targetArea}";

            _gearVariationGuid = gearVariationGuid;
            _targetAreaGuid = targetAreaGuid;
            if (MapFirstItemInList)
            {
                lvYears.Items[0].Checked = true;
                MapTargetAreaGearFishingGround();
            }
        }

        public void SetUpMapping(string treeLevel)
        {
            _treeLevel = treeLevel;
            switch (_treeLevel)
            {
                case "target_area":
                    if (_parentForm == null)
                    {
                        _parentForm = global.mainForm;
                    }
                    var targetArea = _parentForm.TargetArea;
                    FillUpSampledYears(targetArea.SampledYearsEx());
                    labelTitle.Text = $"Mapping of fishing effort in {_parentForm.TargetAreaName}";
                    break;

                case "landing_site":
                    var landingSite = _parentForm.LandingSite;
                    FillUpSampledYears(landingSite.SampledYears());
                    labelTitle.Text = $"Mapping of fishing effort in {_parentForm.LandingSiteName}, {_parentForm.TargetAreaName}";
                    break;

                case "gear":
                    landingSite = _parentForm.LandingSite;
                    FillUpSampledYears(landingSite.SampledYears(_parentForm.GearVariationGUID));
                    labelTitle.Text = $"Mapping of fishing effort using {_parentForm.GearVariationName} in {_parentForm.LandingSiteName}, {_parentForm.TargetAreaName}";
                    break;

                case "sampling":
                    labelTitle.Text = $"Mapping of fishing effort on {_parentForm.SamplingMonth} using {_parentForm.GearVariationName} in {_parentForm.LandingSiteName}, {_parentForm.TargetAreaName}";
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
            _mapperForm = global.MappingForm;
            _mapperForm.MapperClosed += OnMapperClosed;
        }

        private void OnMapperClosed(object sender, EventArgs e)
        {
            Close();
        }
    }
}