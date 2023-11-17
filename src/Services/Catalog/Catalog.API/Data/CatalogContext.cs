namespace Catalog.API.Data
{
    using Catalog.API.Data.Seeds;
    using Catalog.API.Entities;

    using MongoDB.Driver;

    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            IMongoDatabase database = GetDatabase(configuration);

            string collectionName = configuration.GetValue<string>("DatabaseSettings:CollectionName");
            this.Products = database.GetCollection<Product>(collectionName);

            CatalogContextSeed.SeedData(this.Products);
        }

        public IMongoCollection<Product> Products { get; }

        private static IMongoDatabase GetDatabase(IConfiguration configuration)
        {
            string connetionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            string databaseName = configuration.GetValue<string>("DatabaseSettings:DatabaseName");
            MongoClient client = new(connetionString);

            return client.GetDatabase(databaseName);
        }
    }
}
