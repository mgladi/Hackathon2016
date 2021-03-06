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
using System.Reflection;

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
            List<FileMetadata> allFilesMetadata = new List<FileMetadata>();

            // >>>>>> Comments are for folders that are unauthorized access)<<<<<<<<<<<<

            allFilesMetadata.AddRange(await RecursiveFileSearch(KnownFolders.AppCaptures, searchPattern));
            //allFilesMetadata.AddRange(await RecursiveFileSearch(KnownFolders.DocumentsLibrary, searchPattern)); // manifest error
            //allFilesMetadata.AddRange(await RecursiveFileSearch(KnownFolders.HomeGroup, searchPattern));
            //allFilesMetadata.AddRange(await RecursiveFileSearch(KnownFolders.MediaServerDevices, searchPattern));
            allFilesMetadata.AddRange(await RecursiveFileSearch(KnownFolders.MusicLibrary, searchPattern));
            allFilesMetadata.AddRange(await RecursiveFileSearch(KnownFolders.Objects3D, searchPattern));
            allFilesMetadata.AddRange(await RecursiveFileSearch(KnownFolders.PicturesLibrary, searchPattern));
            //allFilesMetadata.AddRange(await RecursiveFileSearch(KnownFolders.Playlists, searchPattern));
            //allFilesMetadata.AddRange(await RecursiveFileSearch(KnownFolders.RecordedCalls, searchPattern));
            //allFilesMetadata.AddRange(await RecursiveFileSearch(KnownFolders.RemovableDevices, searchPattern));
            allFilesMetadata.AddRange(await RecursiveFileSearch(KnownFolders.VideosLibrary, searchPattern));

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
            IReadOnlyList<StorageFile> fileList = await folder.GetFilesAsync();

            List<FileMetadata> filenames = new List<FileMetadata>();
            foreach (StorageFile file in fileList)
            {
                if (isFileNameMatchThePattern(Path.GetFileName(file.Path), searchPattern))
                {
                    filenames.Add(GetFileMetaData(file));
                }
            }

            return filenames;
        }

        private bool isFileNameMatchThePattern(string fileNameAndExtension, string searchPattern)
        {
            string[] fileNameAndExtensionArray = fileNameAndExtension.Split('.');
            string fileName = string.Join(".", fileNameAndExtensionArray, 0, fileNameAndExtensionArray.Length - 1);
            string fileExtension = fileNameAndExtensionArray.Last();

            if (!searchPattern.Contains("*"))
            {
                return fileName.ToLower().Contains(searchPattern.ToLower()); // ToLower to support key sensetive
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

        private FileMetadata GetFileMetaData(StorageFile file)
        {
            Stream stream = file.OpenReadAsync().AsTask().Result.AsStreamForRead();
            
            return new FileMetadata
            {
                FullPathAndName = file.Path,
                Time = file.DateCreated.DateTime,
                Size = (int)stream.Length 
            };

        }
    }
}
