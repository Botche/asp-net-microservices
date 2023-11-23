namespace AspnetRunBasics.Services.Interfaces
{
    using AspnetRunBasics.Models;

    public interface IBasketService
    {
        Task<BasketModel> GetBasketAsync(string userName);
        Task<BasketModel> UpdateBasketAsync(BasketModel model);
        Task CheckoutBasketAsync(BasketCheckoutModel model);
    }
}
