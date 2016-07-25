using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    class HybridListenerProcessor
    {
        public void ProcessRequest(HttpListenerContext context)
        {
            IRequest request = RequestCreator.CreateRequest(context.Request);
            request.ProcessRequest(context);
        }
    }
}
