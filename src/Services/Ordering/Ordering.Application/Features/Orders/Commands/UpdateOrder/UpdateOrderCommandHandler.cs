namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Ordering.Application.Contracts.Persistence;
    using Ordering.Application.Exceptions;
    using Ordering.Domain.Entities;

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateOrderCommandHandler> logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            Order orderToUpdate = await this.orderRepository.GetByIdAsync(request.Id);
            if (orderToUpdate == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }

            this.mapper.Map(request, orderToUpdate);

            await this.orderRepository.UpdateAsync(orderToUpdate);

            this.logger.LogInformation("Order {orderId} is successfully updated.", orderToUpdate.Id);
        }
    }
}
