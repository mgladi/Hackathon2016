﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class ErrorRequest : IRequest
    {
        public void ProcessRequest(HttpListenerContext context)
        {
            Console.WriteLine("Got error request");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.Close();
        }
    }
}
