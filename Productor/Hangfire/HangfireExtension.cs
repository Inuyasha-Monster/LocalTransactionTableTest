using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Productor.Hangfire
{
    public static class HangfireExtension
    {
        public static void AddHangfire(this IServiceCollection service, string connection)
        {

        }

        public static void UseHangfire(this IApplicationBuilder app)
        {

        }
    }
}
