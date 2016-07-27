using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using ServiceInterface;
using ServiceMock;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrossDeviceSearch
{
    public partial class RegisterPage : ContentPage
    {
        string username;
        //string bookText;
        private readonly IService service;
        FileHelper fileHelper = new FileHelper();

        public RegisterPage()
        {
            InitializeComponent();
            StackLayout LogoStack = new StackLayout()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(0, 0, 0, 50)
            };
            LogoStack.Children.Add(new Image
            {
                Source = ImageSource.FromResource("CrossDeviceSearch.Images.Logo.jpg"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 200,
                HeightRequest = 200,

            });
            MainStack.Children.Insert(0, LogoStack);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            this.username = ((Entry)sender).Text;
        }

        async void OnContinueButtonPressed(object sender, EventArgs args)
        {
            //NavigationPage navPage = new NavigationPage();
            IService service = new HybridSearchService("http://hybridsearchsvc.cloudapp.net", this.username);
            Task task = new Task(() =>
            {
                service.Register(username, DeviceType.Windows.ToString());
                while (true)
                {
                    SearchItem searchItem = service.PollService();
                    Task.Run(() =>
                    {
                        switch (searchItem.PollingResultType)
                        {
                            case PollingResultType.SearchQuery:
                                service.SendResult(searchItem.RequestId, SearchQuery(searchItem.ResultQuery));
                                break;
                            case PollingResultType.FileToTransferPath:
                                service.SendResult(searchItem.RequestId, FileToTransferPath(searchItem.ResultQuery));
                                break;
                        }
                    });
                    Task.Delay(2000).Wait();
                }
            });
            task.Start();
            await Navigation.PushModalAsync(new CrossDeviceSearchPage(service, this.username));
        }

        private ResultDataFromAgent SearchQuery(string query)
        {
            FileHelper fileHelper = new FileHelper();
            ResultDataFromAgent results = new ResultDataFromAgent
            {
                DeviceType = fileHelper.DeviceType,
                DeviceName = fileHelper.DeviceModel,
                FilesMetadata = fileHelper.SearchFiles(query),
                ResultType = ResultDataFromAgentType.FilesMetadataList
            };
            return results;
        }
        private ResultDataFromAgent FileToTransferPath(string filepath)
        {
            FileHelper fileHelper = new FileHelper();
            return new ResultDataFromAgent
            {
                DeviceType = fileHelper.DeviceType,
                DeviceName = fileHelper.DeviceModel,
                FileContent = fileHelper.ReadFile(filepath),
                ResultType = ResultDataFromAgentType.FileContent
            };
        }
    }
}
