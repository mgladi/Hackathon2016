﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public interface IRequest
    {
        Task ProcessRequest(HttpListenerContext context);
    }
}
