using Food.Services.EmailAPI.Messaging;

namespace Food.Services.EmailAPI.Extension
{
    public static class ApplicationBuilderExtensions
    {
        private static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }

        public static IApplicationBuilder UseAzureServiceBusConsumer (this IApplicationBuilder app)
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStopping.Register(OnStop);
            return app;

        }

        private static void OnStop()
        {
            ServiceBusConsumer.Start();
        }

        private static void OnStart()
        {
            ServiceBusConsumer.Start();
        }
    }
}
