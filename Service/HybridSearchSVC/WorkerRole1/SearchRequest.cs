using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class SearchRequest : IRequest
    {
        private readonly Guid customerId;
        private readonly string content;
        public SearchRequest(Guid customerId, string content)
        {
            this.customerId = customerId;
            this.content = content;
        }

        public void ProcessRequest(HttpListenerContext context)
        {
            Console.WriteLine("Got request of type SearchRequest");
            HttpHelper.SendString(context.Response, "string");
        }
    }
}
