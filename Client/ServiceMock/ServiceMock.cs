using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceInterface;

namespace ServiceMock
{
    public class ServiceMock : IService
    {
        public SearchItem PollService(Guid AgentGuid, Guid UserGuid)
        {
            Random r = new Random();
            int rInt = r.Next(0, 5);

            SearchItem result = new SearchItem();
            if (rInt == 0)
            {
                result.PollingResultType = PollingResultType.FileToTransferPath;
                result.ResultQuery = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile.txt";
            }
            else if (rInt == 1)
            {
                result.PollingResultType = PollingResultType.SearchQuery;
                result.ResultQuery = "SomeTextFile";
            }
            else
            {
                result.PollingResultType = PollingResultType.NoRequest;
            }

            return result;
        }

        public void SendResult(Guid AgentGUID, ResultDataFromAgent FilesMetadataList)
        {
            //combine all data. return jsut 200 OK to each machine
        }

        public ResultDataFromAgent SearchFileInAllDeveices(SearchItem FileNameToSearch, Guid AgentGuid, Guid UserGuid)
        {
            return new ResultDataFromAgent
            {
                ResultType = ResultDataFromAgentType.FilesMetadataList,
                FilesMetadata = new List<FileMetadata>
            {
                new FileMetadata { AgentGuid = new Guid(), FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile.txt", Size = 22, Time = DateTime.Now, DeviceType = DeviceType.Windows },
                new FileMetadata { AgentGuid = new Guid(), FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile2.txt", Size = 23, Time = DateTime.Now, DeviceType = DeviceType.Android }
                }
            };
        }
    }
}
