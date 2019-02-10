using FAD3.Database.Classes;
using FAD3.Mapping.Classes;
using Microsoft.VisualBasic.PowerPacks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FAD3.Mapping.Forms;
using MapWinGIS;

namespace FAD3
{
    public partial class Grid25GenerateForm : Form
    {
        private static Grid25GenerateForm _instance;
        private bool _formCloseDone = false;
        private MapperForm _parentForm;
        private Grid25MajorGrid _grid25MajorGrid;
        private Dictionary<string, uint> _labelAndGridProperties = new Dictionary<string, uint>();
        private List<LayerEventArg> _savedGridLayers = new List<LayerEventArg>();
        private fadUTMZone _utmZone;
        private double _dragWidth;
        private double _dragHeight;
        private bool _gridFromFileLoaded;

        public void set_UTMZone(fadUTMZone utmZone)
        {
            _utmZone = utmZone;
        }

        public Grid25MajorGrid Grid25MajorGrid
        {
            get { return _grid25MajorGrid; }
        }

        public static Grid25GenerateForm GetInstance(MapperForm parent)
        {
            if (_instance == null) _instance = new Grid25GenerateForm(parent);
            return _instance;
        }

        public Grid25GenerateForm(MapperForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
            _grid25MajorGrid = parent.Grid25MajorGrid;
            _grid25MajorGrid.LayerSaved += OnGrid25LayerSaved;
            _grid25MajorGrid.GridRetrieved += OnGrid25GridRetrieved;
            _grid25MajorGrid.ExtentCreatedInLayer += OnExtentCreated;
            _grid25MajorGrid.OnGridInPanelCreated += OnGridInPanelCreated;
            //_grid25MajorGrid.LayoutDefined += OnGridLayoutDefined;
        }

        private void OnGridInPanelCreated(Grid25MajorGrid s, LayerEventArg e)
        {
            txtMapTitle.Text = e.LayerName;
        }

        //private void OnGridLayoutDefined(Grid25MajorGrid s, ExtentDraggedBoxEventArgs e)
        //{
        //    using (Grid25LayoutHelperForm mglf = new Grid25LayoutHelperForm())
        //    {
        //        var ext = new MapWinGIS.Extents();
        //        ext.SetBounds(e.Left, e.Bottom, 0, e.Right, e.Top, 0);
        //        mglf.SelectionBoxExtent = ext;
        //        mglf.MajorGridSelectionExtent = _grid25MajorGrid.SelectedMajorGridShapesExtent;
        //        mglf.ShowDialog(this);
        //        if (mglf.DialogResult == DialogResult.OK)
        //        {
        //        }
        //    }
        //}

        private void OnExtentCreated(Grid25MajorGrid s, ExtentDraggedBoxEventArgs e)
        {
            GridMapStatusUpdate(e.Top, e.Left, e.Right, e.Bottom, e.InDrag);
        }

        private void OnGrid25GridRetrieved(Grid25MajorGrid s, LayerEventArg e)
        {
            txtMapTitle.Text = e.MapTitle;
        }

        private void OnGrid25LayerSaved(Grid25MajorGrid s, LayerEventArg e)
        {
            _savedGridLayers.Add(e);
        }

        private void DeleteGrid25File(string fileName)
        {
            if (File.Exists($"{fileName}.shp")) File.Delete($"{fileName}.shp");
            if (File.Exists($"{fileName}.prj")) File.Delete($"{fileName}.prj");
            if (File.Exists($"{fileName}.dbf")) File.Delete($"{fileName}.dbf");
            if (File.Exists($"{fileName}.shx")) File.Delete($"{fileName}.shx");
        }

        private uint ColorToUInt(Color clr)
        {
            return (uint)(clr.R + (clr.G << 8) + (clr.B << 16));
        }

        private uint FactorBy100(string property)
        {
            return (uint)(double.Parse(property) * 100);
        }

