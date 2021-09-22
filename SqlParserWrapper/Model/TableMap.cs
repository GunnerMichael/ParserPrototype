using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class TableMap
    {
        private List<TableEntity> _tables = new List<TableEntity>();

        public string Name { get;  set; }
        public List<TableEntity> Tables
        {
            get
            {
                return _tables;
            }
        }

        public string Where { get;  set; }
    }
}
