using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Productor.Data;
using Productor.Model;

namespace Productor.Service
{
    public class OrderService : IOrderService
    {
        private readonly ProductDbContext _dbContext;

        public OrderService(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public OrderOutput GetOrderInfo(string orderNo)
        {
            throw new NotImplementedException();
        }
    }
}
