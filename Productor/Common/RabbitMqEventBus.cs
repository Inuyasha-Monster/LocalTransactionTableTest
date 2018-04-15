using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.NonGeneric;

namespace Productor.Common
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly IBus _bus;

        public RabbitMqEventBus(IBus bus)
        {
            _bus = bus;
        }
        public void Publish<T>(T message) where T : class, new()
        {
            _bus.Publish(message);
        }

        public async Task PublishAsync<T>(T message) where T : class, new()
        {
            await _bus.PublishAsync(message);
        }

        public void Publish(Type messageType, object message)
        {
            _bus.Publish(messageType, message);
        }

        public void PublishAsync(Type messageType, object message)
        {
            _bus.PublishAsync(messageType, message);
        }
    }
}
