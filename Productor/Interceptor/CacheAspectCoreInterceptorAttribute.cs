using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Microsoft.Extensions.Caching.Memory;

namespace Productor.Interceptor
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CacheAspectCoreInterceptorAttribute : AbstractInterceptorAttribute
    {
        [FromContainer]
        public IMemoryCache MemoryCache { get; set; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var cacheKey = GenerateCacheKey(context.ProxyMethod.Name, context.Parameters);
            if (MemoryCache.TryGetValue(cacheKey, out object value))
            {
                context.ReturnValue = MemoryCache.Get(cacheKey);
            }
            else
            {
                await next(context);
                var item = context.ReturnValue;
                MemoryCache.Set(cacheKey, item, new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromSeconds(10)
                });
            }
        }

        private static string GenerateCacheKey(string name, object[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
                return name;
            return name + "_" + string.Join("_", arguments.Select(a => a.ToString()).ToArray());
        }
    }
}
