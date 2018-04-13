using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Message;
using Newtonsoft.Json;
using Productor.Common;
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
            var header = _dbContext.OrderHeaders.SingleOrDefault(x => x.Id == id);
            if (header == null) throw new KnownException($"{id}参数错误找不到订单");
            var details = _dbContext.OrderDetails.Where(x => x.ParentId == header.Id).ToList();
            if (details == null || !details.Any()) throw new KnownException($"{id}参数错误找不到订单详情");
            var ouput = _mapper.Map<OrderOutput>(header);
            ouput.Details = _mapper.Map<List<OrderDetailOutput>>(details);
            return ouput;
        }

        public void CreateOrder(OrderInput input)
        {
            if (input.Details == null || !input.Details.Any()) throw new KnownException("订单错误找不到详情");
            var header = _mapper.Map<OrderHeader>(input);
            var details = _mapper.Map<List<Data.OrderDetail>>(input.Details);
            details.ForEach(x => x.ParentId = header.Id);
            header.Amount = details.Sum(x => x.Price * x.Quantity);
            var msg = _mapper.Map<OrderCreatedEvent>(header);
            msg.Details = _mapper.Map<List<Message.OrderDetail>>(details);
            var dbMessage = new MqMessage()
            {
                Body = JsonConvert.SerializeObject(msg),
                MessageAssemblyName = typeof(OrderCreatedEvent).Assembly.FullName,
                MessageClassFullName = msg.GetType().FullName
            };
            _dbContext.Add(dbMessage);
            _dbContext.Add(header);
            _dbContext.AddRange(details);
            _dbContext.SaveChanges();
        }
    }
}
