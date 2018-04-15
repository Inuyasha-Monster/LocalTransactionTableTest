using System;
using System.Collections.Generic;

namespace Message
{
    [Serializable]
    public class OrderCreatedEvent : AbstractEvent
    {
        public OrderCreatedEvent() : base()
        {
            CreateTime = DateTime.Now;
        }
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateTime { get; set; }
        public string AppUser { get; set; }

        public IList<OrderDetail> Details { get; set; }
    }

    public class OrderDetail
    {
        public OrderDetail()
        {
            CreateTime = DateTime.Now;
        }
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string Sku { get; set; }
        public string SkuName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
