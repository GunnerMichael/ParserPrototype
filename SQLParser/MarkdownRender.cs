using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLParser
{
    public class MarkdownRender : IOutputRender
    {
        private string _path;
        private string _header;

        public MarkdownRender(string path, string header)
        {
            _path = path;
            _header = header;
        }
        public void DoRenderTable(List<ColumnMapEntity> map, Dictionary<string, TableMap> tableMap, string commentHeader, StoredProcedureEntity storedProcedure = null)
        {
            using (var file = File.CreateText(_path))
            {
                file.WriteLine($"# {_header}");
                file.WriteLine();
                foreach (var table in map.Select(x => x.Owner).Distinct().ToList())
                {
                    file.WriteLine($"## {table}");

                    var items = (from m in map
                                 where m.Owner == table
                                 select m).ToList();

                    file.WriteLine();
                    file.WriteLine("|Column|Source or Derivation|");
                    file.WriteLine("|------|--------------------|");
                    foreach (var item in items)
                    {
                        file.WriteLine($"|{item.Column}|{item.Value}|");
                    }

                    file.WriteLine();
                }
            }

        }

        public void DoRender(List<ColumnMapEntity> map, Dictionary<string, TableMap> tableMap, string commentHeader, StoredProcedureEntity storedProcedure = null)
        {
            using (var file = File.CreateText(_path))
            {
                file.WriteLine($"# {_header}");
                file.WriteLine();

                commentHeader = StripCommentMarks(commentHeader);

                if (string.IsNullOrEmpty(commentHeader) == false)
                {
                    file.WriteLine("## Overview");
                    file.WriteLine("```");
                    file.WriteLine(commentHeader);
                    file.WriteLine("```");
                    file.WriteLine();
                }

                if (storedProcedure is not null)
                {
                    file.WriteLine($"The data for this object is built by the {storedProcedure.ProcedureReference.Name.FullName} stored procedure.");
                    file.WriteLine();
                }

                file.WriteLine("[[_TOC_]]");
                file.WriteLine();

                foreach (var table in map.Select(x => x.Owner).Distinct().ToList())
                {
                    file.WriteLine($"## {table}");


                    var items = (from m in map
                                 where m.Owner == table
                                 select m).ToList();

                    file.WriteLine();
                    foreach (var item in items)
                    {
                        file.WriteLine($"### {item.Column}");

                        file.WriteLine($"Source or Derivation");
                        file.WriteLine($"```");
                        file.WriteLine($"{item.Value}");
                        file.WriteLine($"```");

                        file.WriteLine();
                    }


                    TableMap tm = tableMap[table];

                    file.WriteLine("### Criteria");
                    file.WriteLine($"```");
                    file.WriteLine($"{tm.Where}");
                    file.WriteLine($"```");
                    file.WriteLine();

                    if (tm != null && tm.Tables.Count > 0)
                    {
                        file.WriteLine("### Referenced Tables");

                        file.WriteLine("|Table|Alias|");
                        file.WriteLine("|-----|-------|");

                        foreach (var item in tm.Tables)                 
                        {
                            string alias = string.Empty;

                            if (item.TableReference is not null)
                            {
                                if (item.TableReference.NamedTable is not null && item.TableReference.NamedTable.Alias is not null)
                                {
                                    if (item.TableReference.NamedTable.Alias.Identifer is not null)
                                    {
                                        alias = item.TableReference.NamedTable.Alias.Identifer.GetValue();
                                    }
                                }

                                if (item.TableReference.NamedTable is not null)
                                {
                                    file.WriteLine($"|{item.TableReference.NamedTable.SchemaObject.FullName}|{alias}|");
                                }
                            }
                        }
                    }

                    file.WriteLine();

                }
            }

        }

        private string StripCommentMarks(string commentHeader)
        {
            string output = String.Empty;
            string[] lines = commentHeader.Split("\r\n");

            for(int i = 1; i < lines.Count()-1; i++)
            {
                output += lines[i];
                output += "<br/>";
            }


            return output;
        }
    }

}



