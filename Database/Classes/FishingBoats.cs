using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace FAD3.Database.Classes
{
    public class FishingBoats:IEnumerable<FishingBoat>
    {

        private Dictionary<string, FishingBoat> _fishingBoats = new Dictionary<string, FishingBoat>();

        public FishingBoats(TargetArea targetArea)
        {
            ReadDatabase(targetArea);
            
        }

        public FishingBoats(Landingsite landingSite)
        {
            ReadDatabase(landingSite);
        }


        private void ReadDatabase(TargetArea targetArea)
        {
            var sql = $@"SELECT tblLandingSites.LSName, tblFishingBoat.*
                        FROM tblAOI INNER JOIN(
                            tblLandingSites INNER JOIN 
                            tblFishingBoat ON 
                                tblLandingSites.LSGUID = tblFishingBoat.LandingSite) ON 
                                tblAOI.AOIGuid = tblLandingSites.AOIGuid
                        WHERE tblAOI.AOIGuid ={{{targetArea.TargetAreaGuid}}}";
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        Landingsite ls = new Landingsite(dr["LandingSite"].ToString(), targetArea.TargetAreaGuid);
                        ls.LandingSiteName = dr["LSName"].ToString();
                        FishingBoat fb = new FishingBoat(Guid.Parse(dr["BoatGuid"].ToString()), ls, dr["BoatName"].ToString(), dr["OwnerName"].ToString());
                        if (dr["Length"].ToString().Length > 0
                            && dr["Width"].ToString().Length > 0
                            && dr["Height"].ToString().Length > 0)
                        {
                            fb.Dimension(
                                double.Parse(dr["Width"].ToString()),
                                double.Parse(dr["Height"].ToString()),
                                double.Parse(dr["Length"].ToString())
                                );
                        }
                        fb.Engine = dr["Engine"].ToString();
                        if (dr["EngineHp"].ToString().Length > 0)
                        {
                            fb.EngineHp = double.Parse(dr["EngineHp"].ToString());
                        }
                        _fishingBoats.Add(fb.BoatGuid.ToString(), fb);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, "FishingBoat.cs", "ReadDatabase");
                }
            }
        }
        private void ReadDatabase(Landingsite landingSite)
        {
            var sql = $"Select * from tblFishingBoat where LandingSite = {{{landingSite.LandingSiteGUID}}}";
            var dt = new DataTable();
            using (var conection = new OleDbConnection(global.ConnectionString))
            {
                try
                {
                    conection.Open();
                    var adapter = new OleDbDataAdapter(sql, conection);
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        FishingBoat fb = new FishingBoat(Guid.Parse(dr["BoatGuid"].ToString()),landingSite,dr["BoatName"].ToString(), dr["OwnerName"].ToString());
                        if(dr["Length"].ToString().Length>0
                            && dr["Width"].ToString().Length>0
                            && dr["Height"].ToString().Length>0)
                        {
                            fb.Dimension(
                                double.Parse( dr["Width"].ToString()),
                                double.Parse( dr["Height"].ToString()),
                                double.Parse( dr["Length"].ToString())
                                );
                        }
                        fb.Engine = dr["Engine"].ToString();
                        if(dr["EngineHp"].ToString().Length>0)
                        {
                            fb.EngineHp = double.Parse(dr["EngineHp"].ToString());
                        }
                        _fishingBoats.Add(fb.BoatGuid.ToString(), fb);
                    }
                }
                catch(Exception ex)
                {
                    Logger.Log(ex.Message, "FishingBoat.cs", "ReadDatabase");
                }
            }
        }

        private bool AddNew(FishingBoat fb)
        {
            bool success = false;
            string query = "";
            using (OleDbConnection conn = new OleDbConnection(global.ConnectionString))
            {
                try
                {

                    string nullValue = "null";
                    query = $@"Insert into tblFishingBoat
                            (BoatGuid, BoatName, OwnerName, Length, Width, Height, Engine, EngineHp, LandingSite) values (
                             {{{fb.BoatGuid.ToString()}}},
                            '{fb.BoatName}',
                            '{fb.OwnerName}',
                            {(fb.BoatLength==null?nullValue:fb.BoatLength.ToString())},
                            {(fb.BoatWidth==null?nullValue:fb.BoatWidth.ToString())},
                            {(fb.BoatHeight==null?nullValue:fb.BoatHeight.ToString())},
                            '{fb.Engine}',
                            {(fb.EngineHp==null?nullValue:fb.EngineHp.ToString())},
                            {{{fb.LandingSite.LandingSiteGUID}}}
                            )";
                    

                    conn.Open();
                    using (OleDbCommand update = new OleDbCommand(query, conn))
                    {
                        success = (update.ExecuteNonQuery() > 0);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Logger.Log($"{ex.Message}\r\n{ex.Source}");
                }
            }
            return success;
        }

        public bool Add(FishingBoat fishingBoat)
        {
            try
            {
                if (AddNew(fishingBoat))
                {
                    _fishingBoats.Add(fishingBoat.BoatGuid.ToString(), fishingBoat);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message, "FishingBoat.cs", "Add");
                return false;
            }
        }

        public FishingBoat this[string boatName]
        {
            get
            {
                return _fishingBoats[boatName];
            }
            set
            {
                _fishingBoats[boatName] = value;
            }
        }

        public void BoatDimensions(Guid boatGuid, double height, double width, double length)
        {
            _fishingBoats[boatGuid.ToString()].BoatHeight = height;
            _fishingBoats[boatGuid.ToString()].BoatLength = length;
            _fishingBoats[boatGuid.ToString()].BoatWidth = width;

        }
        public IEnumerator<FishingBoat> GetEnumerator()
        {
            return _fishingBoats.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //forces use of the non-generic implementation on the Values collection
            return ((IEnumerable)_fishingBoats.Values).GetEnumerator();
        }

    }

}
