using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using ServiceInterface;
using ServiceMock;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;

namespace CrossDeviceSearch
{
    public partial class CrossDeviceSearchPage : ContentPage
    {
        const double MaxMatches = 100;
        //string bookText;
        private readonly IService service;
        private readonly string username;
        FileHelper fileHelper = new FileHelper();
        private bool isDeviceView;

        public CrossDeviceSearchPage(IService service, string username)
        {
			this.service = service;
            this.username = username;
            InitializeComponent();            
            
            SetDevicesList();
        }

        private void SetDevicesList()
        {
            resultsStack.Children.Clear();
            List<AgentData> devicesList = service.GetAgentsStatus();

            StackLayout devicesInGroupStack = new StackLayout();
            StackLayout devicesTitleStack = new StackLayout();

            if (devicesList.Count == 0)
            {
                devicesTitleStack.Children.Add(new Label()
                {
                    BackgroundColor = Color.White,
                    Text = string.Format("No devices for '{0}'.", username),
                    TextColor = Color.Gray,
                    FontSize = 14
                });

                devicesInGroupStack.Children.Add(devicesTitleStack);
            }
            else
            {
                devicesTitleStack.Children.Add(new Label()
                {
                    BackgroundColor = Color.White,
                    Text = string.Format("Devices for '{0}':", username),
                    TextColor = Color.Gray,
                    FontSize = 14
                });

                devicesInGroupStack.Children.Add(devicesTitleStack);

                StackLayout devicesListStack = new StackLayout();

                foreach (AgentData agentData in devicesList)
                {
                    StackLayout deviceIconStack = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal
                    };

                    string osImage = agentData.DeviceType == DeviceType.Android ? "CrossDeviceSearch.Images.AndroidIcon.png" : "CrossDeviceSearch.Images.WindowsIcon.png";

                    deviceIconStack.Children.Add(new Image()
                    {
                        Source = ImageSource.FromResource(osImage),
                        HeightRequest = 25,
                        WidthRequest = 25
                    });

                    StackLayout deviceNameStack = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                    };


                    deviceNameStack.Children.Add(new Label()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.White,
                        Text = Regex.Split(agentData.DeviceName, "  - ")[0],
                        TextColor = Color.Gray,
                        FontSize = 14,
                        VerticalTextAlignment = TextAlignment.Center
                    });

                    deviceNameStack.Children.Add(new Label()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Text = Regex.Split(agentData.DeviceName, "  - ")[1],
                        FontSize = 11,
                        TextColor = Color.FromRgb(211, 211, 211),
                        BackgroundColor = Color.White,
                        VerticalTextAlignment = TextAlignment.End,
                        VerticalOptions = LayoutOptions.End
                    });

