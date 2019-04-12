﻿using System;
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
    public partial class BatchExportMapSettingsForm : Form
    {
        public bool ExportReverseSide;
        public Dictionary<int, (string layerName, bool showFront, bool showFrontLabel, bool showReverse, bool showReverseLabel)> ExportSettingsDict { get; set; } //= new Dictionary<int, (string layerName, bool showFront, bool showFrontLabel, bool showReverse, bool showReverseLabel)>();
        public BatchExportMapSettingsForm()
        {
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            dgSettings.CellValueChanged -= OnCellValueChanged;
            foreach(var item in ExportSettingsDict)
            {
                int row = dgSettings.Rows.Add(new object[] {
                    item.Value.layerName,
                    item.Value.showFront,
                    item.Value.showFrontLabel,
                    item.Value.showReverse,
                    item.Value.showReverseLabel
                });
                dgSettings.Rows[row].Tag = item.Key;
            }
            dgSettings.CellValueChanged += OnCellValueChanged;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch(((Button)sender).Name)
            {
                case "btnOk":
                    ExportSettingsDict.Clear();
                    foreach(DataGridViewRow row in dgSettings.Rows)
                    {
                        ExportSettingsDict.Add((int)row.Tag, ((string)row.Cells[0].Value, (bool)row.Cells[1].Value, (bool)row.Cells[2].Value, (bool)row.Cells[3].Value, (bool)row.Cells[4].Value));
                    }

                    DialogResult = DialogResult.OK;
                    break;
                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void OnCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            switch(e.ColumnIndex)
            {
                case 1:
                case 3:
                    if(e.RowIndex != -1)
                    {
                        if(!(bool)dgSettings[e.ColumnIndex,e.RowIndex].Value)
                        {
                            dgSettings[e.ColumnIndex + 1, e.RowIndex].Value = false;
                        }
                    }
                    break;
                case 2:
                case 4:
                    if (e.RowIndex != -1)
                    {
                        if ((bool)dgSettings[e.ColumnIndex, e.RowIndex].Value)
                        {
                            dgSettings[e.ColumnIndex - 1, e.RowIndex].Value = true;
                        }
                    }
                    break;
            }
        }

        private void OnDatagridMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 1:
                case 3:
                case 2:
                case 4:
                    if (e.RowIndex != -1)
                    {
                        dgSettings.EndEdit();
                    }
                    break;

            }
        }
    }
}