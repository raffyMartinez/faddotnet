using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapWinGIS;

namespace FAD3
{
    public partial class ShapefileAttributesForm : Form
    {
        private static ShapefileAttributesForm _instance;
        private readonly MapperForm _parentForm;
        public MapInterActionHandler MapInterActionHandler { get; }
        private MapLayer _currentMapLayer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="mapInterActionHandler"></param>
        public ShapefileAttributesForm(MapperForm parent, MapInterActionHandler mapInterActionHandler)
        {
            InitializeComponent();
            MapInterActionHandler = mapInterActionHandler;
            MapInterActionHandler.Selection += OnLayerSelection;
            MapInterActionHandler.MapLayersHandler.CurrentLayer += OnCurrentLayer;
            _parentForm = parent;
            _currentMapLayer = MapInterActionHandler.MapLayersHandler.CurrentMapLayer;
        }

        /// <summary>
        /// Event fired when a layer was selected in the layers form
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void OnCurrentLayer(MapLayersHandler s, LayerEventArg e)
        {
            _currentMapLayer = MapInterActionHandler.MapLayersHandler.CurrentMapLayer;
            DisplayAttributes();
        }

        private void Cleanup()
        {
            _currentMapLayer = null;
        }

        /// <summary>
        /// Resets the back color of all list view items
        /// </summary>
        private void ItemBackColorReset()
        {
            foreach (ListViewItem item in lvAttributes.Items)
            {
                item.BackColor = SystemColors.Window;
            }
        }

        /// <summary>
        /// Event fired when a shape in the current layer was selected. SelectedShapeIndexes is an array that holds the selected shape indexes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLayerSelection(object sender, EventArgs e)
        {
            bool proceed = false;

            lvAttributes.SelectedItems.Clear();
            ItemBackColorReset();
            if (chkRemember.Checked && lvAttributes.Items.Count > 0 && MapInterActionHandler.SelectedShapeIndexes != null)
            {
                Shapefile sf = _currentMapLayer.LayerObject as Shapefile;
                for (int n = 0; n < MapInterActionHandler.SelectedShapeIndexes.Length; n++)
                {
                    var itemName = MapInterActionHandler.SelectedShapeIndexes[n].ToString();
                    if (lvAttributes.Items.ContainsKey(itemName))
                    {
                        lvAttributes.Items[itemName].Selected = true;
                        lvAttributes.Items[itemName].BackColor = SystemColors.Highlight;
                        proceed = true;
                    }
                }
                if (proceed) lvAttributes.SelectedItems[0].EnsureVisible();
            }
            else
            {
                DisplayAttributes();
            }
        }

        public static ShapefileAttributesForm GetInstance(MapperForm parentForm, MapInterActionHandler mapInterActionHandler)
        {
            if (_instance == null) _instance = new ShapefileAttributesForm(parentForm, mapInterActionHandler);
            return _instance;
        }

