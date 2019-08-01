using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FAD3
{
    public partial class ReferenceNumberForm : Form
    {
        private SamplingForm _parent_Form;
        private string _nextRefCode = "";

        public SamplingForm Parent_Form
        {
            get { return _parent_Form; }
            set { _parent_Form = value; }
        }

        public ReferenceNumberForm()
        {
            InitializeComponent();
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonReset":
                    buttonReset.Visible = false;
                    ReferenceNumberManager.ResetReferenceNumbers();
                    _nextRefCode = ReferenceNumberManager.GetNextReferenceNumber(ReferenceNumberManager.FirstCode);
                    labelRefNo.Visible = true;
                    labelRefNo.Text = _nextRefCode;
                    break;

                case "buttonOK":
                    if (_nextRefCode.Length > 0)
                    {
                        _parent_Form.NewReferenceNumber(_nextRefCode);
                        Close();
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;
            }
        }

        private void OnForm_Load(object sender, EventArgs e)
        {
            Text = "Generate reference number";

            if (ReferenceNumberManager.Count == 1)
            {
                lvCodes.Visible = false;
                labelTitle.Text = "A reference number was generated for the sampling.";
                labelRefNo.With(o =>
                {
                    o.Visible = true;
                    _nextRefCode = ReferenceNumberManager.GetNextReferenceNumber(ReferenceNumberManager.FirstCode);

                    if (ReferenceNumberManager.RefNumberIsFree(_nextRefCode))
                    {
                        o.Text = _nextRefCode;
                    }
                    else
                    {
                        buttonReset.Visible = true;
                        o.Visible = false;
                    }
                });
            }
            else
            {
                labelTitle.Text = "Select code of the gear that was used in the sampling";
                labelRefNo.Location = new Point(lvCodes.Location.X, lvCodes.Location.Y + lvCodes.Height + 5);
                labelRefNo.TextAlign = ContentAlignment.MiddleLeft;
                labelRefNo.Text = "";
            }
            lvCodes.With(o =>
            {
                o.View = View.Details;
                o.Columns.Add("Code");
                o.Columns.Add("Sub-variation");
                o.Columns.Add("Local names");
                o.FullRowSelect = true;
            });
            foreach (KeyValuePair<string, ReferenceNumberManager.VariationCode> kv in ReferenceNumberManager.VariationCodes)
            {
                var lvi = new ListViewItem(new string[] { kv.Value.GearCode, kv.Value.IsSubVariation.ToString(), kv.Value.LocalNames });
                lvCodes.Items.Add(lvi);
            }

            foreach (ColumnHeader c in lvCodes.Columns)
            {
                switch (c.Text)
                {
                    case "Code":
                    case "Sub-variation":
                        c.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                        break;

                    case "Local names":
                        c.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                        break;
                }
            }
        }

        private void OnlvCodes_MouseDown(object sender, MouseEventArgs e)
        {
            var item = lvCodes.HitTest(e.X, e.Y).Item;
            if (item != null)
            {
                _nextRefCode = ReferenceNumberManager.GetNextReferenceNumber(item.Text);
                labelRefNo.Text = _nextRefCode;
            }
            else
                labelRefNo.Text = "";
        }
    }
}