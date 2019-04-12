using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapWinGIS;
using AxMapWinGIS;
using FAD3.Mapping.Classes;
using System.IO;
using System.Xml;

namespace FAD3.Mapping.Classes
{
    /// <summary>
    /// Displays grid25 grid map in geographic coordinate system (not UTM) 
    /// </summary>
    public class Grid25GeographicDisplayHelper
    {
        public string SourceFolder { get; set; }                //folder where grid maps are saved
        public MapLayersHandler MapLayersHandler { get; internal set; }
        private string _selectedLayoutCell;
        private Dictionary<string, string> _grid25Layers = new Dictionary<string, string>();
        private int _hMinorGrid;
        private int _hMajorGrid;
        private int _hGridLabels;
        private int _hMBR;


        public bool HasGrid { get; internal set; }
        private float _majorGridThickness;
        private int _majorGridLabelSize;
        private uint _majorGridLabelColor;
        private uint _majorGridLineColor;
        private float _minorGridThickness;
        private uint _minorGridLabelColor;
        private bool _minorGridLabelFontBold;
        private bool _majorGridLabelFontBold;
        private float _minorGridLabelDistance;
        private uint _minorGridLineColor;
        //private int _minorGridLabelWrapped;
        private int _minorGridLabelSize;
        private int _subGridLineThickness;
        private uint _subGridLineColor;
        private uint _borderColor;
        private float _borderThickness;
        private double _minorGridOffsetDistance;
        private bool _gridStateFinishedReading;
        public bool SubgridsVisible { get; set; }
        public bool MinorGridLableFontBold
        {
            get { return _minorGridLabelFontBold; }
            set { _minorGridLabelFontBold = value; }
        }
        public bool MajorGridLableFontBold
        {
            get { return _majorGridLabelFontBold; }
            set { _majorGridLabelFontBold = value; }
        }
        public double MinorGridOffsetDistance
        {
            get { return _minorGridOffsetDistance; }
            set { _minorGridOffsetDistance = value; }
        }

        public float MinorGridLabelDistance
        { 
            get { return _minorGridLabelDistance; }
            set { _minorGridLabelDistance = value; }
        }

        public int MinorGridFontSize
        {
            get { return _minorGridLabelSize; }
            set { _minorGridLabelSize = value; }
        }
        public void SetMapExtents(Extents extents)
        {
            _mapcontrol.Extents = extents;
        }

        public int MajorGridFontSize
        {
            get { return _majorGridLabelSize; }
            set { _majorGridLabelSize = value; }
        }
        public Shapefile MaxDimensionMBR { get; internal set;}
        public Shapefile MBR { get; internal set; }
        private AxMap _mapcontrol;

        public void LockMap()
        {
            //_mapcontrol.Visible = false;
            _mapcontrol.LockWindow(tkLockMode.lmLock);
        }

        public void UnlockMap()
        {
            _mapcontrol.LockWindow(tkLockMode.lmUnlock);
            //_mapcontrol.Visible = true;
        }

        public void RedrawNap()
        {
            _mapcontrol.Redraw();
        }

        private void ResetGrid()
        {
            _hMajorGrid = -1;
            _hMBR = -1;
            _hMinorGrid = -1;
            _hGridLabels = -1;
        }

