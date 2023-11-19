namespace Ordering.Application.Mappings
{
    using AutoMapper;

    using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
    using Ordering.Application.Features.Orders.Queries.GetOrdersList;
    using Ordering.Domain.Entities;

    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            this.CreateMap<Order, OrderVm>()
                .ReverseMap();
            this.CreateMap<CheckoutOrderCommand, Order>();
        }
    }
}
