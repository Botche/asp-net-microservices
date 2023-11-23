namespace AspnetRunBasics
{
    using System.Threading.Tasks;

    using AspnetRunBasics.Models;
    using AspnetRunBasics.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class CartModel : PageModel
    {
        private readonly IBasketService basketService;

        public CartModel(IBasketService basketService)
        {
            this.basketService = basketService;

            this.Cart = new BasketModel();
        }

        public BasketModel Cart { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string userName = "swn";
            this.Cart = await this.basketService.GetBasketAsync(userName);

            return this.Page();
        }

        public async Task<IActionResult> OnPostRemoveFromCartAsync(string productId)
        {
            string userName = "swn";
            BasketModel basket = await this.basketService.GetBasketAsync(userName);

            BasketItemModel item = basket.Items
                .SingleOrDefault(x => x.ProductId == productId);
            basket.Items.Remove(item);

            await this.basketService.UpdateBasketAsync(basket);

            return this.RedirectToPage();
        }
    }
}