using System;
using System.Collections.Generic;
using ServiceInterface;

namespace Agent
{
    public interface IFileHelper
    {
        DeviceType DeviceType { get; }
        string DeviceModel { get; }

        string ReadText(string filepath);

        byte[] ReadFile(string filepath);

        List<FileMetadata> SearchFiles(string searchPattern = "");
    }
}
