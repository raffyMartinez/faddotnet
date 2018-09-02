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
        private MapLayer _mapLayer;

        public ShapefileAttributesForm(MapperForm parent, MapInterActionHandler mapInterActionHandler)
        {
            InitializeComponent();
            MapInterActionHandler = mapInterActionHandler;
            mapInterActionHandler.Selection += OnLayerSelection;
            _parentForm = parent;
            _mapLayer = MapInterActionHandler.MapLayersHandler.CurrentMapLayer;
        }

        private void OnLayerSelection(object sender, EventArgs e)
        {
            DisplayAttributes();
        }

        public static ShapefileAttributesForm GetInstance(MapperForm parentForm, MapInterActionHandler mapInterActionHandler)
        {
            if (_instance == null) _instance = new ShapefileAttributesForm(parentForm, mapInterActionHandler);
            return _instance;
        }

        private void DisplayAttributes()
        {
            lvAttributes.Visible = false;
            lvAttributes.Clear();
            if (_mapLayer.LayerType == "ShapefileClass")
            {
                Shapefile sf = _mapLayer.LayerObject as Shapefile;
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
                        lvAttributes.Columns.Add($"col{sf.Table.Field[n].Name}", sf.Table.Field[n].Name);
                    }
                }
                if (sf.NumSelected == 0)
                {
                    //show all attributes
                }
                else
                {
                    if (sf.NumSelected == 1)
                    {
                        //show attribute of selected shape and display in 2 column property-value format
                        for (int k = 0; k < sf.NumFields; k++)
                        {
                            ListViewItem lvi = new ListViewItem(new string[] { sf.Table.Field[k].Name, sf.CellValue[k, MapInterActionHandler.SelectedShapeIndexes[0]].ToString() });
                            lvAttributes.Items.Add(lvi);
                        };
                    }
                    else
                    {
                        for (int row = 0; row < MapInterActionHandler.SelectedShapeIndexes.Length; row++)
                        {
                            string[] rowData = new string[sf.NumFields + 1];
                            rowData[0] = (row + 1).ToString();
                            for (int col = 0; col < sf.NumFields; col++)
                            {
                                rowData[col + 1] = sf.CellValue[col, MapInterActionHandler.SelectedShapeIndexes[row]].ToString();
                            }
                            ListViewItem lvi = new ListViewItem(rowData);
                            lvAttributes.Items.Add(lvi);
                        }
                    }
                }
            }
            lvAttributes.Visible = true;
        }

        private void ShapefileAttributesForm_Load(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);
            lvAttributes.With(lv =>
            {
                lv.View = View.Details;
                lv.FullRowSelect = true;
            });
            DisplayAttributes();
        }

        private void ShapefileAttributesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
            _instance = null;
        }

        private void OnGridAttributes_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
        }
    }
}