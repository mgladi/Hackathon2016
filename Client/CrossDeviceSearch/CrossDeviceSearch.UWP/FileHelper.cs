using ServiceInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
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
                // TODO: 
                return "";
            }
        }

        public List<FileMetadata> SearchFiles(string searchPattern)
        {
            return SearchFilesAsync(searchPattern).GetAwaiter().GetResult();
        }

        private async Task<List<FileMetadata>> SearchFilesAsync(string searchPattern)
        {
            // TODO: Extend with mask functionality ("*.jpeg"), subfolders, all public libraries
            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;

            IReadOnlyList<StorageFile> fileList = await picturesFolder.GetFilesAsync();

            List<FileMetadata> filenames = new List<FileMetadata>();
            foreach (StorageFile file in fileList)
            {
                filenames.Add(GetFileMetaData(file.Path));
            }

            return filenames;
        }

        public byte[] ReadFile(string filepath)
        {
            return ReadFileAsync(filepath).GetAwaiter().GetResult();
        }

        private async Task<byte[]> ReadFileAsync(string filepath)
        {
            var file = await StorageFile.GetFileFromPathAsync(filepath);
            Stream stream = (await file.OpenReadAsync()).AsStreamForRead();

            byte[] bytes;
            using (BinaryReader reader = new BinaryReader((await file.OpenReadAsync()).AsStreamForRead()))
            {
                 bytes = reader.ReadBytes((int)stream.Length);
            }

            return bytes;
        }

        public string WriteTempFile(string filepath, byte[] bytes)
        {
            return WriteTempFileAsync(filepath, bytes).GetAwaiter().GetResult();
        }

        private Task<string> WriteTempFileAsync(string filepath, byte[] bytesh)
        {
            var file = GetLocalPath().CreateFileAsync(Path.GetFileName(filepath), CreationCollisionOption.ReplaceExisting).AsTask().Result;
            using (BinaryWriter writer = new BinaryWriter(file.OpenStreamForWriteAsync().GetAwaiter().GetResult()))
            {
                writer.Write(bytesh);
            }

            return Task.FromResult(Path.GetFileName(filepath));
        }

        public void OpenFile(string filepath)
        {
            var file = GetLocalPath().GetFileAsync(filepath).AsTask().Result;
            var success = Windows.System.Launcher.LaunchFileAsync(file).AsTask().Result;
        }

        // Private methods.
        StorageFolder GetLocalPath()
        {
            return Windows.Storage.ApplicationData.Current.LocalFolder;
        }

        private FileMetadata GetFileMetaData(string filepath)
        {
            //TODO: finish
            return new FileMetadata
            {
                FullPathAndName = filepath,
                Time = DateTime.Now, // File.GetCreationTime(filepath),
                Size = 10 // Convert.ToInt32(new FileInfo(filepath).Length)
            };
        }
    }
}
