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
using FAD3.GUI.Classes;

namespace FAD3.Database.Forms
{
    public partial class ExportImportProgressForm : Form
    {
        public string FileName { get; internal set; }
        public int ExportImportCount { get; set; }
        public int ExportImportTargetCount { get; internal set; }
        public ExportImportAction ActionExportImport { get; set; }
        public FishingGearInventory FishingGearInventory { get; set; }
        public int BatchSize { get; set; }

        public ExportImportProgressForm(FishingGearInventory fishingGearInventory, string fileName)
        {
            InitializeComponent();
            FileName = fileName;
            FishingGearInventory = fishingGearInventory;
            FishingGearInventory.InventoryLevel += OnInventoryLevel;
            BatchSize = 25;
        }

        private void OnInventoryLevel(object sender, FisheriesInventoryImportEventArg e)
        {
            ExportImportCount++;
            if (ExportImportTargetCount < ExportImportCount)
            {
                ExportImportTargetCount += BatchSize;
            }
            UpdateProgress();
        }

        public ExportImportProgressForm(string fileName, int exportImportTargetCount)
        {
            InitializeComponent();
            FileName = fileName;
            ExportImportTargetCount = exportImportTargetCount;
        }

        public void UpdateProgress(bool forcedComplete = false)
        {
            progressBar.Invoke(new MethodInvoker(delegate
            {
                if (forcedComplete)
                {
                    progressBar.Value = 100;
                }
                else
                {
                    progressBar.Value = (int)(((double)ExportImportCount / (double)ExportImportTargetCount) * 100);
                }
            }));

            if (forcedComplete || (progressBar.Value == 100 && ActionExportImport == ExportImportAction.ActionExport))
            {
                lblTitle.Invoke((MethodInvoker)delegate
                {
                    lblTitle.Text = "Finished processing file!";
                });
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            switch (ActionExportImport)
            {
                case ExportImportAction.ActionImport:
                    ExportImportTargetCount = BatchSize;
                    lblTitle.Text = "Importing inventory";
                    break;

                case ExportImportAction.ActionExport:
                    lblTitle.Text = "Exporting inventory";
                    break;
            }
            lblExportImport.Text = $"Processing {FileName.EllipsisString()}";
        }
    }
}