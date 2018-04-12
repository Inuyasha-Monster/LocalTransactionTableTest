using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace Productor.Interceptor
{
    public class CastleLogExceptionInterceptor : IInterceptor
    {
        private readonly ILogger<CastleLogExceptionInterceptor> _logger;

        public CastleLogExceptionInterceptor(ILogger<CastleLogExceptionInterceptor> logger)
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
