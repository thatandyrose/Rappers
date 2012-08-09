using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.S3;
using Amazon.S3.Model;
using Rappers.HipHop.Models;

namespace Rappers.HipHop.Services.Implementations
{
    public class S3StorageService : BaseStorageService, IRemoteStorageService
    {
        private readonly AmazonS3 _s3Client;
        private readonly string _bucket;

        public S3StorageService(string key, string secret, string bucket)
        {
            _s3Client = Amazon.AWSClientFactory.CreateAmazonS3Client(key, secret);
            _bucket = bucket;
        }

        public override List<RemoteResource> ListDirectory(string relativePath)
        {
            relativePath = relativePath.Replace(@"\", "/");
            try
            {
                relativePath = CleanRelativePath(relativePath);

                var resources = new List<RemoteResource>();
                var listRequest = new ListObjectsRequest()
                    .WithBucketName(_bucket)
                    .WithPrefix(relativePath)
                    .WithDelimiter("/");

                do
                {
                    using (ListObjectsResponse listResponse = _s3Client.ListObjects(listRequest))
                    {
                        resources.AddRange(MapResponse(listResponse));
                        if(listResponse.IsTruncated)
                        {
                            listRequest.Marker = listResponse.NextMarker;
                        }
                        else
                        {
                            listRequest = null;
                        }
                    }
                } while (listRequest != null);
                
                return resources;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("List Directory Error {0}",relativePath),ex);
            }
            
        }

        public override void Dispose()
        {
            _s3Client.Dispose();
        }

        private string GetObjectName(string path)
        {
            return path.Split('/').Last(s => !string.IsNullOrEmpty(s));
        }

        private string CleanRelativePath(string path)
        {
            if (path == "/" || string.IsNullOrEmpty(path))
            {
                path = null;
            }
            else
            {
                string[] parts = path
                    .Split('/')
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();

                path = string.Concat(string.Join("/", parts), "/");
            }

            return path;
        }

        private IEnumerable<RemoteResource> MapResponse(ListObjectsResponse listResponse)
        {
            var resources = new List<RemoteResource>();
            var objects = listResponse.S3Objects;
            resources.AddRange(objects.Where(e => !e.Key.EndsWith("/"))
            .Select(e => new RemoteResource()
            {
                Host = _bucket,
                Name = GetObjectName(e.Key),
                Path = e.Key,
                ResourceType = ResourceType.File,
                Size = e.Size,
                StorageType = StorageType.S3
            }));
            resources.AddRange(listResponse.CommonPrefixes
            .Select(f => new RemoteResource()
            {
                Host = _bucket,
                Name = GetObjectName(f),
                Path = f,
                ResourceType = ResourceType.Directory,
                StorageType = StorageType.S3
            }));
            return resources;
        }
    }
}
