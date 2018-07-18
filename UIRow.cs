using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FAD3
{
    public class UIRowFromXML : EventArgs
    {
        string _rowLabel;
        string _key;
        string _buttonText;
        int _height;
        string _dataType;
        sampling.UserInterfaceStructure.UIControlType _controlType;
        bool _readOnly;
        string _ToolTip;
        bool _Required;

        public UIRowFromXML()
        {
            ;
        }

        public UIRowFromXML(string rowLabel, string key, string buttonText,
                               sampling.UserInterfaceStructure.UIControlType control,
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
            set { _key = value;  }
        }

        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; }
        }

        public sampling.UserInterfaceStructure.UIControlType Control
        {
            get { return _controlType; }
            set { _controlType = value; }
        }
    }
}
