using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Productor.Model
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OrderOutput
    {
        public OrderOutput()
        {
            Details = new List<OrderDetailOutput>();
        }

        /// <summary>
        /// 订单主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 总价值
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 下单用户
        /// </summary>
        public string AppUser { get; set; }
        /// <summary>
        /// 订单详情
        /// </summary>
        public IList<OrderDetailOutput> Details { get; set; }
    }

    /// <summary>
    /// 订单详情
    /// </summary>
    public class OrderDetailOutput
    {
        /// <summary>
        /// 详情主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 订单主键
        /// </summary>
        public Guid ParentId { get; set; }
        /// <summary>
        /// 商品SKU
        /// </summary>
        public string Sku { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string SkuName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
