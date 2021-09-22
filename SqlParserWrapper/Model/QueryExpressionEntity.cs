using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class QueryExpressionEntity
    {
        public QuerySpecificationEntity QuerySpecification { get; set;}
        public BinaryQueryEntity BinaryQuery { get; set; }
    }
 }
