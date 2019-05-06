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
using FAD3.Database.Forms;
using MapWinGIS;
using ISO_Classes;

namespace FAD3.Mapping.Forms
{
    public partial class LocationsCoordinateForm : Form
    {
        private Dictionary<int, Coordinate> _dictCoordinate = new Dictionary<int, Coordinate>();
        private string _inventoryProjectGuid;
        private string _inventoryProjectName;
        private CoordinateEntryForm _coordinateEntryForm;
        private string _treeLevel;

        public string InventoryProjectGuid
        {
            get { return _inventoryProjectGuid; }
            set
            {
                _inventoryProjectGuid = value;
                ReadCoordinates();
            }
        }

        public string InventoryProjectname
        {
            get { return _inventoryProjectName; }
            set
            {
                _inventoryProjectName = value;
                Text = $"Coordinates of locations in {_inventoryProjectName}";
                lblTitle.Text = Text;
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

        public LocationsCoordinateForm(string inventoryProjectGuid, string inventoryProjectName)
        {
            InitializeComponent();
            _inventoryProjectGuid = inventoryProjectGuid;
            InventoryProjectname = inventoryProjectName;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.MapperOpen += OnMapperIsOpen;
            global.MapperClosed += OnMapperIsClosed;
            global.LoadFormSettings(this);
            lvCoordinates.Columns.Add("Location");
            lvCoordinates.Columns.Add("Longitude");
            lvCoordinates.Columns.Add("Latitude");
            lvCoordinates.View = View.Details;
            lvCoordinates.HideSelection = false;
            lvCoordinates.FullRowSelect = true;
            SizeColumns(lvCoordinates);
            ReadCoordinates();
        }

        private void OnMapperIsClosed(object sender, EventArgs e)
        {
            chkShowOnMap.Enabled = false;
            chkShowOnMap.Checked = false;
        }

        private void OnMapperIsOpen(object sender, EventArgs e)
        {
            chkShowOnMap.Enabled = true;
        }

        private void ReadCoordinates()
        {
            lvCoordinates.Visible = false;
            lvCoordinates.Items.Clear();
            BarangayMunicipalityCoordinateHelper bmch = new BarangayMunicipalityCoordinateHelper();
            var coords = bmch.ReadCoordinates(_inventoryProjectGuid);
            foreach (var item in coords)
            {
                switch (item.LGULevel)
                {
                    case "barangay":
                        break;

                    case "municipality":
                        ListViewItem lvi = lvCoordinates.Items.Add(item.LGUNumber.ToString(), $"{item.LGUName}, {item.ProvinceName}", null);
                        if (item.HasCoordinate)
                        {
                            _dictCoordinate.Add(item.LGUNumber, item.Coordinate);
                            var c = item.Coordinate.ToString(global.CoordinateFormatCode).Split(' ');

                            lvi.SubItems.Add(c[1].Trim());
                            lvi.SubItems.Add(c[0].Trim());
                        }
                        else
                        {
                            lvi.SubItems.Add("");
                            lvi.SubItems.Add("");
                        }
                        lvi.Tag = "municipality";
                        break;
                }
            }
            SizeColumns(lvCoordinates, false);
            lvCoordinates.Visible = true;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnMapPoints":

                    break;

                case "btnClose":
                    Close();
                    break;
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
            global.MapperClosed -= OnMapperIsClosed;
            global.MapperOpen -= OnMapperIsOpen;
            _coordinateEntryForm = null;
        }

        private void OnListMouseDown(object sender, MouseEventArgs e)
        {
            chkShowOnMap.Enabled = global.MapIsOpen;
            ListViewHitTestInfo hitTest = lvCoordinates.HitTest(e.X, e.Y);
            _treeLevel = hitTest.Item.Tag.ToString();
            if (_coordinateEntryForm != null)
            {
                _coordinateEntryForm.TreeLevel = _treeLevel;
                _coordinateEntryForm.SetLocation(hitTest.Item.Text, int.Parse(hitTest.Item.Name));
            }

            if (chkShowOnMap.Checked
                && global.MapIsOpen
                && hitTest.Item.SubItems[1].Text.Length > 0
                && hitTest.Item.SubItems[2].Text.Length > 0
               )
            {
                Shapefile sf = new Shapefile();
                sf.GeoProjection = global.MappingForm.MapControl.GeoProjection;
                if (sf.CreateNew("", ShpfileType.SHP_POINT))
                {
                    var ifldLocation = sf.EditAddField("Location", FieldType.STRING_FIELD, 0, 50);
                    Shape shp = new Shape();
                    if (shp.Create(ShpfileType.SHP_POINT))
                    {
                        float y = _dictCoordinate[int.Parse(hitTest.Item.Name)].Latitude;
                        float x = _dictCoordinate[int.Parse(hitTest.Item.Name)].Longitude;
                        var iPt = shp.AddPoint(x, y);
                        if (iPt >= 0)
                        {
                            var iShp = sf.EditAddShape(shp);
                            if (iShp >= 0)
                            {
                                sf.EditCellValue(ifldLocation, iShp, hitTest.Item.Text);
                                sf.DefaultDrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                                sf.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Red);
                                sf.DefaultDrawingOptions.LineVisible = false;
                                sf.DefaultDrawingOptions.PointSize = 8;
                                sf.CollisionMode = tkCollisionMode.AllowCollisions;
                                global.MappingForm.MapLayersHandler.AddLayer(sf, "Location", isVisible: true, uniqueLayer: true);
                            }
                        }
                    }
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                menuDropDown.Items.Clear();
                var item = menuDropDown.Items.Add("Set coordinate");
                item.Name = "itemSetCoordinate";
                item.Enabled = global.MapIsOpen;

                item = menuDropDown.Items.Add("Map coordinates");
                item.Name = "itemMapCoordinates";
                item.Enabled = global.MapIsOpen;

                item = menuDropDown.Items.Add("Copy text");
                item.Name = "itemCopyText";

                menuDropDown.Show(Cursor.Position);
            }
        }

        private void OnDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "itemCopyText":
                    StringBuilder copyText = new StringBuilder();
                    string cols = "";
                    foreach (ColumnHeader col in lvCoordinates.Columns)
                    {
                        cols += $"{col.Text}\t";
                    }
                    copyText.Append($"{cols.TrimEnd()}\r\n");

                    foreach (ListViewItem item in lvCoordinates.Items)
                    {
                        copyText.Append(item.Text);
                        for (int n = 1; n < item.SubItems.Count; n++)
                        {
                            copyText.Append($"\t{item.SubItems[n]?.Text}");
                        }
                        copyText.Append("\r\n");
                    }
                    Clipboard.SetText(copyText.ToString());
                    break;

                case "itemSetCoordinate":
                    _coordinateEntryForm = new CoordinateEntryForm(global.MappingForm.MapControl, global.MappingForm.MapLayersHandler);
                    _coordinateEntryForm.CoordinateAvailable += OnCoordinateAvailable;
                    _coordinateEntryForm.CoordinateFormClosed += OnCoordinateFormClosed;
                    _coordinateEntryForm.TreeLevel = "municipality";
                    _coordinateEntryForm.SetLocation(lvCoordinates.SelectedItems[0].Text, int.Parse(lvCoordinates.SelectedItems[0].Name));
                    _coordinateEntryForm.Show(this);
                    break;
            }
        }

