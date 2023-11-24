namespace AspnetRunBasics
{
    using System.Threading.Tasks;

    using AspnetRunBasics.Models;
    using AspnetRunBasics.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class ProductDetailModel : PageModel
    {
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;

        public ProductDetailModel(ICatalogService catalogService, IBasketService basketService)
        {
            this.catalogService = catalogService;
            this.basketService = basketService;
        }

        public CatalogModel Product { get; set; }

        [BindProperty]
        public string Color { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnGetAsync(string productId)
        {
            if (productId == null)
            {
                return this.NotFound();
            }

            this.Product = await this.catalogService.GetCatalogAsync(productId);
            if (this.Product == null)
            {
                return this.NotFound();
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            CatalogModel product = await this.catalogService.GetCatalogAsync(productId);

            string userName = "swn";
            BasketModel basket = await this.basketService.GetBasketAsync(userName);

            BasketItemModel productFromBasket = basket.Items
                .FirstOrDefault(i => i.ProductId == productId);
            if (productFromBasket is null)
            {
                basket.Items.Add(new BasketItemModel
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    Color = this.Color,
                });
            }
            else
            {
                productFromBasket.Quantity += 1;
            }

            await this.basketService.UpdateBasketAsync(basket);

            return this.RedirectToPage("Cart");
        }
    }
}