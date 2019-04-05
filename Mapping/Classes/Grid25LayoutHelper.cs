using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxMapWinGIS;
using MapWinGIS;
using System.IO;

namespace FAD3.Mapping.Classes
{
    /// <summary>
    /// Helper class for creating  a template shapefile of grids that define the boundary of a grid25 map.
    /// The template shapefile will be used in creating multiple grid25 maps in one go
    /// </summary>
    public class Grid25LayoutHelper : IDisposable
    {
        private AxMap _axMap;
        private Grid25MajorGrid _majorGrid;
        private Shapefile _sfLayout;                    //the template shapefile
        private int _rows;                              //number of rows in the template
        private int _columns;                           //number of columns in the template
        private int _overLap;                           //number of minor grids of overlap of grid map
        private Extents _layoutExtents;
        private Extents _selectedMajorGridShapesExtent;
        private bool _disposed = false;
        private int _hsfLayout;                         //integer handle of the layout shapefile
        private int _hCursorDefineLayout;

        public delegate void LayoutCreatededEvent(Grid25LayoutHelper s, Grid25LayoutHelperEventArgs e);
        public event LayoutCreatededEvent LayerCreated;

        public delegate void LayoutClearedEvent(Grid25LayoutHelper s, Grid25LayoutHelperEventArgs e);
        public event LayoutCreatededEvent LayoutCleared;

        public int SelectionTransparency { get; set; }

        public int MaxPanelWidth { get; internal set; }
        public int MaxPanelHeight { get; internal set; }

        public void SetDefineLayoutCursor(int cursorHandle)
        {
            _hCursorDefineLayout = cursorHandle;
        }

        public int Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }

        public int Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public int Overlap
        {
            get { return _overLap; }
            set { _overLap = value; }
        }

        public int LayerHandle
        {
            get { return _hsfLayout; }
        }

        public string FishingGround { get; set; }
        public string GridFromLayoutSaveFolder { get; set; }

        public void SelectedMajorGridList(List<int> selectedGridList)
        {
            foreach (int item in selectedGridList)
            {
                _majorGrid.Grid25Grid.ShapeSelected[item] = true;
            }
            _selectedMajorGridShapesExtent = _majorGrid.Grid25Grid.BufferByDistance(0, 0, true, true).Extents;
            _majorGrid.Grid25Grid.SelectNone();
        }

        public Extents SelectedMajorGridExtents
        {
            get { return _selectedMajorGridShapesExtent; }
        }

        public Shapefile LayoutShapeFile
        {
            get { return _sfLayout; }
        }

