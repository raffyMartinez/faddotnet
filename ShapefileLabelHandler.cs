using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapWinGIS;
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
        /// labels the shapefile with parameters saved in labelXML
        /// </summary>
        /// <param name="labelXML"></param>
        /// <returns></returns>
        public bool LabelShapefile(string labelXML)
        {
            var lbls = 0;
            _shapeFile.Labels.Deserialize(labelXML);
            var doc = new XmlDocument();
            doc.LoadXml(labelXML);
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
        public static void SaveLabelParameters(int layerHandle, bool AvoidCollision)
        {
            string mapStateFile = $@"{global.ApplicationPath}\mapstate";
            var doc = new XmlDocument();
            var n = 0;
            doc.Load(mapStateFile);
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

                            doc.Save(mapStateFile);
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