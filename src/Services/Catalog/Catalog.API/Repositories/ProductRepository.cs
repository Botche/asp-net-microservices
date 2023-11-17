namespace Catalog.API.Repositories
{
    using Catalog.API.Data;
    using Catalog.API.Entities;

    using MongoDB.Driver;

    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext context;

        public ProductRepository(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task CreateProductAsync(Product product)
        {
            await this.context.Products
                .InsertOneAsync(product);
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter
                .Eq(p => p.Id, id);

            DeleteResult deleteResult = await this.context.Products
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<Product> GetProductAsync(string id)
        {
            return await this.context.Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter
                .Eq(p => p.Category, categoryName);

            return await this.context.Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByNameAsync(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter
                .Eq(p => p.Name, name);

            return await this.context.Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await this.context.Products
                .Find(p => true)
                .ToListAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            ReplaceOneResult updateResult = await this.context.Products
                .ReplaceOneAsync(p => p.Id == product.Id, product);

            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }
    }
}
