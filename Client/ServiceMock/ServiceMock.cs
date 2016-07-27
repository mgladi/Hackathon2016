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

        public SearchItem PollService()
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
                result.ResultQuery = "*.txt";
            }
            else
            {
                result.PollingResultType = PollingResultType.NoRequest;
            }

            return result;
        }

        public void SendResult(Guid requestId, ResultDataFromAgent agentResult)
        {
            //combine all data. return jsut 200 OK to each machine
        }

        public List<ResultDataFromAgent> SearchFileInAllDevices(string query)
        {
            Task.Delay(5000).Wait();
            return new List<ResultDataFromAgent>
            {
                new ResultDataFromAgent
                {
                DeviceName = "My PC",
                DeviceType = DeviceType.Windows,
                AgentGuid = Guid.NewGuid(),
                ResultType = ResultDataFromAgentType.FilesMetadataList,
                FilesMetadata = new List<FileMetadata>
            {
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrin\SomeTextFile11.txt", Size = 22, Time = DateTime.Now, },
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile12.txt", Size = 23, Time = DateTime.Now }
                }
                },
                new ResultDataFromAgent
                {
                DeviceName = "My PC2",
                DeviceType = DeviceType.Android,
                AgentGuid = Guid.NewGuid(),

                ResultType = ResultDataFromAgentType.FilesMetadataList,
                FilesMetadata = new List<FileMetadata>
            {
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile21.txt", Size = 22, Time = DateTime.Now, },
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile22.txt", Size = 23, Time = DateTime.Now },
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile23.txt", Size = 23, Time = DateTime.Now },
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile24.txt", Size = 23, Time = DateTime.Now },
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile25.txt", Size = 23, Time = DateTime.Now },
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile26.txt", Size = 23, Time = DateTime.Now },
                }
                }
            };
        }

        public ResultDataFromAgent GetFileFromDevice(string path, Guid agentId)
        {
            return new ResultDataFromAgent { FileContent = "A file content!" };
        }

        public void Register(string deviceName, string deviceType)
        {

	}

        public List<AgentData> GetAgentsStatus()
        {
            return new List<AgentData>();
        }
    }
}
