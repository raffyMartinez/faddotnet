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
    public enum fadMapText
    {
        mapTextNone,
        mapTextTitle,
        mapTextNote
    }

    public partial class ConfigureMapTextHelper : Form
    {
        private static ConfigureMapTextHelper _instance;

        public static ConfigureMapTextHelper GetInstance(fadMapText mapText)
        {
            if (_instance == null) return new ConfigureMapTextHelper(mapText);
            return _instance;
        }

        public ConfigureMapTextHelper(fadMapText mapText)
        {
            InitializeComponent();
        }

        private void ConfigureMapTextForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }
    }
}