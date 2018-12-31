using System;
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
            foreach (var item in FishingGrid.Grid25.BoundsEx)
            {
                var lvi = lvGrids.Items.Add(item.Value.gridDescription);
                lvi.SubItems.Add(item.Value.ulGridName);
                lvi.SubItems.Add(item.Value.lrGridName);
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