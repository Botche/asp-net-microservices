namespace AspnetRunBasics
{
    using System.Threading.Tasks;

    using AspnetRunBasics.Models;
    using AspnetRunBasics.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class CheckOutModel : PageModel
    {
        private readonly IBasketService basketService;

        public CheckOutModel(IBasketService basketService)
        {
            this.basketService = basketService;

            this.Cart = new BasketModel();
        }

        [BindProperty]
        public BasketCheckoutModel Order { get; set; }

        public BasketModel Cart { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string userName = "swn";
            this.Cart = await this.basketService.GetBasketAsync(userName);

            return this.Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            var userName = "swn";
            this.Cart = await this.basketService.GetBasketAsync(userName);

            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            this.Order.UserName = userName;
            this.Order.TotalPrice = Cart.TotalPrice;

            await this.basketService.CheckoutBasketAsync(Order);

            return this.RedirectToPage("Confirmation", "OrderSubmitted");
        }
    }
}
