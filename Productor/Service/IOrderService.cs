using System;
using Productor.Interceptor;
using Productor.Model;

namespace Productor.Service
{
    public interface IOrderService
    {
        //[CacheAspectCoreInterceptor]
        [RedisCacheAspectCoreInterceptor]
        OrderOutput GetOrderInfo(Guid id);

        void CreateOrder(OrderInput input);
    }
}