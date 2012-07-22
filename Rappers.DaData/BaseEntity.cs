using MongoDB.Bson;

namespace Rappers.DaData
{
    public interface IBaseEntity
    {
        ObjectId Id { get; set; }
        string ExternalId { get; set; }
    }
    public class BaseEntity : IBaseEntity
    {
        public ObjectId Id { get; set; }
        public string ExternalId { get; set; }
    }
}
