using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using ServiceInterface;
using ServiceMock;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CrossDeviceSearch
{
    public class App : Application
    {
        //ServiceMock.ServiceMock service = new ServiceMock.ServiceMock();
        Guid agentGuid = Guid.NewGuid();
        //IService service = new HybridSearchService("http://hybridsearchsvc.cloudapp.net", "G");

        public App()
        {
            MainPage = new RegisterPage();
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
