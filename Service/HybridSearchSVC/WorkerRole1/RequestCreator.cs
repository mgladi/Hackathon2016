using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class RequestCreator
    {
        private const string agentQuery_QueryStringKey = "q";

        public static IRequest CreateRequest(HttpListenerRequest request)
        {
            string path = request.Url.PathAndQuery;
            if (path.StartsWith("/AgentPoll"))
            {
                string customerId = request.Headers["CustomerId"];
                string agentId = request.Headers["AgentId"];

                return new AgentPollRequest(Guid.Parse(customerId), Guid.Parse(agentId));
            }
            else if (path.StartsWith("/GetSearch"))
            {
                string customerId = request.Headers["CustomerId"];
                string query = request.QueryString[agentQuery_QueryStringKey];
                
                return new SearchRequest(Guid.Parse(customerId), query);
            }
            else if (path.StartsWith("/PostResults"))
            {

                string customerId = request.Headers["CustomerId"];
                string agentId = request.Headers["AgentId"];
                string searchId = request.Headers["SearchId"];

                string content = HttpHelper.GetRequestPostData(request);
                return new SearchRequest(Guid.Parse(customerId), content);
            }
            return new ErrorRequest();
        }
    }
}
