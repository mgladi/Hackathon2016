//using ServiceInterface;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Android.OS;
using Android.Content;
using ServiceInterface;

using AndroidOS = Android.OS;
using AndroidOSEnvironment = Android.OS.Environment;


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

        public string WriteTempFile(string filepath, byte[] bytes)
        {
            string tempPath = Path.Combine(GetPersonalPath(), Path.GetFileName(filepath));
            File.WriteAllBytes(tempPath, bytes);
            bool b = File.Exists(tempPath);
            return tempPath;
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

        public void OpenFile(string filepath)
        {
            string extension = Path.GetExtension(filepath);
            string application = "";
            switch (extension.ToLower())
            {
                case ".doc":
                case ".docx":
                    application = "application/msword";
                    break;
                case ".pdf":
                    application = "application/pdf";
                    break;
                case ".xls":
                case ".xlsx":
                    application = "application/vnd.ms-excel";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                    application = "image/jpeg";
                    break;
                default:
                    application = "*/*";
                    break;
            }
            Java.IO.File file = new Java.IO.File(filepath);
            file.SetReadable(true);
            //Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
            Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, application);
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            try
            {
                Xamarin.Forms.Forms.Context.StartActivity(intent);
            }
            catch (Exception)
            {

            }
        }


        // Private methods.
        private string GetRootPath()
        {
            return AndroidOSEnvironment.ExternalStorageDirectory.AbsolutePath;
        }

        private string GetPersonalPath()
        {
            return AndroidOSEnvironment.ExternalStorageDirectory.Path;
            //AndroidOSEnvironment.ExternalStorageDirectory.Path;
            //AndroidOSEnvironment.DownloadCacheDirectory.Path;
            //global::Android.OS.Environment.ExternalStorageDirectory.Path;
            //System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
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
