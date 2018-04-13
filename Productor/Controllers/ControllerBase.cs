using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public override JsonResult Json(object data, JsonSerializerSettings serializerSettings)
        {
            var apiResult = new ApiResult<object>(true)
            {
                Data = data
            };
            return base.Json(apiResult, serializerSettings);
        }

        public JsonResult Json()
        {
            return base.Json(new ApiResult(true));
        }
    }
}
