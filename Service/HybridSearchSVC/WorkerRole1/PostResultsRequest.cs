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
        private readonly Guid customerId;
        private readonly Guid agentId;
        private readonly Guid searchId;
        private readonly string content;
        private readonly ISearchesDB searchesDb;

        public PostResultsRequest(ISearchesDB searchesDb, Guid customerId, Guid agentId, Guid searchId, string content)
        {
            this.searchesDb = searchesDb;
            this.customerId = customerId;
            this.agentId = agentId;
            this.searchId = searchId;
            this.content = content;
        }

        public Task ProcessRequest(HttpListenerContext context)
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(this.content);
            AgentResult result = new AgentResult(contentBytes);
            searchesDb.UpdateSearch(this.searchId, this.agentId, result);
            return Task.FromResult(0);

        }
    }
}
