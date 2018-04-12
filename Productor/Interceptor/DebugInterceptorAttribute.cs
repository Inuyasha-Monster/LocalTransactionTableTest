using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace Productor.Interceptor
{
    public class DebugInterceptorAttribute : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                Debug.WriteLine("Before service call");
                await next(context);
            }
            catch (Exception)
            {
                Debug.WriteLine("Service threw an exception!");
                throw;
            }
            finally
            {
                Debug.WriteLine("After service call");
            }
        }
    }
}
