using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterface
{
    public enum PollingResultType
    {
        NoRequest,
        SearchQuery,
        FileToTransferPath
    }

    public class SearchItem
    {
        public PollingResultType PollingResultType { get; set; }

        public string ResultQuery { get; set; }
    }
}
