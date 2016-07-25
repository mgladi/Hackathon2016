using Java.IO;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using ServiceInterface;


namespace Agent
{
    class FileHelper : IFileHelper
    {
        IFileHelper fileHelper = DependencyService.Get<IFileHelper>();

        public DeviceType DeviceType
        {
            get
            {
                return fileHelper.DeviceType;
            }
        }

        public bool Exists(string filename)
        {
            return fileHelper.Exists(filename);
        }

        public void WriteText(string filename, string text)
        {
            fileHelper.WriteText(filename, text);
        }

        public string ReadText(string filename)
        {
            return fileHelper.ReadText(filename);
        }

        public FileMetadata GetFileMetaData(string filepath)
        {
            return fileHelper.GetFileMetaData(filepath);
        }

        public List<FileMetadata> SearchFiles(string searchPattern)
        {
            return String.IsNullOrEmpty(searchPattern) ? fileHelper.SearchFiles() : fileHelper.SearchFiles(searchPattern);
        }

        public void Delete(string filename)
        {
            fileHelper.Delete(filename);
        }
    }
}
