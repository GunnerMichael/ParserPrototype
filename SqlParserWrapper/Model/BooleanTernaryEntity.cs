
namespace SqlParserWrapper.Model
{
    public class BooleanTernaryEntity
    {
        public ExpressionEntity FirstExpression { get; set; }

        public ExpressionEntity SecondExpression { get; set; }

        public ExpressionEntity ThirdExpression { get; set; }

        public string TernaryExpressionType { get; set;}
        //public ColumnReferenceEntity FirstExpressionColumnReference { get; set;}
        //public string ThirdExpressionLiteral { get; set;}
        //public VariableReferenceEntity SecondExpressionVariable { get; set;}
        //public VariableReferenceEntity ThirdExpressionVariable { get; set;}
        //public FunctionCallEntity FirstExpressionFunctionCall { get; set;}
        //public FunctionCallEntity SecondExpressionFunctionCall { get; set;}
        //public FunctionCallEntity ThirdExpressionFunctionCall { get; set;}
        //public ConvertCallEntity FirstExpressionConvertCall { get; set;}
        //public CastCallEntity FirstExpressionCastCall { get; set;}
        //public ColumnReferenceEntity ThirdExpressionColumnReference { get; set;}
        //public ColumnReferenceEntity SecondExpressionColumnReference { get; set;}
    }
}