using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using Microsoft.Win32;

namespace FAD3.Mapping.Classes
{
    public static class ERDDAPMetadataHandler
    {
        public static string MetadataDirectory { get; set; }
        private static Dictionary<string, string> _parameters = new Dictionary<string, string>();
        public static EventHandler<ERDDAPMetadataReadEventArgs> OnMetadataRead;

        public static int ReadISO9115Metadata()
        {
            _parameters.Clear();
            string[] files = Directory.GetFiles(MetadataDirectory, "*_iso19115.xml");
            bool isTemporalDimension = false;
            bool isTemporalExtent = false;
            bool isElementIdentifier = false;
            bool isURL = false;
            bool isURL2 = false;
            bool isContent = false;
            bool isDimension = false;
            bool isMemberName = false;
            bool isAttributeType = false;
            bool isDataID = false;
            bool isCitationTitle = false;
            bool isAbstract = false;
            bool isCredit = false;
            bool isColumnDimension = false;
            bool isRowDimenstion = false;
            string credit = "";
            int temporalSize = 0;
            double rowSize = 0;
            double columnSize = 0;
            string dateStart = "";
            string dateEnd = "";
            string fileIdentifier = "";
            string url = "";
            string parameterName = "";
            string parameterType = "";
            string dataTitle = "";
            string dataAbstract = "";
            if (files.Length > 0)
            {
                for (int n = 0; n < files.Length; n++)
                {
                    XmlTextReader xmlReader = new XmlTextReader(files[n]);
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType == XmlNodeType.Element)
                        {
                            switch (xmlReader.Name)
                            {
                                case "gmd:fileIdentifier":
                                    isElementIdentifier = true;
                                    break;

                                case "gco:CharacterString":
                                    if (isElementIdentifier)
                                    {
                                        fileIdentifier = xmlReader.ReadElementContentAsString();
                                        isElementIdentifier = false;
                                    }
                                    else if (isAttributeType)
                                    {
                                        parameterType = xmlReader.ReadElementContentAsString();
                                        isAttributeType = false;
                                        _parameters.Add(parameterName, parameterType);
                                    }
                                    else if (isMemberName)
                                    {
                                        parameterName = xmlReader.ReadElementContentAsString();
                                    }
                                    else if (isCitationTitle)
                                    {
                                        dataTitle = xmlReader.ReadElementContentAsString();
                                        isCitationTitle = false;
                                    }
                                    else if (isAbstract)
                                    {
                                        dataAbstract = xmlReader.ReadElementContentAsString();
                                        isAbstract = false;
                                    }
                                    else if (isCredit)
                                    {
                                        credit = xmlReader.ReadElementContentAsString();
                                        isCredit = false;
                                    }
                                    break;

                                case "gmd:MD_DimensionNameTypeCode":
                                    switch (xmlReader.GetAttribute("codeListValue"))
                                    {
                                        case "temporal":
                                            isTemporalDimension = true;
                                            break;

                                        case "column":
                                            isColumnDimension = true;
                                            break;

                                        case "row":
                                            isRowDimenstion = true;
                                            break;
                                    }
                                    break;

                                case "gmd:title":
                                    if (isDataID)
                                    {
                                        isCitationTitle = true;
                                    }
                                    break;

                                case "gmd:abstract":
                                    if (isDataID)
                                    {
                                        isAbstract = true;
                                    }
                                    break;

                                case "gmd:credit":
                                    if (isDataID)
                                    {
                                        isCredit = true;
                                    }
                                    break;

                                case "gco:Measure":
                                    if (isRowDimenstion)
                                    {
                                        rowSize = xmlReader.ReadElementContentAsDouble();
                                        isRowDimenstion = false;
                                    }
                                    else if (isColumnDimension)
                                    {
                                        columnSize = xmlReader.ReadElementContentAsDouble();
                                        isColumnDimension = false;
                                    }
                                    break;

                                case "gco:Integer":
                                    if (isTemporalDimension)
                                    {
                                        temporalSize = xmlReader.ReadElementContentAsInt();
                                        isTemporalDimension = false;
                                    }
                                    break;

                                case "gmd:EX_TemporalExtent":
                                    isTemporalExtent = xmlReader.GetAttribute("id") == "boundingTemporalExtent";
                                    break;

                                case "gmd:MD_DataIdentification":
                                    isDataID = xmlReader.GetAttribute("id") == "DataIdentification";
                                    break;

                                case "gml:beginPosition":
                                    if (isTemporalExtent)
                                    {
                                        dateStart = xmlReader.ReadElementContentAsString();
                                    }
                                    break;

                                case "gml:endPosition":
                                    if (isTemporalExtent)
                                    {
                                        dateEnd = xmlReader.ReadElementContentAsString();
                                        isTemporalExtent = false;
                                    }
                                    break;

                                case "srv:SV_ServiceIdentification":
                                    isURL = xmlReader.GetAttribute("id") == "OPeNDAP";
                                    break;

                                case "srv:containsOperations":
                                    if (isURL)
                                    {
                                        isURL2 = true;
                                    }

                                    break;

                                case "gmd:URL":
                                    if (isURL && isURL2)
                                    {
                                        url = xmlReader.ReadElementContentAsString();
                                        isURL = false;
                                        isURL2 = false;
                                    }
                                    break;

                                case "gmd:contentInfo":
                                    isContent = true;
                                    break;

                                case "gmd:dimension":
                                    if (isContent)
                                    {
                                        isDimension = true;
                                    }
                                    break;

                                case "gco:MemberName":
                                    if (isDimension)
                                    {
                                        isMemberName = true;
                                    }
                                    break;

                                case "gco:attributeType":
                                    if (isMemberName)
                                    {
                                        isAttributeType = true;
                                        isMemberName = false;
                                    }
                                    break;
                            }
                        }
                    }
                    var erd = new ERDDAPMetadataReadEventArgs(dataTitle, dataAbstract, dateStart, dateEnd, fileIdentifier, url, credit, temporalSize, _parameters);
                    erd.RowSize = rowSize;
                    erd.ColumnSize = columnSize;
                    OnMetadataRead?.Invoke(null, erd);
                    _parameters.Clear();
                }
            }
            return files.Length;
        }

        public static void SaveMetadataDirectorySetting(string folderName)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            reg_key.SetValue("ERDDAPMetadataFolder", folderName);
        }

        public static string GetMetadataDirectorySetting()
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            return reg_key.GetValue("ERDDAPMetadataFolder", "").ToString();
        }
    }
}