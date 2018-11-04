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

namespace FAD3.Database.Forms
{
    public partial class NewNameForm : Form
    {
        private string _newName;
        private FisheryObjectNameType _objectNameType;
        private Form _parentForm;
        private Dictionary<string, string> _similarNames = new Dictionary<string, string>();
        private NewFisheryObjectName _newObjectName;

        public static DialogResult Show(string newName, FisheryObjectNameType objectNameType, Form parent)
        {
            NewNameForm f = new NewNameForm(newName, objectNameType, parent);
            return f.ShowDialog();
        }

        public bool Cancel
        {
            get; internal set;
        }

        public NewNameForm(string newName, FisheryObjectNameType objectNameType, Form parent)
        {
            InitializeComponent();
            _newName = newName;
            _objectNameType = objectNameType;
            _parentForm = parent;
            _newObjectName = new NewFisheryObjectName(_newName, _objectNameType);
            switch (_objectNameType)
            {
                case FisheryObjectNameType.CatchLocalName:
                    Text = "New catch local name";
                    _similarNames = names.GetSimilarSoundingLocalNames(_newObjectName);
                    break;

                case FisheryObjectNameType.GearLocalName:
                    Text = "New gear local name";
                    _similarNames = gear.GetSimilarSoundingLocalNames(_newObjectName);
                    break;
            }
            txtLocalName.Text = newName;
            listBoxSimilar.ValueMember = "key";
            listBoxSimilar.DisplayMember = "value";
            foreach (var item in _similarNames)
            {
                //KeyValuePair<string, string> kv = new KeyValuePair<string, string>(item.nameGuid, item.localName);
                listBoxSimilar.Items.Add(item);
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    bool success = false;
                    switch (_objectNameType)
                    {
                        case FisheryObjectNameType.CatchLocalName:
                            success = names.SaveNewLocalName(_newObjectName);
                            break;

                        case FisheryObjectNameType.GearLocalName:
                            success = gear.SaveNewLocalName(_newObjectName);
                            break;
                    }

                    if (success) DialogResult = DialogResult.OK;
                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
            Close();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            listBoxSimilar.Visible = false;
            if (_similarNames.Count > 0)
            {
                listBoxSimilar.Visible = true;
            }
            else
            {
                int space = ClientSize.Height - (btnOk.Top + btnOk.Height);
                btnOk.Top = listBoxSimilar.Top;
                btnCancel.Top = btnOk.Top;
                Height = btnOk.Top + (btnOk.Height) + space + (Height - ClientSize.Height);
            }

            lblSimilar.Visible = listBoxSimilar.Visible;
        }

        private void OnListDblClick(object sender, EventArgs e)
        {
            if (_parentForm.GetType().Name == "GearInventoryEditForm")
            {
                ((GearInventoryEditForm)_parentForm).SelectedSimilarName(listBoxSimilar.Text, _objectNameType);
            }

            Close();
        }
    }
}