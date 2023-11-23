namespace AspnetRunBasics.Services
{
    using AspnetRunBasics.Extensions;
    using AspnetRunBasics.Models;
    using AspnetRunBasics.Services.Interfaces;

    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;

        public BasketService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<BasketModel> GetBasketAsync(string userName)
        {
            HttpResponseMessage response = await _client.GetAsync($"/Basket/{userName}");

            return await response.ReadContentAs<BasketModel>();
        }

        public async Task<BasketModel> UpdateBasketAsync(BasketModel model)
        {
            HttpResponseMessage response = await _client.PostAsJson($"/Basket", model);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong when calling api.");
            }

            return await response.ReadContentAs<BasketModel>();
        }

        public async Task CheckoutBasketAsync(BasketCheckoutModel model)
        {
            HttpResponseMessage response = await _client.PostAsJson($"/Basket/Checkout", model);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }
    }
}
