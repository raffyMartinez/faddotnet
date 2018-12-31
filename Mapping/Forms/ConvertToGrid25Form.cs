using FAD3.Database.Classes;
using MapWinGIS;
using System;
using System.IO;
using System.Windows.Forms;

namespace FAD3
{
    public partial class ConvertToGrid25Form : Form
    {
        private static ConvertToGrid25Form _instance;
        public MapLayer MapLayer { get; set; }

        public static ConvertToGrid25Form GetInstance()
        {
            if (_instance == null) _instance = new ConvertToGrid25Form();
            return _instance;
        }

        public ConvertToGrid25Form()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    if (MapLayer?.LayerType == "ShapefileClass")
                    {
                        var sfSource = MapLayer.LayerObject as Shapefile;
                        if (sfSource?.ShapefileType == ShpfileType.SHP_POINT)
                        {
                            fad3ActionType inlandAction = fad3ActionType.atIgnore;
                            if (rbInlandNote.Checked)
                            {
                                inlandAction = fad3ActionType.atTakeNote;
                            }
                            else if (rbInlandRemove.Checked)
                            {
                                inlandAction = fad3ActionType.atRemove;
                            }

                            var sfConverted = ShapefileLayerHelper.ConvertToGrid25(sfSource, global.MappingForm.UTMZone, inlandAction, includeCoordinates: chkIncludeCoordinates.Checked);
                            var name = Path.GetFileNameWithoutExtension(MapLayer.FileName);
                            if (global.MappingForm.MapLayersHandler.AddLayer(sfConverted, name + "_converted") >= 0)
                            {
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("Was not able to convert to Grid25 point shapefile", "Conversion error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnSelectLimitMap":
                    break;
            }
        }

        private void OnConvertToGrid25Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }

        private void OnmenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (((ToolStripItem)sender).Name)
            {
                case "itemGrid25Field":
                    break;

                case "itemNewGrid25Field":
                    break;
            }
        }

        private void OnConvertToGrid25Form_Load(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
            lblTitle.Text = $"Layer to convert: {MapLayer.Name}";
        }

        private void OnButtonsCheckedChange(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            switch (rb.Name)
            {
                case "rbOutsideIgnore":
                    btnSelectMap.Enabled = !rb.Checked;
                    break;

                case "rbOutsideNote":
                case "rbOutsideRemove":
                    btnSelectMap.Enabled = rb.Checked;
                    break;
            }
        }
    }
}