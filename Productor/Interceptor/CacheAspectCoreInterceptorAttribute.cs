using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace Productor.Interceptor
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CacheAspectCoreInterceptorAttribute : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