        public Grid25MajorGrid MajorGrid
        {
            get { return _majorGrid; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// closes and makes null layout shapefile and sets layout handle to -1
        /// </summary>
        public void ClearLayout()
        {
            if (_sfLayout != null)
            {
                _sfLayout.Close();
                _sfLayout = null;

                if (_hsfLayout > 0)
                {
                    _majorGrid.MapLayers.RemoveLayer(_hsfLayout);
                }
                _hsfLayout = -1;
            }
        }

        /// <summary>
        /// Is true if layout shapefile is created from a file and not created from scratch
        /// </summary>
        public bool LayoutTemplateFromFile { get; internal set; }

        //public void SaveTemplateToFile()
        //{
        //    if (ValidLayoutTemplateShapefile())
        //    {
        //        var sf = (Shapefile)_mapLayersHandler.CurrentMapLayer.LayerObject;
        //        var saveAs = new SaveFileDialog();
        //        saveAs.Filter = "Shapefile *.shp|*.shp|All files *.*|*.*";
        //        saveAs.FilterIndex = 1;
        //        saveAs.ShowDialog();
        //        if (File.Exists(saveAs.FileName))
        //        {
        //            ShapefileDiskStorageHelper.Delete(saveAs.FileName.Replace(".shp", ""));
        //        }
        //        if (saveAs.FileName.Length > 0 && sf.SaveAs(saveAs.FileName))
        //        {
        //            var prjFile = sf.Filename.Replace(".shp", ".prj");
        //            sf.GeoProjection.WriteToFile(prjFile);

        //            if (_mapLayersHandler.CurrentMapLayer.IsFishingGridLayoutTemplate)
        //            {
        //                var layoutFile = sf.Filename.Replace(".shp", ".lay");
        //                if (File.Exists(layoutFile))
        //                {
        //                    try
        //                    {
        //                        File.Delete(layoutFile);
        //                    }
        //                    catch (IOException ioex)
        //                    {
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                    }
        //                }
        //                using (StreamWriter writer = new StreamWriter(layoutFile, true))
        //                {
        //                    writer.WriteLine($"Fishing ground:{ global.MappingForm.Grid25MajorGrid.LayoutHelper.FishingGround}");
        //                    //writer.WriteLine($"Save folder:{global.MappingForm.Grid25MajorGrid.LayoutHelper.GridFromLayoutSaveFolder}");
        //                    writer.WriteLine($"Rows:{global.MappingForm.Grid25MajorGrid.LayoutHelper.Rows}");
        //                    writer.WriteLine($"Columns:{global.MappingForm.Grid25MajorGrid.LayoutHelper.Columns}");
        //                    writer.WriteLine($"Overlap:{global.MappingForm.Grid25MajorGrid.LayoutHelper.Overlap}");
        //                    string selectedMajorGrids = "SelectedMajorGrids:";
        //                    foreach (int item in global.MappingForm.Grid25MajorGrid.SelectedShapeGridNumbers)
        //                    {
        //                        selectedMajorGrids += $"{item.ToString()},";
        //                    }
        //                    selectedMajorGrids = selectedMajorGrids.Trim(',');
        //                    writer.WriteLine(selectedMajorGrids);
        //                }
        //            }
        //        }
        //    }
        //}
        public bool SaveLayoutTemplate(string fileName)
        {
            bool success = true;
            if (ValidLayoutTemplateShapefile())
            {
                if (File.Exists(fileName))
                {
                    ShapefileDiskStorageHelper.Delete(fileName.Replace(".shp", ""));
                }

                if (_sfLayout.SaveAs(fileName))
                {
                    var prjFile = _sfLayout.Filename.Replace(".shp", ".prj");
                    _sfLayout.GeoProjection.WriteToFile(prjFile);

                    var layoutFile = _sfLayout.Filename.Replace(".shp", ".lay");
                    if (File.Exists(layoutFile))
                    {
                        try
                        {
                            File.Delete(layoutFile);
                        }
                        catch (IOException ioex)
                        {
                            success = false;
                        }
                        catch (Exception ex)
                        {
                            success = false;
                        }
                    }
                    using (StreamWriter writer = new StreamWriter(layoutFile, true))
                    {
                        writer.WriteLine($"Fishing ground:{ global.MappingForm.Grid25MajorGrid.LayoutHelper.FishingGround}");
                        //writer.WriteLine($"Save folder:{global.MappingForm.Grid25MajorGrid.LayoutHelper.GridFromLayoutSaveFolder}");
                        writer.WriteLine($"Rows:{global.MappingForm.Grid25MajorGrid.LayoutHelper.Rows}");
                        writer.WriteLine($"Columns:{global.MappingForm.Grid25MajorGrid.LayoutHelper.Columns}");
                        writer.WriteLine($"Overlap:{global.MappingForm.Grid25MajorGrid.LayoutHelper.Overlap}");
                        string selectedMajorGrids = "SelectedMajorGrids:";
                        foreach (int item in global.MappingForm.Grid25MajorGrid.SelectedShapeGridNumbers)
                        {
                            selectedMajorGrids += $"{item.ToString()},";
                        }
                        selectedMajorGrids = selectedMajorGrids.Trim(',');
                        writer.WriteLine(selectedMajorGrids);
                    }
                }
            }
            else
            {
                success = false;
            }
            return success;
        }

        /// <summary>
        /// returns true if all title fields in the grid25 layout template shapefile is filled up
        /// </summary>
        /// <returns></returns>
        public bool ValidLayoutTemplateShapefile()
        {
            bool isValid = true;
            {
                int fldTitle = _sfLayout.FieldIndexByName["Title"];
                for (int n = 0; n < _sfLayout.NumShapes; n++)
                {
                    if (_sfLayout.CellValue[fldTitle, n].ToString().Length == 0)
                    {
                        isValid = false;
                        break;
                    }
                }

                return isValid;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _majorGrid = null;
                }
                if (_sfLayout != null)
                {
                    _sfLayout.Close();
                    _sfLayout = null;
                }
                if (_layoutExtents != null)
                {
                    _layoutExtents = null;
                }
                if (_selectedMajorGridShapesExtent != null)
                {
                    _selectedMajorGridShapesExtent = null;
                }

                _axMap.SelectBoxFinal -= OnMapSelectBoxFinal;
                _axMap = null;
                _disposed = true;
                _hsfLayout = -1;
            }
        }

