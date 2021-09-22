using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class QualifiedJoinEntity
    {
        private TableEntity first = new TableEntity();
        private TableEntity second = new TableEntity();

        public TableEntity First { get => first;  }
        public TableEntity Second { get => second; }

        public ExpressionEntity SearchCondition { get; set; }

    }
}
