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
    public partial class LengthFreqForm : Form
    {
        private string _CatchRowGuid = "";
        private string _CatchName = "";
        private bool _IsNew = false;
        private sampling _sampling;
        private int _spacer = 3;

        private int _y = 0;
        private int _row = 1;
        private int _labelAdjust = 2;
        private TextBox _lastLen;
        private TextBox _LastFreq;
        private bool _UpdateSequence = false;

        public LengthFreqForm(bool IsNew, sampling sampling, string CatchRowGuid, string CatchName)
        {
            InitializeComponent();
            _CatchRowGuid = CatchRowGuid;
            _CatchName = CatchName;
            _sampling = sampling;
            _IsNew = IsNew;
            labelTitle.Text = "Length-frequency table for " + CatchName;
            if (IsNew)
            {
                labelTitle.Text = "New length-frequency table for " + CatchName;
            }
            PopulateFieldControls(IsNew);
        }

        /// <summary>
        /// Dynamically adds a row of controls
        /// </summary>
        private void AddRow(string Len = "", string Freq = "", string LFRowGuid = "")
        {
            var x = 3;
            Label labelRow = new Label
            {
                Width = 30,
                Font = this.Font
            };
            TextBox textLen = new TextBox
            {
                Font = this.Font,
                Width = 60,
                Name = "textLen"
            };
            TextBox textFreq = new TextBox
            {
                Font = this.Font,
                Width = 60,
                Name = "textFreq"
            };

            panelUI.Controls.Add(labelRow);
            labelRow.With(o =>
            {
                o.Location = new Point(x, _y + _labelAdjust);
                o.Text = _row.ToString();
            });

            panelUI.Controls.Add(textLen);
            textLen.With(o =>
            {
                o.Location = new Point(labelRow.Left + labelRow.Width + _spacer, _y);
                o.Text = Len;
            });

            panelUI.Controls.Add(textFreq);
            textFreq.With(o =>
            {
                o.Location = new Point(textLen.Left + textLen.Width + _spacer, _y);
                o.Text = Freq;
            });
            _lastLen = textLen;
            _LastFreq = textFreq;
            if (Len.Length == 0 && Freq.Length == 0 && LFRowGuid.Length == 0)
            {
                var thisTag = new Tuple<int, string, global.fad3DataStatus>(_row, Guid.NewGuid().ToString(), global.fad3DataStatus.statusNew);
                textLen.Tag = thisTag;
                textFreq.Tag = thisTag;
            }
            else
            {
                var ds = global.fad3DataStatus.statusFromDB;
                if (_UpdateSequence) ds = global.fad3DataStatus.statusEdited;
                var thisTag = new Tuple<int, string, global.fad3DataStatus>(_row, LFRowGuid, ds);
                textLen.Tag = thisTag;
                textFreq.Tag = thisTag;
            }

            textLen.Validating += OnTextValidating;
            textFreq.Validating += OnTextValidating;
            textLen.TextChanged += OnTextChanged;
            textFreq.TextChanged += OnTextChanged;

            _y += labelRow.Height + _spacer;
            _row++;
        }

        /// <summary>
        /// Populates fields if LF data exists or adds a new row
        /// </summary>
        /// <param name="IsNew"></param>
        private void PopulateFieldControls(bool IsNew)
        {
            if (IsNew)
            {
                AddRow();
            }
            else
            {
                foreach (KeyValuePair<string, sampling.LFLine> kv in _sampling.LFData(_CatchRowGuid))
                {
                    if (!_UpdateSequence) _UpdateSequence = kv.Value.Sequence == -1;
                    AddRow(kv.Value.Length.ToString(), kv.Value.Freq.ToString(), kv.Value.LFRowGuid);
                }
            }
        }

        /// <summary>
        /// validate Len-Freq field entries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTextValidating(object sender, CancelEventArgs e)
        {
            var msg = "";
            ((TextBox)sender).With(o =>
            {
                var s = o.Text;
                if (s.Length > 0)
                {
                    switch (o.Name)
                    {
                        case "textLen":
                            //could be a double
                            Double Len;
                            msg = "Expected value is a number greater than zero";
                            if (double.TryParse(s, out Len))
                            {
                                if (Len <= 0)
                                    e.Cancel = true;
                            }
                            else
                            {
                                e.Cancel = true;
                            }

                            break;

                        case "textFreq":
                            //must be an int
                            int Freq;
                            msg = "Expected value is a whole number greater than zero";
                            if (int.TryParse(s, out Freq))
                            {
                                //test if input is a whole number
                                if (Freq <= 0 || int.Parse(s) != Freq)
                                    e.Cancel = true;
                            }
                            else
                            {
                                e.Cancel = true;
                            }
                            break;
                    }
                }
            });

            if (e.Cancel)
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    var ValidationPassed = true;
                    foreach (Control c in panelUI.Controls)
                    {
                        if (c.GetType().Name == "TextBox")
                        {
                            if (c.Text.Length == 0)
                            {
                                ValidationPassed = false;
                                break;
                            }
                        }
                    }

                    if (ValidationPassed)
                    {
                        if (SaveLFData()) Close();
                    }
                    else
                    {
                        MessageBox.Show("Cannot save until all fields are filled up",
                                        "Validation error", MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                    }

                    break;

                case "buttonCancel":
                    Close();
                    break;

                case "buttonAdd":
                    if (_lastLen.Text.Length > 0 && _LastFreq.Text.Length > 0)
                        AddRow();
                    else
                        MessageBox.Show("You have to fill up all fields",
                                        "Validation error", MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                    break;

                case "buttonRemove":
                    break;
            }
        }

        private bool SaveLFData()
        {
            var LFDict = new Dictionary<string, sampling.LFLine>();
            foreach (Control c in panelUI.Controls)
            {
                if (c.GetType().Name == "TextBox")
                {
                    ((TextBox)c).With(o =>
                    {
                        var s = o.Text;
                        var thisTag = (Tuple<int, string, global.fad3DataStatus>)o.Tag;
                        var myLF = new sampling.LFLine();
                        switch (o.Name)
                        {
                            case "textLen":
                                myLF.Length = double.Parse(s);
                                var result = GetFreqValue(thisTag.Item1);
                                myLF.Freq = result.Item1;
                                myLF.DataStatus = thisTag.Item3;
                                if (myLF.DataStatus == global.fad3DataStatus.statusFromDB)
                                    myLF.DataStatus = result.Item2;
                                myLF.LFRowGuid = thisTag.Item2;
                                myLF.CatchRowGuid = _CatchRowGuid;
                                myLF.Sequence = thisTag.Item1;
                                LFDict.Add(myLF.LFRowGuid, myLF);
                                break;
                        }
                    });
                }
            }

            return _sampling.SaveEditedLF(LFDict);
        }

        private (int, global.fad3DataStatus) GetFreqValue(int rowNumber)
        {
            int rv = 0;
            global.fad3DataStatus ds = global.fad3DataStatus.statusFromDB;
            foreach (Control c in panelUI.Controls)
            {
                if (c.GetType().Name == "TextBox")
                {
                    var thisTag = (Tuple<int, string, global.fad3DataStatus>)c.Tag;
                    if (thisTag.Item1 == rowNumber && c.Name == "textFreq")
                    {
                        rv = int.Parse(c.Text);
                        ds = thisTag.Item3;
                        break;
                    }
                }
            }
            return (rv, ds);
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).With(o =>
            {
                var thisTag = (Tuple<int, string, global.fad3DataStatus>)o.Tag;
                if (thisTag.Item3 != global.fad3DataStatus.statusNew)
                {
                    var editedTag = new Tuple<int, string, global.fad3DataStatus>(thisTag.Item1, thisTag.Item2, global.fad3DataStatus.statusEdited);
                    o.Tag = editedTag;
                }
            });
        }
    }
}