using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public enum RequestType { Search, File };

    public class SearchesDB : ISearchesDB
    {
        private IDictionary<Guid, IDictionary<Guid, AgentResult>> Searches = new ConcurrentDictionary<Guid, IDictionary<Guid, AgentResult>>();
        private IClientsDB clients;
        private IAgentsPendingDB agentsPending;

        private const int timeToAgentCleanupSec = 60;

        public SearchesDB(IClientsDB clients, IAgentsPendingDB agentsPending)
        {
            this.clients = clients;
            this.agentsPending = agentsPending;
        }

        public Guid CreateSearchRequest(Guid customerId, string query)
        {
            Guid searchId = Guid.NewGuid();
            Searches[searchId] = new ConcurrentDictionary<Guid, AgentResult>();
            List<Agent> agents = this.clients.GetActiveAgents(customerId);
            foreach (Agent agent in agents)
            {
                var agentResult = new AgentResult(agent.agentId, agent.deviceType, agent.deviceName + " - Error", null);
                Searches[searchId].Add(agent.agentId, agentResult);

                SearchQuery searchQuery = new SearchQuery(searchId, query, "search");
                this.agentsPending.SubmitNewQuery(agent, searchQuery);
            }

            return searchId;
        }

        public Guid CreateFileRequest(Guid customerId, Guid agentId, string filePath)
        {
            Guid requestId = Guid.NewGuid();
            Agent agent = this.clients.GetAgent(customerId, agentId);

            var agentResult = new AgentResult(agent.agentId, agent.deviceType, agent.deviceName + " - Error", null);

            Searches[requestId] = new ConcurrentDictionary<Guid, AgentResult>();
            Searches[requestId][agent.agentId] = agentResult;

            SearchQuery searchQuery = new SearchQuery(requestId, filePath, "file");
            this.agentsPending.SubmitNewQuery(agent, searchQuery);

            return requestId;
        }

        public void UpdateSearch(Guid searchId, Guid agentId, AgentResult result)
        {
            result.isSearchDone = true;
            this.Searches[searchId][agentId] = result;
        }

        public bool IsAwaitingResults(Guid customerId, Guid searchId)
        {
            // for Ziv: return !this.Searches[searchId].Any(s => !s.Value.isSearchDone);

            foreach (var searchValue in this.Searches[searchId])
            {
                if (searchValue.Value.isSearchDone == false)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsAwaitingFile(Guid customerId, Guid searchId, Guid agentId)
        {
            if (this.Searches[searchId].ContainsKey(agentId))
            {
                if (Searches[searchId][agentId].isSearchDone == false)
                {
                    return true;
                }
            }

            return false;
        }

        public SearchResults GetSearchResults(Guid customerId, Guid searchId, string type)
        {
            return new SearchResults(this.Searches[searchId], type);
        }

        public void DeleteSearch(Guid searchId)
        {
              this.Searches.Remove(searchId);
        }
    }
}
