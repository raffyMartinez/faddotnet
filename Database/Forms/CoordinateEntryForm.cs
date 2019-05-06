using FAD3.Database.Classes;
using ISO_Classes;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using AxMapWinGIS;
using MapWinGIS;
using FAD3.Mapping.Classes;

namespace FAD3
{
    public partial class CoordinateEntryForm : Form
    {
        private LandingSiteForm _parentForm;
        private string _xCoordinatePrompt;
        private string _yCoordinatePrompt;
        private Coordinate _coordinate;
        private CoordinateDisplayFormat _coordinateFormat;
        private MapLayersHandler _mapLayersHandler;
        private string _treeLevel;
        private int _lguNumber;

        private string _locationName;

        private float _lonDeg;
        private float _latDeg;
        private float _lonMin;
        private float _latMin;
        private float _lonSec;
        private float _latSec;
        private bool _isNorth;
        private bool _isEast;

        private string _format;

        private AxMap _mapControl;

        public AxMap MapControl
        {
            get { return _mapControl; }
            set
            {
                _mapControl = value;
                _mapControl.SendMouseUp = true;
                _mapControl.MouseUpEvent += OnMapMouseUp;
            }
        }

        public event EventHandler CoordinateAvailable;
        public event EventHandler CoordinateFormClosed;

        public string TreeLevel
        {
            get { return _treeLevel; }
            set
            {
                _treeLevel = value;
                mtextLongitude.Enabled = _treeLevel == "barangay" || _treeLevel == "municipality";
                mtextLatitude.Enabled = mtextLongitude.Enabled;
                if (!mtextLatitude.Enabled)
                {
                    mtextLatitude.Clear();
                    mtextLongitude.Clear();
                }
            }
        }

        public void SetLocation(string lguName, int lguNumber, string barangayName = "")
        {
            mtextLatitude.Clear();
            mtextLongitude.Clear();
            switch (_treeLevel)
            {
                case "barangay":
                    _locationName = $"{barangayName}, {lguName}";
                    Text = $"Set coordinate for {barangayName}, {lguName}";
                    break;

                case "municipality":
                    _locationName = $"{lguName}";
                    _lguNumber = lguNumber;
                    break;
            }
            Text = $"Set coordinate for {_locationName}";
            var result = BarangayMunicipalityCoordinateHelper.GetCoordinate(_treeLevel, lguNumber, barangayName);
            if (result.success)
            {
                Coordinate = result.c;
                var CoordString = result.c.ToString(_format).Split(' ');
                mtextLongitude.Text = CoordString[1];
                mtextLatitude.Text = CoordString[0];
            }
        }

        private void OnMapMouseUp(object sender, _DMapEvents_MouseUpEvent e)
        {
            mtextLatitude.Clear();
            mtextLongitude.Clear();
            _coordinate = null;

            if (_treeLevel == "municipality" || TreeLevel == "barangay")
            {
                double projX = 0;
                double projY = 0;
                MapControl.PixelToProj(e.x, e.y, ref projX, ref projY);
                switch (_treeLevel)
                {
                    case "barangay":
                        _coordinate = new Coordinate((float)projY, (float)projX);
                        break;

                    case "municipality":
                        if (_mapLayersHandler.CurrentMapLayer.LayerType == "ShapefileClass"
                            && ((Shapefile)_mapLayersHandler.CurrentMapLayer.LayerObject).ShapefileType == ShpfileType.SHP_POINT
                            && ((Shapefile)_mapLayersHandler.CurrentMapLayer.LayerObject).NumSelected == 1)
                        {
                            var sf = _mapLayersHandler.CurrentMapLayer.LayerObject as Shapefile;
                            var pt = sf.Shape[_mapLayersHandler.CurrentMapLayer.SelectedIndexes[0]].Point[0];
                            _coordinate = new Coordinate((float)pt.y, (float)pt.x);
                        }
                        break;
                }

                if (_coordinate != null)
                {
                    var CoordString = _coordinate.ToString(_format).Split(' ');
                    mtextLongitude.Text = CoordString[1];
                    mtextLatitude.Text = CoordString[0];
                }
            }
        }

