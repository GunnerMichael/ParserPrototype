using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Contract
{
    public interface IQueryProcesser
    {
        void Process(QueryExpressionEntity query);
        void Process(CommonTableEntity cte);
        ParsedContainer BuildOutput(List<TSqlParserTokenEntity> comments);
        void Process(StoredProcedureEntity entity);
    }
}