        public void LayoutDetails(int rows, int columns, int overlap)
        {
            _rows = rows;
            _columns = columns;
            _overLap = overlap;
        }

        public Grid25LayoutHelper(Grid25MajorGrid majorGrid)
        {
            LayoutTemplateFromFile = false;
            SetupClass(majorGrid);
        }

        public Grid25LayoutHelper(Grid25MajorGrid majorGrid, int iconHandle)
        {
            LayoutTemplateFromFile = false;
            _hCursorDefineLayout = iconHandle;
            SetupClass(majorGrid);
        }

        private void SetupClass(Grid25MajorGrid majorGrid)
        {
            _majorGrid = majorGrid;
            _axMap = _majorGrid.MapControl;
            _axMap.SelectBoxFinal += OnMapSelectBoxFinal;
            _axMap.SendSelectBoxFinal = true;
            if (_majorGrid.SelectedShapeGridNumbers.Count > 0)
            {
                for (int n = 0; n < _majorGrid.SelectedShapeGridNumbers.Count; n++)
                {
                    _majorGrid.Grid25Grid.ShapeSelected[_majorGrid.SelectedShapeGridNumbers[n]] = true;
                }
                _selectedMajorGridShapesExtent = _majorGrid.Grid25Grid.BufferByDistance(0, 0, true, true).Extents;
                _majorGrid.Grid25Grid.SelectNone();
            }
            SelectionTransparency = 75;
        }

        /// <summary>
        ///  define the extent of the template shapedfile and pass the extent to the function
        ///  that will create the individual tiles (polygon shapes) comprising the template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapSelectBoxFinal(object sender, _DMapEvents_SelectBoxFinalEvent e)
        {
            if (_axMap.CursorMode == tkCursorMode.cmSelection && _axMap.UDCursorHandle == _hCursorDefineLayout)
            {
                var extL = 0D;
                var extR = 0D;
                var extT = 0D;
                var extB = 0D;

                _axMap.PixelToProj(e.left, e.top, ref extL, ref extT);
                _axMap.PixelToProj(e.right, e.bottom, ref extR, ref extB);

                _layoutExtents = new Extents();
                _layoutExtents.SetBounds(
                    (int)(extL / 2000) * 2000,
                    (int)(extB / 2000) * 2000,
                    0,
                    (int)(extR / 2000) * 2000,
                    (int)(extT / 2000) * 2000,
                    0);

                //modify layout extents using the function called
                _layoutExtents = IntersectSelectionBoxWithMajorGridSelection();

                if (_layoutExtents != null && LayerCreated != null)
                {
                    Grid25LayoutHelperEventArgs lhe = new Grid25LayoutHelperEventArgs(_layoutExtents);
                    LayerCreated(this, lhe);
                    SetupLayout(lhe.Rows, lhe.Columns, lhe.Overlap);
                }
                else if (_layoutExtents == null)
                {
                    if (LayerCreated != null)
                    {
                        Grid25LayoutHelperEventArgs gle = new Grid25LayoutHelperEventArgs(nullLayout: true);
                        LayerCreated(this, gle);
                    }
                }
                LayoutTemplateFromFile = false;
            }
        }

