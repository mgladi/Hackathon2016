using ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello
{
    public class Client
    {
        private IService serviceMock = new ServiceMock.ServiceMock();
        private Guid AgentGuid = new Guid();
        private Guid UserGuid = new Guid();

        public List<ResultDataFromAgent> SearchFileInAllDeveices(string FileNameToSearch) // search acording to User Guid and fileNameToSearch
        {
            List<ResultDataFromAgent> result = serviceMock.SearchFileInAllDeveices(
                FileNameToSearch: new SearchItem
                {
                    PollingResultType = PollingResultType.SearchQuery,
                    ResultQuery = FileNameToSearch
                },
                AgentGuid: AgentGuid,
                UserGuid: UserGuid);

            return result;
        }

        public string GetFileContent(string FileNameToGet) // Search according to specific Agent Guid and it fileName path
        {
            ResultDataFromAgent result = serviceMock.SearchFileInAllDeveices(FileNameToSearch:
                new SearchItem { PollingResultType = PollingResultType.FileToTransferPath, ResultQuery = FileNameToGet },
                AgentGuid: AgentGuid,
                UserGuid: UserGuid)[0]; // will return from a specific single agent

            return result.FileContent;
        }
    }
}
