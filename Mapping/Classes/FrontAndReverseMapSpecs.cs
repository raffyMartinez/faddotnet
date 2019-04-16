using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAD3.Mapping.Classes
{
   public class FrontAndReverseMapSpecs
    {
        public bool IsGrid25Layer { get; set; }
        public int LayerHandle { get; set; }
        public string LayerName { get; set; }
        public bool ShowInFront { get; set; }
        public bool ShowLabelsFront { get; set; }
        public bool ShowInReverse { get; set; }
        public bool ShowLabelsReverse { get; set; }

        public FrontAndReverseMapSpecs(int layerHandle, bool isGrid25Layer, 
            string layerName, bool showInFront, 
            bool showLabelsFront, bool showInReverse, 
            bool showLabelsReverse)
        {
            LayerHandle = layerHandle;
            IsGrid25Layer = isGrid25Layer;
            LayerName = layerName;
            ShowInFront = showInFront;
            ShowLabelsFront = showLabelsFront;
            ShowInReverse = showInReverse;
            ShowLabelsReverse = showLabelsReverse;
        }
    }
}
