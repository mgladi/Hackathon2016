using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class SearchesDB : ISearchesDB
    {
        private IDictionary<Guid, IDictionary<Guid, AgentResult>> Searches = new ConcurrentDictionary<Guid, IDictionary<Guid, AgentResult>>();
        private IClientsDB clients;
        private IAgentsPendingDB agentsPending;

        private const int timeToAgentCleanup = 10;

        public SearchesDB(IClientsDB clients, IAgentsPendingDB agentsPending)
        {
            this.clients = clients;
            this.agentsPending = agentsPending;
        }

        public Guid CreateNewSearch(Guid customerId, string query, string type)
        {
            Guid searchId = Guid.NewGuid();
            Searches[searchId] = new ConcurrentDictionary<Guid, AgentResult>();       
            List<Agent> agents = GetActiveAgents(customerId);
            foreach (Agent agent in agents)
            {
                var agentResult = new AgentResult(agent.agentId, agent.deviceType, agent.deviceName + " - Error", null);
                Searches[searchId].Add(agent.agentId, agentResult);

                SearchQuery searchQuery = new SearchQuery(searchId, query, type);
                this.agentsPending.SubmitNewQuery(agent, searchQuery);
            }

            return searchId;
        }

        public void UpdateSearch(Guid searchId, Guid agentId, AgentResult result)
        {
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
            return !this.Searches[searchId].ContainsKey(agentId);
        }

        public SearchResults GetSearchResults(Guid customerId, Guid searchId, string type)
        {
            return new SearchResults(this.Searches[searchId], type);
        }

        public void DeleteSearch(Guid searchId)
        {
              this.Searches.Remove(searchId);
        }

        private List<Agent> GetActiveAgents(Guid customerId)
        {
            return this.clients.GetAgents(customerId, (agent) => (DateTime.Now - agent.lastSeen).TotalSeconds < timeToAgentCleanup);
        }
    }
}
