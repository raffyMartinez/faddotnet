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

        public GMSDataEntryForm(bool IsNew, sampling sampling, string CatchRowGuid, string CatchName)
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
                    AddRow(kv.Value.Length, kv.Value.Weight, kv.Value.Sex, kv.Value.GMS, kv.Value.Taxa);
                }
            }
        }

        private void AddRow(double? Len = null, double? Wgt = null,
                            GMSManager.sex Sex = GMSManager.sex.Female,
                            GMSManager.FishCrabGMS GMS = GMSManager.FishCrabGMS.AllTaxaNotDetermined,
                            GMSManager.Taxa taxa = GMSManager.Taxa.Fish)
        {
            var x = 3;
            Label labelRow = new Label();
            TextBox textLength = new TextBox();
            TextBox textWeight = new TextBox();
            TextBox textGonadWeight = new TextBox();
            ComboBox cboSex = new ComboBox();
            ComboBox cboGMS = new ComboBox();

            panelUI.Controls.With(o =>
            {
                o.Add(labelRow);
                o.Add(textLength);
                o.Add(textWeight);
                o.Add(cboSex);
                o.Add(cboGMS);
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
            });

            textWeight.With(o =>
            {
                o.Width = 60;
                o.Name = "textWgt";
                o.Location = new Point(textLength.Left + textLength.Width + _spacer, _y);
                if (!_IsNew && Wgt != null) o.Text = Len.ToString();
            });

            cboSex.With(o =>
            {
                o.Width = 120;
                o.Name = "cboSex";
                o.Location = new Point(textWeight.Left + textWeight.Width + _spacer, _y);
            });

            cboGMS.With(o =>
            {
                o.Width = 120;
                o.Name = "cboGMS";
                o.Location = new Point(cboSex.Left + cboSex.Width + _spacer, _y);
            });

            textGonadWeight.With(o =>
            {
                o.Width = 60;
                o.Name = "textGonadWeight";
                o.Location = new Point(cboGMS.Left + cboGMS.Width + _spacer, _y);
            });

            _y += labelRow.Height + _spacer;
            _row++;
        }
    }
}