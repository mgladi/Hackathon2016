using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterface
{
    public interface IService
    {
        SearchItem PollService(Guid agentId, Guid customerId);

        List<ResultDataFromAgent> SearchFileInAllDevices(string query, Guid customerId);

        ResultDataFromAgent GetFileFromDevice(string path, Guid agentId, Guid customerId);

        void SendResult(Guid customerId, Guid agentId, Guid requestId, ResultDataFromAgent agentResult);
    }
}
