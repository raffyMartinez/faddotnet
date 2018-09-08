using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks;

namespace FAD3
{
    public partial class Grid25GenerateForm : Form
    {
        private static Grid25GenerateForm _instance;
        private bool _formCloseDone = false;
        private MapperForm _parentForm;
        private Grid25MajorGrid _grid25MajorGrid;
        private Dictionary<string, uint> _labelAndGridProperties = new Dictionary<string, uint>();

        public Grid25MajorGrid Grid25MajorGrid
        {
            get { return _grid25MajorGrid; }
        }

        public static Grid25GenerateForm GetInstance(MapperForm parent)
        {
            if (_instance == null) _instance = new Grid25GenerateForm(parent);
            return _instance;
        }

        public Grid25GenerateForm(MapperForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
            _grid25MajorGrid = parent.Grid25MajorGrid;
        }

        private uint ColorToUInt(Color clr)
        {
            return (uint)(clr.R + (clr.G << 8) + (clr.B << 16));
        }

        /// <summary>
        /// fills a dictionary with label and grid line properties
        /// </summary>
        private void SetupDictionary()
        {
            _labelAndGridProperties.Clear();
            _labelAndGridProperties.Add("minorGridLabelDistance", uint.Parse(txtMinorGridLabelDistance.Text));
            _labelAndGridProperties.Add("minorGridLabelSize", uint.Parse(txtMinorGridLabelSize.Text));
            _labelAndGridProperties.Add("majorGridLabelSize", uint.Parse(txtMajorGridLabelSize.Text));
            _labelAndGridProperties.Add("borderThickness", uint.Parse(txtBorderThickness.Text));
            _labelAndGridProperties.Add("majorGridThickness", uint.Parse(txtMajorGridThickness.Text));
            _labelAndGridProperties.Add("minorGridThickness", uint.Parse(txtMinorGridThickness.Text));
            _labelAndGridProperties.Add("minorGridLabelColor", ColorToUInt(shapeMinorGridLabelColor.FillColor));
            _labelAndGridProperties.Add("majorGridLabelColor", ColorToUInt(shapeMajorGridLabelColor.FillColor));
            _labelAndGridProperties.Add("borderColor", ColorToUInt(shapeBorderColor.FillColor));
            _labelAndGridProperties.Add("majorGridLineColor", ColorToUInt(shapeMajorGridLineColor.FillColor));
            _labelAndGridProperties.Add("minorGridLineColor", ColorToUInt(shapeMinorGridLineColor.FillColor));
            _labelAndGridProperties.Add("minorGridLabelFontBold", (uint)(chkBold.Checked ? 1 : 0));
            _labelAndGridProperties.Add("minorGridLabelWrapped", (uint)(chkWrapLabels.Checked ? 1 : 0));
        }

        private void OnButtons_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                //updates the grid labels with changes inputted into the properies dictionary
                case "buttonLabel":
                    if (_grid25MajorGrid.grid25LabelManager != null && _grid25MajorGrid.grid25LabelManager.Grid25Labels.NumShapes > 0)
                    {
                        SetupDictionary();
                        _grid25MajorGrid.RedoLabels(txtMapTitle.Text, _labelAndGridProperties);
                    }
                    break;

                //initiates the construction of minor grids and labels. Map cursor is changed to the grid icon to signify
                //that creating minor grids and labels can proceed.
                case "buttonGrid":
                    if (txtMinorGridLabelDistance.Text.Length > 0 && txtMinorGridLabelSize.Text.Length > 0)
                    {
                        SetupDictionary();

                        if (_grid25MajorGrid.DefineMinorGrid((int)((Bitmap)imList.Images["gridCursor"]).GetHicon()))
                        {
                            _grid25MajorGrid.LabelAndGridProperties = _labelAndGridProperties;
                            _grid25MajorGrid.MapTitle = txtMapTitle.Text;
                        }
                        else
                        {
                            MessageBox.Show("No selection in major grid", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Provide minor grid label distance and minor grid label size", "Validation error",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "buttonClear":
                    _grid25MajorGrid.ClearSelectedGrids();
                    _parentForm.SetCursorToSelect();
                    break;

                case "buttonClose":
                    Close();
                    break;
            }
        }

        private void Grid25GenerateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_formCloseDone)
            {
                _formCloseDone = true;
                if (global.MappingForm != null) global.MappingForm.Close();
            }
            _parentForm = null;
            _instance = null;
            _labelAndGridProperties.Clear();

            global.SaveFormSettings(this);
            global.MappingMode = global.fad3MappingMode.defaultMode;
        }

        private void Grid25GenerateForm_Load(object sender, EventArgs e)
        {
            txtMinorGridLabelDistance.Text = "1000";
            txtMinorGridLabelSize.Text = "8";
            txtMajorGridLabelSize.Text = "12";
            txtBorderThickness.Text = "2";
            txtMajorGridThickness.Text = "2";
            txtMinorGridThickness.Text = "1";
            chkLeft.Checked = true;
            chkTop.Checked = true;
            chkRight.Checked = true;
            chkBottom.Checked = true;
            global.LoadFormSettings(this, true);
        }

        private void OnShapeColor_DoubleClick(object sender, EventArgs e)
        {
            var cd = new ColorDialog
            {
                AllowFullOpen = true,
                AnyColor = true,
                FullOpen = true,
                SolidColorOnly = true,
            };
            cd.ShowDialog();
            ((RectangleShape)sender).FillColor = cd.Color;
        }

        public string GetSavedGridsFolder()
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

        private void SetSavedGridsFolder(string folderPath)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            reg_key.SetValue("FolderSavedGrids", folderPath);
        }

        private void OnToolbar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "tsButtonMBRs":
                    break;

                case "tsButtonRetrieve":
                    var folderBrowser = new FolderBrowserDialog();
                    folderBrowser.ShowNewFolderButton = true;
                    folderBrowser.SelectedPath = GetSavedGridsFolder();
                    folderBrowser.Description = "Locate folder containing saved fishing ground grid maps";
                    DialogResult result = FolderBrowserLauncher.ShowFolderBrowser(folderBrowser);
                    if (result == DialogResult.OK)
                    {
                        SetSavedGridsFolder(folderBrowser.SelectedPath);
                    }
                    break;

                case "tsButtonSaveImage":
                case "tsButtonSaveShapefile":
                    var saveForm = Grid25SaveForm.GetInstance(this);
                    if (!saveForm.Visible)
                    {
                        saveForm.Show(this);
                        saveForm.SaveType(e.ClickedItem.Name == "tsButtonSaveShapefile");
                    }
                    else
                    {
                        saveForm.BringToFront();
                    }
                    break;

                case "tsButtonFitMap":
                    _grid25MajorGrid.FitGridToMap();
                    break;

                case "tsButtonExit":
                    Close();
                    break;
            }
        }
    }
}