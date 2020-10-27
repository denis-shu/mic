using Basket.API.Data.Interfaces;
using Basket.API.Entities;
using Basket.API.Repos.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Basket.API.Repos
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _context;

        public BasketRepository(IBasketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Delete(string userName)
        {
            return await _context.Redis.KeyDeleteAsync(userName);
        }

        public async Task<BasketCart> GetBasket(string userName)
        {
            var basket = await _context.Redis.StringGetAsync(userName);

            if (basket.IsNullOrEmpty)
                return null;

            return JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateBasket(BasketCart b)
        {
            var isUpdated = await _context
                .Redis
                .StringSetAsync(b.UserName, JsonConvert.SerializeObject(b));
            if (!isUpdated)
                return null;

            return await GetBasket(b.UserName);

        }
    }
}