        /// <summary>
        /// opens an existing template shapefile by looking for the .lay extension
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool OpenLayoutFile(string fileName)
        {
            bool success = false;
            _sfLayout = new Shapefile();
            if (_sfLayout.Open(fileName.Replace("lay", "shp")))
            {
                _hsfLayout = _majorGrid.MapLayers.AddLayer(_sfLayout, "Layout frame", true, true);
                if (_hsfLayout > 0)
                {
                    MaxPanelHeight = 0;
                    MaxPanelWidth = 0;
                    for (int n = 0; n < _sfLayout.NumShapes; n++)
                    {
                        var ext = _sfLayout.Shape[n].Extents;
                        if (ext.Width > MaxPanelWidth)
                        {
                            MaxPanelWidth = (int)ext.Width;
                        }

                        if (ext.Height > MaxPanelHeight)
                        {
                            MaxPanelHeight = (int)ext.Height;
                        }
                    }
                    _sfLayout.DefaultDrawingOptions.FillVisible = false;
                    _sfLayout.DefaultDrawingOptions.LineWidth = 2;
                    _sfLayout.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.DarkBlue);
                    _majorGrid.MapLayers[_hsfLayout].IsFishingGridLayoutTemplate = true;
                    _axMap.Redraw();
                    success = true;
                    LayoutTemplateFromFile = true;
                    _layoutExtents = _sfLayout.Extents;
                    //_selectedMajorGridShapesExtent = _sfLayout.Extents;
                }
            }
            return success;
        }

        /// <summary>
        /// returns the extent of the template using a contain or intersect function
        /// _layoutExtents is actually the selection box extent
        /// </summary>
        /// <returns></returns>
        private Extents IntersectSelectionBoxWithMajorGridSelection()
        {
            if (_layoutExtents.ToShape().Contains(_selectedMajorGridShapesExtent.ToShape()))
            {
                // extent is the extent of the selected major grids
                return _selectedMajorGridShapesExtent;
            }
            else if (_selectedMajorGridShapesExtent.ToShape().Contains(_layoutExtents.ToShape()))
            {
                //extent is the selection box extent
                return _layoutExtents;
            }
            else if (_layoutExtents.ToShape().Intersects(_selectedMajorGridShapesExtent.ToShape()))
            {
                //extent is the intersection between selected major grids and selection box
                var results = new object();
                if (_layoutExtents.ToShape().GetIntersection(_selectedMajorGridShapesExtent.ToShape(), ref results))
                {
                    //convets results object to an array of shapes that is a product of the intersection
                    object[] shapeArray = results as object[];
                    if (shapeArray != null)
                    {
                        Shape[] shapes = shapeArray.OfType<Shape>().ToArray();

                        //minor grid extent is the intersection of the selected major grids and the selection box
                        return shapes[0].Extents;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// inspects layout shapefile if all Title fields have values
        /// </summary>
        /// <returns></returns>
        public bool HasCompletePanelTitles()
        {
            if (_sfLayout.NumShapes == 0)
            {
                return false;
            }
            else
            {
                int fldTitle = _sfLayout.FieldIndexByName["Title"];
                for (int n = 0; n < _sfLayout.NumShapes; n++)
                {
                    string title = _sfLayout.CellValue[fldTitle, n].ToString();
                    if (title.Length == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// constructs the shapefile consisting of tiles (Polygons) where each individual tile's extents
        /// will be used to define a grid25 map
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="overlap"></param>
        /// <returns></returns>
        public bool SetupLayout(int rows, int columns, int overlap)
        {
            if (_layoutExtents != null
                && rows > 0
                && columns > 0)
            {
                _overLap = overlap;
                _rows = rows;
                _columns = columns;

                int expandLeft = 0;
                int expandRight = 0;
                int expandTop = 0;
                int expandBottom = 0;

                //if shapefile already exists (handle >0) then we close the file
                if (_hsfLayout > 0)
                {
                    _majorGrid.MapLayers.RemoveLayer(_hsfLayout);
                    _sfLayout.Close();
                }

                int panelHeight = ((int)(_layoutExtents.yMax - _layoutExtents.yMin)) / rows;
                int panelWidth = ((int)(_layoutExtents.xMax - _layoutExtents.xMin)) / columns;

                Point origin = new Point
                {
                    x = _layoutExtents.xMin,
                    y = _layoutExtents.yMin
                };

                _sfLayout = new Shapefile();
                _sfLayout.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON);
                _sfLayout.GeoProjection = _axMap.GeoProjection;
                int fldPanelNo = _sfLayout.EditAddField("PanelNumber", FieldType.INTEGER_FIELD, 0, 2);
                int fldTitle = _sfLayout.EditAddField("Title", FieldType.STRING_FIELD, 0, 50);

                //set the shapefile's appearance
                _sfLayout.DefaultDrawingOptions.FillVisible = false;
                _sfLayout.DefaultDrawingOptions.LineWidth = 2;
                _sfLayout.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.DarkBlue);
                _sfLayout.SelectionDrawingOptions.FillTransparency = 255 * ((float)SelectionTransparency / 100);

                if (rows == 1 && columns == 1)
                {
                    _sfLayout.EditAddShape(_layoutExtents.ToShape());
                    _sfLayout.EditCellValue(fldPanelNo, 0, 1);
                    MaxPanelWidth = (int)_layoutExtents.Width;
                    MaxPanelHeight = (int)_layoutExtents.Height;
                }
                else
                {
                    MaxPanelHeight = 0;
                    MaxPanelWidth = 0;
                    int x = 0;
                    int y = 0;
                    int iShp = 0;
                    for (int r = 1; r <= _rows; r++)
                    {
                        //when expand is 0, no overlap
                        //when expand is 1 an overlap is added based on the value of _overlap
                        for (int c = 1; c <= _columns; c++)
                        {
                            expandTop = 0;
                            if (r < rows)
                            {
                                expandTop = 1;
                            }

                            expandBottom = 0;
                            if (r > 1)
                            {
                                expandBottom = 1;
                            }

                            expandLeft = 0;
                            if (c > 1)
                            {
                                expandLeft = 1;
                            }

                            expandRight = 0;
                            if (c < columns)
                            {
                                expandRight = 1;
                            }

                            Extents panelExtent = new Extents();
                            if (panelExtent.Height > MaxPanelHeight)
                            {
                                MaxPanelHeight = (int)panelExtent.Height;
                            }
                            if (panelExtent.Width > MaxPanelWidth)
                            {
                                MaxPanelWidth = (int)panelExtent.Width;
                            }

                            //each cell in the grid is computed based on the polygon's lower left and upper right coordinates
                            //an overlap is added if expand_Side is 1, not added if it is 0
                            panelExtent.SetBounds(
                                origin.x + ((c - 1) * panelWidth) - (expandLeft * _overLap * 2000),
                                origin.y + ((r - 1) * panelHeight) - (expandBottom * _overLap * 2000),
                                0,
                                origin.x + (c * panelWidth) + (expandRight * (_overLap) * 2000),
                                origin.y + (r * panelHeight) + (expandTop * (_overLap) * 2000),
                                0
                                );
                            iShp = _sfLayout.EditAddShape(panelExtent.ToShape());
                            _sfLayout.EditCellValue(fldPanelNo, iShp, iShp + 1);
                        }
                    }
                }
                _hsfLayout = _majorGrid.MapLayers.AddLayer(_sfLayout, "Layout frame", true, true);
                if (_hsfLayout > 0 && _sfLayout.NumShapes > 0)
                {
                    _majorGrid.MapLayers[_hsfLayout].IsFishingGridLayoutTemplate = true;
                    _majorGrid.MapLayers.ClearAllSelections();
                    _axMap.Redraw();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Extents LayoutExtents
        {
            get { return _layoutExtents; }
        }
    }
}