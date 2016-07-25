using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    public class SearchResults
    {
        public List<AgentResult> results { get; set; }

        public string type { set; get; }

        public SearchResults()
        {
        }

        public SearchResults(IDictionary<Guid, AgentResult> resultsDict, string type)
        {
            this.results = new List<AgentResult>();
            foreach (KeyValuePair<Guid, AgentResult> item in resultsDict)
            {
                this.results.Add(item.Value);
            }
            this.type = type;
        }
    }
}
