using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rappers.HipHop.Models;

namespace Rappers.HipHop.Services.Implementations
{
    public abstract class BaseStorageService : IRemoteStorageService
    {
        public abstract List<RemoteResource> ListDirectory(string relativePath);
        public abstract void UploadFile(FileInfo file, Action<int> uploadProgress);
        public virtual void Dispose()
        {
            //nothing by default
        }

        public virtual bool FileExists(string relativePath)
        {
            return GetFile(relativePath) != null;
        }

        public virtual bool DirectoryExists(string relativePath)
        {
            return GetDirectory(relativePath) != null;
        }
        
        public virtual RemoteResource GetFile(string relativePath)
        {
            relativePath = relativePath.Replace(@"\", "/");
            try
            {
                string filename = relativePath.Split('/').Last(s => !string.IsNullOrEmpty(s));
                var resources = ListDirectory(relativePath.Replace(filename, ""));
                return resources.FirstOrDefault(r => r.Name == filename && r.ResourceType == ResourceType.File);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Get File Error: {0}", relativePath), ex);
            }

        }
        public virtual RemoteResource GetDirectory(string relativePath)
        {
            relativePath = relativePath.Replace(@"\", "/");
            try
            {
                string dirName = relativePath.Split('/').Last(s => !string.IsNullOrEmpty(s));
                var resources = ListDirectory(relativePath.Replace(dirName, "").Replace("//", ""));
                return resources.FirstOrDefault(r => r.Name == dirName && r.ResourceType == ResourceType.Directory);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Get Directory Error: {0}", relativePath), ex);
            }
        }
        private ILog _logger;

        public virtual ILog Logger
        {
            get { return _logger ?? (_logger = new MockLog()); }
            set { _logger = value; }
        }
        public RemoteResource FindDirectory(string name, string startPath)
        {
            RemoteResource found = null;
            Logger.Info(string.Format("Searching for '{0}' in '{1}'", name, startPath));
            
            Traverse(startPath, (resources, currentPath) =>
            {
                found = resources.FirstOrDefault(r =>
                r.Name.ToLower() == name.ToLower() &&
                r.ResourceType == ResourceType.Directory);
                if (found != null)
                {
                    Logger.Info(string.Format("Found '{0}' in '{1}'", name, currentPath));
                    return;
                }

                var dirs = resources.Where(r => r.ResourceType == ResourceType.Directory);
                foreach (var dir in dirs)
                {
                    found = FindDirectory(name, dir.Path);
                    if (found != null)
                    {
                        break;
                    }
                }    
            },true);

            return found;
        }

        public virtual long GetSize(string startPath, bool deep)
        {
            long size = 0;
            Traverse(startPath, (resources, currentPath) =>
            {
                var files = resources.Where(r => r.ResourceType == ResourceType.File).ToList();
                Logger.Info(string.Format("Fol: {0}, items: {1}", currentPath, files.Count));

                files.ForEach(r =>
                {
                    size += r.Size;
                });
            }, deep);

            return size;
        }

        public virtual void Traverse(string startPath, Action<List<RemoteResource>, string> action, bool deep)
        {
            var resources = ListDirectory(startPath);
            if (resources != null)
            {
                action(resources, startPath);
                resources.ForEach(r =>
                {
                    if (r.ResourceType == ResourceType.Directory && deep)
                    {
                        Traverse(r.Path, action, deep);
                    }
                });
            }
        }

        public void CompareSingleDiretory(IRemoteStorageService destination, string directoryPath, Action<CompareInfo> onCompare, string directoryAlias)
        {
            Compare(destination, directoryPath, false, onCompare, new[]{new KeyValuePair<string, string>(directoryPath,directoryAlias)});
        }

        public void Compare(IRemoteStorageService destination, Action<CompareInfo> onCompare, IEnumerable<KeyValuePair<string,string>> directoryAliases)
        {
            Compare(destination, "/", true, onCompare, directoryAliases);
        }

        

        private void Compare(IRemoteStorageService destination, string startPath, bool deep, Action<CompareInfo> onCompare, IEnumerable<KeyValuePair<string,string>> directoryAliases)
        {
            Traverse(startPath,(resources,currentPath) =>
            {
                Logger.Info(string.Format("Traversing in source folder '{0}' with {1} items", currentPath, resources.Count));
                string destFolder = currentPath;
                if(directoryAliases != null)
                {
                    var alias = directoryAliases.FirstOrDefault(kv => kv.Key.ToLower() == currentPath.ToLower());
                    if(!string.IsNullOrEmpty(alias.Value))
                    {
                        destFolder = alias.Value;
                    }
                }
               
                List<RemoteResource> destObjects = destination.ListDirectory(destFolder);
                resources.Where(r => r.ResourceType == ResourceType.File).ToList().ForEach(resource =>
                {
                    var dest = destObjects.FirstOrDefault(d => d.Name.ToLower() == resource.Name.ToLower());
                    if (dest != null)
                    {
                        var comp = new CompareInfo()
                        {
                            SourceParent = currentPath,
                            SourcePath = resource.Path,
                            DestinationStatus = CompareStatus.Same,
                            SourceBytes = resource.Size,
                            DestinationBytes = dest.Size
                        };
                        if (dest.Size != resource.Size)
                        {
                            comp.DestinationStatus = CompareStatus.Different;
                        }
                        onCompare(comp); 
                    }
                    else
                    {
                        onCompare(new CompareInfo()
                        {
                            SourceParent = currentPath,
                            SourcePath = resource.Path,
                            DestinationStatus = CompareStatus.Missing,
                            SourceBytes = resource.Size
                        }); 
                    }
                });                      
            },deep);
        }
    }
}
