﻿using System;
using System.Drawing;
using MapWinGIS;
using FAD3.Database.Classes;

namespace FAD3
{
    public class MapLayer : IDisposable
    {
        public string Name { get; set; }
        public bool Visible { get; set; }
        public bool VisibleInLayersUI { get; set; }
        public int Handle { get; set; }
        public string FileName { get; set; }
        public string GeoProjectionName { get; set; }
        public int LayerPosition { get; set; }
        public Bitmap ImageThumbnail { get; set; }
        public string LayerType { get; set; }
        public object LayerObject { get; set; }
        private bool _disposed;
        public int[] SelectedIndexes { get; set; }
        public bool IsGraticule { get; set; }
        public bool IsFishingGrid { get; set; }
        public int? LayerWeight { get; set; }
        public bool IsMaskLayer { get; set; }
        public bool IsLabeled { get; set; }
        public int LabelField { get; set; }
        public string Expression { get; set; }
        public string LabelSource { get; set; }
        public string LabelsVisibilityExpression { get; set; }
        public string ShapesVisibilityExpression { get; set; }
        public string LabelSettingsXML { get; internal set; }
        public string SymbolSettinsXML { get; internal set; }
        public Labels Labels { get; internal set; }
        public fad3MappingMode MappingMode { get; set; }

        public void RestoreSettingsFromXML()
        {
            var sf = LayerObject as Shapefile;
            if (sf != null)
            {
                sf.Labels.Deserialize(LabelSettingsXML);
                sf.DefaultDrawingOptions.Deserialize(SymbolSettinsXML);
            }
        }

        public void SaveXMLSettings()
        {
            var sf = LayerObject as Shapefile;
            if (sf != null)
            {
                LabelSettingsXML = sf.Labels.Serialize();
                SymbolSettinsXML = sf.DefaultDrawingOptions.Serialize();
            }
        }

        public MapLayer(int handle, string name, bool visible, bool visibleInLayersUI)
        {
            Handle = handle;
            Name = name;
            Visible = visible;
            VisibleInLayersUI = visibleInLayersUI;
            IsFishingGrid = false;
        }

        public bool Save(string fileName)
        {
            var success = false;
            if (LayerType == "ShapefileClass")
            {
                ((Shapefile)LayerObject).With(sf =>
                {
                    success = sf.SaveAs(fileName + ".shp");                     //saves the shapefile
                    if (success)
                    {
                        sf.GeoProjection.WriteToFile(fileName + ".prj");        //save the shapefile's projection data
                    }
                });
            }
            else
            {
                //for image type, possibly
            }
            return success;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }
                if (ImageThumbnail != null)
                {
                    ImageThumbnail.Dispose();
                }
                ImageThumbnail = null;
                LayerObject = null;
                Labels = null;
                _disposed = true;
            }
        }
    }
}