using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class SearchQuery
    {
        private Guid searchId;
        private string query;
        public SearchQuery(Guid searchId, string query)
        {
            this.searchId = searchId;
            this.query = query;
        }
    }
}
