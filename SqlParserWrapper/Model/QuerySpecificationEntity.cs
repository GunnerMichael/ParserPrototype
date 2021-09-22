using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class QuerySpecificationEntity
    {
        private List<SelectColumnEntity> _columns = new List<SelectColumnEntity>();

        public List<SelectColumnEntity> Columns
        {
            get
            {
                return _columns;
            }
        }

        public FromClauseEntity FromClause { get; set;}
        public WhereClauseEntity WhereClause { get; set; }

        public string Name { get; set; }

    }
}
