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
        private string _genus;
        private string _species;
        private CatchLocalNamesForm _parentForm;
        private bool _updateParent;

        public static CatchLocalNameSelectedForm GetInstance(Identification idType, string language, string name, CatchLocalNamesForm parent)
        {
            if (_instance == null) return new CatchLocalNameSelectedForm(idType, language, name, parent);
            return _instance;
        }

        public CatchLocalNameSelectedForm(Identification idType, string language, string name, CatchLocalNamesForm parent)
        {
            InitializeComponent();
            _idType = idType;
            _language = language;
            _name = name;
            _parentForm = parent;
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
                    for (int n = 0; n < spName.Length; n++)
                    {
                        if (n == 0)
                        {
                            _genus = spName[n];
                        }
                        else
                        {
                            _species += spName[n] + " ";
                        }
                    }
                    _species = _species.Trim(' ');
                    FillLocalNameList();
                    break;
            }
        }

        private void FillLocalNameList()
        {
            listBox.Items.Clear();
            foreach (var item in Names.LocalNamesFromSpeciesNameLanguage(_genus, _species, _language))
            {
                listBox.Items.Add(item);
            }
            listBox.ValueMember = "Key";
            listBox.DisplayMember = "Value";
        }

        private void OnFormClose(object sender, FormClosedEventArgs e)
        {
            _instance = null;
            global.SaveFormSettings(this);
            if (_updateParent)
            {
                _parentForm.RefreshLists();
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        public void UpdateList(string updatedName)
        {
            FillLocalNameList();
            _updateParent = true;
        }

        private void OnListBoxDblClick(object sender, EventArgs e)
        {
            if (_idType == Identification.Scientific)
            {
                KeyValuePair<string, string> kv = (KeyValuePair<string, string>)listBox.SelectedItem;
                EditSelectedLocalNameForm eslnf = new EditSelectedLocalNameForm(kv.Value, kv.Key, this);
                eslnf.ShowDialog(this);
            }
        }

        private void OnContextMenuItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.Owner.Hide();
            switch (e.ClickedItem.Name)
            {
                case "deleteToolStripMenuItem":
                    if (Names.DeleteLocalNameSpeciesNamePair(listBox.Text, _name, _language))
                    {
                        listBox.Items.Remove(listBox.SelectedItem);
                        _parentForm.RefreshLists();
                    }
                    break;

                case "addToolStripMenuItem":
                    EditSelectedLocalNameForm eslf = new EditSelectedLocalNameForm(_idType, _language, _name, this);
                    eslf.ShowDialog(this);
                    break;

                case "detailsToolStripMenuItem":
                    break;
            }
        }
    }
}