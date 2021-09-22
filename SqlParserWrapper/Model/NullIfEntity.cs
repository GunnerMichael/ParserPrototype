namespace SqlParserWrapper.Model
{
    public class NullIfEntity
    {
        public ExpressionEntity FirstExpression { get; set; }

        public ExpressionEntity SecondExpression { get; set; }

        //public string SecondExpressionLiteral { get; set;}
        //public VariableReferenceEntity SecondExpressionVariable { get; set;}
        //public FunctionCallEntity SecondExpressionFunctionCall { get; set;}
        //public LeftFunctionCallEntity LeftFunctionCall { get; set;}
        //public RightFunctionCallEntity RightFunctionCall { get; set;}
        //public FunctionCallEntity FunctionCall { get; set;}
    }
}