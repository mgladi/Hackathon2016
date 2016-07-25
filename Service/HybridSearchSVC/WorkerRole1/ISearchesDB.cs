using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    interface ISearchesDB
    {
        Guid CreateNewSearch(Guid clientId, SearchQuery searchQuery);
        void UpdateSearch(Guid searchId, Guid agentId, AgentResult result);
        bool IsAwaitingResults(Guid clientId, Guid searchId);
        SearchResults GetSearchResults(Guid clientId, Guid searchId); // <AgentId, result> dictionary
        void DeleteSearch(Guid searchId);
    }
}
