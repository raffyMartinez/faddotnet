using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAD3.GUI.Classes;

namespace FAD3.Database.Forms
{
    public partial class CatchLocalNameSelectedForm : Form
    {
        private static CatchLocalNameSelectedForm _instance;
        private Identification _idType;
        private string _language;
        private string _name;

        public static CatchLocalNameSelectedForm GetInstance(Identification idType, string language, string name)
        {
            if (_instance == null) return new CatchLocalNameSelectedForm(idType, language, name);
            return _instance;
        }

        public CatchLocalNameSelectedForm(Identification idType, string language, string name)
        {
            InitializeComponent();
            _idType = idType;
            _language = language;
            _name = name;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
            switch (_idType)
            {
                case Identification.LocalName:
                    lblTitle.Text = $"Local name: {_name} \r\nLanguage: {_language}";
                    lblList.Text = "Species names";
                    Text = "Local name to scientific names";
                    foreach (var item in Names.ScientificNamesFromLocalNameLanguage(_name, _language))
                    {
                        listBox.Items.Add(item);
                    }
                    break;

                case Identification.Scientific:
                    lblTitle.Text = $"Species name: {_name} \r\nLanguage: {_language}";
                    lblList.Text = "Local names";
                    Text = "Scientific name to local names";
                    var spName = _name.Split(' ');
                    var genus = "";
                    var species = "";
                    for (int n = 0; n < spName.Length; n++)
                    {
                        if (n == 0)
                        {
                            genus = spName[n];
                        }
                        else
                        {
                            species += spName[n] + " ";
                        }
                    }
                    species = species.Trim(' ');
                    foreach (var item in Names.LocalNamesFromSpeciesNameLanguage(genus, species, _language))
                    {
                        listBox.Items.Add(item);
                    }
                    break;
            }
        }

        private void OnFormClose(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}