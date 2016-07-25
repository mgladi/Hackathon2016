using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterface
{
    public interface IService
    {
        SearchItem PollService(Guid AgentGuid, Guid UserGuid);

        void SendResult(Guid AgentGUID, ResultDataFromAgent agentResult);

        List<ResultDataFromAgent> SearchFileInAllDeveices(SearchItem FileNameToSearch, Guid AgentGuid, Guid UserGuid);
    }
}