        private char _SecondSign = '"';
        private bool _isNew;

        public Coordinate Coordinate
        {
            get { return _coordinate; }
            set { _coordinate = value; }
        }

        private void SetUpCoordinateFields()
        {
            if (!_isNew)
            {
                _coordinate.GetD(out _latDeg, out _lonDeg);
            }

            switch (_coordinateFormat)
            {
                case CoordinateDisplayFormat.DegreeDecimal:
                    mtextLongitude.Mask = $"000.0000° L";
                    mtextLatitude.Mask = $"00.0000° L";

                    break;

                case CoordinateDisplayFormat.DegreeMinute:
                    mtextLongitude.Mask = $"000°00.00' L";
                    mtextLatitude.Mask = $"00°00.00' L";
                    if (!_isNew) _coordinate.GetDM(out _latDeg, out _latMin, out _isNorth, out _lonDeg, out _lonMin, out _isEast);
                    _format = "DM";
                    break;

                case CoordinateDisplayFormat.DegreeMinuteSecond:
                    mtextLongitude.Mask = $"000°00'00.0{_SecondSign} L";
                    mtextLatitude.Mask = $"00°00'00.0{_SecondSign} L";
                    if (!_isNew) _coordinate.GetDMS(out _latDeg, out _latMin, out _latSec, out _isNorth, out _lonDeg, out _lonMin, out _lonSec, out _isEast);
                    _format = "DMS";
                    break;

                case CoordinateDisplayFormat.UTM:
                    mtextLongitude.Mask = "";
                    mtextLatitude.Mask = "";
                    break;
            }
            _xCoordinatePrompt = mtextLongitude.Text;
            _yCoordinatePrompt = mtextLatitude.Text;

            if (!_isNew)
            {
                var CoordString = _coordinate.ToString(_format).Split(' ');
                mtextLongitude.Text = CoordString[1];
                mtextLatitude.Text = CoordString[0];
            }
        }

        public CoordinateEntryForm(AxMap mapControl, MapLayersHandler mapLayersHandler)
        {
            InitializeComponent();
            MapControl = mapControl;
            _mapLayersHandler = mapLayersHandler;
            _isNew = true;
            _format = "D";
            _coordinateFormat = global.CoordinateDisplay;
            SetUpCoordinateFields();
        }

        public CoordinateEntryForm(bool isNew, LandingSiteForm parent, Coordinate coordinate)
        {
            InitializeComponent();
            _parentForm = parent;
            _isNew = isNew;
            _coordinate = coordinate;
            _format = "D";
            _coordinateFormat = global.CoordinateDisplay;
            TreeLevel = "";
            mtextLongitude.Enabled = true;
            mtextLatitude.Enabled = true;
            SetUpCoordinateFields();
        }

