namespace AspnetRunBasics.Services.Interfaces
{
    using AspnetRunBasics.Models;

    public interface ICatalogService
    {
        Task<IEnumerable<CatalogModel>> GetCatalogAsync();
        Task<IEnumerable<CatalogModel>> GetCatalogByCategoryAsync(string category);
        Task<CatalogModel> GetCatalogAsync(string id);
        Task<CatalogModel> CreateCatalogAsync(CatalogModel model);
    }
}
