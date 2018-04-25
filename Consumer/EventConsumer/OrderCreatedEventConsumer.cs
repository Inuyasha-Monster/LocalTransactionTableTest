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
            // 拿到rabbitmq消息消费需要持久化,消费失败需要自动重试消费
            _logger.LogDebug(JsonConvert.SerializeObject(message));
        }
    }
}
