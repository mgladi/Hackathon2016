using HybridSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using WorkerRole1;

namespace HybridSearch
{
    public class AgentsStatusRequest : IRequest
    {
        private readonly Guid customerId;
        private readonly IClientsDB clientsDb;
        public AgentsStatusRequest(IClientsDB clientsDb, Guid customerId)
        {
            this.customerId = customerId;
            this.clientsDb = clientsDb;
        }

        public Task ProcessRequest(HttpListenerContext context)
        {
            List<Agent> agents = this.clientsDb.GetActiveAgents(customerId);
            List<AgentData> agentsData = agents.Select(a => new AgentData()
            {
                DeviceName = a.deviceName,
                DeviceType = (DeviceType)Enum.Parse(typeof(DeviceType), a.deviceType)
            }).ToList();

            HttpHelper.SendObject(context.Response, agentsData);

            return Task.FromResult(0);
        }
    }
}
