using ServiceInterface;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Foundation.Collections;
using Xamarin.Forms;

[assembly: Dependency(typeof(Agent.UniversalWindows.FileHelper))]

namespace Agent.UniversalWindows
{
    class FileHelper : IFileHelper
    {
        public DeviceType DeviceType
        {
            get
            {
                return DeviceType.Windows;
            }
        }
        public string DeviceModel
        {
            get
            {
                return "";
            }
        }

        public string ReadText(string filepath)
        {
            return File.ReadAllText(filepath);
        }

        public List<FileMetadata> SearchFiles(string searchPattern)
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            //folderPicker.PickFolderAndContinue()

            // var listToken = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(pickedFile);

            //  Windows.Storage.AccessCache.StorageItemAccessList

            // IObservableVector<Windows.Storage.StorageFolder> myPictureFolders = myPictures.Folders;

            Do();

            var curr = Directory.GetCurrentDirectory();
            var res = Directory.EnumerateFiles(curr);
            var res2 = Directory.GetFiles(curr);
            var mus = @"C:\Users\nirgafni\Music\aaa.txt";
            //var res21 = Directory.GetFiles(mus);
            IEnumerable<string> filepaths = (String.IsNullOrEmpty(searchPattern) ? Directory.GetFiles(GetRootPath()) : Directory.GetFiles(GetRootPath(), searchPattern, SearchOption.AllDirectories));
            List<FileMetadata> filenames = new List<FileMetadata>();

            foreach (string filepath in filepaths)
            {
                filenames.Add(GetFileMetaData(filepath));
            }

            return filenames;
        }

        public byte[] ReadFile(string filepath)
        {
            return File.ReadAllBytes(filepath);
        }

        // Private methods.
        string GetRootPath()
        {
            return @"C:\Users\nirgafni\Documents\";
        }

        private FileMetadata GetFileMetaData(string filepath)
        {
            return new FileMetadata
            {
                FullPathAndName = filepath,
                Time = File.GetCreationTime(filepath),
                Size = Convert.ToInt32(new FileInfo(filepath).Length)
            };
        }

        private async void Do()
        {
            var myPictures = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Music);
            //return myPictures;
            
        }
    }
}
