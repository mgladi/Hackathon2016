using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

        public void ProcessRequest(HttpListenerContext context)
        {
            HttpHelper.SendString(context.Response, "your customer id is " + this.customerId.ToString());
        }
    }
}
