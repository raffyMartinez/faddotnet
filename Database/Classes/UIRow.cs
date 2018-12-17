using FAD3.Database.Classes;
using System;

namespace FAD3
{
    public class UIRowFromXML : EventArgs
    {
        private string _rowLabel;
        private string _key;
        private string _buttonText;
        private int _height;
        private string _dataType;
        private UIControlType _controlType;
        private bool _readOnly;
        private string _ToolTip;
        private bool _Required;

        public UIRowFromXML()
        {
            ;
        }

        public UIRowFromXML(string rowLabel, string key, string buttonText,
                               UIControlType control,
                               int Height, string DataType, bool ReadOnly,
                               string ToolTip, bool Required)
        {
            _rowLabel = rowLabel;
            _key = key;
            _buttonText = buttonText;
            _controlType = control;
            _height = Height;
            _dataType = DataType;
            _readOnly = ReadOnly;
            _ToolTip = ToolTip;
            _Required = Required;
        }

        public bool Required
        {
            get { return _Required; }
            set { _Required = value; }
        }

        public string ToolTip
        {
            get { return _ToolTip; }
            set { _ToolTip = value; }
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public string RowLabel
        {
            get { return _rowLabel; }
            set { _rowLabel = value; }
        }

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; }
        }

        public UIControlType Control
        {
            get { return _controlType; }
            set { _controlType = value; }
        }
    }
}