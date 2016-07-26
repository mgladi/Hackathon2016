using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class AgentResult
    {
        public byte[] result { get; set; }
        public Guid agentId { get; set; }
        public string deviceType { get; set; }
        public string deviceName { get; set; }



        public AgentResult()
        {

        }
        public AgentResult(Guid agentId, string deviceType, string deviceName, byte[] result)
        {
            this.result = result;
            this.agentId = agentId;
            this.deviceType = deviceType;
            this.deviceName = deviceName;
        }
    }
}
