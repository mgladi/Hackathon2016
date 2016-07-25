using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CrossSearch
{
    public class SearcherAgent
    {
        private readonly string details;
        private readonly HttpListenerContext context;
        private readonly string identifier;
        public SearcherAgent(string details, string identifier, HttpListenerContext context)
        {
            this.details = details;
            this.identifier = identifier;
            this.context = context;
        }
    }
}
