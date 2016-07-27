using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    class RegisterRequest : IRequest
    {
        private IClientsDB clientsDb;
        private Guid customerId;
        private string deviceName;
        private string deviceType;

        public RegisterRequest(IClientsDB clientsDb, Guid customerId, string deviceName, string deviceType)
        {
            this.clientsDb = clientsDb;
            this.customerId = customerId;
            this.deviceName = deviceName;
            this.deviceType = deviceType;
        }
        public async Task ProcessRequest(HttpListenerContext context)
        {
            Agent agent = this.clientsDb.CreateNewAgent(this.customerId, this.deviceName, this.deviceType);
            HttpHelper.SendObject(context.Response, agent.getId());
        }
    }
}
