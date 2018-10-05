using MapWinGIS;
using System;
using System.IO;
using System.Windows.Forms;

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

                    var fileOpen = new OpenFileDialog()
                    {
                        CheckFileExists = false,
                        DefaultExt = "mdb",
                        AddExtension = true,
                        Filter = "MS Access database *.mdb|*.mdb|All files *.*|*.*",
                        FilterIndex = 1,
                        Title = "Provide filename for inland grid database",
                    };

                    fileOpen.ShowDialog();
                    if (fileOpen.FileName.Length > 0)
                    {
                        var proceed = true;
                        if (!File.Exists(fileOpen.FileName))
                        {
                            proceed = false;
                            var msg = "The file does not exist. Do you want to create a new database file?";
                            var result = MessageBox.Show(msg, "Create database file", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            proceed = result == DialogResult.OK;
                        }
                        if (proceed)
                        {
                            CreateInlandPointDatabase.MapInterActionHandler = global.MappingForm.MapInterActionHandler;
                            CreateInlandPointDatabase.FileName = fileOpen.FileName;
                            CreateInlandPointDatabase.Start();
                        }
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
            _parentForm = null;
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
                    cboLandArea.Items.Add(item.Name);
                }
            }
        }
    }
}