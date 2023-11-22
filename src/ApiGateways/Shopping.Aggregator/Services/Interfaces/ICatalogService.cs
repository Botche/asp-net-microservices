namespace Shopping.Aggregator.Services.Interfaces
{
    using Shopping.Aggregator.Models;

    public interface ICatalogService
    {
        Task<IEnumerable<CatalogModel>> GetCatalogAsync();

        Task<IEnumerable<CatalogModel>> GetCatalogByCategoryAsync(string category);

        Task<CatalogModel> GetCatalogAsync(string categoryId);
    }
}
