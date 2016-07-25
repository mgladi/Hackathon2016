using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    interface ISearchesDB
    {
        Guid NewSearch();
        void UpdateSearch(Guid searchId, Guid agentId, byte[] result);
        bool AwaitingResults(Guid searchId);
        SearchResults GetSearchResults(Guid searchId); // <AgentId, result> dictionary
        void DeleteSearch(Guid searchId);
    }
}
