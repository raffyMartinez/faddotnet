using MapWinGIS;
using System;
using System.Xml;

namespace FAD3
{
    public class ShapefileLabelHandler : IDisposable
    {
        private bool _disposed;
        private Shapefile _shapeFile;

        public ShapefileLabelHandler(Shapefile sf)
        {
            _shapeFile = sf;
        }

        /// <summary>
        /// labels the shapefile using parameters in labelXML
        /// </summary>
        /// <param name="labelXML"></param>
        /// <returns></returns>
        public bool LabelShapefile(string labelXML)
        {
            var lbls = 0;

            //applies label properties to the current shapefile's labels
            _shapeFile.Labels.Deserialize(labelXML);

            //we convert string labelXML to an xml object
            var doc = new XmlDocument();
            doc.LoadXml(labelXML);

            //we use attributes such as SourceField and Positioning in generating labels for the shapefile
            doc.DocumentElement.With(d =>
            {
                lbls = _shapeFile.GenerateLabels(
                int.Parse(d.Attributes["SourceField"].Value),
                (tkLabelPositioning)int.Parse(d.Attributes["Positioning"].Value), true);

                //AvoidCollision is not included in labelXML so we put it here
                _shapeFile.Labels.AvoidCollisions = d.Attributes["AvoidCollision"].Value == "1";
            });

            return lbls >= 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// saves those parameters that are not included when invoking axMap.SaveMapState
        /// </summary>
        /// <param name="layerHandle"></param>
        /// <param name="AvoidCollision"></param>
        public static void SaveLabelParameters(string fileMapState, int layerHandle, bool AvoidCollision)
        {
            var doc = new XmlDocument();
            var n = 0;
            doc.Load(fileMapState);
            foreach (XmlNode ly in doc.DocumentElement.SelectNodes("//Layer"))
            {
                //if (ly.Attributes["LayerType"].Value == "Shapefile")
                if (n == layerHandle)
                {
                    foreach (XmlNode child in ly.FirstChild.ChildNodes)
                    {
                        if (child.Name == "LabelsClass")
                        {
                            var att = doc.CreateAttribute("AvoidCollision");
                            att.Value = AvoidCollision ? "1" : "0";
                            child.Attributes.Append(att);

                            doc.Save(fileMapState);
                            return;
                        }
                    }
                }
                n++;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }
                _shapeFile = null;
                _disposed = true;
            }
        }
    }
}