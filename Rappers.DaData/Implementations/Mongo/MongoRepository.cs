using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Rappers.DaData.Implementations.Mongo
{
    public class MongoRepository<T> : IRepository<T> where T : IBaseEntity
    {
        private readonly MongoDatabase _db;

        public MongoRepository(MongoConnection mongoConnection)
        {
            _db = mongoConnection.DataBase;
        }
        public T Save(T entity)
        {
            GetCollection().Save(entity);
            return entity;
        }

        public List<T> Search(IMongoQuery query)
        {
            return GetCollection().Find(query).ToList();
        }

        public void Delete(ObjectId id)
        {
            GetCollection().Remove(Query.EQ("_id", id));
        }

        public void DeleteAll()
        {
            var col = GetCollection();
            if (col != null)
            {
                col.RemoveAll();
            }
        }

        public List<T> GetAll()
        {
            return GetCollection().FindAll().ToList();
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> query)
        {
            return GetCollection().AsQueryable<T>().Where(query);
        }

        public T Get(ObjectId id)
        {
            return GetCollection().FindOneById(id);
        }

        public T GetByExternal(string externalId)
        {
            if (string.IsNullOrEmpty(externalId))
            {
                externalId = string.Empty;
            }
            return GetCollection().FindOne(Query.EQ("ExternalId", externalId));
        }

        private MongoCollection<T> GetCollection()
        {
            return _db.GetCollection<T>(typeof(T).Name);
        }
    }
}
