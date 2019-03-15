using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Mapping.Classes;
using MapWinGIS;

namespace FAD3.Mapping.Forms
{
    public partial class ERDDAPSelectDatasetForm : Form
    {
        private string _ERDDAPMetadataFolder;
        private static ERDDAPSelectDatasetForm _instance;
        private Extents _selectionExtent;
        private MapLayersHandler _layersHandler;

        public static ERDDAPSelectDatasetForm GetInstance()
        {
            if (_instance == null) _instance = new ERDDAPSelectDatasetForm();
            return _instance;
        }

        public ERDDAPSelectDatasetForm()
        {
            InitializeComponent();
        }

        private void OnExtentDefined(object sender, ExtentDraggedBoxEventArgs e)
        {
            txtMaxLat.Text = e.Top.ToString();
            txtMinLat.Text = e.Bottom.ToString();
            txtMinLon.Text = e.Left.ToString();
            txtMaxLon.Text = e.Right.ToString();
            _selectionExtent = new Extents();
            _selectionExtent.SetBounds(e.Left, e.Bottom, 0, e.Right, e.Top, 0);
            if (!e.InDrag)
            {
                MakeGridFromPoints.MapLayers = global.MappingForm.MapLayersHandler;
                MakeGridFromPoints.MakeExtentShapeFile();
            }
        }

        private void UnsetMap()
        {
            if (MakeGridFromPoints.MapControl != null)
            {
                MakeGridFromPoints.UnsetMap();
                global.MappingForm.MapInterActionHandler.EnableMapInteraction = true;
            }
        }

        private void OnRadioButtonCheckChange(object sender, EventArgs e)
        {
            txtMaxLat.Text = "";
            txtMaxLon.Text = "";
            txtMinLat.Text = "";
            txtMinLon.Text = "";
            UnsetMap();
            var rbtn = (RadioButton)sender;
            if (rbtn.Checked)
            {
                switch (rbtn.Name)
                {
                    case "rbtnUseSelectionBox":
                        global.MappingForm.MapInterActionHandler.EnableMapInteraction = false;
                        MakeGridFromPoints.MapControl = global.MappingForm.MapControl;
                        break;

                    case "rbtnUseSelectedLayer":
                        if (_layersHandler.CurrentMapLayer?.LayerType == "ShapefileClass")
                        {
                            var shp = _layersHandler.CurrentMapLayer.LayerObject as Shapefile;
                            var ext = shp.Extents;
                            txtMaxLat.Text = ext.yMax.ToString();
                            txtMaxLon.Text = ext.xMax.ToString();
                            txtMinLat.Text = ext.yMin.ToString();
                            txtMinLon.Text = ext.xMin.ToString();

                            _selectionExtent = new Extents();
                            _selectionExtent.SetBounds(
                                double.Parse(txtMinLon.Text),
                                double.Parse(txtMinLat.Text),
                                0,
                                double.Parse(txtMaxLon.Text),
                                double.Parse(txtMaxLat.Text),
                                0);
                            MakeGridFromPoints.MapLayers = global.MappingForm.MapLayersHandler;
                            MakeGridFromPoints.SetDataSetExtent(_selectionExtent);
                            MakeGridFromPoints.MakeExtentShapeFile();
                        }

                        break;

                    case "rbtnManual":

                        break;
                }
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);

            _layersHandler = global.MappingForm.MapLayersHandler;
            _layersHandler.CurrentLayer += OnCurrentLayer;

            lvERDDAP.Visible = false;

            lvERDDAP.Columns.Clear();
            lvERDDAP.Columns.Add("Title");
            lvERDDAP.Columns.Add("Data start");
            lvERDDAP.Columns.Add("Data end");
            lvERDDAP.Columns.Add("Frequency");
            lvERDDAP.Columns.Add("Cell size");
            lvERDDAP.ShowItemToolTips = true;
            SizeColumns(lvERDDAP);

            //ERDDAP data download
            ERDDAPMetadataFiles.OnMetadataRead += OnERDDAPMetadataRead;
            _ERDDAPMetadataFolder = ERDDAPMetadataFiles.GetMetadataDirectorySetting();
            txtMetadataFolderPath.Text = _ERDDAPMetadataFolder;

            if (_ERDDAPMetadataFolder.Length > 0)
            {
                //the xml metadatafiles in the metadata folder determines what FAD3 can download from ERDDAP
                ERDDAPMetadataFiles.MetadataDirectory = _ERDDAPMetadataFolder;

                //read xml metadata for ERDDAP download
                ERDDAPMetadataFiles.ReadISO9115Metadata();
            }

            lvERDDAP.Visible = true;

            MakeGridFromPoints.OnExtentDefined += OnExtentDefined;
            Text = "Download gridded, oceanographic, spatio-temporal data using ERDDAP";
            UpdateDataSetCount();
        }

        private void OnCurrentLayer(MapLayersHandler s, LayerEventArg e)
        {
            if (rbtnUseSelectedLayer.Checked)
            {
                if (_layersHandler.CurrentMapLayer?.LayerType == "ShapefileClass")
                {
                    var shp = _layersHandler.CurrentMapLayer.LayerObject as Shapefile;
                    var ext = shp.Extents;
                    txtMaxLat.Text = ext.yMax.ToString();
                    txtMaxLon.Text = ext.xMax.ToString();
                    txtMinLat.Text = ext.yMin.ToString();
                    txtMinLon.Text = ext.xMin.ToString();
                }
            }
        }

