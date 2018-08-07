using System;
using System.ComponentModel;
using System.Windows.Forms;
using ISO_Classes;

namespace FAD3
{
    public partial class CoordinateEntryForm : Form
    {
        private (double? x, double? y) _LandingSitePosition;
        private frmLandingSite _Parent_form;
        private string _XCoordinatePrompt;
        private string _YCoordinatePrompt;
        private Coordinate _Coordinate;
        private global.CoordinateDisplayFormat _CoordFormat;

        private float _LonDeg;
        private float _LatDeg;
        private float _LonMin;
        private float _LatMin;
        private float _LonSec;
        private float _LatSec;
        private bool _IsNorth;
        private bool _IsEast;

        private char _DegreeSign = '°';
        private char _SecondSign = '"';
        private char _NS;
        private char _EW;

        private bool _IsNew;

        public Coordinate Coordinate
        {
            get { return _Coordinate; }
            set { _Coordinate = value; }
        }

        public CoordinateEntryForm(bool IsNew, frmLandingSite Parent, (double? x, double? y) LandingSitePosition)
        {
            InitializeComponent();
            _LandingSitePosition = LandingSitePosition;
            _Parent_form = Parent;
            _IsNew = IsNew;

            _CoordFormat = global.CoordinateDisplay;
            switch (_CoordFormat)
            {
                case global.CoordinateDisplayFormat.DegreeDecimal:
                    mtextLongitude.Mask = "L 000.00000°";
                    mtextLatitude.Mask = "L 00.00000°";
                    _Coordinate.GetD(out _LatDeg, out _LonDeg);
                    break;

                case global.CoordinateDisplayFormat.DegreeMinute:
                    mtextLongitude.Mask = "L 000°00.000'";
                    mtextLatitude.Mask = "L 00°00.000'";
                    _Coordinate.GetDM(out _LatDeg, out _LatMin, out _IsNorth, out _LonDeg, out _LonMin, out _IsEast);
                    break;

                case global.CoordinateDisplayFormat.DegreeMinuteSecond:
                    mtextLongitude.Mask = $"L 000°00'00.00{_SecondSign}";
                    mtextLatitude.Mask = $"L 00°00'00.00{_SecondSign}";
                    _Coordinate.GetDMS(out _LatDeg, out _LatMin, out _LatSec, out _IsNorth, out _LonDeg, out _LonMin, out _LonSec, out _IsEast);
                    break;

                case global.CoordinateDisplayFormat.UTM:
                    mtextLongitude.Mask = "";
                    mtextLatitude.Mask = "";
                    break;
            }
            _XCoordinatePrompt = mtextLongitude.Text;
            _YCoordinatePrompt = mtextLatitude.Text;

            if (LandingSitePosition.x != null && LandingSitePosition.y != null)
            {
                _Coordinate.Latitude = (float)_LandingSitePosition.y;
                _Coordinate.Longitude = (float)_LandingSitePosition.x;

                _NS = _LatDeg < 0 ? 'S' : 'N';
                _EW = _LonDeg < 0 ? 'W' : 'E';

                switch (_CoordFormat)
                {
                    case global.CoordinateDisplayFormat.DegreeDecimal:
                        mtextLatitude.Text = $"{_NS} {_LatDeg}{_DegreeSign}";
                        mtextLongitude.Text = $"{_EW} {_LonDeg}{_DegreeSign}";
                        break;

                    case global.CoordinateDisplayFormat.DegreeMinute:
                        mtextLatitude.Text = $"{_NS} {_LatDeg}{_DegreeSign}.{_LatMin}'";
                        mtextLongitude.Text = $"{_EW} {_LonDeg}{_DegreeSign}.{_LonMin}'";
                        break;

                    case global.CoordinateDisplayFormat.DegreeMinuteSecond:
                        mtextLatitude.Text = $"{_NS} {_LatDeg}{_DegreeSign}.{_LatMin}'{_LatSec}{_SecondSign}";
                        mtextLongitude.Text = $"{_EW} {_LonDeg}{_DegreeSign}.{_LonMin}'{_LatSec}{_SecondSign}";
                        break;

                    case global.CoordinateDisplayFormat.UTM:
                        mtextLongitude.Mask = "";
                        mtextLatitude.Mask = "";
                        break;
                }
            }
        }

