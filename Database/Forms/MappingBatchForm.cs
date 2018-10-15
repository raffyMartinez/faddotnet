using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.Mapping.Classes;

namespace FAD3.Database.Forms
{
    public partial class MappingBatchForm : Form
    {
        private Form _parentForm;

        public MappingBatchForm(TargetAreaGearsForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnDirectory":
                    var folderBrowser = new FolderBrowserDialog();
                    folderBrowser.ShowNewFolderButton = true;
                    folderBrowser.SelectedPath = SaveMapForm.GetSavedMapsFolder();
                    folderBrowser.Description = "Locate folder containing saved fishing ground";
                    DialogResult result = FolderBrowserLauncher.ShowFolderBrowser(folderBrowser);
                    if (result == DialogResult.OK)
                    {
                        FishingGearMapping.SaveMapFolder = folderBrowser.SelectedPath;
                        txtFolderPath.Text = FishingGearMapping.SaveMapFolder;
                    }
                    break;

                case "btnOk":
                    if (FishingGearMapping.SaveMapFolder.Length > 0 && txtDPI.Text.Length > 0)
                    {
                        switch (_parentForm.GetType().Name)
                        {
                            case "TargetAreaGearsForm":
                                var f = _parentForm as TargetAreaGearsForm;
                                var n = 0;
                                foreach (ListViewItem item in f.GearListView.Items)
                                {
                                    if (item.Checked)
                                    {
                                        var mehf = MapEffortHelperForm.GetInstance();
                                        mehf.BatchMode = true;
                                        mehf.SetUpMapping(f.AOI.AOIGUID, item.Tag.ToString(), item.SubItems[1].Text, f.AOI.AOIName);
                                        mehf.MapTargetAreaGearFishingGround();
                                        var file = $@"{FishingGearMapping.SaveMapFolder}\{item.Text}-{item.SubItems[1].Text}.tif";
                                        global.MappingForm.SaveMapImage(int.Parse(txtDPI.Text), file, false);
                                        n++;
                                    }
                                }
                                if (n > 0)
                                {
                                    MessageBox.Show($"{n} items were mapped and saved");
                                }
                                Close();
                                break;
                        }
                    }
                    Close();
                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }
    }
}