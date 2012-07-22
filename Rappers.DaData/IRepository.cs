using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Rappers.DaData
{
    public interface IRepository<T> where T : IBaseEntity
    {
        T Save(T entity);
        T Get(ObjectId id);
        T GetByExternal(string externalId);
        List<T> GetAll();
        IQueryable<T> Search(Expression<Func<T, bool>> query);
        List<T> Search(IMongoQuery query);
        void Delete(ObjectId id);
        void DeleteAll();
    }
}
