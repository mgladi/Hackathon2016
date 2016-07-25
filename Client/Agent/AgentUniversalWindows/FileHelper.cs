using ServiceInterface;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Agent;

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
        public bool Exists(string filename)
        {
            return false;
        }

        public void WriteText(string filename, string text)
        {
            throw new NotImplementedException("Writing files is not implemented");
        }

        public string ReadText(string filename)
        {
            throw new NotImplementedException("Reading files is not implemented");
        }

        public List<FileMetadata> SearchFiles(string searchPattern)
        {
            return new List<FileMetadata>();
        }

        public void Delete(string filename)
        {
        }

        public FileMetadata GetFileMetaData(string filepath)
        {
            throw new NotImplementedException();
        }
    }
}
