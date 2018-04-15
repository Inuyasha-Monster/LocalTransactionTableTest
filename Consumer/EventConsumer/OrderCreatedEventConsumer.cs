using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.Consumer;
using Message;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Consumer.EventConsumer
{
    public class OrderCreatedEventConsumer : IConsume<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedEventConsumer> _logger;

        public OrderCreatedEventConsumer(ILogger<OrderCreatedEventConsumer> logger)
        {
            _logger = logger;
        }

        public void Consume(OrderCreatedEvent message)
        {
            _logger.LogDebug(JsonConvert.SerializeObject(message));
        }
    }
}
