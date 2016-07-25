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

    public enum DeviceType
    {
        Windows,
        Android
    }

    public class ResultDataFromAgent
    {
        public Guid AgentGuid { get; set; }

        public DeviceType DeviceType { get; set; }

        public string DeviceName { get; set; }

        public ResultDataFromAgentType ResultType { get; set; }

        public string FileContent { get;  set; }

        public List<FileMetadata> FilesMetadata { get; set; }
    }
}
