using HybridSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace CrossSearch
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

        public void ProcessRequest(HttpListenerContext context)
        {
            HttpHelper.GetRequestPostData(context.Request);
        }
    }
}
