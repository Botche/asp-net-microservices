namespace Shopping.Aggregator.Services.Interfaces
{
    using Shopping.Aggregator.Models;

    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUserNameAsync(string userName);
    }
}
