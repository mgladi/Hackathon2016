using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    class AgentsPendingDB2 : IAgentsPendingDB
    {
        public bool TryGetNextQuery(Guid agentId, out SearchQuery searchQuery)
        {
            throw new NotImplementedException();
        }
    }
}
