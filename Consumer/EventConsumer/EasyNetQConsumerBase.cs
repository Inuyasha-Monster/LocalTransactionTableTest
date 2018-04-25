using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.Logging;

namespace Consumer.EventConsumer
{
    public abstract class EasyNetQConsumerBase<T> : IConsume<T> where T : class
    {
        public void Consume(T message)
        {
            try
            {
                ConsumeSync(message);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, $"{message.GetType().Name} 消息消费者消费错误");
                throw;
            }
        }

        protected abstract ILogger Logger { get; }
        protected abstract void ConsumeSync(T message);
    }
}