        private void UpdateDataSetCount()
        {
            if (lvERDDAP.Items.Count == 0)
            {
                lblItemsCount.Text = "There are no items in the list";
            }
            else if (lvERDDAP.Items.Count == 1)
            {
                lblItemsCount.Text = "There is one item in the list";
            }
            else
            {
                lblItemsCount.Text = $"There are {lvERDDAP.Items.Count} items in the list";
            }
        }

        private void OnERDDAPMetadataRead(object sender, ERDDAPMetadataFile e)
        {
            var item = lvERDDAP.Items.Add(e.FileIdentifier, e.DataTitle, null);
            item.SubItems.Add(e.BeginPosition.ToShortDateString());
            item.SubItems.Add(e.EndPosition.ToShortDateString());
            TimeSpan diff = e.EndPosition - e.BeginPosition;
            double days = diff.TotalDays;
            var freq = Math.Round(days / e.TemporalSize, MidpointRounding.AwayFromZero);
            string dataFreq = "Daily";
            switch (freq)
            {
                case 1:
                    break;

                case 30:
                    dataFreq = "Monthly";
                    break;

                default:
                    dataFreq = $"Every {freq} days";
                    break;
            }
            item.SubItems.Add(dataFreq);
            item.SubItems.Add(e.RowSize.ToString("N4"));
            item.ToolTipText = e.DataAbstract;
            Extents ext = new Extents();
            ext.SetBounds(e.EastBound, e.SouthBound, 0, e.WestBound, e.NorthBound, 0);
            SizeColumns(lvERDDAP, false);
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

        public void UpdateEndPosition(string identifier, string endPosition)
        {
            ERDDAPMetadataFiles.UpdateMetadataEndPosition(identifier, endPosition);
            lvERDDAP.Items[identifier].SubItems[2].Text = DateTime.Parse(endPosition).ToShortDateString();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            UnsetMap();
            _instance = null;
            _layersHandler.CurrentLayer -= OnCurrentLayer;
            _layersHandler = null;
            global.SaveFormSettings(this);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnCreateExtent":

                    MakeGridFromPoints.MapLayers = global.MappingForm.MapLayersHandler;
                    MakeGridFromPoints.MakeExtentShapeFile();

                    break;

                case "btnOk":
                    Close();
                    break;

                case "btnGetMetadataFolder":
                    var folderBrowser = new FolderBrowserDialog();
                    folderBrowser.ShowNewFolderButton = true;
                    if (_ERDDAPMetadataFolder == "")
                    {
                        folderBrowser.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }
                    else
                    {
                        folderBrowser.SelectedPath = _ERDDAPMetadataFolder;
                    }
                    folderBrowser.Description = "Locate folder containing ERDDAP metadata";
                    DialogResult result = FolderBrowserLauncher.ShowFolderBrowser(folderBrowser);
                    if (result == DialogResult.OK)
                    {
                        ERDDAPMetadataFiles.MetadataFiles.Clear();
                        lvERDDAP.Visible = false;
                        lvERDDAP.Items.Clear();
                        ERDDAPMetadataFiles.MetadataDirectory = folderBrowser.SelectedPath;
                        ERDDAPMetadataFiles.ReadISO9115Metadata();
                        ERDDAPMetadataFiles.SaveMetadataDirectorySetting(folderBrowser.SelectedPath);
                        txtMetadataFolderPath.Text = folderBrowser.SelectedPath;
                        lvERDDAP.Visible = true;
                        UpdateDataSetCount();
                    }
                    break;

                case "btnDownload":
                    if (txtMinLat.Text.Length > 0
                        && txtMaxLat.Text.Length > 0
                        && txtMinLon.Text.Length > 0
                        && txtMaxLon.Text.Length > 0
                        && lvERDDAP.SelectedItems.Count > 0)
                    {
                        ERDDAPConfigureDownloadForm edf = ERDDAPConfigureDownloadForm.GetInstance(this);
                        if (edf.Visible)
                        {
                            edf.BringToFront();
                        }
                        else
                        {
                            string identifier = lvERDDAP.SelectedItems[0].Name;
                            edf.BeginPosition = ERDDAPMetadataFiles.MetadataFiles[identifier].BeginPosition;
                            edf.EndPosition = ERDDAPMetadataFiles.MetadataFiles[identifier].EndPosition;
                            edf.Dimensions = ERDDAPMetadataFiles.MetadataFiles[identifier].Dimensions;
                            edf.DataExtents = _selectionExtent;
                            edf.GridParameters = ERDDAPMetadataFiles.MetadataFiles[identifier].DataParameters;
                            edf.Credits = ERDDAPMetadataFiles.MetadataFiles[identifier].Credits;
                            edf.Title = ERDDAPMetadataFiles.MetadataFiles[identifier].DataTitle;
                            edf.DataAbstract = ERDDAPMetadataFiles.MetadataFiles[identifier].DataAbstract;
                            edf.LegalConstraint = ERDDAPMetadataFiles.MetadataFiles[identifier].LegalConstraints;
                            edf.GridExtents = ERDDAPMetadataFiles.MetadataFiles[identifier].Extents;
                            edf.URL = ERDDAPMetadataFiles.MetadataFiles[identifier].URL;
                            edf.Identifier = identifier;
                            edf.MetadataFileName = ERDDAPMetadataFiles.MetadataFiles[identifier].MetaDataFilename;
                            edf.Show(this);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Data extents must be provided and data to download must be selected");
                    }
                    break;
            }
        }
    }
}