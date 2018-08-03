using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FAD3
{
    public partial class LengthFreqForm : Form
    {
        private string _CatchName = "";
        private string _CatchRowGuid = "";
        private double _IntervalSize = .5;
        private bool _IsNew = false;
        private int _labelAdjust = 2;
        private TextBox _LastFreq;
        private TextBox _LastLen;
        private int _row = 1;
        private sampling _sampling;
        private int _spacer = 3;
        private bool _UpdateSequence = false;
        private int _y = 5;
        private bool _UniqueLenghtClasses = false;
        private List<double> _LengthClasses = new List<double>();
        private double _OldLength;

        public LengthFreqForm(bool IsNew, sampling sampling, string CatchRowGuid, string CatchName)
        {
            InitializeComponent();
            _CatchRowGuid = CatchRowGuid;
            _CatchName = CatchName;
            _sampling = sampling;
            _IsNew = IsNew;
            labelTitle.Text = $"Length-frequency table for {CatchName}";
            if (IsNew)
            {
                labelTitle.Text = $"New length-frequency table for {CatchName}";
            }
            checkUniqueIntervals.Checked = true;
            textIntervalSize.Text = _IntervalSize.ToString("0.00");
            checkUseSize.Checked = true;
            _UniqueLenghtClasses = checkUniqueIntervals.Checked;
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

            _LastLen = textLen;
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
            textLen.Enter += OnTextEntered;
            textLen.KeyDown += OnText_KeyDown;

            _y += labelRow.Height + _spacer;
            _row++;
        }

        /// <summary>
        /// returns the frequency given a row number and also
        /// returns the edit status of the frequency field
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <returns></returns>
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
                    if (_LastLen.Text.Length > 0 && _LastFreq.Text.Length > 0)
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
                var DataStatus = ((Tuple<int, string, global.fad3DataStatus>)o.Tag).Item3;
                if (s.Length > 0)
                {
                    switch (o.Name)
                    {
                        case "textLen":
                            //could be a double
                            Double Len;
                            msg = "Expected value is a number greater than zero";
                            if (double.TryParse(s, out Len) && Len > 0)
                            {
                                if (checkUseSize.Checked)
                                {
                                    if (Len < _IntervalSize || (Len % _IntervalSize) != 0)
                                    {
                                        msg = "Length must not be smaller and should be evenly divisible by the interval size";
                                        e.Cancel = true;
                                    }
                                }
                            }
                            else
                            {
                                e.Cancel = true;
                            }

                            if (!e.Cancel && _UniqueLenghtClasses && _LengthClasses.Contains(Len))
                            {
                                //if the datastatus is original (fromDB) then the value is okay
                                if (DataStatus != global.fad3DataStatus.statusFromDB)
                                {
                                    e.Cancel = true;
                                    msg = "Length class already in the table";
                                }
                            }

                            if (!e.Cancel)
                            {
                                //remove the previous value of the field if it is in the list of lengths
                                if (Len != _OldLength && _LengthClasses.Contains(_OldLength) && DataStatus != global.fad3DataStatus.statusNew)
                                {
                                    _LengthClasses.Remove(_OldLength);
                                }
                                _LengthClasses.Add(Len);
                                o.Text = Len.ToString("0.00");
                            }

                            if (e.Cancel)
                                CancelButton = null;
                            else
                                CancelButton = buttonCancel;

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
                foreach (KeyValuePair<string, sampling.LFLine> kv in _sampling.LFData(_CatchRowGuid))
                {
                    //if Sequence is -1, it means that the sequence field in the source
                    //database is null, so we set the _UpdateSequence flag to true.
                    //This will make all form fields DataStatus to Edited so that during
                    //Save, an update query will fill up the  null sequence field.
                    if (!_UpdateSequence) _UpdateSequence = kv.Value.Sequence == -1;

                    //adds a new row with fields containing length and frequency
                    var Len = kv.Value.Length;
                    AddRow(Len.ToString("0.00"), kv.Value.Freq.ToString(), kv.Value.LFRowGuid);

                    //Add to the list of unique length classes
                    if (!_LengthClasses.Contains(Len))
                    {
                        _LengthClasses.Add(Len);
                        if (_LengthClasses.Count == 1) _OldLength = Len;
                    }
                }
            }
        }

        /// <summary>
        /// collects all the data in the form fields and puts them in a Dictionary. The
        /// Dictionary is passed to the SaveEditedLF function.
        /// </summary>
        /// <returns></returns>
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

                                //we get the frequency value of the current length
                                var FreqPair = GetFreqValue(thisTag.Item1);
                                myLF.Freq = FreqPair.Item1;

                                //if Length field is unchanged, we get the edit status of the
                                //frequency field.
                                myLF.DataStatus = thisTag.Item3;
                                if (myLF.DataStatus == global.fad3DataStatus.statusFromDB)
                                    myLF.DataStatus = FreqPair.Item2;

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

        /// <summary>
        /// Validation rules for the Interval size field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OntextIntervalSize_Validating(object sender, CancelEventArgs e)
        {
            var msg = "";
            ((TextBox)sender).With(o =>
            {
                var s = o.Text;
                if (s.Length > 0)
                {
                    msg = "Expected value is a number greater than zero";
                    double interval = 0;
                    if (Double.TryParse(s, out interval))
                    {
                        if (interval <= 0)
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                            _IntervalSize = interval;
                            o.Text = _IntervalSize.ToString("0.00");
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            });

            if (e.Cancel)
            {
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnTextEntered(object sender, EventArgs e)
        {
            ((TextBox)sender).With(o =>
            {
                if (o.Name == "textLen" && o.Text.Length > 0)
                {
                    _OldLength = double.Parse(o.Text);
                }
            });
        }

        private void OnText_KeyDown(object sender, KeyEventArgs e)
        {
            ((TextBox)sender).With(o =>
            {
                if (o.Name == "textLen" && e.KeyCode == Keys.Escape &&
                    ((Tuple<int, string, global.fad3DataStatus>)o.Tag).Item3 != global.fad3DataStatus.statusNew)
                {
                    o.Text = _OldLength.ToString();
                }
            });
        }
    }
}