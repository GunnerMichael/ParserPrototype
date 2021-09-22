using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class ParameterEntity
    {
        public ColumnReferenceEntity ColumnReference { get; set; }
        public UnaryExpressionEntity UnaryExpression { get; set;}
        public string Literal { get; set;}
        public SearchCasedEntity SearchedCaseExpression { get; set;}
        public VariableReferenceEntity VariableReference { get; set;}
        public FunctionCallEntity FunctionCall { get; set; }
        public ConvertCallEntity ConvertCall { get; set; }
        public NullIfEntity NullIfExpression { get; set; }
        public BinaryExpressionEntity BinaryExpression { get; set; }
        public IIfCallEntity IIfCall { get; set; }
        public CastCallEntity CastCall { get; set; }
        public ParenthesisEntity ParenthesisExpression { get; set; }
        public CoalesceEntity CoalesceExpression { get; set; }
        public LeftFunctionCallEntity LeftFunctionCall { get; set; }
        public TryCastCallEntity TryCastCall { get; set; }
    }
}
