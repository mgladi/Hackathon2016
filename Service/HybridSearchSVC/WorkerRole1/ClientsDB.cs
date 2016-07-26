using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class ClientsDB : IClientsDB
    {
        private IDictionary<Guid, List<Agent>> Clients = new ConcurrentDictionary<Guid, List<Agent>>();

        private void addAgentToList(Guid clientId, Agent agent)
        {
            if (!this.Clients.ContainsKey(clientId))
            {
                this.Clients[clientId] = new List<Agent>();
            }
            this.Clients[clientId].Add(agent);
        }
        public Agent CreateNewAgent(Guid clientId, string content)
        {
            Guid agentId = Guid.NewGuid();
            return CreateNewAgentByID(clientId, agentId, content);
        }

        public Agent CreateNewAgentByID(Guid clientId, Guid agentId, string content)
        {
            if (!this.Clients.ContainsKey(clientId))
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
            List<Agent> list = new List<Agent>();
            if (this.Clients.ContainsKey(clientId))
            {
                return this.Clients[clientId];
            }
            return list;
        }
    }
}
