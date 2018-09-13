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
    public partial class GraticuleForm : Form
    {
        private static GraticuleForm _instance;
        private MapperForm _parentForm;

        public static GraticuleForm GetInstance(MapperForm parent)
        {
            if (_instance == null) _instance = new GraticuleForm(parent);
            return _instance;
        }

        public GraticuleForm(MapperForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
        }

        private void ShowGraticule()
        {
            _parentForm.Graticule.Configure(txtName.Text, int.Parse(txtLabelSize.Text), int.Parse(txtNumberOfGridlines.Text),
                                int.Parse(txtBordeWidth.Text), int.Parse(txtGridlineWidth.Text), chkShowGrid.Checked,
                                chkBold.Checked, chkLeft.Checked, chkRight.Checked, chkTop.Checked, chkBottom.Checked);
            _parentForm.Graticule.ShowGraticule();
        }

        private void OnButton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    ShowGraticule();
                    Close();
                    break;

                case "btnApply":
                    ShowGraticule();
                    break;

                case "btnCancel":
                    Close();
                    break;

                case "btnRemove":
                    break;
            }
        }

        private void GraticuleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        private void OnGraticuleForm_Load(object sender, EventArgs e)
        {
            txtName.Text = "Graticule";
            txtLabelSize.Text = "8";
            txtBordeWidth.Text = "2";
            txtGridlineWidth.Text = "1";
            txtNumberOfGridlines.Text = "5";
            chkBottom.Checked = true;
            chkTop.Checked = true;
            chkLeft.Checked = true;
            chkRight.Checked = true;
            chkShowGrid.Checked = true;
        }
    }
}