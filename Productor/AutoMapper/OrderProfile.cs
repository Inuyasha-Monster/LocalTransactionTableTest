using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Message;
using Productor.Data;
using Productor.Model;
using OrderDetail = Productor.Data.OrderDetail;

namespace Productor.AutoMapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            this.CreateMap<OrderHeader, OrderOutput>();
            this.CreateMap<OrderDetail, OrderDetailOutput>();
            this.CreateMap<OrderInput, OrderHeader>();
            this.CreateMap<OrderDetailInput, OrderDetail>();
            this.CreateMap<OrderHeader, OrderCreatedEvent>();
            this.CreateMap<OrderDetail, Message.OrderDetail>();
        }
    }
}
