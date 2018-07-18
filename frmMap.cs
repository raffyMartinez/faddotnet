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
    public partial class frmMap : Form
    {
        public frmMap()
        {
            InitializeComponent();
        }

        private void frmMap_Load(object sender, EventArgs e)
        {
            global.MapIsOpen = true;
        }

        private void frmMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            global.MapIsOpen = false;
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem tsi = e.ClickedItem;
            switch (tsi.Name){
                case "tsButtonLayers":
                    break;
                case "tsButtonLayerAdd":
                    break;
            }
        }
    }
}