        public string SelectedLayoutCell
        {
            get { return _selectedLayoutCell; }
            set
            {
                _selectedLayoutCell = value;
                //RemoveGrid25Layers();
                AddSelectedLayoutGrid(_selectedLayoutCell);
            }
        }
        public void SetMaxDimensionGridName(string folderPath, string gridName)
        {
            var sf = new Shapefile();
            string file = $@"{folderPath}\{gridName}_gridlabels.shp";
            if(File.Exists(file) && sf.Open(file))
            {
                int reprojectedCount = 0;
                MaxDimensionMBR = sf.Reproject(_mapcontrol.GeoProjection, ref reprojectedCount);
            }
        }
        private bool AddSelectedLayoutGrid(string layoutCellName)
        {
            _grid25Layers.Clear();
            _grid25Layers.Add("Minor grid", $"{layoutCellName}_gridlines.shp");
            _grid25Layers.Add("Major grid", $"{layoutCellName}_majorgrid.shp");
            _grid25Layers.Add("Labels", $"{layoutCellName}_gridlabels.shp");
            _grid25Layers.Add("MBR", $"{layoutCellName}_gridboundary.shp");

            
            string gridStateFile = $@"{SourceFolder}\{layoutCellName}_gridstate.xml";
            if ( File.Exists(gridStateFile) &&  !_gridStateFinishedReading)
            {
                XmlTextReader xmlReader = new XmlTextReader(gridStateFile);
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        switch (xmlReader.Name)
                        {
                            case "FishingGroundGridMap":
                                _minorGridLabelDistance = float.Parse(xmlReader.GetAttribute("minorGridLabelDistance")) / (60 * 1852);
                                _minorGridLabelFontBold = xmlReader.GetAttribute("minorGridLabelFontBold") != "0";
                                _minorGridThickness = float.Parse(xmlReader.GetAttribute("minorGridThickness")) / 100;
                                _majorGridThickness = float.Parse(xmlReader.GetAttribute("majorGridThickness")) / 100;
                                _majorGridLineColor = uint.Parse(xmlReader.GetAttribute("majorGridLineColor"));
                                _minorGridLineColor = uint.Parse(xmlReader.GetAttribute("minorGridLineColor"));
                                _minorGridLabelColor = uint.Parse(xmlReader.GetAttribute("minorGridLabelColor"));
                                _borderColor = uint.Parse(xmlReader.GetAttribute("borderColor"));
                                _borderThickness = float.Parse(xmlReader.GetAttribute("borderThickness")) / 100;
                                _majorGridLabelSize = int.Parse(xmlReader.GetAttribute("majorGridLabelSize"));
                                _majorGridLabelColor = uint.Parse(xmlReader.GetAttribute("majorGridLabelColor"));
                                _minorGridLabelSize = int.Parse(xmlReader.GetAttribute("minorGridLabelSize"));
                                _majorGridLabelFontBold = false;
                                break;

                        }
                    }
                }
                _gridStateFinishedReading = true;
            }

