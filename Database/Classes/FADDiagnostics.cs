using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dao;
using Microsoft.Win32;

namespace FAD3.Database.Classes
{
    public static class FADDiagnostics
    {
        public static bool ListDBTables(string mdbPath, string productVersion)
        {
            var dbe = new DBEngine();
            var dbTemplate = dbe.OpenDatabase(global.ApplicationPath + "\\template.mdb");
            var dbData = dbe.OpenDatabase(mdbPath);
            var count = 0;
            var os = Environment.OSVersion;

            Logger.LogSimple("");
            Logger.Log("start FADDiagnostics");
            Logger.LogSimple($"OS: {Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "")} {os.Version.Major}.{os.Version.Minor} {os.ServicePack}");
            Logger.LogSimple($"FAD version: {productVersion}");
            Logger.LogSimple($"Application path: {GetAppPath()}");
            Logger.LogSimple("-------------------------------------------------");
            Logger.LogSimple("Logging tables in data database");
            Logger.LogSimple("-------------------------------------------------");
            foreach (TableDef td in dbData.TableDefs)
            {
                if (td.Name.Substring(0, 4) != "MSys" && td.Name.Substring(0, 5) != "temp_")
                {
                    count++;
                    Logger.LogSimple($"{count}. {td.Name}");
                }
            }
            Logger.LogSimple("-------------------------------------------------");
            Logger.LogSimple("");
            Logger.LogSimple("");
            Logger.LogSimple("-------------------------------------------------");
            Logger.LogSimple("Logging tables in template database");
            Logger.LogSimple("-------------------------------------------------");
            count = 0;
            foreach (TableDef tdTemplate in dbTemplate.TableDefs)
            {
                if (tdTemplate.Name.Substring(0, 4) != "MSys" && tdTemplate.Name.Substring(0, 5) != "temp_")
                {
                    count++;
                    Logger.LogSimple($"{count}. {tdTemplate.Name}");
                }
            }
            Logger.LogSimple("-------------------------------------------------");
            Logger.LogSimple("");
            Logger.Log("end FADDiagnostics");

            return count > 0;
        }

        private static string GetAppPath()
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FAD3", true);
            try
            {
                return reg_key.GetValue("ApplicationPath").ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}