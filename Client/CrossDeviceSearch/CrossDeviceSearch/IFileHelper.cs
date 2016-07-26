using System;
using System.Collections.Generic;
using ServiceInterface;

namespace CrossDeviceSearch
{
    public interface IFileHelper
    {
        DeviceType DeviceType { get; }
        string DeviceModel { get; }

        string ReadText(string filepath);

        byte[] ReadFile(string filepath);
        void WriteFile(string filepath, byte[] bytes);

        List<FileMetadata> SearchFiles(string searchPattern = "");
    }
}
