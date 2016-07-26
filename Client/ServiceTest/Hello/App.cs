using ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Hello
{
    public class App : Application
    {
        Client client = new Client();
        Agent agent = new Agent();
        SearchBar textField = new SearchBar();
        Button searchButton = new Button { Text = "Search!", BorderColor = Color.Blue };
        ListView resultsField = new ListView();
        Button Poll = new Button { Text = "Start polling!", BorderColor = Color.Blue };
        Label FileContent = new Label();
        Label ResponedTextLabel = new Label();

        public App()
        {
            searchButton.Clicked += OnSearchButtonClicked;
            Poll.Clicked += OnPollingButtonClicked;
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
                        Poll,
                        resultsField,
                        FileContent,
                        ResponedTextLabel
                    }
                }
            };
        }

        private void OnSearchButtonClicked(object sender, EventArgs e)
        {
            List<ResultDataFromAgent> results = client.SearchFileInAllDeveices(textField.Text);
            resultsField.ItemsSource = results;
            resultsField.ItemSelected += OnResultDataFromAgentItemSelected;
        }

        private void OnResultDataFromAgentItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            resultsField.ItemsSource = (e.SelectedItem as ResultDataFromAgent).FilesMetadata;
            resultsField.ItemSelected -= OnResultDataFromAgentItemSelected;
            resultsField.ItemSelected += OnFileMetadataItemSelected;
        }

        private void OnFileMetadataItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            resultsField.ItemSelected -= OnResultDataFromAgentItemSelected;
            string content = client.GetFileContent(((FileMetadata)(resultsField.SelectedItem)).FullPathAndName);
            FileContent.Text = content;
        }

        private void OnPollingButtonClicked(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    SearchItem responde = agent.PollService();
                    if (responde.PollingResultType != PollingResultType.NoRequest)
                    {
                        Task.Delay(2000).Wait();
                    }
                    Task.Delay(2000).Wait();
                }
            });
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
