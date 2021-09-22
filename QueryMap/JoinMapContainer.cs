using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryMap
{
    public class JoinMapContainer
    {
        private Dictionary<string, JoinMapItem> _joinmap = new Dictionary<string, JoinMapItem>();

        public Dictionary<string, JoinMapItem> JoinMap
        {
            get
            {
                return _joinmap;
            }
        }
    }
}
