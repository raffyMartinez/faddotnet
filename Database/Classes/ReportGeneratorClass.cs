using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace FAD3.Database.Classes
{
    public static class ReportGeneratorClass
    {
        public static TargetArea TargetArea { get; set; }
        public static string Topic { get; set; }
        public static List<int> Years { get; set; }
        private static string _years;

        public static void Generate()
        {
            _years = "";
            foreach (var year in Years)
            {
                _years += year.ToString() + ",";
            }
            _years = _years.Trim(',');
            var sql = "";
            switch (Topic)
            {
                case "effort":
                    sql = SetEffortSQL();
                    break;

                case "catch":
                    sql = SetCatchSQL();
                    break;
            }
            SetData(sql);
        }

        public static DataSet DataSet { get; internal set; }

        private static void SetData(string sql)
        {
            DataSet = new DataSet();
            var myDT = new DataTable();
            try
            {
                using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.MDBPath))
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(myDT);
                    DataSet.Tables.Add(myDT);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ReportGeneratorClass", "GetEffort");
            }
        }

        private static string SetCatchSQL()
        {
            return $@"SELECT Provinces.ProvinceName AS Province,
                            Municipalities.Municipality,
                            tblLandingSites.LSName AS [Landing site],
                            tblSampling.FishingGround AS [Fishing ground],
                            tblGearClass.GearClassName AS [Gear class],
                            tblGearVariations.Variation,
                            tblSampling.RefNo AS [Reference number],
                            tblSampling.SamplingDate AS [Date sampled],
                            temp_AllNames.Name1, temp_AllNames.Name2,
                            tblSampling.WtCatch AS [Weight of catch],
                            tblSampling.WtSample AS [Weight of sample],
                            tblCatchDetail.wt AS Weight, tblCatchDetail.ct AS [Count],
                            tblCatchDetail.swt AS [Subsample weight],
                            tblCatchDetail.sct AS [Subsample count],
                            tblCatchDetail.FromTotal AS [From total],
                            1.50 AS [Computed weight],
                            1 AS [Computed count]
                        FROM Provinces INNER JOIN
                            (Municipalities INNER JOIN
                            (tblLandingSites INNER JOIN
                            ((tblGearClass INNER JOIN
                            tblGearVariations ON
                            tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN
                            ((temp_AllNames INNER JOIN
                            (tblSampling INNER JOIN
                            tblCatchComp ON
                            tblSampling.SamplingGUID = tblCatchComp.SamplingGUID) ON
                            temp_AllNames.NameNo = tblCatchComp.NameGUID) INNER JOIN
                            tblCatchDetail ON
                            tblCatchComp.RowGUID = tblCatchDetail.CatchCompRow) ON
                            tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON
                            tblLandingSites.LSGUID = tblSampling.LSGUID) ON
                            Municipalities.MunNo = tblLandingSites.MunNo) ON
                            Provinces.ProvNo = Municipalities.ProvNo
                        WHERE tblSampling.AOI={{{TargetArea.TargetAreaGuid}}} AND
                            Year([SamplingDate]) In ({_years})
                        ORDER BY Provinces.ProvinceName,
                            Municipalities.Municipality,
                            tblLandingSites.LSName,
                            tblGearClass.GearClassName,
                            tblGearVariations.Variation,
                            tblSampling.RefNo";
        }

        private static string SetEffortSQL()
        {
            return $@"SELECT tblAOI.AOIName AS [Target area],
                            tblLandingSites.LSName AS [Landing site],
                            Municipalities.Municipality,
                            Provinces.ProvinceName AS Province,
                            tblEnumerators.EnumeratorName AS Enumerator,
                            tblGearClass.GearClassName AS [Gear class],
                            tblGearVariations.Variation,
                            tblSampling.RefNo,
                            tblSampling.SamplingDate AS [Sampling date],
                            tblSampling.SamplingTime AS [Sampling time],
                            tblSampling.FishingGround AS [Fishing ground],
                            tblSampling.WtCatch AS [Weight of catch],
                            tblSampling.WtSample AS [Weight of sample],
                            tblSampling.DateSet AS [Date set],
                            tblSampling.TimeSet AS [Time set],
                            tblSampling.DateHauled AS [Date hauled],
                            tblSampling.TimeHauled AS [Time hauled],
                            tblSampling.NoHauls AS [Number of hauls],
                            tblSampling.NoFishers AS [Number of fishers],
                            temp_VesselType.VesselType AS [Vessel type],
                            tblSampling.Engine,
                            tblSampling.hp
                        FROM (tblAOI INNER JOIN
                            (Provinces INNER JOIN
                            (Municipalities INNER JOIN
                            tblLandingSites ON
                            Municipalities.MunNo = tblLandingSites.MunNo) ON
                            Provinces.ProvNo = Municipalities.ProvNo) ON
                            tblAOI.AOIGuid = tblLandingSites.AOIGuid) INNER JOIN
                            ((tblGearClass INNER JOIN
                            tblGearVariations ON
                            tblGearClass.GearClass = tblGearVariations.GearClass) INNER JOIN
                            (tblEnumerators RIGHT JOIN
                            (tblSampling LEFT JOIN
                            temp_VesselType ON tblSampling.VesType = temp_VesselType.VesselTypeNo) ON
                            tblEnumerators.EnumeratorID = tblSampling.Enumerator) ON
                            tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) ON
                            tblLandingSites.LSGUID = tblSampling.LSGUID
                        WHERE tblSampling.AOI={{{TargetArea.TargetAreaGuid}}} AND
                            Year([SamplingDate]) In ({_years})
                        ORDER BY tblLandingSites.LSName,
                            tblGearClass.GearClassName,
                            tblGearVariations.Variation,
                            tblSampling.SamplingDate";
        }
    }
}