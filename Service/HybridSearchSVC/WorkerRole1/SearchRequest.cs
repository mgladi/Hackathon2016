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
        private const int maxIterations = 300;
        private const int iterationDelayMilliseconds = 100;

        private readonly Guid customerId;

        //private Guid searchId;
        private const string type = "search";

		private readonly string query;
        private readonly ISearchesDB searchesDB;

        public SearchRequest(ISearchesDB searchesDB, Guid customerId, string query)
        {
            this.searchesDB = searchesDB;
            this.customerId = customerId;
            this.query = query;
        }

        public async Task ProcessRequest(HttpListenerContext context)
        {
            Console.WriteLine("Got request of type SearchRequest");

            // initiate new search
            var searchId = searchesDB.CreateNewSearch(this.customerId, this.query, type);

            // wait for results
            await GetSearchResults(context.Response, this.customerId, searchId);
        }

        private async Task GetSearchResults(HttpListenerResponse response, Guid customerId, Guid searchId)
        {
            Console.WriteLine("Starting wait for search results for search {0}", searchId);
            bool searchCompletedSuccessfully = false;

            for (int i = 0; i < maxIterations; i++)
            {
                if (searchesDB.IsAwaitingResults(customerId, searchId))
                {
                    await Task.Delay(iterationDelayMilliseconds);

                }
                else
                {
                    Console.WriteLine("All search results ready after {0} iterations", i);
                    searchCompletedSuccessfully = true;
                    break;
                }
            }

            SearchResults results = searchesDB.GetSearchResults(customerId, searchId, type);
            if (searchCompletedSuccessfully)
            {
                HttpHelper.SendObject(response, results);
            }
            else
            {
                HttpHelper.SendObjectWithError(response, results);
            }

            Console.WriteLine("Finished wait for search results for search {0}", searchId);
        }
    }
}
