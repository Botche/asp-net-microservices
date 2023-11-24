namespace AspnetRunBasics.Pages
{
    using AspnetRunBasics.Models;
    using AspnetRunBasics.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class IndexModel : PageModel
    {
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;

        public IndexModel(ICatalogService catalogService, IBasketService basketService)
        {
            this.catalogService = catalogService;
            this.basketService = basketService;

            this.ProductList = new List<CatalogModel>();
        }

        public IEnumerable<CatalogModel> ProductList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            this.ProductList = await this.catalogService.GetCatalogAsync();

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
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    Color = "Black",
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
