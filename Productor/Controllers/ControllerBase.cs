using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Productor.Common;

namespace Productor.Controllers
{
    public class ControllerBase : Controller
    {
        public override JsonResult Json(object data)
        {
            var apiResult = new ApiResult<object>(true)
            {
                Data = data
            };
            return base.Json(apiResult);
        }
    }
}
