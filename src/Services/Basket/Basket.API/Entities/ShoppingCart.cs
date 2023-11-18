namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            this.Items = new List<ShoppingCartItem>();
        }

        public ShoppingCart(string userName)
            : this()
        {
            this.UserName = userName;
        }

        public string UserName { get; set; }

        public IEnumerable<ShoppingCartItem> Items { get; set; }

        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                foreach (ShoppingCartItem item in Items)
                {
                    totalPrice += item.Price * item.Quantity;
                }

                return totalPrice;
            }
        }
    }
}
