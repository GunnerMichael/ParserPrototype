using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class TableEntity
    {
        public TableReferenceEntity TableReference { get; set; }
        public SchemaObjectFunctionTableEntity SchemaObjectFunctionTable { get; set; }

        public QualifiedJoinEntity QualifiedJoin { get; set; }
        public QueryDerivedTableEntity QueryDerivedTable { get; set; }

        public string GetName()
        {
            string name = String.Empty;

            if (TableReference != null && TableReference.NamedTable != null)
            {
                name = TableReference.NamedTable.SchemaObject.FullName;
            }
            else
            {

            }

            return name;
        }
    }
}
