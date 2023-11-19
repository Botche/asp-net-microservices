namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Ordering.Application.Contracts.Persistence;
    using Ordering.Domain.Entities;

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly ILogger<DeleteOrderCommand> logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<DeleteOrderCommand> logger)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            Order orderToDelete = await this.orderRepository.GetByIdAsync(request.Id);
            if (orderToDelete == null)
            {
                this.logger.LogError("Order not exist on database.");
                // throw new NotFoundException(nameof(Order), request.Id);
            }

            await this.orderRepository.DeleteAsync(orderToDelete);
        }
    }
}
