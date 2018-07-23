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
    public partial class frmFishingGround : Form
    {
        public frmFishingGround()
        {
            InitializeComponent();
        }

        string _AOIGuid = "";
        frmSamplingDetail _Parent_form;
        FishingGrid.fadGridType _gt;
        private static frmFishingGround _instance;

        public frmSamplingDetail Parent_form
        {
            get { return _Parent_form; }
            set { _Parent_form = value; }
        }

        public static frmFishingGround GetInstance()
        {
            if (_instance == null) _instance = new frmFishingGround();
            return _instance;
        }

        public string AOIGuid
        {
            get { return _AOIGuid; }
            set
            {
                _AOIGuid = value;
                //_gt = FishingGrid.SetupFishingGrid(_AOIGuid);
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }
    }
}
