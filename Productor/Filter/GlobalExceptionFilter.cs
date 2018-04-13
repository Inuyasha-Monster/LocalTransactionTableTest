using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Productor.Common;

namespace Productor.Filter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(IHostingEnvironment hostingEnvironment, ILogger<GlobalExceptionFilter> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {

            var errorJsonResult = new ApiResult(false);
            if (_hostingEnvironment.IsDevelopment())
            {
                errorJsonResult.Message = context.Exception.Message;
                errorJsonResult.DevelopMessage = context.Exception.StackTrace;
                _logger.LogDebug(context.Exception, context.Exception.Message);
            }
            else
            {
                if (context.Exception is KnownException)
                {
                    errorJsonResult.Message = context.Exception.Message;
                }
                else
                {
                    errorJsonResult.Message = "内部出现未知异常";
                    _logger.LogError(context.Exception, context.Exception.Message);
                }
            }
            context.Result = new JsonResult(errorJsonResult);
        }
    }
}
