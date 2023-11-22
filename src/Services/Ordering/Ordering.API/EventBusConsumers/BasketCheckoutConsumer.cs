namespace Ordering.API.EventBusConsumers
{
    using System.Threading.Tasks;

    using EventBus.Messages.Events;

    using MassTransit;

    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        public Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
