using Rappers.DaData;

namespace Rappers.HipHop.Models
{
    public class RemoteResource : BaseEntity
    {
        public string Host { get; set; }
        public StorageType StorageType { get; set; }
        public ResourceType ResourceType { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
    }
}
