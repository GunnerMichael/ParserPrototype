using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLParser
{
    public class CsvRender : IOutputRender
    {
        private string _filename;

        public CsvRender(string filename)
        {
            _filename = filename;
        }
        public void DoRender(List<ColumnMapEntity> map, Dictionary<string,TableMap> tableMap, string commentHeader, StoredProcedureEntity storedProcedure = null)
        {
            using (var file = File.CreateText(_filename))
            {
                file.WriteLine("Table,Column,Value");
                foreach (var item in map)
                {
                    file.WriteLine($"{item.Owner},{item.Column},{item.Value}");
                }
            }
        }
    }
}

