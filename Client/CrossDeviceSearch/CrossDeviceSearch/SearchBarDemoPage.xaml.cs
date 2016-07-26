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
    public partial class CrossDeviceSearchPage : ContentPage
    {
        const double MaxMatches = 100;
        //string bookText;
        IService service = new HybridSearchService("http://hybridsearchsvc.cloudapp.net");
        FileHelper fileHelper = new FileHelper();
        Guid userGuid = new Guid("d8d41657-c8e4-41dc-9b03-f5ba678e48e8");

        public CrossDeviceSearchPage()
        {
            InitializeComponent();            
        }

        void OnSearchBarTextChanged(object sender, TextChangedEventArgs args)
        {
            resultsStack.Children.Clear();
        }

        async void OnSearchBarButtonPressed(object sender, EventArgs args)
        {
            // Detach resultsStack from layout.
            resultsScroll.Content = null;

            resultsStack.Children.Clear();

            resultsStack.Children.Add(new ActivityIndicator()
            {
                //Color = Color.Red,
                //BackgroundColor = Color.Yellow,
                // VerticalOptions = LayoutOptions.FillAndExpand
            });
            resultsScroll.Content = resultsStack;
            var results = await Task.Run(() =>
            {
                return service.SearchFileInAllDevices(searchBar.Text, userGuid);
            });
            
            resultsStack.Children.Clear();

            if (results.Count == 0)
            {
                PresentNoResultsFound();
            }
            else
            {
                SetResults(results);

            }

            // Reattach resultsStack to layout.

        }

        private void PresentNoResultsFound()
        {
            resultsStack.Children.Add(new Label()
            {
                BackgroundColor = Color.White,
                Text = "No results were found.",
                TextColor = Color.Gray
            });
        }

        private void SetResults(List<ResultDataFromAgent> results)
        {
            foreach (ResultDataFromAgent result in results)
            {
                resultsStack.Children.Add(CreateDeviceStack(result));
            }
        }

        private StackLayout CreateDeviceStack(ResultDataFromAgent resultFromDevice)
        {
            StackLayout deviceStack = new StackLayout();

            StackLayout deviceTitle = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.White
            };

            Label arrowLabel = new Label
            {
                Text = "^",
                TextColor = Color.FromRgb(30, 144, 255),
                BackgroundColor = Color.White
            };

            deviceTitle.Children.Add(arrowLabel);

            Label nameLabel = new Label
            {
                Text = resultFromDevice.DeviceName,
                TextColor = Color.FromRgb(30, 144, 255),
                BackgroundColor = Color.White
            };

            deviceTitle.Children.Add(nameLabel);

            StackLayout deviceResultsStack = CreateDeviceResultsStack(resultFromDevice);

            deviceStack.Children.Add(deviceTitle);

            deviceStack.Children.Add(deviceResultsStack);

            deviceStack.Children.Add(new BoxView
            {
                HeightRequest = 0.5,
                BackgroundColor = Color.FromRgb(30, 144, 255)
            });

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                View view = (View)s;
                StackLayout stackLayout = (StackLayout)view.Parent;
                stackLayout.Children[1].IsVisible = !stackLayout.Children[1].IsVisible;
                StackLayout titleStackLayout = (StackLayout)stackLayout.Children[0];
                Label label = (Label)titleStackLayout.Children[0];
                if (stackLayout.Children[1].IsVisible)
                {                    
                    label.Text = ">";
                }
                else
                {
                    label.Text = "^";
                }
            };
            deviceTitle.GestureRecognizers.Add(tapGestureRecognizer);

            return deviceStack;
        }

        private StackLayout CreateDeviceResultsStack(ResultDataFromAgent resultFromDevice)
        {
            StackLayout resultsStack = new StackLayout()
            {
                IsVisible = false,
                BackgroundColor = Color.White
            };

            foreach (FileMetadata fileMetadata in resultFromDevice.FilesMetadata)
            {
                resultsStack.Children.Add(GetDeviceResultsListStack(fileMetadata, resultFromDevice.AgentGuid));
            }

            StackLayout resultsListStack = (StackLayout)resultsStack.Children[resultsStack.Children.Count - 1];
            resultsListStack.Children.RemoveAt(resultsListStack.Children.Count - 1);
            return resultsStack;
        }

        private StackLayout GetDeviceResultsListStack(FileMetadata fileMetadata, Guid agentGuid)
        {
            StackLayout resultStack = new StackLayout()
            {
                BackgroundColor = Color.White
            };

            StackLayout resultItemStack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(0, 0, 20, 0),
                BackgroundColor = Color.White
            };
            
            resultItemStack.Children.Add(new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = fileMetadata.FullPathAndName,
                FontSize = 14,
                TextColor = Color.Gray,
                BackgroundColor = Color.White
            });

            Button button = new Button
            {
                Text = "Open",
                BackgroundColor = Color.FromRgb(30, 144, 255),
                TextColor = Color.White,
                BorderColor = Color.White
            };

            button.Resources = new ResourceDictionary();
            button.Resources.Add("FullPath", fileMetadata.FullPathAndName);

            button.Clicked += (object s, EventArgs e) =>
            {
                Button openButton = (Button)s;
                string fullPathWithName = (string)openButton.Resources["FullPath"];
                var result = service.GetFileFromDevice(fullPathWithName, agentGuid, userGuid);
                fileHelper.SaveAndOpenFile(fullPathWithName, result.FileContent);
            };

            resultItemStack.Children.Add(button);
            
            resultStack.Children.Add(resultItemStack);

            resultStack.Children.Add(new BoxView
            {
                HeightRequest = 0.5,
                BackgroundColor = Color.FromRgb(211, 211, 211)
            });

            return resultStack;
        }
    }
}
