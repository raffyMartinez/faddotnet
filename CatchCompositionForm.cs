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
    public partial class CatchCompositionForm : Form
    {
        private MainForm _parentForm;
        private string _SamplingGuid;
        private bool _IsNew;

        private ComboBox _comboIdentificationType = new ComboBox();
        private ComboBox _comboGenus = new ComboBox();
        private ComboBox _comboSpecies = new ComboBox();
        private ComboBox _comboLocalName = new ComboBox();
        private int row = 1;
        private int _y = 0;
        private int _ctlHeight;
        private int _ctlWidth = 0;
        private int _spacer = 3;

        public CatchCompositionForm(bool IsNew, MainForm parent, string SamplingGuid)
        {
            InitializeComponent();
            _parentForm = parent;
            _SamplingGuid = SamplingGuid;
            _IsNew = IsNew;
        }

        private bool ValidateForm()
        {
            return true;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (ValidateForm())
                    {
                        if (CatchComposition.UpdateCatchComposition())
                        {
                            Close();
                        }
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;

                case "buttonAdd":
                    break;

                case "buttonRemove":
                    break;
            }
        }

        private void AddRow(bool IsNew, CatchComposition.Identification IDType = CatchComposition.Identification.Scientific,
                            string NameGuid = "", string Name1 = "", string Name2 = "", double catchWeight = 0, long? catchCount = null,
                            double? subWeight = null, long? subCount = null, bool FromTotal = false)
        {
            int x = 0;

            Label labelRow = new Label();
            TextBox textIdentificationType = new TextBox();
            TextBox textName1 = new TextBox();
            TextBox textName2 = new TextBox();
            TextBox textWt = new TextBox();
            TextBox textCount = new TextBox();
            TextBox textSubWt = new TextBox();
            TextBox textSubCount = new TextBox();
            if (row == 1)
            {
                _comboIdentificationType.With(o =>
                {
                    o.Width = 120;
                    o.Name = "cboIdentificationType";
                    o.Location = new Point(0, 0);
                    o.Visible = false;
                    o.DropDownStyle = ComboBoxStyle.DropDownList;
                    o.DataSource = new BindingSource(CatchComposition.IdentificationTypes(), null);
                    o.DisplayMember = "Value";
                    o.ValueMember = "Key";
                    o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    o.Font = Font;
                    _ctlHeight = o.Height;
                    o.Validated += OnComboValidated;
                });

                _comboGenus.With(o =>
                {
                    o.Width = 120;
                    o.Font = Font;
                    o.Name = "cboGenus";
                    o.Location = new Point(0, 0);
                    o.DropDownStyle = ComboBoxStyle.DropDownList;
                    o.Validated += OnComboValidated;
                    //o.DataSource = new BindingSource(gmsDict, null);
                    o.DisplayMember = "Value";
                    o.ValueMember = "Key";
                    o.AutoCompleteSource = AutoCompleteSource.ListItems;

                    o.Visible = false;
                });

                _comboSpecies.With(o =>
                {
                    o.Width = 120;
                    o.Font = Font;
                    o.Name = "cboSpecies";
                    o.Location = new Point(0, 0);
                    o.DropDownStyle = ComboBoxStyle.DropDownList;
                    o.Validated += OnComboValidated;
                    //o.DataSource = new BindingSource(gmsDict, null);
                    o.DisplayMember = "Value";
                    o.ValueMember = "Key";
                    o.AutoCompleteSource = AutoCompleteSource.ListItems;

                    o.Visible = false;
                });

                _comboLocalName.With(o =>
                {
                    o.Width = 120;
                    o.Font = Font;
                    o.Name = "cboLocalName";
                    o.Location = new Point(0, 0);
                    o.DropDownStyle = ComboBoxStyle.DropDownList;
                    o.Validated += OnComboValidated;
                    //o.DataSource = new BindingSource(gmsDict, null);
                    o.DisplayMember = "Value";
                    o.ValueMember = "Key";
                    o.AutoCompleteSource = AutoCompleteSource.ListItems;

                    o.Visible = false;
                });

                panelUI.Controls.Add(_comboIdentificationType);
                panelUI.Controls.Add(_comboGenus);
                panelUI.Controls.Add(_comboSpecies);
                panelUI.Controls.Add(_comboLocalName);
            }

            labelRow.With(o =>
            {
                o.Name = "labelRow";
                o.Location = new Point(x, _y);
                o.Text = row.ToString();
                o.Font = Font;
                o.Width = 40;
            });

            textIdentificationType.With(o =>
            {
                _ctlWidth = o.Width = 100;
                o.Height = _ctlHeight;
                o.Location = new Point(labelRow.Left + labelRow.Width + _spacer, _y);
                o.Name = "txtIdentificationType";
                o.Font = Font;
                o.Text = CatchComposition.IdentificationTypeToString(IDType);
            });

            textName1.With(o =>
            {
                o.Width = (int)(_ctlWidth * 1.2);
                o.Height = _ctlHeight;
                o.Location = new Point(textIdentificationType.Left + textIdentificationType.Width + _spacer, _y);
                o.Name = "txtName1";
                o.Font = Font;
                if (!IsNew) o.Text = Name1;
            });

            textName2.With(o =>
            {
                o.Width = (int)(_ctlWidth * 1.2);
                o.Height = _ctlHeight;
                o.Location = new Point(textName1.Left + textName1.Width + _spacer, _y);
                o.Name = "txtName2";
                o.Font = Font;
                if (!IsNew) o.Text = Name2;
            });

            textWt.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.5);
                o.Height = _ctlHeight;
                o.Location = new Point(textName2.Left + textName2.Width + _spacer, _y);
                o.Name = "txtWt";
                o.Font = Font;
                if (!IsNew) o.Text = catchWeight.ToString();
            });

            textCount.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.5);
                o.Height = _ctlHeight;
                o.Location = new Point(textWt.Left + textWt.Width + _spacer, _y);
                o.Name = "txtCount";
                o.Font = Font;
                if (!IsNew) o.Text = catchCount.ToString();
            });

            textSubWt.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.5);
                o.Height = _ctlHeight;
                o.Location = new Point(textCount.Left + textCount.Width + _spacer, _y);
                o.Name = "txtSubWt";
                o.Font = Font;
                if (!IsNew) o.Text = subWeight.ToString();
            });

            textSubCount.With(o =>
            {
                o.Width = (int)(_ctlWidth * 0.5);
                o.Height = _ctlHeight;
                o.Location = new Point(textSubWt.Left + textSubWt.Width + _spacer, _y);
                o.Name = "txtSubCount";
                o.Font = Font;
                if (!IsNew) o.Text = subCount.ToString();
            });

            panelUI.Controls.Add(labelRow);
            panelUI.Controls.Add(textIdentificationType);
            panelUI.Controls.Add(textName1);
            panelUI.Controls.Add(textName2);
            panelUI.Controls.Add(textWt);
            panelUI.Controls.Add(textCount);
            panelUI.Controls.Add(textSubWt);
            panelUI.Controls.Add(textSubCount);

            _y += labelRow.Height + _spacer;
            row++;
        }

        private void CatchCompositionForm_Load(object sender, EventArgs e)
        {
            if (_IsNew)
            {
                AddRow(IsNew: true);
            }
            else
            {
                foreach (var item in CatchComposition.CatchComp(_SamplingGuid))
                {
                    AddRow(IsNew: false, item.Value.NameType, item.Value.CatchNameGUID,
                           item.Value.Name1, item.Value.Name2, item.Value.CatchWeight,
                           item.Value.CatchCount, item.Value.CatchSubsampleWt, item.Value.CatchSubsampleCount,
                           item.Value.FromTotalCatch);
                }
            }
        }

        private void OnComboValidated(object sender, EventArgs e)
        {
        }
    }
}