using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class HybridSearchListener
    {
        HttpListener listener = new HttpListener();

        public void StartListen()
        {
            var ipendpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["test"].IPEndpoint;
            //string localHost = "*:81";
            string prefix = string.Format("http://{0}/", ipendpoint);
            listener.Prefixes.Add(prefix);
            listener.Start();
        }

        public async Task GetRequests()
        {
            while (true)
            {
                try
                {
                    HttpListenerContext ctx = await listener.GetContextAsync();

                    Task.Factory.StartNew(async () =>
                    {
                        IRequest request = RequestCreator.CreateRequest(ctx.Request);
                        await request.ProcessRequest(ctx);
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }
    }
}
