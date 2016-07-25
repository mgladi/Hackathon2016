using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class Agent
    {
        private Guid agentId;

        public Agent(Guid agentId, string content)
        {
            this.agentId = agentId;
        }

        public Guid getId()
        {
            return this.agentId;
        }
    }
}
