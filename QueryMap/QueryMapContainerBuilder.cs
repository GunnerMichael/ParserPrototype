using SqlParserWrapper;
using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryMap
{
    public class QueryMapContainerBuilder
    {
        public QueryMapContainer Build(ParsedContainer input)
        {
            QueryMapContainer build = new QueryMapContainer();

            if (input != null)
            {
                var qe = input.Queries;
                var cte = input.CteTables;

                foreach (var item in qe)
                {
                    build.Sections.Add(GetResultsetContainer(item));
                }

                foreach (var item in cte)
                {
                    build.Sections.Add(GetCteContainer(item));
                }

                if (input.Comments.Count > 0)
                {
                    build.Sections.Add(new OverviewSection() { Overview = input.Comments[0].Text });
                }

                if (input.StoredProcedure != null)
                {
                    build.Sections.Add(GetHeaderSection(input.StoredProcedure));
                }
            }

            return build;
        }

        private SectionContainer GetHeaderSection(StoredProcedureEntity storedProcedure)
        {
            HeaderSection section = new HeaderSection();

            section.Title = storedProcedure.ProcedureReference.Name.FullName.Replace("usp", string.Empty);

            return section;
        }

        private ResultsetContainer GetResultsetContainer(QueryExpressionEntity item)
        {
            ResultsetContainer container = new ResultsetContainer();

            if (item is not null)
            {
                QueryContainer qc = ShowQueryExpressionEntity(item);

                container.Queries.Add(qc);
            }
            else
            {

            }

            return container;
        }

        private CteContainer GetCteContainer(CommonTableEntity item)
        {
            CteContainer container = new CteContainer();

            container.CteName = item.Name;

            if (item.QueryExpression is not null)
            {
                QueryContainer qc = ShowQueryExpressionEntity(item.QueryExpression);

                container.Queries.Add(qc);              
            }
            else
            {

            }

            return container;
        }

        private QueryContainer ShowQueryExpressionEntity(QueryExpressionEntity item)
        {
            QueryContainer qc = new QueryContainer();

            if (item.BinaryQuery != null)
            {
                qc.Queries.Add(ShowBinaryQuery(item.BinaryQuery));
            }

            if (item.QuerySpecification != null)
            {
                qc.Queries.Add(ShowQuerySpec(item.QuerySpecification));
            }

            return qc;
        }

        private QueryContainer ShowBinaryQuery(BinaryQueryEntity binaryQuery, UnionContainer qc = null, bool isUnion = false)
        {
            if (qc is null)
            {
                qc = new UnionContainer();
            }

            if (binaryQuery.BinaryQueryExpressionType == "Union")
            {
                isUnion = true;
            }

            if (binaryQuery.FirstQuery.BinaryQuery is not null)
            {
                ShowBinaryQuery(binaryQuery.FirstQuery.BinaryQuery,qc);
            }
            else if (binaryQuery.FirstQuery.QuerySpecification is not null)
            {
                qc.Queries.Add(ShowQuerySpec(binaryQuery.FirstQuery.QuerySpecification, isUnion));
            }
            else
            {
            }

            if (binaryQuery.SecondQuery.QuerySpecification is not null)
            {
                qc.Queries.Add(ShowQuerySpec(binaryQuery.SecondQuery.QuerySpecification, isUnion));
            }
            else
            {

            }

            return qc;
        }

        private QueryContainer ShowQuerySpec(QuerySpecificationEntity querySpecification, bool isUnion = false)
        {
            QueryContainer qc = new QueryContainer();
            qc.ContainsUnion = isUnion;

            qc.Name = querySpecification.Name;

            qc.SelectColumns.AddRange(MapColumns(querySpecification.Columns, querySpecification));

            qc.FromClause = ShowFromClause(querySpecification.FromClause, querySpecification);

            qc.WhereClause = GetWhereClause(querySpecification.WhereClause, querySpecification);

            return qc;
        }

        private WhereClauseContainer GetWhereClause(WhereClauseEntity whereClause, QuerySpecificationEntity querySpecification)
        {
            WhereClauseContainer container = new WhereClauseContainer();

            if (whereClause.SearchCondition is not null)
            {
                container.WhereText = new ColumnValueBuilder(querySpecification).BuildOutput(whereClause.SearchCondition);
            }
            else
            {

            }

            return container;
        }

        private List<ColumnContainer> MapColumns(List<SelectColumnEntity> columns, QuerySpecificationEntity querySpecification)
        {
            List<ColumnContainer> cc = new List<ColumnContainer>();
            foreach (var item in columns)
            {
                cc.Add(new ColumnContainer()
                {
                    Name = item.ColumnName,
                    Value = GetColValue(item, querySpecification)
                });
            }

            return cc;
        }

        private string GetColValue(SelectColumnEntity col, QuerySpecificationEntity querySpecification)
        {
            string val = new ColumnValueBuilder(querySpecification).BuildOutput(col.Expression);

            return val;
        }

        private FromClauseContainer ShowFromClause(FromClauseEntity fromClause, QuerySpecificationEntity querySpecification)
        {
            FromClauseContainer entity = new FromClauseContainer();
            if (fromClause == null)
            {
                return null;
            }

            foreach (var item in fromClause.GetUniqueTables().OrderBy(x => x.GetName()))
            {
                entity.Tables.Add(MapTable(item));
            }

            entity.Tables.AddRange(MapDerivedQuery(fromClause, querySpecification));

            string output = GetQualifedJoins(fromClause.QualifiedJoin,querySpecification);

            entity.JoinQuery = output;

            // entity.JoinMapItem.AddRange(GetJoins(fromClause.QualifiedJoin, querySpecification));

            //string x = new ColumnValueBuilder(querySpecification).BuildOutput(fromClause.QualifiedJoin.SearchCondition);

            return entity;
        }

        private IEnumerable<TableContainer> MapDerivedQuery(FromClauseEntity fromClause, QuerySpecificationEntity querySpecification)
        {
            List<TableContainer> containers = new List<TableContainer>();

            var x = (from t in fromClause.Tables
                    where t.QueryDerivedTable is not null
                    select t).ToList();


            if (x.Count > 0)
            {
                foreach(var item in x)
                {
                    if (item.QueryDerivedTable.QueryExpression != null && item.QueryDerivedTable.QueryExpression.QuerySpecification != null)
                    {
                        foreach (var t in item.QueryDerivedTable.QueryExpression.QuerySpecification.FromClause.GetUniqueTables())
                        {
                            containers.Add(MapTable(t));
                        }
                    }
                }
            }

            return containers;
        }

        private List<JoinMapItem> GetJoins(QualifiedJoinEntity qualifiedJoin, QuerySpecificationEntity querySpecification)
        {
            List<JoinMapItem> items = new List<JoinMapItem>();

            if (qualifiedJoin != null)
            {
                if (qualifiedJoin.First != null && qualifiedJoin.First.QualifiedJoin is not null)
                {
                    items.AddRange(GetJoins(qualifiedJoin.First.QualifiedJoin, querySpecification));
                }

                if (qualifiedJoin.Second != null && qualifiedJoin.Second.QualifiedJoin is not null)
                {
                    items.AddRange(GetJoins(qualifiedJoin.Second.QualifiedJoin, querySpecification));
                }

               items.Add(new JoinMapBuilder(querySpecification).BuildOutput(qualifiedJoin.SearchCondition));

            }


            return items;
        }

        private string GetQualifedJoins(QualifiedJoinEntity qualifiedJoin, QuerySpecificationEntity querySpecification)
        {
            string output = string.Empty;

            if (qualifiedJoin != null)
            {
                if (qualifiedJoin.First != null && qualifiedJoin.First.QualifiedJoin is not null)
                {
                    output += GetQualifedJoins(qualifiedJoin.First.QualifiedJoin, querySpecification);
                }

                if (qualifiedJoin.Second != null && qualifiedJoin.Second.QualifiedJoin is not null)
                {
                    output += GetQualifedJoins(qualifiedJoin.Second.QualifiedJoin, querySpecification);
                }

                output += new ColumnValueBuilder(querySpecification).BuildOutput(qualifiedJoin.SearchCondition);
                output += "\r\n";
            }

            return output;
        }


        private TableContainer MapTable(TableEntity item)
        {
            TableContainer container = new TableContainer();


            if (item != null && item.TableReference != null && item.TableReference.NamedTable != null)
            {
                container.Alias = item.TableReference.NamedTable.Alias;
                container.FullName = item.TableReference.NamedTable.SchemaObject.FullName;
                container.Identifers.AddRange(MapIdentifers(item.TableReference.NamedTable.SchemaObject.Identifers));
            }
            else
            {

            }

            return container;
        }

        private List<IdentifierContainer> MapIdentifers(List<IdentiferEntity> identifers)
        {
            List<IdentifierContainer> items = new List<IdentifierContainer>();

            foreach(var item in identifers)
            {
                items.Add(new IdentifierContainer()
                {
                    Value = item.Value,
                    QuoteType = item.QuoteType
                }); ; 
            }

            return items;
        }
    }
}
