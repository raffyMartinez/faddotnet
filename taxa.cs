using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections;

namespace FAD3
{
    public class taxa
    {
        private static Dictionary<int, string> _taxaDictionary;
        public static readonly int To_be_determined = 0;

        static taxa()
        {
            RetrieveTaxaDictionary();
        }

        public static Dictionary<int, string> taxaDictionary
        {
            get { return _taxaDictionary; }
        }

        public static int TaxaFromCatchNameGUID(string CatchNameGUID)
        {
            int taxa = To_be_determined;
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
                    taxa = (int)dr["TaxaNo"];
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
                return taxa;
            }
        }

        public static string TaxaNameFromTaxa(int taxa)
        {
            var taxaName = "";
            foreach (var item in _taxaDictionary)
            {
                if (item.Key == taxa)
                {
                    taxaName = item.Value;
                    break;
                }
            }
            return taxaName;
        }

        public static int TaxaFromTaxaName(string taxaName)
        {
            var taxaNumber = 0;
            foreach (var item in _taxaDictionary)
            {
                if (item.Value == taxaName)
                {
                    taxaNumber = item.Key;
                    break;
                }
            }

            return taxaNumber;
        }

        public static bool TryParse(string taxaText, out int taxaNumber)
        {
            bool isNumeric = false;
            int taxaNumber = taxa.To_be_determined;

            if (int.TryParse(taxaText, out int aNumber))
            {
                isNumeric = true;
                taxaNumber = aNumber;
            }
            return isNumeric;
        }

        public static int Parse(string taxaText)
        {
            int taxaNumber = taxa.To_be_determined;

            if (int.TryParse(taxaText, out int aNumber))
                taxaNumber = aNumber;

            return taxaNumber;
        }

        private static void RetrieveTaxaDictionary()
        {
            _taxaDictionary = new Dictionary<int, string>();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;data source=" + global.mdbPath))
            {
                conection.Open();

                const string query = "Select TaxaNo, Taxa from tblTaxa order by Taxa";

                var adapter = new OleDbDataAdapter(query, conection);
                var dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    _taxaDictionary.Add(dr.Field<int>("TaxaNo"), dr.Field<string>("Taxa"));
                }
            }
        }

        private int _taxaNumber;
        private string _taxaName;

        public string taxaName
        {
            get { return _taxaName; }
        }

        public int taxaNumber
        {
            get { return _taxaNumber; }
            set { _taxaNumber = value; }
        }

        public taxa(int taxaNumber)
        {
            _taxaNumber = taxaNumber;
            _taxaName = TaxaNameFromTaxa(_taxaNumber);
        }
    }
}