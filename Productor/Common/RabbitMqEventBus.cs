using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.NonGeneric;
using Polly;
using Polly.Retry;

namespace Productor.Common
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly IBus _bus;

        private readonly RetryPolicy _retryPolicy;

        private readonly RetryPolicy _retryPolicyAsync;

        public RabbitMqEventBus(IBus bus)
        {
            _bus = bus;
            _retryPolicy = Policy.Handle<Exception>().WaitAndRetry(new[]
            {
                TimeSpan.FromSeconds(0.5),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2)
            });
            _retryPolicyAsync = Policy.Handle<Exception>().WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(0.5),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2)
            });
        }
        public void Publish<T>(T message) where T : class, new()
        {
            _retryPolicy.Execute(() => _bus.Publish(message));
        }

        public async Task PublishAsync<T>(T message) where T : class, new()
        {
            await _retryPolicyAsync.ExecuteAsync(() => _bus.PublishAsync(message));
        }

        public void Publish(Type messageType, object message)
        {
            _retryPolicy.Execute(() => _bus.Publish(messageType, message));
        }

        public void PublishAsync(Type messageType, object message)
        {
            _retryPolicy.ExecuteAsync(() => _bus.PublishAsync(messageType, message));
        }
    }
}
