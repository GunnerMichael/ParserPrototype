using SqlParserWrapper.Model;
using System.Collections.Generic;

namespace SQLParser
{
    public interface IOutputRender
    {
        void DoRender(List<ColumnMapEntity> map, Dictionary<string, TableMap> tableMap, string commentHeader, StoredProcedureEntity storedProcedure);
    }
}