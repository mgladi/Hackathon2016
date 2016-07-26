using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using ServiceInterface;
using System.Text;
using System.Linq;

namespace CrossDeviceSearch
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
            return (String.IsNullOrEmpty(searchPattern) ? fileHelper.SearchFiles() : fileHelper.SearchFiles(searchPattern)).OrderByDescending(fileMetadata => fileMetadata.Time).Take(50).ToList();
        }

        public string ReadFile(string filepath)
        {
            byte[] arr = fileHelper.ReadFile(filepath);
            string content = Convert.ToBase64String(arr);
            return content;
        }

        public void SaveAndOpenFile(string filepath, string fileContent)
        {
            byte[] arr2 = Convert.FromBase64String(fileContent);
            string tempPath = fileHelper.WriteTempFile(filepath, arr2);
            fileHelper.OpenFile(tempPath);
        }
    }
}
