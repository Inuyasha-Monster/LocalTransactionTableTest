using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Consumer.Mongo;
using Consumer.MongoDbEntity;
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
        private readonly MongoDbContext _mongoDbContext;
        private readonly IMapper _mapper;

        public OrderCreatedEventConsumer(ILogger<OrderCreatedEventConsumer> logger, MongoDbContext mongoDbContext, IMapper mapper)
        {
            _logger = logger;
            _mongoDbContext = mongoDbContext;
            _mapper = mapper;
        }

        protected override ILogger Logger => _logger;

        protected override void ConsumeSync(OrderCreatedEvent message)
        {
            // 拿到rabbitmq消息消费需要持久化,消费失败需要自动重试消费
            _logger.LogDebug(JsonConvert.SerializeObject(message));
            // 尝试写入Mongo持久化消息存储,方便手动重试下消费或者自动消费逻辑
            var mongoItem = _mapper.Map<GuidEventLog>(message);
            mongoItem.Body = JsonConvert.SerializeObject(message);
            mongoItem.MessageClassFullName = message.GetType().FullName;
            mongoItem.MessageAssemblyName = typeof(OrderCreatedEvent).Assembly.GetName().Name;
            _mongoDbContext.GuidEventLogs.InsertOne(mongoItem);
        }
    }
}
