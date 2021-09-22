using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper
{
    public class ParsedContainer
    {
        public List<TSqlParserTokenEntity> Comments { get; set; }
        public List<CommonTableEntity> CteTables { get; set; }
        public List<QueryExpressionEntity> Queries { get; set; }
        public StoredProcedureEntity StoredProcedure { get; set; }
    }
}
