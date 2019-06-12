using FAD3.Database.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FAD3
{
    public partial class LengthFreqForm : Form
    {
        private string _catchName = "";
        private string _catchRowGuid = "";
        private double _intervalSize = .5;
        private bool _isNew = false;
        private int _labelAdjust = 2;
        private TextBox _lastFreq;
        private TextBox _lastLen;
        private int _row;
        private Samplings _sampling;
        private int _spacer = 3;
        private int _y = 5;
        private List<double> _lengthClasses = new List<double>();
        private MainForm _parent_form;
        private Dictionary<string, (double len, int freq, fad3DataStatus dataStatus)> _lf = new Dictionary<string, (double len, int freq, fad3DataStatus dataStatus)>();
        private bool _hasDuplicateLength;
        private string _currentRow;
        private int _catchCount;

        public LengthFreqForm(bool isNew, Samplings sampling, string catchRowGuid, string catchName, int catchCount, MainForm parent)
        {
            InitializeComponent();
            _catchRowGuid = catchRowGuid;
            _catchName = catchName;
            _sampling = sampling;
            _isNew = isNew;
            _catchCount = catchCount;
            _parent_form = parent;
            labelTitle.Text = $"Length-frequency table for {catchName}";
            if (isNew)
            {
                labelTitle.Text = $"New length-frequency table for {catchName}";
            }
            else
            {
            }
            checkUniqueIntervals.Checked = true;
            textIntervalSize.Text = _intervalSize.ToString("0.00");
            lblCatchCompCount.Text = $"Catch comp. count: {_catchCount.ToString()}";
            lblTotalFreq.Text = "Total of frequency: 0";
            checkUseSize.Checked = true;
            PopulateFieldControls(isNew);
        }

        private void ComputeFreqTotal()
        {
            int totalFreq = 0;
            foreach (var item in _lf)
            {
                if (item.Value.dataStatus != fad3DataStatus.statusForDeletion)
                {
                    totalFreq += item.Value.freq;
                }
            }
            lblTotalFreq.Text = $"Total of frequency: {totalFreq}";
            lblTotalFreq.ForeColor = Color.Black;
            if (totalFreq > _catchCount)
            {
                lblTotalFreq.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Dynamically adds a row of controls
        /// </summary>
        private void AddRow(string rowGuid, string len = "", string freq = "")
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
                o.Text = (_row + 1).ToString();
            });

            panelUI.Controls.Add(textLen);
            textLen.With(o =>
            {
                o.Location = new Point(lblLen.Left, _y);
                if (double.TryParse(len, out double l))
                {
                    o.Text = l.ToString("0.00");
                }

                o.Tag = rowGuid;
            });

            panelUI.Controls.Add(textFreq);
            textFreq.With(o =>
            {
                o.Location = new Point(lblFreq.Left, _y);
                o.Text = freq;
                o.Tag = rowGuid;
            });

            _lastLen = textLen;
            _lastFreq = textFreq;

            textLen.Validating += OnTextValidating;
            textFreq.Validating += OnTextValidating;
            textLen.TextChanged += OnTextChanged;
            textFreq.TextChanged += OnTextChanged;
            textLen.Enter += OnTextEntered;
            textFreq.Enter += OnTextEntered;
            textLen.KeyDown += OnText_KeyDown;
            textFreq.KeyDown += OnText_KeyDown;

            _y += labelRow.Height + _spacer;
            _row++;
        }

        /// <summary>
        /// returns the frequency given a row number and also
        /// returns the edit status of the frequency field
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <returns></returns>
        private (int, fad3DataStatus) GetFreqValue(int rowNumber)
        {
            int rv = 0;
            fad3DataStatus ds = fad3DataStatus.statusFromDB;
            foreach (Control c in panelUI.Controls)
            {
                if (c.GetType().Name == "TextBox")
                {
                    var thisTag = (Tuple<int, string, fad3DataStatus>)c.Tag;
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

                    bool proceed = false;
                    if (ValidationPassed)
                    {
                        if (checkUniqueIntervals.Checked)
                        {
                            proceed = !_hasDuplicateLength;
                            if (!proceed)
                            {
                                MessageBox.Show("A lenght value is duplicated.\r\nRemove all duplicates", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            proceed = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cannot save until all fields are filled up",
                            "Validation error", MessageBoxButtons.OK,
                             MessageBoxIcon.Exclamation);
                    }

                    if (proceed)
                    {
                        if (SaveLFData())
                        {
                            Close();
                            _parent_form.RefreshLF_GMS();
                        }
                    }

                    break;

                case "buttonCancel":
                    Close();
                    break;

                case "buttonAdd":
                    if (_lastLen.Text.Length > 0 && _lastFreq.Text.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        _lf.Add(guid, (0, 0, fad3DataStatus.statusNew));
                        AddRow(guid);
                        _lastLen.Focus();
                    }
                    else
                        MessageBox.Show("You have to fill up all fields",
                                        "Validation error", MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                    break;

                case "buttonRemove":
                    panelUI.Controls.Clear();
                    var lfData = _lf[_currentRow];
                    lfData.dataStatus = fad3DataStatus.statusForDeletion;
                    _lf[_currentRow] = lfData;
                    _lengthClasses.Clear();
                    if (_lf.Count == 1 || FieldRowCount() == 1)
                    {
                        PopulateFieldControls(true);
                    }
                    else
                    {
                        PopulateFieldControls(false, true);
                    }
                    break;
            }
        }

        private int FieldRowCount()
        {
            var n = 0;
            foreach (var item in _lf)
            {
                if (item.Value.dataStatus != fad3DataStatus.statusForDeletion)
                {
                    n++;
                }
            }
            return n;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
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
                var row = o.Tag.ToString();
                var s = o.Text;
                var DataStatus = _lf[row].dataStatus;
                if (s.Length > 0)
                {
                    switch (o.Name)
                    {
                        case "textLen":
                            //could be a double
                            Double len;
                            msg = "Expected value is a number greater than zero";
                            if (double.TryParse(s, out len) && len > 0)
                            {
                                if (checkUseSize.Checked)
                                {
                                    if (len < _intervalSize || (len % _intervalSize) != 0)
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

                            if (!e.Cancel && checkUniqueIntervals.Checked && _hasDuplicateLength)
                            {
                                var n = 0;
                                List<string> lenList = new List<string>();
                                foreach (Control c in panelUI.Controls)
                                {
                                    if (c.Name == "textLen")
                                    {
                                        if (!lenList.Contains(c.Text))
                                        {
                                            lenList.Add(c.Text);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please remove duplicated length", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            break;
                                        }
                                        n++;
                                    }
                                }
                                if (n == _lf.Count)
                                {
                                    _hasDuplicateLength = false;
                                }
                            }

                            if (!e.Cancel && checkUniqueIntervals.Checked && _lengthClasses.Contains(len))
                            {
                                //if the datastatus is original (fromDB) then the value is okay
                                if (DataStatus != fad3DataStatus.statusFromDB && _lf[row].len != len)
                                {
                                    e.Cancel = true;
                                    msg = "Length class already in the table";
                                }
                            }
                            else
                            {
                                var lfdata = _lf[row];
                                if (lfdata.len != len)
                                {
                                    lfdata.len = len;
                                    if (lfdata.dataStatus == fad3DataStatus.statusNew)
                                    {
                                        lfdata.dataStatus = fad3DataStatus.statusNew;
                                    }
                                    else
                                    {
                                        lfdata.dataStatus = fad3DataStatus.statusEdited;
                                    }
                                    _lf[row] = lfdata;
                                }
                            }

                            if (!e.Cancel)
                            {
                                _lengthClasses.Add(len);
                                o.Text = len.ToString("0.00");
                            }

                            if (e.Cancel)
                                CancelButton = null;
                            else
                                CancelButton = buttonCancel;

                            break;

                        case "textFreq":
                            //must be an int
                            int freq;
                            msg = "Expected value is a whole number greater than zero";
                            if (int.TryParse(s, out freq))
                            {
                                //test if input is a whole number
                                if (freq <= 0 || int.Parse(s) != freq)
                                    e.Cancel = true;
                            }
                            else
                            {
                                e.Cancel = true;
                            }
                            if (!e.Cancel)
                            {
                                var lfdata = _lf[row];
                                if (lfdata.freq != freq)
                                {
                                    lfdata.freq = freq;
                                    if (lfdata.dataStatus == fad3DataStatus.statusNew)
                                    {
                                        lfdata.dataStatus = fad3DataStatus.statusNew;
                                    }
                                    else
                                    {
                                        lfdata.dataStatus = fad3DataStatus.statusEdited;
                                    }
                                    _lf[row] = lfdata;
                                    ComputeFreqTotal();
                                }
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
        private void PopulateFieldControls(bool IsNew, bool repopulate = false)
        {
            _row = 0;
            _y = 5;
            if (IsNew)
            {
                _lf.Clear();
                var guid = Guid.NewGuid().ToString();
                _lf.Add(guid, (0, 0, fad3DataStatus.statusNew));
                AddRow(guid);
            }
            else
            {
                if (repopulate)
                {
                    foreach (var item in _lf)
                    {
                        if (item.Value.dataStatus != fad3DataStatus.statusForDeletion)
                        {
                            AddRow(item.Key, item.Value.len.ToString(), item.Value.freq.ToString());
                            _lengthClasses.Add(item.Value.len);
                        }
                    }
                }
                else
                {
                    _lf.Clear();
                    foreach (var item in LengthFreq.LFData(_catchRowGuid))
                    {
                        var guid = item.Key;
                        _lf.Add(guid, (item.Value.len, item.Value.freq, fad3DataStatus.statusFromDB));
                        //adds a new row with fields containing length and frequency
                        AddRow(guid, _lf[guid].len.ToString("0.00"), _lf[guid].freq.ToString());
                        if (_lengthClasses.Contains(_lf[guid].len))
                        {
                            checkUniqueIntervals.Checked = false;
                        }
                        else
                        {
                            _lengthClasses.Add(_lf[guid].len);
                        }
                    }
                }
            }
            _lastLen.Focus();
            ComputeFreqTotal();
        }

        /// <summary>
        /// collects all the data in the form fields and puts them in a Dictionary. The
        /// Dictionary is passed to the SaveEditedLF function.
        /// </summary>
        /// <returns></returns>
        private bool SaveLFData()
        {
            return LengthFreq.SaveEditedLF(_lf, _catchRowGuid);
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
                            _intervalSize = interval;
                            o.Text = _intervalSize.ToString("0.00");
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
            _currentRow = ((TextBox)sender).Tag.ToString();
        }

        private int GetRow(string guid)
        {
            var n = 0;
            foreach (var item in _lf)
            {
                if (item.Key == guid)
                {
                    break;
                }
                else
                {
                    n++;
                }
            }
            return n;
        }

        private void GoToNextText(TextBox fromText)
        {
            int row = GetRow(fromText.Tag.ToString());
            var toTextName = "";
            switch (fromText.Name)
            {
                case "textLen":
                    toTextName = "textFreq";
                    break;

                case "textFreq":
                    toTextName = "textLen";
                    row++;
                    if (row > _row - 1)
                    {
                        row = 0;
                    }
                    break;
            }

            foreach (Control c in panelUI.Controls)
            {
                if (c.Name == toTextName && row == GetRow(c.Tag.ToString()))
                {
                    c.Focus();
                    break;
                }
            }
        }

        private void OnText_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            switch (txt.Name)
            {
                case "textLen":
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            e.Handled = e.SuppressKeyPress = true;
                            GoToNextText(txt);
                            break;

                        case Keys.Escape:
                            e.Handled = e.SuppressKeyPress = true;
                            break;
                    }
                    break;

                case "textFreq":
                    switch (e.KeyCode)
                    {
                        case Keys.Escape:
                            e.Handled = e.SuppressKeyPress = true;
                            break;

                        case Keys.Enter:
                            e.Handled = e.SuppressKeyPress = true;
                            if (e.Shift == true)
                            {
                                if (_lastFreq.Text.Length > 0 && _lastLen.Text.Length > 0)
                                {
                                    OnButtonClick(buttonAdd, null);
                                }
                            }
                            else
                            {
                                GoToNextText(txt);
                            }
                            break;
                    }
                    break;
            }
        }

        private void OnCheckChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            switch (chk.Name)
            {
                case "checkUniqueIntervals":
                    if (chk.Checked)
                    {
                        List<string> lenList = new List<string>();
                        foreach (Control c in panelUI.Controls)
                        {
                            if (c.Name == "textLen")
                            {
                                if (!lenList.Contains(c.Text))
                                {
                                    lenList.Add(c.Text);
                                }
                                else
                                {
                                    MessageBox.Show("A length value is duplicated\r\nPlease remove duplicate", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    _hasDuplicateLength = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case "checkUseSize":
                    break;
            }
        }
    }
}