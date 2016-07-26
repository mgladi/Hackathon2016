using System;
using System.Collections.Generic;
using ServiceInterface;

namespace CrossDeviceSearch
{
    public interface IFileHelper
    {
        DeviceType DeviceType { get; }
        string DeviceModel { get; }
        byte[] ReadFile(string filepath);
        string WriteTempFile(string filepath, byte[] bytes);
        void OpenFile(string filepath);
        List<FileMetadata> SearchFiles(string searchPattern = "");
    }
}
