using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParserWrapper.Model
{
    public class SchemaObjectEntity
    {
        private List<IdentiferEntity> _identifers = new List<IdentiferEntity>();
        public List<IdentiferEntity> Identifers
        {
            get
            {
                return _identifers;
            }
        }

        public string FullName
        {
            get
            {
                string output= "";

                foreach (var val in _identifers)
                {
                    output += val.Value;
                    output += ".";
                }

                return output.Trim('.');
            }
        }

    }
}
