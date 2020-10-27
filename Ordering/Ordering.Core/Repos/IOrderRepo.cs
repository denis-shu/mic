using Ordering.Core.Entities;
using Ordering.Core.Repos.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.Repos
{
    public interface IOrderRepo: IRepo<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
    }
}
