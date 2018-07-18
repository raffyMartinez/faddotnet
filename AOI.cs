/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/11/2016
 * Time: 7:04 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;

namespace FAD3
{
	/// <summary>
	/// Description of AOI.
	/// </summary>
	public class aoi
	{
		private string _AOIGUID="";
		private string _AOIName="";
		private string _AOILetter="";
		private string _MajorGrids="";
        private bool _HaveEnumerators = false;
        private Dictionary<string, string> _aois = new Dictionary<string, string>();
        private Dictionary<string,string> _landingSites = new Dictionary<string,string>();
        private Dictionary<string, string> _Enumerators = new Dictionary<string, string>();

		
		public string MajorGrids{
			get{return _MajorGrids;}
		}
		
        public Dictionary<string, string> LandingSites
        {
            get
            {
                getLandingSites();
                return _landingSites;
            }
        }

        public Dictionary<string, string> AOIs
        {
            get
            {
                getAOIs();
                return _aois;
            }
        }

        public Dictionary<string, string> Enumerators
        {
            get {
                getAOIEnumerators();
                return _Enumerators;
            }
        }

		public string AOILetter{
			get {return _AOILetter;}
		}
		
		public string AOIName{
			get {return _AOIName;}
			set {_AOIName =value;}
		}
		
		public string AOIGUID{
			get {return _AOIGUID;}
			set {
				_AOIGUID =value;
				_AOIName = GetAOIName(_AOIGUID);
                AOIHaveEnumerators();
            }
		}

        public bool HaveEnumerators
        {
            get { return _HaveEnumerators; }
            set { _HaveEnumerators = value; }
        }

        public aoi(){
            //default constructor
        }


        static public bool AOIHaveEnumeratorsEx(string AOIGuid)
        {
            var HasEnumerator = false;
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT TOP 1 EnumeratorID FROM tblEnumerators WHERE TargetArea ='{" + AOIGuid + "}'";
                    var command = new OleDbCommand(query, conection);
                    var reader = command.ExecuteReader();
                    HasEnumerator = reader.HasRows;
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }

            return HasEnumerator;
        }

        private void AOIHaveEnumerators()
        {
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT TOP 1 EnumeratorID FROM tblEnumerators WHERE TargetArea ='{" + _AOIGUID + "}'";
                    var command = new OleDbCommand(query, conection);
                    var reader = command.ExecuteReader();
                    _HaveEnumerators = reader.HasRows;
                }
                catch(Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
        }



