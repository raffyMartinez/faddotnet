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
    public partial class SampledGear_SpecsForm : Form
    {
        public SampledGear_SpecsForm(string GearVarGuid, string GearVarName, SamplingForm Parent_Form)
        {
            InitializeComponent();
            _GearVarGuid = GearVarGuid;
            _GearVarName = GearVarName;
            ManageGearSpecsClass.GearVariation(_GearVarGuid, GearVarName);
            _GearSpecs = ManageGearSpecsClass.GearSpecifications;
            _Parent_form = Parent_Form;
        }

        private string _SamplingGUID;
        private static SampledGear_SpecsForm _instance;
        private string _GearVarGuid;
        private string _GearVarName;
        private List<ManageGearSpecsClass.GearSpecification> _GearSpecs = new List<ManageGearSpecsClass.GearSpecification>();
        private bool IsNew = true;
        private bool _SampledGearSpecDataIsEdited = false;
        private SamplingForm _Parent_form;



        public string SamplingGUID
        {
            get { return _SamplingGUID; }
            set
            {
                _SamplingGUID = value;
                ManageGearSpecsClass.SamplingGuid = _SamplingGUID;

                //if a gear spec template for the sampled gear exists (_GearSpecs.Count>0) and
                //the sampled gear has spec data in the database OR there are unsaved edits
                if (_GearSpecs.Count > 0 && (ManageGearSpecsClass.HasSampledGearSpecs || ManageGearSpecsClass.HasUnsavedSampledGearSpecEdits))
                {
                    FillFields(); //fills the form controls with the specifications data
                }
            }
        }

        public static SampledGear_SpecsForm GetInstance(string GearVarGuid, string GearVarName, SamplingForm Parent_Form)
        {
            if (_instance == null) _instance = new SampledGear_SpecsForm(GearVarGuid, GearVarName, Parent_Form);
            return _instance;
        }

        private void OnSpecsForm_Load(object sender, EventArgs e)
        {
            //show a message that gear variation has no template specs
            if (_GearSpecs.Count == 0)
            {
                buttonOk.Visible = buttonCancel.Visible = labelTitle.Visible = false;

                Label lbl = new Label
                {
                    Text = "There is no template for specifications for this\r\n" +
                            "gear variation. Do you want to create one now?",
                    Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular,
                           GraphicsUnit.Point, (byte)(0)),
                    AutoSize = false,
                    Width = panelUI.Width - 20,
                    Height = 40,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Button btnOk = new Button
                {
                    Text = "Ok",
                    Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular,
                           GraphicsUnit.Point, (byte)(0)),
                    AutoSize = true,
                    Name = "buttonNoSpecsOk"
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Font = btnOk.Font,
                    AutoSize = true,
                    Name = "buttonNoSpecsCancel"
                };

                panelUI.Controls.Add(lbl);
                lbl.Location = new Point(10, panelUI.Height / 3);
                panelUI.Controls.Add(btnOk);
                btnOk.Location = new Point((lbl.Width / 2) + 20, lbl.Top + lbl.Height + 20);
                panelUI.Controls.Add(btnCancel);
                btnCancel.Location = new Point((lbl.Width / 2) - (20 + btnCancel.Width), lbl.Top + lbl.Height + 20);
                btnCancel.Click += OnButtonNoSpecs_Click;
                btnOk.Click += OnButtonNoSpecs_Click;


            }

            //Dynamically create fields based on the template
            else
            {
                var x = 10;
                var WidestLabel = 0;
                var y = 0;
                var spacer = 15;
                var ControlHeight = 0;
                foreach (ManageGearSpecsClass.GearSpecification spec in _GearSpecs)
                {
                    Label lbl = new Label
                    {
                        Text = spec.Property,
                        AutoSize = true,
                        Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular,
                           GraphicsUnit.Point, (byte)(0)),
                        Name = "lbl" + spec.Property.Trim().Replace(" ", string.Empty)

                    };
                    ControlHeight = lbl.Height;

                    Control c;
                    if (spec.Type != "Yes/No")
                    {
                        TextBox txt = new TextBox();
                        txt.Validating += OnTextValidating;
                        c = txt;
                    }
                    else
                    {
                        ComboBox cbo = new ComboBox
                        {
                            AutoCompleteSource = AutoCompleteSource.ListItems,
                            AutoCompleteMode = AutoCompleteMode.Suggest,
                        };
                        cbo.Validating += OnComboValidating;
                        c = cbo;
                        cbo.Items.Add("Yes");
                        cbo.Items.Add("No");
                    }

                    c.Tag = spec.Type + "|" + global.fad3DataStatus.statusFromDB.ToString();
                    var arr = spec.Property.Split(' ');
                    c.Name = spec.RowGuid;

                    if (spec.Notes.Length > 0)
                    {
                        ToolTip tt = new ToolTip();
                        tt.SetToolTip(c, spec.Notes);
                    }


                    panelUI.Controls.Add(lbl);
                    lbl.Location = new Point(x, y);
                    y += (ControlHeight + spacer);
                    if (lbl.Width > WidestLabel) WidestLabel = lbl.Width;

                    panelUI.Controls.Add(c);
                    c.Location = lbl.Location;
                }

                foreach (Control c in panelUI.Controls)
                {
                    if (c.GetType().Name == "TextBox" ||
                       c.GetType().Name == "ComboBox")
                    {
                        c.Left = WidestLabel + (spacer);
                        c.Width = panelUI.Width - WidestLabel - (spacer * 3);
                    }
                }



            }
            Text = "Gear specifications for " + _GearVarName;
            labelTitle.Text = "Enter the specifications of the gear used in the sampled fishing effort";


        }


        /// <summary>
        /// fill the controls (textbox and combobox) with the specifications data of the sampled gear
        /// </summary>
        private void FillFields()
        {
            if (ManageGearSpecsClass.HasSampledGearSpecs || ManageGearSpecsClass.HasUnsavedSampledGearSpecEdits)
            {
                foreach (Control c in panelUI.Controls)
                {
                    if ((c.GetType().Name == "TextBox" || c.GetType().Name == "ComboBox") &&
                         ManageGearSpecsClass.SampledGearSpecs.Keys.Contains(c.Name))
                    {
                        var spec = ManageGearSpecsClass.SampledGearSpecs[c.Name];
                        c.Text = spec.SpecificationValue;
                        IsNew = false;
                    }
                }
            }
        }

        /// <summary>
        /// Events for the buttons when there is no gear spec template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonNoSpecs_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonNoSpecsOk":
                    ManageGearSpecsForm mf = new ManageGearSpecsForm(_GearVarGuid, _GearVarName);
                    mf.ShowDialog(this);
                    break;
                case "buttonNoSpecsCancel":
                    Close();
                    break;
            }
        }

        private void OnSpecsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        private void OnTextValidating(object sender, CancelEventArgs e)
        {
            var msg = "";
            ((TextBox)sender).With(o =>
            {
                var arr = o.Tag.ToString().Split('|');
                var DataType = arr[0];
                var s = o.Text;

                if (s.Length > 0)
                {
                    switch (DataType)
                    {
                        case "Text":
                            if (s.Length < 4)
                            {
                                e.Cancel = true;
                                msg = "Value is too short";
                            }
                            break;
                        case "Numeric":
                            var v = 0D;
                            e.Cancel = double.TryParse(s, out v) == false;
                            if (e.Cancel)
                                msg = "Expected value must be a number";
                            break;
                    }
                }

                if (e.Cancel)
                    MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    var ds = global.fad3DataStatus.statusFromDB.ToString();
                    if (IsNew)
                    {
                        ds = global.fad3DataStatus.statusNew.ToString();
                        _SampledGearSpecDataIsEdited = true;
                    }
                    else
                        ds = global.fad3DataStatus.statusEdited.ToString();

                    //set the tag of the control
                    o.Tag = arr[0] + "|" + ds;
                }

            });


        }

        private void OnComboValidating(object sender, CancelEventArgs e)
        {
            ((ComboBox)sender).With(o =>
            {
                var s = o.Text;
                if (s.Length > 0)
                {
                    e.Cancel = o.Items.Contains(s) == false;
                    if (e.Cancel)
                        MessageBox.Show("Please select an item in the list",
                            "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                    {
                        var ds = global.fad3DataStatus.statusFromDB.ToString();
                        if (IsNew)
                        {
                            ds = global.fad3DataStatus.statusNew.ToString();
                            _SampledGearSpecDataIsEdited = true;
                        }
                        else
                            ds = global.fad3DataStatus.statusEdited.ToString();

                        var arr = o.Tag.ToString().Split('|');
                        o.Tag = arr[0] + "|" + ds;
                    }
                }
            });
        }


        /// <summary>
        /// determine if the form's data has been edited
        /// </summary>
        private void TestForEdits()
        {
            if (IsNew)
            {
                foreach(Control c in panelUI.Controls)
                {
                    if(c.GetType().Name =="TextBox" || c.GetType().Name=="ComboBox")
                    {
                        _SampledGearSpecDataIsEdited = c.Text.Length > 0;
                        if (_SampledGearSpecDataIsEdited) break;
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, ManageGearSpecsClass.SampledGearSpecData> kv in ManageGearSpecsClass.SampledGearSpecs)
                {
                    _SampledGearSpecDataIsEdited = panelUI.Controls[kv.Value.SpecificationGuid].Text != kv.Value.SpecificationValue;
                    if (_SampledGearSpecDataIsEdited) break;
                }
            }
        }


        /// <summary>
        /// replaces data from the sampled specs dictionary with the edited data
        /// </summary>
        private void PreSaveSampledGearSpecs()
        {
            //test if data is edited
            TestForEdits();

            if (_SampledGearSpecDataIsEdited)
            {
                ManageGearSpecsClass.SetSampledGearSpecsForPreSave();
                foreach (Control c in panelUI.Controls)
                {
                    var spec = new ManageGearSpecsClass.SampledGearSpecData();
                    if (c.GetType().Name == "TextBox" || c.GetType().Name == "ComboBox")
                    {
                        var arr = c.Tag.ToString().Split('|');
                        if (IsNew) spec.RowID = Guid.NewGuid().ToString();
                        spec.SpecificationGuid = c.Name;
                        spec.SpecificationValue = c.Text;
                        spec.SpecificationName = ManageGearSpecsClass.SpecNameFromSpecGUID(spec.SpecificationGuid);
                        var ds = global.fad3DataStatus.statusFromDB;
                        if (Enum.TryParse(arr[1], out ds)) spec.DataStatus = ds;
                        spec.SamplingGuid = _SamplingGUID;

                        ManageGearSpecsClass.SampledGearSpecs.Add(spec.SpecificationGuid, spec);
                    }
                }
                _Parent_form.SampledGearSpecIsEdited = true;
            }
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOk":
                    PreSaveSampledGearSpecs();
                    Close();
                    break;
                case "buttonCancel":
                    Close();
                    break;
            }
        }
    }
}
