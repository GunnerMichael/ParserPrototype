using SqlParserWrapper.Model;
using System.Collections.Generic;

namespace SqlParserWrapper.Model
{
    public class SchemaObjectFunctionTableEntity : BaseTable
    {
        public SchemaObjectFunctionTableEntity()
        {
        }

        private List<ParameterEntity> _parameters = new List<ParameterEntity>();

        public List<ParameterEntity> Parameters
        {
            get
            {
                return _parameters;
            }
        }

        public SchemaObjectEntity SchemaObject { get; set; }

    }

}