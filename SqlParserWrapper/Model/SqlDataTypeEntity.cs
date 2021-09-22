using System.Collections.Generic;

namespace SqlParserWrapper.Model
{
    public class SqlDataTypeEntity
    {
        public SqlDataTypeEntity()
        {
        }

        public string DataTypeOption { get; set;}

        private List<ParameterEntity> _parameters = new List<ParameterEntity>();

        public List<ParameterEntity> Parameters
        {
            get
            {
                return _parameters;
            }
        }

        public SchemaObjectEntity Name { get; set;}
    }
}