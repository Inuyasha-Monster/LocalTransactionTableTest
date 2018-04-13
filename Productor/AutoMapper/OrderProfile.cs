using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Productor.Data;
using Productor.Model;

namespace Productor.AutoMapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            this.CreateMap<OrderHeader, OrderOutput>();
            this.CreateMap<OrderDetail, OrderDetailOutput>();
        }
    }
}