        public (double? x, double? y) LandingSitePosition
        {
            get { return _LandingSitePosition; }
        }

        private void Onbutton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (mtextLatitude.Text != _YCoordinatePrompt && mtextLongitude.Text != _XCoordinatePrompt)
                    {
                        _Parent_form.LandingSiteCoordinate(_LandingSitePosition);
                        Close();
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
                        CardinalDirection = parts[0][0];
                        if (Array.Exists(NEWS, element => element == CardinalDirection))
                        {
                            if (o.Name == "mtextLongitude")
                            {
                                e.Cancel = CardinalDirection != 'E' && CardinalDirection != 'W';
                                msg = "The only accepted letters are 'E' or 'W'";
                                if (!e.Cancel)
                                {
                                    //check the absolute value of the degree part
                                    e.Cancel = Math.Abs(float.Parse(parts[1])) > 180;
                                    IsLimit = float.Parse(parts[1]) == 180;
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
                                    e.Cancel = Math.Abs(float.Parse(parts[1])) > 90;
                                    IsLimit = float.Parse(parts[1]) == 90;
                                    msg = "Degree part cannot be more than 90";
                                    _IsEast = CardinalDirection == 'N';
                                }
                            }

                            if (!e.Cancel)
                            {
                                switch (_CoordFormat)
                                {
                                    case global.CoordinateDisplayFormat.DegreeDecimal:
                                        if (!e.Cancel && o.Name == "mtextLongitude")
                                        {
                                            _LonDeg = float.Parse(parts[1]);
                                        }
                                        else if (!e.Cancel)
                                        {
                                            _LatDeg = float.Parse(parts[1]);
                                        }
                                        break;

                                    case global.CoordinateDisplayFormat.DegreeMinute:
                                        e.Cancel = IsLimit ? float.Parse(parts[2]) > 0 : float.Parse(parts[2]) >= 60;
                                        msg = IsLimit ? "Minute part shoud be zero" : "Minute part should be less than 60";
                                        if (!e.Cancel && o.Name == "mtextLongitude")
                                        {
                                            _LonDeg = float.Parse(parts[1]);
                                            _LonMin = float.Parse(parts[2]);
                                        }
                                        else if (!e.Cancel)
                                        {
                                            _LatDeg = float.Parse(parts[1]);
                                            _LatMin = float.Parse(parts[2]);
                                        }
                                        break;

                                    case global.CoordinateDisplayFormat.DegreeMinuteSecond:
                                        e.Cancel = IsLimit ? float.Parse(parts[3]) > 0 : float.Parse(parts[3]) >= 60;
                                        msg = IsLimit ? "Second part should be zero" : "Second part should be less than 60";
                                        if (!e.Cancel && o.Name == "mtextLongitude")
                                        {
                                            _LonDeg = float.Parse(parts[1]);
                                            _LonMin = float.Parse(parts[2]);
                                            _LonSec = float.Parse(parts[3]);
                                        }
                                        else if (!e.Cancel)
                                        {
                                            _LatDeg = float.Parse(parts[1]);
                                            _LatMin = float.Parse(parts[2]);
                                            _LatSec = float.Parse(parts[3]);
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

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (mtextLatitude.Text != _YCoordinatePrompt && mtextLongitude.Text != _XCoordinatePrompt)
            {
                _Coordinate.SetDMS(_LatDeg, _LatMin, _LatSec, _IsNorth, _LonDeg, _LonMin, _LonSec, _IsEast);
            }
            Close();
        }
    }
}