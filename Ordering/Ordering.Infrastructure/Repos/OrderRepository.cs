using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repos;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repos.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repos
{
    public class OrderRepository : Repo<Order>, IOrderRepo
    {
        public OrderRepository(OrderContext dbContext)
            : base(dbContext)
        { }
        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            return await _dbContext.Orders
                .Where(o => o.UserName == userName).ToListAsync();
        }
    }
}
