using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

                // 默认拿出来第一个错误消息返回


                context.Result = new JsonResult(new ApiResult(false)
                {
                    Message = list.First()
                });
            }

            base.OnActionExecuting(context);
        }
    }
}
