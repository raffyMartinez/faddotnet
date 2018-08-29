using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FAD3
{
    public partial class Grid25GenerateForm : Form
    {
        private static Grid25GenerateForm _instance;
        private bool _formCloseDone = false;
        private MapForm _parentForm;
        private Grid25MajorGrid _grid25MajorGrid;
        private Dictionary<string, uint> _labelAndGridProperties = new Dictionary<string, uint>();

        public static Grid25GenerateForm GetInstance(MapForm parent)
        {
            if (_instance == null) _instance = new Grid25GenerateForm(parent);
            return _instance;
        }

        public Grid25GenerateForm(MapForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
            _grid25MajorGrid = parent.grid25MajorGrid;
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
                        _grid25MajorGrid.LabelAndGridProperties = _labelAndGridProperties;
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
            global.SaveFormSettings(this);
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
    }
}