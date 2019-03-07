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
    public partial class DownloadSpatioTemporalDataForm : Form
    {
        private string _ERDDAPMetadataFolder;
        private static DownloadSpatioTemporalDataForm _instance;
        private bool _willUpdateEndPosition;
        private string _identiferToUpdate;
        private Extents _selectionExtent;

        private Dictionary<string, (Dictionary<string, (string unit, string description)> parameters, string title, Extents gridExtents, string url, string credits, string legalConstraint, string dataAbstract, Dictionary<string, (string name, int size, double spacing)> dimensions, DateTime beginPosition, DateTime endPosition, string metadataFileName)> _dictERDDAP =
           new Dictionary<string, (Dictionary<string, (string unit, string description)> parameters, string title, Extents gridExtents, string url, string credits, string legalConstraint, string dataAbstract, Dictionary<string, (string name, int size, double spacing)> dimensions, DateTime beginPosition, DateTime endPosition, string metadataFileName)>();

        public static DownloadSpatioTemporalDataForm GetInstance()
        {
            if (_instance == null) _instance = new DownloadSpatioTemporalDataForm();
            return _instance;
        }

        public DownloadSpatioTemporalDataForm()
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
            switch (((RadioButton)sender).Name)
            {
                case "rbtnUseSelectionBox":
                    global.MappingForm.MapInterActionHandler.EnableMapInteraction = false;
                    MakeGridFromPoints.MapControl = global.MappingForm.MapControl;
                    break;

                case "rbtnUseSelectedLayer":
                    UnsetMap();
                    break;

                case "rbtnManual":
                    UnsetMap();
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);

            //ERDDAP data download
            ERDDAPMetadataHandler.OnMetadataRead += OnERDDAPMetadataRead;
            _ERDDAPMetadataFolder = ERDDAPMetadataHandler.GetMetadataDirectorySetting();
            txtMetadataFolderPath.Text = _ERDDAPMetadataFolder;

            _dictERDDAP.Clear();
            if (_ERDDAPMetadataFolder.Length > 0)
            {
                //the xml metadatafiles in the metadata folder determines what FAD3 can download from ERDDAP
                ERDDAPMetadataHandler.MetadataDirectory = _ERDDAPMetadataFolder;

                //read xml metadata for ERDDAP download
                ERDDAPMetadataHandler.ReadISO9115Metadata();
            }

            lvERDDAP.Columns.Clear();
            lvERDDAP.Columns.Add("Title");
            lvERDDAP.Columns.Add("Data start");
            lvERDDAP.Columns.Add("Data end");
            lvERDDAP.Columns.Add("Frequency");
            lvERDDAP.Columns.Add("Cell size");
            lvERDDAP.ShowItemToolTips = true;
            SizeColumns(lvERDDAP);

            MakeGridFromPoints.OnExtentDefined += OnExtentDefined;
            Text = "Download gridded, oceanographic, spatio-temporal data using ERDDAP";
        }

        private void OnERDDAPMetadataRead(object sender, ERDDAPMetadataReadEventArgs e)
        {
            if (_willUpdateEndPosition)
            {
                if (_dictERDDAP.Remove(_identiferToUpdate))
                {
                    lvERDDAP.Items.Remove(lvERDDAP.Items[_identiferToUpdate]);
                    _identiferToUpdate = "";
                }
            }
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
            _dictERDDAP.Add(e.FileIdentifier, (e.DataParameters, e.DataTitle, ext, e.URL, e.Credit, e.LegalConstraints, e.DataAbstract, e.Dimensions, e.BeginPosition, e.EndPosition, e.MetaDataFilename));
            SizeColumns(lvERDDAP, false);
            _willUpdateEndPosition = false;
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

        public void UpdateEndPosition(string fileName, string identifier, string endPosition)
        {
            _willUpdateEndPosition = true;
            _identiferToUpdate = identifier;
            ERDDAPMetadataHandler.UpdateMetadataEndPosition(fileName, endPosition);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            _dictERDDAP.Clear();
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
                        ERDDAPMetadataHandler.MetadataDirectory = folderBrowser.SelectedPath;
                        ERDDAPMetadataHandler.ReadISO9115Metadata();
                        ERDDAPMetadataHandler.SaveMetadataDirectorySetting(folderBrowser.SelectedPath);
                    }
                    break;

                case "btnDownload":
                    if (txtMinLat.Text.Length > 0
                        && txtMaxLat.Text.Length > 0
                        && txtMinLon.Text.Length > 0
                        && txtMaxLon.Text.Length > 0
                        && lvERDDAP.SelectedItems.Count > 0)
                    {
                        ERDDAPDownloadForm edf = ERDDAPDownloadForm.GetInstance(this);
                        if (edf.Visible)
                        {
                            edf.BringToFront();
                        }
                        else
                        {
                            string identifier = lvERDDAP.SelectedItems[0].Name;
                            edf.BeginPosition = _dictERDDAP[identifier].beginPosition;
                            edf.EndPosition = _dictERDDAP[identifier].endPosition;
                            edf.Dimensions = _dictERDDAP[identifier].dimensions;
                            edf.DataExtents = _selectionExtent;
                            edf.GridParameters = _dictERDDAP[identifier].parameters;
                            edf.Credits = _dictERDDAP[identifier].credits;
                            edf.Title = _dictERDDAP[identifier].title;
                            edf.DataAbstract = _dictERDDAP[identifier].dataAbstract;
                            edf.LegalConstraint = _dictERDDAP[identifier].legalConstraint;
                            edf.GridExtents = _dictERDDAP[identifier].gridExtents;
                            edf.URL = _dictERDDAP[identifier].url;
                            edf.Identifier = identifier;
                            edf.MetadataFileName = _dictERDDAP[identifier].metadataFileName;
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