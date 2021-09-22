using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryMap
{
    public class TableContainer
    {
        private List<IdentifierContainer> _identifers = new List<IdentifierContainer>();

        public SchemaObjectFunctionTableEntity SchemaObjectFunctionTable { get; internal set; }
        public AliasEntity Alias { get; internal set; }
        public string FullName { get; internal set; }
        public List<IdentifierContainer> Identifers
        {
            get
            {
                return _identifers;
            }
        }
    }
}
