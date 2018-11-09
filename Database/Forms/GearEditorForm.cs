using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FAD3.GUI.Classes;

namespace FAD3
{
    public partial class GearEditorForm : Form
    {
        private fad3GearEditAction _action;
        private string _GearVariationGuid = "";
        private string _GearClassGuid = "";
        private List<string> _List = new List<string>();
        private List<string> comboList = new List<string>();
        private GearCodesUsageForm _parentForm;

        public GearCodesUsageForm parentForm
        {
            get { return _parentForm; }
            set { _parentForm = value; }
        }

        public List<string> InList
        {
            get { return _List; }
            set { _List = value; }
        }

        public string GearClassGuid
        {
            get { return _GearClassGuid; }
            set { _GearClassGuid = value; }
        }

        public string GearVariationGuid
        {
            get { return _GearVariationGuid; }
            set { _GearVariationGuid = value; }
        }

        public fad3GearEditAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public GearEditorForm(GearCodesUsageForm parent)
        {
            InitializeComponent();
            _parentForm = parent;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            checkBox.Visible = false;
            labelCode.Visible = false;
            var x = 0;
            var y = 0;
            int spacer = 20;
            switch (_action)
            {
                case fad3GearEditAction.addGearVariation:
                    labelTitle.Text = "Add a gear variation";
                    labelDescription.Text = "Enter a gear variation name but you cannot use " +
                                             "any of the names listed below";
                    comboBox.Visible = false;
                    break;

                case fad3GearEditAction.addLocalName:
                    labelTitle.Text = "Add a gear local name";
                    labelDescription.Text = "Enter a gear local name but you cannot use " +
                                             "any of the local names already listed";
                    textBox.Visible = false;
                    x = buttonCancel.Location.X;
                    y = comboBox.Location.Y + comboBox.Size.Height + spacer;
                    buttonCancel.Location = new Point(x, y);
                    x = buttonOk.Location.X;
                    buttonOk.Location = new Point(x, y);
                    listBox.Visible = false;
                    this.Height = buttonCancel.Location.Y + buttonCancel.Height + spacer * 2;
                    break;

                case fad3GearEditAction.addGearCode:
                    labelTitle.Text = "Add a gear code";
                    labelDescription.Text = "Enter a gear code name but you cannot use " +
                                             "any of the codes listed below";
                    labelCode.Visible = true;
                    textBox.Left = labelCode.Left + labelCode.Width;
                    textBox.Width = (int)(textBox.Width / 4);
                    comboBox.Visible = false;
                    checkBox.Visible = true;
                    break;

                case fad3GearEditAction.addAOI:
                    labelTitle.Text = "Add a target area where used";
                    labelDescription.Text = "Enter a target area name but you cannot use " +
                                             "any of the names already listed";
                    textBox.Visible = false;
                    x = buttonCancel.Location.X;
                    y = comboBox.Location.Y + comboBox.Size.Height + spacer;
                    buttonCancel.Location = new Point(x, y);
                    x = buttonOk.Location.X;
                    buttonOk.Location = new Point(x, y);
                    listBox.Visible = false;
                    this.Height = buttonCancel.Location.Y + buttonCancel.Height + spacer * 2;
                    break;
            }

            FillList();

            if (comboBox.Visible)
                comboBox.Select();
        }

