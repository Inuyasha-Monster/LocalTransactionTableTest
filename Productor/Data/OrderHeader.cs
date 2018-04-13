using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Productor.Data
{
    public class OrderHeader
    {
        public OrderHeader()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateTime { get; set; }
        public string AppUser { get; set; }
    }
}
