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
    public partial class SampledGear_SpecsForm : Form
    {
        public SampledGear_SpecsForm(string gearVarGuid, string gearVarName, SamplingForm parent_Form)
        {
            InitializeComponent();
            _gearVarGuid = gearVarGuid;
            _gearVarName = gearVarName;
            SetupGearSpecsList();
            _parent_form = parent_Form;
        }

        private void SetupGearSpecsList()
        {
            ManageGearSpecsClass.GearVariation(_gearVarGuid, _gearVarName);
            _listGearSpecs = ManageGearSpecsClass.GearSpecifications;
        }

        private string _samplingGUID;
        private static SampledGear_SpecsForm _instance;
        private string _gearVarGuid;
        private string _gearVarName;
        private List<ManageGearSpecsClass.GearSpecification> _listGearSpecs = new List<ManageGearSpecsClass.GearSpecification>();
        private bool _isNew = true;
        private bool _sampledGearSpecDataIsEdited = false;
        private SamplingForm _parent_form;

        public string SamplingGUID
        {
            get { return _samplingGUID; }
            set
            {
                _samplingGUID = value;
                if (_samplingGUID.Length > 0)
                {
                    ManageGearSpecsClass.SamplingGuid = _samplingGUID;

                    //if a gear spec template for the sampled gear exists (_GearSpecs.Count>0) and
                    //the sampled gear has spec data in the database OR there are unsaved edits
                    if (_listGearSpecs.Count > 0 && (ManageGearSpecsClass.HasSampledGearSpecs || ManageGearSpecsClass.HasUnsavedSampledGearSpecEdits))
                    {
                        FillFields(); //fills the form controls with the specifications data
                    }
                }
            }
        }

        public static SampledGear_SpecsForm GetInstance(string GearVarGuid, string GearVarName, SamplingForm Parent_Form)
        {
            if (_instance == null) _instance = new SampledGear_SpecsForm(GearVarGuid, GearVarName, Parent_Form);
            return _instance;
        }

        private void SetupFields()
        {
            var x = 10;
            var WidestLabel = 0;
            var y = 0;
            var spacer = 15;
            var ControlHeight = 0;
            foreach (ManageGearSpecsClass.GearSpecification spec in _listGearSpecs)
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

                c.Tag = spec.Type + "|" + fad3DataStatus.statusFromDB.ToString();
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

        private void OnSpecsForm_Load(object sender, EventArgs e)
        {
            //show a message that gear variation has no template specs
            if (_listGearSpecs.Count == 0)
            {
                labelTitle.Visible = false;
                buttonOk.Visible = buttonCancel.Visible = labelTitle.Visible;

                Label lbl = new Label
                {
                    Text = "Specifications for this gear variation does not exist. " +
                            "Do you want to create one now?",
                    Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular,
                           GraphicsUnit.Point, (byte)(0)),
                    AutoSize = false,
                    Width = panelUI.Width - 20,
                    Height = 80,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Name = "labelNoSpecs"
                };

                Button btnOk = new Button
                {
                    Text = "Ok",
                    Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular,
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
                SetupFields();
                //var x = 10;
                //var WidestLabel = 0;
                //var y = 0;
                //var spacer = 15;
                //var ControlHeight = 0;
                //foreach (ManageGearSpecsClass.GearSpecification spec in _listGearSpecs)
                //{
                //    Label lbl = new Label
                //    {
                //        Text = spec.Property,
                //        AutoSize = true,
                //        Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular,
                //           GraphicsUnit.Point, (byte)(0)),
                //        Name = "lbl" + spec.Property.Trim().Replace(" ", string.Empty)
                //    };
                //    ControlHeight = lbl.Height;

                //    Control c;
                //    if (spec.Type != "Yes/No")
                //    {
                //        TextBox txt = new TextBox();
                //        txt.Validating += OnTextValidating;
                //        c = txt;
                //    }
                //    else
                //    {
                //        ComboBox cbo = new ComboBox
                //        {
                //            AutoCompleteSource = AutoCompleteSource.ListItems,
                //            AutoCompleteMode = AutoCompleteMode.Suggest,
                //        };
                //        cbo.Validating += OnComboValidating;
                //        c = cbo;
                //        cbo.Items.Add("Yes");
                //        cbo.Items.Add("No");
                //    }

                //    c.Tag = spec.Type + "|" + fad3DataStatus.statusFromDB.ToString();
                //    var arr = spec.Property.Split(' ');
                //    c.Name = spec.RowGuid;

                //    if (spec.Notes.Length > 0)
                //    {
                //        ToolTip tt = new ToolTip();
                //        tt.SetToolTip(c, spec.Notes);
                //    }

                //    panelUI.Controls.Add(lbl);
                //    lbl.Location = new Point(x, y);
                //    y += (ControlHeight + spacer);
                //    if (lbl.Width > WidestLabel) WidestLabel = lbl.Width;

                //    panelUI.Controls.Add(c);
                //    c.Location = lbl.Location;
                //}

                //foreach (Control c in panelUI.Controls)
                //{
                //    if (c.GetType().Name == "TextBox" ||
                //       c.GetType().Name == "ComboBox")
                //    {
                //        c.Left = WidestLabel + (spacer);
                //        c.Width = panelUI.Width - WidestLabel - (spacer * 3);
                //    }
                //}
            }
            Text = "Gear specifications for " + _gearVarName;
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
                        _isNew = false;
                    }
                }
            }
        }

        private void HideNoSpecsControls()
        {
        }

        public void RefreshSpecs()
        {
            SetupGearSpecsList();
            panelUI.Controls.Clear();
            labelTitle.Visible = buttonOk.Visible = buttonCancel.Visible = true;
            SetupFields();
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
                    ManageGearSpecsForm mf = new ManageGearSpecsForm(_gearVarGuid, _gearVarName, this);
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
                    var ds = fad3DataStatus.statusFromDB.ToString();
                    if (_isNew)
                    {
                        ds = fad3DataStatus.statusNew.ToString();
                        _sampledGearSpecDataIsEdited = true;
                    }
                    else
                        ds = fad3DataStatus.statusEdited.ToString();

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
                        var ds = fad3DataStatus.statusFromDB.ToString();
                        if (_isNew)
                        {
                            ds = fad3DataStatus.statusNew.ToString();
                            _sampledGearSpecDataIsEdited = true;
                        }
                        else
                            ds = fad3DataStatus.statusEdited.ToString();

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
            if (_isNew)
            {
                foreach (Control c in panelUI.Controls)
                {
                    if (c.GetType().Name == "TextBox" || c.GetType().Name == "ComboBox")
                    {
                        _sampledGearSpecDataIsEdited = c.Text.Length > 0;
                        if (_sampledGearSpecDataIsEdited) break;
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, ManageGearSpecsClass.SampledGearSpecData> kv in ManageGearSpecsClass.SampledGearSpecs)
                {
                    _sampledGearSpecDataIsEdited = panelUI.Controls[kv.Value.SpecificationGuid].Text != kv.Value.SpecificationValue;
                    if (_sampledGearSpecDataIsEdited) break;
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

            if (_sampledGearSpecDataIsEdited)
            {
                ManageGearSpecsClass.SetSampledGearSpecsForPreSave();
                foreach (Control c in panelUI.Controls)
                {
                    var spec = new ManageGearSpecsClass.SampledGearSpecData();
                    if (c.GetType().Name == "TextBox" || c.GetType().Name == "ComboBox")
                    {
                        var arr = c.Tag.ToString().Split('|');
                        if (_isNew) spec.RowID = Guid.NewGuid().ToString();
                        spec.SpecificationGuid = c.Name;
                        spec.SpecificationValue = c.Text;
                        spec.SpecificationName = ManageGearSpecsClass.SpecNameFromSpecGUID(spec.SpecificationGuid);
                        var ds = fad3DataStatus.statusFromDB;
                        if (Enum.TryParse(arr[1], out ds)) spec.DataStatus = ds;
                        spec.SamplingGuid = _samplingGUID;

                        ManageGearSpecsClass.SampledGearSpecs.Add(spec.SpecificationGuid, spec);
                    }
                }
                _parent_form.SampledGearSpecIsEdited = true;
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