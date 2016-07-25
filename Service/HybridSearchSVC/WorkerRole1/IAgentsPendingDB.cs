using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    interface IAgentsPendingDB
    {
        /// <summary>
        /// Get the search if exists and remove at the end.
        /// </summary>
        bool TryGetNextQuery(Guid agentId, out SearchQuery searchQuery);

        void SubmitNewQuery(Agent agent, SearchQuery searchQuery);
    }

    

   
}