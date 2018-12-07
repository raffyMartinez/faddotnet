using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FAD3.GUI.Forms
{
    public partial class AboutFADForm2 : Form
    {
        private List<String> componentList = new List<string>();

        public AboutFADForm2()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            //global.LoadFormSettings(this, FormBorderStyle == FormBorderStyle.FixedToolWindow);
            labelApp.Text = "Fisheries Assessment Database 3";
            FontFamily ff = new FontFamily("Microsoft Sans Serif");
            labelApp.Font = new Font(ff, 13F, FontStyle.Bold);
            labelApp.Size = new Size(this.Width, labelApp.Height);
            labelApp.Location = new Point((this.Width - labelApp.Width) / 2, 25);

            lblVersion.Text = $"Version: {Application.ProductVersion}";

            lblAuthor.Text = "";
            lblAuthor.Text += "Written by: Raffy Martinez\r\n";
            lblAuthor.Text += "raffy.martinez@gmail.com";

            labelCredits.Text = "";
            if (global.HasMPH)
            {
                componentList.Add("DoubleMetaphone by Adam Nelson\r\n");
            };
            componentList.Add($"Microsoft net framework: {Environment.Version}\r\n");
            componentList.Add("Coordinate class by Jaime Olivares\r\n (http://github.com/jaime-olivares/coordinate)\r\n");
            componentList.Add("Lat-Long to UTM Converter\r\n (https://github.com/shahid28/utm-latlng)\r\n");
            componentList.Add("Ports of Fisher/Jenks natural breaks algorithm\r\n by Philipp Schöpf\r\n (https://github.com/pschoepf/naturalbreaks)\r\n");
            componentList.Add("SimMetrics - SimMetrics is a java library of\r\n Similarity or Distance Metrics\r\n by Sam Chapman\r\n (http://www.dcs.shef.ac.uk/~sam/stringmetrics.html)\r\n");
            componentList.Add("ClosedXML - .NET library for reading, manipulating and\r\n writing Excel 2007+ (.xlsx, .xlsm) files\r\n(https://github.com/ClosedXML/ClosedXML)\r\n");
            componentList.Add("HTML Agility Pack - agile HTML parser that builds \r\n a read/write DOM and \r\nsupports plain XPATH or XSLT\r\n(https://github.com/zzzprojects/html-agility-pack)\r\n");
            foreach (var item in componentList)
            {
                labelCredits.Text += $"{item}\r\n";
            }
            labelCredits.Text = $"This software make use of the following {(componentList.Count == 1 ? "component:" : "components:")}\r\n{labelCredits.Text}\r\n";

            labelCredits.Text += "Mapping component is not installed.\r\nMapping functions are not available\r\n\r\n";
            labelCredits.Font = new Font(ff, 8F, FontStyle.Regular);
            labelCredits.BorderStyle = BorderStyle.None;
            Text = "About FAD";
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            //global.SaveFormSettings(this);
        }
    }
}