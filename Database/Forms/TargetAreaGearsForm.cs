using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FAD3.Mapping.Classes;
using FAD3.Database.Forms;

namespace FAD3
{
    public partial class TargetAreaGearsForm : Form
    {
        private static TargetAreaGearsForm _instance;
        private TargetArea _targetArea;
        public TargetArea TargetArea { get { return _targetArea; } }
        private bool _isGearMapping;

        public TargetAreaGearsForm(TargetArea targetArea)
        {
            InitializeComponent();
            _targetArea = targetArea;
        }

        public static TargetAreaGearsForm GetInstance(TargetArea aoi)
        {
            if (_instance == null) _instance = new TargetAreaGearsForm(aoi);
            return _instance;
        }

        /// <summary>
        /// Sizes all columns so that it fits the widest column content or the column header content
        /// </summary>
        private void SizeColumns(ListView lv, bool init = true)
        {
            foreach (ColumnHeader c in lv.Columns)
            {
                if (init)
                {
                    c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                    c.Tag = c.Width;
                }
                else
                {
                    c.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    c.Width = c.Width > (int)c.Tag ? c.Width : (int)c.Tag;
                }
            }
        }

        private void TargetAreaGearsForm_Load(object sender, EventArgs e)
        {
            var gearUsedList = TargetArea.TargetAreaGearsUsed(_targetArea.TargetAreaGuid);
            lvGears.View = View.Details;
            lvGears.Columns.Add("Gear class");
            lvGears.Columns.Add("Variation");
            lvGears.Columns.Add("Number sampled");

            SizeColumns(lvGears);

            foreach (var item in gearUsedList)
            {
                var lvi = lvGears.Items.Add(item.gearClass);
                lvi.SubItems.Add(item.gearVariation);
                lvi.SubItems.Add(item.count.ToString());
                lvi.Tag = item.GearVarGuid;
            }
            SizeColumns(lvGears, false);

            lvGears.FullRowSelect = true;
        }

        private void TargetAreaGearsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    Close();
                    break;

                case "btnBatch":
                    MappingBatchForm f = new MappingBatchForm(this);
                    f.ShowDialog(this);
                    break;

                case "btnSelect":
                    if (txtCondition.Text.Length > 0)
                    {
                        foreach (ListViewItem lvi in lvGears.Items)
                        {
                            lvi.Checked = int.Parse(lvi.SubItems[2].Text) >= int.Parse(txtCondition.Text);
                        }
                    }
                    break;
            }
        }

        public ListView GearListView
        {
            get { return lvGears; }
        }

        private void OnContextMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.Owner.Hide();

            switch (e.ClickedItem.Name)
            {
                case "mnuMapThisGear":
                    _isGearMapping = false;
                    if (global.MapIsOpen)
                    {
                        MapTargetAreaGear();
                        _isGearMapping = true;
                    }
                    break;
            }
        }

        private void MapTargetAreaGear(bool MapFirstItemInList = false)
        {
            var mehf = MapEffortHelperForm.GetInstance();
            if (mehf.Visible)
            {
                mehf.BringToFront();
            }
            else
            {
                mehf.Show(this);
            }
            mehf.SetUpMapping(_targetArea.TargetAreaGuid, lvGears.SelectedItems[0].Tag.ToString(), lvGears.SelectedItems[0].SubItems[1].Text, _targetArea.TargetAreaName, MapFirstItemInList = true);
        }

        private void OnListViewMouseDown(object sender, MouseEventArgs e)
        {
            mnuMapThisGear.Enabled = FishingGrid.IsCompleteGrid25 && global.MapIsOpen;
            if (e.Button == MouseButtons.Right && _isGearMapping)
            {
                MapTargetAreaGear(MapFirstItemInList: true);
            }
        }

        private void OnListViewMouseClick(object sender, MouseEventArgs e)
        {
        }
    }
}