using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class SearchQuery
    {
        public Guid requestId { get; set; }
        public string query { get; set; }
        public string type { get; set; }
        public SearchQuery(Guid requestId, string query, string type)
        {
            this.requestId = requestId;
            this.query = query;
            this.type = type;
        }
    }
}
