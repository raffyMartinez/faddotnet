using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FAD3.Database.Forms
{
    public partial class ExportCatchMonitoringDataForm : Form
    {
        public string FileName { get; internal set; }

        public ExportCatchMonitoringDataForm()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    FileName = txtFileName.Text;
                    if (FileName.Length > 0)
                    {
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Please provide file name", "File name is missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;

                case "btnFileName":
                    SaveFileDialog saveAs = new SaveFileDialog();
                    saveAs.Title = "File name of exported catch monitoring data";
                    saveAs.Filter = "Excel file|*.xls|XML|*.xml|CSV|*.csv";
                    saveAs.FilterIndex = 2;
                    DialogResult dr = saveAs.ShowDialog();
                    if (dr == DialogResult.OK && saveAs.FileName.Length > 0)
                    {
                        txtFileName.Text = saveAs.FileName;
                    }
                    break;
            }
        }
    }
}