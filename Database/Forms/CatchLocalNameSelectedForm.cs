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
using FAD3.Database.Classes;
using System.Diagnostics;

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
        private bool _loadFormSettings;

        public static CatchLocalNameSelectedForm GetInstance(Identification idType, string language, string name, CatchLocalNamesForm parent)
        {
            if (_instance == null) _instance = new CatchLocalNameSelectedForm(idType, language, name, parent);
            return _instance;
        }

        public void NewSelection(Identification idType, string language, string name)
        {
            _genus = "";
            _species = "";
            _idType = idType;
            _language = language;
            _name = name;
            _loadFormSettings = false;
            listBox.Items.Clear();
            OnFormLoad(null, null);
        }

        public CatchLocalNameSelectedForm(Identification idType, string language, string name, CatchLocalNamesForm parent)
        {
            InitializeComponent();
            _idType = idType;
            _language = language;
            _name = name;
            _parentForm = parent;
            _loadFormSettings = true;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            if (_loadFormSettings) global.LoadFormSettings(this, true);
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
            lblCount.Text = "Number of names: " + listBox.Items.Count;
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

        private void FillSpeciesNameList()
        {
            listBox.Items.Clear();
            foreach (var item in Names.ScientificNamesFromLocalNameLanguage(_name, _language))
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

        public void UpdateList(Identification idType)
        {
            if (idType == Identification.LocalName)
            {
                FillSpeciesNameList();
            }
            else
            {
                FillLocalNameList();
            }
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
                case "menuDelete":
                    var ln = "";
                    var sn = "";
                    switch (_idType)
                    {
                        case Identification.LocalName:
                            ln = _name;
                            sn = listBox.Text;
                            break;

                        case Identification.Scientific:
                            ln = listBox.Text;
                            sn = _name;
                            break;
                    }
                    if (Names.DeleteLocalNameSpeciesNamePair(ln, sn, _language))
                    {
                        listBox.Items.Remove(listBox.SelectedItem);
                        _parentForm.RefreshLists();
                    }
                    break;

                case "menuAdd":
                    EditSelectedLocalNameForm eslf = new EditSelectedLocalNameForm(_idType, _language, _name, this);
                    eslf.ShowDialog(this);
                    break;

                case "menuDetails":
                    break;

                case "menuShowNavigation":
                    switch (_idType)
                    {
                        case Identification.LocalName:
                            _parentForm.SwichViewCombo.SelectedIndex = 0;
                            break;

                        case Identification.Scientific:
                            _parentForm.SwichViewCombo.SelectedIndex = 1;
                            break;
                    }
                    _parentForm.SetTreeItem(listBox.Text);
                    break;
            }
        }

        private void OnListBoxMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dropDownMenu.Items.Clear();
                var item = dropDownMenu.Items.Add("Add");
                item.Name = "menuAdd";

                item = dropDownMenu.Items.Add("Delete");
                item.Name = "menuDelete";

                item = dropDownMenu.Items.Add("Edit");
                item.Name = "menuEdit";

                item = dropDownMenu.Items.Add("Details");
                item.Name = "menuDetails";

                dropDownMenu.Items.Add("-");

                item = dropDownMenu.Items.Add("Show on navigation tree");
                item.Name = "menuShowNavigation";

                ToolStripMenuItem subMenu = new ToolStripMenuItem();
                subMenu.Text = "Browse on WWW";

                CatchNameURLGenerator.CatchName = listBox.Text;
                foreach (var url in CatchNameURLGenerator.URLS)
                {
                    ToolStripMenuItem subItem = new ToolStripMenuItem();
                    subItem.Text = url.Key;
                    subItem.Tag = url.Value;
                    subMenu.DropDownItems.Add(subItem);
                }

                subMenu.DropDownItemClicked += OnSubMenuDropDownClick;

                dropDownMenu.Items.Add(subMenu);
            }
        }

        private void OnSubMenuDropDownClick(object sender, ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.OwnerItem.Owner.Hide();
            Process.Start(e.ClickedItem.Tag.ToString());
        }
    }
}