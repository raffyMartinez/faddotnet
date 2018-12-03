using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Text;
using FAD3.GUI.Classes;

namespace FAD3
{
    public static class CatchComposition
    {
        private static int _CatchCompositionRows;

        private static double _TotalWtOfFromTotal = 0;

        public static int CatchCompositionRows
        {
            get { return _CatchCompositionRows; }
        }

        public static double TotalWtOfFromTotal
        {
            get { return _TotalWtOfFromTotal; }
        }

        public static bool UpdateCatchComposition(Dictionary<string, CatchLine> CatchComposition)

        {
            var sql = "";
            var resultCount = 0;
            var InsertCount = 0;
            var UpdateCount = 0;
            var DeleteCount = 0;
            using (var conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                foreach (var item in CatchComposition)
                {
                    var CatchCount = item.Value.CatchCount == null ? "null" : item.Value.CatchCount.ToString();
                    var SubWeight = item.Value.CatchSubsampleWt == null ? "null" : item.Value.CatchSubsampleWt.ToString();
                    var SubCount = item.Value.CatchSubsampleCount == null ? "null" : item.Value.CatchSubsampleCount.ToString();

                    if (item.Value.dataStatus == fad3DataStatus.statusNew)
                    {
                        InsertCount++;
                        sql = $@"Insert into tblCatchComp (NameGUID, NameType, RowGUID, SamplingGUID, Sequence) values (
                          {{{item.Value.CatchNameGUID}}}, {(int)item.Value.NameType}, {{{item.Value.CatchCompGUID}}},
                          {{{item.Value.SamplingGUID}}}, {item.Value.Sequence})";

                        OleDbCommand update = new OleDbCommand(sql, conn);
                        if (update.ExecuteNonQuery() > 0) resultCount++;

                        sql = $@"Insert into tblCatchDetail (wt,ct,swt,sct,CatchCompRow,RowGUID,FromTotal,Live) values (
                            {item.Value.CatchWeight}, {CatchCount}, {SubWeight}, {SubCount},
                            {{{item.Value.CatchCompGUID}}}, {{{Guid.NewGuid().ToString()}}},{item.Value.FromTotalCatch}, {item.Value.LiveFish})";

                        update = new OleDbCommand(sql, conn);
                        if (update.ExecuteNonQuery() > 0) resultCount++;
                    }
                    else if (item.Value.dataStatus == fad3DataStatus.statusEdited)
                    {
                        UpdateCount++;
                        sql = $@"Update tblCatchDetail set
                            wt = {item.Value.CatchWeight},
                            ct = {CatchCount},
                            swt = {SubWeight},
                            sct = {SubCount},
                            FromTotal = {item.Value.FromTotalCatch},
                            Live = {item.Value.LiveFish}
                            WHERE RowGUID = {{{item.Value.CatchDetailRowGUID}}}";

                        OleDbCommand update = new OleDbCommand(sql, conn);
                        if (update.ExecuteNonQuery() > 0) resultCount++;

                        sql = $@"Update tblCatchComp set
                            NameGUID = {{{item.Value.CatchNameGUID}}},
                            NameType = {(int)item.Value.NameType}
                            WHERE RowGUID = {{{item.Value.CatchCompGUID}}}";

                        update = new OleDbCommand(sql, conn);
                        if (update.ExecuteNonQuery() > 0) resultCount++;
                    }
                    else if (item.Value.dataStatus == fad3DataStatus.statusForDeletion)
                    {
                        DeleteCount++;
                        sql = $"Delete * from tblCatchDetail WHERE CatchCompRow = {{{item.Value.CatchCompGUID}}}";
                        sql = $"Delete * from tbkCatchComp WHERE RowGUID = {{{item.Value.SamplingGUID}}}";
                    }
                }
            }
            if ((InsertCount + UpdateCount) > 0)
                return resultCount > 0;
            else
                return resultCount == (InsertCount + UpdateCount);
        }

        public static Dictionary<string, CatchLine> RetrieveCatchComposition(string SamplingGUID)
        {
            _CatchCompositionRows = 0;
            Dictionary<string, CatchLine> myCatch = new Dictionary<string, CatchLine>();
            DataTable dt = new DataTable();
            string CatchName = "";
            string Name1 = "";
            string Name2 = "";
            int? CatchCount = null;
            int? TaxaNumber = null;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();

                    var query = $@"SELECT tblCatchComp.SamplingGUID, tblCatchComp.RowGUID, tblCatchComp.NameGUID, temp_AllNames.Name1, temp_AllNames.Name2, tblAllSpecies.TaxaNo,
                                temp_AllNames.Identification, tblCatchComp.Sequence, tblCatchComp.LFInterval, tblCatchDetail.RowGUID AS CatchDetailRow, tblCatchDetail.wt, tblCatchDetail.ct,
                                tblCatchDetail.swt, tblCatchDetail.sct, tblCatchDetail.FromTotal, tblCatchDetail.Live, tblCatchDetail.Notes FROM tblAllSpecies RIGHT JOIN
                                (temp_AllNames INNER JOIN (tblCatchComp INNER JOIN tblCatchDetail ON tblCatchComp.RowGUID = tblCatchDetail.CatchCompRow)
                                ON temp_AllNames.NameNo = tblCatchComp.NameGUID) ON tblAllSpecies.SpeciesGUID = tblCatchComp.NameGUID WHERE tblCatchComp.SamplingGUID={{{SamplingGUID}}}
                                ORDER BY tblCatchComp.Sequence";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    _TotalWtOfFromTotal = 0;
                    Identification IdType = Identification.Scientific;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        Name1 = dr["Name1"].ToString();
                        Name2 = dr["Name2"].ToString();
                        CatchName = $"{Name1} {Name2}";
                        IdType = Identification.Scientific;
                        if (dr["Identification"].ToString() == "Local names") IdType = Identification.LocalName;

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

                        //defines a catch line
                        var Sequence = 0;
                        if (int.TryParse(dr["Sequence"].ToString(), out int v))
                            Sequence = v;

                        CatchLine myLine = new CatchLine(Sequence, Name1, Name2, CatchName, dr["SamplingGUID"].ToString(),
                                        dr["RowGUID"].ToString(), dr["NameGUID"].ToString(),
                                        Convert.ToDouble(dr["wt"]), CatchCount, TaxaNumber)
                        {
                            CatchDetailRowGUID = dr["CatchDetailRow"].ToString(),
                            CatchSubsampleWt = null,
                            CatchSubsampleCount = null,
                            NameType = IdType,
                            dataStatus = fad3DataStatus.statusFromDB
                        };

                        if (dr["swt"] != DBNull.Value)
                        {
                            myLine.CatchSubsampleWt = Convert.ToDouble(dr["swt"]);
                        }

                        if (dr["sct"] != DBNull.Value)
                        {
                            myLine.CatchSubsampleCount = Convert.ToInt32(dr["sct"]);
                        }

                        myLine.FromTotalCatch = bool.Parse(dr["FromTotal"].ToString());

                        //add the catch composition row to the dictionary
                        myCatch.Add(dr["RowGUID"].ToString(), myLine);

                        if (dr["FromTotal"].ToString() == "True")
                        {
                            _TotalWtOfFromTotal += Convert.ToDouble(dr["wt"].ToString());
                        }
                        _CatchCompositionRows++;
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

        public static Identification StringToIdentificationType(string IDType)
        {
            var myID = Identification.Scientific;
            switch (IDType)
            {
                case "Local name":
                    myID = Identification.LocalName;
                    break;

                case "Scientific name":
                    myID = Identification.Scientific;
                    break;

                default:
                    myID = Identification.Scientific;
                    break;
            }
            return myID;
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

        public static Dictionary<Identification, string> IdentificationTypes()
        {
            var items = new Dictionary<Identification, string>();

            items.Add(Identification.Scientific, "Scientific name");
            items.Add(Identification.LocalName, "Local name");
            return items;
        }
    }
}