using ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Hello
{
    public class App : Application
    {
        Client client = new Client();
        SearchBar textField = new SearchBar();
        Button searchButton = new Button { Text = "Search!", BorderColor = Color.Blue };
        ListView resultsField = new ListView();
        Button GetContentOfFirstFileButton = new Button { Text = "Get content of first file!", BorderColor = Color.Blue };
        public App()
        {
            searchButton.Clicked += OnSearchButtonClicked;

            // The root page of your application
            MainPage = new ContentPage
            {
                
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Insert file to search:"
                        },
                        textField,
                        searchButton,
                        GetContentOfFirstFileButton,
                        resultsField
                    }
                }
            };
        }

        private void OnSearchButtonClicked(object sender, EventArgs e)
        {
            List<ResultDataFromAgent> results = client.SearchFileInAllDeveices(textField.Text);
            resultsField.ItemsSource = results;
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
