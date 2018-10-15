using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapWinGIS;

namespace FAD3.Mapping.Classes
{
    public class ConfigureMapTextHelper
    {
        private Shapefile _mapTextShapefile;

        public ConfigureMapTextHelper()
        {
            TitleSize = 13;
            NoteSize = 10;
            TitleBold = false;
            TitleText = "Map title";
            NoteText = "Map note";
            TitleVisible = false;
            NoteVisible = false;
            TitleAlignment = tkLabelAlignment.laCenter;
            NoteAlignment = tkLabelAlignment.laCenterLeft;
        }

        public Shapefile MapTextShapefile
        {
            get { return _mapTextShapefile; }
        }

        public bool TitleVisible { get; set; }
        public bool NoteVisible { get; set; }
        public bool TitleBold { get; set; }
        public int TitleSize { get; set; }
        public int NoteSize { get; set; }
        public tkLabelAlignment TitleAlignment { get; set; }
        public tkLabelAlignment NoteAlignment { get; set; }

        public string TitleText { get; set; }
        public string NoteText { get; set; }

        public void SetText(string titleText, string noteText)
        {
            TitleText = titleText;
            NoteText = noteText;
        }
    }
}