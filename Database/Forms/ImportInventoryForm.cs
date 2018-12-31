using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Database.Classes;
using System.IO;
using System.Xml;

namespace FAD3.Database.Forms
{
    public partial class ImportInventoryForm : Form
    {
        private static ImportInventoryForm _instance;

        public static ImportInventoryForm GetInstance()
        {
            if (_instance == null) return new ImportInventoryForm();
            return _instance;
        }

        public ImportInventoryForm()
        {
            InitializeComponent();
        }

        private void OnToolbarClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "tsbClose":
                    Close();
                    break;

                case "tsbFileOpen":
                    FileDialogHelper.Title = "Get filename for importing fisher, vessel and fishing gear inventory";
                    FileDialogHelper.DialogType = FileDialogType.FileOpen;
                    FileDialogHelper.DataFileType = DataFileType.Text | DataFileType.XML | DataFileType.CSV;
                    FileDialogHelper.ShowDialog();
                    var fileName = FileDialogHelper.FileName;
                    if (fileName.Length > 0)
                    {
                        switch (Path.GetExtension(fileName))
                        {
                            case ".txt":
                                break;

                            case ".xml":
                            case ".XML":
                                var elementCounter = 0;
                                var proceed = false;
                                var nodeName = "";
                                var localName = "";
                                var newGear = false;
                                var gearInventoryGuid = "";
                                TreeNode root = new TreeNode();
                                TreeNode brgyNode = new TreeNode();
                                TreeNode gearNode = new TreeNode();
                                XmlTextReader xmlReader = new XmlTextReader(fileName);
                                while ((elementCounter == 0 || (elementCounter > 0 && proceed)) && xmlReader.Read())
                                {
                                    switch (xmlReader.Name)
                                    {
                                        case "FisherVesselGearInventoryProject":
                                            root = treeXML.Nodes.Add("root", xmlReader.GetAttribute("ProjectName"));
                                            break;

                                        case "FisherVesselInventory":
                                            string sitio = xmlReader.GetAttribute("Sitio");
                                            var barangay = xmlReader.GetAttribute("Barangay");
                                            var municipalityName = xmlReader.GetAttribute("Municipality");
                                            var province = xmlReader.GetAttribute("Province");
                                            if (sitio == null)
                                            {
                                                sitio = "Entire barangay";
                                            }
                                            nodeName = $"{sitio}, {barangay}, {municipalityName}, {province}";
                                            brgyNode = root.Nodes.Add(xmlReader.GetAttribute("BarangayInventoryGuid"), nodeName);
                                            break;

                                        case "GearInventory":
                                            newGear = true;
                                            gearInventoryGuid = xmlReader.GetAttribute("GearInventoryGuid");
                                            gearNode = brgyNode.Nodes.Add(xmlReader.GetAttribute("GearInventoryGuid"), xmlReader.GetAttribute("GearVariation"));
                                            break;

                                        case "LocalName":

                                            break;
                                    }
                                }
                                break;

                            case ".csv":
                                break;
                        }
                    }
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }
    }
}