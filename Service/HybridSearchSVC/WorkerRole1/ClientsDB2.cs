using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    class ClientsDB2 : IClientsDB
    {
        public List<Agent> GetAgents(Guid clientId)
        {
            throw new NotImplementedException();
        }

        public Agent NewAgent(Guid clientId, string content)
        {
            throw new NotImplementedException();
        }
    }
}
