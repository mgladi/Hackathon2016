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
        private IService serviceMock = new ServiceInterface.HybridSearchService("http://10.93.172.22:81");
        private Guid AgentGuid = Guid.NewGuid();
        private Guid customerId = new Guid("5286515c-4ddf-41fe-a908-ace03a0318bb");

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
