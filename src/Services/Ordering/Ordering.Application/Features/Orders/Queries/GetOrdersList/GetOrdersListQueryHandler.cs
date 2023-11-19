namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using MediatR;

    using Ordering.Application.Contracts.Persistence;
    using Ordering.Domain.Entities;

    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderVm>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<List<OrderVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Order> orders = await this.orderRepository
                .GetOrdersByUserName(request.UserName);

            return this.mapper.Map<List<OrderVm>>(orders);
        }
    }
}
