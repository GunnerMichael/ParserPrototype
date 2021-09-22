namespace SqlParserWrapper.Model
{
    public class ScalarExpressionEntity
    {
        public ScalarExpressionEntity()
        {
        }

        private ColumnReferenceEntity columnReference;

        public ColumnReferenceEntity ColumnReference
        {
            get
            {
                return columnReference;
            }
            set
            {
                columnReference = value;
            }
        }

        public SearchCasedEntity SearchedCase { get; set;}
        public FunctionCallEntity FunctionCall { get; set; }
        public BinaryExpressionEntity BinaryExpression { get; set; }
        public LeftFunctionCallEntity LeftFunctionCall { get; set; }
        public RightFunctionCallEntity RightFunctionCall { get; set; }
        public IIfCallEntity IIfCall { get; set; }
        public CastCallEntity CastCall { get; set; }
        public CoalesceEntity Coalesce { get; set; }
        public string Literal { get; set;}
    }
}