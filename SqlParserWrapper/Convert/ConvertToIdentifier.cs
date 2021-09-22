using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlParserWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Convert
{
    public class ConvertToIdentifier
    {
        public IdentiferEntity Convert(Identifier source)
        {
            var dest = new IdentiferEntity();

            dest.QuoteType = source.QuoteType.ToString();
            dest.Value = source.Value;

            return dest;
        }
    }
}
