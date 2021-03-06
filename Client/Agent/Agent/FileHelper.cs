using Java.IO;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using ServiceInterface;
using System.Text;

namespace Agent
{
    class FileHelper
    {
        IFileHelper fileHelper = DependencyService.Get<IFileHelper>();

        public DeviceType DeviceType
        {
            get
            {
                return fileHelper.DeviceType;
            }
        }

        public string DeviceModel
        {
            get
            {
                return fileHelper.DeviceModel;
            }
        }

        public string ReadText(string filename)
        {
            return fileHelper.ReadText(filename);
        }

        public List<FileMetadata> SearchFiles(string searchPattern)
        {
            return String.IsNullOrEmpty(searchPattern) ? fileHelper.SearchFiles() : fileHelper.SearchFiles(searchPattern);
        }

        public string ReadFile(string filepath)
        {
            return GetFileContentFromByte(fileHelper.ReadFile(filepath));
        }

        private string GetFileContentFromByte(byte[] result)
        {
            return Encoding.UTF8.GetString(result, 0, result.Length);
        }
    }
}
