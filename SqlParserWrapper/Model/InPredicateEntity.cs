using System.Collections.Generic;

namespace SqlParserWrapper.Model
{
    public class InPredicateEntity
    {
        public ColumnReferenceEntity ColumnReference { get; set;}

        private List<string> _inValues = new List<string>();

        public List<string> InValues
        {
            get
            {
                return _inValues;
            }
        }

        public FunctionCallEntity FunctionCall { get; set; }
        public LeftFunctionCallEntity LeftFunctionCall { get; set; }
    }
}