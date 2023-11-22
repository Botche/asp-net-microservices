namespace Shopping.Aggregator.Services
{
    using Shopping.Aggregator.Models;
    using Shopping.Aggregator.Services.Interfaces;

    public class BasketService : IBasketService
    {
        private readonly HttpClient client;

        public BasketService(HttpClient client)
        {
            this.client = client;
        }

        public Task<BasketModel> GetBasketAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
