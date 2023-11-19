namespace Ordering.API.Controllers
{
    using System.Net;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;

    using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
    using Ordering.Application.Features.Orders.Commands.DeleteOrder;
    using Ordering.Application.Features.Orders.Commands.UpdateOrder;

    using Ordering.Application.Features.Orders.Queries.GetOrdersList;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{userName}", Name = "GetOrders")]
        [ProducesResponseType(typeof(IEnumerable<OrderVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderVm>>> GetOrdersByUserName(string userName)
        {
            GetOrdersListQuery query = new(userName);
            List<OrderVm> orders = await this.mediator.Send(query);

            return this.Ok(orders);
        }

        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        {
            int result = await this.mediator.Send(command);

            return this.Ok(result);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            await this.mediator.Send(command);

            return this.NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            DeleteOrderCommand command = new()
            {
                Id = id
            };
            await this.mediator.Send(command);

            return this.NoContent();
        }
    }
}
