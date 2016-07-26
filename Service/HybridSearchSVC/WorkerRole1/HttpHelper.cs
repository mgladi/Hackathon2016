using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public static class HttpHelper
    {
        public static void SendString(HttpListenerResponse response, string content)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();

            response.StatusCode = (int)HttpStatusCode.OK;
            response.Close();
        }

        public static void SendObject(HttpListenerResponse response, object obj)
        {
            SendObjectWithStatus(response, obj, HttpStatusCode.OK);
        }

        public static void SendObjectWithError(HttpListenerResponse response, object obj)
        {
            SendObjectWithStatus(response, obj, HttpStatusCode.InternalServerError);
        }

        private static void SendObjectWithStatus(HttpListenerResponse response, object obj, HttpStatusCode statusCode)
        {
            var serializedObj = JsonConvert.SerializeObject(obj);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(serializedObj.ToString());
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();

            response.StatusCode = (int)statusCode;
            response.Close();
        }

        public static void SendEmpty(HttpListenerResponse response)
        {
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Close();
        }
        public static string GetRequestPostData(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                return null;
            }
            using (System.IO.Stream body = request.InputStream) // here we have data
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(body, request.ContentEncoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
