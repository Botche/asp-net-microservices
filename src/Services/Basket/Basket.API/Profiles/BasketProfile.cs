namespace Basket.API.Profiles
{
    using AutoMapper;

    using Basket.API.Entities;

    using EventBus.Messages.Events;

    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            this.CreateMap<BasketCheckout, BasketCheckoutEvent>();
        }
    }
}
