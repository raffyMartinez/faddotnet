using FAD3.Database.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace FAD3
{
    public static class GMSManager
    {
        private static int _GMSMeasurementRows;

        public static int GMSMeasurementRows
        {
            get { return _GMSMeasurementRows; }
        }

        public static Dictionary<FishCrabGMS, string> GMSStages(Taxa taxa, ref bool Success)
        {
            Success = false;
            Dictionary<FishCrabGMS, string> myStages = new Dictionary<FishCrabGMS, string>();
            switch (taxa)
            {
                case Taxa.Fish:
                    myStages.Add(FishCrabGMS.AllTaxaNotDetermined, "Not determined");
                    myStages.Add(FishCrabGMS.FishJuvenile, "Juvenile");
                    myStages.Add(FishCrabGMS.FishStg1Immature, "Immature");
                    myStages.Add(FishCrabGMS.FishStg2Maturing, "Maturing");
                    myStages.Add(FishCrabGMS.FishStg3Mature, "Mature");
                    myStages.Add(FishCrabGMS.FishStg4Gravid, "Gravid");
                    myStages.Add(FishCrabGMS.FishStg5Spent, "Spent");
                    Success = true;
                    break;

                case Taxa.Crabs:
                    myStages.Add(FishCrabGMS.AllTaxaNotDetermined, "Not determined");
                    myStages.Add(FishCrabGMS.FemaleCrabImmature, "Immature");
                    myStages.Add(FishCrabGMS.FemaleCrabMature, "Mature");
                    myStages.Add(FishCrabGMS.FemaleCrabBerried, "Berried");
                    Success = true;
                    break;

                case Taxa.Lobsters:
                case Taxa.Sea_cucumbers:
                case Taxa.Sea_urchins:
                case Taxa.Shells:
                case Taxa.Shrimps:
                case Taxa.To_be_determined:
                    break;
            }
            return myStages;
        }

        public static FishCrabGMS GMSStageFromString(string stage, Taxa taxa)
        {
            FishCrabGMS myStage = FishCrabGMS.AllTaxaNotDetermined;
            switch (taxa)
            {
                case Taxa.Fish:
                    switch (stage)
                    {
                        case "Not determined":
                            myStage = FishCrabGMS.AllTaxaNotDetermined;
                            break;

                        case "Juvenile":
                            myStage = FishCrabGMS.FishJuvenile;
                            break;

                        case "Immature":
                            myStage = FishCrabGMS.FishStg1Immature;
                            break;

                        case "Maturing":
                            myStage = FishCrabGMS.FishStg2Maturing;
                            break;

                        case "Mature":
                            myStage = FishCrabGMS.FishStg3Mature;
                            break;

                        case "Gravid":
                            myStage = FishCrabGMS.FishStg4Gravid;
                            break;

                        case "Spent":
                            myStage = FishCrabGMS.FishStg5Spent;
                            break;
                    }
                    break;

                case Taxa.Crabs:
                    switch (stage)
                    {
                        case "Not determined":
                            myStage = FishCrabGMS.AllTaxaNotDetermined;
                            break;

                        case "Immature":
                            myStage = FishCrabGMS.FemaleCrabImmature;
                            break;

                        case "Mature":
                            myStage = FishCrabGMS.FemaleCrabMature;
                            break;

                        case "Berried":
                            myStage = FishCrabGMS.FemaleCrabBerried;
                            break;
                    }
                    break;

                default:
                    myStage = FishCrabGMS.AllTaxaNotDetermined;
                    break;
            }

            return myStage;
        }

        public static string GMSStageToString(Taxa taxa, FishCrabGMS stage)
        {
            string gms_stage = "";
            switch (taxa.ToString())
            {
                case "To_be_determined":
                    break;

                case "Fish":
                    switch (stage)
                    {
                        case FishCrabGMS.AllTaxaNotDetermined:
                            gms_stage = "Not determined";
                            break;

                        case FishCrabGMS.FishJuvenile:
                            gms_stage = "Juvenile";
                            break;

                        case FishCrabGMS.FishStg1Immature:
                            gms_stage = "Immature";
                            break;

                        case FishCrabGMS.FishStg2Maturing:
                            gms_stage = "Maturing";
                            break;

                        case FishCrabGMS.FishStg3Mature:
                            gms_stage = "Mature";
                            break;

                        case FishCrabGMS.FishStg4Gravid:
                            gms_stage = "Gravid";
                            break;

                        case FishCrabGMS.FishStg5Spent:
                            gms_stage = "Spent";
                            break;
                    }
                    break;

                case "Shrimps":
                    break;

                case "Cephalopods":
                    break;

                case "Crabs":
                    switch (stage)
                    {
                        case FishCrabGMS.AllTaxaNotDetermined:
                            gms_stage = "Not determined";
                            break;

                        case FishCrabGMS.FemaleCrabImmature:
                            gms_stage = "Immature";
                            break;

                        case FishCrabGMS.FemaleCrabMature:
                            gms_stage = "Mature";
                            break;

                        case FishCrabGMS.FemaleCrabBerried:
                            gms_stage = "Berried";
                            break;
                    }
                    break;

                case "Shells":
                    break;

                case "Lobsters":
                    break;

                case "Sea_cucumbers":
                    break;

                case "Sea_urchins":
                    break;
            }
            return gms_stage;
        }

        public static Dictionary<string, GMSLineClass> RetrieveGMSData(string CatchCompRowNo)
        {
            _GMSMeasurementRows = 0;
            Dictionary<string, GMSLineClass> mydata = new Dictionary<string, GMSLineClass>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.MDBPath))
            {
                try
                {
                    conection.Open();

                    string query = $@"SELECT tblGMS.RowGUID, tblGMS.Gonadwt, tblGMS.GMS, tblGMS.Sex, tblGMS.Wt, tblGMS.Len,
                                   tblGMS.CatchCompRow, tblGMS.Sequence, tblTaxa.TaxaNo, tblTaxa.Taxa FROM tblTaxa INNER JOIN
                                   (tblAllSpecies INNER JOIN(tblCatchComp INNER JOIN tblGMS ON
                                   tblCatchComp.RowGUID = tblGMS.CatchCompRow) ON
                                   tblAllSpecies.SpeciesGUID = tblCatchComp.NameGUID)
                                   ON tblTaxa.TaxaNo = tblAllSpecies.TaxaNo
                                   WHERE tblGMS.CatchCompRow ={{{CatchCompRowNo}}} ORDER By tblGMS.Sequence";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];

                        FishCrabGMS gms;
                        Enum.TryParse(dr["GMS"].ToString(), out gms);
                        Sex sex;
                        Enum.TryParse(dr["Sex"].ToString(), out sex);
                        Taxa taxa = Taxa.To_be_determined;
                        Enum.TryParse(dr["TaxaNo"].ToString(), out taxa);
                        double? Length = null;
                        double? Weight = null;
                        double? GonadWeight = null;
                        int Sequence = 0;

                        if (dr["Len"].ToString().Length > 0)
                            Length = double.Parse(dr["Len"].ToString());
                        if (dr["Wt"].ToString().Length > 0)
                            Weight = double.Parse(dr["GonadWt"].ToString());
                        if (dr["GonadWt"].ToString().Length > 0)
                            GonadWeight = double.Parse(dr["GonadWt"].ToString());
                        if (dr["Sequence"].ToString().Length > 0)
                            Sequence = int.Parse(dr["Sequence"].ToString());

                        GMSLineClass myGMS = new GMSLineClass(dr["CatchCompRow"].ToString(), dr["RowGUID"].ToString(),
                                    Length, Weight, GonadWeight, sex, gms, dr["Taxa"].ToString(), taxa,
                                    fad3DataStatus.statusFromDB, Sequence);

                        mydata.Add(dr["RowGUID"].ToString(), myGMS);
                        _GMSMeasurementRows++;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                }
            }
            return mydata;
        }

        public static bool UpdateGMSData(Dictionary<string, GMSLineClass> gmsRows)
        {
            var rows = gmsRows.Count;
            var sql = "";
            var n = 0;
            if (rows > 0)
            {
                using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
                {
                    conn.Open();
                    foreach (var item in gmsRows)
                    {
                        var Weight = item.Value.Weight == null ? "null" : item.Value.Weight.ToString();
                        var GonadWeight = item.Value.GonadWeight == null ? "null" : item.Value.GonadWeight.ToString();
                        var Length = item.Value.Length == null ? "null" : item.Value.Length.ToString();

                        switch (item.Value.DataStatus)
                        {
                            case fad3DataStatus.statusEdited:
                                sql = $@"Update tblGMS set
                                 Len = {Length},
                                 Wt = {Weight},
                                 Sex = {(int)item.Value.Sex},
                                 GMS = {(int)item.Value.GMS},
                                 Gonadwt = {GonadWeight}
                                 WHERE RowGuid = {{{item.Value.RowGuid}}}";

                                using (OleDbCommand update = new OleDbCommand(sql, conn))
                                {
                                    if (update.ExecuteNonQuery() > 0) n++;
                                }

                                break;

                            case fad3DataStatus.statusNew:
                                sql = $@"Insert into tblGMS (Len,Wt,Sex,GMS,GonadWt,CatchCompRow,RowGUID,Sequence) values (
                                 {Length}, {Weight}, {(int)item.Value.Sex}, {(int)item.Value.GMS},
                                 {GonadWeight}, {{{item.Value.CatchRowGUID}}}, {{{item.Value.RowGuid}}},
                                 {item.Value.Sequence})";

                                using (OleDbCommand update = new OleDbCommand(sql, conn))
                                {
                                    if (update.ExecuteNonQuery() > 0) n++;
                                }

                                break;

                            case fad3DataStatus.statusForDeletion:
                                sql = $"Delete * from tblGMS where RowGUID = {{{item.Value.RowGuid}}}";

                                using (OleDbCommand update = new OleDbCommand(sql, conn))
                                {
                                    if (update.ExecuteNonQuery() > 0) n++;
                                }

                                break;
                        }
                    }
                }
            }
            return n > 0;
        }
    }
}