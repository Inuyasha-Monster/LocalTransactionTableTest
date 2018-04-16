using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace Productor.Interceptor
{
    public class TestDebugIAspectCorenterceptorAttribute : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            Debug.WriteLine("Before service call");
            await next(context);
            Debug.WriteLine("After service call");
        }
    }
}
