
using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlParserWrapper.Model
{
    public class ColumnMappingEntity : MapBase
    {
        public List<ColumnMapEntity> GetColumnMap(List<QueryExpressionEntity> source, List<CommonTableEntity> ctes)
        {
            List<ColumnMapEntity> items = new List<ColumnMapEntity>();

            foreach(var item in ctes)
            {
                var cols = GetColumns(item.Name, item.QueryExpression);

                if (cols != null)
                {

                    items.AddRange(cols);
                }
            }

            int resultCount = 1;

            foreach (var item in source)
            {
                items.AddRange(GetColumns(item.QuerySpecification.Name, item, sourceCteList: ctes));
                resultCount++;
            }


            return items;
        }

        private List<ColumnMapEntity> GetColumns(string name, QueryExpressionEntity source, List<CommonTableEntity> sourceCteList = null)
        {
            if (source == null)
            {
                return null;
            }

            if (sourceCteList == null)
            {
                sourceCteList = new List<CommonTableEntity>();
            }

            string owner = name;
            List<ColumnMapEntity> map = new List<ColumnMapEntity>();

            //Dictionary<string, TableEntity> tables = new Dictionary<string, TableEntity>();

            //QuerySpecificationEntity qs = null;

            //if (source.QuerySpecification is not null)
            //{
            //    qs = source.QuerySpecification;
            //}
            //else if (source.BinaryQuery is not null)
            //{
            //    qs = source.BinaryQuery.SecondQuery.QuerySpecification;
            //}
            //if (qs is not null)
            //{ 
            //    var t = qs.FromClause.GetUniqueTables();

            //    if (t != null)
            //    {
            //        foreach (var x in t)
            //        {
            //            if (x.TableReference is not null)
            //            {
            //                var usedCte = (from c in sourceCteList
            //                               where x.TableReference.NamedTable != null && c.Name == x.TableReference.NamedTable.SchemaObject.FullName
            //                               select c).ToList();

            //                if (usedCte.Count > 0)
            //                {
            //                    x.TableReference.IsCte = true;
            //                    x.TableReference.CteName = usedCte[0].Name;
            //                }

            //                if (x.TableReference.NamedTable.Alias.Identifer is not null)
            //                {
            //                    if (tables.ContainsKey(x.TableReference.NamedTable.Alias.Identifer.Value.ToLower()) == false)
            //                    {
            //                        tables.Add(x.TableReference.NamedTable.Alias.Identifer.Value.ToLower(), x);
            //                    }
            //                }
            //                else
            //                {
            //                    string identifer = x.TableReference.NamedTable.SchemaObject.FullName.ToLower();

            //                    if (tables.ContainsKey(identifer) == false)
            //                    {
            //                        tables.Add(identifer, x);
            //                    }

            //                }
            //            }
            //        }

            //        _tables = tables;

            //        foreach (var col in qs.Columns)
            //        {
            //            if (string.IsNullOrEmpty(col.TableAlias) == false)
            //            {
            //                if (tables.ContainsKey(col.TableAlias.ToLower()))
            //                {
            //                    var aliasTable = tables[col.TableAlias.ToLower()];

            //                    if (aliasTable.TableReference.IsCte)
            //                    {
            //                        var nonAliasColumn = col.Value.Replace(col.TableAlias, aliasTable.TableReference.NamedTable.SchemaObject.FullName);

            //                        map.Add(new ColumnMapEntity() { Column = col.ColumnName, Owner = owner, Value = nonAliasColumn, CTE = aliasTable.TableReference.CteName });

            //                    }
            //                    else
            //                    {
            //                        var nonAliasColumn = col.Value.Replace(col.TableAlias, aliasTable.TableReference.NamedTable.SchemaObject.FullName);

            //                        map.Add(new ColumnMapEntity() { Column = col.ColumnName, Owner = owner, Value = nonAliasColumn });
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                if (col.Value is not null)
            //                {
            //                    map.Add(new ColumnMapEntity() { Column = col.ColumnName, Owner = owner, Value = col.Value });
            //                }
            //                else
            //                {
            //                    string val = BuildOutput(col.Expression);
            //                    map.Add(new ColumnMapEntity() { Column = col.ColumnName, Owner = owner, Value = val });
            //                }

            //                //else if (col.FunctionCall is not null)
            //                //{
            //                //    string val = GetFunctionVal(col.FunctionCall);

            //                //    map.Add(new ColumnMapEntity() { Column = col.ColumnName, Owner = owner, Value = val });
            //                //}
            //                //else if (col.SearchedCase is not null)
            //                //{
            //                //    string val = GetSearchedCaseVal(col.SearchedCase);
            //                //    map.Add(new ColumnMapEntity() { Column = col.ColumnName, Owner = owner, Value = val });

            //                //}
            //                //else if (col.IIfCall is not null)
            //                //{
            //                //    map.Add(new ColumnMapEntity() { Column = col.ColumnName, Owner = owner, Value = BuildIIf(col.IIfCall) });
            //                //}
            //                //else if (col.BinaryExpression is not null)
            //                //{
            //                //    map.Add(new ColumnMapEntity() { Column = col.ColumnName, Owner = owner, Value = GetBinaryExpression(col.BinaryExpression) });
            //                //}
            //                //else if (col.LeftFunctionCall is not null)
            //                //{
            //                //    map.Add(new ColumnMapEntity() { Column = col.ColumnName, Owner = owner, Value = GetLeftFunction(col.LeftFunctionCall) });
            //                //}
            //                //else if (col.RightFunctionCall is not null)
            //                //{
            //                //    map.Add(new ColumnMapEntity() { Column = col.ColumnName, Owner = owner, Value = GetRightFunction(col.RightFunctionCall) });
            //                //}
            //                //else if (col.CastCall is not null)
            //                //{
            //                //    map.Add(new ColumnMapEntity() { Column = col.ColumnName, Owner = owner, Value = BuildCastCall(source: col.CastCall) });
            //                //}
            //                //else
            //                //{
            //                //    map.Add(new ColumnMapEntity() { Column = col.ColumnName, Owner = owner, Value = col.Value });
            //                //}
            //            }
            //        }
            //    }
            //}
            return map;

        }


        public List<string> GetMappings
        {
            get
            {
                return mappings;
            }
        }
    }
}
