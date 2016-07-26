using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using ServiceInterface;
using ServiceMock;
using System.Collections.Generic;

namespace CrossDeviceSearch
{
    public partial class CrossDeviceSearchPage : ContentPage
    {
        const double MaxMatches = 100;
        //string bookText;
        IService service = new ServiceMock.ServiceMock();

        public CrossDeviceSearchPage()
        {
            InitializeComponent();

            // Load embedded resource bitmap.
            //string resourceID = "CrossDeviceSearch.Texts.MobyDick.txt";
            //Assembly assembly = GetType().GetTypeInfo().Assembly;

            //using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            //{
            //    using (StreamReader reader = new StreamReader(stream))
            //    {
            //        bookText = reader.ReadToEnd();
            //    }
            //}
        }

        void OnSearchBarTextChanged(object sender, TextChangedEventArgs args)
        {
            resultsStack.Children.Clear();
        }

        void OnSearchBarButtonPressed(object sender, EventArgs args)
        {
            // Detach resultsStack from layout.
            resultsScroll.Content = null;

            resultsStack.Children.Clear();
            //SearchBookForText(searchBar.Text);
            List<ResultDataFromAgent> results = service.SearchFileInAllDevices(searchBar.Text, new Guid());

            SetResults(results);

            // Reattach resultsStack to layout.
            resultsScroll.Content = resultsStack;
        }

        //void SearchBookForText(string searchText)
        //{
        //    int count = 0;
        //    bool isTruncated = false;

        //    using (StringReader reader = new StringReader(bookText))
        //    {
        //        int lineNumber = 0;
        //        string line;

        //        while (null != (line = reader.ReadLine()))
        //        {
        //            lineNumber++;
        //            int index = 0;

        //            while (-1 != (index = (line.IndexOf(searchText, index, 
        //                                                StringComparison.OrdinalIgnoreCase))))
        //            {
        //                if (count == MaxMatches)
        //                {
        //                    isTruncated = true;
        //                    break;
        //                }
        //                index += 1;

        //                // Add the information to the StackLayout.
        //                SetResults();

        //                count++;
        //            }

        //            if (isTruncated)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //}

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
                Orientation = StackOrientation.Horizontal
            };

            Button arrowButton = new Button
            {
                BackgroundColor = Color.White,
                Text = ">"
            };
            deviceTitle.Children.Add(arrowButton);

            Label nameLabel = new Label
            {
                Text = resultFromDevice.DeviceName
            };
            deviceTitle.Children.Add(nameLabel);

            StackLayout deviceResultsStack = CreateDeviceResultsStack(resultFromDevice);

            deviceStack.Children.Add(deviceTitle);

            deviceStack.Children.Add(deviceResultsStack);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                View view = (View)s;
                StackLayout stackLayout = (StackLayout)view.Parent;
                stackLayout.Children[1].IsVisible = !stackLayout.Children[1].IsVisible;
            };
            deviceTitle.GestureRecognizers.Add(tapGestureRecognizer);

            return deviceStack;
        }

        private StackLayout CreateDeviceResultsStack(ResultDataFromAgent resultFromDevice)
        {
            StackLayout resultsStack = new StackLayout()
            {
                IsVisible = false
            };

            foreach (FileMetadata fileMetadata in resultFromDevice.FilesMetadata)
            {
                resultsStack.Children.Add(GetDeviceResultsListStack(fileMetadata));
            }

            return resultsStack;
        }

        private StackLayout GetDeviceResultsListStack(FileMetadata fileMetadata)
        {
            StackLayout resultStack = new StackLayout();

            StackLayout resultItemStack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(0, 0, 20, 0)
            };
            
            resultItemStack.Children.Add(new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = fileMetadata.FullPathAndName,
                FontSize = 14

            });
            Button button = new Button
            {
                Text = "Open",
                BackgroundColor = Color.FromRgb(30, 144, 255),
                TextColor = Color.White,
                BorderColor = Color.White
            };
            
            resultItemStack.Children.Add(button);
            resultStack.Children.Add(new BoxView
            {
                HeightRequest = 1,
                BackgroundColor = Color.Gray
            });
            resultStack.Children.Add(resultItemStack);

            return resultStack;
        }

        //private StackLayout GetTextInfoStackLayout()
        //{
        //    StackLayout textStack = new StackLayout()
        //    {
        //        HorizontalOptions = LayoutOptions.FillAndExpand
        //    };
        //    textStack.Children.Add(new Label()
        //    {
        //        Text = "Title: name",
        //        FontSize = 14

        //    });
        //    StackLayout resultDetailsStack = new StackLayout()
        //    {
        //        Orientation = StackOrientation.Horizontal,
        //        Spacing = 50
        //    };

        //    resultDetailsStack.Children.Add(new Label()
        //    {
        //        Text = "Device: mydevice",
        //        TextColor = Color.Gray,
        //        FontSize = 10
        //    });
        //    resultDetailsStack.Children.Add(new Label()
        //    {
        //        Text = "OS: myos",
        //        TextColor = Color.Gray,
        //        FontSize = 10

        //    });

        //    textStack.Children.Add(resultDetailsStack);
        //    return textStack;
        //}

        //private void OnButtonClicked(object sender, EventArgs e)
        //{
        //    Debug.WriteLine("clicked");
        //}
    }
}
