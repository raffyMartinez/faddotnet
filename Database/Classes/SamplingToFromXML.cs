using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FAD3.Database.Classes;
using System.IO;

namespace FAD3.Database.Classes
{
    public class SamplingToFromXML
    {
        public string XMLFilename { get; set; }
        public TargetArea TargetArea { get; set; }
        public bool NewTargetArea { get; set; }
        public string TargetAreaName { get; set; }
        public string TargetAreaGuid { get; set; }
        public string TargetAreaCode { get; set; }
        public EventHandler<SamplingEventArgs> OnExportSamplingStatus;

        public SamplingToFromXML(string fileName)
        {
            XMLFilename = fileName;
        }

        public SamplingToFromXML(TargetArea targetArea, string fileName)
        {
            XMLFilename = fileName;
            TargetArea = targetArea;
        }

        public bool Import()
        {
            int elementCounter = 0;
            int samplingRecords = 0;
            bool proceed = false;
            fadSubgridStyle subgridStyle = fadSubgridStyle.SubgridStyleNone;
            fadUTMZone zone = fadUTMZone.utmZone_Undefined;
            XmlTextReader xmlReader = new XmlTextReader(XMLFilename);
            string gearClassGuid = "";
            int mapCount = 0;
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlReader.Name)
                    {
                        case "FishCatchMonitoring":
                            SamplingEventArgs sev = new SamplingEventArgs(ExportSamplingStatus.StartImport);
                            sev.SamplingToFromXML = this;
                            sev.TargetAreaName = TargetAreaName;
                            OnExportSamplingStatus?.Invoke(null, sev);
                            string targetAreaCode = xmlReader.GetAttribute("TargetAreaCode");
                            subgridStyle = (fadSubgridStyle)Enum.Parse(typeof(fadSubgridStyle), xmlReader.GetAttribute("SubGridStyle"));
                            zone = (fadUTMZone)Enum.Parse(typeof(fadUTMZone), xmlReader.GetAttribute("UTMZone"));
                            if (NewTargetArea
                                && !TargetArea.AddNewTargetArea(TargetAreaName, TargetAreaGuid, targetAreaCode, subgridStyle, zone))
                            {
                            }
                            break;

                        case "FishingGroundMaps":
                            break;

                        case "FishingGroundMap":
                            string ulg = xmlReader.GetAttribute("ulg");
                            string lrg = xmlReader.GetAttribute("lrg");
                            string description = xmlReader.GetAttribute("description");
                            FishingGrid.AddGrid25Map(TargetAreaGuid, subgridStyle, zone, ulg, lrg, description, mapCount == 0);
                            mapCount++;
                            break;

                        case "Enumerators":
                            break;

                        case "Enumerator":
                            string enumeratorGUID = xmlReader.GetAttribute("EnumeratorGuid");
                            string enumeratorName = xmlReader.GetAttribute("Name");
                            DateTime hiredate = DateTime.Parse(xmlReader.GetAttribute("DateHired"));
                            bool active = bool.Parse(xmlReader.GetAttribute("Active"));
                            Enumerators.SaveNewTargetAreaEnumerator(TargetAreaGuid, enumeratorName, hiredate, active, enumeratorGUID);
                            break;

                        case "LandingSites":
                            break;

                        case "LandingSite":
                            string landingsiteGuid = xmlReader.GetAttribute("LandingSiteGuid");
                            string landingsiteName = xmlReader.GetAttribute("LandingSiteName");
                            int munNo = int.Parse(xmlReader.GetAttribute("MunicipalityNumber"));
                            string sx = xmlReader.GetAttribute("x_coordinate");
                            string sy = xmlReader.GetAttribute("y_coordinate");
                            double? x = sx.Length == 0 ? null : (double?)double.Parse(sx);
                            double? y = sy.Length == 0 ? null : (double?)double.Parse(sy);
                            Landingsite.AddNewLandingSite(TargetAreaGuid, landingsiteGuid, landingsiteName, munNo, x, y);
                            break;

                        case "FishingGears":
                            break;

                        case "GearClasses":
                            break;

                        case "GearClass":
                            gearClassGuid = xmlReader.GetAttribute("ClassGuid");
                            string gearClassName = xmlReader.GetAttribute("ClassName");
                            string gearClassLetter = xmlReader.GetAttribute("ClassLetter");
                            GearClass.AddGearClass(gearClassName, gearClassLetter, gearClassGuid);
                            break;

                        case "GearVariation":
                            string gearVariationName = xmlReader.GetAttribute("name");
                            string gearVariationGuid = xmlReader.GetAttribute("guid");
                            Gears.AddGearVariation(gearClassGuid, gearVariationName, gearVariationGuid);
                            break;

                        case "Taxa":
                            break;

                        case "TaxaItem":
                            int taxaNumber = int.Parse(xmlReader.GetAttribute("TaxaNo"));
                            string taxaName = xmlReader.GetAttribute("Taxa");
                            TaxaCategory.AddTaxa(taxaNumber, taxaName);
                            break;

                        case "AllCatchNames":
                            break;

                        case "CatchName":
                            string nameGuid = xmlReader.GetAttribute("NameGuid");
                            string name1 = xmlReader.GetAttribute("Name1");
                            string name2 = xmlReader.GetAttribute("Name2");
                            taxaNumber = int.Parse(xmlReader.GetAttribute("TaxaNumber"));
                            string identification = xmlReader.GetAttribute("Identification");
                            bool inFishbase = bool.Parse(xmlReader.GetAttribute("IsListedInFishbase"));

                            int? fbNumber = null;
                            if (int.TryParse(xmlReader.GetAttribute("FBSpeciesNumber"), out int v))
                            {
                                fbNumber = v;
                            }
                            CatchName.AddCatchName(nameGuid, identification, name1, name2, taxaNumber, inFishbase, fbNumber);
                            break;

                        case "Samplings":
                            samplingRecords = int.Parse(xmlReader.GetAttribute("SamplingRecordsCount"));
                            OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.BeginSamplings, samplingRecords));
                            break;

