using QueryMap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace SqlDocumentRender
{
    public class ColumnMappingMarkdownRender
    {
        public void CreateOutput(QueryMapContainer container, string path, string header, bool tableFormatColumns = true)
        {
            string output = JsonConvert.SerializeObject(container);

            using(var j = File.CreateText(@"c:\\michaelxx\query.json"))
            {
                j.WriteLine(output);
            }

            using (var file = File.CreateText(path))
            {
                RenderHeader(container, file);
                RenderOverview(container, file);
                RenderCteSection(container,file, tableFormatColumns);
                RenderResultset(container, file, tableFormatColumns);
            }
        }

        private void RenderOverview(QueryMapContainer container, StreamWriter file)
        {
            var items = (from x in container.Sections
                         where x.GetType() == typeof(OverviewSection)
                         select x).ToList();

            if (items.Count > 0)
            {
                file.WriteLine("## Overview");
                file.WriteLine();

                file.WriteLine("```");

                foreach(OverviewSection item in items)
                {
                    file.WriteLine(StripCommentMarks(item.Overview));
                }

                file.WriteLine("```");

                file.WriteLine();
            }
            
        }

        private void RenderHeader(QueryMapContainer container, StreamWriter file)
        {
            var items = (from x in container.Sections
                         where x.GetType() == typeof(HeaderSection)
                         select x).ToList();

            if (items.Count > 0)
            {
                file.WriteLine($"# {((HeaderSection)items[0]).Title}");
                file.WriteLine();
            }

            file.WriteLine("[[_TOC_]]");
            file.WriteLine();
        }

        private void RenderResultset(QueryMapContainer container, StreamWriter file, bool tableFormatColumns)
        {
            var items = (from x in container.Sections
                         where x.GetType() == typeof(ResultsetContainer)
                         select x).ToList();


            foreach (var item in items)
            {
                var rs = item as ResultsetContainer;

                Debug.WriteLine(rs.QueryContainsUnion());

                RenderQuerySections(rs.Queries, file, tableFormatColumns);
            }

        }

        private void RenderCteSection(QueryMapContainer container, StreamWriter file, bool tableFormatColumns)
        {
            var items = (from x in container.Sections
                         where x.GetType() == typeof(CteContainer)
                         select x).ToList();

            if (items.Count > 0)
            {
                file.WriteLine();
                file.WriteLine("## CTEs");
            }

            foreach (var item in items)
            {
                var cte = item as CteContainer;

                file.WriteLine($"### {cte.CteName}");
                Debug.WriteLine(cte.QueryContainsUnion());
                file.WriteLine();

                RenderQuerySections(cte.Queries, file, tableFormatColumns);

                file.WriteLine("***");
            }
        }

        public string CreateOutputToString(QueryMapContainer output, bool tableFormatColumns)
        {
            string jout = JsonConvert.SerializeObject(output);

            using (var j = File.CreateText(@"c:\\michaelxx\query.json"))
            {
                j.WriteLine(jout);
            }

            var memStream = new MemoryStream();
            using (StreamWriter file = new StreamWriter(memStream))
            {
                RenderHeader(output, file);
                RenderOverview(output, file);
                RenderCteSection(output, file, tableFormatColumns);
                RenderResultset(output, file, tableFormatColumns);

                file.Flush();
                memStream.Position = 0;
                return new StreamReader(memStream).ReadToEnd();
            }
        }

        private void RenderQuerySections(List<QueryContainer> queries, StreamWriter file, bool tableFormatColumns, bool isUnion = false)
        {
            foreach(var query in queries)
            {
                if (query.Queries.Count > 0)
                {
                    if (query is UnionContainer)
                    {
                        isUnion = true;    
                    }
                    else
                    {
                        isUnion = false;
                    }

                    RenderQuerySections(query.Queries, file, tableFormatColumns, isUnion);
                }

                if (query.SelectColumns.Count > 0)
                {
                    if (string.IsNullOrEmpty(query.Name) == false)
                    {
                        file.WriteLine($"### {query.Name}");                  

                    }
                    else
                    {

                    }

                    RenderSourceAndDerivation(query, file, tableFormatColumns);
                    RenderFromClause(query.FromClause, file);
                    RenderWhereClause(query.WhereClause, file);

                }
                else
                {

                }

            }
        }

        private void RenderSourceAndDerivation(QueryContainer query, StreamWriter file, bool tableFormatColumns)
        {
            if (tableFormatColumns)
            {
                RenderSourceAndDerivation(query, file);
            }
            else
            {
                RenderSourceAndDerivationNoTable(query, file);
            }
        }

        private void RenderWhereClause(WhereClauseContainer whereClause, StreamWriter file)
        {
            file.WriteLine("Criteria");
            file.WriteLine($"```");
            if (whereClause is not null && string.IsNullOrEmpty(whereClause.WhereText) == false)
            {
                file.WriteLine($"{whereClause.WhereText}");
            }
            else
            {
                file.WriteLine("There is no criteria specified to filter these results");
            }
            file.WriteLine($"```");
            file.WriteLine();
        }

        private void RenderSourceAndDerivationNoTable(QueryContainer query, StreamWriter file)
        {
            file.WriteLine("### Column / Source or Derivation");
            file.WriteLine("");
            file.WriteLine("***");

            foreach (var item in query.SelectColumns)
            {
                file.WriteLine($"#### {item.Name}");

                //file.WriteLine($"Source or Derivation");
                file.WriteLine($"```");
                file.WriteLine($"{GetColumnValue(item)}");
                file.WriteLine($"```");
                file.WriteLine();

            }
            file.WriteLine("***");
        }


        private void RenderSourceAndDerivation(QueryContainer query, StreamWriter file)
        {
            file.WriteLine();
            file.WriteLine("Column Definitions");
            file.WriteLine();
            file.WriteLine("|Column|Source or Derivation|");
            file.WriteLine("|---|---|");

            foreach (var item in query.SelectColumns)
            {
                string sOrD = GetColumnValue(item).Replace(Environment.NewLine, "<br/>");
                file.WriteLine($"|{item.Name}|{sOrD}|");
            }

            file.WriteLine();
               
        }


        private void RenderFromClause(FromClauseContainer fromClause, StreamWriter file)
        {
            if (fromClause is null)
            {
                return;
            }


            if (fromClause.Tables.Count > 0)
            {
                file.WriteLine("Referenced Tables");
                file.WriteLine("");
                file.WriteLine("|Table|Alias|");
                file.WriteLine("|-----|-------|");

                foreach (var item in fromClause.Tables)
                {
                    string alias = string.Empty;

                    if (item.Alias is not null && item.Alias.Identifer is not null)
                    {
                        alias = item.Alias.Identifer.GetValue();
                    }
                    string table = item.FullName;

                    file.WriteLine($"|{table}|{alias}|");

                }

                file.WriteLine();

                file.WriteLine("Joins");
                file.WriteLine();

                if (string.IsNullOrEmpty(fromClause.JoinQuery.Trim()))
                {
                    file.WriteLine("There are no joined tables");
                }
                else
                {
                    file.WriteLine("```sql");
                    file.WriteLine(fromClause.JoinQuery);
                    file.WriteLine("```");
                }
                file.WriteLine();


                MermaidDiagram(fromClause, file);




            }
        }

        private void MermaidDiagram(FromClauseContainer fromClause, StreamWriter file)
        {
            string[] joinLines = fromClause.JoinQuery.Split("\r\n");

            List<string> mermaidJoins = BuildJoin(joinLines, fromClause);


            if (fromClause.Tables.Count > 0)
            {
                file.WriteLine(":::mermaid");

                // either left-to-right or top-down depending on number of tables
                if (fromClause.Tables.Count > 4)
                {
                    file.WriteLine("  graph LR;");
                }
                else
                {
                    file.WriteLine("  graph TD;");
                }

                foreach (var item in fromClause.Tables)
                {
                    string alias = string.Empty;

                    if (item.Alias is not null && item.Alias.Identifer is not null)
                    {
                        alias = item.Alias.Identifer.GetValue();
                    }
                    string table = item.FullName;

                    file.WriteLine($"{table}:{alias}".Replace("#", string.Empty));
                }

                var unique = mermaidJoins
                    .GroupBy(t => t)
                    .Select(g => g.FirstOrDefault())
                    .ToList();


                if (unique.Count > 0)
                {
                    foreach(var item in unique)
                    {
                        // remove # from temporary table has mermaid doesn't like this.
                        file.WriteLine(item.Replace("#",string.Empty));
                    }
                }

                file.WriteLine(":::");

                file.WriteLine();

            }
        }

        private List<string> BuildJoin(string[] joinLines, FromClauseContainer fromClause)
        {
            List<string> joins = new List<string>();

            foreach (var j in joinLines)
            {
                if (string.IsNullOrEmpty(j) == false)
                {
                    joins.Add(BuildJoin(line: j, fromClause));
                }
            }


            return joins;
        }

        private string BuildJoin(string line, FromClauseContainer fromClause)
        {
            string output = String.Empty;

            string[] items = line.Split(' ');

            bool multiCondition = false;

            if (items.Count() > 3)
            {
                multiCondition = true;
            }

            string id = string.Empty;

            bool firstTable = true;

            string first = String.Empty;
            string second = String.Empty;

            foreach (var item in items)
            {

                TableContainer table = null;

                if (item.LastIndexOf(".") > 0)
                {
                    table = LookupAlias(fromClause, item.Substring(0, item.LastIndexOf(".")));

                    if (table != null && firstTable)
                    {
                        first = GetMermaidId(table);
                    }
                    else if (table != null && !firstTable)
                    {
                        second = GetMermaidId(table);
                    }

                    id += item.Substring(item.LastIndexOf(".") + 1);
                    id += "/";

                    if (firstTable == true)
                    {
                        firstTable = false;
                    }
                    else
                    {
                        id = id.Trim('/');
                        output += $" {first}--> |{id}| {second} ";

                        if (multiCondition)
                        {
                            output += "\r\n\t";
                        }
                        firstTable = true;
                        id = String.Empty;
                    }

                }
            }


            return output;
        }

        private string GetMermaidId(TableContainer item)
        {
            string alias = string.Empty;

            if (item.Alias is not null && item.Alias.Identifer is not null)
            {
                alias = item.Alias.Identifer.GetValue();
            }
            string table = item.FullName;

            return ($"{table}:{alias}");
        }

        private TableContainer LookupAlias(FromClauseContainer fromClause, string fullname)
        {
            TableContainer output = null;

            var x = (from t in fromClause.Tables
                    where t.FullName.ToLower() == fullname.ToLower() || t.Alias.AliasName.ToLower() == fullname.ToLower()
                    select t).ToList();

            if (x.Count == 1)
            {
                return x[0];
            }

            return output;
        }

        //private void MermaidDiagram(List<JoinMapItem> joinMapItem, StreamWriter file)
        //{
        //    var tables = GetTablesFromJoinMap(joinMapItem)
        //        .GroupBy(t => t)
        //        .Select(g => g.FirstOrDefault())                
        //        .ToList();

        //    if (tables.Count > 0)
        //    {
        //        file.WriteLine("```mermaid");
        //        file.WriteLine("  graph TD;");
        //        foreach(var item in tables)
        //        {
        //            file.WriteLine($"  {item}");
        //        }
        //        foreach(var item in GetJoinsFromJoinMap(joinMapItem))
        //        {
        //            file.WriteLine($"  {item}");
        //        }
        //        file.WriteLine("```");
        //        file.WriteLine();

        //    }

        //    //            ``` mermaid
        //    //  graph TD;
        //    //            _CreditScore_XML.OCONTROL
        //    //  _CreditScore_XML.ApplicationData
        //    //  CreditScore_XML.OCONTROL-- > | fk_postal | _CreditScore_XML.ApplicationData
        //    //```

        //}

        //private List<string> GetTablesFromJoinMap(List<JoinMapItem> joinMapItem)
        //{
        //    List<string> table = new List<string>();

        //    foreach(var item in joinMapItem)
        //    {
        //        table.AddRange(GetTableFromJoinMap(item));
        //    }

        //    return table;
        //}

        //private List<string> GetTableFromJoinMap(JoinMapItem joinMapItem)
        //{
        //    List<string> table = new List<string>();

        //    if (joinMapItem == null)
        //        return table;

        //    if (joinMapItem.First != null)
        //    {
        //        table.AddRange(GetTableFromJoinMap(joinMapItem.First));
        //    }

        //    if (joinMapItem.Second != null)
        //    {
        //        table.AddRange(GetTableFromJoinMap(joinMapItem.Second));
        //    }

        //    if (joinMapItem.JoinMap != null)
        //    {
        //        table.AddRange(GetTableFromJoinMap(joinMapItem.JoinMap));
        //    }

        //    if (joinMapItem.ColumnReference != null)
        //    {
        //        table.Add($"{joinMapItem.ColumnReference.SourceObject}:{joinMapItem.ColumnReference.SourceAlias}");
        //    }


        //    return table;
        //}


        //private List<string> GetJoinsFromJoinMap(List<JoinMapItem> joinMapItem)
        //{
        //    List<string> table = new List<string>();

        //    foreach (var item in joinMapItem)
        //    {
        //        table.AddRange(GetJoinsFromJoinMap(item));
        //    }

        //    return table;
        //}

        //private string GetJoinTable(JoinMapItem jm)
        //{
        //    if (jm.JoinMap != null)
        //    {
        //        return GetJoinTable(jm.JoinMap);
        //    }

        //    if (jm.First != null)
        //    {
        //        return GetJoinTable(jm.First);
        //    }

        //    if (jm.Second != null)
        //    {
        //        return GetJoinTable(jm.Second);
        //    }

        //    if (jm.ColumnReference != null)
        //    {
        //        return $"{jm.ColumnReference.SourceObject}:{ jm.ColumnReference.SourceAlias}";
        //    }

        //    return string.Empty;
        //}

        //private List<string> GetJoinsFromJoinMap(JoinMapItem joinMapItem)
        //{
        //    List<string> table = new List<string>();

        //    if (joinMapItem == null)
        //        return table;

        //    if (joinMapItem.First != null && joinMapItem.Second != null)
        //    {
        //        string map = $"{GetJoinTable(joinMapItem.First)}-->{GetJoinTable(joinMapItem.Second)}";
        //        table.Add(map);
        //    }

        //    if (joinMapItem.JoinMap != null)
        //    {
        //        table.AddRange(GetJoinsFromJoinMap(joinMapItem.JoinMap));
        //    }

        //    //if (joinMapItem.ColumnReference != null)
        //    //{
        //    //    table.Add($"{joinMapItem.ColumnReference.SourceObject}:{joinMapItem.ColumnReference.SourceAlias}");
        //    //}


        //    return table;
        //}


        private string StripCommentMarks(string commentHeader)
        {
            string output = String.Empty;
            string[] lines = commentHeader.Split("\r\n");

            for (int i = 1; i < lines.Count() - 1; i++)
            {
                output += lines[i];
                output += "\r\n";
            }

            return output;
        }

        private string GetColumnValue(ColumnContainer item)
        {
            return item.Value;
        }
    }
}
