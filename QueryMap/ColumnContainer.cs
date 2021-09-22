using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryMap
{
    public class ColumnContainer
    {
        public string Name { get; internal set; }
        public string Value { get; internal set; }

        public string SourceObject { get; set; }

        public string SourceAlias { get; set; }
    }
}
