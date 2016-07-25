using System;
using System.Net;
using System.Threading.Tasks;

namespace HybridSearch
{
    class AgentPollRequest : IRequest
    {
        private readonly Guid customerId;
        private readonly Guid agentId;
        public AgentPollRequest(Guid customerId, Guid agentId)
        {
            this.customerId = customerId;
            this.agentId = agentId;
        }

        public Task ProcessRequest(HttpListenerContext context)
        {

            IAgentsPendingDB agentsPending = new AgentsPendingDB2();
            SearchQuery nextSearchQuery;
            bool isNextQueryAvailable = agentsPending.TryGetNextQuery(this.agentId, out nextSearchQuery);
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
