using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using ServiceInterface;
using ServiceMock;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Agent
{
    public class App : Application
    {
        ServiceMock.ServiceMock mock = new ServiceMock.ServiceMock();
        Guid agentGuid = Guid.NewGuid();
        Guid userGuid = new Guid("c610a71d-91fd-4ef8-947f-8e4d4013106d");

        public App()
        {
            Task task = new Task(() =>
            {
                while (true)
                {
                    SearchItem searchItem = mock.PollService(agentGuid, userGuid);
                    switch (searchItem.PollingResultType)
                    {
                        case PollingResultType.SearchQuery:
                            mock.SendResult(userGuid, agentGuid, searchItem.RequestId, SearchQuery(searchItem.ResultQuery));
                            break;
                        case PollingResultType.FileToTransferPath:
                            //mock.SendResult(userGuid, agentGuid, searchItem.RequestId, FileToTransferPath(searchItem.ResultQuery));
                            break;
                    }
                    Task.Delay(1000).Wait();
                }
            });
            task.Start();
            MainPage = new AgentPage();
        }

        private ResultDataFromAgent SearchQuery(string query)
        {
            FileHelper fileHelper = new FileHelper();
            return new ResultDataFromAgent
            {
                AgentGuid = agentGuid,
                DeviceType = fileHelper.DeviceType,
                DeviceName = fileHelper.DeviceModel,
                FilesMetadata = fileHelper.SearchFiles(query),
                ResultType = ResultDataFromAgentType.FilesMetadataList
            };
        }
        private ResultDataFromAgent FileToTransferPath(string filepath)
        {
            FileHelper fileHelper = new FileHelper();
            return new ResultDataFromAgent
            {
                AgentGuid = agentGuid,
                DeviceType = fileHelper.DeviceType,
                DeviceName = fileHelper.DeviceModel,
                FileContent = fileHelper.ReadFile(filepath),
                ResultType = ResultDataFromAgentType.FileContent
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
