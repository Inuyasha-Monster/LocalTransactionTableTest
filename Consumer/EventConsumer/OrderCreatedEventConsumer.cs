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
    public class OrderCreatedEventConsumer : EasyNetQConsumerBase<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedEventConsumer> _logger;

        public OrderCreatedEventConsumer(ILogger<OrderCreatedEventConsumer> logger)
        {
            _logger = logger;
        }

        protected override ILogger Logger => _logger;

        protected override void ConsumeSync(OrderCreatedEvent message)
        {
            _logger.LogDebug(JsonConvert.SerializeObject(message));
        }
    }
}
