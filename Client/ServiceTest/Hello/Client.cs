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
        private Guid customerId = new Guid();

        public List<ResultDataFromAgent> SearchFileInAllDeveices(string FileNameToSearch) // search acording to User Guid and fileNameToSearch
        {
            List<ResultDataFromAgent> result = serviceMock.SearchFileInAllDevices(
                query: "Some file name",
                customerId: customerId);

            return result;
        }

        public string GetFileContent(string FileNameToGet) // Search according to specific Agent Guid and it fileName path
        {
            ResultDataFromAgent result = serviceMock.GetFileFromDevice(FileNameToGet, AgentGuid, customerId);

            return result.FileContent;
        }
    }
}
