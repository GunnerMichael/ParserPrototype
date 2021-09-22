using System.Collections.Generic;

namespace SqlParserWrapper.Model
{
    public class FunctionCallEntity
    {
        public FunctionCallEntity()
        {
        }

        public IdentiferEntity FunctionName { get; set;}

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