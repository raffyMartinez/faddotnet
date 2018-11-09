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
    public partial class CatchLocalNamesForm : Form
    {
        private static CatchLocalNamesForm _instance;
        private Identification _idType;

        public static CatchLocalNamesForm GetInstance(Identification identification)
        {
            if (_instance == null) return new CatchLocalNamesForm(identification);
            return _instance;
        }

        public CatchLocalNamesForm(Identification identification)
        {
            InitializeComponent();
            _idType = identification;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnAdd":
                    break;

                case "btnRemove":
                    break;

                case "btnOk":
                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            switch (_idType)
            {
                case Identification.LocalName:
                    lblTree.Text = "Local names and languages";
                    lblList.Text = "Scientific names";
                    break;

                case Identification.Scientific:
                    lblTree.Text = "Scientific names and languages";
                    lblList.Text = "Local names";
                    break;
            }
        }

        private void OnFormClose(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }
    }
}