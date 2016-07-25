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
        public static IClientsDB clientsDb = new ClientsDB();
        public static IAgentsPendingDB agentsPendingDb = new AgentsPendingDB();
        public static ISearchesDB searchesDb = new SearchesDB(clientsDb, agentsPendingDb);

        public static IRequest CreateRequest(HttpListenerRequest request)
        {
            string path = request.Url.PathAndQuery;
            if (path.StartsWith("/AgentPoll"))
            {
                string customerId = request.Headers["CustomerId"];
                string agentId = request.Headers["AgentId"];

                return new AgentPollRequest(agentsPendingDb, clientsDb, Guid.Parse(customerId), Guid.Parse(agentId));
            }
            else if (path.StartsWith("/GetSearch"))
            {
                string customerId = request.Headers["CustomerId"];

                string query = request.QueryString["Query"];
                
                return new SearchRequest(searchesDb, Guid.Parse(customerId), query);
            }
            else if (path.StartsWith("/PostResults"))
            {

                string customerId = request.Headers["CustomerId"];
                string agentId = request.Headers["AgentId"];
                string searchId = request.Headers["SearchId"];

                string content = HttpHelper.GetRequestPostData(request);
                return new PostResultsRequest(searchesDb, Guid.Parse(customerId), Guid.Parse(agentId), Guid.Parse(searchId), content);
            }
            return new ErrorRequest();
        }
    }
}
