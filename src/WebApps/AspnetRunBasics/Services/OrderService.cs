namespace AspnetRunBasics.Services
{
    using AspnetRunBasics.Extensions;
    using AspnetRunBasics.Models;
    using AspnetRunBasics.Services.Interfaces;

    public class OrderService : IOrderService
    {
        private readonly HttpClient _client;

        public OrderService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserNameAsync(string userName)
        {
            HttpResponseMessage response = await _client.GetAsync($"/Order/{userName}");

            return await response.ReadContentAs<List<OrderResponseModel>>();
        }
    }
}
