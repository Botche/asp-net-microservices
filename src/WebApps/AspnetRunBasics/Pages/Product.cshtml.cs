namespace AspnetRunBasics
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AspnetRunBasics.Models;
    using AspnetRunBasics.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class ProductModel : PageModel
    {
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;

        public ProductModel(ICatalogService catalogService, IBasketService basketService)
        {
            this.catalogService = catalogService;
            this.basketService = basketService;

            this.CategoryList = new List<string>();
            this.ProductList = new List<CatalogModel>();
        }

        public IEnumerable<string> CategoryList { get; set; }
        public IEnumerable<CatalogModel> ProductList { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string categoryName)
        {
            IEnumerable<CatalogModel> productsList = await this.catalogService.GetCatalogAsync();
            this.ProductList = productsList;
            this.CategoryList = productsList
                .Select(p => p.Category)
                .ToList();

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                this.ProductList = productsList
                    .Where(p => p.Category == categoryName);
                this.SelectedCategory = categoryName;
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            CatalogModel product = await this.catalogService.GetCatalogAsync(productId);

            string userName = "swn";
            BasketModel basket = await this.basketService.GetBasketAsync(userName);

            basket.Items.Add(new BasketItemModel
            {
                ProductId = productId,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1,
                Color = "Black",
            });

            await this.basketService.UpdateBasketAsync(basket);

            return this.RedirectToPage("Cart");
        }
    }
}