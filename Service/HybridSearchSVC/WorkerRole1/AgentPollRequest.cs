using System;
using System.Net;
using System.Threading.Tasks;

namespace HybridSearch
{
    class AgentPollRequest : IRequest
    {
        private readonly Guid customerId;
        private readonly Guid agentId;
        private readonly IAgentsPendingDB agentsPendingDb;
        private readonly IClientsDB clientsDb;
        public AgentPollRequest(IAgentsPendingDB agentsPendingDb, IClientsDB clientsDb, Guid customerId, Guid agentId)
        {
            this.agentsPendingDb = agentsPendingDb;
            this.clientsDb = clientsDb;
            this.customerId = customerId;
            this.agentId = agentId;
        }

        public Task ProcessRequest(HttpListenerContext context)
        {
            SearchQuery nextSearchQuery;
            clientsDb.CreateNewAgentByID(this.customerId, this.agentId, "content");

            bool isNextQueryAvailable = this.agentsPendingDb.TryGetNextQuery(this.agentId, out nextSearchQuery);
            if (isNextQueryAvailable)
            {
                HttpHelper.SendObject(context.Response, nextSearchQuery);
            }
            else
            {
                HttpHelper.SendEmpty(context.Response);
            }

            return Task.FromResult(0);
        }
    }
}
