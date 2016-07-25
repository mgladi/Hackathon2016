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

        //According to this field 1 of the following 2 fields will be relevant:
        public ResultDataFromAgentType ResultType { get; set; }

        //1.
        public string FileContent { get;  set; }

        //2.
        public List<FileMetadata> FilesMetadata { get; set; }

        public override string ToString()
        {
            return "Device Name: " + DeviceName + "\nDevice Type: " + DeviceType;
        }
    }
}
