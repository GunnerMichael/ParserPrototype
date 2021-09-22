
namespace SqlParserWrapper.Model
{
    public class BooleanBinaryEntity
    {
        public ExpressionEntity FirstExpression{ get; set; }
        public ExpressionEntity SecondExpression { get; set; }
        public string BinaryExpressionType { get; set;}

        //public BooleanComparisonEntity FirstExpressionBooleanComparison { get; set;}
        //public BooleanBinaryEntity FirstExpressionBooleanBinary { get; set;}
        //public BooleanIsNullEntity FirstExpressionIsNull { get; set;}
        //public BooleanComparisonEntity SecondExpressionBooleanComparison { get; set;}
        //public BooleanIsNullEntity SecondExpressonIsNull { get; set;}
        //public BooleanTernaryEntity SecondExpressionBooleanTernary { get; set;}
        //public LikePredicateEntity SecondExpressionLikePredicate { get; set;}
        //public ParenthesisEntity SecondExpressionBooleanParenthesis { get; set;}
        //public ParenthesisEntity FirstExpressionBooleanParenthesis { get; set;}
        //public InPredicateEntity SecondExpressionInPredicate { get; set;}
        //public LikePredicateEntity LikePredicate { get; set;}
        //public BooleanTernaryEntity FirstExpressionBooleanTernary { get; set;}
        //public InPredicateEntity FirstExpressionInPredicate { get; set;}
    }
}