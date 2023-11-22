namespace Shopping.Aggregator.Services
{
    using Shopping.Aggregator.Extensions;
    using Shopping.Aggregator.Models;
    using Shopping.Aggregator.Services.Interfaces;

    public class OrderService : IOrderService
    {
        private readonly HttpClient client;

        public OrderService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserNameAsync(string userName)
        {
            HttpResponseMessage response = await this.client.GetAsync($"/api/v1/Order/{userName}");

            return await response.ReadContentAsAsync<IEnumerable<OrderResponseModel>>();
        }
    }
}
