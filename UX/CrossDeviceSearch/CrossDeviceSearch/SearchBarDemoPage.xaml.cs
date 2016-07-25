using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace CrossDeviceSearch
{
    public partial class CrossDeviceSearchPage : ContentPage
    {
        const double MaxMatches = 100;
        string bookText;

        public CrossDeviceSearchPage()
        {
            InitializeComponent();

            // Load embedded resource bitmap.
            string resourceID = "CrossDeviceSearch.Texts.MobyDick.txt";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    bookText = reader.ReadToEnd();
                }
            }
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
            SearchBookForText(searchBar.Text);

            // Reattach resultsStack to layout.
            resultsScroll.Content = resultsStack;
        }

        void SearchBookForText(string searchText)
        {
            int count = 0;
            bool isTruncated = false;

            using (StringReader reader = new StringReader(bookText))
            {
                int lineNumber = 0;
                string line;

                while (null != (line = reader.ReadLine()))
                {
                    lineNumber++;
                    int index = 0;

                    while (-1 != (index = (line.IndexOf(searchText, index, 
                                                        StringComparison.OrdinalIgnoreCase))))
                    {
                        if (count == MaxMatches)
                        {
                            isTruncated = true;
                            break;
                        }
                        index += 1;

                        // Add the information to the StackLayout.
                        SetResults();

                        count++;
                    }

                    if (isTruncated)
                    {
                        break;
                    }
                }
            }
        }

        private void SetResults()
        {
            StackLayout resultItemStack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(0, 0, 20, 0)
            };
            
            resultItemStack.Children.Add(GetTextInfoStackLayout());
            Button button = new Button
            {
                Text = "Download"
            };

            resultItemStack.Children.Add(button);
            resultsStack.Children.Add(new BoxView
            {
                HeightRequest = 1,
                BackgroundColor = Color.Gray
            });
            resultsStack.Children.Add(resultItemStack);
        }

        private StackLayout GetTextInfoStackLayout()
        {
            StackLayout textStack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            textStack.Children.Add(new Label()
            {
                Text = "Title: name",
                FontSize = 14

            });
            StackLayout resultDetailsStack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 50
            };

            resultDetailsStack.Children.Add(new Label()
            {
                Text = "Device: mydevice",
                TextColor = Color.Gray,
                FontSize = 10
            });
            resultDetailsStack.Children.Add(new Label()
            {
                Text = "OS: myos",
                TextColor = Color.Gray,
                FontSize = 10

            });

            textStack.Children.Add(resultDetailsStack);
            return textStack;
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("clicked");
        }
    }
}
