using System;
using System.Windows.Forms;
using FAD3.Database.Classes;

namespace FAD3
{
    public partial class CoordinateFormatSelectForm : Form
    {
        private CoordinateDisplayFormat _DisplayFormat;

        public CoordinateFormatSelectForm()
        {
            InitializeComponent();
        }

        private void CoordinateFormatSelectForm_Load(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
            _DisplayFormat = global.CoordinateDisplay;
            switch (_DisplayFormat)
            {
                case CoordinateDisplayFormat.DegreeDecimal:
                    radioButtonDD.Checked = true;
                    break;

                case CoordinateDisplayFormat.DegreeMinute:
                    radioButtonDM.Checked = true;
                    break;

                case CoordinateDisplayFormat.DegreeMinuteSecond:
                    radioButtonDMS.Checked = true;
                    break;

                case CoordinateDisplayFormat.UTM:
                    radioButtonUTM.Checked = true;
                    break;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            foreach (Control c in groupRadio.Controls)
            {
                ((RadioButton)c).With(o =>
                {
                    if (o.Checked)
                    {
                        switch (o.Name)
                        {
                            case "radioButtonDD":
                                _DisplayFormat = CoordinateDisplayFormat.DegreeDecimal;
                                break;

                            case "radioButtonDM":
                                _DisplayFormat = CoordinateDisplayFormat.DegreeMinute;
                                break;

                            case "radioButtonDMS":
                                _DisplayFormat = CoordinateDisplayFormat.DegreeMinuteSecond;
                                break;

                            case "radioButtonUTM":
                                _DisplayFormat = _DisplayFormat = CoordinateDisplayFormat.UTM;
                                break;
                        }
                    }
                });
            }

            global.CoordinateDisplay = _DisplayFormat;
            Close();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
        }
    }
}