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
    public partial class GenerateRefNumberForm : Form
    {

        frmSamplingDetail _Parent_Form;
        string _NextRefCode = "";

        public frmSamplingDetail Parent_Form
        {
            get { return _Parent_Form; }
            set { _Parent_Form = value; }
        }

        public GenerateRefNumberForm()
        {
            InitializeComponent();
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (_NextRefCode.Length > 0)
                    {
                        _Parent_Form.NewReferenceNumber(_NextRefCode);
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
                    _NextRefCode = ReferenceNumberManager.GetNextReferenceNumber(ReferenceNumberManager.FirstCode);
                    o.Text = _NextRefCode;
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
                _NextRefCode = ReferenceNumberManager.GetNextReferenceNumber(item.Text);
                labelRefNo.Text = _NextRefCode;
            }
            else
                labelRefNo.Text = "";
        }
    }
}
