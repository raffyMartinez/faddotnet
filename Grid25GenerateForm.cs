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

                        //pass label size, label distance, and icon hanlde to majorgrid
                        if (_grid25MajorGrid.DefineMinorGrid(int.Parse(txtMinorGridLabelDistance.Text),
                            int.Parse(txtMinorGridLabelSize.Text), listSidesToLabel, (int)((Bitmap)imList.Images["gridCursor"]).GetHicon()))
                        {
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
            chkLeft.Checked = true;
            chkTop.Checked = true;
            chkRight.Checked = true;
            chkBottom.Checked = true;
            global.LoadFormSettings(this, true);
        }
    }
}