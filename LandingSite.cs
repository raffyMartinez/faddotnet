/*
 * Created by SharpDevelop.
 * User: Raffy
 * Date: 8/11/2016
 * Time: 5:13 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
//using System.Diagnostics;

namespace FAD3
{
	/// <summary>
	/// Description of LandingSite.
	/// </summary>
	public class landingsite
	{
        private string _LandingSiteGUID = "";
		private long _GearUsedCount = 0;
		private string _LandingSiteName = "";
		private string _GearVariationName = "";
		private string _GearVarGUID="";
		private string _GearClassNameFromGearVar = "";
        private double _xCoord = 0;
        private double _yCoord = 0;
        private long _LandingSiteDataCountEx = 0;

        public string GearClassNameFromGearVar{
			get{return _GearClassNameFromGearVar;}
		}
		
        public double xCoord
        {
            get { return _xCoord; }
        }

        public double yCoord
        {
            get { return _yCoord; }
        }

        public string xCoordDegreeMinute
        {
            get
            { return ToDegreeMinute(_xCoord); }
        }

        public string yCoordDegreeMinute
        {
            get
            { return ToDegreeMinute(_yCoord); }
        }

        private string ToDegreeMinute(double value)
        {
            int deg = (int)value;
            value = Math.Abs(value - deg);
            double min = (value * 60);
            return deg.ToString() + "° " + string.Format("{0:0.00000}", min);
        }

		public string GearVarGUID{
			get{return _GearVarGUID;}
			set{
				_GearVarGUID = value;
				string MyGearClass = GearClassFromGearVar(_GearVarGUID);
                gear.GearClassUsed = MyGearClass;
			}
		}
		
		public string GearVariationName{
			get{return _GearVariationName;}
			set{_GearVariationName = value;}
		}
		
		public landingsite(){
			//empty default constructor
		}
		
		public string LandingSiteName{
			get{return _LandingSiteName;}
			set{_LandingSiteName = value;}
		}
		public landingsite(string LandingSiteGUID)
		{
			_LandingSiteGUID=LandingSiteGUID;
		}
		
		public long GearUsedCount{
			get {return  _GearUsedCount;}
		}
		
		public long SampleCount()
		{
			long myCount = 0;
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try
				{
					conection.Open();
					string query = "SELECT Count(SamplingGUID) AS n FROM tblSampling " +
                           "WHERE LSGUID= '" + _LandingSiteGUID + "'";
					var command= new OleDbCommand(query, conection);
					var reader = command.ExecuteReader();
					
					if (reader.Read())
					{
						myCount = Convert.ToInt32(reader["n"]);
					}
				}
				catch(Exception ex){ErrorLogger.Log(ex);}
			}
			return myCount;
		}
		
		private string GearClassFromGearVar(string GearVarGUID){
			string GearClassNo = "";
			using (OleDbConnection conn = new OleDbConnection(global.ConnectionString)){
				try{
					string sql = "SELECT tblGearVariations.GearClass, tblGearClass.GearClassName " +
                       "FROM tblGearClass INNER JOIN tblGearVariations ON tblGearClass.GearClass = tblGearVariations.GearClass " +
                       "WHERE GearVarGUID='{" + GearVarGUID + "}'";
					OleDbCommand query = new OleDbCommand(sql,conn);
					conn.Open();
					OleDbDataReader rd = query.ExecuteReader();
					while(rd.Read()){
						GearClassNo = rd["GearClass"].ToString();
						_GearClassNameFromGearVar = rd["GearClassName"].ToString();
					}
				}
				catch(Exception ex){
					ErrorLogger.Log(ex);
				}
				
				return GearClassNo;
			}
		}
		public static bool UpdateData(bool isNew, Dictionary<string,string>LSData  ){
			bool Success = false;
			string updateQuery="";
			using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
			{
				try{
					if(isNew){
						updateQuery = "Insert into tblLandingSites " +
							"(AOIGUID, LSGUID, LSName, MunNo, cx, cy) values ('{" + 
							LSData["AOIGuid"] + "}','{" +
							LSData["LSGUID"] + "}', '" +
							LSData["LSName"] + "', " +
							LSData["MunNo"] + ", " +
							LSData["cx"] + ", " +
							LSData["cy"] + ")";
					}
					else
					{
						updateQuery = "UPDATE tblLandingSites set " +
							"LSName = '" + LSData["LSName"] + "', " +  
							"cx = " + LSData["cx"] + ", " + 
							"cy = " + LSData["cy"] + ", " +
							"MunNo = " + LSData["MunNo"] + " " +
							"Where LSGUID= '{" + LSData["LSGUID"] + "}'";
					}
					OleDbCommand update = new OleDbCommand(updateQuery,conn);
					conn.Open();
                    Success = (update.ExecuteNonQuery()>0);
					conn.Close();
				}
				catch (Exception ex){
					ErrorLogger.Log(ex);
				}
			}
			
			return Success;
		}
		
		public string LandingSiteGUID{
			get {return _LandingSiteGUID;}
			set {
				_LandingSiteGUID = value;
				_GearVarGUID = "";
				_GearVariationName="";
				_GearClassNameFromGearVar="";

			}
		}
		
		public List <string>MonthsSampledEx(string GearUsed){
			List<string>myMonths = new List<string>();
			var myDT =  new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try
				{
					conection.Open();
					string query = "SELECT Format([SamplingDate],'mmm-yyyy') AS sMonth, Count(SamplingGUID) AS n " +
                                   "FROM tblSampling GROUP BY Format([SamplingDate],'mmm-yyyy'), LSGUID, " +
						           "GearVarGUID, Year([SamplingDate]), Month([SamplingDate]) " +
                                   "HAVING LSGUID='{" + _LandingSiteGUID + "}'  AND GearVarGUID='{" + _GearVarGUID + "}'" +
                                   "ORDER BY Year([SamplingDate]), Month([SamplingDate])";

					//Debug.WriteLine (query);
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDT);
					for (int i=0; i<myDT.Rows.Count;i++)
					{
						DataRow dr = myDT.Rows[i];
						myMonths.Add (dr["sMonth"].ToString() + ": " + dr["n"].ToString());
					}

				}
				catch(Exception ex){ErrorLogger.Log(ex);}
			}
			return myMonths;
			}			
		
		
		public List<string> MonthsSampled()
		{
			List<string> myList = new List<string>();
			var myDT =  new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try
				{
					conection.Open();
					string query = "SELECT Format([SamplingDate],'mmm-yyyy') AS sMonth, Count(SamplingGUID) AS n " +
							"FROM tblSampling WHERE LSGUID= \"" + _LandingSiteGUID + "\" GROUP BY Format([SamplingDate],'mmm-yyyy') " +
							"ORDER BY First(SamplingDate)";
					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDT);
					for (int i=0; i<myDT.Rows.Count;i++)
					{
						DataRow dr = myDT.Rows[i];
						myList.Add (dr[0].ToString() + ": " + dr[1].ToString());
					}

				}
				catch(Exception ex){ErrorLogger.Log(ex);}
			}
			return myList;
		}
		
		public Dictionary<string, string> Gears()
		{
		 	Dictionary<string, string> myGears = new Dictionary<string, string>();
		 	var myDT =  new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try
				{
					conection.Open();
					string query = "SELECT tblSampling.GearVarGUID, tblGearVariations.Variation, tblGearClass.GearClassName, Count(tblSampling.SamplingGUID) AS n " +
                                   "FROM tblGearClass INNER JOIN (tblGearVariations INNER JOIN tblSampling ON tblGearVariations.GearVarGUID = tblSampling.GearVarGUID) " +
						           "ON tblGearClass.GearClass = tblGearVariations.GearClass WHERE tblSampling.LSGUID = \"" + _LandingSiteGUID + 
                                    "\" GROUP BY tblSampling.GearVarGUID, tblGearVariations.Variation, tblGearClass.GearClassName ORDER BY tblGearVariations.Variation";

					var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDT);
					for (int i=0; i<myDT.Rows.Count;i++)
					{
						DataRow dr = myDT.Rows[i];
						myGears.Add(dr[0].ToString(), dr[1].ToString() + ": " + dr[3].ToString());
						_GearUsedCount++;
					}
				}
				catch(Exception ex){ErrorLogger.Log(ex);}
			}
		 	return myGears;
		}
		
		public Dictionary<string,string>LandingSiteDataEx(){
			Dictionary<string,string>myLSData = new Dictionary<string, string>();
			var myDT =  new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try{
					conection.Open();
					string query = "SELECT tblLandingSites.LSName, Municipalities.Municipality, Provinces.ProvinceName, " +
						            "tblLandingSites.cx, tblLandingSites.cy, Municipalities.ProvNo " +
	                                "FROM Provinces INNER JOIN (Municipalities INNER JOIN tblLandingSites ON Municipalities.MunNo = " +
						            "tblLandingSites.MunNo) ON Provinces.ProvNo = Municipalities.ProvNo " +
						             "WHERE tblLandingSites.LSGUID=\"" + _LandingSiteGUID + "\"";
					
	                var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDT);
					for( int i=0; i < myDT.Rows.Count; i++)
					{
						DataRow dr = myDT.Rows[i];
						myLSData.Add("LSName",dr["LSname"].ToString());
						myLSData.Add("cx",dr["cx"].ToString());
						myLSData.Add("cy",dr["cy"].ToString());
						myLSData.Add("Municipality",dr["Municipality"].ToString());
						myLSData.Add("ProvinceName",dr["ProvinceName"].ToString());
                        _LandingSiteDataCountEx += _LandingSiteDataCountEx;

                    }
				}
				catch(Exception ex){
					ErrorLogger.Log(ex);
				}
			}
			
			return myLSData;
		}
		
		public string LandingSiteData()
		{
			string rv="";
			var myDT =  new DataTable();
			using(var conection = new OleDbConnection(global.ConnectionString))
			{
				try{
					conection.Open();
					string query = "SELECT tblLandingSites.LSName, Municipalities.Municipality, Provinces.ProvinceName, " +
						            "tblLandingSites.cx, tblLandingSites.cy " +
	                                "FROM Provinces INNER JOIN (Municipalities INNER JOIN tblLandingSites ON Municipalities.MunNo = " +
						            "tblLandingSites.MunNo) ON Provinces.ProvNo = Municipalities.ProvNo " +
						             "WHERE tblLandingSites.LSGUID=\"" + _LandingSiteGUID + "\"";

                    double num = 0;
                    var adapter = new OleDbDataAdapter(query, conection);
					adapter.Fill(myDT);
					DataRow dr = myDT.Rows[0];
                    _LandingSiteName = dr["LSName"].ToString();

                    if (double.TryParse((dr["cx"].ToString()), out num))
                    {
                        _xCoord = num;
                    }
                    if (double.TryParse((dr["cy"].ToString()), out num))
                    {
                        _yCoord = num;
                    }
                    for ( int i=0; i < myDT.Columns.Count; i++)
					{
						if(i==0)
						{
							rv = dr[i].ToString();
						}
						else
						{
							rv += "," + dr[i].ToString();
						}
					}
				}
				catch(Exception ex){ErrorLogger.Log(ex);}
			}
			return rv;
		}		
	}
}
