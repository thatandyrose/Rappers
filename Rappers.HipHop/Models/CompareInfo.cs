using Rappers.DaData;

namespace Rappers.HipHop.Models
{
    public class CompareInfo : BaseEntity
    {
        public string SourceParent { get; set; }
        public string SourcePath { get; set; }
        public long SourceBytes { get; set; }
        public long DestinationBytes { get; set; }
        public CompareStatus DestinationStatus { get; set; }
    }
}
