namespace Shopping.Aggregator.Services.Interfaces
{
    using Shopping.Aggregator.Models;

    public interface IBasketService
    {
        Task<BasketModel> GetBasketAsync(string userName);
    }
}
