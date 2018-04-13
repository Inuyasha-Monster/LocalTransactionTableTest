using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Productor.Common;

namespace Productor.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> list = (from modelState in context.ModelState.Values
                                     from error in modelState.Errors
                                     select error.ErrorMessage).ToList();

                //Also add exceptions.
                //list.AddRange(from modelState in context.ModelState.Values from error in modelState.Errors select error.Exception.ToString());

                // 记录客户端错误消息
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<ValidateModelStateAttribute>>();
                logger.LogDebug(list.First());

                context.Result = new JsonResult(new ApiResult(false)
                {
                    Message = list.First()
                });
            }

            base.OnActionExecuting(context);
        }
    }
}
