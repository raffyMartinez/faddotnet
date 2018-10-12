using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FAD3.Mapping.Classes
{
    internal static class MappingUtilities
    {
        public static ColorSchemes LayerColors;

        static MappingUtilities()
        {
            LayerColors = new ColorSchemes(ColorSchemeType.Layer);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Properties.Resources.colorschemes);
            LayerColors.LoadXML(doc);
        }
    }
}