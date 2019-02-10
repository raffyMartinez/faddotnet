using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AxMapWinGIS;

namespace FAD3.Mapping.Classes
{
    public static class ShapefileDiskStorageHelper
    {
        public static AxMap MapControl { get; set; }

        public static bool Delete(string shapefileFileName)
        {
            int deleteCount = 0;
            bool proceed = false;
            string folderName = Path.GetDirectoryName(shapefileFileName);
            string filename = Path.GetFileName(shapefileFileName);
            if (shapefileFileName.Length > 0 && File.Exists($@"{shapefileFileName}.shp"))
            {
                for (int n = 0; n < MapControl.NumLayers; n++)
                {
                    if (MapControl.get_LayerFilename(n) == $"{shapefileFileName}.shp")
                    {
                        MapControl.RemoveLayer(n);
                    }
                }
                string[] fileList = Directory.GetFiles(folderName, $"{filename}.*");
                if (fileList.Length > 0)
                {
                    for (int f = 0; f < fileList.Length; f++)
                    {
                        try
                        {
                            File.Delete(fileList[f]);
                            deleteCount++;
                        }
                        catch (IOException ioex)
                        {
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    proceed = deleteCount == fileList.Length;
                }
            }
            else
            {
                proceed = true;
            }

            return proceed;
        }

        static ShapefileDiskStorageHelper()
        {
        }
    }
}