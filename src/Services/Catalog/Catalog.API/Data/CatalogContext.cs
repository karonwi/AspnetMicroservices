using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration config)
        {
            var mongoClient = new MongoClient(config.GetValue<string>("CatalogDatabaseSettings:ConnectionString"));
            var database = mongoClient.GetDatabase(config.GetValue<string>("CatalogDatabaseSettings:DatabaseName"));
            Products = database.GetCollection<Product>(config.GetValue<string>("CatalogDatabaseSettings:DatabaseName"));
            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
