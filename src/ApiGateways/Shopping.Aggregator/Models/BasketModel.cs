namespace Shopping.Aggregator.Models
{
    public class BasketModel
    {
        public BasketModel()
        {
            this.Items = new List<BasketItemExtendedModel>();
        }

        public string UserName { get; set; }

        public IEnumerable<BasketItemExtendedModel> Items { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
