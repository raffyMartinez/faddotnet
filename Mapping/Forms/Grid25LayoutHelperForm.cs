using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapWinGIS;
using FAD3.Mapping.Classes;
using Microsoft.Win32;
using System.IO;

namespace FAD3.Mapping.Forms
{
    /// <summary>
    /// Creates multiple fishing ground grid maps
    /// </summary>
    public partial class Grid25LayoutHelperForm : Form
    {

        public int PageWidth { get; internal set; }
        public int PageHeight { get; internal set; }
        public int Overlap { get; internal set; }
        public int Rows { get; internal set; }
        public int Columns { get; internal set; }
        public Extents SelectionBoxExtent { get; set; }
        public Extents MajorGridSelectionExtent { get; set; }
        public double ExtentTop { get; set; }
        public double ExtentBottom { get; set; }
        public double ExtentLeft { get; set; }
        public double ExtentRight { get; set; }
        public Grid25LayoutHelper LayoutHelper { get; set; }
        private static Grid25LayoutHelperForm _instance;
        private Grid25MajorGrid _majorGrid;
        private string _parentFolder = string.Empty;
        private string _savePath;
        private string _fishingGround;
        private int _mouseX;
        private int _mouseY;
        private Grid25GenerateForm _parentForm;
        private bool _hasSubGrid;
        private int _subGridCount;
        private string _layoutFileName;
        private Shapefile _layoutShapefile;
        private string _gridMapSourceFolderPath;
        private string _exportImageFolderSavePath;
        private Grid25GeographicDisplayHelper _grid25GeographicDisplayHelper;
        public const int XBuffer = 1500;
        public const int YTopBuffer = 4000;
        public const int YBottomBuffer = 3000;
        private int _maxDimensionIndex=-1;
        private int _exportedImageCount;
        private int _exportDPI;
        private string _layoutPanelTitle;
        private Dictionary<int, (string layerName, bool showFront, bool showFrontLabel, bool showReverse, bool showReverseLabel)> _exportSettingsDict = new Dictionary<int, (string layerName, bool showFront, bool showFrontLabel, bool showReverse, bool showReverseLabel)>();

        public static Grid25LayoutHelperForm GetInstance(Grid25MajorGrid majorGrid, Grid25GenerateForm parentForm)
        {
            if (_instance == null) _instance = new Grid25LayoutHelperForm(majorGrid, parentForm);
            return _instance;
        }

        public static Grid25LayoutHelperForm GetInstance(string layoutFileName)
        {
            if (_instance == null) _instance = new Grid25LayoutHelperForm(layoutFileName);
            return _instance;
        }

        public Grid25LayoutHelperForm(Grid25MajorGrid majorGrid, Grid25GenerateForm parentForm)
        {
            InitializeComponent();
            _majorGrid = majorGrid;
            _parentForm = parentForm;
            LayoutHelper = _majorGrid.LayoutHelper;
            LayoutHelper.LayerCreated += OnLayoutCreated;
        }

        private void ReadLayoutData()
        {
            string line;
            List<int> selectedGridHandles = new List<int>();
            StreamReader file = new StreamReader(_layoutFileName);

            while ((line = file.ReadLine()) != null)
            {
                string[] line2 = line.Split(':');
                switch (line2[0])
                {
                    case "Fishing ground":
                        textFishingGround.Text = line2[1];
                        break;

                    case "Rows":
                        txtRows.Text = line2[1];
                        break;

                    case "Columns":
                        txtColumns.Text = line2[1];
                        break;

                    case "Overlap":
                        txtOverlap.Text = line2[1];
                        break;
                }
            }
        }

        public Grid25LayoutHelperForm(string layoutFileName)
        {
            InitializeComponent();
            _layoutFileName = layoutFileName;
        }

