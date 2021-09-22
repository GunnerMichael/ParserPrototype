namespace SqlParserWrapper.Model
{
    public class IIfCallEntity
    {
        public ExpressionEntity Predicate { get; set; }

        public ExpressionEntity Then { get; set; }

        public ExpressionEntity Else { get; set; }



    }
}