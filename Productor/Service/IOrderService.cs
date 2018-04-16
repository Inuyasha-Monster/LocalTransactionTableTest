using System;
using Productor.Interceptor;
using Productor.Model;

namespace Productor.Service
{
    public interface IOrderService
    {
        [CacheAspectCoreInterceptor]
        OrderOutput GetOrderInfo(Guid id);

        void CreateOrder(OrderInput input);
    }
}