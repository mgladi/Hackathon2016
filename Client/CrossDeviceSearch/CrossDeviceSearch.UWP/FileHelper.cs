using ServiceInterface;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(CrossDeviceSearch.UWP.FileHelper))]

namespace CrossDeviceSearch.UWP
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
            //var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            //folderPicker.
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

        public string WriteTempFile(string filepath, byte[] bytes)
        {
            string tempPath = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileName(filepath));
            File.WriteAllBytes(tempPath, bytes);
            return tempPath;
        }

        public void OpenFile(string filepath)
        {
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
    }
}
