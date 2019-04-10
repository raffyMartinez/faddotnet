using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.GUI.Classes
{
    public static class TooltipGlobal
    {
        public static int AutoPopDelay { get;internal set;}
        public static int InitialDelay { get;internal set;}
        public static int ReshowDelay { get;internal set;}
        public static bool ShowAlways { get;internal set;} 

        static TooltipGlobal()
        {
            AutoPopDelay = 5000;
            InitialDelay = 1000;
            ReshowDelay = 500;
            ShowAlways = true;
        }
    }
}
