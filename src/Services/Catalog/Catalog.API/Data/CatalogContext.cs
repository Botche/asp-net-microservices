namespace Catalog.API.Data
{
    using Catalog.API.Entities;

    using MongoDB.Driver;

    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            var database = GetDatabase(configuration);

            var collectionName = configuration.GetValue<string>("DatabaseSettings:CollectionName");
            this.Products = database.GetCollection<Product>(collectionName);

            // CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }

        private static IMongoDatabase GetDatabase(IConfiguration configuration)
        {
            var connetionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            var databaseName = configuration.GetValue<string>("DatabaseSettings:DatabaseName");
            var client = new MongoClient(connetionString);

            return client.GetDatabase(databaseName);
        }
    }
}
