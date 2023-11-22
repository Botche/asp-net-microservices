namespace Shopping.Aggregator.Controllers
{
    using System.Net;

    using Microsoft.AspNetCore.Mvc;

    using Shopping.Aggregator.Models;
    using Shopping.Aggregator.Services.Interfaces;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;
        private readonly IOrderService orderService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            this.catalogService = catalogService;
            this.basketService = basketService;
            this.orderService = orderService;
        }

        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            BasketModel basket = await this.basketService.GetBasketAsync(userName);

            foreach (var item in basket.Items)
            {
                CatalogModel product = await this.catalogService.GetCatalogAsync(item.ProductId);

                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }

            IEnumerable<OrderResponseModel> order = await this.orderService.GetOrdersByUserNameAsync(userName);

            var shoppingModel = new ShoppingModel
            {
                BasketWithProducts = basket,
                Orders = order,
                UserName = userName,
            };

            return this.Ok(shoppingModel);
        }
    }
}
