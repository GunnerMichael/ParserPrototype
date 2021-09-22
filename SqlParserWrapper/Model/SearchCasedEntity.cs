using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class SearchCasedEntity
    {
        private List<WhenClauseEntity> _whenClauses = new List<WhenClauseEntity>();
        public List<WhenClauseEntity> WhenClauses
        {
            get
            {
                return _whenClauses;
            }
        }

        public ExpressionEntity Else { get; set; }
    }

}
