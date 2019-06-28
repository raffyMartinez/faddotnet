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

            VesselType vesselType = VesselType.NotDetermined;
            List<FishingGround> listFishingGrounds = new List<FishingGround>();
            Sampling sampling = new Sampling();
            FishingVessel fishingVessel = new FishingVessel(VesselType.NotDetermined);
            string gearVariationGuid = "";
            string refCode = "";
            string usageCodeID = "";
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlReader.Name)
                    {
                        case "FishCatchMonitoring":
                            SamplingEventArgs sev = new SamplingEventArgs(SamplingRecordStatus.StartImport);
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
                            else
                            {
                                TargetArea = new TargetArea(TargetAreaGuid);
                                TargetArea.TargetAreaName = TargetAreaName;
                                TargetArea.TargetAreaLetter = targetAreaCode;
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
                            SamplingEventArgs sve = new SamplingEventArgs(SamplingRecordStatus.BeginFishingGears);
                            sve.RecordCount = int.Parse(xmlReader.GetAttribute("GearVariationsCount"));
                            OnExportSamplingStatus?.Invoke(null, sve);
                            break;

                        case "GearLocalNames":
                            sve = new SamplingEventArgs(SamplingRecordStatus.BeginGearLocalNames);
                            sve.RecordCount = int.Parse(xmlReader.GetAttribute("LocalNameCount"));
                            OnExportSamplingStatus?.Invoke(null, sve);
                            break;

                        case "GearLocalName":

                            string gearLocalName = xmlReader.GetAttribute("LocalName");

                            NewFisheryObjectName nfo = new NewFisheryObjectName(gearLocalName, FisheryObjectNameType.GearLocalName);
                            Gears.SaveNewLocalName(nfo, xmlReader.GetAttribute("LocalNameGuid"));

                            sve = new SamplingEventArgs(SamplingRecordStatus.GearLocalName);
                            sve.GearLocalName = gearLocalName;
                            OnExportSamplingStatus?.Invoke(null, sve);

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
                            gearVariationGuid = xmlReader.GetAttribute("guid");
                            Gears.AddGearVariation(gearClassGuid, gearVariationName, gearVariationGuid);

                            sve = new SamplingEventArgs(SamplingRecordStatus.FishingGears);
                            sve.GearVariationName = gearVariationName;
                            OnExportSamplingStatus?.Invoke(null, sve);
                            break;

                        case "Specifications":
                            break;

                        case "Specification":
                            GearSpecification gs = new GearSpecification(xmlReader.GetAttribute("ElementName"),
                                                                xmlReader.GetAttribute("ElementType"),
                                                                xmlReader.GetAttribute("SpecRowGUID"),
                                                                int.Parse(xmlReader.GetAttribute("Sequence")));
                            gs.Notes = xmlReader.GetAttribute("Description");
                            ManageGearSpecsClass.SaveGearSpec(gearVariationGuid, gs);
                            break;

                        case "GearRefCodes":
                            break;

                        case "GearRefCode":
                            refCode = xmlReader.GetAttribute("RefCode");
                            Gears.SaveNewGearReferenceCode(refCode, gearVariationGuid, bool.Parse(xmlReader.GetAttribute("IsSubVariation").ToString()));
                            break;

                        case "RefCodeUsage":
                            usageCodeID = xmlReader.GetAttribute("UsageRowID");
                            if (usageCodeID.Length > 0)
                            {
                                var addUsageResult = Gears.AddGearCodeUsageTargetArea(refCode, TargetArea.TargetAreaGuid, usageCodeID);
                                usageCodeID = addUsageResult.NewRow;
                            }
                            break;

                        case "LocalNamesInUse":
                            break;

                        case "LocalNameUsed":
                            if (usageCodeID.Length > 0)
                            {
                                Gears.AddUsageLocalName(usageCodeID, xmlReader.GetAttribute("LocalNameGuid"), xmlReader.GetAttribute("UsageGuid"));
                            }
                            break;

                        case "Taxa":
                            break;

                        case "TaxaItem":
                            int taxaNumber = int.Parse(xmlReader.GetAttribute("TaxaNo"));
                            string taxaName = xmlReader.GetAttribute("Taxa");
                            TaxaCategory.AddTaxa(taxaNumber, taxaName);
                            break;

                        case "AllCatchNames":
                            sve = new SamplingEventArgs(SamplingRecordStatus.BeginCatchNames);
                            sve.RecordCount = int.Parse(xmlReader.GetAttribute("NamesOfCatchCount"));
                            OnExportSamplingStatus?.Invoke(null, sve);
                            break;

                        case "CatchName":
                            string nameGuid = xmlReader.GetAttribute("NameGuid");
                            string name1 = xmlReader.GetAttribute("Name1");
                            string name2 = xmlReader.GetAttribute("Name2");

                            int? taxaNo = null;
                            if (int.TryParse(xmlReader.GetAttribute("TaxaNumber"), out int tn))
                            {
                                taxaNo = tn;
                            }

                            string identification = xmlReader.GetAttribute("Identification");
                            bool inFishbase = bool.Parse(xmlReader.GetAttribute("IsListedInFishbase"));

                            int? fbNumber = null;
                            if (int.TryParse(xmlReader.GetAttribute("FBSpeciesNumber"), out int v))
                            {
                                fbNumber = v;
                            }
                            bool success = CatchName.AddCatchName(nameGuid, identification, name1, name2, taxaNo, inFishbase, fbNumber);
                            sve = new SamplingEventArgs(SamplingRecordStatus.CatchNames);
                            sve.CatchName = $"{name1} {name2}";
                            OnExportSamplingStatus?.Invoke(null, sve);
                            break;

                        case "Samplings":
                            samplingRecords = int.Parse(xmlReader.GetAttribute("SamplingRecordsCount"));
                            OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.BeginSamplings, samplingRecords));
                            break;

                        case "Sampling":
                            sampling = new Sampling(xmlReader.GetAttribute("SamplingGUID"),
                                DateTime.Parse(xmlReader.GetAttribute("SamplingDateTime")),
                                xmlReader.GetAttribute("LandingSiteGuid"),
                                xmlReader.GetAttribute("ReferenceNumber"));
                            sampling.TargetAreaGuid = TargetAreaGuid;
                            sampling.EnumeratorGuid = xmlReader.GetAttribute("EnumeratorGuid");
                            sampling.SamplingType = CatchMonitoringSamplingType.FisheryDependent;
                            if (xmlReader.GetAttribute("SamplingType") != null)
                            {
                                sampling.SamplingType = (CatchMonitoringSamplingType)Enum.Parse(typeof(CatchMonitoringSamplingType), xmlReader.GetAttribute("SamplingType"));
                            }

                            if (DateTime.TryParse(xmlReader.GetAttribute("DateEncoded"), out DateTime result))
                            {
                                sampling.DateEncoded = result;
                            }

                            if (DateTime.TryParse(xmlReader.GetAttribute("GearSetDateTime"), out result))
                            {
                                sampling.GearSettingDateTime = result;
                            }

                            if (DateTime.TryParse(xmlReader.GetAttribute("GearHaulDateTime"), out result))
                            {
                                sampling.GearHaulingDateTime = result;
                            }

                            sampling.GearVariationGuid = xmlReader.GetAttribute("GearVariationGuid");

                            if (bool.TryParse(xmlReader.GetAttribute("HasLiveFish"), out bool hlf))
                            {
                                sampling.HasLiveFish = hlf;
                            }

                            sampling.Notes = xmlReader.GetAttribute("Notes");

                            if (int.TryParse(xmlReader.GetAttribute("NumberOfFishers"), out int nf))
                            {
                                sampling.NumberOfFishers = nf;
                            }

                            if (int.TryParse(xmlReader.GetAttribute("NumberOfHauls"), out int nh))
                            {
                                sampling.NumberOfHauls = nh;
                            }

                            if (int.TryParse(xmlReader.GetAttribute("WeightCatch"), out int wc))
                            {
                                sampling.CatchWeight = wc;
                            }

                            if (int.TryParse(xmlReader.GetAttribute("WeightSample"), out int ws))
                            {
                                sampling.SampleWeight = ws;
                            }

                            break;

                        case "FishingVessel":
                            if (Enum.TryParse(xmlReader.GetAttribute("VesselType"), out VesselType t))
                            {
                                vesselType = t;
                            }
                            fishingVessel = new FishingVessel(vesselType);
                            fishingVessel.Engine = xmlReader.GetAttribute("Engine");
                            if (int.TryParse(xmlReader.GetAttribute("EngineHp"), out int hp))
                            {
                                fishingVessel.EngineHorsepower = hp;
                            }

                            var dimension = xmlReader.GetAttribute("Dimension_BDL").Split(new char[] { 'x', ' ' });
                            if (dimension[0].Length > 0)
                            {
                                fishingVessel.Breadth = double.Parse(dimension[0]);
                                fishingVessel.Depth = double.Parse(dimension[3]);
                                fishingVessel.Length = double.Parse(dimension[6]);
                            }
                            sampling.FishingVessel = fishingVessel;
                            break;

                        case "FishingGrounds":
                            listFishingGrounds.Clear();
                            break;

                        case "FishingGround":
                            int? sg = null;
                            string fg = xmlReader.GetAttribute("Name");
                            if (fg.Length > 0)
                            {
                                if (int.TryParse(xmlReader.GetAttribute("SubGrid"), out int s))
                                {
                                    sg = s;
                                }
                                listFishingGrounds.Add(new FishingGround(fg, sg));
                            }
                            break;

                        case "CatchComposition":
                            sampling.FishingGroundList = listFishingGrounds;
                            Samplings sp = new Samplings();
                            if (sp.UpdateEffort(true, sampling))
                            {
                                OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.Samplings, sampling.ReferenceNumber));
                            }
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
            OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.EndImport));
            xmlReader.Close();
            return proceed;
        }

        public bool Export()
        {
            OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.StartExport));
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
                OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.Header));

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
                        OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.Extents));
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
                    OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.Enumerator));
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
                    OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.LandingSites));
                }

                //fishing gears
                {
                    writer.WriteStartElement("FishingGears");
                    {
                        writer.WriteAttributeString("GearVariationsCount", Gears.GearVariationCount().ToString());

                        //gear local names
                        {
                            writer.WriteStartElement("GearLocalNames");
                            {
                                if (Gears.GearLocalNames.Count == 0)
                                {
                                    Gears.GetGearLocalNames();
                                }
                                var localNames = Gears.GearLocalNames;
                                writer.WriteAttributeString("LocalNameCount", localNames.Count.ToString());
                                foreach (KeyValuePair<string, string> kv in localNames)
                                {
                                    writer.WriteStartElement("GearLocalName");
                                    writer.WriteAttributeString("LocalName", kv.Value);
                                    writer.WriteAttributeString("LocalNameGuid", kv.Key);
                                    writer.WriteEndElement();
                                }
                            }
                            writer.WriteEndElement();
                        }

                        //gear classes
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

                                            //gear specs
                                            {
                                                writer.WriteStartElement("Specifications");
                                                {
                                                    foreach (GearSpecification gs in ManageGearSpecsClass.GearVariationSpecs(gear.Key))
                                                    {
                                                        writer.WriteStartElement("Specification");
                                                        writer.WriteAttributeString("SpecRowGUID", gs.RowGuid);
                                                        writer.WriteAttributeString("ElementType", gs.Type);
                                                        writer.WriteAttributeString("ElementName", gs.Property);
                                                        writer.WriteAttributeString("Description", gs.Notes);
                                                        writer.WriteAttributeString("Sequence", gs.Sequence.ToString());
                                                        writer.WriteEndElement();
                                                    }
                                                }
                                                writer.WriteEndElement();
                                            }

                                            //ref codes
                                            {
                                                writer.WriteStartElement("GearRefCodes");
                                                {
                                                    foreach (KeyValuePair<string, bool> refCode in Gears.GearSubVariations(gear.Key))
                                                    {
                                                        writer.WriteStartElement("GearRefCode");
                                                        writer.WriteAttributeString("RefCode", refCode.Key);
                                                        writer.WriteAttributeString("IsSubVariation", refCode.Value.ToString());

                                                        //ref code usage
                                                        {
                                                            writer.WriteStartElement("RefCodeUsage");
                                                            writer.WriteAttributeString("UsageRowID", Gears.RefCodeTargetAreaRowGuid(refCode.Key, TargetArea.TargetAreaGuid));

                                                            //local name of gear in target area
                                                            {
                                                                writer.WriteStartElement("LocalNamesInUse");
                                                                foreach (var item in Gears.GearLocalName_TargetArea(refCode.Key, TargetArea.TargetAreaGuid))
                                                                {
                                                                    writer.WriteStartElement("LocalNameUsed");
                                                                    writer.WriteAttributeString("LocalNameGuid", item.Key);
                                                                    writer.WriteAttributeString("UsageGuid", item.Value.RowNumber);
                                                                    writer.WriteEndElement();
                                                                }
                                                                writer.WriteEndElement();
                                                            }
                                                            writer.WriteEndElement();
                                                        }
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
                            }
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.FishingGears));
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
                    OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.Taxa));
                }

                //All catch names
                {
                    writer.WriteStartElement("AllCatchNames");
                    var listNames = CatchComposition.RetriveAllCatchFromTargetArea(TargetArea.TargetAreaGuid);
                    writer.WriteAttributeString("NamesOfCatchCount", listNames.Count.ToString());
                    OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.BeginCatchNames, listNames.Count));
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
                        OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.CatchNames, catchItem.Name1, catchItem.Name2));
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
                        OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.BeginSamplings, recordCount));
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
                                    foreach (var fg in sample.FishingGroundList)
                                    {
                                        writer.WriteStartElement("FishingGround");
                                        writer.WriteAttributeString("Name", fg.GridName);
                                        writer.WriteAttributeString("SubGrid", fg.SubGrid.ToString());
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
                            OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.Samplings, sample.ReferenceNumber));
                        }
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            writer.WriteEndDocument();
            writer.Close();
            success = true;

            OnExportSamplingStatus?.Invoke(null, new SamplingEventArgs(SamplingRecordStatus.EndExport));

            return success;
        }
    }
}