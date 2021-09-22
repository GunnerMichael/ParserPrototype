using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class ExpressionEntity
    {
        public BinaryQueryEntity BinaryQuery { get; set;}
        public QuerySpecificationEntity QuerySpecification { get; set;}

        public BooleanComparisonEntity BooleanComparison { get; set; }
        public InPredicateEntity InPredicate { get; set;}
        public BooleanBinaryEntity BooleanBinary { get; set;}
        public string Literal{ get; set;}
        public ColumnReferenceEntity ColumReference { get; set;}
        public FunctionCallEntity FunctionCall { get; set;}
        public BinaryExpressionEntity BinaryExpression { get; set;}
        public BooleanIsNullEntity BooleanIsNull { get; set;}
        public LikePredicateEntity LikePredicate { get; set;}
        public BooleanTernaryEntity BooleanTernary { get; set;}
        public BooleanNotEntity BooleanNot { get; set;}
        public ParenthesisEntity BooleanParenthesis { get; set;}
        public BooleanComparisonEntity BooleanComparsion { get; set;}
        public UnaryExpressionEntity Unary { get; set;}
        public RightFunctionCallEntity RightFunctionCall { get; set;}
        public LeftFunctionCallEntity LeftFunctionCall { get; set;}
        public VariableReferenceEntity Variable { get; set;}
        public CastCallEntity CastCall { get; set;}
        public TryConvertEntity TryConvert { get; set;}
        public ConvertCallEntity ConvertCall { get; set;}

        public SearchCasedEntity SearchCased { get; set;}
        public ScalarSubqueryEntity ScalarSubquery { get; set;}

        public CoalesceEntity Coalesce { get; set; }
        public IIfCallEntity IIfCall { get; set;}
        public NullIfEntity NullIf { get; set; }
    }
}
