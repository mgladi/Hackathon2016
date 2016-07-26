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

    public class SearchResults
    {
        public List<AgentResult> results { get; set; }

        public SearchResults()
        {
            this.results = new List<AgentResult>();
        }

        public SearchResults(IDictionary<Guid, AgentResult> resultsDict)
        {
            this.results = new List<AgentResult>();
            foreach (KeyValuePair<Guid, AgentResult> item in resultsDict)
            {
                this.results.Add(item.Value);
            }
        }
    }

    public class AgentResult
    {
        public byte[] result { get; set; }
        public Guid agentId { get; set; }
        public string deviceType { get; set; }
        public string deviceName { get; set; }

        public AgentResult()
        {

        }
        public AgentResult(Guid agentId, string deviceType, string deviceName, byte[] result)
        {
            this.result = result;
            this.agentId = agentId;
            this.deviceType = deviceType;
            this.deviceName = deviceName;
        }
    }

}
