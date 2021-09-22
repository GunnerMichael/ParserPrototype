using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryMap
{
    public class QueryContainer : SectionContainer
    {
        private List<TableContainer> _tables = new List<TableContainer>();
        private List<ColumnContainer> _columns = new List<ColumnContainer>();
        private List<QueryContainer> _queries = new List<QueryContainer>();

        public List<TableContainer> Tables
        {
            get
            {
                return _tables;
            }
        }

        public List<ColumnContainer> SelectColumns
        {
            get
            {
                return _columns;
            }
        }

        public List<QueryContainer> Queries
        {
            get
            {
                return _queries;
            }
        }

        public FromClauseContainer FromClause { get; set; }
        public string Name { get; internal set; }
        public WhereClauseContainer WhereClause { get; set; }

        public bool ContainsUnion { get; set; }
    }
}
