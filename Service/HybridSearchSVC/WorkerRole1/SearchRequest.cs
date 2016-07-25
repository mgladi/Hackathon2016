using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class SearchRequest : IRequest
    {
        private const int maxIterations = 50;
        private const int iterationDelayMilliseconds = 200;

        private readonly Guid clientId;
        private Guid searchId;

        private readonly string content;
        private readonly ISearchesDB searchesDB;

        public SearchRequest(ISearchesDB searchesDB, Guid customerId, string content)
        {
            this.searchesDB = searchesDB;
            this.clientId = customerId;
            this.content = content;
        }

        public async Task ProcessRequest(HttpListenerContext context)
        {
            Console.WriteLine("Got request of type SearchRequest");
            const string clientIdKeyName = "clientId";
            const string agentQueryKeyName = "q";

            Console.WriteLine("Got request of type SearchRequest");

            // get client id and query
            string clientId = context.Request.QueryString[clientIdKeyName];
            string query = context.Request.QueryString[agentQueryKeyName];

            Guid clientIdGuid;
            Guid.TryParse(clientId, out clientIdGuid);

            // initiate new search
            var searchId = searchesDB.CreateNewSearch(clientIdGuid, query);

            // wait for results
            await GetSearchResults(context.Response, clientIdGuid, searchId);
        }

        private async Task GetSearchResults(HttpListenerResponse response, Guid customerId, Guid searchId)
        {
            Console.WriteLine("Starting wait for search results for search {0}", searchId);
            await Task.Run(async () => 
            {               
                for (int i = 0; i < maxIterations; i++)
                {
                    if (searchesDB.IsAwaitingResults(customerId, searchId))
                    {
                        await Task.Delay(iterationDelayMilliseconds);

                    }
                    else
                    {
                        Console.WriteLine("All search results ready after {0} iterations", i);
                        break;
                    }
                }

                SearchResults results = searchesDB.GetSearchResults(customerId, searchId);
                HttpHelper.SendObject(response, results);
            });

            Console.WriteLine("Finished wait for search results for search {0}", searchId);
        }
    }
}
