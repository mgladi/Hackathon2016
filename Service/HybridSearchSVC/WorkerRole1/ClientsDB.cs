using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    class ClientsDB : IClientsDB
    {
        private IDictionary<Guid, List<Agent>> Clients = new ConcurrentDictionary<Guid, List<Agent>>();

        private void addAgentToList(Guid clientId, Agent agent)
        {
            List<Agent> agents;
            if (!this.Clients.TryGetValue(clientId, out agents))
            {
                this.Clients[clientId] = new List<Agent>();
            }
            agents.Add(agent);
        }
        public Agent CreateNewAgent(Guid clientId, string content)
        {
            Guid agentId = Guid.NewGuid();
            return CreateNewAgentByID(clientId, agentId, content);
        }

        public Agent CreateNewAgentByID(Guid clientId, Guid agentId, string content)
        {
            if (this.Clients[clientId] == null)
            {
                Agent newAgent = new Agent(agentId, content);
                addAgentToList(clientId, newAgent);
                return newAgent;
            }
            
            Agent agent = this.Clients[clientId].FirstOrDefault(a => a.getId() == agentId);
            if (agent == null)
            {
                agent = new Agent(agentId, content);
                addAgentToList(clientId, agent);
            }
            return agent;
        }


        public List<Agent> GetAgents(Guid clientId)
        {
            return this.Clients[clientId];
        }
    }
}
