using System.Collections.Generic;

namespace SqlParserWrapper.Model
{
    public class CoalesceEntity
    {
        private List<ScalarExpressionEntity> _expressions = new List<ScalarExpressionEntity>();

        public List<ScalarExpressionEntity> Expressions
        {
            get
            {
                return _expressions;
            }
        }
    }
}