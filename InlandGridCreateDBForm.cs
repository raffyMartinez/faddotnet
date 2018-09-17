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
    public partial class InlandGridCreateDBForm : Form
    {
        private static InlandGridCreateDBForm _instance;
        private Grid25GenerateForm _parentForm;
        public FishingGrid.fadUTMZone UTMZone { get; set; }

        public static InlandGridCreateDBForm GetInstance(Grid25GenerateForm parent)
        {
            if (_instance == null) _instance = new InlandGridCreateDBForm(parent);
            return _instance;
        }

        public InlandGridCreateDBForm(Grid25GenerateForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    CreateInlandPointDatabase.UTMZone = UTMZone;
                    CreateInlandPointDatabase.StatusUpdate += OnStatusUpdate;
                    CreateInlandPointDatabase.Grid25Shapefile = (Shapefile)global.MappingForm.MapLayersHandler.get_MapLayer(cboGrid25.Text).LayerObject;
                    CreateInlandPointDatabase.LandShapefile = (Shapefile)global.MappingForm.MapLayersHandler.get_MapLayer(cboLandArea.Text).LayerObject;
                    if (cboFastPoly.Text.Length > 0)
                    {
                        CreateInlandPointDatabase.FastPolygonShapefile = (Shapefile)global.MappingForm.MapLayersHandler.get_MapLayer(cboFastPoly.Text).LayerObject;
                    }
                    var saveAs = new SaveFileDialog();
                    saveAs.Filter = "MS Access database *.mdb|*.mdb|All files *.*|*.*";
                    saveAs.FilterIndex = 1;
                    saveAs.Title = "Provide filename for inland grid database";
                    saveAs.ShowDialog();
                    if (saveAs.FileName.Length > 0)
                    {
                        CreateInlandPointDatabase.MapInterActionHandler = global.MappingForm.MapInterActionHandler;
                        CreateInlandPointDatabase.FileName = saveAs.FileName;
                        CreateInlandPointDatabase.Start();
                    }
                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }

        private void OnStatusUpdate(CreateInlandGridEventArgs e)
        {
            txtStatus.Text += $"{e.Status}: {e.GridCount}\r\n";
            if (e.StatusDescription != null && e.StatusDescription.Length > 0)
            {
                txtStatus.Text += $"{e.StatusDescription}\r\n";
            }
        }

        private void OnInlandGridCreateDBForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        private void OnInlandGridCreateDBForm_Load(object sender, EventArgs e)
        {
            txtStatus.Text = "";
            foreach (MapLayer item in global.MappingForm.MapLayersHandler.LayerDictionary.Values)
            {
                cboGrid25.Items.Add(item.Name);
                if (item.Name == "Grid25")
                {
                    cboGrid25.Text = item.Name;
                }
                else
                {
                    cboFastPoly.Items.Add(item.Name);
                    cboLandArea.Items.Add(item.Name);
                }
            }
        }
    }
}