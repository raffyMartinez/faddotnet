using System.IO;
using System.Xml;

namespace FAD3
{
    public class LandingSiteFromKML
    {
        public string KMLFile { get; set; }
        public aoi aoi { get; set; }
        public delegate void LandingSiteRetrieved(LandingSiteFromKML s, Landingsite e);
        public event LandingSiteRetrieved OnLandingSiteRetrieved;

        private bool IsInside(FishingGrid.Grid25Struct grid25, double xCoord, double yCoord)
        {
            foreach (var item in grid25.Bounds)
            {
                if (item.IsInisde(xCoord, yCoord))
                {
                    return true;
                }
            }
            return false;
        }

        public void ParseLandingSites()
        {
            if (aoi != null && File.Exists(KMLFile))
            {
                FishingGrid.AOIGuid = aoi.AOIGUID;
                var grid25 = FishingGrid.Grid25;
                var doc = new XmlDocument();
                var inLandingSiteFolder = false;
                var landingSiteName = string.Empty;
                double xCoordinate = 0, yCoordinate = 0;
                doc.Load(KMLFile);
                foreach (XmlNode nd in doc.DocumentElement.SelectNodes("//*"))
                {
                    if (nd.Name == "Folder")
                    {
                        foreach (XmlNode cnd in nd.ChildNodes)
                        {
                            switch (cnd.Name)
                            {
                                case "name":
                                    inLandingSiteFolder = false;
                                    if (string.Equals(cnd.InnerXml, "Landing site", System.StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        inLandingSiteFolder = true;
                                    }
                                    break;

                                case "Placemark":
                                    if (inLandingSiteFolder)
                                    {
                                        foreach (XmlNode pl in cnd.ChildNodes)
                                        {
                                            if (pl.Name == "name")
                                            {
                                                landingSiteName = pl.InnerXml;
                                            }
                                            else if (pl.Name == "Point")
                                            {
                                                foreach (XmlNode pt in pl.ChildNodes)
                                                {
                                                    if (pt.Name == "coordinates")
                                                    {
                                                        var arr = pt.InnerXml.Split(',');
                                                        xCoordinate = double.Parse(arr[0]);
                                                        yCoordinate = double.Parse(arr[1]);

                                                        if (OnLandingSiteRetrieved != null)
                                                        {
                                                            Landingsite ls = new Landingsite
                                                            {
                                                                LandingSiteName = landingSiteName,
                                                            };
                                                            ls.xyKMLCoordinate(xCoordinate, yCoordinate, IsInside(grid25, xCoordinate, yCoordinate));
                                                            OnLandingSiteRetrieved(this, ls);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}