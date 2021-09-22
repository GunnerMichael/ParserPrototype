using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryMap
{
    public class ColumnValueBuilder
    {
        private readonly QuerySpecificationEntity querySpecification;

        public ColumnValueBuilder(QuerySpecificationEntity querySpecification)
        {
            this.querySpecification = querySpecification;
        }
        public string BuildOutput(ExpressionEntity source)
        {
            string output = string.Empty;

            if (source is null)
            {
                return string.Empty;
            }

            if (source.BooleanBinary is not null)
            {
                output += BuildBooleanBinary(source.BooleanBinary);
            }
            else if (source.BooleanComparison is not null)
            {
                output += BuildBooleanComparison(source.BooleanComparison);
            }
            else if (source.InPredicate is not null)
            {
                output += GetInStatement(source.InPredicate);
            }
            else if (source.BooleanIsNull is not null)
            {
                output += BuildIsNull(source.BooleanIsNull);
            }
            else if (source.ColumReference is not null)
            {
                output += GetColumnReferenceEntity(source.ColumReference);
            }
            else if (source.Literal is not null)
            {
                output += source.Literal;
            }
            else if (source.FunctionCall is not null)
            {
                output += GetFunctionVal(source.FunctionCall);
            }
            else if (source.BinaryExpression is not null)
            {
                output += GetBinaryExpression(source.BinaryExpression);
            }
            else if (source.BooleanParenthesis is not null)
            {
                output += GetParenthesisExpression(source.BooleanParenthesis);
            }
            else if (source.BooleanBinary is not null)
            {
                output += BuildBooleanBinary(source.BooleanBinary);
            }
            else if (source.BooleanComparsion is not null)
            {
                output += BuildBooleanComparison(source.BooleanComparsion);
            }
            else if (source.BooleanTernary is not null)
            {
                output += BuildBooleanTernary(source.BooleanTernary);
            }
            else if (source.LeftFunctionCall is not null)
            {
                output += GetLeftFunction(source.LeftFunctionCall);
            }
            else if (source.RightFunctionCall is not null)
            {
                output += GetRightFunction(source.RightFunctionCall);
            }
            else if (source.Unary is not null)
            {
                output += GetUnary(source.Unary);
            }
            else if (source.Variable is not null)
            {
                output = source.Variable.Name;
            }
            else if (source.TryConvert is not null)
            {
                output += BuildTryConvert(source.TryConvert);
            }
            else if (source.CastCall is not null)
            {
                output += BuildCastCall(source.CastCall);
            }
            else if (source.ConvertCall is not null)
            {
                output += GetConvertCall(source.ConvertCall);
            }
            else if (source.SearchCased is not null)
            {
                output += GetSearchedCaseVal(source.SearchCased);
            }
            else if (source.BooleanParenthesis is not null)
            {
                output += GetParenthesisExpression(source.BooleanParenthesis);
            }
            else if (source.LikePredicate is not null)
            {
                output += BuildLikePredicate(source.LikePredicate);
            }
            else if (source.ScalarSubquery is not null)
            {
                output += BuildScalarSubquery(source: source.ScalarSubquery);
            }
            else if (source.QuerySpecification is not null)
            {
                output += BuildQuerySpecification(source: source.QuerySpecification);
            }
            else if (source.Coalesce is not null)
            {
                output += GetCoalesce(source.Coalesce);
            }
            else if (source.NullIf is not null)
            {
                output += BuildNullIf(source.NullIf);
            }
            else if (source.SearchCased is not null)
            {
                output += GetSearchedCaseVal(source.SearchCased);
            }
            else if (source.IIfCall is not null)
            {
                output += BuildIIf(source.IIfCall);
            }
            //if (source.FirstExpressionBooleanBinary is not null)
            //{
            //    output += BuildBooleanBinary(source: source.FirstExpressionBooleanBinary);
            //}
            //else if (source.FirstExpressionBooleanComparison is not null)
            //{
            //    output += BuildBooleanComparison(source: source.FirstExpressionBooleanComparison);
            //}
            //else if (source.FirstExpressionIsNull is not null)
            //{
            //    output += BuildIsNull(source: source.FirstExpressionIsNull);
            //}
            //else if (source.FirstExpressionBooleanParenthesis is not null)
            //{
            //    output += GetParenthesisExpression(source.FirstExpressionBooleanParenthesis);
            //}
            //else if (source.LikePredicate is not null)
            //{
            //    output += BuildLikePredicate(source.LikePredicate);
            //}
            //else if (source.FirstExpressionBooleanTernary is not null)
            //{
            //    output += BuildBooleanTernary(source.FirstExpressionBooleanTernary);
            //}
            //else if (source.FirstExpressionInPredicate is not null)
            //{
            //    output += GetInStatement(source.FirstExpressionInPredicate);
            //}


            //if (source.SecondExpressionBooleanComparison is not null)
            //{
            //    output += BuildBooleanComparison(source: source.SecondExpressionBooleanComparison);
            //}
            //else if (source.SecondExpressionBooleanTernary is not null)
            //{
            //    output += BuildBooleanTernary(source: source.SecondExpressionBooleanTernary);
            //}
            //else if (source.SecondExpressionLikePredicate is not null)
            //{
            //    output += BuildLikePredicate(source.SecondExpressionLikePredicate);
            //}
            //else if (source.SecondExpressonIsNull is not null)
            //{
            //    output += BuildIsNull(source.SecondExpressonIsNull);
            //}
            //else if (source.SecondExpressionBooleanParenthesis is not null)
            //{
            //    output += GetParenthesisExpression(source.SecondExpressionBooleanParenthesis);
            //}
            //else if (source.SecondExpressionInPredicate is not null)
            //{
            //    output += GetInStatement(source.SecondExpressionInPredicate);
            //}



            else
            {

            }

            //if (source.FirstExpressionColumnReference is not null)
            //{
            //    output += GetColumnReferenceEntity(source.FirstExpressionColumnReference);
            //}
            //else if (source.FirstExpressionFunctionCall is not null)
            //{
            //    output += GetFunctionVal(source.FirstExpressionFunctionCall);
            //}
            //else if (source.FirstExpressionNullIf is not null)
            //{
            //    output += BuildNullIf(source: source.FirstExpressionNullIf);
            //}
            //else if (source.FirstExpressionLiteral is not null)
            //{
            //    output += source.FirstExpressionLiteral;
            //}
            //else if (source.FirstExpressionBinaryExpression is not null)
            //{
            //    output += GetBinaryExpression(source.FirstExpressionBinaryExpression);
            //}
            //else if (source.FirstExpressionParenthesis is not null)
            //{
            //    output += GetParenthesisExpression(source.FirstExpressionParenthesis);
            //}


            //if (source.SecondExpressionColumnReference is not null)
            //{
            //    output += GetColumnReferenceEntity(source.SecondExpressionColumnReference);
            //}
            //else if (source.SecondExpressionFunctionCall is not null)
            //{
            //    output += GetFunctionVal(source.SecondExpressionFunctionCall);
            //}
            //else if (source.SecondExpressionIIfCall is not null)
            //{
            //    output += BuildIIf(source: source.SecondExpressionIIfCall);
            //}
            //else if (source.SecondExpressionLiteral is not null)
            //{
            //    output += source.SecondExpressionLiteral;
            //}
            //else if (source.SecondExpressionConvertCall is not null)
            //{
            //    output += GetConvertCall(source.SecondExpressionConvertCall);
            //}
            //else if (source.SecondExpressionSearchedCase is not null)
            //{
            //    output += GetSearchedCaseVal(source.SecondExpressionSearchedCase);
            //}
            //else if (source.SecondExpressionBinaryExpression is not null)
            //{
            //    output += GetBinaryExpression(source.SecondExpressionBinaryExpression);
            //}
            //else if (source.SecondExpressionParenthesis is not null)
            //{
            //    output += GetParenthesisExpression(source.SecondExpressionParenthesis);
            //}




            return output;
        }

        protected string BuildCastCall(CastCallEntity source)
        {
            string output = string.Empty;

            string dataTypeArgs = BuildParamString(source.DataType.Parameters, includeContainer: true);

            string type = $"{source.DataType.DataTypeOption.ToString().ToUpper()}";

            output += $"CAST({BuildParamString(source.Parameter)} AS {type}{dataTypeArgs})";

            return output;
        }

        protected string GetSearchedCaseVal(SearchCasedEntity searchedCase)
        {
            string output = string.Empty;


            foreach (var item in searchedCase.WhenClauses)
            {
                output += BuildWhenString(item);
                output += Environment.NewLine;
            }

            string elseString = BuildOutput(searchedCase.Else);

            if (string.IsNullOrEmpty(elseString) == false)
            {
                output += $"ELSE {elseString}";
            }


            return output;
        }

        protected string GetRightFunction(RightFunctionCallEntity source)
        {
            string output = $"RIGHT({BuildParamString(source.Parameters)})";


            return output;
        }

        protected string GetLeftFunction(LeftFunctionCallEntity source)
        {
            string output = $"LEFT({BuildParamString(source.Parameters)})";

            return output;
        }

        protected string BuildWhenString(WhenClauseEntity item)
        {
            string output = String.Empty;

            output += "WHEN ";

            output += BuildOutput(item.Expression);
            output += " THEN ";
            output += BuildOutput(item.Then);

            return output;
        }

        protected string BuildWhenInPredicate(InPredicateEntity whenInPredicate)
        {
            string output = String.Empty;

            output = GetInStatement(whenInPredicate);


            return output;
        }

        protected string GetInStatement(InPredicateEntity source)
        {
            string output = string.Empty;
            if (source.FunctionCall is not null)
            {
                output = $"{GetFunctionVal(source.FunctionCall)} IN (";
            }
            else if (source.ColumnReference is not null)
            {
                output = $"{GetColumnReferenceEntity(source.ColumnReference)} IN (";
            }
            else if (source.LeftFunctionCall is not null)
            {
                output = output = $"{GetLeftFunction(source.LeftFunctionCall)} IN (";
            }


            foreach (var item in source.InValues)
            {
                output += item;
                output += ",";
            }

            output = output.Trim(',');
            output += ')';

            return output;

        }


        protected string BuildBooleanComparison(BooleanComparisonEntity source)
        {
            string output = String.Empty;

            output += BuildOutput(source.FirstExpression);
            output += $" {source.ComparisonType} ";
            output += BuildOutput(source.SecondExpression);

            //if (source.SecondExpressionColumnReference is not null)
            //{
            //    output += GetColumnReferenceEntity(source.SecondExpressionColumnReference);
            //}
            //else if (source.SecondExpressionConvertCall is not null)
            //{
            //    output += GetConvertCall(source: source.SecondExpressionConvertCall);
            //}
            //else if (source.SecondExpressionFunctionCall is not null)
            //{
            //    output += GetFunctionVal(source.SecondExpressionFunctionCall);
            //}
            //else if (source.SecondExpressionLiteral is not null)
            //{
            //    output += source.SecondExpressionLiteral;
            //}
            //else if (source.SecondExpressionUnary is not null)
            //{
            //    output += GetUnary(source: source.SecondExpressionUnary);
            //}
            //else if (source.SecondExpressionVariable is not null)
            //{
            //    output += source.SecondExpressionVariable.Name;
            //}
            //else if (source.SecondExpressionLeftFunctionCall is not null)
            //{
            //    output += GetLeftFunction(source.SecondExpressionLeftFunctionCall);
            //}
            //else if (source.SecondExpressionCastCall is not null)
            //{
            //    output += BuildCastCall(source.SecondExpressionCastCall);
            //}
            //else if (source.SecondExpressionBinary is not null)
            //{
            //    output += GetBinaryExpression(source.SecondExpressionBinary);
            //}
            //else if (source.SecondExpresssionTryConvert is not null)
            //{
            //    output += BuildTryConvert(source: source.SecondExpresssionTryConvert);
            //}
            //else
            //{

            //}


            return output;

        }

        protected string BuildTryConvert(TryConvertEntity source)
        {
            string output = String.Empty;

            string dataTypeArgs = BuildParamString(source.SqlDataType.Parameters, includeContainer: true);

            string type = $"{source.SqlDataType.DataTypeOption.ToString().ToUpper()}";

            output += $"TRY_CONVERT({type}{dataTypeArgs},{BuildParamString(source.Parameter)})";

            return output;

        }

        protected string GetParenthesisExpression(ParenthesisEntity source)
        {
            string output = String.Empty;

            if (source.BinaryExpression is not null)
            {
                output += GetBinaryExpression(source.BinaryExpression);
            }
            else if (source.BooleanBinary is not null)
            {
                output += BuildBooleanBinary(source.BooleanBinary);
            }
            else if (source.Literal is not null)
            {
                output += source.Literal;
            }
            else if (source.ParenthesisExpression is not null)
            {
                output += GetParenthesisExpression(source.ParenthesisExpression);
            }
            else if (source.IIfCall is not null)
            {
                output += BuildIIf(source.IIfCall);
            }
            else
            {

            }

            return output;
        }
        protected string GetConvertCall(ConvertCallEntity source)
        {
            string output = String.Empty;

            string dataTypeArgs = BuildParamString(source.SqlDataType.Parameters, includeContainer: true);

            string type = $"{source.SqlDataType.DataTypeOption.ToString().ToUpper()}";

            output += $"CONVERT({type}{dataTypeArgs},{BuildParamString(source.Parameter)})";

            return output;
        }

        protected string GetUnary(UnaryExpressionEntity source)
        {
            string output = String.Empty;

            output += $"{source.ExpressionType}{source.LiteralExpression}";

            return output;
        }

        protected string GetBinaryExpression(BinaryExpressionEntity source)
        {
            string output = String.Empty;

            output += BuildOutput(source.FirstExpression);
            output += $" {source.BinaryExpressionType} ";
            output += BuildOutput(source.SecondExpression);

            return output;
        }

        protected string BuildIIf(IIfCallEntity source)
        {
            string output = "IF ";

            output += BuildOutput(source: source.Predicate);

            output += $" THEN ";

            output += BuildOutput(source: source.Then);

            output += Environment.NewLine;
            string elseString = BuildOutput(source.Else);

            if (string.IsNullOrEmpty(elseString) == false)
            {
                output += $"ELSE {elseString}";
            }

            output += Environment.NewLine;
            return output;
        }


        private string BuildQuerySpecification(QuerySpecificationEntity source)
        {
            string output = String.Empty;

            output += "SELECT ";

            foreach (var item in source.Columns)
            {
                output += BuildOutput(item.Expression);
                output += ",";
            }

            output = output.TrimEnd(',');

            foreach (var item in source.FromClause.Tables)
            {
                if (item.SchemaObjectFunctionTable is not null)
                {
                    output += $" FROM {BuildSchemaObjectFunctionTable(item.SchemaObjectFunctionTable)}";
                }
                else if (item.QueryDerivedTable is not null)
                {

                }
            }



            return output;
        }

        private string BuildSchemaObjectFunctionTable(SchemaObjectFunctionTableEntity table)
        {
            string output = string.Empty;

            string paramString = BuildParamString(table.Parameters);
            output = $"{table.SchemaObject.FullName}({paramString})";


            return output;
        }

        private string BuildScalarSubquery(ScalarSubqueryEntity source)
        {
            string output = string.Empty;

            if (source.QueryExpression is not null)
            {
                output += BuildOutput(source.QueryExpression);
            }

            return output;
        }

        protected string BuildNullIf(NullIfEntity source)
        {
            string output = String.Empty;

            output = "NULLIF(";

            output += BuildOutput(source.FirstExpression);

            output += ",";

            output += BuildOutput(source.SecondExpression);

            output += ")";


            return output;
        }

        protected string GetColumnReferenceEntity(ColumnReferenceEntity source)
        {
            if (source.MultiPart.Count > 0)
            {
                var part = source.MultiPart[source.MultiPart.Count - 1];

                if (part.QuoteType == "SquareBracket")
                {
                    return $"[{GetIdentifer(source.MultiPart)}]";
                }
                else if (part.QuoteType == "DoubleQuote")
                {
                    return $"\"{GetIdentifer(source.MultiPart)}\"";
                }
                else
                {
                    return GetIdentifer(source.MultiPart);
                }
            }
            else if (source.ColumnType.ToLower() == "wildcard")
            {
                return "*";
            }
            else
            {
                return String.Empty;
            }

        }

        protected string GetIdentifer(List<IdentiferEntity> multiPart)
        {
            string output = String.Empty;
            int index = multiPart.Count - 2;

            int count = 0;
            foreach (var item in multiPart)
            {
                if (count == index)
                {
                    string nonAlias = GetSchemaObjectFromAlias(item.Value);

                    if (string.IsNullOrEmpty(nonAlias))
                    {
                        output += item.GetValue();
                    }
                    else
                    {
                        output += nonAlias;
                    }
                }
                else
                {
                    output += item.GetValue();
                }
                output += ".";
                count++;
            }

            output = output.Trim('.');

            return output;
        }

        private string GetSchemaObjectFromAlias(string value)
        {
            string result = string.Empty;

            if (querySpecification != null && querySpecification.FromClause != null)
            {
                var table = querySpecification.FromClause.FindTableFromAlias(value);

                if (table != null)
                {
                    result = table.GetName();
                }

            }

            return result;
        }

        protected string BuildBooleanBinary(BooleanBinaryEntity source)
        {
            string output = String.Empty;

            output += BuildOutput(source.FirstExpression);
            output += $" {source.BinaryExpressionType} ";
            output += BuildOutput(source.SecondExpression);


            return output;

        }

        protected string BuildIsNull(BooleanIsNullEntity source)
        {
            string output = String.Empty;

            if (source.IsNot)
            {
                output += $"{GetColumnReferenceEntity(source.ColumnReference)} IS NOT NULL";
            }
            else
            {
                output += $"{GetColumnReferenceEntity(source.ColumnReference)} IS NULL";
            }

            return output;
        }

        protected string BuildLikePredicate(LikePredicateEntity source)
        {
            string output = String.Empty;

            output += BuildOutput(source.FirstExpression);
            output += " LIKE ";
            output += BuildOutput(source.SecondExpression);

            return output;
        }

        protected string BuildBooleanTernary(BooleanTernaryEntity source)
        {
            string output = String.Empty;

            output += BuildOutput(source.FirstExpression);
            output += $" {source.TernaryExpressionType} ";
            output += BuildOutput(source.SecondExpression);
            output += " AND ";
            output += BuildOutput(source.ThirdExpression);

            return output;
        }

        //public void Create(QueryExpressionEntity source, string sep = " = ")
        //{
        //    if (source == null)
        //    {
        //        return;
        //    }

        //    _tables.Clear();

        //    var t = source.QuerySpecification.FromClause.GetUniqueTables();

        //    if (t != null)
        //    {
        //        foreach (var x in t)
        //        {
        //            if (x.TableReference is not null)
        //            {
        //                if (_tables.ContainsKey(x.TableReference.NamedTable.Alias.Identifer.Value.ToLower()) == false)
        //                {
        //                    _tables.Add(x.TableReference.NamedTable.Alias.Identifer.Value.ToLower(), x);
        //                }
        //            }
        //        }

        //        mappings.Clear();

        //        foreach (var col in source.QuerySpecification.Columns)
        //        {
        //            if (string.IsNullOrEmpty(col.TableAlias) == false)
        //            {
        //                var aliasTable = _tables[col.TableAlias.ToLower()];

        //                var nonAliasColumn = col.Value.Replace(col.TableAlias, aliasTable.TableReference.NamedTable.SchemaObject.FullName);

        //                mappings.Add($"{col.ColumnName}{sep}{nonAliasColumn}");
        //            }
        //            else
        //            {
        //                if (col.Value is not null)
        //                {
        //                    mappings.Add($"{col.ColumnName}{sep}{col.Value}");
        //                }
        //                else
        //                {
        //                    string val = BuildOutput(col.Expression);

        //                    mappings.Add($"{col.ColumnName}{sep}{val}");
        //                }
        //            }
        //        }
        //    }
        //}

        protected string GetFunctionVal(FunctionCallEntity functionCall)
        {
            string output = String.Empty;

            string paramString = BuildParamString(functionCall.Parameters);
            output = $"{functionCall.FunctionName.GetValue()}({paramString})";

            return output;
        }

        protected string BuildParamString(ParameterEntity item)
        {
            if (item == null)
            {
                return string.Empty;
            }
            string output = string.Empty;

            if (item.ColumnReference is not null)
            {
                output += GetColumnReferenceEntity(item.ColumnReference);
            }
            else if (item.BinaryExpression is not null)
            {
                output += GetBinaryExpression(item.BinaryExpression);
            }
            else if (item.Literal is not null)
            {
                output += item.Literal;
            }
            else if (item.ConvertCall is not null)
            {
                output += GetConvertCall(item.ConvertCall);
            }
            else if (item.FunctionCall is not null)
            {
                output += GetFunctionVal(item.FunctionCall);
            }
            else if (item.IIfCall is not null)
            {
                output += BuildIIf(item.IIfCall);
            }
            else if (item.UnaryExpression is not null)
            {
                output += GetUnary(item.UnaryExpression);
            }
            else if (item.NullIfExpression is not null)
            {
                output += BuildNullIf(item.NullIfExpression);
            }
            else if (item.SearchedCaseExpression is not null)
            {
                output += GetSearchedCaseVal(item.SearchedCaseExpression);
            }
            else if (item.ParenthesisExpression is not null)
            {
                output += GetParenthesisExpression(item.ParenthesisExpression);
            }
            else if (item.LeftFunctionCall is not null)
            {
                output += GetLeftFunction(item.LeftFunctionCall);
            }
            else if (item.CoalesceExpression is not null)
            {
                output += GetCoalesce(source: item.CoalesceExpression);
            }
            else if (item.VariableReference is not null)
            {
                output += item.VariableReference.Name;
            }
            else if (item.TryCastCall is not null)
            {
                output += TryCastCall(item.TryCastCall);
            }
            else if (item.CastCall is not null)
            {
                output += BuildCastCall(item.CastCall);
            }
            else
            {

            }

            return output;
        }

        protected string TryCastCall(TryCastCallEntity source)
        {
            string output = string.Empty;

            string dataTypeArgs = BuildParamString(source.DataType.Parameters, includeContainer: true);

            string type = $"{source.DataType.DataTypeOption.ToString().ToUpper()}";

            output += $"TRYCAST({BuildParamString(source.Parameter)} AS {type}{dataTypeArgs})";

            return output;

        }

        protected string GetCoalesce(CoalesceEntity source)
        {
            string output = String.Empty;

            output = "COALESCE(";

            foreach (var item in source.Expressions)
            {
                output += GetExpression(item);
                output += ",";
            }

            output = output.Trim(',');

            output += ")";

            return output;
        }

        protected string GetExpression(ScalarExpressionEntity item)
        {
            string output = String.Empty;

            if (item.ColumnReference is not null)
            {
                output += GetColumnReferenceEntity(item.ColumnReference);
            }
            else if (item.BinaryExpression is not null)
            {
                output += GetBinaryExpression(item.BinaryExpression);
            }
            else if (item.Literal is not null)
            {
                output += item.Literal;
            }
            //else if (item.ConvertCall is not null)
            //{
            //    output += GetConvertCall(item.ConvertCall);
            //}
            else if (item.FunctionCall is not null)
            {
                output += GetFunctionVal(item.FunctionCall);
            }
            else if (item.IIfCall is not null)
            {
                output += BuildIIf(item.IIfCall);
            }
            //else if (item.UnaryExpression is not null)
            //{
            //    output += GetUnary(item.UnaryExpression);
            //}
            //else if (item.NullIfExpression is not null)
            //{
            //    output += BuildNullIf(item.NullIfExpression);
            //}
            //else if (item.SearchedCaseExpression is not null)
            //{
            //    output += GetSearchedCaseVal(item.SearchedCaseExpression);
            //}
            //else if (item.ParenthesisExpression is not null)
            //{
            //    output += GetParenthesisExpression(item.ParenthesisExpression);
            //}
            else if (item.LeftFunctionCall is not null)
            {
                output += GetLeftFunction(item.LeftFunctionCall);
            }
            //else if (item.CoalesceExpression is not null)
            //{
            //    output += GetCoalesce(source: item.CoalesceExpression);
            //}
            else
            {

            }

            return output;
        }

        protected string BuildParamString(List<ParameterEntity> parameters, bool includeContainer = false)
        {
            string output = String.Empty;
            foreach (var item in parameters)
            {
                output += BuildParamString(item);

                output += ",";
            }

            output = output.TrimEnd(',');

            if (includeContainer && string.IsNullOrEmpty(output) == false)
            {
                return $"({output})";
            }
            else
                return output;
        }

    }
}
