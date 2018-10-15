using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FAD3.Mapping.Forms;

namespace FAD3
{
    public partial class GraticuleForm : Form
    {
        private static GraticuleForm _instance;
        private MapperForm _parentForm;
        public event EventHandler GraticuleRemoved;

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
                    GraticuleRemoved?.Invoke(this, EventArgs.Empty);
                    Close();
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

        private void OnCheckChange(object sender, EventArgs e)
        {
            var chkBox = (CheckBox)sender;
            switch (chkBox.Name)
            {
                case "chkTitle":
                    lnkTitle.Visible = chkBox.Checked;
                    break;

                case "chkNote":
                    lnkNote.Visible = chkBox.Checked;
                    break;
            }
        }

        private void OnLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            fadMapText mapTextPart = fadMapText.mapTextNone;
            switch (((LinkLabel)sender).Name)
            {
                case "lnkTitle":
                    mapTextPart = fadMapText.mapTextTitle;
                    break;

                case "lnkNote":
                    mapTextPart = fadMapText.mapTextNote;
                    break;
            }

            var configForm = ConfigureMapTextHelper.GetInstance(mapTextPart);
            if (configForm.Visible)
            {
                configForm.BringToFront();
            }
            else
            {
                configForm.Show(this);
            }
        }
    }
}