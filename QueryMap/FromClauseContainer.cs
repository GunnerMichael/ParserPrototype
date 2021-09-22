using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryMap
{
    public class FromClauseContainer
    {
        private List<TableContainer> _tables = new List<TableContainer>();

        private List<JoinMapItem> _joinMap = new List<JoinMapItem>();

        public List<TableContainer> Tables
        {
            get
            {
                return _tables;
            }
        }

        public string JoinQuery { get; internal set; }
        public List<JoinMapItem> JoinMapItem
        {
            get
            {
                return _joinMap;
            }
        }
    }
}
