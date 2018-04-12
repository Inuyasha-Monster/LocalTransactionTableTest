using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Productor.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Productor.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        [HttpGet]
        public IActionResult Get(string orderNo)
        {
            OrderOutput output = new OrderOutput();
            return Json(output);
        }

        [HttpPost]
        public IActionResult Order(OrderInput input)
        {
            return Ok();
        }
    }
}
