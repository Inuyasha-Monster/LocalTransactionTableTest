using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Productor.Model
{
    public class OrderInput
    {
        public OrderInput()
        {
            Details = new List<OrderDetailInput>();
        }
        [Required]
        [StringLength(20)]
        public string AppUser { get; set; }
        public IList<OrderDetailInput> Details { get; set; }

    }

    public class OrderDetailInput
    {
        [Required]
        [StringLength(50)]
        public string Sku { get; set; }
        [Required]
        [StringLength(50)]
        public string SkuName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
