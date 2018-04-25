using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consumer.MongoDbEntity;
using Consumer.Option;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Consumer.Mongo
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoConfig> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<GuidEventLog> GuidEventLogs => _database.GetCollection<GuidEventLog>("consumerlog");
    }
}
