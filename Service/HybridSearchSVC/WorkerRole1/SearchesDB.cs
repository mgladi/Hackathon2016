﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1
{
    class SearchesDB : ISearchesDB
    {
        private IDictionary<Guid, IDictionary<Guid, AgentResult>> Searches = new ConcurrentDictionary<Guid, IDictionary<Guid, AgentResult>>();
        private IClientsDB clients;
        private IAgentsPendingDB agentsPending;

        SearchesDB(IClientsDB clients, IAgentsPendingDB agentsPending)
        {
            this.clients = clients;
            this.agentsPending = agentsPending;
        }

        public Guid CreateNewSearch(Guid clientId, SearchQuery searchQuery)
        {
            Guid searchId = Guid.NewGuid();
            Searches[searchId] = new ConcurrentDictionary<Guid, AgentResult>();

            List<Agent> agents = this.clients.GetAgents(clientId);
            foreach (Agent agent in agents)
            {
                this.agentsPending.CreateNewAgent(agent, searchQuery);
            }

            return searchId;
        }

        public void UpdateSearch(Guid searchId, Guid agentId, AgentResult result)
        {
            this.Searches[searchId][agentId] = result;
        }

        public bool IsAwaitingResults(Guid clientId, Guid searchId)
        {
            List<Agent> agents = this.clients.GetAgents(clientId);
            foreach(Agent agent in agents)
            {
                if (!this.Searches[searchId].ContainsKey(agent.getId()));
                {
                    return true;
                }
            }
            return false;
        }

        public SearchResults GetSearchResults(Guid clientId, Guid searchId)
        {
            List<Agent> agents = this.clients.GetAgents(clientId);
            SearchResults results = new SearchResults(this.Searches[searchId]);
            return results;
        }

        public void DeleteSearch(Guid searchId)
        {
            this.Searches.Remove(searchId);
        }
    }
}
