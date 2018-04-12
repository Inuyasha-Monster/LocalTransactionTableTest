using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Productor.Filter
{
    public class ExceptionActionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<ExceptionActionFilter> _logger;

        public ExceptionActionFilter(IHostingEnvironment hostingEnvironment, ILogger<ExceptionActionFilter> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorReponse();
            if (context.Exception is UserOperationException)
            {
                json.Message = context.Exception.Message;
                json.DevelopMessage = context.Exception.StackTrace;
                context.Result = new BadRequestObjectResult(json);
            }
            else
            {
                json.Message = "未知内部异常";
                if (_hostingEnvironment.IsDevelopment())
                {
                    json.DevelopMessage = context.Exception.StackTrace;
                }
                context.Result = new InternalServerErrorObjectResult(json);
            }
            _logger.LogError(context.Exception, context.Exception.Message);
        }
    }

    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }

    public class JsonErrorReponse
    {
        public string Message { get; set; }
        public object DevelopMessage { get; set; }
    }

    public class UserOperationException : Exception
    {
        public UserOperationException()
        {

        }

        public UserOperationException(string message) : base(message)
        {

        }

        public UserOperationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
