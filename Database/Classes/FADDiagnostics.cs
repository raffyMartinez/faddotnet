using dao;
using Microsoft.Win32;
using System;
using Microsoft.VisualBasic.Devices;

namespace FAD3.Database.Classes
{
    public static class FADDiagnostics
    {
        private static string GetProcessor()
        {
            string cpu = "";
            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", false);
            if (processor_name != null && processor_name.GetValue("ProcessorNameString") != null)
            {
                cpu = processor_name.GetValue("ProcessorNameString").ToString();
            }
            return cpu;
        }

        public static bool Diagnose(string mdbPath, string productVersion)
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
            Logger.LogSimple($"Logged in user: {Environment.UserName}");
            Logger.LogSimple($"Machine name: {Environment.MachineName}");
            Logger.LogSimple($"RAM: {((int)(new ComputerInfo().TotalPhysicalMemory / (Math.Pow(1024, 3)) + 0.5)).ToString()} GB");
            Logger.LogSimple($"CPU: {GetProcessor()}");

            Logger.LogSimple("");
            Logger.LogSimple("------------------------------------------------------------");
            Logger.LogSimple("Database tables");
            Logger.LogSimple($"Row\t{string.Format("{0,-40}", "Template table")}\tData table");
            Logger.LogSimple("------------------------------------------------------------");
            count = 0;
            foreach (TableDef tdTemplate in dbTemplate.TableDefs)
            {
                if (tdTemplate.Name.Substring(0, 4) != "MSys" && tdTemplate.Name.Substring(0, 5) != "temp_")
                {
                    count++;
                    Logger.LogSimpleEx($"{count}\t{string.Format("{0,-40}", tdTemplate.Name)}");
                    foreach (TableDef tdData in dbData.TableDefs)
                    {
                        if (tdData.Name == tdTemplate.Name)
                        {
                            Logger.LogSimpleEx("\tx\r\n");
                            break;
                        }
                    }
                }
            }
            Logger.LogSimple("------------------------------------------------------------");
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