﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using ISO_Classes;

namespace FAD3
{
    public partial class CoordinateEntryForm : Form
    {
        private LandingSiteForm _Parent_form;
        private string _XCoordinatePrompt;
        private string _YCoordinatePrompt;
        private Coordinate _Coordinate;
        private global.CoordinateDisplayFormat _CoordinateFormat;

        private float _LonDeg;
        private float _LatDeg;
        private float _LonMin;
        private float _LatMin;
        private float _LonSec;
        private float _LatSec;
        private bool _IsNorth;
        private bool _IsEast;

        private char _SecondSign = '"';
        private bool _IsNew;

        public Coordinate Coordinate
        {
            get { return _Coordinate; }
            set { _Coordinate = value; }
        }

        public CoordinateEntryForm(bool IsNew, LandingSiteForm Parent, Coordinate coordinate)
        {
            InitializeComponent();
            _Parent_form = Parent;
            _IsNew = IsNew;
            _Coordinate = coordinate;

            var format = "D";
            _CoordinateFormat = global.CoordinateDisplay;

            if (!_IsNew)
            {
                _Coordinate.GetD(out _LatDeg, out _LonDeg);
            }

            switch (_CoordinateFormat)
            {
                case global.CoordinateDisplayFormat.DegreeDecimal:
                    mtextLongitude.Mask = $"000.0000° L";
                    mtextLatitude.Mask = $"00.0000° L";

                    break;

                case global.CoordinateDisplayFormat.DegreeMinute:
                    mtextLongitude.Mask = $"000°00.00' L";
                    mtextLatitude.Mask = $"00°00.00' L";
                    if (!_IsNew) _Coordinate.GetDM(out _LatDeg, out _LatMin, out _IsNorth, out _LonDeg, out _LonMin, out _IsEast);
                    format = "DM";
                    break;

                case global.CoordinateDisplayFormat.DegreeMinuteSecond:
                    mtextLongitude.Mask = $"000°00'00.0{_SecondSign} L";
                    mtextLatitude.Mask = $"00°00'00.0{_SecondSign} L";
                    if (!_IsNew) _Coordinate.GetDMS(out _LatDeg, out _LatMin, out _LatSec, out _IsNorth, out _LonDeg, out _LonMin, out _LonSec, out _IsEast);
                    format = "DMS";
                    break;

                case global.CoordinateDisplayFormat.UTM:
                    mtextLongitude.Mask = "";
                    mtextLatitude.Mask = "";
                    break;
            }
            _XCoordinatePrompt = mtextLongitude.Text;
            _YCoordinatePrompt = mtextLatitude.Text;

            if (!_IsNew)
            {
                var CoordString = _Coordinate.ToString(format).Split(' ');
                mtextLongitude.Text = CoordString[1];
                mtextLatitude.Text = CoordString[0];
            }
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (mtextLatitude.Text != _YCoordinatePrompt && mtextLongitude.Text != _XCoordinatePrompt)
                    {
                        _Coordinate.SetDMS(_LatDeg, _LatMin, _LatSec, _IsNorth, _LonDeg, _LonMin, _LonSec, _IsEast);
                        _Parent_form.RefreshCoordinate();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Please fill up both coordinates", "Validation error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        mtextLatitude.Focus();
                    }
                    break;

                case "buttonCancel":
                    Close();
                    break;
            }
        }

