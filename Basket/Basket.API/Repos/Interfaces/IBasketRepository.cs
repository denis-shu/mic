using Basket.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repos.Interfaces
{
    public interface IBasketRepository
    {
        Task<BasketCart> GetBasket(string userName);

        Task<BasketCart> UpdateBasket(BasketCart b);

        Task<bool> Delete(string userName);
    }
}
