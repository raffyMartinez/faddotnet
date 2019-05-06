using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISO_Classes;
using FAD3.Database.Classes;
using System.Data;
using System.Data.OleDb;

namespace FAD3.Mapping.Classes
{
    public class BarangayMunicipalityCoordinateHelper
    {
        public int ParentFeature { get; set; }
        public string LGULevel { get; set; }
        public string LGUName { get; set; }
        public int LGUNumber { get; set; }
        public Coordinate Coordinate { get; set; }

        public List<LocationCoordinate> ReadCoordinates(string inventoryGuid)
        {
            List<LocationCoordinate> coordinates = new List<LocationCoordinate>();
            string sql = $@"SELECT DISTINCT Provinces.ProvinceName, Municipalities.MunNo, Municipalities.Municipality,
                            Municipalities.xCoord, Municipalities.yCoord, Barangay.BarangayName, Barangay.x, Barangay.y
                            FROM Barangay
                                RIGHT JOIN(Provinces
                                INNER JOIN (tblGearInventories
                                INNER JOIN (Municipalities
                                INNER JOIN tblGearInventoryBarangay
                                ON (Municipalities.MunNo = tblGearInventoryBarangay.Municipality) AND(Municipalities.MunNo = tblGearInventoryBarangay.Municipality))
                                ON tblGearInventories.InventoryGuid = tblGearInventoryBarangay.InventoryGuid)
                                ON Provinces.ProvNo = Municipalities.ProvNo)
                                ON(Barangay.Parent_Feature = tblGearInventoryBarangay.Municipality) AND(Barangay.BarangayName = tblGearInventoryBarangay.Barangay)
                                WHERE tblGearInventories.InventoryGuid = {{{inventoryGuid}}}
                                Order By ProvinceName, Municipalities.Municipality,BarangayName";

            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    bool hasCoordinate = false;
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Coordinate c = new Coordinate();
                            if (dr["yCoord"].ToString().Length > 0)
                            {
                                c = new Coordinate(float.Parse(dr["yCoord"].ToString()), float.Parse(dr["xCoord"].ToString()));
                                hasCoordinate = true;
                            }
                            else
                            {
                                hasCoordinate = false;
                            }

                            LocationCoordinate lc = new LocationCoordinate(dr["ProvinceName"].ToString(),
                                dr["Municipality"].ToString(),
                                int.Parse(dr["MunNo"].ToString()), c, "municipality");
                            coordinates.Add(lc);
                            lc.HasCoordinate = hasCoordinate;

                            string barangay = dr["BarangayName"].ToString();

                            if (barangay.Length > 0)
                            {
                                c = new Coordinate(float.Parse(dr["y"].ToString()), float.Parse(dr["y"].ToString()));
                                LocationCoordinate lc1 = lc;
                                lc1.LGULevel = "barangay";
                                lc1.BarangayName = barangay;
                                coordinates.Add(lc1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "BarangayMunicipalityCoordinateHelper", "ReadCoordinates");
                }
            }
            return coordinates;
        }

        public BarangayMunicipalityCoordinateHelper()
        {
        }

        public BarangayMunicipalityCoordinateHelper(string lguLevel, string lguName)
        {
            LGULevel = lguLevel;
            LGUName = lguName;
        }

        public static (Coordinate c, bool success) GetCoordinate(string lguLevel, int LGUNumber, string barangayName = "")
        {
            Coordinate c = new Coordinate();
            bool success = false;
            string sql = "";
            switch (lguLevel)
            {
                case "barangay":
                    sql = $"Select x,y from Barangay where BarangayName = '{barangayName}' AND Parent_Feature = {LGUNumber}";
                    break;

                case "municipality":
                    sql = $"Select xCoord, yCoord from Municipalities where MunNo ={LGUNumber}";
                    break;
            }
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        if (dr["yCoord"].ToString().Length > 0)
                        {
                            c.Latitude = float.Parse(dr["yCoord"].ToString());
                            c.Longitude = float.Parse(dr["xCoord"].ToString());
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "BarangayMunicipalityCoordinateHelper", "GetCoordinateString");
                }
            }
            return (c, success);
        }

        public bool SetCoordinate()
        {
            bool success = false;
            string sql = "";
            switch (LGULevel)
            {
                case "barangay":
                    if (DBCheck.TableList.Contains("Barangay"))
                    {
                    }

                    break;

                case "municipality":

                    sql = $@"Update Municipalities set
                        xCoord={Coordinate.Longitude},
                        yCoord={Coordinate.Latitude}
                        Where MunNo = {LGUNumber}";

                    break;
            }
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                conn.Open();
                using (OleDbCommand update = new OleDbCommand(sql, conn))
                {
                    success = (update.ExecuteNonQuery() > 0);
                }
            }
            return success;
        }
    }
}