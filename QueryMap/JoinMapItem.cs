using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryMap
{
    public class JoinMapItem
    {
        public ColumnContainer ColumnReference { get; internal set; }
        public JoinMapItem First { get; internal set; }
        public JoinMapItem Second { get; internal set; }
        public JoinMapItem JoinMap { get; internal set; }
    }
}
