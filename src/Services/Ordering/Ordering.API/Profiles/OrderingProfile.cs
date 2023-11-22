namespace Ordering.API.Profiles
{
    using AutoMapper;

    using EventBus.Messages.Events;

    using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            this.CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>();
        }
    }
}
