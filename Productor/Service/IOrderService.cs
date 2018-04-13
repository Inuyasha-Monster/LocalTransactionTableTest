using Productor.Model;

namespace Productor.Service
{
    public interface IOrderService
    {
        OrderOutput GetOrderInfo(string orderNo);
    }
}