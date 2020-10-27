using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILoggerFactory logger, int? retry = 0)
        {
            int retryForAvailablility = retry.Value;

            try
            {
                orderContext.Database.Migrate();
                if (!orderContext.Orders.Any())
                {
                    orderContext.Orders.AddRange(GetPreconfigureOrders());
                    await orderContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailablility < 3)
                {
                    retryForAvailablility++;
                    var log = logger.CreateLogger<OrderContextSeed>();
                    log.LogWarning(ex.Message);
                    await SeedAsync(orderContext, logger, retryForAvailablility);
                }
            }
        }

        private static IEnumerable<Order> GetPreconfigureOrders()
        {
            return new List<Order>
            {
                new Order() {UserName="den", FirstName="dn", LastName="sh",
                    EmailAddress="sh@c.com", AddressLine="adrressLine", Country="ua" }
            };
        }
    }
}
