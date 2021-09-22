
namespace SqlParserWrapper.Model
{
    public  class BinaryQueryEntity
    {
        public BinaryQueryEntity()
        {
        }

        public ExpressionEntity FirstQuery { get; set; }

        public ExpressionEntity SecondQuery { get; set; }
        public string BinaryQueryExpressionType { get; set;}
    }
}