using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.GUI.Classes;

namespace FAD3.Database.Forms
{
    public partial class ExportDialogForm : Form
    {
        private ExportDataType _exportDataType;
        public string TaxaCSV { get; set; }

        public ExportDialogForm(ExportDataType exportDataType)
        {
            InitializeComponent();
            _exportDataType = exportDataType;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            checkBox1.Visible = false;
            var spacing = 4;
            switch (_exportDataType)
            {
                case ExportDataType.ExportDataSpecies:
                    checkBox1.Visible = true;
                    checkBox1.Text = "Export selected taxa only";
                    checkBox1.Tag = "taxa";
                    break;

                case ExportDataType.ExportDataCatchLocalNames:
                    break;

                case ExportDataType.ExportDataGears:
                    break;

                case ExportDataType.ExportDataLanguages:
                    break;

                case ExportDataType.ExportDataCatchLocalNameSpecies:

                    break;

                case ExportDataType.ExportDataSelect:

                    checkBox1.Text = "Export languages";
                    checkBox1.Tag = "exportLanguage";
                    checkBox1.Location = new Point(10, 15);
                    checkBox1.Visible = true;

                    CheckBox checkBox2 = new CheckBox();
                    checkBox2.Text = "Export local names";
                    checkBox2.Left = checkBox1.Left;
                    checkBox2.Top = checkBox1.Top + checkBox1.Height + spacing;
                    checkBox2.Tag = "exportLocalName";
                    checkBox2.AutoSize = true;
                    Controls.Add(checkBox2);

                    CheckBox checkBox3 = new CheckBox();
                    checkBox3.Text = "Export local name - scientific name pairs";
                    checkBox3.Left = checkBox1.Left;
                    checkBox3.Top = checkBox2.Top + checkBox2.Height + spacing;
                    checkBox3.Tag = "exportLNSNPair";
                    checkBox3.AutoSize = true;
                    Controls.Add(checkBox3);
                    break;
            }

            checkBox1.CheckedChanged += OnCheckBoxCheckChanged;
        }

        private void OnCheckBoxCheckChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            switch (chk.Tag.ToString())
            {
                case "taxa":
                    break;
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }
    }
}