using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterface
{
    public interface IService
    {
        SearchItem PollService(Guid agentId, string customerName);

        List<ResultDataFromAgent> SearchFileInAllDevices(string query, string customerName);

        ResultDataFromAgent GetFileFromDevice(string path, Guid agentId, string customerName);

        void SendResult(string customerName, Guid agentId, Guid requestId, ResultDataFromAgent agentResult);
    }
}
