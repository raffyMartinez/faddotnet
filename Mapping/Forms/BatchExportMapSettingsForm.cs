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

namespace FAD3.Mapping.Forms
{
    public partial class BatchExportMapSettingsForm : Form
    {
        private int _labelsRow;
        public bool ExportReverseSide;
        public bool ShowTitleOnReverseSide;
        public bool ShowZoneOnReverseSide;
        public Dictionary<int, FrontAndReverseMapSpecs> ExportSettingsDict { get; set; } //= new Dictionary<int, (string layerName, bool showFront, bool showFrontLabel, bool showReverse, bool showReverseLabel)>();

        public BatchExportMapSettingsForm()
        {
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            int row = 0;
            dgSettings.CellValueChanged -= OnCellValueChanged;
            foreach (var item in ExportSettingsDict)
            {
                row = dgSettings.Rows.Add(new object[] {
                    item.Value.LayerName,
                    item.Value.ShowInFront,
                    item.Value.ShowLabelsFront,
                    item.Value.ShowInReverse,
                    item.Value.ShowLabelsReverse
                });
                if (item.Value.LayerName == "Labels")
                {
                    _labelsRow = row;
                }
                dgSettings.Rows[row].Tag = item.Key;
                dgSettings.Rows[row].Cells[0].Tag = item.Value.IsGrid25Layer;
            }
            dgSettings.CellValueChanged += OnCellValueChanged;
            Text = "Visibility of layers and labels";
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    ExportSettingsDict.Clear();
                    foreach (DataGridViewRow row in dgSettings.Rows)
                    {
                        int key = (int)row.Tag;
                        ExportSettingsDict.Add(key, new FrontAndReverseMapSpecs(key, (bool)row.Cells[0].Tag, (string)row.Cells[0].Value, (bool)row.Cells[1].Value, (bool)row.Cells[2].Value, (bool)row.Cells[3].Value, (bool)row.Cells[4].Value));
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
            switch (e.ColumnIndex)
            {
                case 1:
                case 3:
                    if (e.RowIndex != -1)
                    {
                        if (!(bool)dgSettings[e.ColumnIndex, e.RowIndex].Value)
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

        private void OnCellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right
                && e.ColumnIndex == 0
                && e.RowIndex == _labelsRow
                && (bool)dgSettings.Rows[e.RowIndex].Cells[4].Value)
            {
                dropdownSettings.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void OnCellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                dgSettings.EditMode = DataGridViewEditMode.EditProgrammatically;
            }
            else
            {
                dgSettings.EditMode = DataGridViewEditMode.EditOnEnter;
            }
        }

        private void OnDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "menuItemSetupLabels":
                    using (ReverseGridLabelsSetupForm rslf = new ReverseGridLabelsSetupForm())
                    {
                        rslf.ShowDialog(this);
                        if (rslf.DialogResult == DialogResult.OK)
                        {
                            ShowTitleOnReverseSide = rslf.ShowTitleOnReverse;
                            ShowZoneOnReverseSide = rslf.ShowZoneOnReverse;
                        }
                    }

                    break;
            }
        }
    }
}