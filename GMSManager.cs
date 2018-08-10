using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace FAD3
{
    public static class GMSManager
    {
        public enum Taxa
        {
            To_be_determined,
            Fish,
            Shrimps,
            Cephalopods,
            Crabs,
            Shells,
            Lobsters,
            Sea_cucumbers,
            Sea_urchins,
        }

        public enum FishCrabGMS
        {
            AllTaxaNotDetermined,
            FishJuvenile = 1,
            FishStg1Immature,
            FishStg2Maturing,
            FishStg3Mature,
            FishStg4Gravid,
            FishStg5Spent,
            FemaleCrabImmature = 2,
            FemaleCrabMature = 4,
            FemaleCrabBerried,
        }

        public enum sex
        {
            Juvenile,
            Male,
            Female
        }

        public struct GMSLine
        {
            public string CatchRowGUID { get; set; }
            public GMSManager.FishCrabGMS GMS { get; set; }
            public double? GonadWeight { get; set; }
            public double? Length { get; set; }
            public GMSManager.sex Sex { get; set; }
            public GMSManager.Taxa Taxa { get; set; }
            public string TaxaName { get; set; }
            public double? Weight { get; set; }
            public global.fad3DataStatus DataStatus { get; set; }
        }

        public static Taxa TaxaFromCatchNameGUID(string CatchNameGUID)
        {
            Taxa taxa = Taxa.To_be_determined;
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = $"Select TaxaNo from tblAllSpecies where SpeciesGUID = {{{CatchNameGUID}}}";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    DataRow dr = dt.Rows[0];
                    taxa = (Taxa)int.Parse(dr["TaxaNo"].ToString());
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
                return taxa;
            }
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

        public static FishCrabGMS MaturityStageFromText(string stage, Taxa taxa)
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

        public static Dictionary<string, GMSLine> GMSData(string CatchCompRowNo)
        {
            Dictionary<string, GMSLine> mydata = new Dictionary<string, GMSLine>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
            {
                try
                {
                    conection.Open();

                    string query = $@"SELECT tblGMS.RowGUID, tblGMS.Gonadwt, tblGMS.GMS, tblGMS.Sex, tblGMS.Wt, tblGMS.Len,
                                   tblGMS.CatchCompRow, tblTaxa.TaxaNo, tblTaxa.Taxa FROM tblTaxa INNER JOIN
                                   (tblAllSpecies INNER JOIN(tblCatchComp INNER JOIN tblGMS ON
                                   tblCatchComp.RowGUID = tblGMS.CatchCompRow) ON
                                   tblAllSpecies.SpeciesGUID = tblCatchComp.NameGUID)
                                   ON tblTaxa.TaxaNo = tblAllSpecies.TaxaNo
                                   WHERE tblGMS.CatchCompRow ={{{CatchCompRowNo}}}";

                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];

                        GMSManager.FishCrabGMS gms;
                        Enum.TryParse(dr["GMS"].ToString(), out gms);
                        GMSManager.sex sex;
                        Enum.TryParse(dr["Sex"].ToString(), out sex);
                        GMSManager.Taxa taxa = Taxa.To_be_determined;
                        Enum.TryParse(dr["TaxaNo"].ToString(), out taxa);
                        var myGMS = new GMSLine
                        {
                            TaxaName = dr["Taxa"].ToString(),
                            Taxa = taxa,
                            Sex = sex,
                            GMS = gms,
                            DataStatus = global.fad3DataStatus.statusFromDB
                        };
                        if (dr["Len"].ToString().Length > 0)
                            myGMS.Length = double.Parse(dr["Len"].ToString());
                        if (dr["Wt"].ToString().Length > 0)
                            myGMS.Weight = double.Parse(dr["GonadWt"].ToString());
                        if (dr["GonadWt"].ToString().Length > 0)
                            myGMS.GonadWeight = double.Parse(dr["GonadWt"].ToString());

                        mydata.Add(dr["RowGUID"].ToString(), myGMS);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return mydata;
        }

        public static bool UpdateGMS(bool isNew, Double? wt, double? len, GMSManager.sex sex,
                                                    GMSManager.FishCrabGMS stage, double? gonadwt,
                                                    string CatchCompRow, string RowGUID)
        {
            bool Success = false;
            string query = "";

            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    string slen = len == null ? "null" : len.ToString();
                    string swt = wt == null ? "null" : wt.ToString();
                    string sgonadwt = gonadwt == null ? "null" : gonadwt.ToString();

                    if (isNew)
                    {
                        query = $@"Insert into tblGMS (Len, Wt, Sex, GMS, CatchCompRow, RowGUID, Gonadwt) values
                                 ({slen} , {swt} , {(int)sex} , {(int)stage} , {{{CatchCompRow}}}, {{{RowGUID}}}, {sgonadwt})";
                    }
                    else
                    {
                        query = $@"Update tblGMS set Len= {slen},
                                                  Wt= {swt},
                                                  Sex= {(int)sex},
                                                  GMS= {(int)stage},
                                                  GonadWt= {sgonadwt}
                                                  Where RowGUID = {{{RowGUID}}}";
                    }
                    OleDbCommand update = new OleDbCommand(query, conn);
                    conn.Open();
                    Success = (update.ExecuteNonQuery() > 0);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return Success;
        }

        public static bool DeleteGMSLine(string RowGUID)
        {
            bool Success = false;
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    string query = $"Delete * from tblGMS where RowGUID = {{{RowGUID}}}";
                    OleDbCommand update = new OleDbCommand(query, conn);
                    conn.Open();
                    Success = (update.ExecuteNonQuery() > 0);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return Success;
        }
    }
}