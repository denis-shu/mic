using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Data;
using System;

namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            CreateAndSeedDB(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void CreateAndSeedDB(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFac = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var orderContext = services.GetRequiredService<OrderContext>();
                    OrderContextSeed.SeedAsync(orderContext, loggerFac);
                }
                catch (Exception e)
                {
                    var log = loggerFac.CreateLogger<Program>();
                    log.LogError(e.Message);
                    throw;
                }
            }
        }
    }
}