            var sf = new Shapefile();
            string file = $@"{SourceFolder}\{_grid25Layers["Minor grid"]}";
            if (File.Exists(file) && sf.Open(file))
            {
                _hMinorGrid = MapLayersHandler.AddLayer(sf, "Minor grid",true,true);



                sf = new Shapefile();
                file = $@"{SourceFolder}\{_grid25Layers["Major grid"]}";
                if (File.Exists(file) && sf.Open(file))
                {
                    _hMajorGrid = MapLayersHandler.AddLayer(sf, "Major grid",true,true);
                }

                sf = new Shapefile();
                file = $@"{SourceFolder}\{_grid25Layers["Labels"]}";
                if (File.Exists(file) && sf.Open(file))
                {
                    _hGridLabels = MapLayersHandler.AddLayer(sf, "Labels",true,true);
                    int reprojectedCount = 0;
                    MBR = sf.Reproject(_mapcontrol.GeoProjection, ref reprojectedCount);
                }

                sf = new Shapefile();
                file = $@"{SourceFolder}\{_grid25Layers["MBR"]}";
                if (File.Exists(file) && sf.Open(file))
                {
                    _hMBR = MapLayersHandler.AddLayer(sf, "MBR",true,true);
                }


                SymbolizeGrid();
                return true;
            }
            else
            {
                return false;
            }

        }

        public void SymbolizeGrid()
        {
            var sf = MapLayersHandler[_hMinorGrid].LayerObject as Shapefile;
            if (!SubgridsVisible)
            {
                sf.VisibilityExpression = @"[LineType]=""MG""";
            }
            else
            {
                sf.VisibilityExpression = "";
            }
            sf.DefaultDrawingOptions.LineWidth = _minorGridThickness;

            sf = MapLayersHandler[_hMajorGrid].LayerObject as Shapefile;
            sf.DefaultDrawingOptions.FillVisible = false;
            sf.DefaultDrawingOptions.LineWidth = _majorGridThickness;
            sf.DefaultDrawingOptions.LineColor = _majorGridLineColor;

            sf = MapLayersHandler[_hMBR].LayerObject as Shapefile;
            sf.DefaultDrawingOptions.FillVisible = false;
            sf.DefaultDrawingOptions.LineWidth = _borderThickness;
            sf.DefaultDrawingOptions.LineColor = _borderColor;

            sf = MapLayersHandler[_hGridLabels].LayerObject as Shapefile;
            sf.DefaultDrawingOptions.PointSize = 0;
            sf.DefaultDrawingOptions.LineVisible = false;
            sf.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.White);
            sf.Labels.Alignment = tkLabelAlignment.laCenter;
            sf.Labels.FrameVisible = false;
            sf.Labels.AutoOffset = false;
            sf.Labels.AvoidCollisions = false;
            sf.Labels.Visible = true;
            sf.Labels.FontBold = _minorGridLabelFontBold;
            //sf.Labels.Expression = "[Label]";
            sf.Labels.Generate("[Label]",tkLabelPositioning.lpCentroid,false);


            var lc= sf.Labels.AddCategory("majorGrid");
            lc.Expression = @"[Location]=""MG""";
            lc.FontSize = _majorGridLabelSize;
            lc.FontBold = _majorGridLabelFontBold;
            lc.FontColor = _majorGridLabelColor;

            lc = sf.Labels.AddCategory("title");
            lc.Expression = @"[Location]=""MT""";
            lc.FontSize = 15;
            lc.FontBold = true;
            lc.Alignment = tkLabelAlignment.laCenterRight;
            lc.FontColor = new Utils().ColorByName(tkMapColor.Black);

            lc = sf.Labels.AddCategory("zone");
            lc.Expression = @"[Location]=""MZ""";
            lc.FontSize = 12;
            lc.Alignment = tkLabelAlignment.laCenterRight;
            lc.FontColor = new Utils().ColorByName(tkMapColor.Black);

            lc = sf.Labels.AddCategory("left");
            lc.Expression = @"[Location]=""L""";
            lc.FontSize = _minorGridLabelSize;
            lc.OffsetX = _minorGridOffsetDistance *-1;
            lc.FontColor = new Utils().ColorByName(tkMapColor.Black);

            lc = sf.Labels.AddCategory("right");
            lc.Expression = @"[Location]=""R""";
            lc.FontSize = _minorGridLabelSize;
            lc.OffsetX = _minorGridOffsetDistance;
            lc.FontColor = new Utils().ColorByName(tkMapColor.Black);

            lc = sf.Labels.AddCategory("top");
            lc.Expression = @"[Location]=""T""";
            lc.FontSize = _minorGridLabelSize;
            lc.OffsetY = _minorGridOffsetDistance *-1;
            lc.FontColor = new Utils().ColorByName(tkMapColor.Black);

            lc = sf.Labels.AddCategory("bottom");
            lc.Expression = @"[Location]=""B""";
            lc.FontSize = _minorGridLabelSize;
            lc.FontColor = new Utils().ColorByName(tkMapColor.Black);
            lc.OffsetY = _minorGridOffsetDistance;

            sf.Labels.ApplyCategories();
            HasGrid = true;
        }


        public bool RemoveGrid25Layers()
        {
            MapLayersHandler.RemoveLayer("Minor grids");
            MapLayersHandler.RemoveLayer("Major grids");
            MapLayersHandler.RemoveLayer("Labels");
            MapLayersHandler.RemoveLayer("MBR");
            ResetGrid();
            HasGrid = false;
            return true;
        }


        public Grid25GeographicDisplayHelper( MapLayersHandler mapLayersHandler, AxMap mapControl)
        {
            _mapcontrol = mapControl;
            MapLayersHandler = mapLayersHandler;
            _minorGridThickness = 1;
            _subGridLineThickness = 1;
            _majorGridLabelColor = new Utils().ColorByName(tkMapColor.Red);
            _majorGridLineColor = new Utils().ColorByName(tkMapColor.Red);
            _minorGridLabelColor  = new Utils().ColorByName(tkMapColor.Black);
            _subGridLineColor = new Utils().ColorByName(tkMapColor.Gray);
            _borderColor = new Utils().ColorByName(tkMapColor.Black);
            HasGrid = false;
            _minorGridOffsetDistance = 0;
        }
    }
}
