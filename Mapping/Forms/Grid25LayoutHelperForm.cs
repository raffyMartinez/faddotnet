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
    public partial class Grid25LayoutHelperForm : Form
    {
        public static Grid25LayoutHelperForm GetInstance(Grid25MajorGrid majorGrid)
        {
            if (_instance == null) _instance = new Grid25LayoutHelperForm(majorGrid);
            return _instance;
        }

        public Grid25LayoutHelperForm(Grid25MajorGrid majorGrid)
        {
            InitializeComponent();
            _majorGrid = majorGrid;
            LayoutHelper = _majorGrid.LayoutHelper;
            LayoutHelper.LayerCreated += OnLayoutCreated;
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

        public void SetUpFields()
        {
            _fishingGround = LayoutHelper.FishingGround;
            textFishingGround.Text = _fishingGround;
            _savePath = LayoutHelper.GridFromLayoutSaveFolder;
            textFolderToSave.Text = _savePath;
            txtRows.Text = LayoutHelper.Rows.ToString();
            txtColumns.Text = LayoutHelper.Columns.ToString();
            txtOverlap.Text = LayoutHelper.Overlap.ToString();

            SetupResultsView();

            int panelCount = LayoutHelper.Columns * LayoutHelper.Rows;
            for (int n = 0; n < panelCount; n++)
            {
                int fldTitle = LayoutHelper.LayoutShapeFile.FieldIndexByName["Title"];
                string panelTitle = LayoutHelper.LayoutShapeFile.CellValue[fldTitle, n].ToString();
                Extents ext = LayoutHelper.LayoutShapeFile.Shape[n].Extents;
                if (File.Exists($@"{_savePath}\{_fishingGround}-{panelTitle}_gridlines.shp"))
                {
                    ListViewItem lvi = lvResults.Items.Add(panelTitle);
                    lvi.SubItems.Add(ext.Width.ToString());
                    lvi.SubItems.Add(ext.Height.ToString());
                    lvi.Tag = $"{_fishingGround}-{panelTitle}";
                }
                else
                {
                    lvResults.Items.Add("");
                }
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

        private bool AcceptOptions()
        {
            bool accept = true;
            foreach (Control c in Controls)
            {
                if (c.GetType().Name == "TextBox" && ((TextBox)c).Text.Length == 0)
                {
                    accept = false;
                    break;
                }
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

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)(sender)).Name)
            {
                case "btnSaveLayout":
                    if (LayoutHelper.ValidLayoutTemplateShapefile())
                    {
                        var saveAs = new SaveFileDialog();
                        saveAs.Filter = "Shapefile *.shp|*.shp|All files *.*|*.*";
                        saveAs.FilterIndex = 1;
                        saveAs.ShowDialog();
                        if (saveAs.FileName.Length > 0)
                        {
                            if (File.Exists(saveAs.FileName))
                            {
                                ShapefileDiskStorageHelper.Delete(saveAs.FileName.Replace(".shp", ""));
                            }
                            if (LayoutHelper.SaveLayoutTemplate(saveAs.FileName))
                            {
                                MessageBox.Show("Layout template successfuly saved!");
                            }
                            else
                            {
                                MessageBox.Show("Layout template was not saved");
                            }
                        }
                    }
                    break;

                case "btnSelectFolderSave":
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.Description = "Select folder to save fishing ground grid map";
                    fbd.SelectedPath = GetSavedMapsFolder();
                    DialogResult result = FolderBrowserLauncher.ShowFolderBrowser(fbd);
                    if (result == DialogResult.OK && fbd.SelectedPath.Length > 0)
                    {
                        _parentFolder = fbd.SelectedPath;
                        SetSavedMapsFolder(_parentFolder);
                        textFolderToSave.Text = _parentFolder;
                        btnSave.Enabled = textFishingGround.Text.Length > 0 && _parentFolder.Length > 0;
                    }
                    break;

                case "btnSave":
                    if (AcceptOptions())
                    {
                        if (_majorGrid.LayoutHelper.LayoutShapeFile.NumShapes > 0
                        && _majorGrid.LayoutHelper.HasCompletePanelTitles())

                        {
                            if (_majorGrid.GenerateMinorGridFromLayout(textFishingGround.Text, _parentFolder))
                            {
                                if (_majorGrid.LayoutHelper.LayerHandle > 0)
                                {
                                    //_majorGrid.LayoutHelper.FishingGround = textFishingGround.Text;
                                    //_majorGrid.LayoutHelper.GridFromLayoutSaveFolder = textFolderToSave.Text;

                                    FillResultList();
                                    _majorGrid.MapLayers.ClearAllSelections();
                                }
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

                case "btnApply":
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
            lvResults.Columns.Add("Width");
            lvResults.Columns.Add("Height");
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
            lvResults.Items[lvResults.Items.Count - 1].Checked = true;

            SizeColumns(lvResults, false);
            lvResults.ItemChecked += OnListItemChecked;
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
            //var ext = DefinePageExtent(MajorGridSelectionExtent, SelectionBoxExtent);
            //if (ext != null)
            //{
            //}
            global.LoadFormSettings(this, true);
            txtRows.Text = _majorGrid.LayoutRows.ToString();
            txtColumns.Text = _majorGrid.LayoutCols.ToString();
            txtOverlap.Text = _majorGrid.LayoutOverlap.ToString();
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
            global.SaveFormSettings(this);
            global.MappingForm.SetCursor(tkCursorMode.cmSelection);
        }

        private void OnListItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var lv = sender as ListView;
            ListViewItem item = e.Item;
            bool itemIsChecked = !item.Checked;

            LayerEventArg lve = new LayerEventArg(e.Item.Text);

            if (e.Item.Text.Length > 0)
            {
                lve.FileName = $"{item.Tag.ToString()}";
                lve.SelectedIndex = item.Index;
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
                lvResults.ItemChecked -= OnListItemChecked;
                foreach (ListViewItem lvi in lvResults.Items)
                {
                    if (lvi.Index != e.Item.Index)
                    {
                        lvi.Checked = false;
                    }
                }
                lvResults.ItemChecked += OnListItemChecked;
            }

            //load/hide the grid inside the checked panel
            _majorGrid.LoadPanelGrid(chkAutoExpand.Checked, lve);
        }

        private void OnTabsSelectionChanged(object sender, EventArgs e)
        {
            chkAutoExpand.Visible = false;
            btnSaveLayout.Visible = false;
            var t = sender as TabControl;
            switch (t.SelectedTab.Name)
            {
                case "tabResults":
                    btnSaveLayout.Visible = true;
                    chkAutoExpand.Visible = true;
                    btnSaveLayout.Enabled = btnSave.Enabled && lvResults.Items.Count > 0 && _majorGrid.LayoutHelper.HasCompletePanelTitles();
                    break;

                case "tabSave":

                    break;
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
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
    }
}