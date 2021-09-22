namespace SqlParserWrapper.Model
{
    public class ParenthesisEntity
    {
        public BinaryExpressionEntity BinaryExpression { get; set;}

        public BooleanBinaryEntity BooleanBinary { get; set;}
        public string Literal { get; set;}
        public ParenthesisEntity ParenthesisExpression { get; set;}
        public IIfCallEntity IIfCall { get; set;}
    }
}