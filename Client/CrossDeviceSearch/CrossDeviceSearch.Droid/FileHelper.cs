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

        public List<FileMetadata> SearchFiles(string searchPattern)
        {
            //IEnumerable<string> filepaths = (String.IsNullOrEmpty(searchPattern) ? Directory.GetFiles(GetRootPath()) : Directory.GetFiles(GetRootPath(), searchPattern, SearchOption.AllDirectories));
            List<string> filepaths = new List<string>();
            if (!String.IsNullOrEmpty(searchPattern))
            {
                filepaths.AddRange(GetFilesInFolder(AndroidOSEnvironment.DirectoryDownloads, searchPattern));
                filepaths.AddRange(GetFilesInFolder(AndroidOSEnvironment.DirectoryDcim, searchPattern));
                filepaths.AddRange(GetFilesInFolder(AndroidOSEnvironment.DirectoryDocuments, searchPattern));
                filepaths.AddRange(GetFilesInFolder(AndroidOSEnvironment.DirectoryPictures, searchPattern));
                filepaths.AddRange(GetFilesInFolder(AndroidOSEnvironment.DirectoryMovies, searchPattern));
                filepaths.AddRange(GetFilesInFolder(AndroidOSEnvironment.DirectoryMusic, searchPattern));
            }
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
            return Path.Combine(AndroidOSEnvironment.ExternalStorageDirectory.AbsolutePath, AndroidOSEnvironment.DirectoryDownloads);
                //AndroidOSEnvironment.ExternalStorageDirectory.AbsolutePath;
        }

        private IEnumerable<string> GetFilesInFolder(string folder, string searchPattern)
        {
            string[] files;
            try
            {
                string folderPath = Path.Combine(AndroidOSEnvironment.ExternalStorageDirectory.AbsolutePath, folder);
                files = Directory.GetFiles(folderPath, searchPattern, SearchOption.AllDirectories);
            }
            catch {
                files = new string[0];
            }
            return files;
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
