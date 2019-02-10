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
            e.LayoutSpecs(int.Parse(txtRows.Text), int.Parse(txtColumns.Text), int.Parse(txtOverlap.Text));
        }

        private ListViewItem _lastItemChecked;
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
                case "btnOpenLayout":
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Title = "Open a layout grid template file";
                    ofd.Filter = "Layout file|*.lay|All files|*.*";
                    ofd.FilterIndex = 0;
                    ofd.ShowDialog();
                    if (ofd.FileName.Length > 0)
                    {
                        if (LayoutHelper.OpenLayoutFile(ofd.FileName))
                        {
                            string layoutData = File.ReadAllText(ofd.FileName);
                            textFishingGround.Text = layoutData;
                            textFolderToSave.Text = Path.GetDirectoryName(ofd.FileName);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Selected file is not valid", "Invalid file", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    //if (AcceptOptions()
                    //    && _majorGrid.LayoutHelper.SetupLayout(Rows, Columns, Overlap)
                    //    && _majorGrid.GenerateMinorGridFromLayout())
                    if (AcceptOptions())
                    {
                        if (_majorGrid.LayoutHelper.LayoutShapeFile.NumShapes > 0
                        && _majorGrid.LayoutHelper.HasCompletePanelTitles())

                        {
                            if (_majorGrid.GenerateMinorGridFromLayout(textFishingGround.Text, _parentFolder))
                            {
                                if (_majorGrid.LayoutHelper.LayerHandle > 0)
                                {
                                    _majorGrid.LayoutHelper.FishingGround = textFishingGround.Text;
                                    _majorGrid.MapLayers[_majorGrid.LayoutHelper.LayerHandle].FishingGridFishingGroundName = textFishingGround.Text;
                                    FillResultList();
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

        private void FillResultList()
        {
            lvResults.ItemChecked -= OnListItemChecked;
            lvResults.View = View.Details;
            lvResults.Columns.Clear();
            lvResults.Columns.Add("Title");
            lvResults.Columns.Add("Width");
            lvResults.Columns.Add("Height");
            lvResults.CheckBoxes = true;
            SizeColumns(lvResults);

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
            switch (((TextBox)sender).Name)
            {
                case "txtPageWidth":
                case "txtPageHeight":
                    break;

                case "txtRows":
                case "txtColumns":
                    break;

                case "txtOverlap":
                    break;

                case "textFishingGround":
                    btnSave.Enabled = textFishingGround.Text.Length > 0 && _parentFolder.Length > 0;
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            var ext = DefinePageExtent(MajorGridSelectionExtent, SelectionBoxExtent);
            if (ext != null)
            {
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
            _majorGrid.ReleaseLayoutHelper();
            _majorGrid = null;
            _instance = null;
        }

        private void OnListItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var lv = sender as ListView;
            ListViewItem item = e.Item;
            bool itemIsChecked = !item.Checked;

            LayerEventArg lve = new LayerEventArg(e.Item.Text);
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
            _majorGrid.LoadPanelGrid(lve);
        }
    }
}