
namespace SqlParserWrapper.Model
{
    public class AliasEntity
    {
        public IdentiferEntity Identifer { get; set; }
        //public QuoteType QuoteType { get; set;}
        //public string Value { get; set;}

        public string AliasName
        {
            get
            {
                string alias = string.Empty;

                if (Identifer != null)
                {
                    return Identifer.Value;
                }

                return alias;
            }
        }
    }
}