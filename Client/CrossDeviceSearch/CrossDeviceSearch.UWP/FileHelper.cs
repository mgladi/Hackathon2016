using ServiceInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;
using Windows.Networking;
using Windows.Networking.Connectivity;
using System.Linq;

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
                var hostNames = NetworkInformation.GetHostNames();
                var hostName = hostNames.FirstOrDefault(name => name.Type == HostNameType.DomainName)?.DisplayName ?? "???";
                return hostName;
            }
        }

        public List<FileMetadata> SearchFiles(string searchPattern)
        {
            return SearchFilesAsync(searchPattern).GetAwaiter().GetResult();
        }

        private async Task<List<FileMetadata>> SearchFilesAsync(string searchPattern)
        {
            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            List<FileMetadata> allFilesMetadata = await RecursiveFileSearch(picturesFolder, searchPattern);

            return allFilesMetadata;
        }

        private async Task<List<FileMetadata>> RecursiveFileSearch(StorageFolder folder, string searchPattern)
        {
            List<FileMetadata> filesInCurrentFolder = await GetMetadataFilesListOfFolder(folder, searchPattern); // initialized with first level files, and will be added recursivly

            var subFoldersList = await folder.GetFoldersAsync();
            foreach (var subFolder in subFoldersList)
            {
                List<FileMetadata> filesInSubFolders = await RecursiveFileSearch(subFolder, searchPattern);
                filesInCurrentFolder.AddRange(filesInSubFolders);
            }

            return filesInCurrentFolder;
        }

        private async Task<List<FileMetadata>> GetMetadataFilesListOfFolder(StorageFolder folder, string searchPattern)
        {
            IReadOnlyList <StorageFile> fileList = await folder.GetFilesAsync();

            List<FileMetadata> filenames = new List<FileMetadata>();
            foreach (StorageFile file in fileList)
            {
                if (isFileNameMatchThePattern(Path.GetFileName(file.Path), searchPattern))
                {
                    filenames.Add(GetFileMetaData(file.Path));
                }
            }

            return filenames;
        }

        private bool isFileNameMatchThePattern(string fileNameAndExtension, string searchPattern)
        {
            string[] fileNameAndExtensionArray = fileNameAndExtension.Split('.');
            string fileName = string.Join(".", fileNameAndExtensionArray, 0, fileNameAndExtensionArray.Length - 1);
            string fileExtension = fileNameAndExtensionArray.Last();

            if(!searchPattern.Contains("*"))
            {
                return fileName.Contains(searchPattern);
            }
            else // has *. search by extenstion
            {
                string[] patternNameAndExtenstionArray = searchPattern.Split('.');
                string patternExtenstion = patternNameAndExtenstionArray.Last();

                return fileExtension.StartsWith(patternExtenstion) || patternExtenstion.Equals("*");
            }
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