        private void FillList()
        {
            switch (_action)
            {
                case fad3GearEditAction.addGearVariation:
                    _List = Gear.AllGearVariationNames();
                    foreach (var item in _List)
                    {
                        listBox.Items.Add(item);
                    }
                    break;

                case fad3GearEditAction.addGearCode:
                    foreach (var item in Gear.GearCodesByClass(_GearClassGuid))
                    {
                        listBox.Items.Add(item);
                    }

                    labelCode.Text = Gear.GearLetterFromGearClass(_GearClassGuid);
                    break;

                case fad3GearEditAction.addAOI:
                case fad3GearEditAction.addLocalName:
                    TargetArea AOI = new TargetArea();

                    ((ComboBox)comboBox).With(o =>
                    {
                        if (_action == fad3GearEditAction.addAOI)
                            o.DataSource = new BindingSource(AOI.TargetAreas, null);
                        else
                            o.DataSource = new BindingSource(Gear.GearLocalNames, null); ;

                        o.DisplayMember = "Value";
                        o.ValueMember = "Key";
                        o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    });

                    for (int i = 0; i < comboBox.Items.Count; i++)
                        comboList.Add(((KeyValuePair<string, string>)comboBox.Items[i]).Value);

                    break;
            }
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonCancel":
                    this.Close();
                    break;

                case "buttonOk":

                    switch (_action)
                    {
                        case fad3GearEditAction.addGearVariation:
                            _parentForm.UsageGearVariation(textBox.Text);
                            break;

                        case fad3GearEditAction.addGearCode:
                            _parentForm.UsageGearCode(textBox.Text, checkBox.Checked);
                            break;

                        case fad3GearEditAction.addAOI:
                            var AOIGuid = ((KeyValuePair<string, string>)comboBox.SelectedItem).Key;
                            _parentForm.UsageTargetArea(AOIGuid, comboBox.Text);
                            break;

                        case fad3GearEditAction.addLocalName:
                            var localNameGuid = ((KeyValuePair<string, string>)comboBox.SelectedItem).Key;
                            _parentForm.UsageLocalName(localNameGuid, comboBox.Text);
                            break;
                    }
                    Close();
                    break;
            }
        }

        private void OnTextBoxValidating(object sender, CancelEventArgs e)
        {
            var s = textBox.Text;
            var msg = "";
            if (s.Length > 0)
            {
                switch (_action)
                {
                    case fad3GearEditAction.addGearVariation:
                        if (s.Length > 5)
                        {
                            if (_List.Contains(s, StringComparer.OrdinalIgnoreCase))
                            {
                                msg = "Gear variation name already in use. Select another name";
                            }
                        }
                        else
                        {
                            msg = "Gear variation name is too short. Use a name longer than 5 letters";
                        }
                        break;

                    case fad3GearEditAction.addGearCode:
                        try
                        {
                            int Code = int.Parse(s);
                            if (!(Code > 0 && s.Length == 2))
                            {
                                //entered code is numeric but not greater than zero and
                                //not formatted to two digits
                                msg = "Expected value is a number greater than \r\n" +
                                      "zero and formatted to two digits";
                            }
                            else
                            {
                                if (listBox.Items.Contains(labelCode.Text + s))
                                {
                                    msg = "Code already exists. Use another code";
                                }
                            }
                        }
                        catch
                        {
                            //entered code is not a number
                            msg = "Expected value is a number greater than  \r\n" +
                                   "zero and formatted to two digits";
                        }
                        break;
                }
            }

            if (msg.Length > 0)
            {
                e.Cancel = true;
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void OnComboBoxValidating(object sender, CancelEventArgs e)
        {
            var cbo = (ComboBox)sender;
            var msg = "";
            var s = "";

            s = comboBox.Text;

            if (comboList.Contains(s, StringComparer.OrdinalIgnoreCase))
            {
                if (_List.Contains(s, StringComparer.OrdinalIgnoreCase))
                {
                    msg = s + " already in the list. Please use another";
                }
            }
            else
            {
                if (_action == fad3GearEditAction.addLocalName)
                {
                    msg = s + " is not in the drop-down list\r\n" +
                          "Do you wish to add this as a new gear local name?";

                    DialogResult dr = MessageBox.Show(msg, "Add new local name", MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Exclamation);

                    if (dr == DialogResult.Yes)
                    {
                        AddNewGearLocalName();
                    }
                    else
                        e.Cancel = true;

                    msg = "";
                }
                else
                {
                    msg = s + " is not a valid Target area name";
                }
            }

            if (msg.Length > 0)
            {
                e.Cancel = true;
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AddNewGearLocalName()
        {
            ;
        }
    }
}