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
        public SampledGear_SpecsForm(string GearVarGuid, string GearVarName)
        {
            InitializeComponent();
            _GearVarGuid = GearVarGuid;
            _GearVarName = GearVarName;
            ManageGearSpecsClass.GearVariation(_GearVarGuid, GearVarName);
            _GearSpecs = ManageGearSpecsClass.GearSpecifications;
        }

        private string _SamplingGUID;
        private static SampledGear_SpecsForm _instance;
        private string _GearVarGuid;
        private string _GearVarName;
        private List<ManageGearSpecsClass.GearSpecification> _GearSpecs = new List<ManageGearSpecsClass.GearSpecification>();
        private Dictionary<string, ManageGearSpecsClass.SampledGearSpecData> _SampledGearSpecs = new Dictionary<string, ManageGearSpecsClass.SampledGearSpecData>();
        private bool IsNew = true;

        public string SamplingGUID
        {
            get { return _SamplingGUID; }
            set
            {
                _SamplingGUID = value;
                ManageGearSpecsClass.SamplingGuid = _SamplingGUID;
                if (_GearSpecs.Count > 0 && ManageGearSpecsClass.HasSampledGearSpecs)
                {
                    _SampledGearSpecs = ManageGearSpecsClass.SampledGearSpecs;
                    FillFields();
                }
            }
        }

        public static SampledGear_SpecsForm GetInstance(string GearVarGuid, string GearVarName)
        {
            if (_instance == null) _instance = new SampledGear_SpecsForm(GearVarGuid, GearVarName);
            return _instance;
        }

        private void OnSpecsForm_Load(object sender, EventArgs e)
        {
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
                    //for (int n = 0; n < arr.Length; n++)
                    //{
                    //    arr[n] = arr[n].Substring(0, 1).ToUpper() + arr[n].Substring(1, arr[n].Length - 1);
                    //    c.Name += arr[n];
                    //}

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

        private void FillFields()
        {
            if (ManageGearSpecsClass.HasSampledGearSpecs)
            {
                foreach (Control c in panelUI.Controls)
                {
                    if ((c.GetType().Name == "TextBox" || c.GetType().Name == "ComboBox") &&
                         _SampledGearSpecs.Keys.Contains(c.Name))
                    {
                        var spec = _SampledGearSpecs[c.Name];
                        c.Text = spec.SpecValue;
                        IsNew = false;
                    }
                }
            }
        }

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
                        ds = global.fad3DataStatus.statusNew.ToString();
                    else
                        ds = global.fad3DataStatus.statusEdited.ToString();

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
                            ds = global.fad3DataStatus.statusNew.ToString();
                        else
                            ds = global.fad3DataStatus.statusEdited.ToString();

                        var arr = o.Tag.ToString().Split('|');
                        o.Tag = arr[0] + "|" + ds;
                    }
                }
            });
        }

        private bool SaveSampledGearSpecs()
        {
            _SampledGearSpecs.Clear();
            foreach (Control c in panelUI.Controls)
            {
                var spec = new ManageGearSpecsClass.SampledGearSpecData();
                if (c.GetType().Name=="TextBox" || c.GetType().Name == "ComboBox" )
                {
                    var arr = c.Tag.ToString().Split('|');
                    if (IsNew) spec.RowID = Guid.NewGuid().ToString();
                    spec.SpecGUID = c.Name;
                    spec.SpecValue = c.Text;
                    var ds = global.fad3DataStatus.statusFromDB;
                    if (Enum.TryParse(arr[1], out ds)) spec.DataStatus = ds;
                    spec.SamplingGUID = _SamplingGUID;
                    _SampledGearSpecs.Add(spec.SpecGUID, spec);
                }
            }



            return ManageGearSpecsClass.SaveSampledGearSpecs(_SampledGearSpecs);
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOk":
                    if (SaveSampledGearSpecs()) Close();
                    break;
                case "buttonCancel":
                    Close();
                    break;
            }
        }
    }
}
