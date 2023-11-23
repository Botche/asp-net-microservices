namespace AspnetRunBasics.Services.Interfaces
{
    using AspnetRunBasics.Models;

    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUserNameAsync(string userName);
    }
}
