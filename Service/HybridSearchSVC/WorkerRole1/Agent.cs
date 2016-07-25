using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1
{
    class Agent
    {
        private Guid agentId;

        Agent(Guid agentId, string content)
        {
            this.agentId = agentId;
        }
    }
}
