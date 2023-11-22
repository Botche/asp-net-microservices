namespace Shopping.Aggregator.Services
{
    using Shopping.Aggregator.Extensions;
    using Shopping.Aggregator.Models;
    using Shopping.Aggregator.Services.Interfaces;

    public class CatalogService : ICatalogService
    {
        private readonly HttpClient client;

        public CatalogService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogAsync()
        {
            HttpResponseMessage response = await this.client.GetAsync("/api/v1/Catalog");

            return await response.ReadContentAsAsync<List<CatalogModel>>();
        }

        public async Task<CatalogModel> GetCatalogAsync(string categoryId)
        {
            HttpResponseMessage response = await this.client.GetAsync($"/api/v1/Catalog/{categoryId}");

            return await response.ReadContentAsAsync<CatalogModel>();
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategoryAsync(string category)
        {
            HttpResponseMessage response = await this.client.GetAsync($"/api/v1/Catalog/GetProductsByCategory/{category}");

            return await response.ReadContentAsAsync<List<CatalogModel>>();
        }
    }
}
