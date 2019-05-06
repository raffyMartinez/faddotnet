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
        private GearInventoryForm _parentForm;
        public string NameOfSavedGear { get; set; }

        public ExportImportProgressForm(FishingGearInventory fishingGearInventory, string fileName, GearInventoryForm parentForm)
        {
            InitializeComponent();
            FileName = fileName;
            FishingGearInventory = fishingGearInventory;
            FishingGearInventory.InventoryLevel += OnInventoryLevel;
            BatchSize = 25;
            _parentForm = parentForm;
        }

        private void OnInventoryLevel(object sender, FisheriesInventoryImportEventArg e)
        {
            NameOfSavedGear = e.NameOfSavedGear;
            if (e.StartImporting)
            {
                lblStatus.Invoke(new MethodInvoker(delegate
                {
                    lblStatus.Visible = true;
                }));
            }
            else if (NameOfSavedGear?.Length > 0)
            {
                ExportImportCount++;
                if (ExportImportTargetCount < ExportImportCount)
                {
                    ExportImportTargetCount += BatchSize;
                }
                UpdateProgress();
                lblStatus.Invoke(new MethodInvoker(delegate
                {
                    lblStatus.Text = $"processed {NameOfSavedGear}";
                }));
            }
        }

        public ExportImportProgressForm(string fileName, int exportImportTargetCount, GearInventoryForm parentForm)
        {
            InitializeComponent();
            FileName = fileName;
            ExportImportTargetCount = exportImportTargetCount;
            _parentForm = parentForm;
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
                    if (ActionExportImport == ExportImportAction.ActionExport)
                    {
                        lblStatus.Invoke(new MethodInvoker(delegate
                        {
                            lblStatus.Text = $"processed {NameOfSavedGear}";
                        }));
                    }
                }
            }));

            if (forcedComplete || (progressBar.Value == 100 && ActionExportImport == ExportImportAction.ActionExport))
            {
                lblTitle.Invoke((MethodInvoker)delegate
                {
                    lblTitle.Text = "Finished processing file!";
                });

                lblStatus.Invoke(new MethodInvoker(delegate
                {
                    lblStatus.Text = "Done";
                }));
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
            lblStatus.Visible = true;
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            if (ActionExportImport == ExportImportAction.ActionImport)
            {
                FishingGearInventory.InventoryLevel -= OnInventoryLevel;
            }
            _parentForm.ResetProgressForm();
        }
    }
}