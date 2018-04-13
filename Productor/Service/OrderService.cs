using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Productor.Data;
using Productor.Model;

namespace Productor.Service
{
    public class OrderService : IOrderService
    {
        private readonly ProductDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderService(ProductDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public OrderOutput GetOrderInfo(Guid id)
        {
            var query = from header in _dbContext.OrderHeaders.Where(x => x.Id == id)
                        join detail in _dbContext.OrderDetails on header.Id equals detail.ParentId
                        select new
                        {

                        };
            return null;
        }
    }
}
