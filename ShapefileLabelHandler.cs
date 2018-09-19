using MapWinGIS;
using System;
using System.Xml;

namespace FAD3
{
    public class ShapefileLabelHandler : IDisposable
    {
        private bool _disposed;
        private Shapefile _shapeFile;
        public LabelProperty LabelProperty { get; set; }

        private void SetupLabelClass(Shapefile sf)
        {
            sf.Labels.With(l =>
            {
                LabelProperty.Alignment = l.Alignment;
                LabelProperty.FontName = l.FontName;
                LabelProperty.FontSize = l.FontSize;
                LabelProperty.FontColor = l.FontColor;
                LabelProperty.FontColor2 = l.FontColor2;
                LabelProperty.FontGradientMode = l.FontGradientMode;
                LabelProperty.FontBold = l.FontBold;
                LabelProperty.FontItalic = l.FontItalic;
                LabelProperty.FontStrikeout = l.FontStrikeOut;
                LabelProperty.FontUnderline = l.FontUnderline;
                LabelProperty.FontTransparency = l.FontTransparency;
                LabelProperty.FontOutlineColor = l.FontOutlineColor;
                LabelProperty.FontOutlineWidth = l.FrameOutlineWidth;
                LabelProperty.FontOutlineVisible = l.FontOutlineVisible;

                LabelProperty.FrameBackColor = l.FrameBackColor;
                LabelProperty.FrameBackColor2 = l.FrameBackColor2;
                LabelProperty.FrameGradientMode = l.FrameGradientMode;
                LabelProperty.FrameOutlineColor = l.FrameOutlineColor;
                LabelProperty.FrameOutlineStyle = l.FrameOutlineStyle;
                LabelProperty.FrameOutlineWidth = l.FrameOutlineWidth;
                LabelProperty.FramePaddingX = l.FramePaddingX;
                LabelProperty.FramePaddingY = l.FramePaddingY;
                LabelProperty.FrameTransparency = l.FrameTransparency;
                LabelProperty.FrameType = l.FrameType;
                LabelProperty.FrameVisible = l.FrameVisible;

                LabelProperty.HaloColor = l.HaloColor;
                LabelProperty.HaloSize = l.HaloSize;
                LabelProperty.HaloVisible = l.HaloVisible;

                LabelProperty.InboxAlignment = l.InboxAlignment;
                LabelProperty.LineOrientation = l.LineOrientation;
                LabelProperty.OffsetX = l.OffsetX;
                LabelProperty.OffsetY = l.OffsetY;

                LabelProperty.ShadowColor = l.ShadowColor;
                LabelProperty.ShadowOffsetX = l.ShadowOffsetX;
                LabelProperty.ShadowOffsetY = l.ShadowOffsetY;
                LabelProperty.ShadowVisible = l.ShadowVisible;
            });
        }

        public ShapefileLabelHandler(Shapefile sf)
        {
            _shapeFile = sf;
            SetupLabelClass(sf);
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
            SetupLabelClass(_shapeFile);

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