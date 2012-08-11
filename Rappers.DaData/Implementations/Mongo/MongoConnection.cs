using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace Rappers.DaData.Implementations.Mongo
{
    public class MongoConnection
    {
        public MongoDatabase DataBase { get; set; }
        public MongoConnection(string connectionString)
        {
            DataBase = MongoDatabase.Create(connectionString);
        }
    }
}
