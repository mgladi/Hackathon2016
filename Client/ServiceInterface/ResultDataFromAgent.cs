using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterface
{
    public enum ResultDataFromAgentType
    {
        FileContent,
        FilesMetadataList
    }
    public class ResultDataFromAgent
    {
        public ResultDataFromAgentType ResultType { get; set; }

        public string FileContent { get;  set; }

        public List<FileMetadata> FilesMetadata { get; set; }
    }
}
