using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    class SearchesDB2 : ISearchesDB
    {
        public bool AwaitingResults(Guid searchId)
        {
            throw new NotImplementedException();
        }

        public void DeleteSearch(Guid searchId)
        {
            throw new NotImplementedException();
        }

        public SearchResults GetSearchResults(Guid searchId)
        {
            throw new NotImplementedException();
        }

        public Guid NewSearch()
        {
            throw new NotImplementedException();
        }

        public void UpdateSearch(Guid searchId, Guid agentId, byte[] result)
        {
            throw new NotImplementedException();
        }
    }
}
