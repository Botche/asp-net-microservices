﻿namespace AspnetRunBasics.Models
{
    public class BasketModel
    {
        public BasketModel()
        {
            this.Items = new List<BasketItemModel>();
        }

        public string UserName { get; set; }
        public ICollection<BasketItemModel> Items { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
