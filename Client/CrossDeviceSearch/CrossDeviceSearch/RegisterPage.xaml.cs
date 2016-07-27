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

        public RegisterPage(IService service)
        {
			this.service = service;
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
                WidthRequest = 100,
                HeightRequest = 100,

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
            await Navigation.PushModalAsync(new CrossDeviceSearchPage(service, this.username));            
        }

    }
}
