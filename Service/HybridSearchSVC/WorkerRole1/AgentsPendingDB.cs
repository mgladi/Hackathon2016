﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridSearch
{
    class AgentsPendingDB : IAgentsPendingDB
    {
        private IDictionary<Guid, ConcurrentQueue<SearchQuery>> agentsPending = new ConcurrentDictionary<Guid, ConcurrentQueue<SearchQuery>>();
        public void SubmitNewQuery(Agent agent, SearchQuery searchQuery)
        {
            Guid agentId = agent.getId();
            ConcurrentQueue<SearchQuery> queue = this.agentsPending[agentId];
            if (queue == null)
            {
                queue = new ConcurrentQueue<SearchQuery>();
            }
            queue.Enqueue(searchQuery);
        }

        public bool TryGetNextQuery(Guid agentId, out SearchQuery searchQuery)
        {
            ConcurrentQueue<SearchQuery> queue = this.agentsPending[agentId];
            if (queue == null)
            {
                searchQuery = null;
                return false;
            }
            return queue.TryDequeue(out searchQuery);
        }
    }
}