using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterface
{
    public interface IService
    {

        SearchItem PollService();

        List<ResultDataFromAgent> SearchFileInAllDevices(string query);

        ResultDataFromAgent GetFileFromDevice(string path, Guid agentId);

        void SendResult(Guid requestId, ResultDataFromAgent agentResult);

        void Register(string deviceName, string deviceType);
    }
}
