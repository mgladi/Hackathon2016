﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public interface ISearchesDB
    {
        Guid CreateNewSearch(Guid customerId, string query);
        void UpdateSearch(Guid searchId, Guid agentId, AgentResult result);
        bool IsAwaitingResults(Guid customerId, Guid searchId);
        bool IsAwaitingFile(Guid customerId, Guid searchId, Guid agentId);
        SearchResults GetSearchResults(Guid customerId, Guid searchId, string type = "search");
        void DeleteSearch(Guid searchId);
    }
}
