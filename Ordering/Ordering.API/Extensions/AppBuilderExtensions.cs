using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Ordering.API.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.API.Extensions
{
    public static class AppBuilderExtensions
    {
        public static EventBusRabbitMQConsumer Listener { get; set; }
        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<EventBusRabbitMQConsumer>();
            var lt = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            lt.ApplicationStarted.Register(OnStarted);
            lt.ApplicationStopped.Register(OnStoppeing);

            return app;
        }

        private static void OnStarted()
        {
            Listener.Consume();
        }

        private static void OnStoppeing()
        {
            Listener.Disconnect();
        }
    }
}