        public List<string> SampledYears()
        {
            DataTable dt = new DataTable();
            List<string> myList = new List<string>();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT Year([SamplingDate]) AS sYear, Count(SamplingGUID) AS n FROM tblSampling GROUP BY Year([SamplingDate]) ORDER BY Year([SamplingDate]);";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(dr["sYear"].ToString() + ": " + dr["n"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
                return myList;
            }
        }

		public static bool UpdateEnumeratorData(bool isNew, Dictionary<string,string>EnumeratorData){
			bool Success = false;
			string updateQuery="";
			using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
			{
				try{
					if(isNew){
					updateQuery= "Insert into tblEnumerators (TargetArea, EnumeratorId, EnumeratorName,HireDate,Active) values ('{" +
							EnumeratorData["TargetArea"] + "}', '{" + 
							EnumeratorData["EnumeratorId"] + "}', '" +
							EnumeratorData["EnumeratorName"] + "', '" +
							EnumeratorData["HireDate"] + "', " +
							EnumeratorData["Active"] + ")";
					}
					else{
					 updateQuery="Update tblEnumerators set" +
					 	" EnumeratorName = '" + EnumeratorData["EnumeratorName"] +
					 	"', HireDate = '" + EnumeratorData["HireDate"] +
					 	"', Active = " + EnumeratorData["Active"] + " where " + 
					 	" EnumeratorId= '{" + EnumeratorData["EnumeratorId"] + "}'"; 
					}
					OleDbCommand update = new OleDbCommand(updateQuery,conn);
					conn.Open();
                    Success = (update.ExecuteNonQuery()>0);
                    
                    conn.Close();								
				}
				catch(Exception ex){
				 ErrorLogger.Log(ex);
				}
			}
			return Success;
		}
		public  string GetAOIName(string AOIGUID){
			string myName="";
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try
				{
					conection.Open();
					string query = "SELECT AOIName, Letter, MajorGridList from tblAOI WHERE AOIGuid= '" + _AOIGUID + "'";
					var command= new OleDbCommand(query, conection);
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						myName = reader["AOIName"].ToString();
						_AOILetter = reader["Letter"].ToString();
						_MajorGrids = reader["MajorGridList"].ToString();
					}
				}
				catch(Exception ex){
					ErrorLogger.Log(ex);
				}
			return myName;	
			}
		}
		
		private void getAOIs(){
            _aois.Clear();
			DataTable dt = new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try{
					conection.Open();
					string query = "SELECT AOIGuid, AOIName FROM tblAOI order by AOIName";
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(dt);
					for (int i=0; i<dt.Rows.Count;i++)
					{
						DataRow dr = dt.Rows[i];	
						_aois.Add(dr["AOIGuid"].ToString(),  dr["AOIName"].ToString());
					}
				}
				catch (Exception ex){
					ErrorLogger.Log(ex);
				}
			}
		}
		
        public static Dictionary<string, string>LandingSitesFromAOI(string AOIguid)
        {
            Dictionary<string, string> LandingSites = new Dictionary<string, string>();
            DataTable dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT LSGUID, LSName FROM tblLandingSites WHERE AOIGuid= \"" + AOIguid + "\" order by LSName";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        LandingSites.Add(dr["LSGUID"].ToString(), dr["LSName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }

            return LandingSites;
        }

		public aoi(string AOIGUID)
		{
			_AOIGUID = AOIGUID;
		}
		
		/*public static Dictionary<string,string> Enumerators(string AOIGUID){
			Dictionary<string, string> myEnum = new Dictionary<string, string>();
			DataTable dt = new DataTable();
		}*/
		
		public long SampleCount()
		{
			long myCount = 0;
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try
				{
					conection.Open();
					string query = "SELECT Count(SamplingGUID) AS n FROM tblSampling " +
                           "WHERE AOI= \"" + _AOIGUID + "\"";
					var command= new OleDbCommand(query, conection);
					var reader = command.ExecuteReader();
					
					if (reader.Read())
					{
						myCount = Convert.ToInt32(reader["n"]);
					}
				}
				catch (Exception ex)
				{
					ErrorLogger.Log(ex);
				}
			}
			return myCount;
		}
		
		public Dictionary<string,string>AOIWithSamplingCount(){
			Dictionary<string,string>myDict = new Dictionary<string,string>();
			DataTable dt = new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString)){
				try{
					conection.Open();
					string query = "SELECT tblAOI.AOIName, tblAOI.AOIGuid, Count(tblSampling.SamplingGUID) AS n FROM tblSampling " + 
						            "RIGHT JOIN tblAOI ON tblSampling.AOI = tblAOI.AOIGuid GROUP BY tblAOI.AOIName, tblAOI.AOIGuid";
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(dt);
					for (int i=0; i<dt.Rows.Count;i++)
					{
						DataRow dr = dt.Rows[i];
						myDict.Add(dr["AOIGuid"].ToString(),dr["AOIName"].ToString() + ": " + dr["n"].ToString());
					}
				}
				catch(Exception ex){
					ErrorLogger.Log(ex);
				}
			}
			
			return myDict;
		}
		
		public List<string> ListLandingSiteWithSamplingCount(){
			List<string> myLandingSiteList = new List<string>();
			DataTable dt = new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString)){
				try{
					conection.Open();
					string query = "SELECT tblLandingSites.LSName, Count(tblSampling.SamplingGUID) AS n " +
                                   "FROM tblLandingSites LEFT JOIN tblSampling ON tblLandingSites.LSGUID = tblSampling.LSGUID " +
                                   "WHERE tblLandingSites.AOIGuid = \"" + _AOIGUID + "\"" +
                                   "GROUP BY tblLandingSites.LSName";
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(dt);
					for (int i=0; i<dt.Rows.Count;i++)
					{
						DataRow dr = dt.Rows[i];
						myLandingSiteList.Add(dr["LSName"].ToString() + ": " + dr["n"].ToString());
					}
				}
				catch (Exception ex){
					ErrorLogger.Log(ex);
				}
			}
			
			return myLandingSiteList;
		}
		
		void getLandingSites(){
            _landingSites.Clear();
            DataTable dt = new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString)){
				try{
					conection.Open();
					string query = "SELECT LSGUID, LSName FROM tblLandingSites WHERE AOIGuid= \"" + _AOIGUID + "\" order by LSName";
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(dt);
					for (int i=0; i<dt.Rows.Count;i++)
					{
						DataRow dr = dt.Rows[i];
						_landingSites.Add(dr["LSGUID"].ToString(), dr["LSName"].ToString());
					}
				}
				catch (Exception ex){
					ErrorLogger.Log(ex);
				}
			}
		}
		
		public static bool UpdateData(bool IsNew, Dictionary<string, string>AOIData){
			string updateQuery="";
			bool Success = false;
			using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
			{
				try{
					if(IsNew){
						updateQuery= "Insert into tblAOI (AOIGUID, AOIName, Letter, MajorGridList) " + 
							"Values ('{" + 
							      AOIData["AOIGUID"] + "}', '" +
							      AOIData["AOIName"] + "', '" +
							      AOIData["Letter"] + "', '" +
							      AOIData["MajorGridList"] + "') ";
					}
					else{
						updateQuery ="Update tblAOI set " +
							" AOIName = '" + AOIData["AOIName"] +
							"', Letter = '" + AOIData["Letter"] + 
							"', MajorGridList = '" + AOIData["MajorGridList"] + 
							"' Where AOIGUID = '{" + AOIData["AOIGUID"] + "}'";
					}
					OleDbCommand update = new OleDbCommand(updateQuery,conn);
					conn.Open();
					Success = (update.ExecuteNonQuery()>0);
					conn.Close();
				}
				catch(Exception ex){
					ErrorLogger.Log(ex);
				}
			}
			return Success;
		}
		
		
		public Dictionary<string,string>AOIDataEx(){
			Dictionary<string,string> myData=new Dictionary<string,string>();
			var dt = new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try
				{
					conection.Open();
					string query="Select AOIName, Letter, MajorGridList from tblAOI where AOIGuid = \"" + _AOIGUID + "\"";		
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(dt);
					for( int i=0; i < dt.Rows.Count; i++){
						DataRow dr = dt.Rows[i];
						myData.Add("AOIName", dr["AOIName"].ToString());
						myData.Add("Letter", dr["Letter"].ToString());
						myData.Add("MajorGridList", dr["MajorGridList"].ToString());
					}
				}
				catch (Exception ex)
				{
					ErrorLogger.Log(ex);
				}
			}
			return myData;
		}
		
		public string AOIData()
		{
			string rv="";
			var myDT =  new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try
				{
					conection.Open();
					
					string query="Select AOIName, Letter, MajorGridList from tblAOI where AOIGuid = \"" + _AOIGUID + "\"";
	                var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDT);
					DataRow dr = myDT.Rows[0];
					for( int i=0; i < myDT.Columns.Count; i++)
					{
						if(i==0)
						{
							rv = dr[i].ToString();
						}
						else
						{
							rv += "|" + dr[i].ToString();
						}
					}					
				}
				catch(Exception ex) 
				{
					ErrorLogger.Log(ex);
				}
			}
			return rv;
		}
		
	
		public Dictionary<string,string> ListYearsWithSamplingCount(){
			Dictionary<string, string> myYears=new Dictionary<string, string>();
			var myDT =  new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try
				{
					conection.Open();
					string query = "SELECT Year([SamplingDate]) AS sYear, Count(tblSampling.SamplingGUID) AS n " +
                                    "FROM tblSampling WHERE AOI=\"" + _AOIGUID + "\" GROUP BY Year([SamplingDate])";
	                var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDT);
					for (int i = 0; i < myDT.Rows.Count; i++){
						DataRow dr = myDT.Rows[i];
						myYears.Add(dr["sYear"].ToString(), dr["n"].ToString());
					}
				}
				catch (Exception ex)
				{
					ErrorLogger.Log(ex);
				}
			}
			return myYears;
		}
			
		static public Dictionary<string, string> AOIEnumeratorsList(string AOIGuid)
        {
            Dictionary<string, string> myAOIEnumerators = new Dictionary<string, string>();
            var myDT = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();


                    string query = "Select EnumeratorID, EnumeratorName from tblEnumerators where TargetArea =\"" + AOIGuid + "\" " +
                                    "Order by EnumeratorName";
                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(myDT);
                    for (int i = 0; i < myDT.Rows.Count; i++)
                    {
                        DataRow dr = myDT.Rows[i];
                        myAOIEnumerators.Add(dr["EnumeratorID"].ToString(), dr["EnumeratorName"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }

            return myAOIEnumerators;
        }
        
		private  void getAOIEnumerators()
		{
            _Enumerators.Clear();
            var myDT =  new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try
				{
					conection.Open();


                    string query = "Select EnumeratorID, EnumeratorName from tblEnumerators where TargetArea =\"" + _AOIGUID + "\" " + 
						            "Order by EnumeratorName";
	                var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDT);
					for (int i = 0; i < myDT.Rows.Count; i++){
						DataRow dr = myDT.Rows[i];
						_Enumerators.Add (dr["EnumeratorID"].ToString(), dr["EnumeratorName"].ToString());
					}
				}
				catch (Exception ex)
				{
					ErrorLogger.Log(ex);
				}
			}
		}

        public Dictionary<string, string> EnumeratorsWithCount()
        {
            Dictionary<string, string> myList = new Dictionary<string, string>();
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    string query = "SELECT EnumeratorID, EnumeratorName, Count(Enumerator) AS n " +
                                   "FROM tblEnumerators LEFT JOIN tblSampling ON tblEnumerators.EnumeratorID = tblSampling.Enumerator " +
                                   "GROUP BY tblEnumerators.EnumeratorID, tblEnumerators.EnumeratorName, tblEnumerators.TargetArea " +
                                   "HAVING tblEnumerators.TargetArea ='{" + _AOIGUID + "}'";
                    


                    var adapter = new OleDbDataAdapter(query, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        myList.Add(dr["EnumeratorName"].ToString(), dr["EnumeratorID"].ToString() + "," + dr["n"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }
            }
            return myList;
        }

        public string AOIEnumerators(ref int EnumeratorCount)
		{
			string rv="";
			int myCount=0;
			var myDT =  new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try
				{
					conection.Open();
					string query = "Select EnumeratorID, EnumeratorName from tblEnumerators where TargetArea =\"" + _AOIGUID + "\"";
	                var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDT);
					
					for (int i=0; i < myDT.Rows.Count;i++){
						DataRow dr = myDT.Rows[i];
						if(i==0)
						{
							rv = dr[1].ToString();
						}
						else
						{
							rv += "|" + dr[1].ToString();
						}
						myCount++;
					}
				}
				catch(Exception ex){ErrorLogger.Log(ex);}

			}
			EnumeratorCount=myCount;
			return rv;
		}		
	}
}