                    deviceIconStack.Children.Add(deviceNameStack);
                    devicesListStack.Children.Add(deviceIconStack);
                    devicesListStack.Children.Add(new BoxView
                    {
                        HeightRequest = 0.5,
                        BackgroundColor = Color.FromRgb(211, 211, 211)
                    });
                }

                devicesInGroupStack.Children.Add(devicesListStack);
            }
            resultsStack.Children.Add(devicesInGroupStack);
            isDeviceView = true;
        }

        void OnSearchBarTextChanged(object sender, TextChangedEventArgs args)
        {
            if(!isDeviceView && args.NewTextValue == string.Empty)
            {
                SetDevicesList();
            }
            //resultsStack.Children.Clear();

        }

        async void OnSearchBarButtonPressed(object sender, EventArgs args)
        {
            // Detach resultsStack from layout.
            resultsScroll.Content = null;

            resultsStack.Children.Clear();

            ActivityIndicator activityIndicator = new ActivityIndicator()
            {
                VerticalOptions = LayoutOptions.Center,
                IsRunning = true,
                IsEnabled = true
            };
            resultsStack.Children.Add(activityIndicator);
            
            resultsScroll.Content = resultsStack;
            var results = await Task.Run(() =>
            {
                return service.SearchFileInAllDevices(searchBar.Text);
            });
            
            resultsStack.Children.Clear();
            activityIndicator.IsRunning = false;
            activityIndicator.IsEnabled = false;

            if (results.Count == 0)
            {
                PresentNoResultsFound();
            }
            else
            {
                SetResults(results);
            }

            isDeviceView = false;
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
                Text = "▶",
                TextColor = Color.FromRgb(30, 144, 255),
                BackgroundColor = Color.White
            };

            deviceTitle.Children.Add(arrowLabel);
            string resultCount = resultFromDevice.FilesMetadata.Count.ToString();
            if(resultFromDevice.FilesMetadata.Count >= 50)
            {
                resultCount = "50+";
            }
            Label nameLabel = new Label
            {
                Text = resultFromDevice.DeviceName+ "  (" + resultCount + ")",
                TextColor = Color.FromRgb(30, 144, 255),
                BackgroundColor = Color.White,
                VerticalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center
            };

            deviceTitle.Children.Add(nameLabel);

            StackLayout deviceResultsStack = null;
            if (resultFromDevice.FilesMetadata.Count > 0)
            {
                deviceResultsStack = CreateDeviceResultsStack(resultFromDevice);
            }

            deviceStack.Children.Add(deviceTitle);

            if (resultFromDevice.FilesMetadata.Count > 0)
            {
                deviceStack.Children.Add(deviceResultsStack);
            }

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
                    label.Text = "▼";
                }
                else
                {
                    label.Text = "▶";
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
                Padding = new Thickness(0, 0, 10, 0),
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.Center
            };
            
            resultItemStack.Children.Add(GetFileNameStack(fileMetadata));

            Button button = new Button
            {
                Text = "View",
                BackgroundColor = Color.FromRgb(30, 144, 255),
                TextColor = Color.White,
                BorderColor = Color.White,
                BorderWidth = 0.5,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 30,
                WidthRequest = 60,
                FontSize = 10
            };

            button.Resources = new ResourceDictionary();
            button.Resources.Add("FullPath", fileMetadata.FullPathAndName);
            button.Resources.Add("Time", fileMetadata.Time);
            button.Resources.Add("Size", fileMetadata.Size);

            button.Clicked += async(object s, EventArgs e) =>
            {
                Button openButton = (Button)s;
                string fullPathWithName = (string)openButton.Resources["FullPath"];
                Task<ResultDataFromAgent> getFile = new Task<ResultDataFromAgent>(
                    () => service.GetFileFromDevice(fullPathWithName, agentGuid));

                getFile.Start();

                bool shouldOpen = !(await DisplayAlert("", GetDetailsString(openButton) , "Close", "Open File"));
                if (shouldOpen)
                {
                    getFile.ContinueWith(
                        task => fileHelper.SaveAndOpenFile(fullPathWithName, task.Result.FileContent)).Wait();                 
                }
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

        private StackLayout GetFileNameStack(FileMetadata fileMetadata)
        {
            StackLayout filenameStack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White
            };

            filenameStack.Children.Add(new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = GetFileName(fileMetadata.FullPathAndName),
                FontSize = 14,
                TextColor = Color.Gray,
                BackgroundColor = Color.White,
                VerticalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center
            });
            filenameStack.Children.Add(new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Created: " + fileMetadata.Time.ToString(),
                FontSize = 10,
                TextColor = Color.FromRgb(211, 211, 211),
                BackgroundColor = Color.White,
                VerticalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.End
            });
            return filenameStack;
        }

        private string GetFileName(string path)
        {
            string fileName = Path.GetFileName(path);
            if (fileName == path)
            {
                fileName = fileName.Split(new char[] { '\\' }).Last();
            }
            return fileName;
        }

        private string GetDetailsString(Button button)
        {
            string result = "Created: " + button.Resources["Time"].ToString() + "\n";
            result += "Size: " + SizeSuffix((int)button.Resources["Size"]) + "\n";
            result += "File: " + (string)button.Resources["FullPath"];
            return result;
        }

        readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        string SizeSuffix(int value, int decimalPlaces = 0)
        {
            if (value < 0)
            {
                throw new ArgumentException("Bytes should not be negative", "value");
            }
            var mag = (int)Math.Max(0, Math.Log(value, 1024));
            var adjustedSize = Math.Round(value / Math.Pow(1024, mag), decimalPlaces);
            return String.Format("{0} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}
