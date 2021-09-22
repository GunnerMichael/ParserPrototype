using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Convert
{
    public class BooleanComparisonTypeConverter
    {
        public string ConvertToFriendly(BooleanComparisonType comparisonType)
        {
            switch (comparisonType)
            {
                case BooleanComparisonType.Equals:
                    return "Equals";
                case BooleanComparisonType.GreaterThan:
                    return "Greater than";
                case BooleanComparisonType.GreaterThanOrEqualTo:
                    return "Greater than or equal to";
                case BooleanComparisonType.LessThan:
                    return "Less than";
                case BooleanComparisonType.LessThanOrEqualTo:
                    return "Less than or Equal to";
                case BooleanComparisonType.NotEqualToBrackets:
                    return "Not equal to";
                case BooleanComparisonType.NotEqualToExclamation:
                    return "Not equal to";
                case BooleanComparisonType.NotGreaterThan:
                    return "Not greater than";
                case BooleanComparisonType.NotLessThan:
                    return "Not less than";
                default:
                    return comparisonType.ToString();

            }
        }


    }
}
