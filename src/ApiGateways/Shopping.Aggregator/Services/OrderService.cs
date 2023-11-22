namespace Shopping.Aggregator.Services
{
    using Shopping.Aggregator.Models;
    using Shopping.Aggregator.Services.Interfaces;

    public class OrderService : IOrderService
    {
        private readonly HttpClient client;

        public OrderService(HttpClient client)
        {
            this.client = client;
        }

        public Task<IEnumerable<OrderResponseModel>> GetOrdersByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
