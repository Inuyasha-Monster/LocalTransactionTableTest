﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Productor.EasyNetQ
{
    public class ProductorMessageDispatcher : IAutoSubscriberMessageDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ProductorMessageDispatcher> _logger;

        public ProductorMessageDispatcher(IServiceProvider serviceProvider, ILogger<ProductorMessageDispatcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void Dispatch<TMessage, TConsumer>(TMessage message) where TMessage : class where TConsumer : IConsume<TMessage>
        {
            try
            {
                TConsumer consumer = _serviceProvider.GetRequiredService<TConsumer>();
                consumer.Consume(message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"创建消费者或者消费异常");
                throw;
            }
        }

        public async Task DispatchAsync<TMessage, TConsumer>(TMessage message) where TMessage : class where TConsumer : IConsumeAsync<TMessage>
        {
            try
            {
                TConsumer consumer = _serviceProvider.GetRequiredService<TConsumer>();
                await consumer.Consume(message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"创建消费者或者消费异常");
                throw;
            }
        }
    }
}
