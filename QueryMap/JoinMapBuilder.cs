using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;

namespace QueryMap
{
    internal class JoinMapBuilder
    {
        private QuerySpecificationEntity querySpecification;

        public JoinMapBuilder(QuerySpecificationEntity querySpecification)
        {
            this.querySpecification = querySpecification;
        }

        internal JoinMapItem BuildOutput(ExpressionEntity source)
        {
            JoinMapItem item = new JoinMapItem();

            if (source.ColumReference is not null)
            {
                item.ColumnReference = GetColumnReferenceEntity(source.ColumReference);
            }
            else if (source.BooleanComparison is not null)
            {
                item.JoinMap = GetBooleanComparison(source: source.BooleanComparison);
            }
            else if (source.BooleanBinary is not null)
            {
                item.JoinMap = GetBooleanBinary(source: source.BooleanBinary);
            }
            else
            {

            }

            return item;

        }

        private JoinMapItem GetBooleanBinary(BooleanBinaryEntity source)
        {
            JoinMapItem item = new JoinMapItem();

            item.First = BuildOutput(source.FirstExpression);

            item.Second = BuildOutput(source.SecondExpression);

            return item;
        }

        private JoinMapItem GetBooleanComparison(BooleanComparisonEntity source)
        {
            JoinMapItem item = new JoinMapItem();

            item.First = BuildOutput(source.FirstExpression);

            item.Second = BuildOutput(source.SecondExpression);

            return item;
        }

        private ColumnContainer GetColumnReferenceEntity(ColumnReferenceEntity columReference)
        {
            return GetIdentifer(columReference.MultiPart);
        }

        protected ColumnContainer GetIdentifer(List<IdentiferEntity> multiPart)
        {
            ColumnContainer col = new ColumnContainer();
            string output = String.Empty;
            int index = multiPart.Count - 2;

            int count = 0;
            foreach (var item in multiPart)
            {
                if (count == index)
                {
                    col.SourceObject =  GetSchemaObjectFromAlias(item.Value);
                    col.SourceAlias = item.GetValue();
                }
                else
                {
                    col.Name += item.GetValue();
                }
                col.Name += ".";
                count++;
            }

            col.Name = col.Name.Trim('.');

            return col;
        }

        private string GetSchemaObjectFromAlias(string value)
        {
            string result = string.Empty;

            if (querySpecification != null && querySpecification.FromClause != null)
            {
                var table = querySpecification.FromClause.FindTableFromAlias(value,true);

                if (table != null)
                {
                    result = table.GetName();
                }

            }

            return result;
        }


    }
}