using System;
using System.Collections.Generic;
using ServiceInterface;

namespace Agent
{
    public interface IFileHelper
    {
        DeviceType DeviceType { get; }
        bool Exists(string filename);

        void WriteText(string filename, string text);

        string ReadText(string filename);

        List<FileMetadata> SearchFiles(string searchPattern = "");

        void Delete(string filename);

        FileMetadata GetFileMetaData(string filepath);
    }
}
