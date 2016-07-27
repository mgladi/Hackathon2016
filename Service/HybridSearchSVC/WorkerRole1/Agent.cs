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
        public string deviceName;
        public string deviceType;
        public DateTime lastSeen;

        public Agent(Guid agentId, string deviceName, string deviceType)
        {
            this.agentId = agentId;
            this.deviceName = deviceName;
            this.deviceType = deviceType;
            this.lastSeen = DateTime.Now;
        }

        public Guid getId()
        {
            return this.agentId;
        }
    }
}