        /// <summary>
        /// fills a dictionary with label and grid line properties
        /// </summary>
        public void SetupDictionary()
        {
            _labelAndGridProperties.Clear();
            _labelAndGridProperties.Add("minorGridLabelDistance", uint.Parse(txtMinorGridLabelDistance.Text));
            _labelAndGridProperties.Add("minorGridLabelSize", uint.Parse(txtMinorGridLabelSize.Text));
            _labelAndGridProperties.Add("majorGridLabelSize", uint.Parse(txtMajorGridLabelSize.Text));
            _labelAndGridProperties.Add("borderThickness", FactorBy100(txtBorderThickness.Text));
            _labelAndGridProperties.Add("majorGridThickness", FactorBy100(txtMajorGridThickness.Text));
            _labelAndGridProperties.Add("minorGridThickness", FactorBy100(txtMinorGridThickness.Text));
            _labelAndGridProperties.Add("minorGridLabelColor", ColorToUInt(shapeMinorGridLabelColor.FillColor));
            _labelAndGridProperties.Add("majorGridLabelColor", ColorToUInt(shapeMajorGridLabelColor.FillColor));
            _labelAndGridProperties.Add("borderColor", ColorToUInt(shapeBorderColor.FillColor));
            _labelAndGridProperties.Add("majorGridLineColor", ColorToUInt(shapeMajorGridLineColor.FillColor));
            _labelAndGridProperties.Add("minorGridLineColor", ColorToUInt(shapeMinorGridLineColor.FillColor));
            _labelAndGridProperties.Add("minorGridLabelFontBold", (uint)(chkBold.Checked ? 1 : 0));
            _labelAndGridProperties.Add("minorGridLabelWrapped", (uint)(chkWrapLabels.Checked ? 1 : 0));
        }

        public void GridMapStatusUpdate(double top, double left, double right, double bottom,
                     bool inDrag, bool fromFile = false)
        {
            _dragWidth = Math.Abs(left - right);
            _dragHeight = Math.Abs(top - bottom);
            if (inDrag)
            {
                lblGridStatus.Text = "Defining map dimensions:\r\n" +
                 "Height: " + (_dragHeight / 1000).ToString("N1") + " km\r\n" +
                 "Width:" + (_dragWidth / 1000).ToString("N1") + " km";
            }
            else
            {
                if (fromFile)
                {
                    _gridFromFileLoaded = true;
                    lblGridStatus.Text = "Grid map dimension:";
                }
                else
                {
                    _gridFromFileLoaded = false;
                    lblGridStatus.Text = "Final map dimension:";
                }
                lblGridStatus.Text += "\r\nHeight: " + (_dragHeight / 1000).ToString("N1") + " km" +
                     "\r\nWidth: " + (_dragWidth / 1000).ToString("N1") + " km";
            }
            lblGridStatus.Text += "\r\nRows x Columns : " + ((int)((_dragHeight / 1000) / 2)).ToString("") + " x " + ((int)((_dragWidth / 1000) / 2)).ToString();

            if (_gridFromFileLoaded)
            {
                chkWrapLabels.Enabled = false;
                chkWrapLabels.Checked = false;
            }
        }

        private void RedoGridLabel()
        {
            SetupDictionary();
            _grid25MajorGrid.RedoLabels(txtMapTitle.Text, _labelAndGridProperties);
            _grid25MajorGrid.ApplyGridSymbology(txtMapTitle.Text);
        }

        private void OnButtons_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                //updates the grid labels with changes inputted into the properies dictionary
                case "buttonLabel":
                    if (_grid25MajorGrid.grid25LabelManager != null && _grid25MajorGrid.grid25LabelManager.Grid25Labels.NumShapes > 0)
                    {
                        RedoGridLabel();
                    }
                    break;

