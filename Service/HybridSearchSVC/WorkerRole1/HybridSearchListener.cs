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
        string endpoint = "127.0.0.1:55646";
        HybridListenerProcessor processor = new HybridListenerProcessor();

        public void StartListen()
        {
            var ipendpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["test"].IPEndpoint;
            string prefix = string.Format("http://{0}/", ipendpoint.ToString());
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

                    Task.Factory.StartNew(() =>
                    {
                        IRequest request = RequestCreator.CreateRequest(ctx.Request);
                        request.ProcessRequest(ctx);
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
