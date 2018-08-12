using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace FAD3
{
    public static class CatchComposition
    {
        private static string _SamplingGUID = "";

        private static double _TotalWtOfFromTotal = 0;

        static CatchComposition()
        {
        }

        public static double TotalWtOfFromTotal
        {
            get { return _TotalWtOfFromTotal; }
        }

        public static bool UpdateCatchComposition()
        {
            return true;
        }

        public static Dictionary<string, CatchLine> CatchComp(string SamplingGUID)
        {
            Dictionary<string, CatchLine> myCatch = new Dictionary<string, CatchLine>();
            DataTable dt = new DataTable();
            string CatchName = "";
            string Name1 = "";
            string Name2 = "";
            long? CatchCount = null;
            int? TaxaNumber = null;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    //string query = $@"SELECT tblSampling.SamplingGUID, tblCatchDetail.CatchCompRow, tblCatchComp.NameGUID, temp_AllNames.Name1,
                    //               temp_AllNames.Name2, tblSampling.WtCatch, tblSampling.WtSample, tblCatchDetail.Live, tblCatchDetail.wt,
                    //               tblCatchDetail.ct, tblCatchDetail.swt, tblCatchDetail.sct, tblCatchDetail.FromTotal
                    //               FROM(tblSampling INNER JOIN(tblCatchComp INNER JOIN temp_AllNames ON tblCatchComp.NameGUID = temp_AllNames.NameNo)
                    //               ON tblSampling.SamplingGUID = tblCatchComp.SamplingGUID) INNER JOIN tblCatchDetail ON tblCatchComp.RowGUID =
                    //               tblCatchDetail.CatchCompRow WHERE tblSampling.SamplingGUID = {{{_SamplingGUID}}} ORDER BY tblCatchComp.Sequence";

                    string query = $@"SELECT tblSampling.SamplingGUID, tblCatchDetail.CatchCompRow, tblCatchComp.NameGUID, temp_AllNames.Name1,
                                    temp_AllNames.Name2, tblSampling.WtCatch, tblSampling.WtSample, tblCatchDetail.Live, tblCatchDetail.wt,
                                    tblCatchDetail.ct, tblCatchDetail.swt, tblCatchDetail.sct, tblCatchDetail.FromTotal, tblAllSpecies.TaxaNo
                                    FROM ((tblSampling INNER JOIN (tblCatchComp INNER JOIN temp_AllNames ON tblCatchComp.NameGUID = temp_AllNames.NameNo)
                                    ON tblSampling.SamplingGUID = tblCatchComp.SamplingGUID) INNER JOIN tblCatchDetail ON
                                    tblCatchComp.RowGUID = tblCatchDetail.CatchCompRow) LEFT JOIN tblAllSpecies ON temp_AllNames.NameNo = tblAllSpecies.SpeciesGUID
                                    WHERE tblSampling.SamplingGUID={{{SamplingGUID}}} ORDER BY tblCatchComp.Sequence";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    _TotalWtOfFromTotal = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TaxaNumber = null;
                        CatchCount = null;
                        DataRow dr = dt.Rows[i];
                        Name1 = dr["Name1"].ToString();
                        Name2 = dr["Name2"].ToString();
                        CatchName = $"{Name1} {Name2}";

                        if (dr["ct"].ToString().Length > 0)
                        {
                            if (int.TryParse(dr["ct"].ToString(), out int myCount))
                            {
                                CatchCount = myCount;
                            }
                            else
                            {
                                CatchCount = null;
                            }
                        }

                        if (dr["TaxaNo"].ToString().Length > 0)
                        {
                            if (int.TryParse(dr["TaxaNo"].ToString(), out int myTaxaNo))
                            {
                                TaxaNumber = myTaxaNo;
                            }
                            else
                            {
                                TaxaNumber = null;
                            }
                        }

                        CatchLine myLine = new CatchLine(Name1, Name2, CatchName, dr["SamplingGUID"].ToString(),
                                        dr["CatchCompRow"].ToString(), dr["NameGUID"].ToString(),
                                        Convert.ToDouble(dr["wt"]), CatchCount, TaxaNumber);

                        myLine.CatchSubsampleWt = null;
                        myLine.CatchSubsampleCount = null;

                        if (dr["swt"] != DBNull.Value)
                        {
                            myLine.CatchSubsampleWt = Convert.ToDouble(dr["swt"]);
                        }

                        if (dr["sct"] != DBNull.Value)
                        {
                            myLine.CatchSubsampleCount = Convert.ToInt32(dr["sct"]);
                        }

                        myLine.FromTotalCatch = bool.Parse(dr["FromTotal"].ToString());
                        myCatch.Add(dr["CatchCompRow"].ToString(), myLine);

                        if (dr["FromTotal"].ToString() == "True")
                        {
                            _TotalWtOfFromTotal += Convert.ToDouble(dr["wt"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            return myCatch;
        }

        public static Dictionary<string, string> CatchCompRow(string RowGUID)
        {
            Dictionary<string, string> myRow = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $@"SELECT tblCatchComp.NameGUID, tblCatchComp.RowGUID, temp_AllNames.Name1, temp_AllNames.Name2,
                         tblSampling.WtCatch, tblSampling.WtSample, tblCatchDetail.Live, tblCatchDetail.wt, tblCatchDetail.ct,
                         tblCatchDetail.swt, tblCatchDetail.RowGUID as DetailRow, tblCatchDetail.sct, tblCatchDetail.FromTotal, tblCatchComp.NameType, tblTaxa.TaxaNo, tblTaxa.Taxa
                         FROM((tblSampling INNER JOIN(tblCatchComp INNER JOIN temp_AllNames ON tblCatchComp.NameGUID = temp_AllNames.NameNo)
                         ON tblSampling.SamplingGUID = tblCatchComp.SamplingGUID) INNER JOIN tblCatchDetail ON
                         tblCatchComp.RowGUID = tblCatchDetail.CatchCompRow) LEFT JOIN tblTaxa ON tblCatchComp.Taxa = tblTaxa.TaxaNo
                         WHERE tblCatchDetail.CatchCompRow ={{{RowGUID}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myRow.Add("DetailRow", dr["DetailRow"].ToString());
                        myRow.Add("RowGUID", dr["RowGUID"].ToString());
                        myRow.Add("NameGUID", dr["NameGUID"].ToString());
                        myRow.Add("NameType", dr["NameType"].ToString());
                        myRow.Add("Name1", dr["Name1"].ToString());
                        myRow.Add("Name2", dr["Name2"].ToString());
                        myRow.Add("WtCatch", dr["WtCatch"].ToString());
                        myRow.Add("WtSample", dr["WtSample"].ToString());
                        myRow.Add("wt", dr["wt"].ToString());
                        myRow.Add("ct", dr["ct"].ToString());
                        myRow.Add("swt", dr["swt"].ToString());
                        myRow.Add("sct", dr["sct"].ToString());
                        myRow.Add("FromTotal", dr["FromTotal"].ToString());
                        myRow.Add("Live", dr["Live"].ToString());
                        myRow.Add("TaxaNo", dr["TaxaNo"].ToString());
                        myRow.Add("Taxa", dr["Taxa"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return myRow;
        }

        public struct CatchLine
        {
            private readonly string _CatchLineGUID;

            private readonly string _CatchName;

            private readonly string _Name1;
            private readonly string _Name2;

            private readonly string _CatchNameGIUD;

            private readonly double _CatchWeight;

            private readonly string _SamplingGUID;

            private long? _CatchCount;

            private string _CatchDetailRowGUID;

            private long? _CatchSubsampleCount;

            private double? _CatchSubsampleWt;

            private bool _FromTotalCatch;

            private bool _LiveFish;

            private Identification _NameType;

            private int? _TaxaNumber;

            public CatchLine(string Name1, string Name2, string CatchName, string SamplingGUID,
                                          string CatchLineGUID, string CatchNameGUID,
                                          double CatchWeight, long? CatchCount = null,
                                          int? TaxaNumber = null)
            {
                _Name1 = Name1;
                _Name2 = Name2;
                _CatchName = CatchName;
                _SamplingGUID = SamplingGUID;
                _CatchLineGUID = CatchLineGUID;
                _CatchNameGIUD = CatchNameGUID;
                _CatchWeight = CatchWeight;
                _CatchCount = CatchCount;
                _CatchSubsampleWt = 0;
                _CatchSubsampleCount = 0;
                _FromTotalCatch = false;
                _NameType = Identification.Scientific;
                _LiveFish = false;
                _CatchDetailRowGUID = "";
                _TaxaNumber = TaxaNumber;
            }

            /// <summary>
            /// A structure representing an individual catch which includes catch wt, count, subsample wt and count,
            /// the guid of the parent sampling (SamplingGUID) and the guid of the individual catch.
            /// </summary>
            public CatchLine(string Name1, string Name2, string CatchName, string SamplingGUID,
                                          string CatchLineGUID, string CatchNameGUID,
                                          double CatchWeight, int? TaxaNumber = null)
            {
                _Name1 = Name1;
                _Name2 = Name2;
                _CatchName = CatchName;
                _SamplingGUID = SamplingGUID;
                _CatchLineGUID = CatchLineGUID;
                _CatchNameGIUD = CatchNameGUID;
                _CatchWeight = CatchWeight;
                _CatchCount = null;
                _CatchSubsampleWt = 0;
                _CatchSubsampleCount = 0;
                _FromTotalCatch = false;
                _NameType = Identification.Scientific;
                _LiveFish = false;
                _CatchDetailRowGUID = "";
                _TaxaNumber = TaxaNumber;
            }

            public long? CatchCount
            {
                set { _CatchCount = value; }
                get { return _CatchCount; }
            }

            public string CatchDetailRowGUID
            {
                get { return _CatchDetailRowGUID; }
                set { _CatchDetailRowGUID = value; }
            }

            public string CatchLineGUID
            {
                get { return _CatchLineGUID; }
            }

            public string CatchName
            {
                get { return _CatchName; }
            }

            public string Name1
            {
                get { return _Name1; }
            }

            public string Name2
            {
                get { return _Name2; }
            }

            public string CatchNameGUID
            {
                get { return _CatchNameGIUD; }
            }

            public long? CatchSubsampleCount
            {
                get { return _CatchSubsampleCount; }
                set { _CatchSubsampleCount = value; }
            }

            public int? TaxaNumber
            {
                get { return _TaxaNumber; }
                set { _TaxaNumber = value; }
            }

            public double? CatchSubsampleWt
            {
                get { return _CatchSubsampleWt; }
                set { _CatchSubsampleWt = value; }
            }

            public double CatchWeight
            {
                get { return _CatchWeight; }
            }

            public bool FromTotalCatch
            {
                get { return _FromTotalCatch; }
                set { _FromTotalCatch = value; }
            }

            public bool LiveFish
            {
                get { return _LiveFish; }
                set { _LiveFish = value; }
            }

            public Identification NameType
            {
                get { return _NameType; }
                set { _NameType = value; }
            }

            public string SamplingGUID
            {
                get { return _SamplingGUID; }
            }

            public bool Save(bool isNew)
            {
                //string RowGUID = "";
                bool Success = false;
                string updateQuery = "";
                using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
                {
                    try
                    {
                        if (isNew)
                        {
                            //RowGUID = Guid.NewGuid().ToString();
                            updateQuery = $@"Insert into tblCatchComp (NameGUID, NameType,RowGUID,SamplingGUID) values (
                                          {{{ _CatchNameGIUD}}}, {Convert.ToInt16(_NameType)}, {{{_CatchLineGUID}}}, {{{_SamplingGUID}}})";
                        }
                        else
                        {
                            if (SaveCatchDetail(isNew, "", _CatchDetailRowGUID))
                            {
                                updateQuery = $@"Update tblCatchComp set
                                    NameGUID = {{{_CatchNameGIUD}}},
                                    NameType = {Convert.ToInt16(_NameType)}
                                    WHERE RowGUID = {{{_CatchLineGUID}}}";
                            }
                        }
                        OleDbCommand update = new OleDbCommand(updateQuery, conn);
                        conn.Open();
                        Success = (update.ExecuteNonQuery() > 0);
                        conn.Close();
                        if (Success)
                        {
                            if (isNew)
                            {
                                Success = SaveCatchDetail(isNew, _CatchLineGUID);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex);
                    }
                }
                return Success;
            }

            private bool SaveCatchDetail(bool isNew, string CatchCompRow = "", string CatchDetailRowGUID = "")
            {
                bool Success = false;
                string updateQuery = "";
                string RowGUID = "";
                using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
                {
                    try
                    {
                        if (isNew)
                        {
                            RowGUID = Guid.NewGuid().ToString();

                            if (_CatchCount == null)
                            {
                                updateQuery = $@"Insert into tblCatchDetail (wt,swt,sct,CatchCompRow,RowGUID,FromTotal,Live) values (
                                               {_CatchWeight}, {_CatchSubsampleWt}, {_CatchSubsampleCount}, {{{CatchCompRow}}}, {{{RowGUID}}},
                                               {_FromTotalCatch}, {_LiveFish})";
                            }
                            else
                            {
                                updateQuery = $@"Insert into tblCatchDetail (wt,ct,swt,sct,CatchCompRow,RowGUID,FromTotal,Live) values (
                                               {_CatchWeight}, {(long)_CatchCount},  {_CatchSubsampleWt}, {_CatchSubsampleCount},
                                               {{{CatchCompRow}}}, {{{RowGUID}}}, {_FromTotalCatch}, {_LiveFish})";
                            }
                        }
                        else
                        {
                            if (_CatchCount == null)
                            {
                                updateQuery = $@"Update tblCatchDetail set
                                           wt = {CatchWeight}, swt= {_CatchSubsampleWt},
                                           sct = {_CatchSubsampleCount}, FromTotal = {_FromTotalCatch},
                                           Live = {_LiveFish} Where RowGUID = {{{CatchDetailRowGUID}}}";
                            }
                            else
                            {
                                updateQuery = $@"Update tblCatchDetail set
                                           wt = {_CatchWeight},
                                           ct = {_CatchCount},
                                           swt = {_CatchSubsampleWt},
                                           sct = {_CatchSubsampleCount},
                                           FromTotal = {_FromTotalCatch},
                                           Live = {_LiveFish}
                                           Where RowGUID = {{{CatchDetailRowGUID}}}";
                            }
                        }
                        OleDbCommand update = new OleDbCommand(updateQuery, conn);
                        conn.Open();
                        Success = (update.ExecuteNonQuery() > 0);
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex);
                    }
                }
                return Success;
            }
        }

        public enum Identification
        {
            Scientific = 1,
            LocalName
        }

        public static string IdentificationTypeToString(Identification IDType)
        {
            var myID = "";
            switch (IDType)
            {
                case Identification.LocalName:
                    myID = "Local name";
                    break;

                case Identification.Scientific:
                    myID = "Scientific name";
                    break;
            }
            return myID;
        }

        public static Dictionary<CatchComposition.Identification, string> IdentificationTypes()
        {
            var items = new Dictionary<CatchComposition.Identification, string>();

            items.Add(Identification.Scientific, "Scientific name");
            items.Add(Identification.LocalName, "Local name");
            return items;
        }
    }
}