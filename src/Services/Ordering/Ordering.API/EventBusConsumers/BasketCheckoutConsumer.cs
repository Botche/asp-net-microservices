namespace Ordering.API.EventBusConsumers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using EventBus.Messages.Events;

    using MassTransit;

    using MediatR;

    using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ILogger<BasketCheckoutConsumer> logger;

        public BasketCheckoutConsumer(IMapper mapper, IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
        {
            this.mapper = mapper;
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            CheckoutOrderCommand command = this.mapper.Map<CheckoutOrderCommand>(context.Message);
            int result = await this.mediator.Send(command);

            this.logger.LogInformation("BasketCheckoutEvent consumed successfully. Created Order Id: {newOrderId}", result);
        }
    }
}
