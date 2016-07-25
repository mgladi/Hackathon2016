using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1
{
    interface IClientsDB
    {
        Agent NewAgent(Guid clientId, string content);
        List<Agent> GetAgents(Guid clientId);
    }
}
