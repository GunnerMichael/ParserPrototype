using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlParserWrapper.Contract;
using SqlParserWrapper.Convert;
using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace SqlParserWrapper
{
    public class OwnVisitor : TSqlFragmentVisitor
    {
        private StreamWriter _debug;
        public OwnVisitor(SqlParserWrapper.Contract.IQueryProcesser queryProcesser, StreamWriter debug = null)
        {
            _queryProcesser = queryProcesser;
            _debug = debug;
        }

        private void WriteDebug(string output)
        {
            if (_debug != null)
            {
                _debug.WriteLine(output);
            }

            System.Diagnostics.Debug.WriteLine(output);
        }


        private List<CommonTableEntity> _cte = new List<CommonTableEntity>();
        private IQueryProcesser _queryProcesser;

        public override void ExplicitVisit(SelectStatement node)
        {
            QueryExpressionEntity entity = new QueryExpressionEntity();

            if (node.QueryExpression is QuerySpecification)
            {
                entity.QuerySpecification = QueryExpression(node.QueryExpression as QuerySpecification);
                string name = "Resultset";

                if (node.Into is not null && node.Into is SchemaObjectName)
                {
                    var sso = SelectSchemaObjectName(node.Into as SchemaObjectName);

                    name = sso.FullName;

                }

                entity.QuerySpecification.Name = name;

                _queryProcesser.Process(entity);
            }
            else
            {

            }

            base.ExplicitVisit(node);
        }


        public override void ExplicitVisit(WithCtesAndXmlNamespaces node)
        {
            foreach (var cte in node.CommonTableExpressions)
            {
                CommonTableEntity myCte = new CommonTableEntity();
                    
                if (cte.ExpressionName is Identifier)
                {
                    myCte.Name = cte.ExpressionName.Value;
                }
                else
                {
                    WriteDebug($"Unknown CTE expression name {cte.ExpressionName.GetType()}"); 
                }

                _cte.Add(myCte);

                QueryExpressionEntity entity = new QueryExpressionEntity();

                if (cte.QueryExpression is QuerySpecification)
                {
                    entity.QuerySpecification = QueryExpression(cte.QueryExpression as QuerySpecification);
                }
                else if (cte.QueryExpression is BinaryQueryExpression)
                {
                    entity.BinaryQuery = GetBinaryQueryExpression(cte.QueryExpression as BinaryQueryExpression);
                }
                else
                {
                    WriteDebug($"Unknown CTE QueryExpression {cte.QueryExpression}");
                }


                myCte.QueryExpression = entity;

                // myCte.QueryExpression.Columns.AddRange(entity.Columns);

                _queryProcesser.Process(myCte);
            }

            //var test = _cte[0];


            //var map = new ColumnMappingEntity();
            
            //map.Create(test.QueryExpression);

            //foreach(var item in map.GetMappings)
            //{
            //    WriteDebug(item);
            //}

            //var where = test.QueryExpression.WhereClause;

        }

        private BinaryQueryEntity GetBinaryQueryExpression(BinaryQueryExpression binaryQueryExpression)
        {
            SqlParserWrapper.Model.BinaryQueryEntity entity = new SqlParserWrapper.Model.BinaryQueryEntity();

            entity.FirstQuery = GetExpression(source: binaryQueryExpression.FirstQueryExpression);
            entity.SecondQuery= GetExpression(source: binaryQueryExpression.SecondQueryExpression);

            entity.BinaryQueryExpressionType = binaryQueryExpression.BinaryQueryExpressionType.ToString();


            return entity;
        }

        private ExpressionEntity GetQueryExpression(QueryExpression source)
        {
            ExpressionEntity entity = new ExpressionEntity();

            if (source is BinaryQueryExpression)
            {
                entity.BinaryQuery = GetBinaryQueryExpression(source as BinaryQueryExpression);
            }
            else if (source is QuerySpecification)
            {
                entity.QuerySpecification = GetQuerySpecification(source as QuerySpecification);
            }
            else
            {

            }

            return entity;
        }

        private QuerySpecificationEntity GetQuerySpecification(QuerySpecification querySpecification)
        {
            return QueryExpression(querySpecification);
        }

        public override void ExplicitVisit(CommonTableExpression node)
        {
        }

        public override void ExplicitVisit(StatementWithCtesAndXmlNamespaces node)
        {
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(TableDefinition node)
        {
            base.ExplicitVisit(node);
        }

        private QuerySpecificationEntity QueryExpression(QuerySpecification qs)
        {
            SqlParserWrapper.Model.QuerySpecificationEntity entity = new SqlParserWrapper.Model.QuerySpecificationEntity();

            if (qs == null)
            {
                return null;
            }


            FromClauseEntity fromClauseEntity = FromClause(qs.FromClause);

            SelectElements(qs.SelectElements, entity);

            entity.FromClause = fromClauseEntity;

            entity.WhereClause = GetWhereClause(qs.WhereClause);
              

            return entity;

        }

        private WhereClauseEntity GetWhereClause(WhereClause whereClause)
        {
            WhereClauseEntity entity = new WhereClauseEntity();

            if (whereClause is null)
            {
                return entity;
            }

            entity.SearchCondition = GetExpression(whereClause.SearchCondition);
          

            return entity;
        }

        private SqlParserWrapper.Model.BooleanNotEntity GetBooleanNot(BooleanNotExpression booleanNotExpression)
        {
            SqlParserWrapper.Model.BooleanNotEntity entity = new SqlParserWrapper.Model.BooleanNotEntity();

            if (booleanNotExpression.Expression is BooleanParenthesisExpression)
            {
                entity.BooleanParenthesis = GetBooleanParenthesisExpression(booleanNotExpression.Expression as BooleanParenthesisExpression);

            }
            else
            {
                WriteDebug($"GetBooleanNot: Unknown {booleanNotExpression.Expression}");
            }

            return entity;
        }

        private SqlParserWrapper.Model.ParenthesisEntity GetBooleanParenthesisExpression(BooleanParenthesisExpression booleanParenthesisExpression)
        {
            SqlParserWrapper.Model.ParenthesisEntity entity = new SqlParserWrapper.Model.ParenthesisEntity();

            if (booleanParenthesisExpression.Expression is BooleanBinaryExpression)
            {
                entity.BooleanBinary = GetBooleanBinaryExpression(booleanParenthesisExpression.Expression as BooleanBinaryExpression);
            }
            else
            {
                WriteDebug($"GetBooleanParenthesisExpression unknown Expression {booleanParenthesisExpression.Expression.ToString()}");
            }

            return entity;
        }

        private FromClauseEntity FromClause(FromClause fromClause)
        {
            FromClauseEntity entity = new FromClauseEntity();

            List<TableReference> localTables = null;

            if (fromClause is null)
            {
                return entity;
            }

            foreach(var table in fromClause.TableReferences)
            {
                if (table is QualifiedJoin)
                {
                    entity.QualifiedJoin = SelectQualifiedJoin(table as QualifiedJoin);

                }

            }


            localTables =  GetTablesFrom(fromClause.TableReferences);

            var tableReferencees = localTables;

            foreach (var table in tableReferencees)
            {
                TableEntity te = new TableEntity();
                if (table is NamedTableReference)
                {
                    te.TableReference = SelectTableReference(table);
                    entity.Tables.Add(te);
                }
                //else if (table is VariableTableReference)
                //{

                //}
                else if (table is SchemaObjectFunctionTableReference)
                {
                    te.SchemaObjectFunctionTable = GetSchemaObjectFunctionTable(table: table as SchemaObjectFunctionTableReference);

                    entity.Tables.Add(te);
                }
                else if (table is QueryDerivedTable)
                {
                    te.QueryDerivedTable = GetQueryDerivedTable(table as QueryDerivedTable);
                    entity.Tables.Add(te);
                }
                else
                {
                    WriteDebug($"FromClause: Unknown Table Type {table.ToString()}");
                }
            }

            return entity;
            
        }

        private List<TableReference> GetTablesFrom(IList<TableReference> tableReferences)
        {
            List<TableReference> tables = new List<TableReference>();

            foreach (var item in tableReferences)
            {
                if (item is QualifiedJoin)
                {
                    var qj = (QualifiedJoin)item;

                    tables.AddRange(GetTablesFromQualifedJoin(qj));
                }
                else if (item is NamedTableReference)
                {
                    tables.Add(item as NamedTableReference);
                }
                else if (item is QueryDerivedTable)
                {
                    tables.Add(item as QueryDerivedTable);
                }

                
                else
                {
                    WriteDebug($"GetTablesFrom-Unknown type {item.ToString()}");
                }

            }

            return tables;
        }

        private List<TableReference> GetTablesFromQualifedJoin(QualifiedJoin qj)
        {
            List<TableReference> tables = new List<TableReference>();

            if (qj.FirstTableReference is QualifiedJoin)
            {
                tables.AddRange(GetTablesFromQualifedJoin(qj.FirstTableReference as QualifiedJoin));
            }
            else if (qj.FirstTableReference is TableReference)
            {
                tables.Add(qj.FirstTableReference);
            }
            else
            {
                WriteDebug($"GetTabvlesFromQualifedJoin: Unknown FirstTableReference {qj.FirstTableReference}");
            }

            if (qj.SecondTableReference is TableReference)
            {
                tables.Add(qj.SecondTableReference);
            }
            else
            {
                WriteDebug($"GetTabvlesFromQualifedJoin: Unknown SeconndTableReference {qj.SecondTableReference}");
            }

            return tables;
        }

        private void DebugNameTableReference(TableReference firstTableReference)
        {
            if (firstTableReference is NamedTableReference)
            {
                DebugIdentifer((firstTableReference as NamedTableReference).SchemaObject.BaseIdentifier);
            }
            else
            {

            }
        }

        private SqlParserWrapper.Model.SchemaObjectFunctionTableEntity GetSchemaObjectFunctionTable(SchemaObjectFunctionTableReference table)
        {
            SqlParserWrapper.Model.SchemaObjectFunctionTableEntity entity = new SqlParserWrapper.Model.SchemaObjectFunctionTableEntity();

            foreach (ScalarExpression p in table.Parameters)
            {
                ParameterEntity myParam = GetParameter(p);

                entity.Parameters.Add(myParam);

            }

            entity.SchemaObject = SelectSchemaObjectName(table.SchemaObject);


            return entity;
        }

        private QualifiedJoinEntity SelectQualifiedJoin(QualifiedJoin join)
        {
            QualifiedJoinEntity entity = new QualifiedJoinEntity();

            if (join.FirstTableReference is QualifiedJoin)
            {
                entity.First.QualifiedJoin = SelectQualifiedJoin(join.FirstTableReference as QualifiedJoin);
            }
            else if(join.FirstTableReference is TableReference)
            {
                entity.First.TableReference = SelectTableReference(join.FirstTableReference as TableReference);
              
            }
            //else if (join.FirstTableReference is VariableTableReference)
            //{

            //}
            else
            {
                WriteDebug($"SelectQualifedJoin  {join.FirstTableReference.ToString()}");
            }

            if (join.SecondTableReference is QualifiedJoin)
            {
                entity.Second.QualifiedJoin = SelectQualifiedJoin(join.SecondTableReference as QualifiedJoin);
            }
            else if (join.SecondTableReference is TableReference)
            {
                entity.Second.TableReference = SelectTableReference(join.SecondTableReference as TableReference);
            }
            //else if (join.SecondTableReference is VariableTableReference)
            //{

            //}
            else
            {
                WriteDebug($"SelectQualifedJoin  {join.FirstTableReference.ToString()}");
            }

            entity.SearchCondition = GetExpression(join.SearchCondition);

            return entity;
        }

        private TableReferenceEntity SelectTableReference(TableReference tableReference)
        {
            TableReferenceEntity entity = new TableReferenceEntity();

            if (tableReference is NamedTableReference)
            {
                entity.NamedTable = GetNamedTableReference(tableReference as NamedTableReference);


            }
            else if (tableReference is VariableTableReference)
            {
                entity.VariableTable = GetVariableTableReference(tableReference as VariableTableReference);
            }
            else if (tableReference is QueryDerivedTable)
            {
                entity.QueryDerivedTable = GetQueryDerivedTable(tableReference as QueryDerivedTable);
            }
            else
            {
                WriteDebug($"SelectTableReference: Unknown {tableReference.ToString()}");
            }

            return entity;
        }

        private QueryDerivedTableEntity GetQueryDerivedTable(QueryDerivedTable queryDerivedTable)
        {
            QueryDerivedTableEntity entity = new QueryDerivedTableEntity();

            entity.Alias = GetIdentifer(queryDerivedTable.Alias);
            entity.QueryExpression = GetExpression(queryDerivedTable.QueryExpression);


            return entity;
        }

        private VariableTableEntity GetVariableTableReference(VariableTableReference variableTableReference)
        {
            SqlParserWrapper.Model.VariableTableEntity entity = new SqlParserWrapper.Model.VariableTableEntity();

            entity.Variable = GetVariableReference(variableTableReference.Variable); 

            return entity;
        }

        private ReferenceTableEntity GetNamedTableReference(NamedTableReference namedTableReference)
        {
            SqlParserWrapper.Model.ReferenceTableEntity entity = new SqlParserWrapper.Model.ReferenceTableEntity();

            entity.Alias = GetAlias(namedTableReference);

            var schemaObject = SelectSchemaObjectName(namedTableReference.SchemaObject);

            entity.SchemaObject = schemaObject;


            return entity;
        }

        private SqlParserWrapper.Model.AliasEntity GetAlias(NamedTableReference namedTableReference)
        {
            SqlParserWrapper.Model.AliasEntity entity = new SqlParserWrapper.Model.AliasEntity();

            if (namedTableReference.Alias != null)
            {
                entity.Identifer = GetIdentifer(namedTableReference.Alias);                                      
            }


            return entity;
        }

        private IdentiferEntity GetIdentifer(Identifier identifier)
        {
            IdentiferEntity entity = new IdentiferEntity();

            entity.QuoteType = identifier.QuoteType.ToString();
            entity.Value = identifier.Value;

            return entity;
        }

        private SchemaObjectEntity SelectSchemaObjectName(SchemaObjectName schemaObject)
        {
            SchemaObjectEntity entity = new SchemaObjectEntity();

            foreach(var item in schemaObject.Identifiers)
            {
                IdentiferEntity myIdentifer = new IdentiferEntity();

                myIdentifer.QuoteType = item.QuoteType.ToString();
                myIdentifer.Value = item.Value;

                entity.Identifers.Add(myIdentifer);

            }

            return entity;
        }

        private void SelectElements(IList<SelectElement> se, QuerySpecificationEntity qee)
        {
            foreach (SelectElement item in se)
            {
                qee.Columns.Add(SelectElement(item));
            }
        }

        private SelectColumnEntity SelectElement(SelectElement sse)
        {
            SelectColumnEntity entity = null; ;

            if (sse is SelectScalarExpression)
            {
                entity = SelectScalarExpression(sse as SelectScalarExpression);
            }
            else
            {
                WriteDebug(sse.ToString());
            }

            return entity;
        }


        public ScalarExpressionEntity GetScalarExpression(ScalarExpression sse)
        {
            ScalarExpressionEntity entity = new SqlParserWrapper.Model.ScalarExpressionEntity();

            if (sse is ColumnReferenceExpression)
            {
                var colRefEntity = GetColumnReferenceExpression(sse as ColumnReferenceExpression);
                entity.ColumnReference = colRefEntity;
            }
            else if (sse is SearchedCaseExpression)
            {
                entity.SearchedCase = SearchedCaseExpression(sse as SearchedCaseExpression);
            }
            else if (sse is IIfCall)
            {
                entity.IIfCall = GetIIfCallExpression(sse as IIfCall);

            }
            else if (sse is Literal)
            {
                entity.Literal = GetLiteral(sse as StringLiteral);
            }
            else if (sse is FunctionCall)
            {
                entity.FunctionCall = GetFunctionCall(sse as FunctionCall);
            }
            else if (sse is BinaryExpression)
            {
                entity.BinaryExpression = GetBinaryExpression(sse as BinaryExpression);
            }
            else if (sse is LeftFunctionCall)
            {
                entity.LeftFunctionCall = GetLeftFunctionCall(sse as LeftFunctionCall);
            }
            else if (sse is RightFunctionCall)
            {
                entity.RightFunctionCall = GetRightFunctionCall(sse as RightFunctionCall);
            }
            else if (sse is CastCall)
            {
                entity.CastCall = GetCastCall(source: sse as CastCall);
            }
            else if (sse is CoalesceExpression)
            {
                entity.Coalesce = GetCoalesce(sse as CoalesceExpression);
            }
            else
            {
                WriteDebug(sse.ToString());
            }


            return entity;
        }

        private SelectColumnEntity SelectScalarExpression(SelectScalarExpression sse)
        {
            SelectColumnEntity entity = new SelectColumnEntity();

            if (sse.ColumnName != null && string.IsNullOrEmpty(sse.ColumnName.Value) == false)
            {
                entity.ColumnName = sse.ColumnName.Value;
            }

            if (sse.Expression is ColumnReferenceExpression)
            {
                var colRefEntity = GetColumnReferenceExpression(sse.Expression as ColumnReferenceExpression);

                if (string.IsNullOrEmpty(entity.ColumnName))
                {
                    entity.ColumnName = colRefEntity.MultiPart[colRefEntity.MultiPart.Count - 1].Value;

                }
            }

            entity.Expression = GetExpression(sse.Expression);
            //else if (sse.Expression is SearchedCaseExpression)
            //{
            //    entity.SearchedCase = SearchedCaseExpression(sse.Expression as SearchedCaseExpression);
            //}
            //else if (sse.Expression is IIfCall)
            //{
            //    entity.IIfCall = GetIIfCallExpression(sse.Expression as IIfCall);

            //}
            //else if (sse.Expression is Literal)
            //{
            //    entity.Value = GetLiteral(sse.Expression as Literal);
            //}
            //else if (sse.Expression is FunctionCall)
            //{
            //    entity.FunctionCall = GetFunctionCall(sse.Expression as FunctionCall);
            //}
            //else if (sse.Expression is BinaryExpression)
            //{
            //    entity.BinaryExpression = GetBinaryExpression(sse.Expression as BinaryExpression);
            //}
            //else if (sse.Expression is LeftFunctionCall)
            //{
            //    entity.LeftFunctionCall = GetLeftFunctionCall(sse.Expression as LeftFunctionCall);
            //}
            //else if (sse.Expression is RightFunctionCall)
            //{
            //    entity.RightFunctionCall = GetRightFunctionCall(sse.Expression as RightFunctionCall);
            //}
            //else if (sse.Expression is CastCall)
            //{
            //    entity.CastCall = GetCastCall(source: sse.Expression as CastCall);
            //}
            //else if (sse.Expression is CoalesceExpression)
            //{
            //    entity.Coalesce = GetCoalesce(sse.Expression as CoalesceExpression);
            //}
            //else
            //{
            //    WriteDebug(sse.Expression);
            //}

            return entity;
        }

        private SqlParserWrapper.Model.CoalesceEntity GetCoalesce(CoalesceExpression coalesceExpression)
        {
            SqlParserWrapper.Model.CoalesceEntity entity = new SqlParserWrapper.Model.CoalesceEntity();

            foreach (var item in coalesceExpression.Expressions)
            {
                entity.Expressions.Add(GetScalarExpression(item));
            }


            return entity;
        }

        private SqlParserWrapper.Model.CastCallEntity GetCastCall(CastCall source)
        {
            SqlParserWrapper.Model.CastCallEntity entity = new SqlParserWrapper.Model.CastCallEntity();

            if (source.DataType is SqlDataTypeReference)
            {
                entity.DataType = GetSqlDataTypeReference(source.DataType as SqlDataTypeReference);
            }
            else
            {
                WriteDebug(source.DataType.ToString());
            }

            if (source.Parameter is ColumnReferenceExpression)
            {
                entity.Parameter = GetParameter(source.Parameter as ColumnReferenceExpression);
            }
            else if (source.Parameter is IIfCall )
            {
                entity.Parameter = GetParameter(source.Parameter as IIfCall);
            }
            else if (source.Parameter is SearchedCaseExpression)
            {
                entity.Parameter = GetParameter(source.Parameter as SearchedCaseExpression);
            }
            else if (source.Parameter is CoalesceExpression)
            {
                entity.Parameter = GetParameter(source.Parameter as CoalesceExpression);
            }
            else if (source.Parameter is LeftFunctionCall)
            {
                entity.Parameter = GetParameter(source.Parameter as LeftFunctionCall);
            }
            else if (source.Parameter is UnaryExpression)
            {
                entity.Parameter = GetParameter(source.Parameter as UnaryExpression);
            }
            else
            {
                WriteDebug(source.Parameter.ToString());
            }

            return entity;
        }


        private SqlParserWrapper.Model.TryCastCallEntity GetTryCastCall(TryCastCall source)
        {
            SqlParserWrapper.Model.TryCastCallEntity entity = new SqlParserWrapper.Model.TryCastCallEntity();

            if (source.DataType is SqlDataTypeReference)
            {
                entity.DataType = GetSqlDataTypeReference(source.DataType as SqlDataTypeReference);
            }
            else
            {
                WriteDebug(source.DataType.ToString());
            }

            if (source.Parameter is ColumnReferenceExpression)
            {
                entity.Parameter = GetParameter(source.Parameter as ColumnReferenceExpression);
            }
            else if (source.Parameter is IIfCall)
            {
                entity.Parameter = GetParameter(source.Parameter as IIfCall);
            }
            else if (source.Parameter is SearchedCaseExpression)
            {
                entity.Parameter = GetParameter(source.Parameter as SearchedCaseExpression);
            }
            else if (source.Parameter is CoalesceExpression)
            {
                entity.Parameter = GetParameter(source.Parameter as CoalesceExpression);
            }
            else if (source.Parameter is LeftFunctionCall)
            {
                entity.Parameter = GetParameter(source.Parameter as LeftFunctionCall);
            }
            else if (source.Parameter is ParenthesisExpression)
            {
                entity.Parameter = GetParameter(source.Parameter as ParenthesisExpression);
            }
            else
            {
                WriteDebug(source.Parameter.ToString());
            }

            return entity;
        }


        private SqlParserWrapper.Model.FunctionCallEntity GetFunctionCall(FunctionCall functionCall)
        {
            SqlParserWrapper.Model.FunctionCallEntity outputEntity = new SqlParserWrapper.Model.FunctionCallEntity();

            outputEntity.FunctionName = new Convert.ConvertToIdentifier().Convert(functionCall.FunctionName);

            foreach (ScalarExpression p in functionCall.Parameters)
            {
                ParameterEntity myParam = GetParameter(p);

                outputEntity.Parameters.Add(myParam);

            }



            return outputEntity;
        }

        private ParameterEntity GetParameter(ScalarExpression p)
        {
            ParameterEntity outputEntity = new ParameterEntity();

            if (p is ColumnReferenceExpression)
            {
                outputEntity.ColumnReference = GetColumnReferenceExpression(p as ColumnReferenceExpression);
            }
            else if (p is UnaryExpression)
            {
                outputEntity.UnaryExpression = GetUnaryExpression(p as UnaryExpression);

            }
            else if (p is FunctionCall)
            {
                outputEntity.FunctionCall = GetFunctionCall(p as FunctionCall);
            }
            else if (p is Literal)
            {
                outputEntity.Literal = GetLiteral(p as Literal);
            }
            else if (p is ConvertCall)
            {
                outputEntity.ConvertCall = GetConvertCall(p as ConvertCall);
            }
            else if(p is NullIfExpression)
            {
                outputEntity.NullIfExpression = GetNullIfExpression(p as NullIfExpression);
            }
            else if (p is BinaryExpression)
            {
                outputEntity.BinaryExpression = GetBinaryExpression(p as BinaryExpression);
            }
            else if (p is IIfCall)
            {
                outputEntity.IIfCall = GetIIfCallExpression(p as IIfCall);
            }
            else if (p is CastCall)
            {
                outputEntity.CastCall = GetCastCall(p as CastCall);
            }
            else if (p is ParenthesisExpression)
            {
                outputEntity.ParenthesisExpression = GetParenthesisExpression(source: p as ParenthesisExpression);
            }
            else if (p is SearchedCaseExpression)
            {
                outputEntity.SearchedCaseExpression = SearchedCaseExpression(p as SearchedCaseExpression);
            }
            else if (p is CoalesceExpression)
            {
                outputEntity.CoalesceExpression = GetCoalesce(p as CoalesceExpression);
            }
            else if (p is LeftFunctionCall)
            {
                outputEntity.LeftFunctionCall = GetLeftFunctionCall(p as LeftFunctionCall);
            }
            else if (p is VariableReference)
            {
                outputEntity.VariableReference = GetVariableReference(p as VariableReference);
            }
            else if (p is TryCastCall)
            {
                outputEntity.TryCastCall = GetTryCastCall(p as TryCastCall);
            }
            else
            {
                WriteDebug(p.ToString());
            }

            return outputEntity;
        }

        private SqlParserWrapper.Model.ParenthesisEntity GetParenthesisExpression(ParenthesisExpression source)
        {
            SqlParserWrapper.Model.ParenthesisEntity entity = new SqlParserWrapper.Model.ParenthesisEntity();

            if (source.Expression is BinaryExpression)
            {
                entity.BinaryExpression = GetBinaryExpression(source.Expression as BinaryExpression);
            }
            else if (source.Expression is Literal)
            {
                entity.Literal = GetLiteral(source.Expression as Literal);
            }
            else if (source.Expression is ParenthesisExpression)
            {
                entity.ParenthesisExpression = GetParenthesisExpression(source.Expression as ParenthesisExpression);
            }
            else if (source.Expression is IIfCall)
            {
                entity.IIfCall = GetIIfCallExpression(source.Expression as IIfCall);
            }
            else
            {
                WriteDebug(source.Expression.ToString());
            }

            return entity;
        }

        private NullIfEntity GetNullIfExpression(NullIfExpression nullIfExpression)
        {
            SqlParserWrapper.Model.NullIfEntity entity = new SqlParserWrapper.Model.NullIfEntity();

            entity.FirstExpression = GetExpression(nullIfExpression.FirstExpression);
            entity.SecondExpression= GetExpression(nullIfExpression.SecondExpression);

            return entity;
        }

        private SqlParserWrapper.Model.LeftFunctionCallEntity GetLeftFunctionCall(LeftFunctionCall leftFunctionCall)
        {
            SqlParserWrapper.Model.LeftFunctionCallEntity entity = new LeftFunctionCallEntity();

            foreach (ScalarExpression p in leftFunctionCall.Parameters)
            {
                ParameterEntity myParam = GetParameter(p);

                entity.Parameters.Add(myParam);

            }
            return entity;
        }

        private SqlParserWrapper.Model.RightFunctionCallEntity GetRightFunctionCall(RightFunctionCall RightFunctionCall)
        {
            SqlParserWrapper.Model.RightFunctionCallEntity entity = new RightFunctionCallEntity();

            foreach (ScalarExpression p in RightFunctionCall.Parameters)
            {
                ParameterEntity myParam = GetParameter(p);

                entity.Parameters.Add(myParam);

            }
            return entity;
        }


        private SqlParserWrapper.Model.ConvertCallEntity GetConvertCall(ConvertCall convertCall)
        {
            SqlParserWrapper.Model.ConvertCallEntity entity = new SqlParserWrapper.Model.ConvertCallEntity();

            
            if (convertCall.DataType is SqlDataTypeReference)
            {
                entity.SqlDataType = GetSqlDataTypeReference(convertCall.DataType as SqlDataTypeReference);
            }
            else
            {
                WriteDebug(convertCall.DataType.ToString());
            }

            ParameterEntity myParam = GetParameter(convertCall.Parameter);

            entity.Parameter = myParam;

            return entity;
        }

        private SqlDataTypeEntity GetSqlDataTypeReference(SqlDataTypeReference sqlDataTypeReference)
        {
            SqlParserWrapper.Model.SqlDataTypeEntity entity = new SqlParserWrapper.Model.SqlDataTypeEntity();

            if(sqlDataTypeReference.Name is SchemaObjectName)
            {
                entity.Name = SelectSchemaObjectName(sqlDataTypeReference.Name as SchemaObjectName);
            }
            else
            {
                WriteDebug(sqlDataTypeReference.Name.ToString());
            }

            entity.DataTypeOption = sqlDataTypeReference.SqlDataTypeOption.ToString();


            foreach (ScalarExpression p in sqlDataTypeReference.Parameters)
            {
                ParameterEntity myParam = GetParameter(p);

                entity.Parameters.Add(myParam);

            }

            //            entity.Parameters = GetParameter()

            return entity;
        }


        private SqlParserWrapper.Model.UnaryExpressionEntity GetUnaryExpression(UnaryExpression unaryExpression)
        {
            SqlParserWrapper.Model.UnaryExpressionEntity entity = new UnaryExpressionEntity();

            entity.ExpressionType = ConvertUnaryType(unaryExpression.UnaryExpressionType);

            if (unaryExpression.Expression is Literal)
            {
                entity.LiteralExpression = GetLiteral(unaryExpression.Expression as Literal);
            }
            else
            {
                WriteDebug(unaryExpression.Expression.ToString());
            }

            return entity;
        }

        private string ConvertUnaryType(UnaryExpressionType unaryExpressionType)
        {
            switch (unaryExpressionType)
            {
                case UnaryExpressionType.Positive:
                    return "+";
                case UnaryExpressionType.Negative:
                    return "-";
                case UnaryExpressionType.BitwiseNot:
                    return "~";
            }

            return unaryExpressionType.ToString();
        }

        private SearchCasedEntity SearchedCaseExpression(SearchedCaseExpression searchedCaseExpression)
        {
            SearchCasedEntity searchCasedEntity = new SearchCasedEntity();

            if (searchedCaseExpression.WhenClauses is not null)
            {
                foreach (var whenClause in searchedCaseExpression.WhenClauses)
                {
                    SqlParserWrapper.Model.WhenClauseEntity whenEntity = new WhenClauseEntity();
                    searchCasedEntity.WhenClauses.Add(whenEntity);

                    whenEntity.Expression = GetExpression(whenClause.WhenExpression);
                    whenEntity.Then = GetExpression(whenClause.ThenExpression);
                }
            }

            searchCasedEntity.Else = GetExpression(searchedCaseExpression.ElseExpression);
           
            return searchCasedEntity;
        }

        private LikePredicateEntity GetLikePredicate(LikePredicate likePredicate)
        {
            SqlParserWrapper.Model.LikePredicateEntity entity = new SqlParserWrapper.Model.LikePredicateEntity();

            entity.FirstExpression = GetExpression(likePredicate.FirstExpression);
            entity.SecondExpression = GetExpression(likePredicate.SecondExpression);
            return entity;
        }


        private SqlParserWrapper.Model.BooleanTernaryEntity GetBooleanTernaryExpression(BooleanTernaryExpression booleanTernaryExpression)
        {
            SqlParserWrapper.Model.BooleanTernaryEntity outputEntity = new SqlParserWrapper.Model.BooleanTernaryEntity();

            outputEntity.FirstExpression = GetExpression(booleanTernaryExpression.FirstExpression);
            outputEntity.SecondExpression = GetExpression(booleanTernaryExpression.SecondExpression);
            outputEntity.ThirdExpression = GetExpression(booleanTernaryExpression.ThirdExpression);
            outputEntity.TernaryExpressionType = booleanTernaryExpression.TernaryExpressionType.ToString();
                     
            return outputEntity;
        }

        private SqlParserWrapper.Model.VariableReferenceEntity GetVariableReference(VariableReference variableReference)
        {
            SqlParserWrapper.Model.VariableReferenceEntity entity = new VariableReferenceEntity();

            entity.Name = variableReference.Name;
           

            return entity;
        }

        private SqlParserWrapper.Model.IIfCallEntity GetIIfCallExpression(IIfCall ifCall)
        {
            SqlParserWrapper.Model.IIfCallEntity outputEntity = new IIfCallEntity();

            outputEntity.Predicate = GetExpression(source:ifCall.Predicate);
            outputEntity.Then = GetExpression(source: ifCall.ThenExpression);
            outputEntity.Else = GetExpression(source: ifCall.ElseExpression);

            return outputEntity;
        }

        private ExpressionEntity GetExpression(TSqlFragment source)
        {
            if (source is null)
            {
                return null;
            }
            ExpressionEntity entity = new ExpressionEntity();

            if (source is BooleanComparisonExpression)
            {
                entity.BooleanComparison = GetBooleanComparisonExpression(source as BooleanComparisonExpression);
            }
            else if (source is InPredicate)
            {
                entity.InPredicate = InPredicate(source as InPredicate);
            }
            else if (source is Microsoft.SqlServer.TransactSql.ScriptDom.BooleanBinaryExpression)
            {
                entity.BooleanBinary = GetBooleanBinaryExpression(source as Microsoft.SqlServer.TransactSql.ScriptDom.BooleanBinaryExpression);
            }
            else if (source is BooleanIsNullExpression)
            {
                entity.BooleanIsNull = GetBooleanIsNullExpression(source as BooleanIsNullExpression);
            }
            else if (source is Literal)
            {
                entity.Literal = GetLiteral(source as Literal);
            }
            else if (source is ColumnReferenceExpression)
            {
                entity.ColumReference = GetColumnReferenceExpression(source as ColumnReferenceExpression);
            }
            else if (source is FunctionCall)
            {
                entity.FunctionCall = GetFunctionCall(source as FunctionCall);
            }
            else if (source is BinaryExpression)
            {
                entity.BinaryExpression = GetBinaryExpression(source as BinaryExpression);
            }
            else if (source is ParenthesisExpression)
            {
                entity.BooleanParenthesis = GetParenthesisExpression(source as ParenthesisExpression);
            }
            else if (source is LikePredicate)
            {
                entity.LikePredicate = GetLikePredicate(source as LikePredicate);
            }
            else if (source is BooleanTernaryExpression)
            {
                entity.BooleanTernary = GetBooleanTernaryExpression(source as BooleanTernaryExpression);
            }
            else if (source is BooleanNotExpression)
            {
                entity.BooleanNot = GetBooleanNot(source as BooleanNotExpression);
            }
            else if (source is BooleanBinaryExpression)
            {
                entity.BooleanBinary = GetBooleanBinaryExpression(source as BooleanBinaryExpression);
            }
            else if (source is BooleanParenthesisExpression)
            {
                entity.BooleanParenthesis = this.GetBooleanParenthesisExpression(source as BooleanParenthesisExpression);
            }
            else if (source is LeftFunctionCall)
            {
                entity.LeftFunctionCall = this.GetLeftFunctionCall(source as LeftFunctionCall);
            }
            else if (source is RightFunctionCall)
            {
                entity.RightFunctionCall = this.GetRightFunctionCall(source as RightFunctionCall);
            }
            else if (source is UnaryExpression)
            {
                entity.Unary = GetUnaryExpression(source as UnaryExpression);
            }
            else if (source is VariableReference)
            {
                entity.Variable = GetVariableReference(source as VariableReference);
            }
            else if (source is CastCall)
            {
                entity.CastCall = GetCastCall(source as CastCall);
            }
            else if (source is TryConvertCall)
            {
                entity.TryConvert = GetTryConvert(source as TryConvertCall);
            }
            else if (source is ConvertCall)
            {
                entity.ConvertCall = GetConvertCall(source as ConvertCall);
            }
            else if (source is SearchedCaseExpression)
            {
                entity.SearchCased = SearchedCaseExpression(source as SearchedCaseExpression);
            }
            else if (source is ScalarSubquery)
            {
                entity.ScalarSubquery = GetScalarSubquery(source as ScalarSubquery);
            }
            else if (source is CoalesceExpression)
            {
                entity.Coalesce = GetCoalesce(source as CoalesceExpression);
            }
            else if (source is IIfCall)
            {
                entity.IIfCall = GetIIfCallExpression(source as IIfCall);
            }
            else if (source is BinaryQueryExpression)
            {
                entity.BinaryQuery = GetBinaryQueryExpression(source as BinaryQueryExpression);
            }
            else if (source is QuerySpecification)
            {
                entity.QuerySpecification = GetQuerySpecification(source as QuerySpecification);
            }
            else if (source is NullIfExpression)
            {
                entity.NullIf = GetNullIfExpression(source as NullIfExpression);
            }

            else
            {
                WriteDebug(source.ToString());
            }

            //if ()
            //{
            //    SqlParserWrapper.Model.BooleanComparisonEntity bce = GetBooleanComparisonExpression(booleanBinaryExpression.FirstExpression as BooleanComparisonExpression);

            //    booleanBinaryEntity.FirstExpressionBooleanComparison = bce;
            //}
            //else if (booleanBinaryExpression.FirstExpression is BooleanBinaryExpression)
            //{
            //    booleanBinaryEntity.FirstExpressionBooleanBinary = GetBooleanBinaryExpression(booleanBinaryExpression.FirstExpression as BooleanBinaryExpression);
            //}
            //else if (booleanBinaryExpression.FirstExpression is BooleanIsNullExpression)
            //{
            //    booleanBinaryEntity.FirstExpressionIsNull = GetBooleanIsNullExpression(booleanBinaryExpression.FirstExpression as BooleanIsNullExpression);
            //}
            //else if (booleanBinaryExpression.FirstExpression is BooleanParenthesisExpression)
            //{
            //    booleanBinaryEntity.FirstExpressionBooleanParenthesis = GetBooleanParenthesisExpression(booleanBinaryExpression.FirstExpression as BooleanParenthesisExpression);
            //}
            //else if (booleanBinaryExpression.FirstExpression is LikePredicate)
            //{
            //    booleanBinaryEntity.LikePredicate = GetLikePredicate(booleanBinaryExpression.FirstExpression as LikePredicate);
            //}
            //else if (booleanBinaryExpression.FirstExpression is BooleanTernaryExpression)
            //{
            //    booleanBinaryEntity.FirstExpressionBooleanTernary = GetBooleanTernaryExpression(booleanBinaryExpression.FirstExpression as BooleanTernaryExpression);
            //}
            //else if (booleanBinaryExpression.FirstExpression is InPredicate)
            //{
            //    booleanBinaryEntity.FirstExpressionInPredicate = InPredicate(booleanBinaryExpression.FirstExpression as InPredicate);
            //}

            //else
            //{
            //    WriteDebug(booleanBinaryExpression.FirstExpression);
            //}

            //if (booleanBinaryExpression.SecondExpression is BooleanComparisonExpression)
            //{
            //    booleanBinaryEntity.SecondExpressionBooleanComparison = GetBooleanComparisonExpression(booleanBinaryExpression.SecondExpression as BooleanComparisonExpression);
            //}
            //else if (booleanBinaryExpression.SecondExpression is BooleanIsNullExpression)
            //{
            //    booleanBinaryEntity.SecondExpressonIsNull = GetBooleanIsNullExpression(booleanBinaryExpression.SecondExpression as BooleanIsNullExpression);
            //}
            //else if (booleanBinaryExpression.SecondExpression is BooleanTernaryExpression)
            //{
            //    booleanBinaryEntity.SecondExpressionBooleanTernary = GetBooleanTernaryExpression(booleanBinaryExpression.SecondExpression as BooleanTernaryExpression);
            //}
            //else if (booleanBinaryExpression.SecondExpression is LikePredicate)
            //{
            //    booleanBinaryEntity.SecondExpressionLikePredicate = GetLikePredicate(booleanBinaryExpression.SecondExpression as LikePredicate);
            //}
            //else if (booleanBinaryExpression.SecondExpression is BooleanParenthesisExpression)
            //{
            //    booleanBinaryEntity.SecondExpressionBooleanParenthesis = GetBooleanParenthesisExpression(booleanBinaryExpression.SecondExpression as BooleanParenthesisExpression);
            //}
            //else if (booleanBinaryExpression.SecondExpression is InPredicate)
            //{
            //    booleanBinaryEntity.SecondExpressionInPredicate = InPredicate(booleanBinaryExpression.SecondExpression as InPredicate);
            //}
            //else
            //{
            //    WriteDebug(booleanBinaryExpression.SecondExpression);
            //}


            //if (binaryExpression.FirstExpression is ColumnReferenceExpression)
            //{
            //    entity.FirstExpressionColumnReference = GetColumnReferenceExpression(binaryExpression.FirstExpression as ColumnReferenceExpression);
            //}
            //else if (binaryExpression.FirstExpression is FunctionCall)
            //{
            //    entity.FirstExpressionFunctionCall = GetFunctionCall(binaryExpression.FirstExpression as FunctionCall);
            //}
            //else if (binaryExpression.FirstExpression is NullIfExpression)
            //{
            //    entity.FirstExpressionNullIf = GetNullIfExpression(binaryExpression.FirstExpression as NullIfExpression);
            //}
            //else if (binaryExpression.FirstExpression is Literal)
            //{
            //    entity.FirstExpressionLiteral = GetLiteral(binaryExpression.FirstExpression as Literal);
            //}
            //else if (binaryExpression.FirstExpression is BinaryExpression)
            //{
            //    entity.FirstExpressionBinaryExpression = GetBinaryExpression(binaryExpression.FirstExpression as BinaryExpression);
            //}
            //else if (binaryExpression.FirstExpression is ParenthesisExpression)
            //{
            //    entity.FirstExpressionParenthesis = GetParenthesisExpression(binaryExpression.FirstExpression as ParenthesisExpression);
            //}
            //else
            //{
            //    System.WriteDebug(binaryExpression.FirstExpression);
            //}

            //entity.BinaryExpressionType = ConvertBinaryExpressionType(binaryExpression.BinaryExpressionType);

            //if (binaryExpression.SecondExpression is ColumnReferenceExpression)
            //{
            //    entity.SecondExpressionColumnReference = GetColumnReferenceExpression(binaryExpression.SecondExpression as ColumnReferenceExpression);
            //}
            //else if (binaryExpression.SecondExpression is FunctionCall)
            //{
            //    entity.SecondExpressionFunctionCall = GetFunctionCall(binaryExpression.SecondExpression as FunctionCall);
            //}
            //else if (binaryExpression.SecondExpression is Literal)
            //{
            //    entity.SecondExpressionLiteral = GetLiteral(binaryExpression.SecondExpression as Literal);
            //}
            //else if (binaryExpression.SecondExpression is IIfCall)
            //{
            //    entity.SecondExpressionIIfCall = GetIIfCallExpression(binaryExpression.SecondExpression as IIfCall);
            //}
            //else if (binaryExpression.SecondExpression is ConvertCall)
            //{
            //    entity.SecondExpressionConvertCall = GetConvertCall(binaryExpression.SecondExpression as ConvertCall);
            //}
            //else if (binaryExpression.SecondExpression is SearchedCaseExpression)
            //{
            //    entity.SecondExpressionSearchedCase = SearchedCaseExpression(binaryExpression.SecondExpression as SearchedCaseExpression);
            //}
            //else if (binaryExpression.SecondExpression is ParenthesisExpression)
            //{
            //    entity.SecondExpressionParenthesis = GetParenthesisExpression(binaryExpression.SecondExpression as ParenthesisExpression);
            //}
            //else if (binaryExpression.FirstExpression is BinaryExpression)
            //{
            //    entity.SecondExpressionBinaryExpression = GetBinaryExpression(binaryExpression.SecondExpression as BinaryExpression);
            //}

            //else
            //{
            //    System.WriteDebug(binaryExpression.SecondExpression);
            //}


            //else if (whereClause.SearchCondition is BooleanComparisonExpression)
            //{
            //    entity.BooleanComparsion = this.GetBooleanComparisonExpression(whereClause.SearchCondition as BooleanComparisonExpression);
            //}
            //else if (whereClause.SearchCondition is BooleanIsNullExpression)
            //{
            //    entity.BooleanIsNull = this.GetBooleanIsNullExpression(whereClause.SearchCondition as BooleanIsNullExpression);
            //}
            //else if (whereClause.SearchCondition is InPredicate)
            //{
            //    entity.SearchClauseInPredicate = InPredicate(whereClause.SearchCondition as InPredicate);
            //}
            //else
            //{
            //    WriteDebug(whereClause.SearchCondition);
            //}

            return entity;
        }

        private ScalarSubqueryEntity GetScalarSubquery(ScalarSubquery scalarSubquery)
        {
            SqlParserWrapper.Model.ScalarSubqueryEntity entity = new SqlParserWrapper.Model.ScalarSubqueryEntity();

            entity.QueryExpression = GetQueryExpression(scalarSubquery.QueryExpression);

            return entity;
        }

        private BooleanBinaryEntity GetBooleanBinaryExpression(BooleanBinaryExpression booleanBinaryExpression)
        {
            BooleanBinaryEntity booleanBinaryEntity = new BooleanBinaryEntity();

            booleanBinaryEntity.FirstExpression = GetExpression(booleanBinaryExpression.FirstExpression);
            booleanBinaryEntity.BinaryExpressionType = ConvertBinaryExpressionType(booleanBinaryExpression.BinaryExpressionType);
            booleanBinaryEntity.SecondExpression = GetExpression(booleanBinaryExpression.SecondExpression);

            return booleanBinaryEntity;
        }

        private string ConvertBinaryExpressionType(BooleanBinaryExpressionType binaryExpressionType)
        {
            switch(binaryExpressionType)
            {
                case BooleanBinaryExpressionType.And:
                    return "AND";
                case BooleanBinaryExpressionType.Or:
                    return "OR";
                default:
                    return binaryExpressionType.ToString();
            }
        }

        private SqlParserWrapper.Model.BooleanIsNullEntity GetBooleanIsNullExpression(BooleanIsNullExpression booleanIsNullExpression)
        {
            BooleanIsNullEntity bNull = new BooleanIsNullEntity();

            string output = String.Empty;

            if (booleanIsNullExpression.Expression is ColumnReferenceExpression)
            {
                var entity = GetColumnReferenceExpression(booleanIsNullExpression.Expression as ColumnReferenceExpression);

                bNull.ColumnReference = entity;            
               
            }
            else
            {
                WriteDebug(booleanIsNullExpression.Expression.ToString());
            }

            bNull.IsNot = booleanIsNullExpression.IsNot;

            if (booleanIsNullExpression.IsNot == true)
            {
                output += " IS NOT NULL ";
            }
            else
            {
                output += " IS NULL ";
            }

            return bNull;
        }

        private SqlParserWrapper.Model.InPredicateEntity InPredicate(InPredicate inPredicate)
        {
            InPredicateEntity outputEntity = new InPredicateEntity();

            if (inPredicate.Expression is ColumnReferenceExpression)
            {
                outputEntity.ColumnReference = GetColumnReferenceExpression(inPredicate.Expression as ColumnReferenceExpression);

            }
            else if (inPredicate.Expression is FunctionCall)
            {
                outputEntity.FunctionCall = GetFunctionCall(inPredicate.Expression as FunctionCall);
            }
            else if (inPredicate.Expression is LeftFunctionCall)
            {
                outputEntity.LeftFunctionCall = GetLeftFunctionCall(inPredicate.Expression as LeftFunctionCall);
            }
            else
            {
                WriteDebug(inPredicate.Expression.ToString());
            }

            foreach (var val in inPredicate.Values)
            {
                outputEntity.InValues.Add(GetLiteral(val as Literal));
            }


            return outputEntity;
        }

        private SqlParserWrapper.Model.BooleanComparisonEntity GetBooleanComparisonExpression(BooleanComparisonExpression predicate)
        {
            SqlParserWrapper.Model.BooleanComparisonEntity bce = new BooleanComparisonEntity();

            if (predicate != null)
            {
                bce.FirstExpression = GetExpression(predicate.FirstExpression);
                bce.SecondExpression = GetExpression(predicate.SecondExpression);

                bce.ComparisonType = ConvertToString(predicate.ComparisonType);
            }

            return bce;
        }

        private string ConvertToString(BooleanComparisonType comparisonType)
        {
            return new BooleanComparisonTypeConverter().ConvertToFriendly(comparisonType);
        }

        private TryConvertEntity GetTryConvert(TryConvertCall tryConvertCall)
        {
            SqlParserWrapper.Model.TryConvertEntity entity = new SqlParserWrapper.Model.TryConvertEntity();

            if (tryConvertCall.DataType is SqlDataTypeReference)
            {
                entity.SqlDataType = GetSqlDataTypeReference(tryConvertCall.DataType as SqlDataTypeReference);
            }
            else
            {
                WriteDebug(tryConvertCall.DataType.ToString());
            }

            ParameterEntity myParam = GetParameter(tryConvertCall.Parameter);

            entity.Parameter = myParam;


            return entity;
        }

        private BinaryExpressionEntity GetBinaryExpression(BinaryExpression binaryExpression)
        {
            SqlParserWrapper.Model.BinaryExpressionEntity entity = new SqlParserWrapper.Model.BinaryExpressionEntity();

            entity.FirstExpression = GetExpression(binaryExpression.FirstExpression);
            entity.SecondExpression = GetExpression(binaryExpression.SecondExpression);
            entity.BinaryExpressionType = ConvertBinaryExpressionType(binaryExpression.BinaryExpressionType);

            return entity;
        }

        private string ConvertBinaryExpressionType(BinaryExpressionType binaryExpressionType)
        {
            switch (binaryExpressionType)
            {
                case BinaryExpressionType.Add:
                    return "+";
                case BinaryExpressionType.Subtract:
                    return "-";
                case BinaryExpressionType.Multiply:
                    return "*";
                case BinaryExpressionType.Divide:
                    return "/";
                case BinaryExpressionType.Modulo:
                    return "mod";
                default:
                    return binaryExpressionType.ToString();
            }
        }

        private string GetLiteral(Literal val)
        {
            string output = string.Empty;
            if (val is StringLiteral)
            {
                output = $"'{((StringLiteral)val).Value}'";
            }
            else if (val is IntegerLiteral)
            {
                output = $"{((IntegerLiteral)val).Value}";
            }
            else if (val is NullLiteral)
            {
                if ((val as NullLiteral).LiteralType == LiteralType.Null)
                {
                    output = "NULL";
                }
                else
                { 

                }

            }

            return output;
        }

        private string IdentifierOrValueExpression(Microsoft.SqlServer.TransactSql.ScriptDom.IdentifierOrValueExpression expression)
        {
            string output = "";

            output += expression.Value;

            return output;
        }

        private ColumnReferenceEntity  GetColumnReferenceExpression(ColumnReferenceExpression cref)
        {
            ColumnReferenceEntity entity = new ColumnReferenceEntity();

            if (cref == null)
                return null;

            entity.ColumnType = cref.ColumnType.ToString();
            entity.Value = MultiPartIdentifier(cref.MultiPartIdentifier);

            if (cref.MultiPartIdentifier is not null)
            {
                foreach (var item in cref.MultiPartIdentifier.Identifiers)
                {
                    var identifer = new IdentiferEntity();

                    identifer.QuoteType = item.QuoteType.ToString();
                    identifer.Value = item.Value;

                    entity.MultiPart.Add(identifer);

                }
            }


            return entity;
        }

        private string MultiPartIdentifier(MultiPartIdentifier multiPartIdentifier)
        {
            string output = "";

            if (multiPartIdentifier != null)
            {

                foreach (var val in multiPartIdentifier.Identifiers)
                {
                    output += val.Value;
                    output += ".";
                }
            }

            return output.Trim('.');
        }


        public override void ExplicitVisit(ExternalTableColumnDefinition node)
        {
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(ColumnDefinition node)
        {
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(CreateProcedureStatement node)
        {
            ProcessStoredProcedure(node);

            base.ExplicitVisit(node);
        }

        private void ProcessStoredProcedure(CreateProcedureStatement node)
        {
            SqlParserWrapper.Model.StoredProcedureEntity entity = new SqlParserWrapper.Model.StoredProcedureEntity();

            if (node.ProcedureReference is ProcedureReference)
            {
                entity.ProcedureReference = GetProcedureReference(source:node.ProcedureReference as ProcedureReference);

                _queryProcesser.Process(entity);

            }
            else
            {

            }
        }

        private ProcedureReferenceEntity GetProcedureReference(ProcedureReference source)
        {
            SqlParserWrapper.Model.ProcedureReferenceEntity entity = new SqlParserWrapper.Model.ProcedureReferenceEntity();

            entity.Name = SelectSchemaObjectName(source.Name);


            return entity;
        }

        private void DebugIdentifer(Identifier id)
        {
            WriteDebug(id.Value);
        }

        public override void ExplicitVisit(CreateTableStatement node)
        {

        }
    }
}