                //initiates the construction of minor grids and labels. Map cursor is changed to the grid icon to signify
                //that creating minor grids and labels can proceed.
                case "buttonGrid":
                    if (txtMinorGridLabelDistance.Text.Length > 0 && txtMinorGridLabelSize.Text.Length > 0)
                    {
                        SetupDictionary();

                        if (_grid25MajorGrid.DefineMinorGrid((int)((Bitmap)imList.Images["gridCursor"]).GetHicon()))
                        {
                            _grid25MajorGrid.LabelAndGridProperties = _labelAndGridProperties;
                            _grid25MajorGrid.MapTitle = txtMapTitle.Text;
                        }
                        else
                        {
                            MessageBox.Show("No selection in major grid", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Provide minor grid label distance and minor grid label size", "Validation error",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "buttonClear":
                    _grid25MajorGrid.ClearSelectedGrids();
                    //_parentForm.SetCursorToSelect();
                    _parentForm.SetCursor(MapWinGIS.tkCursorMode.cmSelection);
                    break;

                case "buttonClose":
                    Close();
                    break;

                case "buttonLocateGrid":

                    if (_grid25MajorGrid.SelectedShapeGridNumbers.Count > 0)
                    {
                        if (txtMinorGridLabelDistance.Text.Length > 0 && txtMinorGridLabelSize.Text.Length > 0)
                        {
                            SetupDictionary();
                            _grid25MajorGrid.LabelAndGridProperties = _labelAndGridProperties;
                            _grid25MajorGrid.DefineGridLayout((int)((Bitmap)imList.Images["gridLayout"]).GetHicon());
                            Grid25LayoutHelperForm g25lhf = Grid25LayoutHelperForm.GetInstance(_grid25MajorGrid);

                            if (g25lhf.Visible)
                            {
                                g25lhf.BringToFront();
                            }
                            else
                            {
                                // _grid25MajorGrid.DefineGridLayout((int)((Bitmap)imList.Images["gridLayout"]).GetHicon());
                                g25lhf.Show(this);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Provide minor grid label distance and minor grid label size", "Validation error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No selection in major grid", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
            }
        }

        private void ReviewSavedGridFiles()
        {
            if (_savedGridLayers != null)
            {
                bool willDelete = false;
                foreach (var item in _savedGridLayers)
                {
                    if (!item.LayerSaved)
                    {
                        willDelete = true;
                        break;
                    }
                }

                if (willDelete)
                {
                    foreach (var item in _savedGridLayers)
                    {
                        DeleteGrid25File(item.FileName);
                    }
                }
            }
        }

        private void Grid25GenerateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_formCloseDone)
            {
                _formCloseDone = true;
                if (global.MappingForm != null) global.MappingForm.Close();
            }

            CleanUp();

            global.SaveFormSettings(this);
            global.MappingMode = fad3MappingMode.defaultMode;
            ReviewSavedGridFiles();
            global.Grid25GenerateForm = null;
        }

        private void CleanUp()
        {
            _parentForm = null;
            _grid25MajorGrid = null;
            if (_labelAndGridProperties != null)
            {
                _labelAndGridProperties.Clear();
                _labelAndGridProperties = null;
            }
            _savedGridLayers = null;
            _instance = null;
        }

        public void CreateInlandGridDB()
        {
            InlandGridCreateDBForm igcf = InlandGridCreateDBForm.GetInstance(this);
            igcf.UTMZone = _utmZone;
            if (!igcf.Visible)
            {
                igcf.Show(this);
            }
            else
            {
                igcf.BringToFront();
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            txtMinorGridLabelDistance.Text = "1000";
            txtMinorGridLabelSize.Text = "8";
            txtMajorGridLabelSize.Text = "12";
            txtBorderThickness.Text = "2";
            txtMajorGridThickness.Text = "2";
            txtMinorGridThickness.Text = "1";
            chkLeft.Checked = true;
            chkTop.Checked = true;
            chkRight.Checked = true;
            chkBottom.Checked = true;
            global.LoadFormSettings(this, true);
            global.Grid25GenerateForm = this;
        }

        private void OnShapeColor_DoubleClick(object sender, EventArgs e)
        {
            var cd = new ColorDialog
            {
                AllowFullOpen = true,
                AnyColor = true,
                FullOpen = true,
                SolidColorOnly = true,
            };
            cd.ShowDialog();
            ((RectangleShape)sender).FillColor = cd.Color;
        }

        private void OnToolbar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "tsButtonMBRs":
                    break;

                //retrieve fishing grid boundaries and shows them on the map form
                case "tsButtonRetrieve":
                    var folderBrowser = new FolderBrowserDialog();
                    folderBrowser.ShowNewFolderButton = true;
                    folderBrowser.SelectedPath = SaveMapForm.GetSavedMapsFolder();
                    folderBrowser.Description = "Locate folder containing saved fishing ground grid maps";
                    DialogResult result = FolderBrowserLauncher.ShowFolderBrowser(folderBrowser);
                    if (result == DialogResult.OK)
                    {
                        SaveMapForm.SetSavedMapsFolder(folderBrowser.SelectedPath);
                        SetupDictionary();
                        _grid25MajorGrid.ShowGridBoundaries(folderBrowser.SelectedPath, _utmZone, _labelAndGridProperties);
                    }
                    break;

                //Saves fishing grids either as shapefile or image file
                case "tsButtonSaveImage":
                case "tsButtonSaveShapefile":
                    if (txtMapTitle.Text.Length > 0)
                    {
                        RedoGridLabel();
                        var saveForm = new SaveMapForm(this);
                        saveForm.SaveType(e.ClickedItem.Name == "tsButtonSaveShapefile");
                        saveForm.ShowDialog(this);
                    }
                    else
                    {
                        MessageBox.Show("Please provide map title", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "tsButtonFitMap":
                    _grid25MajorGrid.FitGridToMap();
                    break;

                case "tsButtonExit":
                    Close();
                    break;
            }
        }
    }
}