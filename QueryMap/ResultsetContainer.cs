using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryMap
{
    public class ResultsetContainer : SectionContainer
    {
        private List<QueryContainer> _queries = new List<QueryContainer>();
        public List<QueryContainer> Queries
        {
            get
            {
                return _queries;
            }
        }

        public bool QueryContainsUnion()
        {
            foreach (var query in Queries)
            {
                if (query.Queries.Count > 0)
                {
                    if (query.ContainsUnion)
                    {
                        return true;
                    }
                }

            }

            return false;
        }

    }
}
