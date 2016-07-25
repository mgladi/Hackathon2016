using ServiceInterface;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Android.OS;
using AndroidOSEnvironment = Android.OS.Environment;

[assembly: Dependency(typeof(Agent.Android.FileHelper))]

namespace Agent.Android
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

        public bool Exists(string filename)
        {
            string filepath = GetFilePath(filename);
            var dirList = Directory.GetFiles(GetDocsPath());
            return File.Exists(filepath);
        }

        public void WriteText(string filename, string text)
        {
            string filepath = GetFilePath(filename);
            File.WriteAllText(filepath, text);
        }

        public string ReadText(string filename)
        {
            string filepath = GetFilePath(filename);
            return File.ReadAllText(filepath);
        }

        public List<FileMetadata> SearchFiles(string searchPattern)
        {
            //GetExternalStoragePublicDirectory(AndroidOS.Environment.RootDirectory)
            //AndroidOSEnvironment.ExternalStorageDirectory.AbsolutePath
            IEnumerable<string> filepaths = (String.IsNullOrEmpty(searchPattern) ? Directory.GetFiles(GetDocsPath()) : Directory.GetFiles(GetDocsPath(), searchPattern, SearchOption.AllDirectories));
            List<FileMetadata> filenames = new List<FileMetadata>();

            foreach (string filepath in filepaths)
            {
                filenames.Add(GetFileMetaData(filepath));
            }
            return filenames;
        }

        public FileMetadata GetFileMetaData(string filepath)
        {
            return new FileMetadata
            {
                FullPathAndName = filepath,
                Time = File.GetCreationTime(filepath),
                Size =  Convert.ToInt32(new FileInfo(filepath).Length)
            };
        }

        public void Delete(string filename)
        {
            File.Delete(GetFilePath(filename));
        }

        // Private methods.
        string GetFilePath(string filename)
        {
            return Path.Combine(GetDocsPath(), filename);
        }

        string GetDocsPath()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }
    }
}
