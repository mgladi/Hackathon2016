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
        private const int timeToAgentCleanup = 10;
        private void addAgentToList(Guid customerId, Agent agent)
        {
            if (!this.Clients.ContainsKey(customerId))
            {
                this.Clients[customerId] = new List<Agent>();
            }
            this.Clients[customerId].Add(agent);
        }
        public Agent CreateNewAgent(Guid customerId, string deviceName, string deviceType)
        {
            Guid agentId = Guid.NewGuid();
            return CreateNewAgentByID(customerId, agentId, deviceName, deviceType);
        }

        public Agent CreateNewAgentByID(Guid customerId, Guid agentId, string deviceName, string deviceType)
        {
            if (!this.Clients.ContainsKey(customerId))
            {
                Agent newAgent = new Agent(agentId, deviceName, deviceType);
                addAgentToList(customerId, newAgent);
                return newAgent;
            }

            Agent agent = this.Clients[customerId].FirstOrDefault(a => a.getId() == agentId);
            if (agent == null)
            {
                agent = new Agent(agentId, deviceName, deviceType);
                addAgentToList(customerId, agent);
            }
            return agent;
        }


        public List<Agent> GetAgents(Guid customerId, Func<Agent, bool> shouldInclude = null)
        {
            if (shouldInclude == null)
            {
                shouldInclude = (agent) => true;
            }
            List<Agent> list = new List<Agent>();
            if (this.Clients.ContainsKey(customerId))
            {
                return this.Clients[customerId].Where(shouldInclude).ToList();
            }
            return list;
        }

        public void updateAgentLastSeen(Guid customerId, Guid agentId)
        {
            if (this.Clients.ContainsKey(customerId))
            {
                foreach (Agent agent in this.Clients[customerId])
                {
                    if (agent.getId() == agentId)
                    {
                        agent.lastSeen = DateTime.Now;
                        return;
                    }
                }
            }
        }

        public List<Agent> GetActiveAgents(Guid customerId)
        {
            return GetAgents(customerId, (agent) => (DateTime.Now - agent.lastSeen).TotalSeconds < timeToAgentCleanup);
        }
    }
}
