using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapWinGIS;
using System.Windows.Media;

namespace FAD3
{
    public partial class PointLayerSymbologyForm : Form
    {
        private static PointLayerSymbologyForm _instance;
        private MapLayer _mapLayer;
        private Shapefile _shapeFile;
        private ShpfileType _shpFileType;

        public static PointLayerSymbologyForm GetInstance(MapLayer mapLayer)
        {
            if (_instance == null) return new PointLayerSymbologyForm(mapLayer);
            return _instance;
        }

        public PointLayerSymbologyForm(MapLayer mapLayer)
        {
            InitializeComponent();
            _mapLayer = mapLayer;
            _shapeFile = mapLayer.LayerObject as Shapefile;
            _shpFileType = _shapeFile.ShapefileType;
        }

        private void ShowCharacterMap(string fontName)
        {
            characterControl1.SetFontName(fontName);
        }

        private void LoadCharacterSymbols()
        {
            foreach (System.Drawing.FontFamily family in System.Drawing.FontFamily.Families)
            {
                string name = family.Name.ToLower();

                if (name == "webdings" ||
                    name == "wingdings" ||
                    name == "wingdings 2" ||
                    name == "wingdings 3")
                {
                    comboCharacterFont.Items.Add(family.Name);
                    comboCharacterFont.SelectedIndex = 0;
                }
            }

            ShowCharacterMap(comboCharacterFont.Text);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            global.LoadFormSettings(this, true);
            LoadCharacterSymbols();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            global.SaveFormSettings(this);
            _instance = null;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    break;

                case "btnCancel":
                    Close();
                    break;
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!_noEvents)
            //{
            //    btnApply.Enabled = true;
            //}

            characterControl1.SetFontName(comboCharacterFont.Text);
        }
    }
}