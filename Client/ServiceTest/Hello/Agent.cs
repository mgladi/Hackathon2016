using ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hello
{
    public class Agent
    {
        private IService serviceMock = new ServiceInterface.HybridSearchService("http://10.93.172.22:81");
        private Guid AgentGuid = Guid.NewGuid();
        private Guid UserGuid = new Guid("5286515c-4ddf-41fe-a908-ace03a0318bb");
        private Guid RequestGuid;



        public SearchItem PollService()
        {
            SearchItem result = serviceMock.PollService(AgentGuid, UserGuid);

            if (result.PollingResultType == PollingResultType.SearchQuery)
            {
                RequestGuid = result.RequestId;
                //SearchFileInTheDevice(result.ResultQuery);
            }
            else if (result.PollingResultType == PollingResultType.FileToTransferPath)
            {
                RequestGuid = result.RequestId;
                //SendFileContent(result.ResultQuery);
            }
            return result;
        }

        private void SendFileContent(string FilePathToSend)
        {
            //Send file
            string dummyData = "ThisIsDummyContent of file: - from agent " + FilePathToSend;
            serviceMock.SendResult(UserGuid, AgentGuid, RequestGuid, new ResultDataFromAgent { ResultType = ResultDataFromAgentType.FileContent, FileContent = dummyData });
        }

        private void SearchFileInTheDevice(string FileNameToSearch)
        {
            //search and send the local results to server
            serviceMock.SendResult(UserGuid, AgentGuid, RequestGuid, agentResult: new ResultDataFromAgent
            {
                AgentGuid = AgentGuid,
                DeviceType = DeviceType.Windows,
                DeviceName = "My PC",
                ResultType = ResultDataFromAgentType.FilesMetadataList,
                FilesMetadata = new List<FileMetadata>
            {
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile.txt - from agent", Size = 22, Time = DateTime.Now},
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile2.txt - from agent", Size = 23, Time = DateTime.Now}
                }
            });
        }
    }
}
