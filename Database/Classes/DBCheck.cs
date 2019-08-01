using dao;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FAD3
{
    /// <summary>
    /// updates tables, indexes and relations in the data mdb based on contents of template mdb. Uses DAO because ADO will not work.
    /// </summary>
    public static class DBCheck
    {
        public static List<string> TableList;

        public static bool CheckDB(string mdbPath)
        {
            TableList = new List<string>();
            List<string> colList = new List<string>();

            var dbe = new DBEngine();
            var dbTemplate = dbe.OpenDatabase(global.ApplicationPath + "\\template.mdb");
            var dbData = dbe.OpenDatabase(mdbPath);
            var tdName = "";

            foreach (TableDef td in dbData.TableDefs)
            {
                if (td.Name.Substring(0, 4) != "MSys" && td.Name.Substring(0, 5) != "temp_")
                    TableList.Add(td.Name);
            }

            foreach (TableDef tdTemplate in dbTemplate.TableDefs)
            {
                if (tdTemplate.Name.Substring(0, 4) != "MSys" && tdTemplate.Name.Substring(0, 5) != "temp_")
                {
                    tdName = tdTemplate.Name;
                    if (!TableList.Contains(tdTemplate.Name))
                    {
                        var sql = $@"SELECT [{tdTemplate.Name}].* INTO
                                           [{tdTemplate.Name}] IN '{dbData.Name}'
                                            FROM [{tdTemplate.Name}]";
                        var qdf = dbTemplate.CreateQueryDef("", sql);
                        qdf.Execute();
                        dbData.TableDefs.Refresh();
                        qdf.Close();
                    }

                    //enumerate all fields in the current data table
                    foreach (Field f in dbData.TableDefs[tdTemplate.Name].Fields)
                    {
                        colList.Add(f.Name);
                    }

                    //enumerate all fields in the corresponding template table
                    foreach (Field fldTemplate in tdTemplate.Fields)
                    {
                        //if a template field is not found in the data table field
                        //we will add this field to the data table
                        var fldData = new Field();
                        if (!colList.Contains(fldTemplate.Name))
                        {
                            dbData.TableDefs[tdTemplate.Name].With(o =>
                            {
                                fldData = o.CreateField(fldTemplate.Name, fldTemplate.Type, fldTemplate.Size);
                                o.Fields.Append(fldData);
                                o.Fields.Refresh();
                            });
                        }
                        fldData = dbData.TableDefs[tdTemplate.Name].Fields[fldTemplate.Name];
                        fldData.AllowZeroLength = fldTemplate.AllowZeroLength;
                        fldData.Required = fldTemplate.Required;
                        if (fldData.Type != fldTemplate.Type)
                        {
                            FixField(dbData.TableDefs[tdName], fldData, fldTemplate, mdbPath);
                        }
                        foreach (Property pTemplate in fldTemplate.Properties)
                        {
                            if (pTemplate.Name == "Description")
                            {
                                try
                                {
                                    fldData.Properties["Description"].Value = pTemplate.Value;
                                }
                                catch
                                {
                                    fldData.Properties.Append(fldData.CreateProperty(pTemplate.Name, pTemplate.Type, pTemplate.Value));
                                }
                                fldData.Properties.Refresh();
                            }
                        }
                    }
                    colList.Clear();

                    //enumerate all indexes in the current data table and put in the list
                    foreach (Index i in dbData.TableDefs[tdTemplate.Name].Indexes)
                    {
                        var name = tdTemplate.Name;
                        colList.Add(i.Name);

                        if (tdTemplate.Name == "tblGearInventoryBarangayData" && i.Name == "AltKey")
                        {
                            RemoveGearInventoryBarangayDataAltKey(mdbPath);
                        }
                    }

                    //enumerate all indexes in the current template table
                    foreach (Index templateIndex in tdTemplate.Indexes)
                    {
                        //if the current index is not in the list,
                        //we will make one and add it to the indexes of the data table
                        var dataIndex = new Index();
                        if (!colList.Contains(templateIndex.Name))
                        {
                            dbData.TableDefs[tdTemplate.Name].With(o =>
                            {
                                dataIndex = o.CreateIndex(templateIndex.Name);
                                dataIndex.Fields = templateIndex.Fields;

                                dataIndex.Primary = templateIndex.Primary;
                                dataIndex.Required = templateIndex.Primary;
                                dataIndex.IgnoreNulls = templateIndex.IgnoreNulls;
                                dataIndex.Unique = templateIndex.Unique;

                                try
                                {
                                    o.Indexes.Append(dataIndex);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log(ex.Message, "DBCheck.cs", $"CheckDB: Add index {templateIndex.Name} to table { tdTemplate.Name}");
                                }
                                o.Indexes.Refresh();
                            });
                        }
                        else
                        {
                            foreach (Index i in dbData.TableDefs[tdTemplate.Name].Indexes)
                            {
                                if (i.Name == templateIndex.Name)
                                {
                                }
                            }
                        }
                    }
                }
            }

            colList.Clear();
            foreach (Relation rel in dbData.Relations)
            {
                colList.Add(rel.Table + "|" + rel.ForeignTable);
            }

            foreach (Relation templateRel in dbTemplate.Relations)
            {
                var dataRel = new Relation();
                if (!colList.Contains(templateRel.Table + "|" + templateRel.ForeignTable))
                {
                    dataRel = dbData.CreateRelation(templateRel.Name, templateRel.Table, templateRel.ForeignTable);
                    foreach (Field f in templateRel.Fields)
                    {
                        dataRel.Fields.Append(dataRel.CreateField(f.Name));
                        dataRel.Fields[f.Name].ForeignName = templateRel.Fields[f.Name].ForeignName;
                    }

                    try
                    {
                        dbData.Relations.Append(dataRel);
                    }
                    catch
                    {
                        dataRel.Name += "1";
                        try
                        {
                            dbData.Relations.Append(dataRel);
                        }
                        catch (Exception ex)
                        {
                            if (templateRel.ForeignTable == "tblSampling" && templateRel.Table == "tblFishingExpense")
                            {
                                FixSamplingExpenseRelation(mdbPath);
                            }
                            Logger.LogError(ex.Message, ex.StackTrace);
                        }
                    }
                    finally
                    {
                        dbData.Relations.Refresh();
                    }
                }
            }

            dbData.Close();
            dbTemplate.Close();
            dbData = null;
            dbTemplate = null;

            return true;
        }

        private static void FixSamplingExpenseRelation(string mdbPath)
        {
            var dbe = new DBEngine();
            var dbData = dbe.OpenDatabase(mdbPath);
            const string sql = @"ALTER TABLE tblFishingExpense
                                ADD CONSTRAINT FK_SamplingGUID
                                FOREIGN KEY (SamplingGUID) REFERENCES
                                tblSampling (SamplingGUID)";

            try
            {
                dbData.Execute(sql);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex.StackTrace);
            }

            dbData.Close();
            dbData = null;
        }

        private static void RemoveGearInventoryBarangayDataAltKey(string mdbPath)
        {
            var dbe = new DBEngine();
            var dbData = dbe.OpenDatabase(mdbPath);
            var sql = "DROP INDEX AltKey ON tblGearInventoryBarangayData";

            try
            {
                dbData.Execute(sql);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            }

            dbData.Close();
            dbData = null;
        }

        private static void FixField(TableDef td, Field fData, Field fTemplate, string mdbPath)
        {
            var newType = "";
            switch (fTemplate.Type)
            {
                case (short)DataTypeEnum.dbInteger:
                    newType = "SMALLINT";
                    break;

                case (short)DataTypeEnum.dbLong:
                    newType = "INTEGER";
                    break;

                case (short)DataTypeEnum.dbText:
                    newType = "TEXT";
                    break;

                case (short)DataTypeEnum.dbDouble:
                    newType = "DOUBLE";
                    break;
            }
            var dbe = new DBEngine();
            var dbData = dbe.OpenDatabase(mdbPath);
            var sql = $"ALTER TABLE {td.Name} ALTER COLUMN {fData.Name} {newType}";

            dbData.Execute(sql);

            dbData.Close();
            dbData = null;
        }
    }
}