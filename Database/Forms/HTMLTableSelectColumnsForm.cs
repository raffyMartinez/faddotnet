﻿using FAD3.Database.Classes;
using HtmlAgilityPack;
using System;
using System.Windows.Forms;

namespace FAD3.Database.Forms
{
    public partial class HTMLTableSelectColumnsForm : Form
    {
        private string _htmlFile;
        private CatchNameDataType _catchNameDataType;
        private int _resultImport;
        public int SpeciesNameColumn { get; internal set; }
        public int LanguageColumn { get; internal set; }
        public int LocalNameColumn { get; internal set; }

        public int RowsImported { get; internal set; }

        public HTMLTableSelectColumnsForm(string htmlFile, CatchNameDataType catchNameDataType)
        {
            InitializeComponent();
            _htmlFile = htmlFile;
            _catchNameDataType = catchNameDataType;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            switch (_catchNameDataType)
            {
                case CatchNameDataType.CatchLocalName:
                    cboLocalName.Enabled = true;
                    lblLocalName.Enabled = true;
                    break;

                case CatchNameDataType.CatchSpeciesLocalNamePair:
                    cboLocalName.Enabled = true;
                    lblLocalName.Enabled = true;
                    cboSpName.Enabled = true;
                    lblSpName.Enabled = true;
                    cboLanguage.Enabled = true;
                    lblLanguage.Enabled = true;
                    break;

                case CatchNameDataType.CatchSpeciesName:
                    cboSpName.Enabled = true;
                    lblSpName.Enabled = true;
                    break;
            }

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(_htmlFile);
            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                foreach (HtmlNode header in table.SelectNodes("thead"))
                {
                    foreach (HtmlNode row in header.SelectNodes("tr"))
                    {
                        foreach (HtmlNode cell in row.SelectNodes("th|td"))
                        {
                            switch (_catchNameDataType)
                            {
                                case CatchNameDataType.CatchLocalName:
                                    cboLocalName.Items.Add(cell.InnerText);
                                    break;

                                case CatchNameDataType.CatchSpeciesLocalNamePair:
                                    cboSpName.Items.Add(cell.InnerText);
                                    cboLocalName.Items.Add(cell.InnerText);
                                    cboLanguage.Items.Add(cell.InnerText);
                                    break;

                                case CatchNameDataType.CatchSpeciesName:
                                    cboSpName.Items.Add(cell.InnerText);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnOk":
                    string msg = string.Empty;
                    switch (_catchNameDataType)
                    {
                        case CatchNameDataType.CatchLocalName:
                            if (cboLocalName.Text.Length > 0)
                            {
                                LocalNameColumn = cboLocalName.SelectedIndex;
                            }
                            else
                            {
                                msg = "Please select column of local name";
                            }
                            break;

                        case CatchNameDataType.CatchSpeciesLocalNamePair:
                            if (cboLocalName.Text.Length > 0
                                && cboLanguage.Text.Length > 0
                                && cboSpName.Text.Length > 0)
                            {
                                LocalNameColumn = cboLocalName.SelectedIndex;
                                SpeciesNameColumn = cboSpName.SelectedIndex;
                                LanguageColumn = cboLanguage.SelectedIndex;
                            }
                            else
                            {
                                msg = "Please select columns of species name, local name, and language";
                            }
                            break;

                        case CatchNameDataType.CatchSpeciesName:
                            if (cboSpName.Text.Length > 0)
                            {
                                SpeciesNameColumn = cboSpName.SelectedIndex;
                            }
                            else
                            {
                                msg = "Please select column of species name";
                            }
                            break;
                    }
                    if (msg.Length > 0)
                    {
                        MessageBox.Show(msg, "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        DialogResult = DialogResult.OK;
                    }

                    break;

                case "btnCancel":
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }
    }
}