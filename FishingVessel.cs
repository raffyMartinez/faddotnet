﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dao;

namespace FAD3
{
    public static class FishingVessel
    {
        public static void MakeVesselTypeTable()
        {
            var dbe = new DBEngine();
            var dbData = dbe.OpenDatabase(global.mdbPath);

            try
            {
                dbData.TableDefs.Delete("temp_VesselType");
            }
            catch { }

            string sql = "SELECT 1 AS VesselTypeNo, 'Motorized' AS VesselType into temp_VesselType";
            var qd = dbData.CreateQueryDef("", sql);
            qd.Execute();

            sql = "Insert into temp_VesselType (VesselTypeNo, VesselType) values (2,'Non-Motorized')";
            qd = dbData.CreateQueryDef("", sql);
            qd.Execute();

            sql = "Insert into temp_VesselType (VesselTypeNo, VesselType) values (3,'No vessel used')";
            qd = dbData.CreateQueryDef("", sql);
            qd.Execute();

            sql = "Insert into temp_VesselType (VesselTypeNo, VesselType) values (4,'Not provided')";
            qd = dbData.CreateQueryDef("", sql);
            qd.Execute();

            qd.Close();
            qd = null;

            dbData.Close();
            dbData = null;
        }
    }
}