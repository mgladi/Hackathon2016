using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public interface ISearchesDB
    {
        Guid CreateSearchRequest(Guid customerId, string query);
        Guid CreateFileRequest(Guid customerId, Guid agentId, string filePath);
        void UpdateSearch(Guid searchId, Guid agentId, AgentResult result);
        bool IsAwaitingResults(Guid customerId, Guid searchId);
        bool IsAwaitingFile(Guid customerId, Guid searchId, Guid agentId);
        SearchResults GetSearchResults(Guid customerId, Guid searchId, string type);
        void DeleteSearch(Guid searchId);
    }
}
