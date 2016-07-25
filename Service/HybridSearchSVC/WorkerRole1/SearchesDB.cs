﻿using System;
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

        public SearchesDB(IClientsDB clients, IAgentsPendingDB agentsPending)
        {
            this.clients = clients;
            this.agentsPending = agentsPending;
        }

        public Guid CreateNewSearch(Guid customerId, string query)
        {
            Guid searchId = Guid.NewGuid();
            Searches[searchId] = new ConcurrentDictionary<Guid, AgentResult>();

            List<Agent> agents = this.clients.GetAgents(customerId);
            foreach (Agent agent in agents)
            {
                SearchQuery searchQuery = new SearchQuery(searchId, query, "search");
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
            List<Agent> agents = this.clients.GetAgents(customerId);
            foreach(Agent agent in agents)
            {
                if (!this.Searches[searchId].ContainsKey(agent.getId()));
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

        public SearchResults GetSearchResults(Guid customerId, Guid searchId, string type = "search")
        {
            return new SearchResults(this.Searches[searchId], type);
        }

        public void DeleteSearch(Guid searchId)
        {
              this.Searches.Remove(searchId);
        }
    }
}
