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

        ResultDataFromAgent GetFileFromDevice(string path);

        void SendResult(Guid requestId, ResultDataFromAgent agentResult);
    }
}
