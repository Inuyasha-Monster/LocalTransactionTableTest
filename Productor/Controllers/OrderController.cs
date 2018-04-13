using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Productor.Model;
using Productor.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Productor.Controllers
{
    /// <summary>
    /// 订单服务
    /// </summary>
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        //private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            //_mapper = mapper;
            _orderService = orderService;
        }

        /// <summary>
        /// 获取订单头和明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AcceptVerbs("GET")]
        public IActionResult Get([FromQuery]Guid id)
        {
            return Json(_orderService.GetOrderInfo(id));
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AcceptVerbs("POST")]
        public IActionResult Create([FromBody]OrderInput input)
        {
            _orderService.CreateOrder(input);
            return Json();
        }
    }
}
