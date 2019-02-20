using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FAD3
{
    public partial class AboutFadForm : Form
    {
        private List<String> componentList = new List<string>();

        public AboutFadForm()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
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
            lblAuthor.Text += "raffy.martinez@gmail.com\r\n";
            lblAuthor.Text += "https://github.com/raffyMartinez/faddotnet";
            labelCredits.Text = "";
            if (global.HasMPH)
            {
                componentList.Add("DoubleMetaphone by Adam Nelson\r\n");
            }
            componentList.Add($"Mapwindows Mapping Components version {axMap.VersionNumber}\r\nwww.Mapwindows.org\r\n");
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
            labelCredits.Text = $"This software make use of the following {(componentList.Count == 1 ? "component:" : "components:")}\r\n{labelCredits.Text}\r\n\r\n";
            labelCredits.Font = new Font(ff, 8F, FontStyle.Regular);
            labelCredits.BorderStyle = BorderStyle.None;
        }

        private void AboutFadForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //global.SaveFormSettings(this);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}