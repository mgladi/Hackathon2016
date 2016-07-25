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

        public AgentResult()
        {

        }
        public AgentResult(Guid agentId, byte[] result)
        {
            this.result = result;
            this.agentId = agentId;
        }
    }
}
