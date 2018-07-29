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
    public partial class frmGearEditor : Form
    {
        private global.fad3GearEditAction _action;
        private string _GearVariationGuid = "";
        private string _GearClassGuid = "";
        private List<string> _List = new List<string>();
        private List<string> comboList = new List<string>();

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

        public global.fad3GearEditAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public frmGearEditor()
        {
            InitializeComponent();
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
                case global.fad3GearEditAction.addGearVariation:
                    labelTitle.Text = "Add a gear variation";
                    labelDescription.Text = "Enter a gear variation name but you cannot use " +
                                             "any of the names listed below";
                    comboBox.Visible = false;
                    break;
                case global.fad3GearEditAction.addLocalName:
                    labelTitle.Text = "Add a gear local name";
                    labelDescription.Text = "Enter a gear local name but you cannot use " +
                                             "any of the local names already listed";
                    textBox.Visible = false;
                    x = buttonCancel.Location.X;
                    y = comboBox.Location.Y + comboBox.Size.Height + spacer;
                    buttonCancel.Location = new Point(x,y);
                    x = buttonOk.Location.X;
                    buttonOk.Location = new Point(x, y);
                    listBox.Visible = false;
                    this.Height = buttonCancel.Location.Y + buttonCancel.Height + spacer*2;
                    break;
                case global.fad3GearEditAction.addGearCode:
                    labelTitle.Text = "Add a gear code";
                    labelDescription.Text = "Enter a gear code name but you cannot use " +
                                             "any of the codes listed below";
                    labelCode.Visible = true;
                    textBox.Left = labelCode.Left + labelCode.Width;
                    textBox.Width = (int)(textBox.Width / 4);
                    comboBox.Visible = false;
                    checkBox.Visible = true;
                    break;
                case global.fad3GearEditAction.addAOI:
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
                    this.Height = buttonCancel.Location.Y + buttonCancel.Height + spacer*2;
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
                case
                global.fad3GearEditAction.addGearVariation:
                    _List = gear.AllGearVariationNames();
                    foreach (var item in _List)
                    {
                        listBox.Items.Add(item);
                    }
                    break;
                case global.fad3GearEditAction.addGearCode:
                    foreach (var item in gear.GearCodesByClass(_GearClassGuid))
                    {
                        listBox.Items.Add(item);
                    }

                    labelCode.Text = gear.GearLetterFromGearClass(_GearClassGuid);
                    break;

                case global.fad3GearEditAction.addAOI:
                case global.fad3GearEditAction.addLocalName:
                    aoi AOI = new aoi();

                    ((ComboBox)comboBox).With(o =>
                    {
                        if (_action == global.fad3GearEditAction.addAOI)
                            o.DataSource = new BindingSource(AOI.AOIs, null);
                        else
                            o.DataSource = new BindingSource(gear.GearLocalNames, null); ;

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
                case "buttonOK":
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
                    case global.fad3GearEditAction.addGearVariation:
                        if(s.Length>5)
                        {
                            if (_List.Contains(s,StringComparer.OrdinalIgnoreCase))
                            {
                                msg = "Gear variation name already in use. Select another name";
                            }
                        }
                        else
                        {
                            msg = "Gear variation name is too short. Use a name longer than 5 letters";
                        }
                        break;
                    case global.fad3GearEditAction.addGearCode:
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
                                if(listBox.Items.Contains(labelCode.Text + s))
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

            if(msg.Length>0)
            {
                e.Cancel = true;
                MessageBox.Show(msg, "Validation error",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                    msg = s + " already in use. Please use another";
                }
            }
            else
            {
                if (_action == global.fad3GearEditAction.addLocalName)
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

        void AddNewGearLocalName()
        {
            ;
        }
    }
}
