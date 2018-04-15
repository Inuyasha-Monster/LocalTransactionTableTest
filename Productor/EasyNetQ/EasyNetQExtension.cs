using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Productor.EasyNetQ
{
    public static class EasyNetQExtension
    {
        private static void InternalInitEasyNetQ(IServiceCollection service, string rabbitMqConnection)
        {
            var bus = RabbitHutch.CreateBus(rabbitMqConnection);
            service.AddSingleton<IBus>(bus);
            service.AddSingleton<IAutoSubscriberMessageDispatcher, ProductorMessageDispatcher>(serviceProvider => new ProductorMessageDispatcher(serviceProvider, serviceProvider.GetRequiredService<ILogger<ProductorMessageDispatcher>>()));
        }

        public static void AddEasyNetQ(this IServiceCollection service, Func<string> getRabbitMqConneciton)
        {
            InternalInitEasyNetQ(service, getRabbitMqConneciton());
        }

        public static void AddEasyNetQ(this IServiceCollection service, string rabbitMqConnectionString)
        {
            InternalInitEasyNetQ(service, rabbitMqConnectionString);
        }

        public static void UseEasyNetQ(this IApplicationBuilder app)
        {
            var bus = app.ApplicationServices.GetRequiredService<IBus>();
            var autoSubscriber = new AutoSubscriber(bus, "productor")
            {
                AutoSubscriberMessageDispatcher = app.ApplicationServices.GetRequiredService<IAutoSubscriberMessageDispatcher>()
            };
            autoSubscriber.Subscribe(Assembly.GetExecutingAssembly());
            autoSubscriber.SubscribeAsync(Assembly.GetExecutingAssembly());
        }
    }
}
