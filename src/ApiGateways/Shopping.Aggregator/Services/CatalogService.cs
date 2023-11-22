using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Interfaces;

namespace Shopping.Aggregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient client;

        public CatalogService(HttpClient client)
        {
            this.client = client;
        }

        public Task<IEnumerable<CatalogModel>> GetCatalogAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CatalogModel> GetCatalogAsync(string categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CatalogModel>> GetCatalogByCategoryAsyn()
        {
            throw new NotImplementedException();
        }
    }
}
