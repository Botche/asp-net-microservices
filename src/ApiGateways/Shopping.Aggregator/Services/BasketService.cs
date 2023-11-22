namespace Shopping.Aggregator.Services
{
    using Shopping.Aggregator.Extensions;
    using Shopping.Aggregator.Models;
    using Shopping.Aggregator.Services.Interfaces;

    public class BasketService : IBasketService
    {
        private readonly HttpClient client;

        public BasketService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<BasketModel> GetBasketAsync(string userName)
        {
            HttpResponseMessage response = await this.client.GetAsync($"/api/v1/Basket/{userName}");

            return await response.ReadContentAsAsync<BasketModel>();
        }
    }
}
