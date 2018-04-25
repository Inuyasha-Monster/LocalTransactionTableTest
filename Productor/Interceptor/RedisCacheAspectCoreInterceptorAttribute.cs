using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Productor.Interceptor
{
    public class RedisCacheAspectCoreInterceptorAttribute : AbstractInterceptorAttribute
    {
        [FromContainer]
        public IDistributedCache DistributedCache { get; set; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var cacheKey = GenerateCacheKey(context.ProxyMethod.Name, context.Parameters);
            var str = await DistributedCache.GetStringAsync(cacheKey);
            if (!string.IsNullOrWhiteSpace(str))
            {
                var value = JsonConvert.DeserializeObject(str, context.ProxyMethod.ReturnType);
                context.ReturnValue = value;
            }
            else
            {
                await next(context);
                var item = context.ReturnValue;
                await DistributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(item),
                     new DistributedCacheEntryOptions()
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
