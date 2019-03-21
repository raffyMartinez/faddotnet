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
        public int NumberOfCategories { get { return _intervals.Count; } }
        public double MinimumValue { get; set; }
        public double MaximumValue { get; set; }
        public string ClassificationScheme { get; set; }
        public string ParameterToClassify { get; set; }
        private List<double> _intervals;
        public List<double> Intervals { get { return _intervals; } }

        public ShapefileClassificationSchemeForm()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnMakeIntervals":
                    string msg = "";
                    double startTarget = 0;
                    double stepSize = 0;
                    double endTarget = 0;
                    int steps = 0;
                    txtIntervals.Text = "";
                    if (txtStartValue.Text.Length > 0 && double.TryParse(txtStartValue.Text, out double st))
                    {
                        startTarget = st;
                    }
                    else
                    {
                        msg += "Please provide start target\r\n";
                    }

                    if (txtSize.Text.Length > 0 && double.TryParse(txtSize.Text, out double vv))
                    {
                        stepSize = vv;
                    }
                    else
                    {
                        msg += "Please provide size of step\r\n";
                    }

                    if (rbtnEndTarget.Checked)
                    {
                        if (txtEndTarget.Text.Length > 0 && double.TryParse(txtEndTarget.Text, out double et))
                        {
                            endTarget = et;
                            steps = (int)((endTarget - startTarget) / stepSize);
                        }
                        else
                        {
                            msg += "Provide end target";
                        }
                    }
                    else
                    {
                        if (txtNumberOfSteps.Text.Length > 0 && int.TryParse(txtNumberOfSteps.Text, out int ns))
                        {
                            steps = ns;
                        }
                        else
                        {
                            msg += "Please provide number of steps (whole number is expected)\r\n";
                        }
                    }

                    if (msg.Length > 0)
                    {
                        MessageBox.Show(msg, "One or more values are required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        _intervals = new List<double>();
                        double stepValue = startTarget;
                        _intervals.Add(stepValue);
                        for (int n = 0; n < steps; n++)
                        {
                            stepValue += stepSize;
                            _intervals.Add(stepValue);
                        }
                        if (rbtnEndTarget.Checked && _intervals.Last() < endTarget)
                        {
                            stepValue += stepSize;
                            _intervals.Add(stepValue);
                        }
                        string intervalList = "";
                        foreach (double item in _intervals)
                        {
                            intervalList += $"{item.ToString()}\r\n";
                        }
                        txtIntervals.Text = intervalList;
                    }
                    break;

                case "btnOk":
                    if (txtIntervals.Text.Length > 0)
                    {
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Provide list of intervals", "Intervals not found", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                    }
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
            Text = ParameterToClassify;
            txtEndTarget.Enabled = false;
            txtNumberOfSteps.Enabled = false;
        }

        private void OnCheckChanged(object sender, EventArgs e)
        {
            txtEndTarget.Text = "";
            txtNumberOfSteps.Text = "";
            txtEndTarget.Enabled = rbtnEndTarget.Checked;
            txtNumberOfSteps.Enabled = rbtnNumberOfSteps.Checked;
        }
    }
}