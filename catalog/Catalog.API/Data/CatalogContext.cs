using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Settings;
using MongoDB.Driver;
using System;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatologContext
    {
        public CatalogContext(ICatalogDatabaseSettings sts)
        {
            var client = new MongoClient(sts.ConectionString);
            var db = client.GetDatabase(sts.DatabaseName);

            Products = db.GetCollection<Product>(sts.CollectionName);
            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
