using System;
using System.Windows.Forms;

namespace FAD3
{
    public partial class CoordinateFormatSelectForm : Form
    {
        private global.CoordinateDisplayFormat _DisplayFormat;

        public CoordinateFormatSelectForm()
        {
            InitializeComponent();
        }

        private void CoordinateFormatSelectForm_Load(object sender, EventArgs e)
        {
            _DisplayFormat = global.CoordinateDisplay;
            switch (_DisplayFormat)
            {
                case global.CoordinateDisplayFormat.DegreeDecimal:
                    radioButtonDD.Checked = true;
                    break;

                case global.CoordinateDisplayFormat.DegreeMinute:
                    radioButtonDM.Checked = true;
                    break;

                case global.CoordinateDisplayFormat.DegreeMinuteSecond:
                    radioButtonDMS.Checked = true;
                    break;

                case global.CoordinateDisplayFormat.UTM:
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
                                _DisplayFormat = global.CoordinateDisplayFormat.DegreeDecimal;
                                break;

                            case "radioButtonDM":
                                _DisplayFormat = global.CoordinateDisplayFormat.DegreeMinute;
                                break;

                            case "radioButtonDMS":
                                _DisplayFormat = global.CoordinateDisplayFormat.DegreeMinuteSecond;
                                break;

                            case "radioButtonUTM":
                                _DisplayFormat = _DisplayFormat = global.CoordinateDisplayFormat.UTM;
                                break;
                        }
                    }
                });
            }

            global.CoordinateDisplay = _DisplayFormat;
            Close();
        }
    }
}