using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace Productor.Interceptor
{
    public class LogExceptionCastleInterceptor : Castle.DynamicProxy.IInterceptor
    {
        private readonly ILogger<LogExceptionCastleInterceptor> _logger;

        public LogExceptionCastleInterceptor(ILogger<LogExceptionCastleInterceptor> logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{invocation.Method.Name}调用异常");
                throw;
            }
        }
    }
}
