using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    interface ISearchesDB
    {
        Guid CreateNewSearch(Guid customerId, string query);
        void UpdateSearch(Guid searchId, Guid agentId, AgentResult result);
        bool IsAwaitingResults(Guid customerId, Guid searchId);
        SearchResults GetSearchResults(Guid customerId, Guid searchId); // <AgentId, result> dictionary
        void DeleteSearch(Guid searchId);
    }
}