        /// <summary>
        /// Displays selected shape and shapefile attribute data
        /// </summary>
        private void DisplayAttributes()
        {
            lvAttributes.Visible = false;

            if (_currentMapLayer != null && _currentMapLayer.LayerType == "ShapefileClass")
            {
                Text = $"Shapefile: {_currentMapLayer.Name}";

                //set up the column headers
                Shapefile sf = _currentMapLayer.LayerObject as Shapefile;
                if (!chkRemember.Checked)
                {
                    lvAttributes.Clear();
                    if (sf.NumSelected == 1)
                    {
                        lvAttributes.Columns.Add("ColProperty", "Property");
                        lvAttributes.Columns.Add("colValue", "Value");
                    }
                    else
                    {
                        lvAttributes.Columns.Add("colRow", "Row");
                        for (int n = 0; n < sf.Table.NumFields; n++)
                        {
                            if (sf.Table.Field[n].Alias.Length > 0)
                            {
                                lvAttributes.Columns.Add($"col{sf.Table.Field[n].Name}", sf.Table.Field[n].Alias);
                            }
                            else
                            {
                                lvAttributes.Columns.Add($"col{sf.Table.Field[n].Name}", sf.Table.Field[n].Name);
                            }
                        }
                    }

                    //set all column widths to fit the column text
                    SizeColumns(init: true);

                    //reads the data and adds them to the listview
                    if (sf.NumSelected == 0)
                    {
                        labelShapeFileName.Text = $"Attribute data of the shapefile (n={sf.NumShapes.ToString()})";
                        //show attributes of all shapes in the shapefile
                        for (int row = 0; row < sf.NumShapes; row++)
                        {
                            string[] rowData = new string[sf.NumFields + 1];
                            rowData[0] = (row + 1).ToString();
                            for (int col = 0; col < sf.NumFields; col++)
                            {
                                rowData[col + 1] = sf.CellValue[col, row] == null ? "" : sf.CellValue[col, row].ToString();
                            }
                            ListViewItem lvi = new ListViewItem(rowData);
                            lvAttributes.Items.Add(lvi);
                            lvi.Name = row.ToString();
                        }
                    }
                    else
                    {
                        if (sf.NumSelected == 1 && _currentMapLayer.SelectedIndexes != null)
                        {
                            labelShapeFileName.Text = "Attribute data of the selected shape (n=1)";

                            //show attribute of selected shape and display in 2 column property-value format
                            for (int k = 0; k < sf.NumFields; k++)
                            {
                                ListViewItem lvi = new ListViewItem(new string[] { sf.Table.Field[k].Alias.Length > 0 ? sf.Table.Field[k].Alias : sf.Table.Field[k].Name, sf.CellValue[k, _currentMapLayer.SelectedIndexes[0]].ToString() });
                                lvAttributes.Items.Add(lvi);
                            }
                        }
                        else
                        {
                            if (_currentMapLayer.SelectedIndexes != null)
                            {
                                labelShapeFileName.Text = $"Attribute data of the selected shapes (n={_currentMapLayer.SelectedIndexes.Length.ToString()})";

                                //show attributes of selected shapes
                                for (int row = 0; row < _currentMapLayer.SelectedIndexes.Length; row++)
                                {
                                    string[] rowData = new string[sf.NumFields + 1];
                                    rowData[0] = (row + 1).ToString();
                                    for (int col = 0; col < sf.NumFields; col++)
                                    {
                                        rowData[col + 1] = sf.CellValue[col, _currentMapLayer.SelectedIndexes[row]].ToString();
                                    }
                                    ListViewItem lvi = new ListViewItem(rowData);
                                    lvAttributes.Items.Add(lvi);
                                    lvi.Name = _currentMapLayer.SelectedIndexes[row].ToString();
                                }
                            }
                        }
                    }
                }
                SizeColumns(init: false);
            }
            lvAttributes.Visible = true;
        }

        /// <summary>
        /// Sizes all columns so that it fits the widest column content or the column header content
        /// </summary>
        private void SizeColumns(bool init = true)
        {
            foreach (ColumnHeader c in lvAttributes.Columns)
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

        private void ShapefileAttributesForm_Load(object sender, EventArgs e)
        {
            lvAttributes.MouseDown += OnListViewMouseDown;
            lvAttributes.DoubleClick += OnListViewDoubleClick;
            global.LoadFormSettings(this);
            lvAttributes.With(lv =>
            {
                lv.View = View.Details;
                lv.FullRowSelect = true;
            });
            labelShapeFileName.Text = "No shapefile was selected";
            DisplayAttributes();
        }

        private void OnListViewDoubleClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Equivalent to clicking on a list item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnListViewMouseDown(object sender, MouseEventArgs e)
        {
            var lvi = lvAttributes.HitTest(e.X, e.Y).Item;
            if (lvi != null && _currentMapLayer.LayerType == "ShapefileClass")
            {
                ItemBackColorReset();
                if (int.TryParse(lvi.Name, out int layerHandle))
                {
                    MapInterActionHandler.SelectedShapeIndex = layerHandle;
                }
            }
        }

        private void ShapefileAttributesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MapInterActionHandler.MapLayersHandler.CurrentLayer -= OnCurrentLayer;
            MapInterActionHandler.Selection -= OnLayerSelection;
            global.SaveFormSettings(this);
            _instance = null;
            Cleanup();
        }
    }
}