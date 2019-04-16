using FAD3.Mapping.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FAD3.Database.Forms
{
    public partial class MappingBatchForm : Form
    {
        private Form _parentForm;
        private ListViewItem _currentGearItem;

        public void MappedYears(List<int> years)
        {
            string yearsMapped = "";
            foreach (var item in years)
            {
                yearsMapped += $"{item},";
            }
            yearsMapped = yearsMapped.Trim(',');

            var file = $@"{FishingGearMapping.SaveMapFolder}\{_currentGearItem.Text}-{_currentGearItem.SubItems[1].Text}-{yearsMapped}.tif";
            global.MappingForm.SaveMapImage(int.Parse(txtDPI.Text), file, false);
        }

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
                                        _currentGearItem = item;
                                        var mehf = MapEffortHelperForm.GetInstance();
                                        mehf.BatchMode = true;
                                        mehf.CombineYearsInOneMap = chkCombinedMap.Checked;
                                        mehf.SetUpMapping(f.TargetArea.TargetAreaGuid, item.Tag.ToString(), item.SubItems[1].Text, f.TargetArea.TargetAreaName);
                                        mehf.MapTargetAreaGearFishingGroundBatch(this);
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