                        case "Sampling":
                            string refNumber = xmlReader.GetAttribute("ReferenceNumber");
                            OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.Samplings, refNumber));
                            break;

                        case "FishingVessel":
                            break;

                        case "FishingGrounds":
                            break;

                        case "FishingGround":
                            break;

                        case "CatchComposition":
                            break;

                        case "Catch":
                            break;

                        case "LengthFrequency":
                            break;

                        case "LengthFrequencyItem":
                            break;

                        case "GonadalMaturityStage":
                            break;

                        case "GMSItem":
                            break;
                    }
                    elementCounter++;
                }
                //if (elementCounter > 0 && !proceed)
                //{
                //    OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.Error, "XML file does not contain fish catch monitoring data", 0));
                //    proceed = false;
                //}
            }
            OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.EndImport));
            xmlReader.Close();
            return proceed;
        }

        public bool Export()
        {
            OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.StartExport));
            bool success = false;
            //StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(XMLFilename);
            writer.WriteStartDocument();

            writer.WriteStartElement("FishCatchMonitoring");
            {
                //Sampling header data
                writer.WriteAttributeString("TargetAreaname", TargetArea.TargetAreaName);
                writer.WriteAttributeString("TargetAreaGUID", TargetArea.TargetAreaGuid);
                writer.WriteAttributeString("TargetAreaCode", TargetArea.TargetAreaLetter);
                writer.WriteAttributeString("UTMZone", TargetArea.UTMZone.ToString());
                writer.WriteAttributeString("SubGridStyle", TargetArea.SubgridStyle.ToString());
                OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.Header));

                //target area extents
                {
                    if (TargetArea.ListGridExtents != null)
                    {
                        writer.WriteStartElement("FishingGroundMaps");
                        foreach (var map in TargetArea.ListGridExtents)
                        {
                            writer.WriteStartElement("FishingGroundMap");
                            writer.WriteAttributeString("ulg", map.UpperLeft);
                            writer.WriteAttributeString("lrg", map.LowerRight);
                            writer.WriteAttributeString("description", map.GridDescription);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.Extents));
                    }
                }

                //sampling enumerators
                {
                    writer.WriteStartElement("Enumerators");
                    foreach (var en in Enumerators.GetTargetAreaEnumerators(TargetArea.TargetAreaGuid))
                    {
                        writer.WriteStartElement("Enumerator");
                        writer.WriteAttributeString("EnumeratorGuid", en.Key);
                        writer.WriteAttributeString("Name", en.Value.EnumeratorName);
                        writer.WriteAttributeString("DateHired", en.Value.DateHired.ToShortDateString());
                        writer.WriteAttributeString("Active", en.Value.IsActive.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.Enumerator));
                }

                //Landing sites
                {
                    var landingSites = Landingsite.GetLandingSitesInTargetArea(TargetArea.TargetAreaGuid);
                    writer.WriteStartElement("LandingSites");
                    foreach (var ls in landingSites)
                    {
                        writer.WriteStartElement("LandingSite");
                        writer.WriteAttributeString("LandingSiteGuid", ls.Key);
                        writer.WriteAttributeString("LandingSiteName", ls.Value.name);
                        writer.WriteAttributeString("MunicipalityNumber", ls.Value.municipalityNumber.ToString());
                        writer.WriteAttributeString("HasCoordinate", ls.Value.hasCoordinate.ToString());
                        if (ls.Value.hasCoordinate)
                        {
                            writer.WriteAttributeString("x_coordinate", ls.Value.coord.Longitude.ToString());
                            writer.WriteAttributeString("y_coordinate", ls.Value.coord.Latitude.ToString());
                        }
                        else
                        {
                            writer.WriteAttributeString("x_coordinate", "");
                            writer.WriteAttributeString("y_coordinate", "");
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.LandingSites));
                }

                //fishing gears
                {
                    writer.WriteStartElement("FishingGears");
                    {
                        writer.WriteStartElement("GearClasses");
                        {
                            foreach (var gearClass in Gears.GearClasses.Values)
                            {
                                writer.WriteStartElement("GearClass");
                                writer.WriteAttributeString("ClassGuid", gearClass.GearClassGUID);
                                writer.WriteAttributeString("ClassLetter", gearClass.GearClassLetter);
                                writer.WriteAttributeString("ClassName", gearClass.GearClassName);

                                {
                                    writer.WriteStartElement("GearVariations");
                                    {
                                        foreach (var gear in Gears.GetAllVariations(gearClass.GearClassGUID))
                                        {
                                            writer.WriteStartElement("GearVariation");
                                            writer.WriteAttributeString("guid", gear.Key);
                                            writer.WriteAttributeString("name", gear.Value.VariationName);
                                            writer.WriteAttributeString("mph1", gear.Value.MetaphoneKey1.ToString());
                                            writer.WriteAttributeString("mph2", gear.Value.MetaphoneKey2.ToString());
                                            writer.WriteAttributeString("name2", gear.Value.VariationName2);
                                            writer.WriteEndElement();
                                        }
                                    }
                                    writer.WriteEndElement();
                                }

                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.FishingGears));
                }

                //Taxa
                {
                    writer.WriteStartElement("Taxa");
                    foreach (var item in CatchTaxa.Taxa)
                    {
                        writer.WriteStartElement("TaxaItem");
                        writer.WriteAttributeString("TaxaNo", item.Key.ToString());
                        writer.WriteAttributeString("TaxaName", item.Value);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.Taxa));
                }

                //All catch names
                {
                    writer.WriteStartElement("AllCatchNames");
                    var listNames = CatchComposition.RetriveAllCatchFromTargetArea(TargetArea.TargetAreaGuid);
                    OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.BeginCatchNames, listNames.Count));
                    foreach (var catchItem in listNames)
                    {
                        writer.WriteStartElement("CatchName");
                        writer.WriteAttributeString("NameGuid", catchItem.CatchNameGUID);
                        writer.WriteAttributeString("Name1", catchItem.Name1);
                        writer.WriteAttributeString("Name2", catchItem.Name2);
                        writer.WriteAttributeString("TaxaNumber", catchItem.TaxaNumber.ToString());
                        writer.WriteAttributeString("Identification", catchItem.NameType.ToString());
                        writer.WriteAttributeString("IsListedInFishbase", catchItem.IsListedFB.ToString());
                        writer.WriteAttributeString("FBSpeciesNumber", catchItem.FishBaseSpeciesNumber.ToString());
                        writer.WriteEndElement();
                        OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.CatchNames, catchItem.Name1, catchItem.Name2));
                    }
                    writer.WriteEndElement();
                }

                //samplings
                {
                    writer.WriteStartElement("Samplings");
                    {
                        Samplings samplings = new Samplings(TargetArea.TargetAreaGuid, null);
                        samplings.GetSamplingGuids(TargetArea.TargetAreaGuid);
                        int recordCount = samplings.SamplingGuids.Count;
                        OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.BeginSamplings, recordCount));
                        writer.WriteAttributeString("SamplingRecordsCount", recordCount.ToString());
                        foreach (string guid in samplings.SamplingGuids)
                        {
                            Sampling sample = samplings.ReadSamplings(guid);
                            writer.WriteStartElement("Sampling");
                            writer.WriteAttributeString("SamplingGUID", guid);
                            writer.WriteAttributeString("EnumeratorGuid", sample.EnumeratorGuid);
                            writer.WriteAttributeString("ReferenceNumber", sample.ReferenceNumber);
                            writer.WriteAttributeString("DateEncoded", sample.DateEncoded != null ?
                                 ((DateTime)sample.DateEncoded).ToString("yyyy-MMM-dd HH:mm:ss") : "");
                            writer.WriteAttributeString("SamplingDateTime", sample.SamplingDateTime.ToString("yyyy-MMM-dd HH:mm"));
                            writer.WriteAttributeString("GearSetDateTime", sample.GearSettingDateTime != null ?
                                 ((DateTime)sample.GearSettingDateTime).ToString("yyyy-MMM-dd HH:mm") : "");
                            writer.WriteAttributeString("GearHaulDateTime", sample.GearHaulingDateTime != null ?
                                ((DateTime)sample.GearHaulingDateTime).ToString("yyyy-MMM-dd HH:mm") : "");
                            writer.WriteAttributeString("LandingSiteGuid", sample.LandingSiteGuid);
                            writer.WriteAttributeString("GearVariationGuid", sample.GearVariationGuid);
                            writer.WriteAttributeString("HasLiveFish", sample.HasLiveFish.ToString());
                            writer.WriteAttributeString("Notes", sample.Notes);
                            writer.WriteAttributeString("NumberOfFishers", sample.NumberOfFishers != null ?
                                sample.NumberOfFishers.ToString() : "");
                            writer.WriteAttributeString("NumberOfHauls", sample.NumberOfHauls != null ?
                                sample.NumberOfHauls.ToString() : "");

                            double? wtCatch = sample.CatchWeight;
                            double? wtSample = sample.SampleWeight;
                            if (wtCatch != null)
                            {
                                writer.WriteAttributeString("WeightCatch", wtCatch.ToString());
                                if (wtSample == null)
                                {
                                    writer.WriteAttributeString("WeightSample", "");
                                }
                                else
                                {
                                    writer.WriteAttributeString("WeightSample", wtSample.ToString());
                                }
                            }
                            else
                            {
                                writer.WriteAttributeString("WeightCatch", "");
                                writer.WriteAttributeString("WeightSample", "");
                            }

                            //fishing vessel
                            {
                                writer.WriteStartElement("FishingVessel");
                                FishingVessel fv = sample.FishingVessel;
                                writer.WriteAttributeString("VesselType", fv.VesselType.ToString());
                                writer.WriteAttributeString("Engine", fv.Engine);
                                writer.WriteAttributeString("EngineHp", fv.EngineHorsepower != null ?
                                    fv.EngineHorsepower.ToString() : "");
                                string breadth = fv.Breadth == null ? "" : fv.Breadth.ToString();
                                string depth = fv.Depth == null ? "" : fv.Depth.ToString();
                                string length = fv.Length == null ? "" : fv.Length.ToString();
                                writer.WriteAttributeString("Dimension_BDL", $"{breadth} x {depth} x {length}"); ;
                                writer.WriteEndElement();
                            }

                            //fishing grounds
                            {
                                writer.WriteStartElement("FishingGrounds");
                                {
                                    foreach (var fg in sample.FishingGrounds)
                                    {
                                        writer.WriteStartElement("FishingGround");
                                        writer.WriteAttributeString("Name", fg.FishingGround);
                                        writer.WriteAttributeString("SubGrid", fg.SubGrid);
                                        writer.WriteEndElement();
                                    }
                                }
                                writer.WriteEndElement();
                            }

                            //catch composition
                            {
                                writer.WriteStartElement("CatchComposition");
                                foreach (var catchline in sample.CatchComposition)
                                {
                                    writer.WriteStartElement("Catch");
                                    writer.WriteAttributeString("CatchCompositionRow", catchline.Key);
                                    writer.WriteAttributeString("NameGuid", catchline.Value.CatchNameGUID);
                                    writer.WriteAttributeString("Weight", catchline.Value.CatchWeight.ToString());
                                    writer.WriteAttributeString("Count", catchline.Value.CatchCount.ToString());
                                    writer.WriteAttributeString("SubSampleWeight", catchline.Value.CatchSubsampleWt.ToString());
                                    writer.WriteAttributeString("SubSampleCount", catchline.Value.CatchSubsampleCount.ToString());
                                    writer.WriteAttributeString("FromTotal", catchline.Value.FromTotalCatch.ToString());
                                    writer.WriteAttributeString("IsLiveFish", catchline.Value.LiveFish.ToString());

                                    //LenFreq
                                    {
                                        writer.WriteStartElement("LengthFrequency");
                                        foreach (var lfItem in LengthFreq.LenFreqList(catchline.Key))
                                        {
                                            writer.WriteStartElement("LengthFrequencyItem");
                                            writer.WriteAttributeString("Length", lfItem.Length.ToString());
                                            writer.WriteAttributeString("Freq", lfItem.Freq.ToString());
                                            writer.WriteEndElement();
                                        }
                                        writer.WriteEndElement();
                                    }

                                    //GMS
                                    {
                                        writer.WriteStartElement("GonadalMaturityStage");
                                        foreach (var gmsLine in GMSManager.RetrieveGMSData(catchline.Key))
                                        {
                                            writer.WriteStartElement("GMSItem");
                                            writer.WriteAttributeString("Sex", gmsLine.Value.Sex.ToString());
                                            writer.WriteAttributeString("Length", gmsLine.Value.Length.ToString());
                                            writer.WriteAttributeString("Weight", gmsLine.Value.Weight.ToString());
                                            writer.WriteAttributeString("MaturityStage", gmsLine.Value.GMSNumeric.ToString());
                                            writer.WriteAttributeString("GonadWeight", gmsLine.Value.GonadWeight.ToString());
                                            writer.WriteEndElement();
                                        }
                                        writer.WriteEndElement();
                                    }

                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement();
                            OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.Samplings, sample.ReferenceNumber));
                        }
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            writer.WriteEndDocument();
            writer.Close();
            success = true;

            OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(ExportSamplingStatus.EndExport));

            return success;
        }
    }
}