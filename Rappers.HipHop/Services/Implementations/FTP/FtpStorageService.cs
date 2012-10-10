using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Rappers.HipHop.Models;

namespace Rappers.HipHop.Services.Implementations.FTP
{
    public class FtpStorageService : BaseStorageService, IRemoteStorageService
    {
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;

        public FtpStorageService(string host, string username, string password)
        {
            _password = password;
            _username = username;
            _host = string.Format("ftp://{0}", host.Replace("ftp://", ""));
        }

        public override List<RemoteResource> ListDirectory(string relativePath)
        {
            relativePath = relativePath.Replace(@"\", "/");
            if(!relativePath.EndsWith("/"))
            {
                relativePath = string.Concat(relativePath, "/");
            }
            
            try
            {
                Logger.Info(string.Format("Listing for '{0}'",relativePath));
                return ProcessResponse(
                    WebRequestMethods.Ftp.ListDirectory,
                    relativePath,
                    (r) =>
                    {
                        List<string> names = blobToLines(r.ReadToEnd());
                        return GetResourceDetail(relativePath, names);
                    })
                    .ToList().FirstOrDefault(); 
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("List Error: {0}",relativePath),ex);
            }
            
        }

        public override void UploadFile(FileInfo file, Action<int> uploadProgress)
        {
            var r = GetResponse(WebRequestMethods.Ftp.UploadFile, "/", file, uploadProgress);
        }

        private List<string> blobToLines(string blob)
        {
            return blob.Replace(Environment.NewLine, ",").Split(',').ToList();
        } 

        private List<RemoteResource> GetResourceDetail(string relativePath, IEnumerable<string> resourceNames)
        {
            Func<StreamReader, List<RemoteResource>> action = (r) =>
            {
                var resources = new List<RemoteResource>();
                blobToLines(r.ReadToEnd()).ForEach(lineDetail =>
                {
                    var resourceName = resourceNames.First(s => lineDetail.EndsWith(s));
                    if (!string.IsNullOrEmpty(resourceName))
                    {
                        var model = new RemoteResource()
                        {
                            Host = _host,
                            Name = resourceName,
                            Path = string.Concat(relativePath.EndsWith("/") ? relativePath : string.Concat(relativePath,"/"),resourceName),
                            ResourceType = lineDetail.StartsWith("d") ? ResourceType.Directory : ResourceType.File,
                            StorageType = StorageType.FTP
                        };
                        if (model.ResourceType == ResourceType.File)
                        {
                            string size = lineDetail.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ElementAtOrDefault(4);
                            model.Size = long.Parse(string.IsNullOrEmpty(size) ? "0" : size);
                        }
                        resources.Add(model);
                    }                                     
                });
                return resources;
            };

            return ProcessResponse(WebRequestMethods.Ftp.ListDirectoryDetails, relativePath, action).FirstOrDefault();
        }

        private IEnumerable<T> ProcessResponse<T>(string method, string relativePath, Func<StreamReader, T> streamAction)
        {
            using (var response = GetResponse(method, relativePath))
            {
                var stream = response.GetResponseStream();
                using (var streamReader = new StreamReader(stream))
                {
                    while (StreamStillAvailable(streamReader))
                    {
                        yield return streamAction(streamReader);
                    }
                }
            }
        }

        private bool StreamStillAvailable(StreamReader reader)
        {
            try
            {
                return !reader.EndOfStream;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }

        private FtpWebResponse GetResponse(string method, string relativePath, FileInfo fileToUpload, Action<int> uploadProgress)
        {
            if (!relativePath.StartsWith("/"))
            {
                relativePath = string.Format("/{0}", relativePath);
            }
            string uri;
            if(fileToUpload!=null)
            {
                uri = string.Format("{0}{1}{2}", _host, relativePath,fileToUpload.Name);
            }
            else
            {
                uri = string.Format("{0}{1}", _host, relativePath);
            }
            
            var ftpRequest = (FtpWebRequest)WebRequest.Create(uri);
            ftpRequest.Credentials = new NetworkCredential(_username, _password);
            ftpRequest.Method = method;

            if(fileToUpload != null)
            {
                ftpRequest.ContentLength = fileToUpload.Length;

                using (var inputStream = File.OpenRead(fileToUpload.FullName))
                using (var outputStream = ftpRequest.GetRequestStream())
                {
                    var buffer = new byte[1024 * 1024];
                    int totalReadBytesCount = 0;
                    int readBytesCount;
                    while ((readBytesCount = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outputStream.Write(buffer, 0, readBytesCount);
                        
                        if(uploadProgress != null)
                        {
                            totalReadBytesCount += readBytesCount;
                            var progress = totalReadBytesCount * 100.0 / inputStream.Length;
                            uploadProgress((int)progress);
                        }
                    }
                }
            }

            return (FtpWebResponse)ftpRequest.GetResponse();
        }

        private FtpWebResponse GetResponse(string method, string relativePath)
        {
            return GetResponse(method, relativePath, null, null);
        }
    }
}
