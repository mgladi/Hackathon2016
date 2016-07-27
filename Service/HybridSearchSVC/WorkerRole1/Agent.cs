using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class Agent
    {
        public Guid agentId;
        public string name;
        public DateTime lastSeen;

        public Agent(Guid agentId, string content)
        {
            this.agentId = agentId;
            this.lastSeen = DateTime.Now;
        }

        public Guid getId()
        {
            return this.agentId;
        }
    }
}
