using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class TableReferenceEntity : BaseTable
    {
        //private IdentiferEntity _alias = new IdentiferEntity();

        //public IdentiferEntity Alias
        //{
        //    get
        //    {
        //        return  _alias;
        //    }
        //}

        public SchemaObjectEntity SchemaObject { get; set;}
        public VariableTableEntity VariableTable { get; set;}
        public ReferenceTableEntity NamedTable { get; set;}
        internal QueryDerivedTableEntity QueryDerivedTable { get; set; }
    }
}
