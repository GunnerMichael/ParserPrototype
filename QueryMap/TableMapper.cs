using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;

using System.Linq;

namespace SQLParser
{
    public class TableMapper : MapBase
    {

        public TableMapper()
        {
        }

        public Dictionary<string, TableMap> MapTables(List<QueryExpressionEntity> qe, List<CommonTableEntity> cte)
        {
//            QueryLogger(qe, cte);

            Dictionary<string,TableMap> map = new Dictionary<string, TableMap>();

            foreach (var item in cte)
            {
                if (item.QueryExpression is not null)
                {
                    map.Add(item.Name, BuildMap(item.QueryExpression, item.Name, cte));
                }

            }



            int resultCount = 1;

            foreach (var item in qe)
            {
                if (item.QuerySpecification is not null)
                {
                    map.Add(item.QuerySpecification.Name, BuildMap(item, item.QuerySpecification.Name, cte));
                }

                resultCount++;                   
            }

            return map;
        }


        private TableMap BuildMap(QueryExpressionEntity qe, string name, List<CommonTableEntity> sourceCteList = null)
        {
            TableMap map = new TableMap();
            map.Name = name;

            if (qe is null)
            {
                return map;
            }

            Dictionary<string, TableEntity> tables = new Dictionary<string, TableEntity>();

            if (qe.QuerySpecification is not null)
            {
                var t = qe.QuerySpecification.FromClause.GetUniqueTables();

                if (t != null)
                {
                    foreach (var x in t)
                    {
                        var usedCte = (from c in sourceCteList
                                       where x.TableReference.NamedTable != null && c.Name == x.TableReference.NamedTable.SchemaObject.FullName
                                       select c).ToList();

                        if (usedCte.Count > 0)
                        {
                            x.TableReference.IsCte = true;
                            x.TableReference.CteName = usedCte[0].Name;
                        }

                        if (x.TableReference.NamedTable.Alias.Identifer is not null)
                        {
                            if (tables.ContainsKey(x.TableReference.NamedTable.Alias.Identifer.Value.ToLower()) == false)
                            {
                                tables.Add(x.TableReference.NamedTable.Alias.Identifer.Value.ToLower(), x);
                            }
                        }
                    }
                }

                _tables = tables;
                if (qe.QuerySpecification.FromClause.Tables != null && qe.QuerySpecification.FromClause.Tables.Count > 0)
                {
                    map.Tables.AddRange(qe.QuerySpecification.FromClause.Tables);
                }
                else if (qe.QuerySpecification.FromClause.QualifiedJoin != null)
                {
                    var join = qe.QuerySpecification.FromClause.QualifiedJoin;

                    if (join.First != null)
                    {
                        map.Tables.Add(join.First);
                    }

                    if (join.Second != null)
                    {
                        map.Tables.Add(join.Second);
                    }

                }

                if (qe.QuerySpecification.WhereClause != null)
                {
                    string where = GetWhereClause(qe.QuerySpecification.WhereClause);

                    if (string.IsNullOrEmpty(where) == false)
                        map.Where = $"WHERE {where}";
                    else
                        map.Where = "No selection criteria";
                }
            }
            else
            {
                map.Tables.AddRange(GetReferencedTables(qe.BinaryQuery));
            }
            
            return map;
        }

        private IEnumerable<TableEntity> GetReferencedTables(BinaryQueryEntity binaryQuery)
        {
            List<TableEntity> tables = new List<TableEntity>();

            if (binaryQuery.SecondQuery.QuerySpecification is not null)
            {
                tables.AddRange(GetReferencedTables(binaryQuery.SecondQuery.QuerySpecification));
            }

            if (binaryQuery.FirstQuery.BinaryQuery is not null)
            {
                tables.AddRange(GetReferencedTables(binaryQuery.FirstQuery.BinaryQuery));
            }

            var u = GetUniqueTables(tables);


            return tables;
        }

        private IEnumerable<TableEntity> GetReferencedTables(QuerySpecificationEntity qe)
        {
            List<TableEntity> tables = new List<TableEntity>();

            tables.AddRange(qe.FromClause.GetUniqueTables());

            return tables;
        }

        private IEnumerable<TableEntity> GetUniqueTables(List<TableEntity> tables)
        {
            var remove = (from t in tables
                          where t.TableReference.NamedTable == null
                          select t).ToList();

            foreach (var item in remove)
                tables.Remove(item);

            List<TableEntity> distinctTables = tables
                .GroupBy(t => new { t.TableReference.NamedTable.SchemaObject.FullName, t.TableReference.NamedTable.Alias.AliasName })
                .Select(g => g.FirstOrDefault())
                .ToList();

            return distinctTables;

        }

        private string GetWhereClause(WhereClauseEntity source)
        {
            string output = string.Empty;

            if (source.SearchCondition is not null)
            {
                output += BuildOutput(source: source.SearchCondition);
            }

            return output;
        }
    }
}