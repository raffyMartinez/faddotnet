using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FAD3.Mapping.Forms
{
    public partial class ShapefileClassificationSchemeForm : Form
    {
        public double MinimumValue { get; set; }
        public double MaximumValue { get; set; }
        public int NumberOfClasses { get; set; }
        public string ClassificationScheme { get; set; }

        public ShapefileClassificationSchemeForm()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            string msg = "";
            switch (((Button)sender).Name)
            {
                case "btnMakeIntervals":
                    if (rbtnSetNumber.Checked)
                    {
                        if (txtNumber.Text.Length > 0 && int.TryParse(txtNumber.Text, out int v))
                        {
                        }
                        else
                        {
                            msg = "Please provide number of intervals (whole number is expected)";
                        }
                    }
                    else if (rbtnSetSize.Checked)
                    {
                        if (txtSize.Text.Length > 0 && double.TryParse(txtSize.Text, out double vv))
                        {
                        }
                        else
                        {
                            msg = "Please provide size of interval";
                        }
                    }
                    if (msg.Length > 0)
                    {
                        MessageBox.Show(msg, "A value is required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "btnOk":
                    DialogResult = DialogResult.OK;
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            switch (ClassificationScheme)
            {
                case "Jenk's-Fisher's":
                    break;

                case "Equal interval":
                    lblMinValue.Text += MinimumValue.ToString();
                    lblMaxValue.Text += MaximumValue.ToString();
                    tabControl.TabPages["tabEqualInterval"].Select();
                    break;

                case "User defined":
                    tabControl.TabPages["tabUserDefined"].Select();
                    break;
            }
            txtNumber.Enabled = false;
            txtSize.Enabled = false;
        }

        private void OnCheckStateChanged(object sender, EventArgs e)
        {
            txtNumber.Enabled = false;
            txtSize.Enabled = false;
            txtSize.Text = "";
            txtNumber.Text = "";
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                switch (rb.Name)
                {
                    case "rbtnSetNumber":
                        txtNumber.Enabled = true;
                        break;

                    case "rbtnSetSize":
                        txtSize.Enabled = true;
                        break;
                }
            }
        }
    }
}