        private void OnCoordinateFormClosed(object sender, EventArgs e)
        {
            _coordinateEntryForm = null;
        }

        private void OnCoordinateAvailable(object sender, EventArgs e)
        {
            var coord = _coordinateEntryForm.Coordinate;
            BarangayMunicipalityCoordinateHelper bmsc = new BarangayMunicipalityCoordinateHelper(_treeLevel, lvCoordinates.SelectedItems[0].Text);
            bmsc.Coordinate = coord;
            switch (_treeLevel)
            {
                case "barangay":
                    //bmsc.LGUNumber = int.Parse(treeInventory.SelectedNode.Parent.Name);
                    break;

                case "municipality":
                    bmsc.LGUNumber = int.Parse(lvCoordinates.SelectedItems[0].Name);
                    break;
            }
            bmsc.SetCoordinate();
            var c = bmsc.Coordinate;
            var coordinate = c.ToString(global.CoordinateFormatCode).Split(' ');
            lvCoordinates.Items[bmsc.LGUNumber.ToString()].SubItems[1].Text = coordinate[1];
            lvCoordinates.Items[bmsc.LGUNumber.ToString()].SubItems[2].Text = coordinate[0];
            if (!_dictCoordinate.ContainsKey(bmsc.LGUNumber))
            {
                _dictCoordinate.Add(bmsc.LGUNumber, bmsc.Coordinate);
            }
        }
    }
}