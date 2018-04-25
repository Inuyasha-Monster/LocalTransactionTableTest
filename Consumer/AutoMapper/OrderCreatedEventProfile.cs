using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Consumer.MongoDbEntity;
using Message;

namespace Consumer.AutoMapper
{
    public class OrderCreatedEventProfile : Profile
    {
        public OrderCreatedEventProfile()
        {
            this.CreateMap<OrderCreatedEvent, GuidEventLog>().ForMember(d => d.DatabaseId, s => s.MapFrom(soucre => soucre.Id));
        }
    }
}
