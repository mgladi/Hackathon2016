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

        private readonly Guid customerId;
        private Guid searchId;

        private readonly string content;

        static private ISearchesDB searchesDB = new SearchesDB2();

        public SearchRequest(Guid customerId, string content)
        {
            this.customerId = customerId;
            this.content = content;
        }

        public async Task ProcessRequest(HttpListenerContext context)
        {
            Console.WriteLine("Got request of type SearchRequest");
            const string customerIdKeyName = "customerId";
            const string agentQueryKeyName = "q";

            Console.WriteLine("Got request of type SearchRequest");

            // get client id and query
            string customerId = context.Request.QueryString[customerIdKeyName];
            string query = context.Request.QueryString[agentQueryKeyName];

            Guid customerIdGuid;
            Guid.TryParse(customerId, out customerIdGuid);

            // initiate new search
            var searchId = searchesDB.CreateNewSearch(customerIdGuid, query);

            // wait for results
            await GetSearchResults(context.Response, customerIdGuid, searchId);
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
