using ISO_Classes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FAD3
{
    public partial class LandingSiteFromKMLForm : Form
    {
        private static LandingSiteFromKMLForm _instance;
        private TargetArea _targetArea;
        private LandingSiteFromKML _landingSiteFromKML;
        public string LandingSiteName { get; set; }
        public int LandingSiteMunicipalityNumber { get; set; }
        public string LandingSiteMunicipalityName { get; set; }
        private Dictionary<int, (string LSName, string Municipality, string Province, int MunicipalityNumber)> _lsDictionary = new Dictionary<int, (string, string, string, int)>();

        public static LandingSiteFromKMLForm GetInstance(TargetArea targetArea)
        {
            if (_instance == null) _instance = new LandingSiteFromKMLForm(targetArea);
            return _instance;
        }

        public LandingSiteFromKMLForm(TargetArea targetArea)
        {
            InitializeComponent();
            _targetArea = targetArea;
            _landingSiteFromKML = new LandingSiteFromKML();
            _landingSiteFromKML.OnLandingSiteRetrieved += OnLandingSiteRetrieved;
        }

        private void OnLandingSiteRetrieved(LandingSiteFromKML s, Landingsite e)
        {
            var lvi = lvLS.Items.Add(e.LandingSiteName);
            var municipalityFound = false;
            foreach (var item in _lsDictionary.Values)
            {
                if (item.LSName == e.LandingSiteName)
                {
                    var subItem1 = lvi.SubItems.Add($"{item.Municipality}, {item.Province}");
                    subItem1.Tag = item.MunicipalityNumber;
                    municipalityFound = true;
                    break;
                }
            }
            if (!municipalityFound)
            {
                lvi.SubItems.Add("");
            }

            var coordinate = new Coordinate((float)e.yCoord, (float)e.xCoord);
            var subItem = lvi.SubItems.Add(coordinate.ToString(false, global.CoordinateFormatCode));
            subItem.Tag = e.xCoord;

            subItem = lvi.SubItems.Add(coordinate.ToString(true, global.CoordinateFormatCode));
            subItem.Tag = e.yCoord;

            lvi.SubItems.Add(e.IsInsideTargetArea ? "yes" : "no");
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    List<string> itemsNotAdded = new List<string>();
                    bool isNew = false;
                    var landingSite = new Landingsite();
                    Dictionary<string, string> LSData = new Dictionary<string, string>();
                    foreach (ListViewItem item in lvLS.Items)
                    {
                        if (item.Checked && item.SubItems[4].Text == "yes")
                        {
                            if (item.SubItems[1].Text.Length > 0)
                            {
                                var landingSiteName = item.Text;
                                LSData.Clear();
                                LSData.Add("LSName", landingSiteName);
                                LSData.Add("MunNo", item.SubItems[1].Tag.ToString());
                                LSData.Add("AOIGuid", _targetArea.TargetAreaGuid);
                                if (_targetArea.LandingSiteExists(landingSiteName))
                                {
                                    isNew = false;
                                    landingSite = TargetArea.LandingSiteFromName(landingSiteName, _targetArea.TargetAreaGuid);
                                    LSData.Add("LSGUID", landingSite.LandingSiteGUID);
                                    //landingSite
                                }
                                else
                                {
                                    var coordinate = new Coordinate(float.Parse(item.SubItems[3].Tag.ToString()), float.Parse(item.SubItems[2].Tag.ToString()));
                                    LSData.Add("HasCoordinate", true.ToString());
                                    landingSite.Coordinate = coordinate;
                                    isNew = true;
                                    LSData.Add("LSGUID", Guid.NewGuid().ToString());
                                }
                                if (landingSite.UpdateData(isNew, LSData))
                                {
                                    if (isNew)
                                    {
                                        global.mainForm.NewLandingSite(LSData["LSName"], LSData["LSGUID"], _targetArea.TargetAreaGuid);
                                    }
                                    else
                                    {
                                        global.mainForm.RefreshLV("landing_site");
                                    }
                                }
                            }
                            else
                            {
                                itemsNotAdded.Add(item.Text);
                            }
                        }
                    }
                    if (itemsNotAdded.Count > 0)
                    {
                        string notAdded = "";
                        for (int x = 0; x < itemsNotAdded.Count; x++)
                        {
                            notAdded += $"{itemsNotAdded[x]}, ";
                        }
                        notAdded = notAdded.Trim(new char[] { ' ', ',' });
                        DialogResult dr = MessageBox.Show($"The municipality of the landing sites is missing:\r\n{notAdded}", "Municipality is not provided", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
                        if (dr == DialogResult.Cancel)
                        {
                            Close();
                        }
                    }
                    else
                    {
                        Close();
                    }
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnKML":
                    var fileOpen = new OpenFileDialog
                    {
                        Title = "Open KML file",
                        Filter = "KML file|*.kml|All files|*.*",
                        FilterIndex = 1,
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    };
                    fileOpen.ShowDialog();
                    if (fileOpen.FileName.Length > 0)
                    {
                        _landingSiteFromKML.TargetArea = _targetArea;
                        _lsDictionary.Clear();
                        _lsDictionary = Landingsite.LandingSiteDictionary(_targetArea.TargetAreaGuid);
                        _landingSiteFromKML.KMLFile = fileOpen.FileName;
                        _landingSiteFromKML.ParseLandingSites();
                        SizeColumns(false);
                    }
                    break;
            }
        }

        private void LandingSiteFromKMLForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        /// <summary>
        /// Sizes all columns so that it fits the widest column content or the column header content
        /// </summary>
        private void SizeColumns(bool init = true)
        {
            foreach (ColumnHeader c in lvLS.Columns)
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

        private void OnFormLoad(object sender, EventArgs e)
        {
            lvLS.Clear();
            lvLS.Columns.Add("Name");
            lvLS.Columns.Add("Municipality");
            lvLS.Columns.Add("Longitude");
            lvLS.Columns.Add("Latitude");
            lvLS.Columns.Add("Inside target area");
            lvLS.View = View.Details;
            lvLS.FullRowSelect = true;
            lvLS.CheckBoxes = true;
            SizeColumns();
        }

        private void OnItemMouseUp(object sender, MouseEventArgs e)
        {
            var hitItem = lvLS.HitTest(e.X, e.Y);
            if (hitItem.Item != null && e.Button == MouseButtons.Right)
            {
                contextMenuLV.Show(lvLS, e.X, e.Y);
            }
        }

        private void OnContextMenuClick(object sender, ToolStripItemClickedEventArgs e)
        {
            using (var lsForm = new LandingSiteForm(_targetArea, this, lvLS.SelectedItems[0].Text, double.Parse(lvLS.SelectedItems[0].SubItems[2].Tag.ToString()), double.Parse(lvLS.SelectedItems[0].SubItems[3].Tag.ToString()), false, true))
            {
                if (lsForm.ShowDialog(this) == DialogResult.OK)
                {
                    //lvLS.SelectedItems[0].SubItems[1].Text = LandingSiteMunicipalityName;
                    //lvLS.SelectedItems[0].SubItems[1].Tag = LandingSiteMunicipalityNumber;
                    lvLS.SelectedItems[0].SubItems[1].Text = lsForm.LandingSiteMunicipalityName;
                    lvLS.SelectedItems[0].SubItems[1].Tag = lsForm.LandingSiteMunicipalityNumber;
                }
            }
        }
    }
}