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
        private IService serviceMock = new ServiceMock.ServiceMock();
        private Guid AgentGuid = new Guid();
        private Guid UserGuid = new Guid();
        private Guid RequestGuid;



        public void PollService()
        {
            SearchItem result = serviceMock.PollService(AgentGuid, UserGuid);

            if (result.PollingResultType == PollingResultType.SearchQuery)
            {
                SearchFileInTheDevice(result.ResultQuery);
                RequestGuid = result.RequestId;
            }
            else if (result.PollingResultType == PollingResultType.FileToTransferPath)
            {
                SendFileContent(result.ResultQuery);
                RequestGuid = result.RequestId;
            }
        }

        private void SendFileContent(string FilePathToSend)
        {
            //Send file
            string dummyData = "ThisIsDummyContent of file: " + FilePathToSend;
            serviceMock.SendResult(UserGuid, AgentGuid, RequestGuid, new ResultDataFromAgent { ResultType = ResultDataFromAgentType.FileContent, FileContent = dummyData });
        }

        private void SearchFileInTheDevice(string FileNameToSearch)
        {
            //search and send the local results to server
            serviceMock.SendResult(UserGuid, AgentGuid, RequestGuid, agentResult: new ResultDataFromAgent
            {
                AgentGuid = new Guid(),
                DeviceType = DeviceType.Windows,
                DeviceName = "My PC",
                ResultType = ResultDataFromAgentType.FilesMetadataList,
                FilesMetadata = new List<FileMetadata>
            {
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile.txt", Size = 22, Time = DateTime.Now},
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile2.txt", Size = 23, Time = DateTime.Now}
                }
            });
        }
    }
}
