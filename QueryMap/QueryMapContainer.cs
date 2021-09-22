using System;
using System.Collections.Generic;

namespace QueryMap
{
    public class QueryMapContainer  
    {
        private List<SectionContainer> _sections = new List<SectionContainer>();

        public List<SectionContainer> Sections
        {
            get
            {
                return _sections;
            }
        }

    }
}
