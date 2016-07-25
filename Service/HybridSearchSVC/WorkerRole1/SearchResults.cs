using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class SearchResults
    {
        private List<AgentResult> results;

        public SearchResults(IDictionary<Guid, AgentResult> resultsDict)
        {
            this.results = new List<AgentResult>();
            foreach (KeyValuePair<Guid, AgentResult> item in resultsDict)
            {
                this.results.Add(item.Value);
            }
        }
    }
}
