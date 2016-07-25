using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkerRole1;

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

        public void ProcessRequest(HttpListenerContext context)
        {
            IAgentsPendingDB agentsPending;
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
        }
    }
}
