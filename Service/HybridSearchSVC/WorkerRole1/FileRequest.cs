using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class FileRequest : IRequest
    {
        private const int maxIterations = 150;
        private const int iterationDelayMilliseconds = 200;

        private const string type = "file";

        private readonly Guid customerId;
        private readonly Guid agentId;
        private Guid searchId;

        private readonly string query;
        private readonly ISearchesDB searchesDB;

        public FileRequest(ISearchesDB searchesDB, Guid customerId, Guid agentId, string query)
        {
            this.searchesDB = searchesDB;
            this.customerId = customerId;
            this.agentId = agentId;
            this.query = query;
        }

        public async Task ProcessRequest(HttpListenerContext context)
        {
            Console.WriteLine("Got request of type SearchRequest");

            // initiate new search
            var searchId = searchesDB.CreateNewSearch(this.customerId, this.query, type);

            // wait for results
            await GetSearchResults(context.Response, this.customerId, this.agentId, searchId);
        }

        private async Task GetSearchResults(HttpListenerResponse response, Guid customerId, Guid agentId, Guid searchId)
        {
            Console.WriteLine("Starting wait for file {0}", searchId);
            await Task.Run(async () =>
            {
                for (int i = 0; i < maxIterations; i++)
                {
                    if (searchesDB.IsAwaitingFile(customerId, searchId, agentId))
                    {
                        await Task.Delay(iterationDelayMilliseconds);

                    }
                    else
                    {
                        Console.WriteLine("File ready after {0} iterations", i);
                        break;
                    }
                }

                SearchResults results = searchesDB.GetSearchResults(customerId, searchId, type);
                HttpHelper.SendObject(response, results);
            });

            Console.WriteLine("Finished wait for file {0}", searchId);
        }
    }
}
