//using ServiceInterface;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Android.OS;
using AndroidOS = Android.OS;
using AndroidOSEnvironment = Android.OS.Environment;
using ServiceInterface;

[assembly: Dependency(typeof(CrossDeviceSearch.Droid.FileHelper))]

namespace CrossDeviceSearch.Droid
{
    class FileHelper : IFileHelper
    {
        public DeviceType DeviceType
        {
            get
            {
                return DeviceType.Android;
            }
        }

        public string DeviceModel
        {
            get
            {
                return AndroidOS.Build.Model;
            }
        }

        public void WriteFile(string filepath, byte[] bytes)
        {
            string tempPath = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileName(filepath));
            File.WriteAllBytes(Directory.GetCurrentDirectory(), bytes);
        }

        public string ReadText(string filepath)
        {
            return File.ReadAllText(filepath);
        }

        public List<FileMetadata> SearchFiles(string searchPattern)
        {
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
            return AndroidOSEnvironment.ExternalStorageDirectory.AbsolutePath;
            //return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
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
