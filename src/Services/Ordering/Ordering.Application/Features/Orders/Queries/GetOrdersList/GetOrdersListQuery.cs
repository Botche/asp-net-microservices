namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    using MediatR;

    public class GetOrdersListQuery : IRequest<List<OrderVm>>
    {
        public GetOrdersListQuery(string userName)
        {
            this.UserName = userName;
        }

        public string UserName { get; set; }
    }
}
