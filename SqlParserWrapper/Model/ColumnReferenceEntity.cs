using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class ColumnReferenceEntity
    {
        private List<IdentiferEntity> multiPart = new List<IdentiferEntity>();
        public string ColumnType { get; set;}
        public string Value { get; set;}

        public List<IdentiferEntity> MultiPart
        {
            get
            {
                return multiPart;
            }
        }

        //public string GetColumnName()
        //{
        //    if (MultiPart.Count > 0)
        //    {
        //        var part = MultiPart[MultiPart.Count - 1];

        //        if (part.QuoteType == Microsoft.SqlServer.TransactSql.ScriptDom.QuoteType.SquareBracket)
        //        {
        //            return $"[{part.Value}]";
        //        }
        //        else if (part.QuoteType == Microsoft.SqlServer.TransactSql.ScriptDom.QuoteType.DoubleQuote)
        //        {
        //            return $"\"{part.Value}\"";
        //        }
        //        else
        //        {
        //            return part.Value;
        //        }
        //    }
        //    else if(ColumnType.ToLower() == "wildcard")
        //    {
        //        return "*";
        //    }
        //    else
        //    {
        //        return String.Empty;
        //    }
        //}
    }
}
