namespace AspnetRunBasics.Services
{
    using AspnetRunBasics.Extensions;
    using AspnetRunBasics.Models;
    using AspnetRunBasics.Services.Interfaces;

    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("/Catalog");

            return await response.ReadContentAs<List<CatalogModel>>();
        }

        public async Task<CatalogModel> GetCatalogAsync(string id)
        {
            HttpResponseMessage response = await _client.GetAsync($"/Catalog/{id}");

            return await response.ReadContentAs<CatalogModel>();
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategoryAsync(string category)
        {
            HttpResponseMessage response = await _client.GetAsync($"/Catalog/GetProductByCategory/{category}");

            return await response.ReadContentAs<List<CatalogModel>>();
        }

        public async Task<CatalogModel> CreateCatalogAsync(CatalogModel model)
        {
            HttpResponseMessage response = await _client.PostAsJson($"/Catalog", model);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong when calling api.");
            }

            return await response.ReadContentAs<CatalogModel>();
        }
    }
}
