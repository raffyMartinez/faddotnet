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
    public partial class GMSDataEntryForm : Form
    {
        private string _CatchName = "";
        private string _CatchRowGuid = "";
        private bool _IsNew = false;
        private int _labelAdjust = 2;
        private TextBox _LastWeight;
        private TextBox _lastLength;
        private ComboBox _lastSex;
        private ComboBox _lastGMS;
        private int _row = 1;
        private sampling _sampling;
        private int _spacer = 3;
        private bool _UpdateSequence = false;
        private int _y = 5;
        private GMSManager.Taxa _taxa = GMSManager.Taxa.To_be_determined;
        private int _ctlHeight = 0;
        private int _ctlWidth = 0;

        public GMSDataEntryForm(bool IsNew, sampling sampling, string CatchRowGuid, string CatchName, GMSManager.Taxa taxa)
        {
            InitializeComponent();
            _CatchRowGuid = CatchRowGuid;
            _CatchName = CatchName;
            _sampling = sampling;
            _IsNew = IsNew;
            labelTitle.Text = $"GMS data table for {CatchName}";
            if (IsNew)
            {
                labelTitle.Text = $"New GMS data table for {CatchName}";
            }
            _taxa = taxa;
            PopulateFieldControls(IsNew);
        }

        /// <summary>
        /// Populates fields if LF data exists or adds a new row
        /// </summary>
        /// <param name="IsNew"></param>
        private void PopulateFieldControls(bool IsNew)
        {
            if (IsNew)
            {
                //adds a new row of empty fields
                AddRow();
            }
            else
            {
                foreach (KeyValuePair<string, GMSManager.GMSLine> kv in GMSManager.GMSData(_CatchRowGuid))
                {
                    //adds a new row with fields containing length and frequency
                    AddRow(kv.Value.Length, kv.Value.Weight, kv.Value.Sex, kv.Value.GMS, kv.Value.Taxa, kv.Value.GonadWeight);
                }
            }

            foreach (Control c in panelUI.Controls)
            {
                if (c.GetType().Name == "TextBox")
                {
                    ((TextBox)c).With(o =>
                    {
                        o.Font = Font;
                        o.Height = _ctlHeight;
                    });
                }
            }
        }

        private void AddRow(double? Len = null, double? Wgt = null,
                            GMSManager.sex Sex = GMSManager.sex.Female,
                            GMSManager.FishCrabGMS GMS = GMSManager.FishCrabGMS.AllTaxaNotDetermined,
                            GMSManager.Taxa taxa = GMSManager.Taxa.Fish, double? GonadWt = null)
        {
            var x = 3;
            Label labelRow = new Label();
            TextBox textLength = new TextBox();
            TextBox textWeight = new TextBox();
            TextBox textGonadWeight = new TextBox();
            TextBox textGMS = new TextBox();
            TextBox textSex = new TextBox();
            ComboBox cboSex = new ComboBox();
            ComboBox cboGMS = new ComboBox();

            //we only add the comboboxes once
            if (_row == 1)
            {
                cboSex.With(o =>
                    {
                        o.Width = 120;
                        o.Name = "cboSex";
                        o.Location = new Point(0, 0);
                        o.Visible = false;
                        o.DataSource = Enum.GetValues(typeof(GMSManager.sex));
                        o.AutoCompleteMode = AutoCompleteMode.Suggest;
                        o.AutoCompleteSource = AutoCompleteSource.ListItems;
                        o.Font = Font;
                        _ctlHeight = o.Height;
                    });

                cboGMS.With(o =>
                {
                    o.Width = 120;
                    o.Font = Font;
                    o.Name = "cboGMS";
                    o.Location = new Point(0, 0);
                    var hasGMSStage = false;
                    var gmsDict = GMSManager.GMSStages(_taxa, ref hasGMSStage);
                    if (hasGMSStage)
                    {
                        o.DataSource = new BindingSource(gmsDict, null);
                        o.DisplayMember = "Value";
                        o.ValueMember = "Key";
                        o.AutoCompleteMode = AutoCompleteMode.Suggest;
                        o.AutoCompleteSource = AutoCompleteSource.ListItems;
                    }
                    else
                    {
                    }
                    o.Visible = false;
                });

                panelUI.Controls.Add(cboSex);
                panelUI.Controls.Add(cboGMS);
            }

            panelUI.Controls.With(o =>
            {
                o.Add(labelRow);
                o.Add(textLength);
                o.Add(textWeight);
                o.Add(textSex);
                o.Add(textGMS);
                o.Add(textGonadWeight);
            });

            labelRow.With(o =>
            {
                o.Text = _row.ToString();
                o.Location = new Point(x, _y + _labelAdjust);
                o.Width = 30;
            });

            textLength.With(o =>
            {
                o.Width = 60;
                o.Name = "textLen";
                o.Location = new Point(labelRow.Left + labelRow.Width + _spacer, _y);
                if (!_IsNew && Len != null) o.Text = Len.ToString();
                _ctlWidth = o.Width;
            });

            textWeight.With(o =>
            {
                o.Width = 60;
                o.Name = "textWgt";
                o.Location = new Point(textLength.Left + textLength.Width + _spacer, _y);
                if (!_IsNew && Wgt != null) o.Text = Wgt.ToString();
            });

            textSex.With(o =>
            {
                o.Width = 60;
                o.Name = "textSex";
                o.Location = new Point(textWeight.Left + textWeight.Width + _spacer, _y);
                if (!_IsNew) o.Text = Sex.ToString();
                o.Width += (int)(_ctlWidth * 0.5);
            });

            textGMS.With(o =>
            {
                o.Width = 60;
                o.Name = "textGMS";
                o.Location = new Point(textSex.Left + textSex.Width + _spacer, _y);
                o.Width += _ctlWidth;
                if (!_IsNew) o.Text = GMSManager.GMSStageToString(taxa, GMS);
            });

            textGonadWeight.With(o =>
            {
                o.Width = 60;
                o.Name = "textGonadWeight";
                o.Location = new Point(textGMS.Left + textGMS.Width + _spacer, _y);
                if (!_IsNew && GonadWt != null) o.Text = GonadWt.ToString();
            });

            _y += labelRow.Height + _spacer;
            _row++;
        }
    }
}