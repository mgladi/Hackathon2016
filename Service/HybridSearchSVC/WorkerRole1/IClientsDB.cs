using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public interface IClientsDB
    {
        Agent CreateNewAgent(Guid customerId, string content);
        Agent CreateNewAgentByID(Guid customerId, Guid agentId, string content);
        List<Agent> GetAgents(Guid customerId, Func<Agent,bool> shouldInclude = null);
        void updateAgentLastSeen(Guid customerId, Guid agentId);
    }
}
