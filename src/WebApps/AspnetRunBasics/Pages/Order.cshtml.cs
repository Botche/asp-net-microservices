namespace AspnetRunBasics
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AspnetRunBasics.Models;
    using AspnetRunBasics.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class OrderModel : PageModel
    {
        private readonly IOrderService orderService;

        public OrderModel(IOrderService orderService)
        {
            this.orderService = orderService;

            this.Orders = new List<OrderResponseModel>();
        }

        public IEnumerable<OrderResponseModel> Orders { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string userName = "swn";
            this.Orders = await this.orderService.GetOrdersByUserNameAsync(userName);

            return this.Page();
        }       
    }
}