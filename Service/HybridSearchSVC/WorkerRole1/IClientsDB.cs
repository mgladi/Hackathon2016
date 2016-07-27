using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public interface IClientsDB
    {
        Agent CreateNewAgent(Guid customerId, string deviceName, string deviceType);
        Agent CreateNewAgentByID(Guid customerId, Guid agentId, string deviceName, string deviceType);
        List<Agent> GetAgents(Guid customerId, Func<Agent,bool> shouldInclude = null);
        void updateAgentLastSeen(Guid customerId, Guid agentId);
        List<Agent> GetActiveAgents(Guid customerId);
        Agent GetAgent(Guid customerId, Guid agentId);

    }
}
