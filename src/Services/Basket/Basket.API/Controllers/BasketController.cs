namespace Basket.API.Controllers
{
    using System.Net;

    using AutoMapper;

    using Basket.API.Entities;
    using Basket.API.GrpcServices;
    using Basket.API.Repositories;

    using Discount.Grpc.Protos;

    using EventBus.Messages.Events;

    using MassTransit;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly DiscountGrpcService discountGrpcService;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IMapper mapper;

        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.discountGrpcService = discountGrpcService;
            this.publishEndpoint = publishEndpoint;
            this.mapper = mapper;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            ShoppingCart basket = await this.basketRepository.GetBasketAsync(userName);

            return this.Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items.Where(i => i.AppliedDiscount == false))
            {
                CouponModel coupon = await this.discountGrpcService.GetDiscountAsync(item.ProductName);
                item.Price -= coupon.Amount;
                item.AppliedDiscount = true;
            }

            return this.Ok(await this.basketRepository.UpdateBasketAsync(basket));
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout model)
        {
            ShoppingCart basket = await this.basketRepository.GetBasketAsync(model.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            BasketCheckoutEvent checkoutEventMessage = this.mapper.Map<BasketCheckoutEvent>(model);
            checkoutEventMessage.TotalPrice = basket.TotalPrice;
            await this.publishEndpoint.Publish(checkoutEventMessage);

            await this.basketRepository.DeleteBasketAsync(model.UserName);

            return this.Accepted();
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> DeleteBasket(string userName)
        {
            await this.basketRepository.DeleteBasketAsync(userName);

            return this.Ok();
        }
    }
}
