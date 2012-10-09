using System;
using System.Collections.Generic;
using System.IO;
using Rappers.HipHop.Models;

namespace Rappers.HipHop.Services
{
    public interface IRemoteStorageService : IDisposable
    {
        List<RemoteResource> ListDirectory(string relativePath);
        bool FileExists(string relativePath);
        bool DirectoryExists(string relativePath);
        RemoteResource GetFile(string relativePath);
        RemoteResource GetDirectory(string relativePath);
        ILog Logger { get; set; }
        RemoteResource FindDirectory(string name, string startPath);
        void Traverse(string startPath, Action<List<RemoteResource>,string> action, bool deep);
        void CompareSingleDiretory(IRemoteStorageService destination, string directoryPath, Action<CompareInfo> onCompare, string directoryAlias);
        void Compare(IRemoteStorageService destination, Action<CompareInfo> onCompare, IEnumerable<KeyValuePair<string,string>> directoryAliases);
        void UploadFile(FileInfo file);
    }
}
