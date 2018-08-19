using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace FAD3
{
    public class CatchName
    {
        private Taxa _taxa;
        private string _taxaName;
        private int _catchCompositionRecordCount;

        public string genus { get; set; }
        public string species { get; set; }

        public string taxaName
        {
            get { return _taxaName; }
        }

        public Taxa taxa
        {
            get { return _taxa; }

            set
            {
                _taxa = value;
                _taxaName = TaxaNameFromTaxa(_taxa);
            }
        }

        public bool inFishBase { get; set; }
        public int? fishBaseSpeciesNumber { get; set; }
        public string Notes { get; set; }
        public short genusMetaPhoneKey1 { get; set; }
        public short genusMetaPhoneKey2 { get; set; }
        public short speciesMetaPhoneKey1 { get; set; }
        public short speciesMetaPhoneKey2 { get; set; }
        public string catchNameGuid { get; set; }
        public int catchCompositionRecordCount { get { return _catchCompositionRecordCount; } }

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
                    Logger.Log(ex);
                }
                return taxa;
            }
        }

        public static Dictionary<int, string> RetrieveTaxaDictionary()
        {
            var taxaDictionary = new Dictionary<int, string>();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
            {
                conection.Open();

                const string query = "Select TaxaNo, Taxa from tblTaxa order by Taxa";

                var adapter = new OleDbDataAdapter(query, conection);
                var dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    taxaDictionary.Add(dr.Field<int>("TaxaNo"), dr.Field<string>("Taxa"));
                }
            }

            return taxaDictionary;
        }

        public static Taxa TaxaFromTaxaName(string taxaName)
        {
            var taxa = Taxa.To_be_determined;
            switch (taxaName)
            {
                case "To be determined":
                    taxa = Taxa.To_be_determined;
                    break;

                case "Cephalopods":
                case "Cephalopods (squids etc)":
                    taxa = Taxa.Cephalopods;
                    break;

                case "Crabs":
                    taxa = Taxa.Crabs;
                    break;

                case "Fish":
                    taxa = Taxa.Fish;
                    break;

                case "Lobsters":
                    taxa = Taxa.Lobsters;
                    break;

                case "Sea cucumbers":
                    taxa = Taxa.Sea_cucumbers;
                    break;

                case "Sea urchins":
                    taxa = Taxa.Sea_urchins;
                    break;

                case "Shells":
                    taxa = Taxa.Shells;
                    break;

                case "Shrimps":
                    taxa = Taxa.Shrimps;
                    break;

                default:
                    taxa = Taxa.To_be_determined;
                    break;
            }
            return taxa;
        }

        public static string TaxaNameFromTaxa(Taxa taxa)
        {
            var taxaName = "";
            switch (taxa)
            {
                case Taxa.To_be_determined:
                    taxaName = "To be determined";
                    break;

                case Taxa.Cephalopods:
                    taxaName = "Cephalopods";
                    break;

                case Taxa.Crabs:
                    taxaName = "Crabs";
                    break;

                case Taxa.Fish:
                    taxaName = "Fish";
                    break;

                case Taxa.Lobsters:
                    taxaName = "Lobsters";
                    break;

                case Taxa.Sea_cucumbers:
                    taxaName = "Sea cucumbers";
                    break;

                case Taxa.Sea_urchins:
                    taxaName = "Sea urchins";
                    break;

                case Taxa.Shells:
                    taxaName = "Shells";
                    break;

                case Taxa.Shrimps:
                    taxaName = "Shrimps";
                    break;

                default:
                    taxaName = "To be determined";
                    break;
            }

            return taxaName;
        }

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

        public CatchName(string inCatchNameGuid, string inGenus, string inSpecies, Taxa inTaxa,
                              bool inInFishBase, int? inFishBaseSpeciesNumber, string inNotes, int inCatchCompRecordCount,
                              short inGenusMPHKey1, short inGenusMPHKey2, short inSpeciesMPHKey1, short inSpeciesMPHKey2)
        {
            catchNameGuid = inCatchNameGuid;
            genus = inGenus;
            species = inSpecies;
            taxa = inTaxa;
            inFishBase = inInFishBase;
            fishBaseSpeciesNumber = inFishBaseSpeciesNumber;
            Notes = inNotes;
            genusMetaPhoneKey1 = inGenusMPHKey1;
            genusMetaPhoneKey2 = inGenusMPHKey2;
            speciesMetaPhoneKey1 = inSpeciesMPHKey1;
            speciesMetaPhoneKey2 = inSpeciesMPHKey2;
            _catchCompositionRecordCount = inCatchCompRecordCount;
        }

        public CatchName(string inCatchNameGuid, string inGenus, string inSpecies, Taxa inTaxa,
                              bool inInFishBase, int? inFishBaseSpeciesNumber, string inNotes, int inCatchCompRecordCount)

        {
            catchNameGuid = inCatchNameGuid;
            genus = inGenus;
            species = inSpecies;
            taxa = inTaxa;
            inFishBase = inInFishBase;
            fishBaseSpeciesNumber = inFishBaseSpeciesNumber;
            Notes = inNotes;
            _catchCompositionRecordCount = inCatchCompRecordCount;
        }
    }
}