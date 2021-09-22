using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class CommonTableEntity : TableEntity
    {
        public string Name { get; set; }

        public QueryExpressionEntity QueryExpression { get; set; }

    }
}
