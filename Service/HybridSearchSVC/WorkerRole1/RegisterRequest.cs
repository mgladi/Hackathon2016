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
        private string content;

        public RegisterRequest(IClientsDB clientsDb, Guid customerId, string content)
        {
            this.clientsDb = clientsDb;
            this.customerId = customerId;
            this.content = content;
        }
        public async Task ProcessRequest(HttpListenerContext context)
        {
            Agent agent = this.clientsDb.CreateNewAgent(this.customerId, this.content);
            HttpHelper.SendObject(context.Response, agent.getId());
        }
    }
}
