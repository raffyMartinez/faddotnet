using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using MapWinGIS;
using AxMapWinGIS;

namespace FAD3.Mapping.Classes
{
    public class InventoryMapping
    {
        private AxMap _mapControl;
        private MapLayersHandler _layersHandler;
        public string InventoryGuid { get; set; }
        public static int NumberOfBreaks { get; set; }
        public bool ComparisonAmongLGUs { get; set; }
        public static List<double> GearDataFisherJenksBreaks;
        public static List<double> FisherVesselDataFisherJenksBreaks;
        public static double BreakSourceMaximum { get; set; }
        public static double BreakSourceMinimum { get; set; }
        private List<FisherVesselInventoryItem> _fisherVesselDistributionList;
        private static string _labelXML;
        private static Labels _labels;
        public int InventoryLayerHandle { get; internal set; }

        public static Labels Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                _labelXML = _labels.Serialize();
                if (!_labels.AvoidCollisions)
                {
                    _labelXML = _labelXML.Replace("LabelsClass", "LabelsClass AvoidCollision=\"0\" ");
                }
            }
        }

        public InventoryMapping(AxMap mapControl, MapLayersHandler layersHandler)
        {
            _mapControl = mapControl;
            _layersHandler = layersHandler;
        }

        public List<double> GetFisherVesselJenksBreaks()
        {
            if (FisherVesselDataFisherJenksBreaks == null)
            {
                FisherVesselDataFisherJenksBreaks = GetBreaksFromFisherVesselInventoryData(InventoryGuid, NumberOfBreaks);
            }
            return FisherVesselDataFisherJenksBreaks;
        }

        public List<double> GetGearFisherJenksBreaks()
        {
            if (GearDataFisherJenksBreaks == null)
            {
                GearDataFisherJenksBreaks = GetBreaksFromInventoryData(InventoryGuid, NumberOfBreaks);
            }
            return GearDataFisherJenksBreaks;
        }

        private List<double> GetBreaksFromFisherVesselInventoryData(string inventoryGuid, int numBreaks = 5)
        {
            List<double> source = new List<double>();
            var dt = new DataTable();
            string sql = $@"SELECT Sum(CountMunicipalMotorized) AS Motorized,
                            Sum(CountMunicipalNonMotorized) AS NonMotorized,
                            Sum(CountCommercial) AS Commercial
                            FROM tblGearInventoryBarangay
                            WHERE InventoryGuid={{{inventoryGuid}}}
                            GROUP BY Municipality";

            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];

                        double v = (double)dr["Motorized"];
                        if (v > 0)
                        {
                            source.Add(v);
                        }

                        v = (double)dr["NonMotorized"];
                        if (v > 0)
                        {
                            source.Add(v);
                        }

                        v = (double)dr["Commercial"];
                        if (v > 0)
                        {
                            source.Add(v);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "InventoryMapping.cs", "GetBreaksFromInventoryData");
                }
            }
            BreakSourceMaximum = source.Max();
            BreakSourceMinimum = source.Min();
            return JenksFisher.CreateJenksFisherBreaksArray(source, numBreaks);
        }

        private List<double> GetBreaksFromInventoryData(string inventoryGuid, int numBreaks = 5)
        {
            List<double> source = new List<double>();
            var dt = new DataTable();
            string sql = $@"SELECT Sum([tblGearInventoryBarangayData].[CountCommercial]
                            +[tblGearInventoryBarangayData].[CountMunicipalMotorized]
                            +[tblGearInventoryBarangayData].[CountMunicipalNonMotorized]
                            +[tblGearInventoryBarangayData].[CountNoBoat]) AS TotalCount
                        FROM tblGearInventoryBarangay INNER JOIN tblGearInventoryBarangayData
                            ON tblGearInventoryBarangay.BarangayInventoryGuid = tblGearInventoryBarangayData.BarangayInventoryGUID
                        WHERE tblGearInventoryBarangay.InventoryGuid={{{inventoryGuid}}}
                        GROUP BY tblGearInventoryBarangay.Municipality, tblGearInventoryBarangayData.GearVariation
                        ORDER BY Sum([tblGearInventoryBarangayData].[CountCommercial]
                            +[tblGearInventoryBarangayData].[CountMunicipalMotorized]
                            +[tblGearInventoryBarangayData].[CountMunicipalNonMotorized]
                            +[tblGearInventoryBarangayData].[CountNoBoat]) DESC";

            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        BreakSourceMaximum = (double)dt.Rows[0]["TotalCount"];
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        source.Add((double)dr["TotalCount"]);
                    }
                    BreakSourceMinimum = (double)dt.Rows[dt.Rows.Count - 1]["TotalCount"];
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "InventoryMapping.cs", "GetBreaksFromInventoryData");
                }
            }
            return JenksFisher.CreateJenksFisherBreaksArray(source, numBreaks);
        }

        public bool MapFisherBoatDistribution(List<FisherVesselInventoryItem> distributionList, string itemToMap)
        {
            Shapefile sf = new Shapefile();
            if (sf.CreateNew("", ShpfileType.SHP_POINT))
            {
                sf.GeoProjection = _mapControl.GeoProjection;
                int ifldProjectName = sf.EditAddField("Inventory project", FieldType.STRING_FIELD, 1, 50);
                int ifldProvince = sf.EditAddField("Province", FieldType.STRING_FIELD, 1, 30);
                int ifldMunicipalityName = sf.EditAddField("Municipality", FieldType.STRING_FIELD, 1, 30);
                int ifldMuni = sf.EditAddField("Muni", FieldType.STRING_FIELD, 1, 4);
                int ifldX = sf.EditAddField("x", FieldType.DOUBLE_FIELD, 9, 12);
                int ifldY = sf.EditAddField("y", FieldType.DOUBLE_FIELD, 9, 12);
                int ifldFishers = sf.EditAddField("Fishers", FieldType.INTEGER_FIELD, 1, 7);
                int ifldCommercial = sf.EditAddField("Commercial", FieldType.INTEGER_FIELD, 1, 7);
                int ifldMunicipalMot = sf.EditAddField("Municipal Motorized", FieldType.INTEGER_FIELD, 1, 7);
                int ifldMunicipalNonMot = sf.EditAddField("Municipal Non-motorized", FieldType.INTEGER_FIELD, 1, 7);
                foreach (var item in distributionList)
                {
                    Shape sh = new Shape();
                    if (sh.Create(ShpfileType.SHP_POINT) && sh.AddPoint(item.X, item.Y) >= 0)
                    {
                        var iShp = sf.EditAddShape(sh);
                        if (iShp >= 0)
                        {
                            sf.EditCellValue(ifldProjectName, iShp, item.InventoryProjectName);
                            sf.EditCellValue(ifldProvince, iShp, item.ProvinceName);
                            sf.EditCellValue(ifldMunicipalityName, iShp, item.Municipality);
                            sf.EditCellValue(ifldMuni, iShp, Database.Classes.LGUs.ShortenPlaceName(item.Municipality));
                            sf.EditCellValue(ifldX, iShp, item.X);
                            sf.EditCellValue(ifldY, iShp, item.Y);
                            sf.EditCellValue(ifldFishers, iShp, item.CountFisher);
                            sf.EditCellValue(ifldCommercial, iShp, item.CountCommercial);
                            sf.EditCellValue(ifldMunicipalMot, iShp, item.CountMunicipalMotorized);
                            sf.EditCellValue(ifldMunicipalNonMot, iShp, item.CountMunicipalNonMotorized);
                        }
                    }
                }
                sf.DefaultDrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                sf.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Blue);
                sf.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.White);
                sf.DefaultDrawingOptions.LineVisible = true;
                sf.CollisionMode = tkCollisionMode.AllowCollisions;

                int fld = 0;
                string itemName = "";
                switch (itemToMap)
                {
                    case "fishers":
                        itemName = "Total number of fishers";
                        fld = ifldFishers;
                        break;

                    case "commercial":
                        fld = ifldCommercial;
                        itemName = "Total number of commercial fishing vessels";
                        break;

                    case "municipalMotorized":
                        fld = ifldMunicipalMot;
                        itemName = "Total number of municipal motorized vessels";
                        break;

                    case "municipalNonMotorized":
                        fld = ifldMunicipalNonMot;
                        itemName = "Total number of municipal non-motorized vessels";
                        break;
                }

                Database.Classes.ClassificationType classificationType = Database.Classes.ClassificationType.NaturalBreaks;
                if (itemToMap == "fishers" || ComparisonAmongLGUs)
                {
                    ShapefileLayerHelper.CategorizeNumericPointLayer(sf, fld);
                }
                else
                {
                    ShapefileLayerHelper.CategorizeNumericPointLayer(sf, FisherVesselDataFisherJenksBreaks, fld, BreakSourceMaximum);
                    classificationType = Database.Classes.ClassificationType.JenksFisher;
                }

                InventoryLayerHandle = _layersHandler.AddLayer(sf, itemName);
                _layersHandler[InventoryLayerHandle].IgnoreZeroWhenClassifying = true;
                _layersHandler[InventoryLayerHandle].ClassificationType = classificationType;

                if (Labels != null)
                {
                    sf.Labels.Clear();
                    _layersHandler.ShapeFileLableHandler.LabelShapefile(_labelXML);
                }
                _layersHandler.SetAsPointLayerFromDatabase(_layersHandler[InventoryLayerHandle]);
            }
            return sf.NumShapes > 0;
        }

        public bool MapGearDistribution(List<GearInventoryMappingItem> distributionList, string gearName, bool doFisherJenks = false)
        {
            Shapefile sf = new Shapefile();
            if (sf.CreateNew("", ShpfileType.SHP_POINT))
            {
                sf.GeoProjection = _mapControl.GeoProjection;
                int ifldProjectName = sf.EditAddField("Inventory project", FieldType.STRING_FIELD, 1, 50);
                int ifldProvince = sf.EditAddField("Province", FieldType.STRING_FIELD, 1, 30);
                int ifldMunicipalityName = sf.EditAddField("Municipality", FieldType.STRING_FIELD, 1, 30);
                int ifldMuni = sf.EditAddField("Muni", FieldType.STRING_FIELD, 1, 8);
                int ifldGear = sf.EditAddField("Gear", FieldType.STRING_FIELD, 1, 50);
                int ifldX = sf.EditAddField("x", FieldType.DOUBLE_FIELD, 9, 12);
                int ifldY = sf.EditAddField("y", FieldType.DOUBLE_FIELD, 9, 12);
                int ifldCount = sf.EditAddField("n", FieldType.INTEGER_FIELD, 1, 7);
                int ifldCommercial = sf.EditAddField("Commercial", FieldType.INTEGER_FIELD, 1, 7);
                int ifldMunicipalMot = sf.EditAddField("Motorized", FieldType.INTEGER_FIELD, 1, 7);
                int ifldMunicipalNonMot = sf.EditAddField("Non-motorized", FieldType.INTEGER_FIELD, 1, 7);
                int ifldNoBoat = sf.EditAddField("No boat", FieldType.INTEGER_FIELD, 1, 7);
                foreach (var item in distributionList)
                {
                    Shape sh = new Shape();
                    if (sh.Create(ShpfileType.SHP_POINT) && sh.AddPoint(item.X, item.Y) >= 0)
                    {
                        var iShp = sf.EditAddShape(sh);
                        if (iShp >= 0)
                        {
                            sf.EditCellValue(ifldProjectName, iShp, item.InventoryProjectName);
                            sf.EditCellValue(ifldProvince, iShp, item.ProvinceName);
                            sf.EditCellValue(ifldMunicipalityName, iShp, item.Municipality);
                            //sf.EditCellValue(ifldMunNameAbbrev, iShp, item.Municipality.Substring(0, 4));
                            sf.EditCellValue(ifldMuni, iShp, Database.Classes.LGUs.ShortenPlaceName(item.Municipality));
                            sf.EditCellValue(ifldGear, iShp, item.GearVariationName);
                            sf.EditCellValue(ifldX, iShp, item.X);
                            sf.EditCellValue(ifldY, iShp, item.Y);
                            sf.EditCellValue(ifldCount, iShp, item.TotalUsed);
                            sf.EditCellValue(ifldCommercial, iShp, item.CountCommercial);
                            sf.EditCellValue(ifldMunicipalMot, iShp, item.CountMunicipalMotorized);
                            sf.EditCellValue(ifldMunicipalNonMot, iShp, item.CountMunicipalNonMotorized);
                            sf.EditCellValue(ifldNoBoat, iShp, item.CountNoBoat);
                        }
                    }
                }
                sf.DefaultDrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                sf.DefaultDrawingOptions.FillColor = new Utils().ColorByName(tkMapColor.Blue);
                sf.DefaultDrawingOptions.LineColor = new Utils().ColorByName(tkMapColor.White);
                sf.DefaultDrawingOptions.LineVisible = true;
                sf.CollisionMode = tkCollisionMode.AllowCollisions;
                Database.Classes.ClassificationType classificationType = Database.Classes.ClassificationType.NaturalBreaks;
                if (ComparisonAmongLGUs)
                {
                    ShapefileLayerHelper.CategorizeNumericPointLayer(sf, ifldCount);
                }
                else
                {
                    ShapefileLayerHelper.CategorizeNumericPointLayer(sf, GearDataFisherJenksBreaks, ifldCount, BreakSourceMaximum);
                    classificationType = Database.Classes.ClassificationType.JenksFisher;
                }
                InventoryLayerHandle = _layersHandler.AddLayer(sf, gearName);
                _layersHandler[InventoryLayerHandle].ClassificationType = classificationType;
                //_layersHandler.set_MapLayer(h);

                if (Labels != null)
                {
                    sf.Labels.Clear();
                    _layersHandler.ShapeFileLableHandler.LabelShapefile(_labelXML);
                }

                _layersHandler.SetAsPointLayerFromDatabase(_layersHandler[InventoryLayerHandle]);
            }
            return sf.NumShapes > 0;
        }

        public List<FisherVesselInventoryItem> GetFisherVesselDistribution(string itemToMap, bool refresh = false)
        {
            int count = 0;
            if (_fisherVesselDistributionList == null || refresh)
            {
                _fisherVesselDistributionList = new List<FisherVesselInventoryItem>();

                string sql = $@"SELECT tblGearInventories.InventoryName, Provinces.ProvinceName,
                        Municipalities.Municipality, Sum(tblGearInventoryBarangay.CountFishers) AS Fishers,
                        Sum(tblGearInventoryBarangay.CountCommercial) AS Commercial,
                        Sum(tblGearInventoryBarangay.CountMunicipalMotorized) AS [Municipal motorized],
                        Sum(tblGearInventoryBarangay.CountMunicipalNonMotorized) AS [Non-motorized],
                        Municipalities.xCoord, Municipalities.yCoord
                        FROM tblGearInventories
                            INNER JOIN ((Provinces
                            INNER JOIN Municipalities
                                ON Provinces.ProvNo = Municipalities.ProvNo)
                            INNER JOIN tblGearInventoryBarangay
                                ON (Municipalities.MunNo = tblGearInventoryBarangay.Municipality)
                                AND (Municipalities.MunNo = tblGearInventoryBarangay.Municipality))
                                ON tblGearInventories.InventoryGuid = tblGearInventoryBarangay.InventoryGuid
                        WHERE tblGearInventoryBarangay.InventoryGuid={{{InventoryGuid}}}
                        GROUP BY tblGearInventories.InventoryName, Provinces.ProvinceName, Municipalities.Municipality,
                        Municipalities.xCoord, Municipalities.yCoord";
                var dt = new DataTable();
                using (var conection = new OleDbConnection(global.ConnectionString))
                {
                    try
                    {
                        conection.Open();
                        var adapter = new OleDbDataAdapter(sql, conection);
                        adapter.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            FisherVesselInventoryItem item = new FisherVesselInventoryItem((double)dr["xCoord"], (double)dr["yCoord"], dr["InventoryName"].ToString(), dr["ProvinceName"].ToString());
                            item.Municipality = dr["Municipality"].ToString();

                            count = (int)(double)dr["Fishers"];
                            if (count > 0)
                            {
                                item.CountFisher = count;
                            }

                            count = (int)(double)dr["Commercial"];
                            if (count > 0)
                            {
                                item.CountCommercial = count;
                            }

                            count = (int)(double)dr["Municipal motorized"];
                            if (count > 0)
                            {
                                item.CountMunicipalMotorized = count;
                            }

                            count = (int)(double)dr["Non-motorized"];
                            if (count > 0)
                            {
                                item.CountMunicipalNonMotorized = count;
                            }
                            _fisherVesselDistributionList.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.Message, "InventoryMapping.cs", "GetGearVariationDistribution");
                    }
                }
            }
            MapFisherBoatDistribution(_fisherVesselDistributionList, itemToMap);
            return _fisherVesselDistributionList;
        }

        public List<GearInventoryMappingItem> GetGearVariationDistribution(string gearName, string gearVariationGuid, int provinceNumber = 0)
        {
            List<GearInventoryMappingItem> gearDistributionList = new List<GearInventoryMappingItem>();
            string sql = "";
            if (provinceNumber > 0)
            {
                sql = "";
            }
            else
            {
                sql = $@"SELECT tblGearInventories.InventoryName, Provinces.ProvinceName, Municipalities.Municipality,
                    tblGearVariations.Variation, Municipalities.xCoord, Municipalities.yCoord,
                    Sum([tblGearInventoryBarangayData.CountCommercial]
                        +[tblGearInventoryBarangayData.CountMunicipalMotorized]
                        +[tblGearInventoryBarangayData.CountMunicipalNonMotorized]
                        +[tblGearInventoryBarangayData.CountNoBoat]) AS [Total used],
                    Sum(tblGearInventoryBarangayData.CountCommercial) AS Commercial,
                    Sum(tblGearInventoryBarangayData.CountMunicipalMotorized) AS [Municipal motorized],
                    Sum(tblGearInventoryBarangayData.CountMunicipalNonMotorized) AS [Non-motorized],
                    Sum(tblGearInventoryBarangayData.CountNoBoat) AS [No boat]
                    FROM tblGearVariations
                        INNER JOIN ((tblGearInventories
                        INNER JOIN ((Provinces
                        INNER JOIN Municipalities
                            ON Provinces.ProvNo = Municipalities.ProvNo)
                        INNER JOIN tblGearInventoryBarangay
                            ON (Municipalities.MunNo = tblGearInventoryBarangay.Municipality)
                            AND (Municipalities.MunNo = tblGearInventoryBarangay.Municipality))
                            ON tblGearInventories.InventoryGuid = tblGearInventoryBarangay.InventoryGuid)
                        INNER JOIN tblGearInventoryBarangayData
                            ON tblGearInventoryBarangay.BarangayInventoryGuid = tblGearInventoryBarangayData.BarangayInventoryGUID)
                            ON tblGearVariations.GearVarGUID = tblGearInventoryBarangayData.GearVariation
                    WHERE tblGearInventories.InventoryGuid= {{{InventoryGuid}}}
                        AND tblGearVariations.GearVarGUID={{{gearVariationGuid}}}
                    GROUP BY tblGearInventories.InventoryName, Provinces.ProvinceName,
                    Municipalities.Municipality, tblGearVariations.Variation, Municipalities.xCoord, Municipalities.yCoord";
            }

            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        GearInventoryMappingItem item = new GearInventoryMappingItem((double)dr["xCoord"], (double)dr["yCoord"], dr["Variation"].ToString(), dr["InventoryName"].ToString(), dr["ProvinceName"].ToString());
                        item.Municipality = dr["Municipality"].ToString();
                        item.TotalUsed = (int)(double)dr["Total used"];
                        item.CountCommercial = (int)(double)dr["Commercial"];
                        item.CountMunicipalMotorized = (int)(double)dr["Municipal motorized"];
                        item.CountMunicipalNonMotorized = (int)(double)dr["Non-motorized"];
                        item.CountNoBoat = (int)(double)dr["No boat"];
                        gearDistributionList.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "InventoryMapping.cs", "GetGearVariationDistribution");
                }
            }
            MapGearDistribution(gearDistributionList, gearName);
            return gearDistributionList;
        }
    }
}