        private void OnLayoutCreated(Grid25LayoutHelper s, Grid25LayoutHelperEventArgs e)
        {
            if (e.NullLayout)
            {
                MessageBox.Show("Selection is outside selected major grid", "Selection error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                e.LayoutSpecs(int.Parse(txtRows.Text), int.Parse(txtColumns.Text), int.Parse(txtOverlap.Text));
            }
        }

        public bool AutoExpandSelectedPanel
        {
            get { return chkAutoExpand.Checked; }
        }



        /// <summary>
        ///populate the various text fields of the form
        ///and populate the list of grid maps that are
        ///defined by the layout template
        /// </summary>
        public void SetUpFields()
        {
            _fishingGround = LayoutHelper.FishingGround;
            textFishingGround.Text = _fishingGround;
            _savePath = LayoutHelper.GridFromLayoutSaveFolder;
            //textFolderToSave.Text = _savePath;
            txtRows.Text = LayoutHelper.Rows.ToString();
            txtColumns.Text = LayoutHelper.Columns.ToString();
            txtOverlap.Text = LayoutHelper.Overlap.ToString();

            SetupResultsView();

            //populate the list of grid maps
            int panelCount = LayoutHelper.Columns * LayoutHelper.Rows;
            for (int n = 0; n < panelCount; n++)
            {
                int fldTitle = LayoutHelper.LayoutShapeFile.FieldIndexByName["Title"];
                string panelTitle = LayoutHelper.LayoutShapeFile.CellValue[fldTitle, n].ToString();
                Extents ext = LayoutHelper.LayoutShapeFile.Shape[n].Extents;

                ListViewItem lvi = lvResults.Items.Add(panelTitle);
                lvi.SubItems.Add(ext.Width.ToString());
                lvi.SubItems.Add(ext.Height.ToString());
                lvi.Tag = $"{_fishingGround}-{panelTitle}";
            }

            SizeColumns(lvResults, false);
            lvResults.ItemChecked += OnListItemChecked;

            bool enable = !LayoutHelper.LayoutTemplateFromFile;
            txtColumns.Enabled = enable;
            txtRows.Enabled = enable;
            txtOverlap.Enabled = enable;
            txtPageHeight.Enabled = enable;
            txtPageWidth.Enabled = enable;
        }

        private bool AcceptOptions(bool forLayout=true)
        {
            bool accept = txtColumns.Text.Length > 0
                && txtRows.Text.Length > 0
                && txtOverlap.Text.Length > 0
                && txtPageHeight.Text.Length > 0
                && txtPageWidth.Text.Length > 0;

            if(!forLayout && accept)
            {
                accept = textFolderToSave.Text.Length > 0;

            }

            

            if (accept)
            {
                PageWidth = int.Parse(txtPageWidth.Text);
                PageHeight = int.Parse(txtPageHeight.Text);
                Overlap = int.Parse(txtOverlap.Text);
                Rows = int.Parse(txtRows.Text);
                Columns = int.Parse(txtColumns.Text);
            }
            else
            {
                MessageBox.Show("Please provide values for all fields", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return accept;
        }

        public static string GetSavedMapsFolder()
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            try
            {
                return reg_key.GetValue("FolderSavedGrids").ToString();
            }
            catch
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        public static void SetSavedMapsFolder(string folderPath)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            reg_key.SetValue("FolderSavedGrids", folderPath);
        }



        private async void BatchExportAsync()
        {
            bool layerFormIsOpen=false;
            foreach(Form f in Application.OpenForms)
            {
                if(f.Name=="MapLayersForm")
                {
                    layerFormIsOpen = true;
                    break;
                }
            }

            if (!layerFormIsOpen)
            {
                _grid25GeographicDisplayHelper.LockMap();
                _exportedImageCount = 0;
                _fishingGround = textFishingGround.Text;
                _exportDPI = int.Parse(txtDPI.Text);
                for (int n = 0; n < lvResults.Items.Count; n++)
                {
                    _layoutPanelTitle = lvResults.Items[n].Text;
                    if(await BatchExportTask())
                    {
                        _exportedImageCount++;
                    }
                }

                var msg = "";
                if ((_exportedImageCount) == lvResults.Items.Count)
                {
                    msg = $"Successfully exported all {_exportedImageCount} grid maps";
                }
                else
                {
                    msg = $"Not all grids were exported.\r\nOnly {_exportedImageCount} grids were exported";
                }
                MessageBox.Show(msg, "Export grid map to image", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please close layers form to proceed", "Cannot proceed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            _grid25GeographicDisplayHelper.UnlockMap();
            _grid25GeographicDisplayHelper.RedrawNap();
        }

        private Task<bool> BatchExportTask()
        {
            return Task.Run(() => BatchExportMain());
        }

         private bool BatchExportMain()
        {
            _grid25GeographicDisplayHelper.SourceFolder = _gridMapSourceFolderPath;
            _grid25GeographicDisplayHelper.SelectedLayoutCell = _layoutPanelTitle; ;
            var adjustValue = 60 * 1852;
            var ext = new Extents();

            var extMBRMax = _grid25GeographicDisplayHelper.MaxDimensionMBR.Extents;
            var maxWidth = extMBRMax.xMax - extMBRMax.xMin;
            var maxHeight = extMBRMax.yMax - extMBRMax.yMin;

            var extMBRGrid = _grid25GeographicDisplayHelper.MBR.Extents;
            var labelDistance = _grid25GeographicDisplayHelper.MinorGridLabelDistance;

            ext.SetBounds(extMBRGrid.xMin - (labelDistance * 3) - XBuffer / adjustValue,
                extMBRGrid.yMin - (labelDistance * 3) - YBottomBuffer / adjustValue,
                0,
                (extMBRGrid.xMin + maxWidth) + (labelDistance * 3) + XBuffer / adjustValue,
                (extMBRGrid.yMin + maxHeight) + (labelDistance * 3) + YTopBuffer / adjustValue,
                0);


            _grid25GeographicDisplayHelper.SetMapExtents(ext);
            SaveMapImage smi = new SaveMapImage($@"{_exportImageFolderSavePath}\{_layoutPanelTitle}_grid.tif", _exportDPI, global.MappingForm.MapControl);
            smi.MapLayersHandler = global.MappingForm.MapLayersHandler;
            smi.PreviewImage = false;

            bool rv = smi.Save(true);
            if(rv)
            {
                smi.Dispose();
                smi = null;
            }
            return rv;
        }


        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)(sender)).Name)
            {
                case "btnExportSettings":
                    using (BatchExportMapSettingsForm bef = new BatchExportMapSettingsForm())
                    {
                        if (_exportSettingsDict.Count == 0)
                        {
                            foreach (MapLayer ml in global.MappingForm.MapLayersHandler)
                            {
                                if (ml.Visible)
                                {
                                    _exportSettingsDict.Add(ml.Handle, (ml.Name, true, false, true, true));
                                }
                            }
                        }
                        
                        bef.ExportSettingsDict = _exportSettingsDict;
                        bef.ShowDialog();
                        if(bef.DialogResult==DialogResult.OK)
                        {
                            _exportSettingsDict = new Dictionary<int, (string layerName, bool showFront, bool showFrontLabel, bool showReverse, bool showReverseLabel)>(bef.ExportSettingsDict);    
                        }
                    }
                    break;

                case "btnExport":
                    BatchExportAsync();
                    break;

                case "btnFolderExportImage":
                    var folderBrowser = new FolderBrowserDialog();
                    folderBrowser.ShowNewFolderButton = true;
                    folderBrowser.SelectedPath = SaveMapForm.GetSavedMapsFolder();
                    folderBrowser.Description = "Locate folder to save exported map images";
                    DialogResult result = FolderBrowserLauncher.ShowFolderBrowser(folderBrowser);
                    if (result == DialogResult.OK && folderBrowser.SelectedPath.Length>0)
                    {
                        _exportImageFolderSavePath = folderBrowser.SelectedPath;
                        txtFolderExportPath.Text = _exportImageFolderSavePath;
                        btnExport.Enabled = true;
                        btnExportSettings.Enabled = btnExport.Enabled && chkExportFrontBack.Checked;
                    }
                    break;
                case "btnGridSettings":
                    using (Grid25GeographicDisplayOptionsForm ggdf = new Grid25GeographicDisplayOptionsForm(
                        _grid25GeographicDisplayHelper.MinorGridFontSize,
                        _grid25GeographicDisplayHelper.MinorGridOffsetDistance,
                        _grid25GeographicDisplayHelper.MajorGridFontSize,
                        _grid25GeographicDisplayHelper.SubgridsVisible,
                        _grid25GeographicDisplayHelper.MinorGridLableFontBold,
                        _grid25GeographicDisplayHelper.MajorGridLableFontBold))
                    {
                        ggdf.ShowDialog();
                        if (ggdf.DialogResult == DialogResult.OK)
                        {
                            _grid25GeographicDisplayHelper.MinorGridOffsetDistance = ggdf.MinorGridDistance;
                            _grid25GeographicDisplayHelper.MinorGridFontSize = ggdf.MinorGridFontSize;
                            _grid25GeographicDisplayHelper.MajorGridFontSize = ggdf.MajorGridFontSize;
                            _grid25GeographicDisplayHelper.SubgridsVisible = ggdf.ShowSubgrid;
                            _grid25GeographicDisplayHelper.MinorGridLableFontBold = ggdf.MinorGridBoldLabels;
                            _grid25GeographicDisplayHelper.MajorGridLableFontBold = ggdf.MajorGridBoldLabels;
                            //_grid25GeographicDisplayHelper.RemoveGrid25Layers();
                            _grid25GeographicDisplayHelper.SelectedLayoutCell = lvResults.CheckedItems[0].Text;
                            _grid25GeographicDisplayHelper.SymbolizeGrid();
                        }
                    }
                        break;

                case "btnLocateSourceFolder":
                    folderBrowser = new FolderBrowserDialog();
                    folderBrowser.ShowNewFolderButton = true;
                    folderBrowser.SelectedPath = SaveMapForm.GetSavedMapsFolder();
                    folderBrowser.Description = "Locate folder containing saved fishing ground grid maps";
                    result = FolderBrowserLauncher.ShowFolderBrowser(folderBrowser);
                    if (result == DialogResult.OK && folderBrowser.SelectedPath.Length>0)
                    {
                        _gridMapSourceFolderPath = folderBrowser.SelectedPath;
                        _grid25GeographicDisplayHelper.SetMaxDimensionGridName(_gridMapSourceFolderPath, lvResults.Items[_maxDimensionIndex].Text);
                    }
                    break;
                case "btnSaveTemplate":
                    var saveAs = new SaveFileDialog();
                    saveAs.Filter = "Shapefile *.shp|*.shp|All files *.*|*.*";
                    saveAs.FilterIndex = 1;
                    saveAs.FileName = $"{textFishingGround.Text}_template.shp";
                    DialogResult dr = saveAs.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        if (!_parentForm.Grid25MajorGrid.LayoutHelper.SaveLayoutTemplate(saveAs.FileName))
                        {
                            MessageBox.Show("Template file was not saved");
                        }
                    }
                    break;

                case "buttonSubGrid":
                    _hasSubGrid = _majorGrid.HasSubgrid;
                    using (Grid25SubGridForm sgf = new Grid25SubGridForm())
                    {
                        sgf.SubGridCount = _majorGrid.SubGridCount;
                        sgf.ShowDialog();
                        if (sgf.DialogResult == DialogResult.OK)
                        {
                            _hasSubGrid = true;
                            _subGridCount = sgf.SubGridCount;
                            _majorGrid.HasSubgrid = _hasSubGrid;
                            _majorGrid.SubGridCount = _subGridCount;

                            //reflect changes of subgrid choice
                            string gridTitle = lvResults.CheckedItems[0].Text;
                            LayerEventArg lve = new LayerEventArg(gridTitle);
                            lve.FileName = $"{lvResults.CheckedItems[0].Tag.ToString()}";
                            lve.SelectedIndex = lvResults.CheckedItems[0].Index;
                            lve.SelectedExtent = LayoutHelper.LayoutShapeFile.Shape[lvResults.CheckedItems[0].Index].Extents;

                            lve.Action = "LoadGridMap";
                            _majorGrid.LoadPanelGrid(chkAutoExpand.Checked, lve);
                            _parentForm.MapTitle(gridTitle);
                        }
                    }
                    break;

                case "btnInputTitles":
                    _majorGrid.MapLayers.set_MapLayer(LayoutHelper.LayerHandle);
                    EditShapeAttributeForm esaf = EditShapeAttributeForm.GetInstance(global.MappingForm, _majorGrid.MapInterActionHandler);
                    if (esaf.Visible)
                    {
                        esaf.BringToFront();
                    }
                    else
                    {
                        esaf.Show(this);
                    }
                    break;


                case "btnSelectFolderSave":
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.Description = "Select folder to save fishing ground grid map";
                    fbd.SelectedPath = GetSavedMapsFolder();
                    dr = FolderBrowserLauncher.ShowFolderBrowser(fbd);
                    if (dr == DialogResult.OK && fbd.SelectedPath.Length > 0)
                    {
                        _parentFolder = fbd.SelectedPath;
                        SetSavedMapsFolder(_parentFolder);
                        textFolderToSave.Text = _parentFolder;
                        btnSave.Enabled = textFishingGround.Text.Length > 0 && _parentFolder.Length > 0;
                    }
                    break;

                case "btnSave":
                    if (AcceptOptions(forLayout:false))
                    {
                        if (_majorGrid.LayoutHelper.LayoutShapeFile.NumShapes > 0
                        && _majorGrid.LayoutHelper.HasCompletePanelTitles())

                        {
                            //_majorGrid.HasSubgrid = _parentForm.HasSubGrid;
                            //_majorGrid.SubGridCount = _parentForm.SubGridCount;
                            if (_majorGrid.SaveMinorGridsInLayout(textFishingGround.Text, _parentFolder))
                            {
                                //if (_majorGrid.LayoutHelper.LayerHandle > 0)
                                //{
                                //    FillResultList();
                                //    _majorGrid.MapLayers.ClearAllSelections();
                                //}
                                MessageBox.Show("Grid maps saved", "Save successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please provide a layout complete with titles", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnApplyDimension":
                    if (AcceptOptions())
                    {
                        _majorGrid.LayoutHelper.SetupLayout(Rows, Columns, Overlap);
                    }
                    break;
            }
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

        private void SetupResultsView()
        {
            lvResults.ItemChecked -= OnListItemChecked;
            lvResults.View = View.Details;
            lvResults.FullRowSelect = true;
            lvResults.Columns.Clear();
            lvResults.Columns.Add("Title");
            lvResults.HideSelection = false;
            if (global.MappingMode == Database.Classes.fad3MappingMode.grid25Mode)
            {
                lvResults.Columns.Add("Width");
                lvResults.Columns.Add("Height");
            }
            lvResults.CheckBoxes = true;
            SizeColumns(lvResults);
        }

        private void FillResultList()
        {
            SetupResultsView();
            lvResults.Items.Clear();
            int fldTitle = LayoutHelper.LayoutShapeFile.FieldIndexByName["Title"];
            for (int n = 0; n < LayoutHelper.LayoutShapeFile.NumShapes; n++)
            {
                string mapTitle = LayoutHelper.LayoutShapeFile.CellValue[fldTitle, n].ToString();
                var lvi = lvResults.Items.Add(mapTitle);
                var shp = LayoutHelper.LayoutShapeFile.Shape[n];
                lvi.SubItems.Add(shp.Extents.Width.ToString());
                lvi.SubItems.Add(shp.Extents.Height.ToString());
                lvi.Checked = false;
                lvi.Tag = $"{textFishingGround.Text}-{mapTitle}";
            }

            SizeColumns(lvResults, false);
            lvResults.ItemChecked += OnListItemChecked;
            lvResults.Items[lvResults.Items.Count - 1].Checked = true;
        }

        private void OnTextValidating(object sender, CancelEventArgs e)
        {
            string s = ((TextBox)sender).Text;
            string msg = "Value must be whole number greater than zero";
            int v = 0;
            if (s.Length > 0)
            {
                switch (((TextBox)sender).Name)
                {
                    case "txtPageWidth":
                    case "txtPageHeight":
                    case "txtRows":
                    case "txtColumns":
                    case "txtDP!":
                        if (int.TryParse(s, out v))
                        {
                            e.Cancel = v <= 0;
                        }
                        else
                        {
                            e.Cancel = true;
                        }

                        break;

                    case "txtOverlap":
                        if (int.TryParse(s, out v))
                        {
                            e.Cancel = v < 0;
                            if (e.Cancel)
                            {
                                msg = "Value must be a whole number not less than zero";
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                            msg = "Value must be a whole number not less than zero";
                        }
                        break;

                    case "textFishingGround":
                        btnSave.Enabled = textFishingGround.Text.Length > 0 && _parentFolder.Length > 0;
                        break;
                }
            }
            if (e.Cancel)
            {
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            chkAutoExpand.Visible = false;
            _gridMapSourceFolderPath = "";
            global.LoadFormSettings(this, true);
            if (global.MappingMode == Database.Classes.fad3MappingMode.grid25Mode)
            {
                txtRows.Text = _majorGrid.LayoutRows.ToString();
                txtColumns.Text = _majorGrid.LayoutCols.ToString();
                txtOverlap.Text = _majorGrid.LayoutOverlap.ToString();
                textLayoutTemplateFileName.Text = _majorGrid.LayoutHelper.LayoutShapeFile?.Filename;
                textLayoutTemplateFileName.Enabled = false;
                //tabsLayout.TabPages.Remove(tabExport);
            }
            else
            {
                _grid25GeographicDisplayHelper = new Grid25GeographicDisplayHelper(global.MappingForm.MapLayersHandler, global.MappingForm.MapControl);
                ReadLayoutData();
                var layoutShapeFileName = _layoutFileName.Replace(".lay", ".shp");

                var result = global.MappingForm.MapLayersHandler.FileOpenHandler(layoutShapeFileName, "Layout frame",reproject:true);
                if (!result.success)
                {
                    MessageBox.Show(result.errMsg, "File open error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _layoutShapefile = (Shapefile)global.MappingForm.MapLayersHandler.get_MapLayer("Layout frame").LayerObject;
                    _layoutShapefile.DefaultDrawingOptions.FillVisible = false;
                    _layoutShapefile.DefaultDrawingOptions.LineWidth = 1.5F;
                    _layoutShapefile.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.Blue);
                    int fTitle = _layoutShapefile.FieldIndexByName["Title"];
                    SetupResultsView();

                    double maxDimension = 0;
                    lvResults.Items.Clear();
                    for (int n = 0; n < _layoutShapefile.NumShapes; n++)
                    {
                        ListViewItem lvi = lvResults.Items.Add(_layoutShapefile.CellValue[fTitle, n].ToString());
                        var ext = _layoutShapefile.Shape[n].Extents;

                        if(ext.Width+ext.Height >maxDimension)
                        {
                            maxDimension = ext.Width + ext.Height;
                            _maxDimensionIndex = n;
                        }

                        Console.WriteLine($"w: {ext.Width}, h: {ext.Height}, n: {n}");
                    }

                    SizeColumns(lvResults, false);
                    global.MappingForm.MapControl.Extents = _layoutShapefile.Extents;
                    buttonSubGrid.Visible = false;
                    btnInputTitles.Visible = false;
                    btnApplyDimension.Visible = false;
                    btnGridSettings.Visible = true;
                    btnGridSettings.Location = btnLocateSourceFolder.Location;
                    btnLocateSourceFolder.Visible = true;
                    btnLocateSourceFolder.Location = buttonSubGrid.Location;
                    tabsLayout.TabPages.Remove(tabSave);
                    lvResults.ItemChecked += OnListItemChecked;
                    btnFolderExportImage.Enabled = false;
                    btnExport.Enabled = false;
                }
            }
        }

        private Extents DefinePageExtent(Extents selectedMajorGridShapesExtent, Extents selectionBoxExtent)
        {
            var ext = new Extents();
            if (selectionBoxExtent.ToShape().Contains(selectedMajorGridShapesExtent.ToShape()))
            {
                //use extent of the selected major grid shapes
                ext = selectedMajorGridShapesExtent;
            }
            else if (selectedMajorGridShapesExtent.ToShape().Contains(selectionBoxExtent.ToShape()))
            {
                //use extent of the selection box
                ext = selectionBoxExtent;
            }
            else if (selectionBoxExtent.ToShape().Intersects(selectedMajorGridShapesExtent.ToShape()))
            {
                var results = new object();
                if (selectionBoxExtent.ToShape().GetIntersection(selectedMajorGridShapesExtent.ToShape(), ref results))
                {
                    //convets results object to an array of shapes that is a product of the intersection
                    object[] shapeArray = results as object[];
                    if (shapeArray != null)
                    {
                        Shape[] shapes = shapeArray.OfType<Shape>().ToArray();

                        // extent is the intersection of the selected major grids and the selection box
                        ext = shapes[0].Extents;
                    }
                }
            }
            else
            {
                //mbrExtent is outside of extent of selected major grid
                ext = null;
            }

            return ext;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (_majorGrid != null)
            {
                _majorGrid.ReleaseLayoutHelper();
                _majorGrid = null;
            }
            _instance = null;

            if (_layoutShapefile != null)
            {
                _layoutShapefile.Close();
                _layoutShapefile = null;
                global.MappingForm.MapLayersHandler.RemoveLayer("Layout frame");
            }
            if(_grid25GeographicDisplayHelper!=null)
            {
                _grid25GeographicDisplayHelper.RemoveGrid25Layers();
            }
            global.SaveFormSettings(this);
            global.MappingForm.SetCursor(tkCursorMode.cmSelection);
            _parentForm?.SetupGridButtons(enabled: true);
            _parentForm = null;
        }


 


        private void OnListItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var lv = sender as ListView;
            ListViewItem item = e.Item;
            bool itemIsChecked = !item.Checked;

            LayerEventArg lve = new LayerEventArg(e.Item.Text);

            if (e.Item.Text.Length > 0)
            {
                lve.FileName = $"{item.Tag?.ToString()}";
                lve.SelectedIndex = item.Index;
                lve.SelectedExtent = LayoutHelper?.LayoutShapeFile.Shape[item.Index].Extents;
                if (item.Checked)
                {
                    lve.Action = "LoadGridMap";
                }
                else
                {
                    lve.Action = "UnloadGridMap";
                }
            }
            else
            {
                lve.Action = "UnloadGridMap";
            }

            //only allow one checked item in the listbox
            if (lvResults.CheckedItems.Count > 1)
            {
                UncheckAllItems(e.Item.Index);
            }

            if (lvResults.CheckedItems.Count == 0)
            {
                _parentForm?.MapTitle("");
            }

            if (global.MappingMode == Database.Classes.fad3MappingMode.grid25Mode)
            {
                _majorGrid.LoadPanelGrid(chkAutoExpand.Checked, lve);
                _parentForm.MapTitle(item.Text);
            }
            else
            {
                _layoutShapefile.SelectNone();
                _grid25GeographicDisplayHelper.RemoveGrid25Layers();
                if (lvResults.CheckedItems.Count > 0)
                {
                    _layoutShapefile.ShapeSelected[e.Item.Index] = true;
                    if(_gridMapSourceFolderPath.Length>0)
                    {
                        _grid25GeographicDisplayHelper.SourceFolder = _gridMapSourceFolderPath;
                        _grid25GeographicDisplayHelper.SelectedLayoutCell = lvResults.CheckedItems[0].Text;
                        if(chkAutoExpand.Checked)
                        {
                            var adjustValue = 60 * 1852;
                            var ext = new Extents();

                            var extMBRMax = _grid25GeographicDisplayHelper.MaxDimensionMBR.Extents;
                            var maxWidth = extMBRMax.xMax - extMBRMax.xMin;
                            var maxHeight = extMBRMax.yMax - extMBRMax.yMin;

                            var extMBRGrid = _grid25GeographicDisplayHelper.MBR.Extents;
                            var labelDistance = _grid25GeographicDisplayHelper.MinorGridLabelDistance;
                            //ext.SetBounds(extMBR.xMin - (labelDistance * 3) - XBuffer / adjustValue,
                            //    extMBR.yMin - (labelDistance * 3) - YBottomBuffer / adjustValue,
                            //    0, 
                            //    extMBR.xMax + (labelDistance * 3) + XBuffer / adjustValue, 
                            //    extMBR.yMax + (labelDistance * 3) + YTopBuffer / adjustValue, 
                            //    0);
                            //ext.SetBounds(extMBR.xMin - (labelDistance * 5) - XBuffer / adjustValue,
                            //    extMBR.yMin - (labelDistance * 5) - YBottomBuffer / adjustValue,
                            //    0,
                            //    (extMBR.xMin + _maxPanelWidth) + (labelDistance * 5) + XBuffer / adjustValue,
                            //    (extMBR.yMin + _maxPanelHeight) + (labelDistance * 5) + YTopBuffer / adjustValue,
                            ext.SetBounds(extMBRGrid.xMin - (labelDistance * 3) - XBuffer / adjustValue,
                                extMBRGrid.yMin - (labelDistance * 3) - YBottomBuffer / adjustValue,
                                0,
                                (extMBRGrid.xMin + maxWidth) + (labelDistance * 3) + XBuffer / adjustValue,
                                (extMBRGrid.yMin + maxHeight) + (labelDistance * 3) + YTopBuffer / adjustValue,
                                0);
                            global.MappingForm.MapControl.Extents = ext;
                        }
                    }
                    else
                    {
                        if(chkAutoExpand.Checked)
                        {
                            global.MappingForm.MapControl.Extents = _layoutShapefile.Shape[lv.CheckedItems[0].Index].Extents;
                        }
                    }
                }
                
                global.MappingForm.MapControl.Redraw();
            }

            if (lvResults.CheckedItems.Count > 0)
            {
                global.MappingForm.MapLayersHandler.RefreshLayers();
            }
        }


        private void UncheckAllItems(int index)
        {
            lvResults.ItemChecked -= OnListItemChecked;
            foreach (ListViewItem lvi in lvResults.Items)
            {
                if (lvi.Index != index)
                {
                    lvi.Checked = false;
                }
            }
            lvResults.ItemChecked += OnListItemChecked;
        }


        private void OnTabsSelectionChanged(object sender, EventArgs e)
        {
            //chkAutoExpand.Visible = false;

            var t = sender as TabControl;
            switch (t.SelectedTab.Name)
            {
                case "tabResults":
                    chkAutoExpand.Visible = true;
                    if (global.MappingMode == Database.Classes.fad3MappingMode.grid25Mode)
                    {
                        buttonSubGrid.Enabled = true;
                        lblProvideTitles.Visible = false;
                        if (textFishingGround.Text.Length > 0
                            && _majorGrid.LayoutHelper.LayoutShapeFile.NumShapes > 0
                            && _majorGrid.LayoutHelper.HasCompletePanelTitles())
                        {
                            _majorGrid.SetExtentFromLayout();
                            lvResults.Visible = true;
                            FillResultList();
                        }
                        else
                        {
                            if (lvResults.CheckedItems.Count > 0)
                            {
                                lvResults.CheckedItems[0].Checked = false;
                            }
                            lvResults.Visible = false;
                            lblProvideTitles.Visible = true;
                            buttonSubGrid.Enabled = false;
                        }
                        _majorGrid.LayoutHelper.FishingGround = textFishingGround.Text;
                        
                        
                    }
                    else
                    {
                        lblProvideTitles.Visible = false;
                        lvResults.Visible = true;
                    }
                    break;

                case "tabSave":
                    chkAutoExpand.Visible = false;
                    switch (global.MappingMode)
                    {
                        case Database.Classes.fad3MappingMode.defaultMode:
                            break;
                        case Database.Classes.fad3MappingMode.grid25Mode:
                            btnSaveTemplate.Enabled = textFishingGround.Text.Length > 0
                                && lvResults.Items.Count > 0
                                && !_majorGrid.LayoutHelper.LayoutTemplateFromFile;
                            break;
                    }
                    break;

                case "tabExport":
                    chkAutoExpand.Visible = false;
                    if (global.MappingMode == Database.Classes.fad3MappingMode.defaultMode)
                    {


                        if (_grid25GeographicDisplayHelper.HasGrid)
                        {
                            btnFolderExportImage.Enabled = true;
                            btnExport.Enabled = txtFolderExportPath.Text.Length > 0;


                        }
                    }
                    else if(global.MappingMode==Database.Classes.fad3MappingMode.grid25Mode)
                    {
                        btnFolderExportImage.Enabled = textFishingGround.Text.Length > 0
                            && lvResults.Items.Count > 0;
                    }
                    break;
                default:
                    chkAutoExpand.Visible = false;
                    break;
            }
        }

        private void OnListViewMouseDown(object sender, MouseEventArgs e)
        {
            ListView lv = (ListView)sender;
            _mouseX = e.Location.X;
            _mouseY = e.Location.Y;

            bool rebuildAll = true;
            foreach (ListViewItem lvi in lvResults.Items)
            {
                if (lvi.Text.Length > 0)
                {
                    rebuildAll = false;
                    break;
                }
            }

            ListViewHitTestInfo lvh = lv.HitTest(_mouseX, _mouseY);
            if (lvh.Item != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    menuDropDown.Items.Clear();

                    if (rebuildAll)
                    {
                        var tsi = menuDropDown.Items.Add("Rebuild all Grid25 map");
                        tsi.Name = "menuRebuildAllGrid25";
                    }
                    else
                    {
                        var tsi = menuDropDown.Items.Add("Rebuild Grid25 map");
                        tsi.Name = "menuRebuildGrid25";
                    }
                }
                //else if (global.MappingMode==Database.Classes.fad3MappingMode.defaultMode &&  e.Button == MouseButtons.Left)
                //{
                //    global.MappingForm.MapControl.Extents = _layoutShapefile.Shape[lvh.Item.Index].Extents;
                //    global.MappingForm.MapControl.ExtentPad = 10D;
                //}
            }
        }

        private void OnDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.Owner.Hide();
            int fldTitleHandle = _majorGrid.LayoutHelper.LayoutShapeFile.FieldIndexByName["Title"];
            var panelExtent = new Extents();
            double w = 0;
            double h = 0;
            string panelTitle = "";

            switch (e.ClickedItem.Name)
            {
                case "menuRebuildGrid25":
                    panelExtent = _majorGrid.LayoutHelper.LayoutShapeFile.Shape[lvResults.SelectedItems[0].Index].Extents;
                    w = panelExtent.Width;
                    h = panelExtent.Height;
                    panelTitle = _majorGrid.LayoutHelper.LayoutShapeFile.CellValue[fldTitleHandle, lvResults.SelectedItems[0].Index].ToString();
                    if (_majorGrid.GenerateMinorGridInsidePanelExtent(panelExtent, panelTitle)
                        && _majorGrid.Save($@"{textFolderToSave.Text}\{textFishingGround.Text}-{panelTitle}"))
                    {
                        lvResults.SelectedItems[0].Text = panelTitle;
                        lvResults.SelectedItems[0].SubItems.Add(w.ToString());
                        lvResults.SelectedItems[0].SubItems.Add(h.ToString());
                        lvResults.SelectedItems[0].Tag = $"{textFishingGround.Text}-{panelTitle}";
                        lvResults.SelectedItems[0].Checked = true;
                    }
                    break;

                case "menuRebuildAllGrid25":
                    foreach (ListViewItem lvi in lvResults.Items)
                    {
                        panelExtent = _majorGrid.LayoutHelper.LayoutShapeFile.Shape[lvi.Index].Extents;
                        w = panelExtent.Width;
                        h = panelExtent.Height;
                        panelTitle = _majorGrid.LayoutHelper.LayoutShapeFile.CellValue[fldTitleHandle, lvi.Index].ToString();
                        if (_majorGrid.GenerateMinorGridInsidePanelExtent(panelExtent, panelTitle)
                            && _majorGrid.Save($@"{textFolderToSave.Text}\{textFishingGround.Text}-{panelTitle}"))
                        {
                            lvi.Text = panelTitle;
                            lvi.SubItems.Add(w.ToString());
                            lvi.SubItems.Add(h.ToString());
                            lvi.Tag = $"{textFishingGround.Text}-{panelTitle}";
                            lvi.Checked = true;
                        }
                    }
                    break;
            }
            SizeColumns(lvResults, false);
        }

        private void OnListViewSelectedIndexChange(object sender, EventArgs e)
        {
            //ListView lv = (ListView)sender;

            //if (global.MappingMode == Database.Classes.fad3MappingMode.defaultMode && lv.SelectedIndices.Count > 0)
            //{
            //    if (_grid25GeographicDisplayHelper.HasGrid)
            //    {
            //        var adjustValue = 60 * 1852;
            //        var ext = new Extents();
            //        var extMBR = _grid25GeographicDisplayHelper.MBR.Extents;
            //        var labelDistance = _grid25GeographicDisplayHelper.MinorGridLabelDistance;
            //        ext.SetBounds(extMBR.xMin - (labelDistance * 3) - XBuffer/adjustValue , extMBR.yMin - (labelDistance * 3) - YBottomBuffer/adjustValue, 0, extMBR.xMax + (labelDistance * 3) + XBuffer/adjustValue, extMBR.yMax + (labelDistance * 3) + YTopBuffer/adjustValue, 0);
            //        global.MappingForm.MapControl.Extents = ext;
            //    }
            //    else
            //    {
            //        global.MappingForm.MapControl.Extents = _layoutShapefile.Shape[lv.SelectedItems[0].Index].Extents;
            //    }
            //}
        }

        private void OnFormActivated(object sender, EventArgs e)
        {
            if (_majorGrid != null 
                &&_majorGrid.MinorGrids.MinorGridLinesShapeFile == null)
            {
                _majorGrid.DefineGridLayout((int)((Bitmap)imageList1.Images["grid_layout"]).GetHicon());
            }
        }

        private void OnCheckChanged(object sender, EventArgs e)
        {
            if(btnExport.Enabled)
            {
                btnExportSettings.Enabled = chkExportFrontBack.Checked;
            }
        }
    }
}