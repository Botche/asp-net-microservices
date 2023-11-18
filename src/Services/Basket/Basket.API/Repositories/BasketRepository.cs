namespace Basket.API.Repositories
{
    using System.Text.Json;

    using Basket.API.Entities;

    using Microsoft.Extensions.Caching.Distributed;

    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            this.redisCache = redisCache;
        }

        public async Task DeleteBasketAsync(string userName)
        {
            await this.redisCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasketAsync(string userName)
        {
            var basket = await this.redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
            {
                return null;
            }

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            var basketAsJson = JsonSerializer.Serialize(basket);
            await this.redisCache.SetStringAsync(basket.UserName, basketAsJson);

            return await GetBasketAsync(basket.UserName);
        }
    }
}
