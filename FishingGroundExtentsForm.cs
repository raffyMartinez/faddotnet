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
    public partial class FishingGroundExtentsForm : Form
    {
        private string _AOIGuid = "";

        public FishingGroundExtentsForm(string AOIGuid)
        {
            InitializeComponent();
            _AOIGuid = AOIGuid;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FishingGroundExtentsForm_Load(object sender, EventArgs e)
        {
            foreach (var item in FishingGrid.Grid25.Bounds)
            {
                var lvi = lvGrids.Items.Add(item.gridDescription);
                lvi.SubItems.Add(item.ulGridName);
                lvi.SubItems.Add(item.lrGridName);
            }

            foreach (ColumnHeader c in lvGrids.Columns)
            {
                if (c.Index == 0)
                    c.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                else
                    c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }
    }
}