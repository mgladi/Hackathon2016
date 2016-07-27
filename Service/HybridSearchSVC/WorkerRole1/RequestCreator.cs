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
        private const string agentQuery_QueryStringKeyFile = "p";

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

                return new AgentPollRequest(agentsPendingDb, clientsDb, StringToGuid(customerId), Guid.Parse(agentId));
            }
            else if (path.StartsWith("/GetSearch"))
            {
                string customerId = request.Headers["CustomerId"];
                string query = request.QueryString[agentQuery_QueryStringKey];              
                
                return new SearchRequest(searchesDb, StringToGuid(customerId), query);
            }
            else if (path.StartsWith("/GetFile"))
            {
                string customerId = request.Headers["CustomerId"];
                string agentId = request.Headers["AgentId"];
                string query = request.QueryString[agentQuery_QueryStringKeyFile];

                return new FileRequest(searchesDb, StringToGuid(customerId), Guid.Parse(agentId), query);
            }
            else if (path.StartsWith("/PostResults"))
            {

                string customerId = request.Headers["CustomerId"];
                string agentId = request.Headers["AgentId"];
                string searchId = request.Headers["RequestId"];
                string deviceType = request.Headers["DeviceType"];
                string deviceName = request.Headers["DeviceName"];

                string content = HttpHelper.GetRequestPostData(request);
                return new PostResultsRequest(searchesDb, StringToGuid(customerId), Guid.Parse(agentId), Guid.Parse(searchId), deviceType, deviceName, content);
            }
            else if (path.StartsWith("/Register"))
            {
                string customerId = request.Headers["CustomerId"];
                string content = request.Headers["agentId"];
                string deviceName = request.Headers["deviceName"];
                string deviceType = request.Headers["deviceType"];

                return new RegisterRequest(clientsDb, StringToGuid(customerId), deviceName, deviceType);
            }
            return new ErrorRequest();
        }


        private static Guid StringToGuid(string src)
        {
            byte[] stringbytes = Encoding.UTF8.GetBytes(src);
            byte[] hashedBytes = new System.Security.Cryptography
                .SHA1CryptoServiceProvider()
                .ComputeHash(stringbytes);
            Array.Resize(ref hashedBytes, 16);
            return new Guid(hashedBytes);
        }
    }
}
