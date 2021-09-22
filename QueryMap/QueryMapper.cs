using SqlParserWrapper;
using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLParser
{
    public class QueryMapper : SqlParserWrapper.Contract.IQueryProcesser
    {
        public QueryMapper()
        {
        }

        private List<QueryExpressionEntity> _queries = new List<QueryExpressionEntity>();
        private List<CommonTableEntity> _cte = new List<CommonTableEntity>();

        private StoredProcedureEntity _storedProcedure;

        public ParsedContainer BuildOutput(List<TSqlParserTokenEntity> comments)
        {
            ParsedContainer container = new ParsedContainer();

            container.Comments = comments;
            container.CteTables = _cte;
            container.Queries = _queries;
            container.StoredProcedure = _storedProcedure;


            var map = new ColumnMappingEntity();
            var tableMap = new TableMapper();

            if (_queries.Count > 0)
            {


                var tables = tableMap.MapTables(_queries, _cte);

                var output = map.GetColumnMap(_queries, _cte);

                //_render.DoRender(map: output, tableMap:tables, comments[0].Text, storedProcedure:_storedProcedure); 
            }
            else
            {

            }

            return container;
        }

        public void Process(QueryExpressionEntity query)
        {
            _queries.Add(query);
        }

        public void Process(CommonTableEntity cte)
        {
            _cte.Add(cte);
        }

        public void Process(StoredProcedureEntity proc)
        {
            _storedProcedure = proc;
        }
    }
}
