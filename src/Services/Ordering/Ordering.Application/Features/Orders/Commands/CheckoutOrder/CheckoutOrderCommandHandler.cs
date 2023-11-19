namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    using System.Threading.Tasks;

    using AutoMapper;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Ordering.Application.Contracts.Infrastructure;
    using Ordering.Application.Contracts.Persistence;
    using Ordering.Application.Models;
    using Ordering.Domain.Entities;

    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly ILogger<CheckoutOrderCommand> logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommand> logger)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.emailService = emailService;
            this.logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            Order orderToCreate = this.mapper.Map<Order>(request);
            Order newOrder = await this.orderRepository.AddAsync(orderToCreate);

            this.logger.LogInformation("Order {orderId} is successfully created.", newOrder.Id);

            await SendMail(newOrder.Id);

            return newOrder.Id;
        }

        private async Task SendMail(int orderId)
        {
            var email = new Email()
            {
                To = "gabriel.v.petkov@gmail.com",
                Body = $"Order was created.",
                Subject = "Order was created"
            };

            try
            {
                await this.emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Order {orderId} failed due to an error with the mail service: {ex.Message}", orderId, ex.Message);
            }
        }
    }
}
