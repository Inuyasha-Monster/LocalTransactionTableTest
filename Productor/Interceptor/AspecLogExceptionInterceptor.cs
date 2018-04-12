using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace Productor.Interceptor
{
    public class AspecLogExceptionInterceptor : IInterceptor
    {
        private readonly ILogger<AspecLogExceptionInterceptor> _logger;

        public AspecLogExceptionInterceptor(ILogger<AspecLogExceptionInterceptor> logger)
        {
            _logger = logger;
        }

        public async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{context.ProxyMethod.Name}调用异常");
                throw;
            }
        }

        public bool AllowMultiple { get; }
        public bool Inherited { get; set; }
        public int Order { get; set; }
    }
}
