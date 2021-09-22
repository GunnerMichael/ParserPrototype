
namespace SqlParserWrapper.Model
{
    public class BinaryExpressionEntity
    {
        public string BinaryExpressionType { get; set;}
        public ExpressionEntity FirstExpression {get;  set;}
        public ExpressionEntity SecondExpression { get; set;}
        //    public ColumnReferenceEntity FirstExpressionColumnReference { get; set;}
        //    public ColumnReferenceEntity SecondExpressionColumnReference { get; set;}
        //    public FunctionCallEntity FirstExpressionFunctionCall { get; set;}
        //    public FunctionCallEntity SecondExpressionFunctionCall { get; set;}
        //    public NullIfEntity FirstExpressionNullIf { get; set;}
        //    public string SecondExpressionLiteral { get; set;}
        //    public IIfCallEntity SecondExpressionIIfCall { get; set;}
        //    public string FirstExpressionLiteral { get; set;}
        //    public ConvertCallEntity SecondExpressionConvertCall { get; set;}
        //    public BinaryExpressionEntity FirstExpressionBinaryExpression { get; set;}
        //    public SearchCasedEntity SecondExpressionSearchedCase { get; set;}
        //    public ParenthesisEntity FirstExpressionParenthesis { get; set;}
        //    public ParenthesisEntity SecondExpressionParenthesis { get; set;}
        //    public BinaryExpressionEntity SecondExpressionBinaryExpression { get; set;}
    }
}