using System.Collections.Generic;

namespace SqlParserWrapper.Model
{
    public class RightFunctionCallEntity
    {
        private List<ParameterEntity> _parameters = new List<ParameterEntity>();

        public List<ParameterEntity> Parameters
        {
            get
            {
                return _parameters;
            }
        }

    }
}