        private void OnButton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "buttonOK":
                    if (mtextLatitude.Text != _yCoordinatePrompt && mtextLongitude.Text != _xCoordinatePrompt)
                    {
                        if (TreeLevel.Length == 0)
                        {
                            _coordinate.SetDMS(_latDeg, _latMin, _latSec, _isNorth, _lonDeg, _lonMin, _lonSec, _isEast);
                            Close();
                        }
                        else
                        {
                            CoordinateAvailable?.Invoke(this, EventArgs.Empty);
                            mtextLongitude.Clear();
                            mtextLatitude.Clear();
                        }
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

        private void OnCoordinate_Validating(object sender, CancelEventArgs e)
        {
            var cardinalDirection = 'N';
            var msg = "";
            var isLimit = false;
            ((MaskedTextBox)sender).With(o =>
            {
                if (o.Text != _xCoordinatePrompt && o.Text != _yCoordinatePrompt)
                {
                    if (o.MaskCompleted)
                    {
                        char[] NEWS = { 'N', 'E', 'W', 'S' };
                        string[] parts = o.Text.ToUpper().Split(new char[] { ' ', '\'', '\"', '°', });

                        //indicates what part of the parts[] array is the NEWS direction indicator located
                        var directionPart = 3;
                        switch (_coordinateFormat)
                        {
                            case CoordinateDisplayFormat.DegreeDecimal:
                                directionPart = 2;
                                break;

                            case CoordinateDisplayFormat.DegreeMinute:
                                directionPart = 3;
                                break;

                            case CoordinateDisplayFormat.DegreeMinuteSecond:
                                directionPart = 4;
                                break;

                            case CoordinateDisplayFormat.UTM:
                                break;
                        }

                        cardinalDirection = parts[directionPart][0];

                        if (Array.Exists(NEWS, element => element == cardinalDirection))
                        {
                            if (o.Name == "mtextLongitude")
                            {
                                e.Cancel = cardinalDirection != 'E' && cardinalDirection != 'W';
                                msg = "The only accepted letters are 'E' or 'W'";
                                if (!e.Cancel)
                                {
                                    //check the absolute value of the degree part
                                    e.Cancel = Math.Abs(float.Parse(parts[0])) > 180;
                                    isLimit = float.Parse(parts[0]) == 180;
                                    msg = "Degree part cannot be more than 180";
                                    _isEast = cardinalDirection == 'E';
                                }
                            }
                            else
                            {
                                e.Cancel = cardinalDirection != 'N' && cardinalDirection != 'S';
                                msg = "The only accepted letters are 'N' or 'S'";
                                if (!e.Cancel)
                                {
                                    //check the absolute value of the degree part
                                    e.Cancel = Math.Abs(float.Parse(parts[0])) > 90;
                                    isLimit = float.Parse(parts[0]) == 90;
                                    msg = "Degree part cannot be more than 90";
                                    _isNorth = cardinalDirection == 'N';
                                }
                            }

                            if (!e.Cancel)
                            {
                                switch (_coordinateFormat)
                                {
                                    case CoordinateDisplayFormat.DegreeDecimal:
                                        if (!e.Cancel && o.Name == "mtextLongitude")
                                        {
                                            _lonDeg = float.Parse(parts[0]);
                                        }
                                        else if (!e.Cancel)
                                        {
                                            _latDeg = float.Parse(parts[0]);
                                        }
                                        break;

                                    case CoordinateDisplayFormat.DegreeMinute:
                                        e.Cancel = isLimit ? float.Parse(parts[1]) > 0 : float.Parse(parts[1]) >= 60;
                                        msg = isLimit ? "Minute part shoud be zero" : "Minute part should be less than 60";
                                        if (!e.Cancel && o.Name == "mtextLongitude")
                                        {
                                            _lonDeg = float.Parse(parts[0]);
                                            _lonMin = float.Parse(parts[1]);
                                        }
                                        else if (!e.Cancel)
                                        {
                                            _latDeg = float.Parse(parts[0]);
                                            _latMin = float.Parse(parts[1]);
                                        }
                                        break;

                                    case CoordinateDisplayFormat.DegreeMinuteSecond:
                                        e.Cancel = isLimit ? float.Parse(parts[2]) > 0 : float.Parse(parts[2]) >= 60;
                                        msg = isLimit ? "Second part should be zero" : "Second part should be less than 60";
                                        if (!e.Cancel && o.Name == "mtextLongitude")
                                        {
                                            _lonDeg = float.Parse(parts[0]);
                                            _lonMin = float.Parse(parts[1]);
                                            _lonSec = float.Parse(parts[2]);
                                        }
                                        else if (!e.Cancel)
                                        {
                                            _latDeg = float.Parse(parts[0]);
                                            _latMin = float.Parse(parts[1]);
                                            _latSec = float.Parse(parts[2]);
                                        }
                                        break;

                                    case CoordinateDisplayFormat.UTM:

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

        private void OnCoordinateEntryForm_Load(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
            if (_mapControl != null)
            {
                _mapControl.MouseUpEvent -= OnMapMouseUp;
            }
            _mapControl = null;
            CoordinateFormClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}