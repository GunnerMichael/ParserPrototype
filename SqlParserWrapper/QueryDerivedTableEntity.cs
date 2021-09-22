using SqlParserWrapper.Model;

namespace SqlParserWrapper
{
    public class QueryDerivedTableEntity
    {
        public IdentiferEntity Alias { get; internal set; }
        public ExpressionEntity QueryExpression { get; internal set; }
    }
}