        private void onCoordinate_Validating(object sender, CancelEventArgs e)
        {
            var CardinalDirection = 'N';
            var msg = "";
            var IsLimit = false;
            ((MaskedTextBox)sender).With(o =>
            {
                if (o.Text != _XCoordinatePrompt && o.Text != _YCoordinatePrompt)
                {
                    if (o.MaskCompleted)
                    {
                        char[] NEWS = { 'N', 'E', 'W', 'S' };
                        string[] parts = o.Text.ToUpper().Split(new char[] { ' ', '\'', '\"', '°', });
                        var DirectionPart = 3;
                        switch (_CoordinateFormat)
                        {
                            case global.CoordinateDisplayFormat.DegreeDecimal:
                                DirectionPart = 2;
                                break;

                            case global.CoordinateDisplayFormat.DegreeMinute:
                                DirectionPart = 3;
                                break;

                            case global.CoordinateDisplayFormat.DegreeMinuteSecond:
                                DirectionPart = 4;
                                break;

                            case global.CoordinateDisplayFormat.UTM:
                                break;
                        }

                        CardinalDirection = parts[DirectionPart][0];

                        if (Array.Exists(NEWS, element => element == CardinalDirection))
                        {
                            if (o.Name == "mtextLongitude")
                            {
                                e.Cancel = CardinalDirection != 'E' && CardinalDirection != 'W';
                                msg = "The only accepted letters are 'E' or 'W'";
                                if (!e.Cancel)
                                {
                                    //check the absolute value of the degree part
                                    e.Cancel = Math.Abs(float.Parse(parts[0])) > 180;
                                    IsLimit = float.Parse(parts[0]) == 180;
                                    msg = "Degree part cannot be more than 180";
                                    _IsEast = CardinalDirection == 'E';
                                }
                            }
                            else
                            {
                                e.Cancel = CardinalDirection != 'N' && CardinalDirection != 'S';
                                msg = "The only accepted letters are 'N' or 'S'";
                                if (!e.Cancel)
                                {
                                    //check the absolute value of the degree part
                                    e.Cancel = Math.Abs(float.Parse(parts[0])) > 90;
                                    IsLimit = float.Parse(parts[0]) == 90;
                                    msg = "Degree part cannot be more than 90";
                                    _IsNorth = CardinalDirection == 'N';
                                }
                            }

                            if (!e.Cancel)
                            {
                                switch (_CoordinateFormat)
                                {
                                    case global.CoordinateDisplayFormat.DegreeDecimal:
                                        if (!e.Cancel && o.Name == "mtextLongitude")
                                        {
                                            _LonDeg = float.Parse(parts[0]);
                                        }
                                        else if (!e.Cancel)
                                        {
                                            _LatDeg = float.Parse(parts[0]);
                                        }
                                        break;

                                    case global.CoordinateDisplayFormat.DegreeMinute:
                                        e.Cancel = IsLimit ? float.Parse(parts[1]) > 0 : float.Parse(parts[1]) >= 60;
                                        msg = IsLimit ? "Minute part shoud be zero" : "Minute part should be less than 60";
                                        if (!e.Cancel && o.Name == "mtextLongitude")
                                        {
                                            _LonDeg = float.Parse(parts[0]);
                                            _LonMin = float.Parse(parts[1]);
                                        }
                                        else if (!e.Cancel)
                                        {
                                            _LatDeg = float.Parse(parts[0]);
                                            _LatMin = float.Parse(parts[1]);
                                        }
                                        break;

                                    case global.CoordinateDisplayFormat.DegreeMinuteSecond:
                                        e.Cancel = IsLimit ? float.Parse(parts[2]) > 0 : float.Parse(parts[2]) >= 60;
                                        msg = IsLimit ? "Second part should be zero" : "Second part should be less than 60";
                                        if (!e.Cancel && o.Name == "mtextLongitude")
                                        {
                                            _LonDeg = float.Parse(parts[0]);
                                            _LonMin = float.Parse(parts[1]);
                                            _LonSec = float.Parse(parts[2]);
                                        }
                                        else if (!e.Cancel)
                                        {
                                            _LatDeg = float.Parse(parts[0]);
                                            _LatMin = float.Parse(parts[1]);
                                            _LatSec = float.Parse(parts[2]);
                                        }
                                        break;

                                    case global.CoordinateDisplayFormat.UTM:

                                        break;
                                }

                                if (!e.Cancel)
                                {
                                    o.Text = o.Text.ToUpper();
                                }
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                            msg = "The only letters accepted are N, S, E, and W";
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                        msg = "Coordinate is not complete";
                    }
                }
            });

            if (e.Cancel)
            {
                MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void CoordinateEntryForm_Shown(object sender, EventArgs e)
        {
            mtextLatitude.Focus();
        }

        private void Onmtext_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                switch (((MaskedTextBox)sender).Name)
                {
                    case "mtextLongitude":
                        if (mtextLongitude.MaskCompleted)
                            buttonOK.Focus();
                        break;

                    case "mtextLatitude":
                        if (mtextLatitude.MaskCompleted)
                            mtextLongitude.Focus();
                        break;
                }
                e.Handled = e.SuppressKeyPress = true;
            }
        }
    }
}