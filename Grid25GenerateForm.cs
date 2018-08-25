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
                    if (_grid25MajorGrid.SelectedShapeGridNumbers.Count > 0
                        && txtMinorGridLabelDistance.Text.Length > 0
                        && txtMinorGridLabelSize.Text.Length > 0)
                    {
                    }
                    else
                    {
                        MessageBox.Show("No selection in major grid", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }
    }
}