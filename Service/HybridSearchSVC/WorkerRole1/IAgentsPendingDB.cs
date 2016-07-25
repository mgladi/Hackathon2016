using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1
{
    interface IAgentsPendingDB
    {
        /// <summary>
        /// Get the search if exists and remove at the end.
        /// </summary>
        bool TryGetNextQuery(Guid agentId, out SearchQuery searchQuery);
    }

    

   
}