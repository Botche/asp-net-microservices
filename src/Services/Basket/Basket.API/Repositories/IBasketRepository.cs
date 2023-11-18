namespace Basket.API.Repositories
{
    using Basket.API.Entities;

    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasketAsync(string userName);

        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket);

        Task DeleteBasketAsync(string userName);
    }
}
