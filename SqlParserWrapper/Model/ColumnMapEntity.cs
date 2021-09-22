using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class ColumnMapEntity
    {
        public string Column { get; set; }

        public string Value { get; set; }

        public string Owner { get; set; }

        public string CTE { get; set; }

        public ColumnMapEntity SourceColumn { get; set; }
    }
}
