using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace FAD3.Mapping.Classes
{
    /// <summary>
    /// Reads metadata in an ISO 19115 xml file.
    /// </summary>
    public static class ERDDAPMetadataHandler
    {
        public static EventHandler<ERDDAPMetadataReadEventArgs> OnMetadataRead;
        private static Dictionary<string, (string unit, string description)> _parameters = new Dictionary<string, (string unit, string description)>();
        public static string MetadataDirectory { get; set; }
        private static string _filenameForUpdate = "";
        private static string _endPositionForUpdate = "";

        public static string GetMetadataDirectorySetting()
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            return reg_key.GetValue("ERDDAPMetadataFolder", "").ToString();
        }

        /// <summary>
        /// reads iso9115 metadata from a folder and raises an event that is used for posting metadata in a form
        ///
        /// </summary>
        /// <returns></returns>
        public static int ReadISO9115Metadata()
        {
            _parameters.Clear();
            string currentDimension = "";
            string currentStringParameter = "";
            string identifier = "";
            int columnSize = 0;
            int rowSize = 0;
            int verticalSize = 0;
            int temporalSize = 0;
            string columnUOM = "";
            string rowUOM = "";
            string verticalUOM = "";
            string temporalUOM = "";
            double columnResolution = 0D;
            double rowResolution = 0D;
            double verticalResolution = 0D;
            double temporalResolution = 0D;
            bool inIdentification = false;
            string dataTitle = "";
            string dataAbstract = "";
            string url = "";
            string currentBoundParameter = "";
            double westBound = 0;
            double eastBound = 0;
            double northBound = 0;
            double southBound = 0;
            bool inTemporalExtent = false;
            bool inVerticalExtent = false;
            string temporalUnit = "";
            DateTime beginPosition = DateTime.Now;
            DateTime endPosition = DateTime.Now;
            string currentMinMaxValue = "";
            double verticalExtentMax = 0D;
            double verticalExtentMin = 0D;
            bool inERDDAP = false;
            bool inOperationMetadata = false;
            bool inParameterDimension = false;
            string parameterName = "";
            string parameterType = "";
            string credit = "";
            string legalConstraints = "";
            string parameterDescription = "";
            bool inGridDimension = false;
            bool proceedRead = true;

            //get ISO 19115 metadata xml files  inside a folder
            string[] files = Directory.GetFiles(MetadataDirectory, "*_iso19115.xml");

            if (files.Length > 0)
            {
                //read all the metadata files
                for (int n = 0; n < files.Length; n++)
                {
                    if (_filenameForUpdate.Length > 0)
                    {
                        if (files[n] != _filenameForUpdate)
                        {
                            proceedRead = false;
                        }
                        else
                        {
                            _filenameForUpdate = "";
                            proceedRead = true;
                        }
                    }
                    else
                    {
                        proceedRead = true;
                    }
                    if (proceedRead)
                    {
                        XmlTextReader xmlReader = new XmlTextReader(files[n]);
                        while (xmlReader.Read())
                        {
                            if (xmlReader.NodeType == XmlNodeType.Element)
                            {
                                switch (xmlReader.Name)
                                {
                                    case "gmd:fileIdentifier":
                                        currentStringParameter = "identifier";
                                        break;

                                    case "gco:CharacterString":
                                        switch (currentStringParameter)
                                        {
                                            case "identifier":
                                                identifier = xmlReader.ReadElementContentAsString();
                                                break;

                                            case "citation":
                                                dataTitle = xmlReader.ReadElementContentAsString();
                                                break;

                                            case "abstract":
                                                dataAbstract = xmlReader.ReadElementContentAsString();
                                                break;

                                            case "parameterName":
                                                parameterName = xmlReader.ReadElementContentAsString();
                                                break;

                                            case "parameterAttribute":
                                                parameterType = xmlReader.ReadElementContentAsString();

                                                break;

                                            case "parameterDescription":
                                                parameterDescription = xmlReader.ReadElementContentAsString();
                                                _parameters.Add(parameterName, (parameterType, parameterDescription));
                                                break;

                                            case "credit":
                                                credit = xmlReader.ReadElementContentAsString();
                                                break;

                                            case "legalConstraints":
                                                legalConstraints = xmlReader.ReadElementContentAsString();
                                                break;
                                        }
                                        currentStringParameter = "";
                                        break;

                                    case "gco:Decimal":
                                        switch (currentBoundParameter)
                                        {
                                            case "west":
                                                westBound = xmlReader.ReadElementContentAsDouble();
                                                break;

                                            case "east":
                                                eastBound = xmlReader.ReadElementContentAsDouble();
                                                break;

                                            case "north":
                                                northBound = xmlReader.ReadElementContentAsDouble();
                                                break;

                                            case "south":
                                                southBound = xmlReader.ReadElementContentAsDouble();
                                                break;
                                        }
                                        currentBoundParameter = "";
                                        break;

                                    case "gmd:spatialRepresentationInfo":
                                        inGridDimension = true;
                                        break;

                                    case "gmd:cellGeometry":
                                        inGridDimension = false;
                                        break;

                                    case "gmd:MD_DimensionNameTypeCode":

                                        if (inGridDimension && xmlReader.Name == "gmd:MD_DimensionNameTypeCode")
                                        {
                                            currentDimension = xmlReader.GetAttribute("codeListValue");
                                        }
                                        break;

                                    case "gco:Integer":

                                        if (inGridDimension && xmlReader.Name == "gco:Integer")
                                        {
                                            switch (currentDimension)
                                            {
                                                case "column":
                                                    columnSize = xmlReader.ReadElementContentAsInt();
                                                    break;

                                                case "row":
                                                    rowSize = xmlReader.ReadElementContentAsInt();
                                                    break;

                                                case "vertical":
                                                    verticalSize = xmlReader.ReadElementContentAsInt();
                                                    break;

                                                case "temporal":
                                                    temporalSize = xmlReader.ReadElementContentAsInt();
                                                    break;
                                            }
                                        }
                                        break;

                                    case "gco:Real":
                                        switch (currentMinMaxValue)
                                        {
                                            case "verticalExtentValueMinimum":
                                                verticalExtentMin = xmlReader.ReadElementContentAsDouble();
                                                break;

                                            case "verticalExtentValueMaximum":
                                                verticalExtentMax = xmlReader.ReadElementContentAsDouble();
                                                break;
                                        }
                                        break;

                                    case "gmd:resolution":
                                    case "gco:Measure":
                                        if (xmlReader.Name == "gco:Measure")
                                        {
                                            switch (currentDimension)
                                            {
                                                case "column":
                                                    columnUOM = xmlReader.GetAttribute("uom");
                                                    columnResolution = xmlReader.ReadElementContentAsDouble();
                                                    break;

                                                case "row":
                                                    rowUOM = xmlReader.GetAttribute("uom");
                                                    rowResolution = xmlReader.ReadElementContentAsDouble();
                                                    break;

                                                case "vertical":
                                                    verticalUOM = xmlReader.GetAttribute("uom");
                                                    verticalResolution = xmlReader.ReadElementContentAsDouble();
                                                    break;

                                                case "temporal":
                                                    temporalUOM = xmlReader.GetAttribute("uom");
                                                    temporalResolution = xmlReader.ReadElementContentAsDouble();
                                                    break;
                                            }
                                        }
                                        break;

                                    case "gmd:identificationInfo":
                                    case "gmd:MD_DataIdentification":

                                        if (xmlReader.Name == "gmd:MD_DataIdentification")
                                        {
                                            inIdentification = true;
                                        }

                                        break;

                                    case "gmd:citation":
                                        if (inIdentification)
                                        {
                                            currentStringParameter = "citation";
                                        }
                                        break;

                                    case "gmd:abstract":
                                        if (inIdentification)
                                        {
                                            currentStringParameter = "abstract";
                                        }
                                        break;

                                    case "gmd:credit":
                                        if (inIdentification)
                                        {
                                            currentStringParameter = "credit";
                                        }
                                        break;

                                    case "gmd:MD_LegalConstraints":
                                        if (inIdentification)
                                        {
                                            currentStringParameter = "legalConstraints";
                                        }
                                        break;

                                    case "gmd:westBoundLongitude":
                                    case "gmd:eastBoundLongitude":
                                    case "gmd:northBoundLatitude":
                                    case "gmd:southBoundLatitude":
                                        if (inIdentification)
                                        {
                                            switch (xmlReader.Name)
                                            {
                                                case "gmd:westBoundLongitude":
                                                    currentBoundParameter = "west";
                                                    break;

                                                case "gmd:eastBoundLongitude":
                                                    currentBoundParameter = "east";
                                                    break;

                                                case "gmd:northBoundLatitude":
                                                    currentBoundParameter = "north";
                                                    break;

                                                case "gmd:southBoundLatitude":
                                                    currentBoundParameter = "south";
                                                    break;
                                            }
                                        }
                                        break;

                                    case "gmd:temporalElement":
                                        if (inIdentification)
                                        {
                                            inTemporalExtent = true;
                                        }
                                        break;

                                    case "gml:description":
                                        if (inTemporalExtent)
                                        {
                                            temporalUnit = xmlReader.ReadElementContentAsString();
                                        }
                                        break;

                                    case "gml:beginPosition":
                                        if (inTemporalExtent)
                                        {
                                            beginPosition = xmlReader.ReadElementContentAsDateTime();
                                        }
                                        break;

                                    case "gml:endPosition":
                                        if (inTemporalExtent)
                                        {
                                            endPosition = xmlReader.ReadElementContentAsDateTime();
                                        }
                                        break;

                                    case "gmd:VerticalElement":
                                        if (inIdentification)
                                        {
                                            inVerticalExtent = true;
                                        }
                                        break;

                                    case "gmd:minimumValue":
                                        if (inVerticalExtent)
                                        {
                                            currentMinMaxValue = "verticalExtentValueMinimum";
                                        }
                                        break;

                                    case "gmd:maximumValue":
                                        if (inVerticalExtent)
                                        {
                                            currentMinMaxValue = "verticalExtentValueMaximum";
                                        }
                                        break;

                                    case "srv:SV_ServiceIdentification":
                                        switch (xmlReader.GetAttribute("id"))
                                        {
                                            case "ERDDAP-griddap":
                                                inIdentification = false;
                                                inTemporalExtent = false;
                                                inVerticalExtent = false;
                                                inERDDAP = true;
                                                break;
                                        }
                                        break;

                                    case "srv:SV_OperationMetadata":
                                        if (inERDDAP)
                                        {
                                            inOperationMetadata = true;
                                        }
                                        break;

                                    case "gmd:URL":
                                        if (inOperationMetadata)
                                        {
                                            url = xmlReader.ReadElementContentAsString();
                                            inOperationMetadata = false;
                                            inERDDAP = false;
                                        }
                                        break;

                                    case "gmd:dimension":
                                        inParameterDimension = true;
                                        break;

                                    case "gco:MemberName":
                                        if (inParameterDimension)
                                        {
                                            currentStringParameter = "parameterName";
                                        }
                                        break;

                                    case "gco:attributeType":
                                        if (inParameterDimension)
                                        {
                                            currentStringParameter = "parameterAttribute";
                                        }
                                        break;

                                    case "gmd:descriptor":
                                        if (inParameterDimension)
                                        {
                                            currentStringParameter = "parameterDescription";
                                        }
                                        break;

                                    case "gmd:distributionInfo":
                                        inParameterDimension = false;
                                        break;
                                }
                            }
                        }
                        Dictionary<string, (string name, int size, double spacing)> dictDimensions = new Dictionary<string, (string name, int size, double spacing)>();
                        dictDimensions.Add("row", (rowUOM, rowSize, rowResolution));
                        dictDimensions.Add("column", (columnUOM, columnSize, columnResolution));
                        dictDimensions.Add("temporal", (temporalUOM, temporalSize, temporalResolution));
                        dictDimensions.Add("vertical", (verticalUOM, verticalSize, verticalResolution));
                        var erd = new ERDDAPMetadataReadEventArgs(dataTitle, dataAbstract, beginPosition, endPosition, identifier, url, credit, temporalSize, new Dictionary<string, (string unit, string description)>(_parameters), files[n]);
                        erd.SetBounds(westBound, eastBound, northBound, southBound);
                        erd.Dimensions = dictDimensions;
                        erd.RowSize = rowResolution;
                        erd.ColumnSize = columnResolution;
                        erd.LegalConstraints = legalConstraints;
                        OnMetadataRead?.Invoke(null, erd);
                        columnSize = 0;
                        rowSize = 0;
                        verticalSize = 0;
                        temporalSize = 0;
                        _parameters.Clear();
                    }
                }
            }
            return files.Length;
        }

        public static void UpdateMetadataEndPosition(string fileName, string updateEndPosition)
        {
            _filenameForUpdate = fileName;
            _endPositionForUpdate = updateEndPosition;
            ReadISO9115Metadata();
        }

        public static void SaveMetadataDirectorySetting(string folderName)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            reg_key.SetValue("ERDDAPMetadataFolder", folderName);
        }
    }
}