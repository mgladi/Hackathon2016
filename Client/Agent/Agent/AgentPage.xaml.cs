using System;
using System.Linq;
using Xamarin.Forms;

namespace Agent
{
    public partial class AgentPage : ContentPage
    {
        FileHelper fileHelper = new FileHelper();
        string searchPattern = "";

        public AgentPage()
        {
            InitializeComponent();

            RefreshListView();
        }
        async void OnFindButtonClicked(object sender, EventArgs args)
        {
            searchPattern = filenameEntry.Text;
            RefreshListView();
        }

        async void OnSaveButtonClicked(object sender, EventArgs args)
        {
            string filename = filenameEntry.Text;

            if (fileHelper.Exists(filename))
            {
                bool okResponse = await DisplayAlert("Agent",
                                                     "File " + filename +
                                                     " already exists. Replace it?",
                                                     "Yes", "No");
                if (!okResponse)
                    return;
            }

            string errorMessage = null;

            try
            {
                fileHelper.WriteText(filenameEntry.Text, fileEditor.Text);
            }
            catch (Exception exc)
            {
                errorMessage = exc.Message;
            }

            if (errorMessage == null)
            {
                filenameEntry.Text = "";
                fileEditor.Text = "";
                RefreshListView();
            }
            else
            {
                await DisplayAlert("Agent", errorMessage, "OK");
            }
        }

        async void OnFileListViewItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (args.SelectedItem == null)
                return;

            string filename = (string)args.SelectedItem;
            string errorMessage = null;

            try
            {
                fileEditor.Text = fileHelper.ReadText((string)args.SelectedItem);
                filenameEntry.Text = filename;
            }
            catch (Exception exc)
            {
                errorMessage = exc.Message;
            }

            if (errorMessage != null)
            {
                await DisplayAlert("Agent", errorMessage, "OK");
            }
        }

        void OnDeleteMenuItemClicked(object sender, EventArgs args)
        {
            string filename = (string)((MenuItem)sender).BindingContext;
            fileHelper.Delete(filename);
            RefreshListView();
        }

        void RefreshListView()
        {
            fileListView.ItemsSource = fileHelper.SearchFiles(searchPattern).Select(metadata => metadata.FullPathAndName);
            fileListView.SelectedItem = null;
        }
    }
}
