﻿using System;
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

        public List<ResultDataFromAgent> SearchFileInAllDeveices(SearchItem FileNameToSearch, Guid AgentGuid, Guid UserGuid)
        {
            return new List<ResultDataFromAgent>
            {
                new ResultDataFromAgent
                {
                DeviceName = "My PC",
                DeviceType = DeviceType.Windows,
                AgentGuid = new Guid(),
                ResultType = ResultDataFromAgentType.FilesMetadataList,
                FilesMetadata = new List<FileMetadata>
            {
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile11.txt", Size = 22, Time = DateTime.Now, },
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile12.txt", Size = 23, Time = DateTime.Now }
                }
                },
                new ResultDataFromAgent
                {
                DeviceName = "My PC2",
                DeviceType = DeviceType.Android,
                AgentGuid = new Guid(),
                ResultType = ResultDataFromAgentType.FilesMetadataList,
                FilesMetadata = new List<FileMetadata>
            {
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile21.txt", Size = 22, Time = DateTime.Now, },
                new FileMetadata { FullPathAndName = @"C:\Users\yaland\Desktop\HelloXamarin\SomeTextFile22.txt", Size = 23, Time = DateTime.Now }
                }
                }
            };
        }
    }
}