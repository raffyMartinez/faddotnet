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
        private bool _definingMinorGrids = false;
        private Dictionary<string, uint> labelAndGridProperties = new Dictionary<string, uint>();

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

        private uint ColorToUInt(Color c)
        {
            return (uint)(((c.A << 24) | (c.R << 16) | (c.G << 8) | c.B) & 0xffffffffL);
        }

        private void SetupDictionary()
        {
            labelAndGridProperties.Clear();
            labelAndGridProperties.Add("minorGridLabelDistance", uint.Parse(txtMinorGridLabelDistance.Text));
            labelAndGridProperties.Add("minorGridLabelSize", uint.Parse(txtMinorGridLabelSize.Text));
            labelAndGridProperties.Add("majorGridLabelSize", uint.Parse(txtMajorGridLabelSize.Text));
            labelAndGridProperties.Add("borderThickness", uint.Parse(txtBorderThickness.Text));
            labelAndGridProperties.Add("majorGridThickness", uint.Parse(txtMajorGridThickness.Text));
            labelAndGridProperties.Add("minorGridThickness", uint.Parse(txtMinorGridThickness.Text));
            labelAndGridProperties.Add("minorGridLabelColor", ColorToUInt(shapeMinorGridLabelColor.BackColor));
            labelAndGridProperties.Add("majorGridLabelColor", ColorToUInt(shapeMajorGridLabelColor.BackColor));
            labelAndGridProperties.Add("borderColor", ColorToUInt(shapeBorderColor.BackColor));
            labelAndGridProperties.Add("majorGridLineColor", ColorToUInt(shapeMajorGridLineColor.BackColor));
            labelAndGridProperties.Add("minorGridLineColor", ColorToUInt(shapeMinorGridLineColor.BackColor));
            labelAndGridProperties.Add("minorGridLabelFontBold", (uint)(chkBold.Checked ? 1 : 0));
            labelAndGridProperties.Add("minorGridLabelWrapped", (uint)(chkWrapLabels.Checked ? 1 : 0));
        }

        private void OnButtons_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonGrid":
                    _definingMinorGrids = false;
                    if (txtMinorGridLabelDistance.Text.Length > 0
                        && txtMinorGridLabelSize.Text.Length > 0)
                    {
                        var listSidesToLabel = new List<string>();
                        if (chkLeft.Checked) listSidesToLabel.Add("left");
                        if (chkTop.Checked) listSidesToLabel.Add("top");
                        if (chkRight.Checked) listSidesToLabel.Add("right");
                        if (chkBottom.Checked) listSidesToLabel.Add("bottom");
                        if (listSidesToLabel.Count == 0) listSidesToLabel.Add("none");

                        SetupDictionary();

                        if (_grid25MajorGrid.DefineMinorGrid(int.Parse(txtMinorGridLabelDistance.Text),
                            int.Parse(txtMinorGridLabelSize.Text), listSidesToLabel, (int)((Bitmap)imList.Images["gridCursor"]).GetHicon()))
                        {
                            _grid25MajorGrid.LabelAndGridProperties = labelAndGridProperties;

                            _definingMinorGrids = true;
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