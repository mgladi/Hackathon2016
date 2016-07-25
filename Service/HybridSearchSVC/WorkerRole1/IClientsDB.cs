using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public interface IClientsDB
    {
        Agent CreateNewAgent(Guid clientId, string content);
        Agent CreateNewAgentByID(Guid clientId, Guid agentId, string content);
        List<Agent> GetAgents(Guid clientId);
    }
}
