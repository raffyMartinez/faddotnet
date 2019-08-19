using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAD3.Database.Classes;
using ISO_Classes;

namespace FAD3.Mapping.Classes
{
    public class CoordinateDisplayFormatHelper
    {
        public Coordinate Coordinate
        {
            get { return _coordinate; }
            set { _coordinate = value; }
        }

        public CoordinateDisplayFormat CoordinateDisplayFormat
        {
            get { return _coordinateDisplayFormat; }
            set
            {
                _coordinateDisplayFormat = value;
                switch (_coordinateDisplayFormat)
                {
                    case CoordinateDisplayFormat.DegreeDecimal:
                        MaskLongitude = "000.0000° L";
                        MaskLatitude = "00.0000° L";
                        break;

                    case CoordinateDisplayFormat.DegreeMinute:
                        MaskLongitude = "000°00.00' L";
                        MaskLatitude = "00°00.00' L";
                        break;

                    case CoordinateDisplayFormat.DegreeMinuteSecond:
                        MaskLongitude = $"000°00'00.0{_secondSign} L";
                        MaskLatitude = $"00°00'00.0{_secondSign} L";
                        break;
                }
            }
        }

        private float _lonDeg;
        private float _latDeg;
        private float _lonMin;
        private float _latMin;
        private float _lonSec;
        private float _latSec;
        private bool _isNorth;
        private bool _isEast;
        private Coordinate _coordinate;
        private CoordinateDisplayFormat _coordinateDisplayFormat;
        private char _secondSign = '"';
        public string CoordinateDisplayMask { get; internal set; }
        public string MaskLongitude { get; internal set; }
        public string MaskLatitude { get; internal set; }

        public CoordinateDisplayFormatHelper()
        {
            CoordinateDisplayFormat = CoordinateDisplayFormat.DegreeDecimal;
        }
    }
}