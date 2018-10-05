using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FAD3
{
    public partial class TargetAreaGearsForm : Form
    {
        private static TargetAreaGearsForm _instance;
        private aoi _aoi;

        public TargetAreaGearsForm(aoi aoi)
        {
            InitializeComponent();
            _aoi = aoi;
        }

        public static TargetAreaGearsForm GetInstance(aoi aoi)
        {
            if (_instance == null) return new TargetAreaGearsForm(aoi);
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
            var gearUsedList = aoi.TargetAreaGearsUsed(_aoi.AOIGUID);
            lvGears.View = View.Details;
            lvGears.Columns.Add("Gear class");
            lvGears.Columns.Add("Variation");
            lvGears.Columns.Add("Variation code");

            SizeColumns(lvGears);

            foreach (var item in gearUsedList)
            {
                var lvi = lvGears.Items.Add(item.gearClass);
                lvi.SubItems.Add(item.gearVariarion);
                lvi.SubItems.Add(item.gearCode);
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
            Close();
        }
    }
}