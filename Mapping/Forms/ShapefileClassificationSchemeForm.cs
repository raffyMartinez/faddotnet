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
            switch (((Button)sender).Name)
            {
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
                    tabControl.TabPages["tabEqualInterval"].Select();
                    break;

                case "User defined":
                    tabControl.TabPages["tabUserDefined"].Select();
                    break;
            }
        }
    }
}