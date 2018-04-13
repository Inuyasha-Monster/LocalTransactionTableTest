using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Productor.Model
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OrderInput
    {
        public OrderInput()
        {
            Details = new List<OrderDetailInput>();
        }
        /// <summary>
        /// 用户
        /// </summary>
        [Required]
        [StringLength(20)]
        public string AppUser { get; set; }
        /// <summary>
        /// 订单明细
        /// </summary>
        public IList<OrderDetailInput> Details { get; set; }

    }

    /// <summary>
    /// 订单明细
    /// </summary>
    public class OrderDetailInput
    {
        /// <summary>
        /// 商品SKU
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Sku { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string SkuName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }
    }
}
