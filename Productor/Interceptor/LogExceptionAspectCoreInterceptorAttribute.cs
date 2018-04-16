using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Microsoft.Extensions.Logging;

namespace Productor.Interceptor
{
    public class LogExceptionAspectCoreInterceptorAttribute : AbstractInterceptor
    {
        [FromContainer]
        public ILogger<LogExceptionAspectCoreInterceptorAttribute> Logger { get; set; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"{context.ProxyMethod.Name}调用异常");
                throw;
            }
        }
    }
}
