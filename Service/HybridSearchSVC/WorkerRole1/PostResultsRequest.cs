using HybridSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace HybridSearch
{
    class PostResultsRequest : IRequest
    {
        Guid customerId;
        Guid agentId;
        Guid searchId;
        string content;

        public PostResultsRequest(Guid customerId, Guid agentId, Guid searchId, string content)
        {
            this.customerId = customerId;
            this.agentId = agentId;
            this.searchId = searchId;
            this.content = content;
        }

        public Task ProcessRequest(HttpListenerContext context)
        {
            ISearchesDB db = new SearchesDB2();
            byte[] contentBytes = Encoding.UTF8.GetBytes(this.content);
            db.UpdateSearch(this.searchId, this.agentId, contentBytes);
            return Task.FromResult(0);

        }
    }
}
