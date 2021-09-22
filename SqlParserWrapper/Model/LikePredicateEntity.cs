namespace SqlParserWrapper.Model
{
    public class LikePredicateEntity
    {
        public ExpressionEntity FirstExpression { get; set; }

        public ExpressionEntity SecondExpression { get; set; }

        //public ColumnReferenceEntity FirstExpressionColumnReference { get; set;}
        //public FunctionCallEntity FirstExpressionFunctionCall { get; set;}
        //public string SecondExpressionLiteral { get; set;}
        //public VariableReferenceEntity SecondExpressionVariable { get; set;}
        //public FunctionCallEntity SecondExpressionFunctionCall { get; set;}
        //public ParenthesisEntity SecondExpressionParenthesis { get; set;}
    }
}