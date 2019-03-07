using System;
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
    public partial class DisplayCopyableTextForm : Form
    {
        private static DisplayCopyableTextForm _instance;
        private string _textToDisplay;

        public string TextToDisplay
        {
            get { return _textToDisplay; }
            set
            {
                _textToDisplay = value;
                txtDisplay.Text = _textToDisplay;
            }
        }

        public static DisplayCopyableTextForm GetInstance()
        {
            if (_instance == null) _instance = new DisplayCopyableTextForm();
            return _instance;
        }

        public DisplayCopyableTextForm()
        {
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this);
            txtDisplay.Text = _textToDisplay;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }
    }
}