using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Productor.Data;
using Productor.Filter;
using Productor.Interceptor;

namespace Productor
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //    .AddEnvironmentVariables();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();

            var mvcBuilder = services.AddMvc(x =>
            {
                //x.Filters.Add<ExceptionFilterAttribute>();
                x.Filters.Add<GlobalExceptionFilter>();
            }).AddControllersAsServices();

            mvcBuilder.AddJsonOptions(x =>
            {
                x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                x.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                x.SerializerSettings.DateFormatString = "yyyy-MM-dd hh:mm:ss";
            });

            services.AddDbContext<ProductDbContext>(builderDb =>
            {
                var connectionStr = Configuration.GetConnectionString("Mysql");
                builderDb.UseMySql(connectionStr);
            });


            //添加你的服务注册到services...



            //将IServiceCollection的服务添加到ServiceContainer容器中
            var container = services.ToServiceContainer();

            container.Configure(config =>
            {
                config.Interceptors.AddTyped<AspecLogExceptionInterceptor>();
            });

            return container.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddLog4Net("log4net.config");

            app.UseMvc();
        }
    }
}
